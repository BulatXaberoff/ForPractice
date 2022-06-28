namespace ForPractice
{
    public class Lagrange
    {
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

        public static double InterpolateLagrange3D(double x0, double y0, double[] x, double[] y, double[,] z, int size)
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
            //Console.WriteLine(InterpolateLagrangePolynomial(y0, y, a, size));
            return InterpolateLagrangePolynomial(y0, y, a, size);
        }
        
    }
}
