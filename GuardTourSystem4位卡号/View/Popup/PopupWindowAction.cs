using GuardTourSystem.Utils;
using GuardTourSystem.View.Popup;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Windows;
using System.Windows.Interactivity;

namespace GuardTourSystem.View
{
    public class PopupWindowAction : TriggerAction<FrameworkElement>
    {
        /// <summary>
        /// The content of the child window to display as part of the popup.
        /// </summary>
        public static readonly DependencyProperty WindowContentProperty =
            DependencyProperty.Register(
                "WindowContent",
                typeof(FrameworkElement),
                typeof(PopupWindowAction),
                new PropertyMetadata(null));

        /// <summary>
        /// Determines if the content should be shown in a modal window or not.
        /// </summary>
        public static readonly DependencyProperty IsModalProperty =
            DependencyProperty.Register(
                "IsModal",
                typeof(bool),
                typeof(PopupWindowAction),
                new PropertyMetadata(null));

        /// <summary>
        /// Determines if the content should be initially shown centered over the view that raised the interaction request or not.
        /// </summary>
        public static readonly DependencyProperty CenterOverAssociatedObjectProperty =
            DependencyProperty.Register(
                "CenterOverAssociatedObject",
                typeof(bool),
                typeof(PopupWindowAction),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the content of the window.
        /// </summary>
        public FrameworkElement WindowContent
        {
            get { return (FrameworkElement)GetValue(WindowContentProperty); }
            set { SetValue(WindowContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets if the window will be modal or not.
        /// </summary>
        public bool IsModal
        {
            get { return (bool)GetValue(IsModalProperty); }
            set { SetValue(IsModalProperty, value); }
        }

        /// <summary>
        /// Gets or sets if the window will be initially shown centered over the view that raised the interaction request or not.
        /// </summary>
        public bool CenterOverAssociatedObject
        {
            get { return (bool)GetValue(CenterOverAssociatedObjectProperty); }
            set { SetValue(CenterOverAssociatedObjectProperty, value); }
        }

        /// <summary>
        /// Displays the child window and collects results for <see cref="IInteractionRequest"/>.
        /// </summary>
        /// <param name="parameter">The parameter to the action. If the action does not require a parameter, the parameter may be set to a null reference.</param>
        protected override void Invoke(object parameter)
        {
            var args = parameter as InteractionRequestedEventArgs;
            if (args == null)
            {
                return;
            }

            // If the WindowContent shouldn't be part of another visual tree.
            if (this.WindowContent != null && this.WindowContent.Parent != null)
            {
                return;
            }

            Window wrapperWindow = this.GetWindow(args.Context);
            wrapperWindow.SizeToContent = SizeToContent.WidthAndHeight;

            // We invoke the callback when the interaction's window is closed.
            var callback = args.Callback;
            EventHandler handler = null;
            handler =
                (o, e) =>
                {
                    wrapperWindow.Closed -= handler;
                    wrapperWindow.Content = null;
                    callback?.Invoke();
                };
            wrapperWindow.Closed += handler;

            if (this.CenterOverAssociatedObject && this.AssociatedObject != null)
            {
                // If we should center the popup over the parent window we subscribe to the SizeChanged event
                // so we can change its position after the dimensions are set.
                SizeChangedEventHandler sizeHandler = null;
                sizeHandler =
                    (o, e) =>
                    {
                        wrapperWindow.SizeChanged -= sizeHandler;

                        FrameworkElement view = this.AssociatedObject;
                        Point position = view.PointToScreen(new Point(0, 0));

                        wrapperWindow.Top = position.Y + ((view.ActualHeight - wrapperWindow.ActualHeight) / 2);
                        wrapperWindow.Left = position.X + ((view.ActualWidth - wrapperWindow.ActualWidth) / 2);
                    };
                wrapperWindow.SizeChanged += sizeHandler;
            }

            if (this.IsModal)
            {
                wrapperWindow.ShowDialog();
            }
            else
            {
                wrapperWindow.Show();
            }
        }

        /// <summary>
        /// Returns the window to display as part of the trigger action.
        /// </summary>
        /// <param name="notification">The notification to be set as a DataContext in the window.</param>
        /// <returns></returns>
        protected virtual Window GetWindow(Notification notification)
        {
            //默认在父窗口中心
            this.CenterOverAssociatedObject = true;
            this.IsModal = true;

            Window wrapperWindow;

            if (this.WindowContent != null)
            {
                wrapperWindow = new Window()
                {
                    WindowStyle = WindowStyle.None,
                    Owner = MainWindow.Instance,
                    ShowInTaskbar = false,

                    DataContext = notification,
                    Title = notification.Title
                };
                ResourceDictionary langRd = null;
                try
                {
                    langRd = Application.LoadComponent(new Uri("/Resource/Styles/WindowStyle.xaml", UriKind.RelativeOrAbsolute)) as ResourceDictionary;
                    if (langRd != null)
                    {
                        if (wrapperWindow.Resources.MergedDictionaries.Count > 0)
                        {
                            wrapperWindow.Resources.MergedDictionaries.Clear();
                        }
                        wrapperWindow.Resources.MergedDictionaries.Add(langRd);

                    }
                    this.PrepareContentForWindow(notification, wrapperWindow);
                }
                catch (Exception e)
                {
                    DBug.w(e);
                }
            }
            else
            {
                throw new Exception("窗口内容为空");
            }

            return wrapperWindow;
        }

        /// <summary>
        /// Checks if the WindowContent or its DataContext implements <see cref="IInteractionRequestAware"/>.
        /// If so, it sets the corresponding value.
        /// Also, if WindowContent does not have a RegionManager attached, it creates a new scoped RegionManager for it.
        /// </summary>
        /// <param name="notification">The notification to be set as a DataContext in the HostWindow.</param>
        /// <param name="wrapperWindow">The HostWindow</param>
        protected virtual void PrepareContentForWindow(Notification notification, Window wrapperWindow)
        {
            if (this.WindowContent == null)
            {
                return;
            }

            // We set the WindowContent as the content of the window. 
            wrapperWindow.Content = this.WindowContent;

            IInteractionRequestAware interactionAware;

            // If the WindowContent implements IInteractionRequestAware we set the corresponding properties.
            interactionAware = this.WindowContent as IInteractionRequestAware;
            if (interactionAware != null)
            {
                interactionAware.Notification = notification;
                interactionAware.FinishInteraction = () => wrapperWindow.Close();
            }

            // If the WindowContent's DataContext implements IInteractionRequestAware we set the corresponding properties.
            interactionAware = this.WindowContent.DataContext as IInteractionRequestAware;
            if (interactionAware != null)
            {
                interactionAware.Notification = notification;
                interactionAware.FinishInteraction = () => wrapperWindow.Close();
            }
        }

    }
}