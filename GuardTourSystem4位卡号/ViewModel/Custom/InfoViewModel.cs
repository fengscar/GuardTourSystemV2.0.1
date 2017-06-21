using GuardTourSystem.Utils;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuardTourSystem.ViewModel
{
    // 操作信息 
    class InfoViewModel : NotificationObject
    {
        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }

        private string result;
        public string Result
        {
            get { return result; }
            set
            {
                result = value;
                RaisePropertyChanged("Result");
            }
        }


        public StringBuilder ResultBuilder { get; set; }

        public InfoViewModel(string title = null)
        {
            if (title == null)
            {
                title = LanLoader.Load(LanKey.InfoViewDefaultTitle);
            }
            Title = title;
            Result = "";
            ResultBuilder = new StringBuilder();
        }

        public void Clear()
        {
            ResultBuilder.Clear();
            Result = "";
        }

        public void Append(string str)
        {
            ResultBuilder.Append(str).Append("\n");
            Result = ResultBuilder.ToString();
        }

        public void Append(params string[] strs)
        {
            foreach (var item in strs)
            {
                ResultBuilder.Append(item);
            }
            ResultBuilder.Append("\n");
            Result = ResultBuilder.ToString();
        }

        public void Append(params LanKey[] Lan)
        {
            foreach (var item in Lan)
            {
                ResultBuilder.Append(LanLoader.Load(item));
            }
            ResultBuilder.Append("\n");
            Result = ResultBuilder.ToString();
        }

        public void Append(LanKey Lan, params object[] param)
        {
            ResultBuilder.Append(LanLoader.Load(Lan, param)).Append("\n");
            Result = ResultBuilder.ToString();
        }
    }
}
