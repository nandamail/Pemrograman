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
        private TcpClient client;
        private NetworkStream stream;

        public Form1()
        {
            InitializeComponent();

            if (!ConnectToServer())
            {
                MessageBox.Show("Unable to connect to server. The application will now exit.");
                Application.Exit(); // Keluar dari aplikasi jika koneksi gagal
            }

            timer1.Interval = 50;
            timer1.Enabled = true;
        }

        private bool ConnectToServer()
        {
            try
            {
                client = new TcpClient("127.0.0.2", 5000); // IP dan port server
                stream = client.GetStream();
                return true; // Koneksi berhasil
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to server: " + ex.Message);
                return false; // Koneksi gagal
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ReceiveData();
            Invalidate();
        }

        private void ReceiveData()
        {
            if (stream != null && stream.DataAvailable)
            {
                byte[] data = new byte[1024];
                int bytesRead = stream.Read(data, 0, data.Length);
                string receivedData = Encoding.ASCII.GetString(data, 0, bytesRead);

                // Parsing posisi rectangle dari string
                string[] parts = receivedData.Split(',');
                if (parts.Length == 4)
                {
                    int x = int.Parse(parts[0]);
                    int y = int.Parse(parts[1]);
                    int width = int.Parse(parts[2]);
                    int height = int.Parse(parts[3]);

                    rect = new Rectangle(x, y, width, height);
                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawRectangle(red, rect);
            g.FillRectangle(fillBlue, rect);
        }
    }
}
