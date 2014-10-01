using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionFramework
{
    public abstract class Feedable : IFeedable
    {
        public Feedable() { }
        protected Feedable(double foodConsumedInLifetime) { construct(foodConsumedInLifetime); }
        protected Feedable(Feedable original) { construct(original.FoodConsumedInLifetime); }

        private void construct(double foodConsumedInLifetime) { this.FoodConsumedInLifetime = foodConsumedInLifetime; }

        public double FoodConsumedInLifetime { get; set; }

        public void Feed(double resources) { FoodConsumedInLifetime += resources; feed(resources); }

        protected abstract void feed(double resources);
    }
}
