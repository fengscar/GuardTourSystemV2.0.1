using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Utils
{
    public class DBug
    {
        public static bool DEBUG { get; set; }
        public static void w(Object obj)
        {
            if (DEBUG)
            {
                String str = DateTime.Now + " " + obj.ToString();
                System.Diagnostics.Debug.WriteLine(str);
            }
        }
        public static void w(Object model, Object obj)
        {
            if (DEBUG)
            {
                String str = DateTime.Now + " [" + model.ToString() + "] " + obj.ToString();
                System.Diagnostics.Debug.WriteLine(str);
            }
        }
    }
}
