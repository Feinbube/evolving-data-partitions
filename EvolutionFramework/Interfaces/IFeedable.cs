using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionFramework
{
    public interface IFeedable
    {
        double FoodConsumedInLifetime { get; }
        void Feed(double resources);
    }
}
