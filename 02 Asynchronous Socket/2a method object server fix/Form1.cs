using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovingObject
{
    public partial class Form1 : Form
    {
        private Pen red = new Pen(Color.Red);
        private Rectangle rect = new Rectangle(20, 20, 30, 30);
        private SolidBrush fillBlue = new SolidBrush(Color.Blue);
        private int slide = 10;

        private TcpListener listener;
        private List<TcpClient> clients = new List<TcpClient>();

        public Form1()
        {
            InitializeComponent();
            timer1.Interval = 50;
            timer1.Enabled = true;

            StartServer();
        }

        private void StartServer()
        {
            listener = new TcpListener(System.Net.IPAddress.Any, 5000);
            listener.Start();

            Task.Run(() =>
            {
                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    lock (clients)
                    {
                        clients.Add(client);
                    }
                }
            });
        }

        private void SendDataToClients()
        {
            string dataToSend = $"{rect.X},{rect.Y},{rect.Width},{rect.Height}";
            byte[] bytesToSend = Encoding.ASCII.GetBytes(dataToSend);

            lock (clients)
            {
                foreach (var client in clients)
                {
                    NetworkStream stream = client.GetStream();
                    stream.Write(bytesToSend, 0, bytesToSend.Length);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            back();

            rect.X += slide;
            Invalidate();

            SendDataToClients(); // Kirim data ke client
        }

        private void back()
        {
            if (rect.X >= this.Width - rect.Width * 2)
                slide = -10;
            else if (rect.X <= rect.Width / 2)
                slide = 10;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawRectangle(red, rect);
            g.FillRectangle(fillBlue, rect);
        }
    }
}
