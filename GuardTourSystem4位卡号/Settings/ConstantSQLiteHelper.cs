using GuardTourSystem.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.Settings
{
    class ConstantSQLiteHelper : SQLiteHelper
    {
        public override string DATABASE_FILE_NAME
        {
            get
            {
                return "AppSetting.db";
            }
        }
        private static ConstantSQLiteHelper instance;
        public new static ConstantSQLiteHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ConstantSQLiteHelper();
                }
                return instance;
            }
            private set { }
        }
    }
}
