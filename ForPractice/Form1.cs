using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ForPractice
{
    public partial class Form1 : Form
    {
        public DrawG G;
        public Form1()
        {
            InitializeComponent();
            G = new DrawG(pictureBox1.Width, pictureBox1.Height);
            G.drawGraphic();
            pictureBox1.Image = G.GetBitmap();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down)
            {
                G.beta = G.beta - 0.1;
                G.InvalidGraphic();
                pictureBox1.Image = G.GetBitmap();
            }
            if (e.KeyData == Keys.Up)
            {
                G.beta = G.beta + 0.1;
                G.InvalidGraphic();
                pictureBox1.Image = G.GetBitmap();
            }
            if (e.KeyData == Keys.Right)
            {
                G.alfa = G.alfa + 0.1;
                G.InvalidGraphic();
                pictureBox1.Image = G.GetBitmap();
            }
            if (e.KeyData == Keys.Left)
            {
                G.alfa = G.alfa - 0.1;
                G.InvalidGraphic();
                pictureBox1.Image = G.GetBitmap();
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
    }
}
