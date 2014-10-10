using EvolutionFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;

namespace EvolutionWpfControls
{
    public class AnyPopulationControl : EvolvablePopulationControl
    {
        protected override IPresentable AsChildPresentable(string title, object evolvable)
        {
            if (evolvable is EvolvablePopulation)
                return new Presentable(title, new AnyPopulationControl() { Population = evolvable as EvolvablePopulation });

            return base.AsChildPresentable(title, evolvable);
        }

        protected override List<NamedValue> Stats()
        {
            List<NamedValue> result = base.Stats();

            if (Population is IndividualMutateAndCrossoverPopulation)
            {
                result.Add(new NamedValue("Food For Population", (Population as IndividualMutateAndCrossoverPopulation).FoodForPopulation));
                result.Add(new NamedValue("Max Size", (Population as IndividualMutateAndCrossoverPopulation).MaxSize));
            }

            if(Population is SelectMutateCrossoverPopulation)
            {
                result.Add(new NamedValue("Mutation Rate", (Population as SelectMutateCrossoverPopulation).MutationRate));
                result.Add(new NamedValue("Elite Clone %", (Population as SelectMutateCrossoverPopulation).EliteClonePercentage));
                result.Add(new NamedValue("Selected %", (Population as SelectMutateCrossoverPopulation).SelectedPercentage));
                result.Add(new NamedValue("Feedings For Breeding", (Population as SelectMutateCrossoverPopulation).EnoughFeedingsForBreeding));
                result.Add(new NamedValue("Breedings so Far", (Population as SelectMutateCrossoverPopulation).BreedingsSoFar));
            }

            return result;
        }
    }
}
