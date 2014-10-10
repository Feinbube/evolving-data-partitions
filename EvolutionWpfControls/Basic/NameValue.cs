using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionWpfControls
{
    public class NamedValue
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public NamedValue() { }

        public NamedValue(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
