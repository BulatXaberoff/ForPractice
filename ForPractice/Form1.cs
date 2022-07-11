using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WpfMath;

namespace ForPractice
{
    public partial class Form1 : Form
    {
        public DrawG G1;
        public DrawG G2;
        static int countfunction = 7;
        static double h=7;


        static int nx1 = 10;
        static int ny1 = 10;

        static int nx2 = 3;
        static int ny2 = 3;
        

        static double n = 5;
        string[] functions = { "f(x,y)=1", "f(x,y)=x", "f(x,y)=y",
            "f(x,y)=x+y", @"f(x,y)=\sqrt{x^2+y^2}", "f(x,y)=x^2+y^2", 
            "f(x,y)=e^{x^2-y^2}", @"f(x,y)=\frac{1}{25\cdot(x^2+y^2)+1}" };

        public List<(double, double, double)> coordinates;

        public Form1()
        {
            InitializeComponent();
            SetPicture(functions[countfunction]);
            helpProvider1.HelpNamespace = Application.StartupPath + "\\readme.txt";

            G1 = new DrawG(pictureBox1.Width, pictureBox1.Height);
            G1.drawGraphic(countfunction, h, nx1, ny1,n);
            pictureBox1.Image = G1.GetBitmap();

            G2 = new DrawG(pictureBox2.Width, pictureBox2.Height);
            G2.InterpolateGraphic(G1.GetCoordinates(), h, nx2, ny2, n);
            pictureBox2.Image = G2.GetBitmap();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            coordinates = G1.GetCoordinates();
            if (e.KeyData == Keys.Down)
            {
                G1.beta = G1.beta - 0.1;
                G1.InvalidGraphic(countfunction, h, nx1, ny1, n);
                pictureBox1.Image = G1.GetBitmap();

                G2.beta = G2.beta - 0.1;
                G2.InterpolateGraphic(coordinates, h, nx2, ny2, n);
                pictureBox2.Image = G2.GetBitmap();

            }
            if (e.KeyData == Keys.Up)
            {
                G1.beta = G1.beta + 0.1;
                G1.InvalidGraphic(countfunction, h, nx1, ny1, n);
                pictureBox1.Image = G1.GetBitmap();

                G2.beta = G2.beta + 0.1;
                G2.InterpolateGraphic(coordinates, h, nx2, ny2, n);
                pictureBox2.Image = G2.GetBitmap();
            }
            if (e.KeyData == Keys.Right)
            {
                G1.alfa = G1.alfa + 0.1;
                G1.InvalidGraphic(countfunction, h, nx1, ny1, n);
                pictureBox1.Image = G1.GetBitmap();

                G2.alfa = G2.alfa + 0.1;
                G2.InterpolateGraphic(coordinates, h, nx2, ny2, n);
                pictureBox2.Image = G2.GetBitmap();
            }
            if (e.KeyData == Keys.Left)
            {
                G1.alfa = G1.alfa - 0.1;
                G1.InvalidGraphic(countfunction, h, nx1, ny1, n);
                pictureBox1.Image = G1.GetBitmap();

                G2.alfa = G2.alfa - 0.1;
                G2.InterpolateGraphic(coordinates, h, nx2, ny2, n);
                pictureBox2.Image = G2.GetBitmap();
            }
            if (e.KeyData == Keys.D2)
            {
                try
                {
                    n = 0.05 * (double)(++numericUpDown3.Value);
                    G1.InvalidGraphic(countfunction, h, 10, 10, n);
                    pictureBox1.Image = G1.GetBitmap();

                    G2.InterpolateGraphic(G1.GetCoordinates(), h, nx2, ny2, n);
                    pictureBox2.Image = G2.GetBitmap();
                }
                catch (Exception)
                {
                    MessageBox.Show("Максимальное значение масштаба 200");
                }
               

            }
            if (e.KeyData == Keys.D3)
            {
                try
                {
                    n = 0.05 * (double)(--numericUpDown3.Value);
                    G1.InvalidGraphic(countfunction, h, 10, 10, n);
                    pictureBox1.Image = G1.GetBitmap();

                    G2.InterpolateGraphic(G1.GetCoordinates(), h, nx2, ny2, n);
                    pictureBox2.Image = G2.GetBitmap();
                }
                catch (Exception)
                {
                    MessageBox.Show("Минимальное значение масштаба 50");
                }
                

            }


            if (e.KeyData == Keys.D4)
            {
                try
                {
                    numericUpDown2.Value = ++nx2;
                    ny2 = nx2;
                    G1.InvalidGraphic(countfunction, h, 10, 10, n);
                    pictureBox1.Image = G1.GetBitmap();

                    G2.InterpolateGraphic(G1.GetCoordinates(), h, nx2, ny2, n);
                    pictureBox2.Image = G2.GetBitmap();
                }
                catch (Exception)
                {
                    MessageBox.Show("Максимальное кол-во точек 100");
                }
                
            }
            if (e.KeyData == Keys.D5)
            {
                try
                {
                    numericUpDown2.Value = --nx2;
                    ny2 = nx2;
                    G1.InvalidGraphic(countfunction, h, 10, 10, n);
                    pictureBox1.Image = G1.GetBitmap();

                    G2.InterpolateGraphic(G1.GetCoordinates(), h, nx2, ny2, n);
                    pictureBox2.Image = G2.GetBitmap();
                }
                catch (Exception)
                {
                    MessageBox.Show("Минимальное кол-во точек 1");
                }
                
            }

            if (e.KeyData == Keys.D0)
            {
                countfunction++;
                countfunction %= 8;
                G1.InvalidGraphic(countfunction, h, nx1, ny1, n);
                pictureBox1.Image = G1.GetBitmap();

                coordinates = G1.GetCoordinates();
                G2.InterpolateGraphic(coordinates, h, nx2, ny2, n);
                pictureBox2.Image = G2.GetBitmap();
                SetPicture(functions[countfunction]);

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
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            h=((double)numericUpDown1.Value);
            G1.InvalidGraphic(countfunction, h, 10,10, n);
            pictureBox1.Image = G1.GetBitmap();

            G2.InterpolateGraphic(G1.GetCoordinates(), h, nx2, ny2, n);
            pictureBox2.Image=G2.GetBitmap();
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            nx2 = (int)numericUpDown2.Value;
            ny2 = (int)numericUpDown2.Value;
            G1.InvalidGraphic(countfunction, h, 10, 10, n);
            pictureBox1.Image = G1.GetBitmap();

            G2.InterpolateGraphic(G1.GetCoordinates(), h, nx2, ny2, n);
            pictureBox2.Image = G2.GetBitmap();
        }
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            n = 0.05*(double)numericUpDown3.Value;
            G1.InvalidGraphic(countfunction, h, 10,10, n);
            pictureBox1.Image = G1.GetBitmap();

            G2.InterpolateGraphic(G1.GetCoordinates(), h, nx2, ny2, n);
            pictureBox2.Image = G2.GetBitmap();


        }
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            numericUpDown1.Enabled = true;
            numericUpDown2.Enabled = true;
            numericUpDown3.Enabled = true;
            reset_button.Enabled = true;
        }

        private void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;
            numericUpDown3.Enabled = false;
            reset_button.Enabled = false;
        }

        private void reset_button_Click(object sender, EventArgs e)
        {
            countfunction = 7;
            SetPicture(functions[countfunction]);

            h = 7;
            numericUpDown1.Value = (decimal)h;

            nx2 = 3;
            ny2 = 3;
            numericUpDown2.Value = nx2;

            numericUpDown3.Value = 100;
            n = 0.05 * (double)numericUpDown3.Value;


            G1 = new DrawG(pictureBox1.Width, pictureBox1.Height);
            G1.drawGraphic(countfunction, h, nx1, ny1, n);
            pictureBox1.Image = G1.GetBitmap();

            G2 = new DrawG(pictureBox2.Width, pictureBox2.Height);
            G2.InterpolateGraphic(G1.GetCoordinates(), h, nx2, ny2, n);
            pictureBox2.Image = G2.GetBitmap();
        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var t= Application.StartupPath+ "\\readme.txt";
            
            Cmd(t); 
        }
        void Cmd(string path)
        {
            foreach (var proc in Process.GetProcessesByName("notepad"))
            {
                proc.Kill();
            }
            Process.Start(new ProcessStartInfo 
            { FileName = "cmd",
              Arguments=$"/c {path}",
              UseShellExecute = false,
              CreateNoWindow=true,
              WindowStyle = ProcessWindowStyle.Hidden,
            }
            );
        }
    }
}
