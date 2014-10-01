/*    
*    MatrixVectorMultiplication.cs
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
    public class MatrixVectorMultiplication : Algorithm // http://myssa.upcrc.illinois.edu/files/Lab_OpenMP_Assignments/ 
    {
        // Sparse matrix structure
        struct SpMat
        {
            public int nrow;  //Num of rows;   
            public int ncol;  //Num of columns;   
            public int nnze;  //Num of non-zero elements;   
            public Arr<double> val;   //Stores the non-zero elements;   
            public Arr<int> rc_ind;  //Stores the row (CRS) or column (CCS) indexes of the elements in the val vector;   
            public Arr<int> rc_ptr;  //Stores the locations in the val vector that start a row (CRS) or column (CCS);   
            public Arr<int> rc_len;  //Stores the length of each column (CRS) or row (CCS);   
        }

        SpMat inputMatrix;
        Arr<double> fullInputMatrix;

        Arr<double> inputVector;

        Arr<double> resultVector;
        Arr<double> fullResultVector;

        int M;
        int N;

        protected override void setup()
        {
            if (sizeX * sizeY > 1024 * 1024 || sizeX * sizeY < 0)
            {
                sizeX = 1024;
                sizeY = 1024;
            }

            M = (int)sizeX;
            N = (int)sizeY;

            fullInputMatrix = new Arr<double>(M, N);

            inputVector = new Arr<double>(N);

            fullResultVector = new Arr<double>(M);
            resultVector = new Arr<double>(M);

            init_fullMatrix(fullInputMatrix);
            init_vector(inputVector);

            convert2CSR(ref inputMatrix, fullInputMatrix);
        }

        void init_fullMatrix(Arr<double> matrix)
        {
            // create random, directed weight matrix
            int i, j, k;

            for (i = 0; i < M; i++)
                for (j = 0; j < N; j++)
                    matrix[i, j] = 0.0;

            for (k = 0; k < 5 * M; k++)
            {
                i = Random.Next(0, (int)M);
                j = Random.Next(0, (int)N);
                matrix[i, j] = Random.NextDouble() * 10.0;
            }
        }

        void init_vector(Arr<double> vector)
        {
            // intialize random vector
            int i;
            for (i = 0; i < N; i++)
            {
                vector[i] = Random.NextDouble() * 10.0;
            }
        }

        void convert2CSR(ref SpMat spMat, Arr<double> A)
        {
            int i, j, k;
            int totalNumNZ = 0;

            spMat.nrow = M;
            spMat.ncol = N;

            spMat.rc_len = new Arr<int>(M);
            for (i = 0; i < M; i++)
            {
                int colCount = 0;
                for (j = 0; j < N; j++)
                {
                    if (A[i, j] != 0.0)
                    {
                        totalNumNZ++;
                        colCount++;
                    }
                }
                spMat.rc_len[i] = colCount;
            }
            spMat.nnze = totalNumNZ;

            spMat.rc_ptr = new Arr<int>(M + 1);
            spMat.rc_ptr[0] = 0;

            // exclusive prefix scan to find new row/column index pointers
            for (i = 1; i <= M; i++)
            {
                spMat.rc_ptr[i] = spMat.rc_ptr[i - 1] + spMat.rc_len[i - 1];
            }

            spMat.rc_ind = new Arr<int>(totalNumNZ);
            spMat.val = new Arr<double>(totalNumNZ);

            k = 0;
            for (i = 0; i < M; i++)
            {
                for (j = 0; j < N; j++)
                {
                    if (A[i, j] != 0.0)
                    {
                        spMat.val[k] = A[i, j];
                        spMat.rc_ind[k] = j;
                        k++;
                    }
                }
            }
        }

        protected override void printInput()
        {
            for (int i = 0; i < N; i++)
            {
                Console.Write(doubleToString(inputVector[i]) + " ");
                if ((i + 1) % 11 == 0) Console.WriteLine();
            }
            Console.WriteLine();
        }

        protected override void algorithm()
        {
            Parallel.For(0, (int)inputMatrix.nrow, delegate(int i)
            {
                resultVector[i] = 0.0;
                for (int j = inputMatrix.rc_ptr[i]; j < inputMatrix.rc_ptr[i + 1]; j++)
                {
                    resultVector[i] += inputMatrix.val[j] * inputVector[inputMatrix.rc_ind[j]];
                }
            });
        }

        protected override void printResult()
        {
            double diff = 0.0;

            fullMVmult();

            Console.WriteLine("Results Vector from Sparse MV:");
            for (int i = 0; i < M; i++)
            {
                Console.Write(doubleToString(resultVector[i]) + " ");
                diff += Math.Abs(fullResultVector[i] - resultVector[i]);
                if ((i + 1) % 11 == 0) Console.WriteLine();            
            }

            Console.WriteLine();
            Console.WriteLine("Diff = " + diff);
        }

        protected override bool isValid()
        {          
            double diff = 0.0;

            fullMVmult();

            for (int i = 0; i < M; i++)
                diff += Math.Abs(fullResultVector[i] - resultVector[i]);

            return diff < 1.0;
        }

        void fullMVmult()
        {
            int i, j;
            for (i = 0; i < M; i++)
            {
                fullResultVector[i] = 0.0;
                for (j = 0; j < N; j++)
                    fullResultVector[i] += fullInputMatrix[i, j] * inputVector[j];
            }
        }

        protected override void cleanup()
        {
            inputMatrix.val = null;
            inputMatrix.rc_ind = null;
            inputMatrix.rc_ptr = null;
            inputMatrix.rc_len = null;

            fullInputMatrix = null;
            inputVector = null;
            resultVector = null;
            fullResultVector = null;
        }
    }
}
