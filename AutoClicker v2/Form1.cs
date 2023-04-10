using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AutoClickerv2
{
    public partial class Form1 : Form
    {
        Screen primaryScreen = Screen.PrimaryScreen;

        public void Payload()
        {
            if ((int)numericUpDown1.Value != 0)
            {
                stop = !stop;
                int interval = (int)numericUpDown1.Value;
                this.Invoke((MethodInvoker)delegate {
                    timer1.Interval = interval;
                    timer1.Enabled = true;
                    if (!stop)
                    {
                        panel1.BackColor = Color.Green;
                        timer1.Start();
                    }
                    else
                    {
                        timer1.Stop();
                        panel1.BackColor = Color.Red;
                    }
                });
            }
            else
            {
                MessageBox.Show("Value cannot be 0. Will be auto-set to 100 Intervals", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public Form1()
        {
            InitializeComponent();
            KeyPreview = true; // add this line to enable key preview
            int screenWidth = primaryScreen.Bounds.Width;
            int screenHeight = primaryScreen.Bounds.Height;
            Location = new Point(screenWidth-392, screenHeight-125);

        }
        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwdata, int dwextrainfo);

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Keys vKey);
        public enum mouseeventflags
        {
            LeftDown = 2,
            LeftUp = 4,
        }
        public void leftclick(Point p)
        {
            mouse_event((int)(mouseeventflags.LeftDown), p.X, p.Y, 0, 0);
            mouse_event((int)(mouseeventflags.LeftUp), p.X, p.Y, 0, 0);
            mouse_event((int)(mouseeventflags.LeftDown), p.X, p.Y, 0, 0);
            mouse_event((int)(mouseeventflags.LeftUp), p.X, p.Y, 0, 0);
            mouse_event((int)(mouseeventflags.LeftDown), p.X, p.Y, 0, 0);
            mouse_event((int)(mouseeventflags.LeftUp), p.X, p.Y, 0, 0);
        }
        bool stop = true;
        private void button1_Click(object sender, EventArgs e)
        {
            Payload();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            leftclick(new Point(MousePosition.X, MousePosition.Y)); ;
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            bool isF9KeyDown = false;
            while (true)
            {
                if (GetAsyncKeyState(Keys.F9) < 0)
                {
                    isF9KeyDown = true;
                }
                else if (isF9KeyDown)
                {
                    Payload();
                    isF9KeyDown = false;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }
    }
}