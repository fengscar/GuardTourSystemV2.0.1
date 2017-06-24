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
            Title = "关于我们";

            AppName = AppSetting.Default.SoftwareName;
            AppVersion = AppSetting.Default.SoftwareVersion;
            CompanyCopyright = AppSetting.Default.CopyrightInfo;
            CompanyName = AppSetting.Default.CompanyName;
            CompanyAddress = AppSetting.Default.CompanyAddress;
            CompanyTel = AppSetting.Default.CompanyTel;
            CompanyWebsite = AppSetting.Default.CompanyWebsite;
        }
    }
}
