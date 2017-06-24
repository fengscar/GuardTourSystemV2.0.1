using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Model.Settings
{
    class Language
    {
        public const string ZH_CN = "zh-cn";
        public const string ZH_HK = "zh-hk";
        public const string EN_US = "en-us";

        public string Name { get; set; }

        public string Code { get; set; }

        private Language(string name,string code)
        {
            Name = name;
            Code = code;
        }

        public static Language Lan_ZH_CN = new Language("中文(简体)", ZH_CN);
        public static Language Lan_ZH_HK = new Language("中文(繁体)", ZH_HK);
        public static Language Lan_EN_US = new Language("English", EN_US);

        //得到当前支持的所有语言
        public static List<Language> GetAllLanguage()
        {
            List<Language> list=new List<Language>();
            list.Add(Lan_ZH_CN);
            list.Add(Lan_ZH_HK);
            list.Add(Lan_EN_US);
            return list;
        }

        public static Language FromCode(string lanCode)
        {
            switch (lanCode)
            {
                case ZH_CN:
                    return Lan_ZH_CN;
                case ZH_HK:
                    return Lan_ZH_HK;
                case EN_US:
                    return Lan_EN_US;
                default: 
                    return Lan_ZH_CN;
            }
        }
    }
}
