using GuardTourSystem.Model.Settings;
using GuardTourSystem.Utils;
using GuardTourSystem.Properties;
using GuardTourSystem.Settings;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GuardTourSystem.ViewModel
{
    class LanguageViewModel : AbstractPopupNotificationViewModel
    {
        private List<Language> languageList;

        public List<Language> LanguageList
        {
            get { return languageList; }
            set
            {
                SetProperty(ref this.languageList, value);
            }
        }

        private Language language;
        public Language Language
        {
            get
            {
                return language;
            }
            set
            {
                // 没有改变 不修改..
                if (Language != null && Language.Code.Equals(value.Code))
                {
                    return;
                }
                SetProperty(ref this.language, value);
            }
        }

        /// <summary>
        /// Constractor
        /// </summary>
        public LanguageViewModel()
        {
            Title = "选择语言";
            ConfirmButtonText = "确定";
            // 初始化语言菜单栏
            this.LanguageList = Language.GetAllLanguage();
            this.Language = Language.FromCode(AppSetting.Default.Language);
            this.CConfirm = new DelegateCommand(this.ChangeLanguage);
        }

        private void ChangeLanguage()
        {
            AppSetting.Default.Language = this.Language.Code;
            AppSetting.Default.Save();

            LanLoader.ChangeLanguage(Language.Code);
            this.Finish();
        }
    }
}
