using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EvolutionWpfControls
{
    /// <summary>
    /// Interaction logic for TitleDetailsAndChildrenControl.xaml
    /// </summary>
    public partial class TitleDetailsAndChildrenControl : UserControl
    {
        public virtual string Title { get; set; }
        public virtual string SubTitle { get; set; }
        public virtual ObservableCollection<IPresentable> Details { get; set; }


        public virtual bool ForceShowChildren { get; set; }
        public virtual bool ShowChildren { get { return ForceShowChildren || (Children != null && Children.Count > 0); } }
        public virtual string ChildrenTitle { get; set; }
        public virtual ObservableCollection<IPresentable> Children { get; set; }

        public TitleDetailsAndChildrenControl()
        {
            ForceShowChildren = true;

            InitializeComponent();

            this.DataContext = this;
        }

        protected virtual void updateView()
        {
            this.DataContext = null;
            this.DataContext = this;
        }
    }
}
