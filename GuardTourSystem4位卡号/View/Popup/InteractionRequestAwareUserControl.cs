using GuardTourSystem.ViewModel;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace GuardTourSystem.View
{
    // 弹出窗口 的父类, 初始化了 Action Close 
    public class InteractionRequestAwareUserControl : ShowMetroDialogView, IInteractionRequestAware
    {
        public AbstractPopupNotificationViewModel ViewModel { get; set; }
        public InteractionRequestAwareUserControl()
        {
        }
        public Action finishInteraction { get; set; }
        public Action FinishInteraction
        {
            get { return finishInteraction; }
            set
            {
                finishInteraction = value;

                this.ViewModel= notification as AbstractPopupNotificationViewModel;
                this.ViewModel.Close = this.FinishInteraction; //初始化Close
                this.ViewModel.Close += () => { MainWindow.Instance.HideOverlay(); }; //初始化Close

                this.KeyUp -= KeyUpAction;
                this.KeyUp += KeyUpAction;
            }
        }
        private void KeyUpAction(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) //点击Enter,进入
            {
                if (ViewModel.CConfirm != null && ViewModel.CConfirm.CanExecute())
                {
                    ViewModel.CConfirm.Execute();
                }
            }
            else if (e.Key == Key.Escape) //点击Escape,退出
            {
                if (ViewModel.CClose != null)
                {
                    ViewModel.CClose.Execute();
                }
            }
        }

        //初始化时 ,value 为 ViewModel  , 并自动绑定为DataContext
        public INotification notification { get; set; }
        public INotification Notification
        {
            get
            {
                return notification;
            }
            set
            {
                notification = value;
                MainWindow.Instance.ShowOverlay();
            }
        }

    }
}
