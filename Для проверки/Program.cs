using ForPractice;




int n = 3;
int num = 5;
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
Console.WriteLine(Matrix.FindSolveOverridenSLU(matrix1,matrix2));
//Matrix matrix3 = matrix1.Transposition();
//Matrix matrix4 = matrix1 * matrix3;
//Matrix matrix5 = matrix1 * matrix2;
//Matrix matrix6 = matrix4.InverseMatrix();
//Matrix matrix7 = matrix6 * matrix5;
//Console.WriteLine((object)matrix4);
//Console.WriteLine((object)matrix5);
//Console.WriteLine((object)matrix6);
//Console.WriteLine((object)matrix7);
