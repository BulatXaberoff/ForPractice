using ForPractice;
#region
double[,] arr1 = new double[5, 3]
{
  {
    2.5,
    -3.0,
    1.2
  },
  {
    3.7,
    -2.0,
    4.1
  },
  {
    2.6,
    -1.1,
    5.9
  },
  {
    4.9,
    0.0,
    3.3
  },
  {
    7.4,
    1.8,
    5.7
  }
};
double[,] arr2 = new double[1, 5]
{
  {
    13.2,
    10.2,
    8.7,
    5.5,
    12.2
  }
};
Matrix matrix1 = new Matrix(arr1);
matrix1 = matrix1.Transposition();

Matrix matrix2 = new Matrix(arr2);
Console.WriteLine(Matrix.FindSolveOverridenSLU(matrix1, matrix2));
//Matrix matrix3 = matrix1.Transposition();
//Matrix matrix4 = matrix1 * matrix3;
//Matrix matrix5 = matrix1 * matrix2;
//Matrix matrix6 = matrix4.InverseMatrix();
//Matrix matrix7 = matrix6 * matrix5;
//Console.WriteLine((object)matrix4);
//Console.WriteLine((object)matrix5);
//Console.WriteLine((object)matrix6);
//Console.WriteLine((object)matrix7);
#endregion
//var size = 6;
//double x0 = 0.3;
//double y0 = 0.4;
//double[] x = FillArr(size);
//Showarr(x);
//double[] y = FillArr(size);
//Showarr(y);
//Console.WriteLine();
//double[,] z = Func(x, y);

//Showarr(z);
//Console.WriteLine(LagrangePolynomial(0.7, 0.1, x, y, z));
//Console.WriteLine(LagrangePolynomial(0.2, 0.1, x, y, z));
//InterpolateLagrange3D(x0, y0, x, y, z, size);
//InterpolateLagrange3D(x0 + 1, y0 + 1, x, y, z, size);
//InterpolateLagrange3D(x0 + 2, y0 + 2, x, y, z, size);

static void Showarr(double[] arr)
{
    for (int i = 0; i < arr.Length; i++)
    {
        Console.Write(arr[i] + " ");
    }
    Console.WriteLine();
}


static double[] FillArr(int size)
{
    double[] arr = new double[size];
    for (int i = 1; i < arr.Length; i++)
    {
        arr[i] = i;
    }
    return arr;
}

static double[,] Func(double[] x, double[] y)
{
    double[,] z = new double[x.Length, y.Length];
    for (int i = 0; i < x.Length; i++)
    {
        for (int j = 0; j < x.Length; j++)
        {
            z[i, j] = Math.Exp(x[j] + y[i]);
            Console.Write(z[i, j] + " ");
        }
        Console.WriteLine();
    }
    return z;
}

static double LagrangePolynomial(double x, double y, double[] xval, double[] yval, double[] zval)
{
    double lagraPol = 0;
    var size = xval.Length;
    double[] a = new double[size];
    for (int i = 0; i < size; i++)
    {
        double basicsPol = 1;
        for (int j = 0; j < size; j++)
        {
            if (i != j)
            {
                basicsPol *= (x - xval[j]) * (y - yval[j]) / ((xval[i] - xval[j]) * (yval[i] - yval[j]));
            }
        }
        lagraPol += basicsPol;
    }
    return lagraPol;
}

static double InterpolateLagrangePolynomial(double x, double[] xValues, double[] yValues, int size)
{
    double lagrangePol = 0;

    for (int i = 0; i < size; i++)
    {
        double basicsPol = 1;
        for (int j = 0; j < size; j++)
        {
            if (j != i)
            {
                basicsPol *= (x - xValues[j]) / (xValues[i] - xValues[j]);
            }
        }
        lagrangePol += basicsPol * yValues[i];
    }

    return lagrangePol;
}

static void InterpolateLagrange3D(double x0,double y0, double[] x, double[] y, double[,] z, int size)
{
    double[] a = new double[size];
    double[] temp = new double[size];

    for (int i = 0; i < size; i++)
    {
        for (int j = 0; j < size; j++)
        {
            temp[j] = z[i, j];
        }
        a[i] = InterpolateLagrangePolynomial(x0, x, temp, size);
    }
    Console.WriteLine(InterpolateLagrangePolynomial(y0, y, a, size));
}