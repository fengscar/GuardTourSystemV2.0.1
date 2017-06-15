using GuardTourSystem.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.ViewModel.Popup
{
    class AboutUsViewModel : AbstractPopupNotificationViewModel
    {

        public string AppName { get; set; }
        public string AppVersion { get; set; }
        public string CompanyCopyright { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyTel { get; set; }
        public string CompanyWebsite { get; set; }


        public AboutUsViewModel()
        {
            this.Title = "关于我们";

            this.AppName = AppSetting.Default.SoftwareName;
            this.AppVersion = AppSetting.Default.SoftwareVersion;
            this.CompanyCopyright = AppSetting.Default.CopyrightInfo;
            this.CompanyName = AppSetting.Default.CompanyName;
            this.CompanyAddress = AppSetting.Default.CompanyAddress;
            this.CompanyTel = AppSetting.Default.CompanyTel;
            this.CompanyWebsite = AppSetting.Default.CompanyWebsite;
        }
    }
}
