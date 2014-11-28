using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaPS_Task2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Point[] Points = new Point[] { new Point(40, 250), 
                                       new Point(40, 50), 
                                       new Point(170, 50), 
                                       new Point(170, 250),
                                       new Point(40, 250)};
        CashMachine b = new CashMachine(10,10,10,10,10, 3000);
        Simulation sim;
        private void Form1_Load(object sender, EventArgs e)
        {
            sim = new Simulation(b, 500, 15000);
            sim.OnSimulation += sim_OnSimulation;


        }

        private void DrawPerson(Graphics g, int x, int y, int h, string s)
        {
            Pen pen = Pens.Black;
            int w = 4 * h / 15;
            //голова
            g.DrawEllipse(pen, x, y, w, w);
            //тело
            g.DrawLine(pen, x + w / 2, y + w, x + w / 2, y + 2 * w);
            //руки
            g.DrawLine(pen, x + w / 2, y + w, x, y + 2 * w);
            g.DrawLine(pen, x + w / 2, y + w, x + w, y + 2 * w);
            //ноги
            g.DrawLine(pen, x + w / 2, y + 2 * w, x, y + 3 * w);
            g.DrawLine(pen, x + w / 2, y + 2 * w, x + w, y + 3 * w);
            
            Font f = new Font("Arial", 7F);
            g.DrawString(s, f, Brushes.Black, x - 2, y - 10);
        }

        void sim_OnSimulation(object sender, SimulationEventArgs e)
        {
            Graphics s = this.CreateGraphics();
            Bitmap bmp = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(bmp);

            //рисование
            g.Clear(Color.White);
            g.DrawLines(Pens.Black, Points);
            string str = string.Format("5000:\t{0}\n1000:\t{1}\n500:\t{2}\n100:\t{3}\n50:\t{4}", 
                                        e.CashMachine.Money[5000], 
                                        e.CashMachine.Money[1000], 
                                        e.CashMachine.Money[500], 
                                        e.CashMachine.Money[100], 
                                        e.CashMachine.Money[50]);
            g.DrawString(str, new Font("Arial", 8F), Brushes.Black, Points[1].X + 5, Points[1].Y + 5);

            int x0 = 180, y0 = 150, h = 100;

            foreach(User user in e.Users)
            {
                DrawPerson(g, x0, y0, h, user.Amount.ToString());
                x0 += h;
            }

            s.DrawImage(bmp, 0, 0, Width, Height);
            s.Dispose();
            g.Dispose();
            bmp.Dispose();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //Graphics g = this.CreateGraphics();
            //g.DrawLines(Pens.Black, Points);
            Graphics s = this.CreateGraphics();
            Bitmap bmp = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(bmp);

            //рисование
            g.Clear(Color.White);
            g.DrawLines(Pens.Black, Points);
            string str = string.Format("5000:\t{0}\n1000:\t{1}\n500:\t{2}\n100:\t{3}\n50:\t{4}",
                                        b.Money[5000],
                                        b.Money[1000],
                                        b.Money[500],
                                        b.Money[100],
                                        b.Money[50]);
            g.DrawString(str, new Font("Arial", 8F), Brushes.Black, Points[1].X + 5, Points[1].Y + 5);

            s.DrawImage(bmp, 0, 0, Width, Height);
            s.Dispose();
            g.Dispose();
            bmp.Dispose();
        }

            
    }
}
