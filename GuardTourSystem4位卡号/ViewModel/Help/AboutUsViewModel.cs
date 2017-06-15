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

        public string SoftwareName { get; set; }
        public string SoftwareVersion { get; set; }
        public string CopyrightInfo { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyTel { get; set; }
        public string CompanyWebsite { get; set; }


        public AboutUsViewModel()
        {
            this.Title = "关于我们";

            this.SoftwareName = AppSetting.Default.SoftwareName;
            this.SoftwareVersion = AppSetting.Default.SoftwareVersion;
            this.CopyrightInfo = AppSetting.Default.CopyrightInfo;
            this.CompanyName = AppSetting.Default.CompanyName;
            this.CompanyAddress = AppSetting.Default.CompanyAddress;
            this.CompanyTel = AppSetting.Default.CompanyTel;
            this.CompanyWebsite = AppSetting.Default.CompanyWebsite;
        }
    }
}
