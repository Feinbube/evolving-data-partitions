﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Species
{
    public class GhostCellSwapMutator : BorderSwapMutator
    {
        public GhostCellSwapMutator() { IncludeFieldBorders = false; }
    }
}
