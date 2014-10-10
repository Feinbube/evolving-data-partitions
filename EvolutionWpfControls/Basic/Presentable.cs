using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EvolutionWpfControls
{
    public class Presentable : IPresentable
    {
        public string PresentableTitle { get; set; }
        public Control PresentableControl { get; set; }

        public Presentable() { }

        public Presentable(string title, Control control)
        {
            this.PresentableTitle = title;
            this.PresentableControl = control;
        }
    }
}
