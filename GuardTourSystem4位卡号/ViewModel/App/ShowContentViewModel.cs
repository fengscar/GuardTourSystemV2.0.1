using GuardTourSystem.Utils;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GuardTourSystem.ViewModel
{
    /// <summary>
    /// 在APP界面的Content区域所显示的View对应的ViewModel
    /// 1. 支持关闭当前界面, 对应的命令位 CClose
    /// 2. 支持显示错误提示, 对应的绑定属性为 Error
    /// 3. 支持导出Excel,只要有Name为 ExportGrid的控件
    /// </summary>
    public abstract class ShowContentViewModel : ShowMetroDialogViewModel
    {

        #region 关闭当前界面
        //关闭当前的显示内容
        public DelegateCommand CClose { get; set; }
        //班次的子类可能要在Close前 提醒用户是否需要保存
        protected virtual void Close()
        {
            AppContentViewModel.Instance.CloseView();
        }
        #endregion

        #region Excel的导出
        public DelegateCommand<Control> CExport { get; set; }
        protected virtual string ExportTitle { get { return null; } } //子类重载该属性即可在第一行显示文本
        protected virtual void Export(Control control)
        {
            ExcelExporter.ExportData(control, true, ExportTitle);
        }
        #endregion


        /// <summary>
        /// 通用的错误提示
        /// </summary>
        private string error;
        public string Error
        {
            get { return error; }
            set
            {
                SetProperty(ref this.error, value);
            }
        }


        public ShowContentViewModel()
        {
            this.CClose = new DelegateCommand(this.Close);
            this.CExport = new DelegateCommand<Control>(this.Export);
        }


    }
}
