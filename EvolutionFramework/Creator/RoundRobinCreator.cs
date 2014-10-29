using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionFramework
{
    public class RoundRobinCreator : ICreator
    {
        List<ICreator> creators = null;
        int index = -1;

        public RoundRobinCreator(List<ICreator> creators) {            this.creators = creators; }

        public IEvolvable Create()
        {
            index = (index + 1) % creators.Count;
            return creators[index].Create();
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}
