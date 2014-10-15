using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EvolutionWpfControls
{
    public class Presentable : IPresentable
    {
        public string PresentableTitle { get; set; }
        public UIElement PresentableControl { get; set; }

        public Presentable() { }

        public Presentable(string title, UIElement control)
        {
            this.PresentableTitle = title;
            this.PresentableControl = control;
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}
