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
using ExecutionEnvironment;

namespace EvolutionWpfControls
{
    public class EvolutionControl : AnyPopulationControl
    {
        public override ObservableCollection<IPresentable> Details
        {
            get
            {
                if (population == null)
                    return null;

                var result = base.Details;
                result.Insert(0, new Presentable("Limits", new NamedValueGrid(Limits())));
                return result;
            }
            set { base.Details = value; }
        }

        protected virtual List<NamedValue> Limits()
        {
            Evolution evolution = (Evolution)Population;
            return new List<NamedValue>()
            {
                new NamedValue("Generations", evolution.SimulationRound + " / " + evolution.MaxSimulationRounds),
                new NamedValue("Runtime", evolution.Runtime + " / " + evolution.MaxRuntime),
                new NamedValue("Fitness", evolution.Fitness + " / " + evolution.MaxFitness),
                new NamedValue("No Fitness Change", evolution.RoundsWithoutFitnessChange + " / " + evolution.MaxRoundsWithoutFitnessChange),
                new NamedValue("Food", evolution.FoodConsumedInLifetime + " / " + evolution.MaxFeedings)
            };
        }
    }
}
