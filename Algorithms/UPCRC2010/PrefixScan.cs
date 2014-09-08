/*    
*    PrefixScan.cs
*
﻿*    Copyright (C) 2012  Frank Feinbube, Jan-Arne Sobania, Ralf Diestelkämper
*
*    This library is free software: you can redistribute it and/or modify
*    it under the terms of the GNU Lesser General Public License as published by
*    the Free Software Foundation, either version 3 of the License, or
*    (at your option) any later version.
*
*    This library is distributed in the hope that it will be useful,
*    but WITHOUT ANY WARRANTY; without even the implied warranty of
*    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*    GNU Lesser General Public License for more details.
*
*    You should have received a copy of the GNU Lesser General Public License
*    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*
*    Frank [at] Feinbube [dot] de
*    jan-arne [dot] sobania [at] gmx [dot] net
*    ralf [dot] diestelkaemper [at] hotmail [dot] com
*
*/


﻿using ExecutionEnvironment;
using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithms
{
    public class PrefixScan : Algorithm // http://myssa.upcrc.illinois.edu/files/Lab_OpenMP_Assignments/
    {
        Array<float> startData;
        Array<float> IscanData;

        protected override void setup()
        {
            if (sizeX > Int32.MaxValue || sizeX < 0)
                sizeX = Int32.MaxValue;

            startData = new Array<float>(sizeX);
            IscanData = new Array<float>(sizeX);

            for (int i = 0; i < sizeX; i++)
                startData[i] = (float)(Random.NextDouble() * 10.0);
        }

        protected override void printInput()
        {
            Console.WriteLine("Starting array:");
            printField(startData, sizeX);
        }

        protected override void algorithm()
        {
            int increment = 1;

            Array<float> p1 = startData;
            Array<float> p2 = IscanData;
            Array<float> ptmp;

            IscanData[0] = startData[0];

            while (increment < sizeX)
            {

                Parallel.For(1, increment, delegate(int i)
                {
                    p2[i] = p1[i];
                });

                Parallel.For(increment, sizeX, delegate(int i)
                {
                    p2[i] = p1[i] + p1[i - increment];
                });

                // increment
                increment = increment << 1;

                // switch arrays
                ptmp = p1;
                p1 = p2;
                p2 = ptmp;
            }
        }

        protected override void printResult()
        {
            printField(IscanData, sizeX);
        }

        protected override bool isValid()
        {
            for (int i = 0; i < sizeX / 2 /* FIX ME */; i++)
                if (IscanData[i + 1] / IscanData[i] < 0.99999f)
                    return false;

            return true;
        }

        protected override void cleanup()
        {
            startData = null;
            IscanData = null;
        }
    }
}
