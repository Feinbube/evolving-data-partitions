using EvolutionFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace EvolutionWpfControls
{
    public class PopulationControl : TitleDetailsAndChildrenControl
    {
        public IPopulation population = null;
        public IPopulation Population { get { return population; } set { population = value; updateView(); } }

        public override System.Collections.ObjectModel.ObservableCollection<IPresentable> Details
        {
            get
            {
                return Population == null ? null : new System.Collections.ObjectModel.ObservableCollection<IPresentable>()
                {
                    StatsAsPresentable(), AsDetailPresentable("Best", Population.Best), AsDetailPresentable("Worst", Population.Worst)
                };
            }
            set { base.Details = value; }
        }

        public override ObservableCollection<IPresentable> Children
        {
            get
            {
                if (Population == null) return null;

                var result = new ObservableCollection<IPresentable>();
                int index = 0;
                foreach (var individual in Population.IndividualsSortedByFitness)
                    result.Add(AsChildPresentable((++index).ToString(), individual));
                return result;
            }
            set { base.Children = value; }
        }

        protected override void updateView()
        {
            this.Title = population.GetType().Name;
            this.SubTitle = "";
            this.ChildrenTitle = "Population";

            base.updateView();
        }

        protected virtual IPresentable AsDetailPresentable(string title, IEvolvable evolvable)
        {
            if (evolvable is IPopulation)
                new Presentable("Population", new Label() { Content = evolvable.ToString() });

            if (evolvable is IPresentable)
                return evolvable as IPresentable;

            return new Presentable(title, new Label() { Content = evolvable.ToString() });
        }

        protected virtual IPresentable AsChildPresentable(string title, object evolvable)
        {
            if (evolvable is IPresentable)
                return evolvable as IPresentable;

            if (evolvable is IPopulation)
                return new Presentable(title, new PopulationControl() { Population = evolvable as IPopulation });

            return new Presentable(title, new Label() { Content = evolvable.ToString() });
        }

        protected IPresentable StatsAsPresentable()
        {
            return new Presentable("Stats", new NamedValueGrid(Stats()));
        }

        protected IEnumerable<NamedValue> Stats()
        {
            return new List<NamedValue>()
            {
                new NamedValue("Generations", Population.Generations),
                new NamedValue("FoodConsumedInLifetime", Population.FoodConsumedInLifetime),
                new NamedValue("PopulationSize", Population.Individuals.Count),
                new NamedValue("Muations", Population.Mutations),
                new NamedValue("Crossovers", Population.Crossovers),
                new NamedValue("FitnessEvaluations", Population.FitnessEvaluations)
            };
        }
    }
}
