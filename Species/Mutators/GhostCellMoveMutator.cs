using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Species
{
    public class GhostCellMoveMutator : BorderMoveMutator
    {
        public GhostCellMoveMutator() { IncludeFieldBorders = false; }
    }
}
