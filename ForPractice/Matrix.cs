using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForPractice
{
    public class Matrix
    {
        private double[,] data;
        public int m;
        public int n;

        public int M => this.m;

        public int N => this.n;

        public Matrix(int m, int n)
        {
            this.m = m;
            this.n = n;
            this.data = new double[m, n];
        }

        public Matrix(double[,] arr)
        {
            this.m = arr.GetLength(0);
            this.n = arr.GetLength(1);
            this.data = arr;
        }

        public double this[int x, int y]
        {
            get => this.data[x, y];
            set => this.data[x, y] = value;
        }

        public static Matrix operator *(Matrix matrix2, Matrix matrix)
        {
            if (matrix.N != matrix2.M)
                throw new ArgumentException("matrixes can not be multiplied");
            Matrix result = new Matrix(matrix.M, matrix2.N);
            result.ProcessFunctionOverData((Action<int, int>)((i, j) =>
            {
                for (int index = 0; index < matrix.N; ++index)
                    result[i, j] += matrix[i, index] * matrix2[index, j];
            }));
            return result;
        }

        public static Matrix operator *(Matrix matrix, double value)
        {
            Matrix result = new Matrix(matrix.M, matrix.N);
            result.ProcessFunctionOverData((Action<int, int>)((i, j) => result[i, j] = matrix[i, j] * value));
            return result;
        }

        public static Matrix operator /(Matrix matrix, double value)
        {
            Matrix result = new Matrix(matrix.M, matrix.N);
            result.ProcessFunctionOverData((Action<int, int>)((i, j) => result[i, j] = matrix[i, j] / value));
            return result;
        }

        public void ProcessFunctionOverData(Action<int, int> func)
        {
            for (int index1 = 0; index1 < this.M; ++index1)
            {
                for (int index2 = 0; index2 < this.N; ++index2)
                    func(index1, index2);
            }
        }

        public Matrix Copy()
        {
            Matrix matrix = new Matrix(this.n, this.m);
            for (int index1 = 0; index1 < this.n; ++index1)
            {
                for (int index2 = 0; index2 < this.m; ++index2)
                    matrix.data[index1, index2] = this.data[index1, index2];
            }
            return matrix;
        }

        public bool IsSquare => this.M == this.N;

        public double Determinant()
        {
            if (!this.IsSquare)
                throw new InvalidOperationException("determinant can be calculated only for square matrix");
            if (this.N == 2)
                return this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];
            double num = 0.0;
            for (int index = 0; index < this.N; ++index)
                num += (index % 2 == 1 ? 1.0 : -1.0) * this[1, index] * this.CreateMatrixWithoutColumn(index).CreateMatrixWithoutRow(1).Determinant();
            return num;
        }

        public Matrix CreateMatrixWithoutColumn(int column)
        {
            Matrix matrix = column >= 0 && column < this.N ? new Matrix(this.M, this.N - 1) : throw new ArgumentException("invalid column index");
            matrix.ProcessFunctionOverData((Action<int, int>)((i, j) => matrix[i, j] = j < column ? this[i, j] : this[i, j + 1]));
            return matrix;
        }

        public Matrix CreateMatrixWithoutRow(int row)
        {
            Matrix result = row >= 0 && row < this.M ? new Matrix(this.M - 1, this.N) : throw new ArgumentException("invalid row index");
            result.ProcessFunctionOverData((Action<int, int>)((i, j) => result[i, j] = i < row ? this[i, j] : this[i + 1, j]));
            return result;
        }

        public Matrix Transposition()
        {
            Matrix matrix = new Matrix(this.N, this.M);
            for (int index1 = 0; index1 < this.M; ++index1)
            {
                for (int index2 = 0; index2 < this.N; ++index2)
                    matrix[index2, index1] = this[index1, index2];
            }
            return matrix;
        }

        public Matrix InverseMatrix()
        {
            double num = this.Determinant();
            if (num == 0.0)
                throw new InvalidOperationException("determinant must be not 0");
            Matrix matrix = new Matrix(this.M, this.N);
            return this.MatrixOfMinorAdd(1).MatrixOfMinorAdd(2).Transposition() / num;
        }

        public Matrix MatrixOfMinorAdd(int check)
        {
            Matrix matrix1 = new Matrix(this.M, this.M);
            double num1 = 0.0;
            if (!this.IsSquare)
                throw new InvalidOperationException("determinant can be calculated only for square matrix");
            Matrix matrix2 = new Matrix(this.N, this.N);
            switch (check)
            {
                case 1:
                    num1 = 0.0;
                    if (this.N == 2)
                    {
                        for (int index1 = 0; index1 < this.n; ++index1)
                        {
                            for (int index2 = 0; index2 < this.n; ++index2)
                            {
                                Matrix matrixWithoutRow = this.CreateMatrixWithoutColumn(index2).CreateMatrixWithoutRow(index1);
                                matrix1[index1, index2] = matrixWithoutRow[0, 0];
                            }
                        }
                        return matrix1;
                    }
                    for (int index3 = 0; index3 < this.n; ++index3)
                    {
                        for (int index4 = 0; index4 < this.n; ++index4)
                        {
                            matrix2 = this.CreateMatrixWithoutColumn(index4).CreateMatrixWithoutRow(index3);
                            double num2 = this.CreateMatrixWithoutColumn(index4).CreateMatrixWithoutRow(index3).Determinant();
                            matrix1[index4, index3] = num2;
                        }
                    }
                    return matrix1;
                case 2:
                    num1 = 0.0;
                    if (this.N == 2)
                    {
                        for (int x = 0; x < this.n; ++x)
                        {
                            for (int y = 0; y < this.n; ++y)
                                matrix1[x, y] = Math.Pow(-1.0, (double)(x + y)) * this[x, y];
                        }
                        return matrix1;
                    }
                    for (int index5 = 0; index5 < this.n; ++index5)
                    {
                        for (int index6 = 0; index6 < this.n; ++index6)
                        {
                            double num3 = Math.Pow(-1.0, (double)(index5 + index6)) * this[index5, index6];
                            matrix1[index6, index5] = num3;
                        }
                    }
                    return matrix1;
                default:
                    return matrix1;
            }
        }

        public override string ToString()
        {
            string str = "";
            for (int index1 = 0; index1 < this.M; ++index1)
            {
                for (int index2 = 0; index2 < this.N; ++index2)
                    str = str + this.data[index1, index2].ToString() + " ";
                str += "\n";
            }
            return str;
        }

        static public Matrix FindSolveOverridenSLU(Matrix A, Matrix B)
        {
            Matrix aT = A.Transposition();
            Matrix aTA = aT * A;
            Matrix aTB = aT * B;
            return aTA.InverseMatrix() * aTB;
        }
    }
}
