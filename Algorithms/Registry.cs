using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Algorithms.CudaByExample;
using Algorithms.FurtherExamples;
using Algorithms.Upcrc2010.MatrixMultiplication;

namespace Algorithms
{
    public class Registry
    {
        public static Algorithm[] RegisteredAlgorithms = new Algorithm[]
        {
            // CUDAByExample
            new Average(),
            new DotProduct(),
            new HeatTransfer(),
            new JuliaSet(),
            new RayTracing(),
            new Ripple(),
            new SummingVectors(),

            // FurhterExamples
            new ConvolutionNPP(),
            new GameOfLife(),
            new Sum(),
            new Wator(),
            // SudokuValidator
            new SudokuValidator(),
            new SudokuValidator2D(),
            new SudokuValidatorInvalidColumn(),
            new SudokuValidatorInvalidNumbers(),
            new SudokuValidatorInvalidRow(),
            new SudokuValidatorInvalidSubfield(),

            // UPCRC2010
            new Convolution(),
            new MatrixVectorMultiplication(),
            new MinimumSpanningTree(),
            new PrefixScan(),
            // MatrixMultiplication
            new MatrixMultiplication0(),
            new MatrixMultiplication1(),
            new MatrixMultiplication2(),
            new MatrixMultiplication3(),
            new MatrixMultiplication4(),
            new MatrixMultiplication5(),
        };

        public static Algorithm ByName(string name) { return RegisteredAlgorithms.Where(a => a.ToString() == name).First(); }
    }
}
