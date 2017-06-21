using GuardTourSystem.Utils;
using Microsoft.Practices.Prism.Interactivity;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GuardTourSystem.View
{
    public class CustomPopupWindowAction : PopupWindowAction
    {

        /// <summary>
        /// 通过重写PopupWindowAction中的GetWindow方法，设置Window的Style属性。
        /// 否则打开的只能是默认窗体，无法设置样式。
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        protected override Window GetWindow(INotification notification)
        {
            //默认在父窗口中心
            this.CenterOverAssociatedObject = true;
            this.IsModal = true;

            Window wrapperWindow;

            if (this.WindowContent != null)
            {
                wrapperWindow = new Window();
                wrapperWindow.Owner = MainWindow.Instance;
                wrapperWindow.ShowInTaskbar = false;

                wrapperWindow.DataContext = notification;
                wrapperWindow.Title = notification.Title;

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
                wrapperWindow = this.CreateDefaultWindow(notification);
            }

            return wrapperWindow;
        }
    }
}
