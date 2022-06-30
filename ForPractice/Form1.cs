using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMath;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace ForPractice
{
    public partial class Form1 : Form
    {
        public DrawG G;
        public DrawG G1;
        static int count = 7;
        static double h=7;
        static int nx = 3;
        static int ny = 3;
        static double n = 5;
        string[] functions = { "f(x,y)=1", "f(x,y)=x", "f(x,y)=y",
            "f(x,y)=x+y", @"f(x,y)=\sqrt{x^2+y^2}", "f(x,y)=x^2+y^2", 
            "f(x,y)=e^{x^2-y^2}", @"f(x,y)=\frac{1}{25\cdot(x^2+y^2)+1}" };

        public List<(double, double, double)> coordinates;

        public Form1()
        {
            InitializeComponent();
            G = new DrawG(pictureBox1.Width, pictureBox1.Height);
            G.drawGraphic(count, h, 10, 10,n);
            pictureBox1.Image = G.GetBitmap();
            G1 = new DrawG(pictureBox2.Width, pictureBox2.Height);
            G1.InterpolateGraphic(G.GetCoordinates(), h, nx, ny, n);
            pictureBox2.Image = G1.GetBitmap();
            SetPicture(functions[count]);
            //G=new DrawG(pictureBox2.Width, pictureBox2.Height);
            //G.drawGraphic(count, h, nx, ny, n);
            //pictureBox2.Image = G.GetBitmap();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            coordinates = G.GetCoordinates();
            if (e.KeyData == Keys.Down)
            {
                G.beta = G.beta - 0.1;
                G.InvalidGraphic(count, h,10,10, n);
                pictureBox1.Image = G.GetBitmap();

                G1.beta = G1.beta - 0.1;
                G1.InterpolateGraphic(coordinates, h, nx, ny, n);
                pictureBox2.Image = G1.GetBitmap();

            }
            if (e.KeyData == Keys.Up)
            {
                G.beta = G.beta + 0.1;
                G.InvalidGraphic(count, h, 10, 10, n);
                pictureBox1.Image = G.GetBitmap();

                G1.beta = G1.beta + 0.1;
                G1.InterpolateGraphic(coordinates, h, nx, ny, n);
                pictureBox2.Image = G1.GetBitmap();
            }
            if (e.KeyData == Keys.Right)
            {
                G.alfa = G.alfa + 0.1;
                G.InvalidGraphic(count, h, 10, 10, n);
                pictureBox1.Image = G.GetBitmap();

                G1.alfa = G1.alfa + 0.1;
                G1.InterpolateGraphic(coordinates, h, nx, ny, n);
                pictureBox2.Image = G1.GetBitmap();
            }
            if (e.KeyData == Keys.Left)
            {
                G.alfa = G.alfa - 0.1;
                G.InvalidGraphic(count, h, 10, 10, n);
                pictureBox1.Image = G.GetBitmap();

                G1.alfa = G1.alfa - 0.1;
                G1.InterpolateGraphic(coordinates, h, nx, ny, n);
                pictureBox2.Image = G1.GetBitmap();
            }
            if (e.KeyData == Keys.Escape)
            {
                Close();
            }
            if (e.KeyData == Keys.S)
            {
                if (pictureBox1.Image != null) //если в pictureBox есть изображение
                {
                    //создание диалогового окна "Сохранить как..", для сохранения изображения
                    SaveFileDialog savedialog = new SaveFileDialog();
                    savedialog.Title = "Сохранить картинку как...";
                    //отображать ли предупреждение, если пользователь указывает имя уже существующего файла
                    savedialog.OverwritePrompt = true;
                    //отображать ли предупреждение, если пользователь указывает несуществующий путь
                    savedialog.CheckPathExists = true;
                    //список форматов файла, отображаемый в поле "Тип файла"
                    savedialog.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
                    //отображается ли кнопка "Справка" в диалоговом окне
                    savedialog.ShowHelp = true;
                    if (savedialog.ShowDialog() == DialogResult.OK) //если в диалоговом окне нажата кнопка "ОК"
                    {
                        try
                        {
                            pictureBox1.Image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        catch
                        {
                            MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            if(e.KeyData == Keys.D0)
            {
                count++;
                count %= 8;
                G.InvalidGraphic(count,h, 10, 10, n);
                pictureBox1.Image = G.GetBitmap();

                coordinates = G.GetCoordinates();
                G1.InterpolateGraphic(coordinates,h, nx, ny, n);
                pictureBox2.Image = G1.GetBitmap();
                SetPicture(functions[count]);

            }
            if (e.KeyData == Keys.Enter)
            {
                numericUpDown1.Enabled = false;

            }
        }
        void PrintLatex(string latex)
        {
            
        }
        void SetPicture(string latex)
        {
          
            string l = @"\color{purple}{" + latex+"}";
            latex = l;
            var parser = new TexFormulaParser();
            var formula = parser.Parse(latex);
            var pngBytes = formula.RenderToPng(20.0, 1.0, 0.0, "Arial");
            Bitmap bmp;
            using (var ms = new MemoryStream(pngBytes))
            {
                bmp = new Bitmap(ms);
            }
            pictureBox3.Image = bmp;
            //using (Graphics g=Graphics.FromImage(bmp))
            //{
            //    using (Image im=bmp)
            //    {

            //    }
            //}
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            h=((double)numericUpDown1.Value);
            G.InvalidGraphic(count, h, 10,10, n);
            pictureBox1.Image = G.GetBitmap();

            G1.InterpolateGraphic(G.GetCoordinates(), h, nx, ny, n);
            pictureBox2.Image=G1.GetBitmap();
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            nx = (int)numericUpDown2.Value;
            ny = (int)numericUpDown2.Value;
            G.InvalidGraphic(count, h, 10, 10, n);
            pictureBox1.Image = G.GetBitmap();

            G1.InterpolateGraphic(G.GetCoordinates(), h, nx, ny, n);
            pictureBox2.Image = G1.GetBitmap();
        }
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            n = 0.05*(double)numericUpDown3.Value;
            G.InvalidGraphic(count, h, 10,10, n);
            pictureBox1.Image = G.GetBitmap();

            G1.InterpolateGraphic(G.GetCoordinates(), h, nx, ny, n);
            pictureBox2.Image = G1.GetBitmap();


        }
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            numericUpDown1.Enabled = true;
            numericUpDown2.Enabled = true;
            numericUpDown3.Enabled = true;
        }

        private void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;
            numericUpDown3.Enabled = false;
        }
       
    }
}
