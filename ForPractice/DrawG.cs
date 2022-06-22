using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForPractice
{
    public class DrawG
    {
        static int c = 0;

        private int xx1, xx2, yy1, yy2;
        private int[] xx = new int[4];
        private int[] yy = new int[4];

        private double[] xInter = new double[4];
        private double[] yInter = new double[4];
        private double[] zInter = new double[4];


        public int left;
        public int top;
        public int width;
        public int height;
        double n = 5;
        public double X_min, Y_min, X_max, Y_max;
        public double x0=0, y0=0, z0=0;
        public double A=-8;
        public bool f_show=false;
        public double alfa=10, beta=12;
        public List<(double[], double[],double[])> coordinatesGraphicLists;



        Bitmap bitmap;
        Graphics gr;

        public DrawG(int width,int height)
        {
            this.width = width;
            this.height = height;
            bitmap = new Bitmap(width+10, height);
            gr = Graphics.FromImage(bitmap);
            clearSheet();
            X_min = -n; Y_min = -n+3; X_max = n; Y_max = n;
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

        public void drawGraphic(int count,double h=4.0,int nx=3,int ny=3,double n=5.0)
        {
            
            c = count;
            coordinatesGraphicLists = new List<(double[], double[],double[])>();
            h = 1 / h;
            this.n = n;
            X_min = -n; Y_min = -n + 3; X_max = n; Y_max = n;

            const double h0 = -0.3;
            int i, j;
            #region 
            //Rectangle r1 = new Rectangle(left, top, left + width, top + height);
            Pen p = new Pen(Color.Black);
            //e.Graphics.DrawRectangle(p, r1);

            // Создать шрифт
            Font font = new Font("Courier New", 12, FontStyle.Bold);
            SolidBrush b = new SolidBrush(Color.Blue);

            #endregion
            #region Рисование осей
            // рисование осей
            // ось X
            Zoom_XY(-0.3, 0, 0, out xx1, out yy1);
            Zoom_XY(2.5, 0, 0, out xx2, out yy2);

            gr.DrawLine(p, xx1, yy1, xx2, yy2);
            gr.DrawString("X", font, b, xx2 + 3, yy2);

            // ось Y
            Zoom_XY(0, -0.3, 0, out xx1, out yy1);
            Zoom_XY(0, 2.5, 0, out xx2, out yy2);
            gr.DrawLine(p, xx1, yy1, xx2, yy2);
            gr.DrawString("Y", font, b, xx2 + 3, yy2);

            // ось Z
            Zoom_XY(0, 0, -0.3, out xx1, out yy1);
            Zoom_XY(0, 0, 2.5, out xx2, out yy2);
            gr.DrawLine(p, xx1, yy1, xx2, yy2);
            gr.DrawString("Z", font, b, xx2 + 3, yy2 - 3);

            b.Color = Color.Red;

            for (i = 0; i <= 2; i++)
            {
                int t_X, t_Y;

                Zoom_XY(i, 0, 0, out t_X, out t_Y);
                gr.FillEllipse(Brushes.Red, t_X, t_Y, 6, 6);
                gr.DrawString(i.ToString(), font, b, t_X + 3, t_Y);

                Zoom_XY(0, i, 0, out t_X, out t_Y);
                gr.FillEllipse(Brushes.Red, t_X, t_Y, 6, 6);
                gr.DrawString(i.ToString(), font, b, t_X + 3, t_Y);

                Zoom_XY(0, 0, i, out t_X, out t_Y);
                gr.FillEllipse(Brushes.Red, t_X, t_Y, 6, 6);
                gr.DrawString(i.ToString(), font, b, t_X + 3, t_Y);
            }

            #endregion

            // рисование поверхности
            p.Color = Color.Black;
            p.Width = 1;
            double t =10.0 / nx;
            for (j = 0; j <= nx; j++)
            {
                for (i = 0; i <= ny; i++)
                {
                    //var x = h0 + h * i;
                    //var y = h0 + h * j;
                    #region начальный метод построения по i,j
                    //Zoom_XY(h0 + h * i, h0 + h * j, func(h0 + h * i, h0 + h * j),
                    //        out xx[0], out yy[0]);
                    //xInter[0] = h0 + h * i;
                    //yInter[0] = h0 + h * j;
                    //zInter[0] = func(h0 + h * i, h0 + h * j);

                    //Zoom_XY(h0 + h * i, h + h * j, func(h0 + h * i, h + h * j),
                    //        out xx[1], out yy[1]);
                    //xInter[1] = h0 + h * i;
                    //yInter[1] = h + h * j;
                    //zInter[1] = func(h0 + h * i, h + h * j);

                    //Zoom_XY(h + h * i, h + h * j, func(h + h * i, h + h * j),
                    //        out xx[2], out yy[2]);
                    //xInter[2] = h + h * i;
                    //yInter[2] = h + h * j;
                    //yInter[2] = func(h + h * i, h + h * j);

                    //Zoom_XY(h + h * i, h0 + h * j, func(h + h * i, h0 + h * j),
                    //        out xx[3], out yy[3]);
                    //xInter[3] = h + h * i;
                    //yInter[3] = h0 + h * j;
                    //zInter[3]= func(h + h * i, h0 + h * j);
                    #endregion
                    Zoom_XY(h0 + h * i*t, h0 + h * j * t, func(h0 + h * i * t, h0 + h * j * t),
                            out xx[0], out yy[0]);
                    xInter[0] = h0 + h * i;
                    yInter[0] = h0 + h * j;
                    zInter[0] = func(h0 + h * i, h0 + h * j);

                    Zoom_XY(h0 + h * i * t, h + h * j * t, func(h0 + h * i * t, h + h * j * t),
                            out xx[1], out yy[1]);
                    xInter[1] = h0 + h * i;
                    yInter[1] = h + h * j;
                    zInter[1] = func(h0 + h * i, h + h * j);

                    Zoom_XY(h + h * i * t, h + h * j * t, func(h + h * i * t, h + h * j * t),
                            out xx[2], out yy[2]);
                    xInter[2] = h + h * i;
                    yInter[2] = h + h * j;
                    yInter[2] = func(h + h * i, h + h * j);

                    Zoom_XY(h + h * i * t, h0 + h * j * t, func(h + h * i * t, h0 + h * j * t),
                            out xx[3], out yy[3]);
                    xInter[3] = h + h * i;
                    yInter[3] = h0 + h * j;
                    zInter[3] = func(h + h * i, h0 + h * j);

                    //coordinatesGraphicLists.Add((xInter, yInter,zInter));
                    //gr.DrawLine(p, xx[0], yy[0], xx[1], yy[1]);
                    //gr.DrawLine(p, xx[1], yy[1], xx[2], yy[2]);
                    //gr.DrawLine(p, xx[2], yy[2], xx[3], yy[3]);
                    //gr.DrawLine(p, xx[3], yy[3], xx[0], yy[0]);
                    gr.FillEllipse(Brushes.Black, xx[0], yy[0], 3, 3);
                    gr.FillEllipse(Brushes.Black, xx[1], yy[1], 3, 3);
                    gr.FillEllipse(Brushes.Black, xx[2], yy[2], 3, 3);
                    gr.FillEllipse(Brushes.Black, xx[3], yy[3], 3, 3);
                }
            }

            //for (j = 0; j <= 11; j++)
            //    for (i = 0; i <= 9; i++)
            //    {
            //        Zoom_XY(h0 + h * i, func(h0 + h * i, h0 + h * j), 0,
            //                out xx[0], out yy[0]);
            //        Zoom_XY(h0 + h * i, func(h0 + h * i, h0 + h * j), 0,
            //                out xx[1], out yy[1]);
            //        Zoom_XY(h0 + h * i, func(h0 + h * i, h0 + h * j), 0,
            //                out xx[2], out yy[2]);
            //        Zoom_XY(h0 + h * i, func(h0 + h * i, h0 + h * j), 0,
            //                out xx[3], out yy[3]);
            //        e.Graphics.DrawLine(p, xx[0], yy[0], xx[1], yy[1]);
            //        e.Graphics.DrawLine(p, xx[1], yy[1], xx[2], yy[2]);
            //        e.Graphics.DrawLine(p, xx[2], yy[2], xx[3], yy[3]);
            //        e.Graphics.DrawLine(p, xx[3], yy[3], xx[0], yy[0]);
            //    }
        }










        private void Zoom_XY(double x, double y, double z, out int xx, out int yy)
        {
            //double xn, yn, zn;

            double xn, yn;
            double tx, ty, tz;
            tx = (x - x0) * Math.Cos(alfa) - (y - y0) * Math.Sin(alfa);
            ty = ((x - x0) * Math.Sin(alfa) + (y - y0) * Math.Cos(alfa)) * Math.Cos(beta) -
                 (z - z0) * Math.Sin(beta);
            tz = ((x - x0) * Math.Sin(alfa) + (y - y0) * Math.Cos(alfa)) * Math.Sin(beta) +
                 (z - z0) * Math.Cos(beta);
            xn = tx / (tz / A + 1);
            yn = ty / (ty / A + 1);

            xx = (int)(width * (xn - X_min) / (X_max - X_min));
            yy = (int)(height * (yn - Y_max) / (Y_min - Y_max));
        }
        private double func(double x, double y)
        {
            double res = 0;
            switch (c)
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
    }
}
