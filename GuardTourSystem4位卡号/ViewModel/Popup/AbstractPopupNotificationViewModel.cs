using GuardTourSystem.Services;
using GuardTourSystem.View;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GuardTourSystem.ViewModel
{
    //所有通过InteractionRequest的Raise方法触发的 弹出窗口的 ViewModel 
    public abstract class AbstractPopupNotificationViewModel : ShowMetroDialogViewModel
    {
        public DelegateCommand CConfirm { get; set; } //点击 确认
        public DelegateCommand CClose { get; set; } //点击 取消

        public Action Close; //关闭弹出窗口
        public bool IsCancel { get; private set; } // 更新时,用户是否取消操作

        public virtual void Cancel() //点击取消
        {
            IsCancel = true;
            if (this.Close != null)
            {
                this.Close();
            }
        }
        public void Finish()  //点击确认后
        {
            IsCancel = false;
            if (this.Close != null)
            {
                this.Close();
            }
        }
        public AbstractPopupNotificationViewModel()
        {
            IsCancel = true;
            this.CClose = new DelegateCommand(this.Cancel);
        }

        public object Content { get; set; }
        public string Title { get; set; }
        private string errorInfo;
        public string ErrorInfo
        {
            get { return errorInfo; }
            set
            {
                errorInfo = value;
                RaisePropertyChanged("ErrorInfo");
            }
        }
        private string confirmButtonText;
        public string ConfirmButtonText
        {
            get { return confirmButtonText; }
            set
            {
                confirmButtonText = value;
                RaisePropertyChanged("ConfirmButtonText");
            }
        }
    }
}
