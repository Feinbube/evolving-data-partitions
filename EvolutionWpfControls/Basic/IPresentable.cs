﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EvolutionWpfControls
{
    public interface IPresentable
    {
        string PresentableTitle { get; }
        UIElement PresentableControl { get; }
    }
}
