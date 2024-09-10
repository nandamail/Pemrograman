using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovingObject
{
    public partial class Form2 : Form
    {

        Pen red = new Pen(Color.Red);
        Rectangle rect = new Rectangle(20, 20, 30, 30);
        SolidBrush fillBlue = new SolidBrush(Color.Blue);
        int slide = 10; 

        public Form2()
        {
            InitializeComponent();
            timer1.Interval = 50;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            back();

            rect.X += slide;
            Invalidate();
        }

        private void back()
        {
            if (rect.X >= this.Width - rect.Width * 2)
                slide = -10;
            else
            if (rect.X <= rect.Width / 2)
                slide = 10;
        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawRectangle(red, rect);
            g.FillRectangle(fillBlue, rect);

            // Drawing the text "client" in the center of the form
            string text = "CLIENT 1";
            Font font = new Font("Arial", 16, FontStyle.Bold);
            SolidBrush textBrush = new SolidBrush(Color.Black);

            // Calculate the position to center the text
            SizeF textSize = g.MeasureString(text, font);
            float x = (this.ClientSize.Width - textSize.Width) / 2;
            float y = (this.ClientSize.Height - textSize.Height) / 2;

            g.DrawString(text, font, textBrush, x, y);
        }
    }
}
