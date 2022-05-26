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
       
        public Form1()
        {
            InitializeComponent();
            Form2 form2 = new Form2();

            // Прямоугольник, в котором будет выведен график функции
            int n = 5;
            form2.left = 0;
            form2.top = 0;
            form2.width = 800;
            form2.height = 800;

            form2.f_show = false;
            form2.x0 = 0;
            form2.y0 = 0;
            form2.z0 = 0;
            form2.A = -8;
            form2.alfa = 9;
            form2.beta = 12;

            form2.X_min = -n;
            form2.X_max = n;
            form2.Y_min = -n;
            form2.Y_max = n;
            form2.ShowDialog();
            Close();
        }

    }
}
