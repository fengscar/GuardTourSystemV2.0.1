using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GuardTourSystem.ViewModel.Query.ChartViewModel
{
    public class AssetData : BindableBase
    {
        private double value;
        public double Value
        {
            get { return value; }
            set
            {
                SetProperty(ref this.value, value);
            }
        }

        private double missValue;
        public double MissValue //只在Route Bar中使用
        {
            get { return missValue; }
            set
            {
                SetProperty(ref this.missValue, value);
            }
        }

        private string label;
        public string Label
        {
            get { return label; }
            set
            {
                SetProperty(ref this.label, value);
            }
        }


        private Brush brush;
        public Brush Brush
        {
            get { return brush; }
            set
            {
                SetProperty(ref this.brush, value);
            }
        }

        public AssetData(bool isGreen)
        {
            if (isGreen)
            {
                this.Brush = new SolidColorBrush(Color.FromArgb(255, 46, 204, 113)); //FlatGreen 
            }
            else
            {
                this.Brush = new SolidColorBrush(Color.FromArgb(255, 231, 76, 60));//FlatRed
            }
        }
    }
}
