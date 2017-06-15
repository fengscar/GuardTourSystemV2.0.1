using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuardTourSystem.Utils
{
    class ChmHelper
    {
        public static string Help_CHM_Path
        {
            get
            {
                System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
                return asm.Location.Remove(asm.Location.LastIndexOf("\\")) + "\\help.chm";
            }
            private set { }
        }

        /// <summary>
        /// 打开 帮助文档 CHM
        /// </summary>
        /// <returns>如果未找到文件返回false</returns>
        public static bool OpenHelp()
        {
            if (File.Exists(Help_CHM_Path) == false)
            {
                return false;
            }
            Process.Start(Help_CHM_Path);
            return true;
        }



        /// <summary>
        /// 打开 帮助文档 CHM 指定的页面, 参数是 
        /// <param name="topic">指定的页面名称 比如: "index.html"</param>
        /// <returns>如果未找到文件返回false</returns>
        /// </summary>
        public static bool OpenHelp(string topic)
        {
            if (File.Exists(Help_CHM_Path) == false)
            {
                return false;
            }
            Help.ShowHelp(null, Help_CHM_Path, HelpNavigator.Topic, "index.html");
            return true;
        }
    }
}
