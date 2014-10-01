using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFieldLayoutSimulation
{
    public class GhostCellMoveMutator : BorderMoveMutator
    {
        public GhostCellMoveMutator() { IncludeFieldBorders = false; }
    }
}
