using System;
using System.Collections.Generic;
using System.Drawing;

namespace ForPractice
{
    public class DrawG
    {
        static int CountFunction = 0;

        private int xx1, xx2, yy1, yy2; //вспомогательные переменные для построения осей координат
                                        //в прострастве(используются как out в функции Zoom_XY()) 
        private int x, y; //пара точек предназначенные для вывода функции

        public int width;
        public int height;

        double n = 5;
        public double X_min, Y_min, X_max, Y_max; //величины которые представляют координаты параллепипеда,
                                                  //в котором выводит график функции
        public double A=-8; //коэффициент перспективы
        public double alfa=10, beta=12;//углы наблюдения графика
        public List<(double, double,double)> coordinatesGraphicLists; //содержит в себе хранимые
                                                                      //координаты точек для последующего интерполирования
        Bitmap bitmap;
        Graphics gr;

        public DrawG(int width,int height,List<(double, double, double)> coord=null)
        {
            this.width = width;
            this.height = height;
            this.coordinatesGraphicLists = coord;
            bitmap = new Bitmap(width+10, height);
            gr = Graphics.FromImage(bitmap);
            clearSheet();
            X_min = -n; Y_min = -n+3; X_max = n; Y_max = n;
        }
        public List<(double, double, double)> GetCoordinates()
        {
            return coordinatesGraphicLists;
        }
        public Bitmap GetBitmap()
        {
            return bitmap;
        }

        public void clearSheet()
        {
            gr.Clear(Color.White);
        }
        public void InvalidGraphic(int count,double h=1.0,int nx = 3, int ny = 3,double n=5.0)
        {
            clearSheet();
            drawGraphic(count,h, nx,ny,n);
        }

        public void drawGraphic(int countFunc,double h=4.0,int nx=3,int ny=3,double n=5.0)
        {

            CountFunction = countFunc;
            coordinatesGraphicLists = new List<(double, double, double)>();
            h = 1 / h;
            this.n = n;
            X_min = -n; Y_min = -n + 3; X_max = n; Y_max = n;

            const double h0 = -0.3;

            #region Рисование осей
            Pen p = new Pen(Color.Black);
            Font font = new Font("Courier New", 12, FontStyle.Bold); // Создать шрифт
            SolidBrush b = new SolidBrush(Color.Blue);
            DrawAxis(p, font, b); //Рисование Осей
            #endregion

            // рисование поверхности
            p.Color = Color.Black;
            p.Width = 1;
            double t = 10.0 / nx;
            for (int j = 0; j < ny; j++)
            {
                coordinatesGraphicLists.Add((h0 + h * j * t, h0 + h * j * t, Function(h0 + h * j * t, h0 + h * j * t)));
            }
            double[,] arrz = new double[nx,ny];
            double xtemp,ytemp,ztemp;
            for (int i = 0; i < nx; i++)
            {
                for (int j = 0; j < ny; j++)
                {
                    xtemp = h0 + h * i * t;
                    ytemp = h0 + h * j * t;
                    ztemp = Function(xtemp, ytemp);
                    arrz[i,j] = ztemp;
                    Zoom_G(xtemp, ytemp, ztemp,
                            out x, out y);
                    gr.FillEllipse(Brushes.Black, x, y, 3, 3);
                }
            }
            AmountPointers(nx*ny);
            ValueFuncMax(arrz);
            ValueFuncMin(arrz);
            
        }

     

        public void InterpolateGraphic(List<(double, double, double)> xyz,double h = 4.0, int nx = 3, int ny = 3, double n = 5.0)
        {
            clearSheet();
            coordinatesGraphicLists = xyz;
            double[] arrx; double[] arry;
            ToArr(out arrx, out arry);
            var arrz = ValueZ(arrx, arry);
            double t = 11.0 / nx;
            h = 1 / h;
            this.n = n;
            X_min = -n; Y_min = -n + 3; X_max = n; Y_max = n;
            const double h0 = -0.3;
            #region Шрифт и рисование оси
            Pen p = new Pen(Color.Black);

            // Создать шрифт
            Font font = new Font("Courier New", 12, FontStyle.Bold);
            SolidBrush b = new SolidBrush(Color.Blue);

            DrawAxis(p, font, b); //Рисование Осей
            p.Color = Color.Black;
            p.Width = 1;

            #endregion

            double[,] zInter = new double[nx, ny];
            double xval, yval, zval;
            for (int i = 0; i < nx; i++)
            {
                for (int j = 0; j < ny; j++)
                {
                    xval = h0 + h * i * t;
                    yval = h0 + h * j * t;
                    zval = Lagrange.InterpolateLagrange3D(xval, yval, arrx, arry, arrz, arrx.Length);
                    zInter[i, j] = zval;
                    Zoom_G(xval, yval, zval, out x, out y);
                    gr.FillEllipse(Brushes.Black, x, y, 3, 3);
                }
            }
            AmountPointers(nx * ny);
            ValueFuncMax(zInter);
            ValueFuncMin(zInter);
        }

        private void Zoom_G(double x, double y, double z, out int xx, out int yy)
        {
            double xn, yn;
            double rx, ry, rz;
            //Формула преобразования системы координат в 3-х мерном пространстве относительно точки(x0;y0;z0;) и углов (alfa,beta)
            //rx = (x - x0) * Math.Cos(alfa) - (y - y0) * Math.Sin(alfa);
            //ry = ((x - x0) * Math.Sin(alfa) + (y - y0) * Math.Cos(alfa)) * Math.Cos(beta) -
            //     (z - z0) * Math.Sin(beta);
            //rz = ((x - x0) * Math.Sin(alfa) + (y - y0) * Math.Cos(alfa)) * Math.Sin(beta) +
            //     (z - z0) * Math.Cos(beta);


            //но так как СК строить во круг точки (0;0;0), то формулу можно представить в таком виде
            rx = x  * Math.Cos(alfa) - y * Math.Sin(alfa);
            ry = (x * Math.Sin(alfa) + y * Math.Cos(alfa)) * Math.Cos(beta) - z  * Math.Sin(beta);
            rz = (x * Math.Sin(alfa) + y * Math.Cos(alfa)) * Math.Sin(beta) +
                 z * Math.Cos(beta);
            //Одноточечное проецирование на плоскость Z=0
            xn = rx / (rz / A + 1);
            yn = ry / (ry / A + 1);

            xx = (int)(width * (xn - X_min) / (X_max - X_min));
            yy = (int)(height * (yn - Y_max) / (Y_min - Y_max));
        }

        private void AmountPointers(int amt)
        {
            string st = $"Кол-во nx*ny точек {amt}";
            gr.DrawString(st, new Font(FontFamily.GenericSansSerif, 12), new SolidBrush(Color.Black), 0, 32);
        }

        private void DrawAxis(Pen p, Font font, SolidBrush b)
        {
            // рисование осей
            // ось X
            Zoom_G(-0.3, 0, 0, out xx1, out yy1);
            Zoom_G(2.5, 0, 0, out xx2, out yy2);

            gr.DrawLine(p, xx1, yy1, xx2, yy2);
            gr.DrawString("X", font, b, xx2 + 3, yy2);

            // ось Y
            Zoom_G(0, -0.3, 0, out xx1, out yy1);
            Zoom_G(0, 2.5, 0, out xx2, out yy2);
            gr.DrawLine(p, xx1, yy1, xx2, yy2);
            gr.DrawString("Y", font, b, xx2 + 3, yy2);

            // ось Z
            Zoom_G(0, 0, -0.3, out xx1, out yy1);
            Zoom_G(0, 0, 2.5, out xx2, out yy2);
            gr.DrawLine(p, xx1, yy1, xx2, yy2);
            gr.DrawString("Z", font, b, xx2 + 3, yy2 - 3);

            b.Color = Color.Red;

            for (int z = 0; z <= 2; z++)
            {
                int t_X, t_Y;

                Zoom_G(z, 0, 0, out t_X, out t_Y);
                gr.FillEllipse(Brushes.Red, t_X, t_Y, 6, 6);
                gr.DrawString(z.ToString(), font, b, t_X + 3, t_Y);

                Zoom_G(0, z, 0, out t_X, out t_Y);
                gr.FillEllipse(Brushes.Red, t_X, t_Y, 6, 6);
                gr.DrawString(z.ToString(), font, b, t_X + 3, t_Y);

                Zoom_G(0, 0, z, out t_X, out t_Y);
                gr.FillEllipse(Brushes.Red, t_X, t_Y, 6, 6);
                gr.DrawString(z.ToString(), font, b, t_X + 3, t_Y);
            }
        }

        private double Function(double x, double y)
        {
            double res = 0;
            switch (CountFunction)
            {
                case 0:
                    res = 1;
                    break;
                case 1:
                    res = x;
                    break;
                case 2:
                    res = y;
                    break;
                case 3:
                    res = x + y;
                    break;
                case 4:
                    res=Math.Sqrt(Math.Pow(x,2)+Math.Pow(y,2));
                    break;
                case 5:
                    res = Math.Pow(x, 2) + Math.Pow(y, 2);
                    break;
                case 6:
                    res = Math.Exp(Math.Pow(x, 2) - Math.Pow(y, 2));
                    break;
                case 7:
                    res = 1 / (25 * (Math.Pow(x, 2) + Math.Pow(y, 2)) + 1);
                    break;
            }
            return res;
        }

        private double[,] ValueZ(double[] x, double[] y)
        {
            double[,] z = new double[x.Length, y.Length];
            for (int i = 0; i < x.Length; i++)
            {
                for (int j = 0; j < x.Length; j++)
                {
                    z[i, j] = Function(x[j], y[i]);
                    //Console.Write(z[i, j] + " ");
                }
                //Console.WriteLine();
            }
            return z;
        }
        private void ValueFuncMax(double[,] arr)
        {
            var max = -1.0;
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (max < arr[i, j])
                    {
                        max = arr[i, j];
                    }
                }
            }
            var st = $"|Fmax|={max}";
            gr.DrawString(st, new Font(FontFamily.GenericSansSerif, 12), new SolidBrush(Color.Black), 0, 0);
        }
        private void ValueFuncMin(double[,] arr)
        {
            var min = arr[0, 0];
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (min > arr[i, j])
                    {
                        min = arr[i, j];
                    }
                }
            }
            var st = $"|Fmin|={min}";
            gr.DrawString(st, new Font(FontFamily.GenericSansSerif, 12), new SolidBrush(Color.Black), 0, 16);
        }
        private void ToArr(out double[] arrx, out double[] arry)
        {
            int size = coordinatesGraphicLists.Count;
            arrx = new double[size];
            arry = new double[size];
            int count = 0;
            foreach (var item in coordinatesGraphicLists)
            {
                arrx[count] = item.Item1;
                arry[count] = item.Item2;
                count++;
            }
        }
    }
}
