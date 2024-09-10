using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketServerDemo
{
    class Program
    {
        // Main Method
        static void Main(string[] args)
        {
            int port = 11111; // Default port
            if (args.Length > 0)
            {
                // Jika ada argumen yang diberikan, gunakan sebagai port
                int.TryParse(args[0], out port);
            }

            Console.WriteLine($"Server running on port: {port}");
            ExecuteServer(port);
        }

        public static void ExecuteServer(int port)
        {
            // Menentukan endpoint lokal
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, port);

            // Membuat dan mengonfigurasi socket
            Socket listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // Mengikat socket ke alamat IP dan port yang ditentukan
                listener.Bind(localEndPoint);

                // Mendengarkan koneksi masuk
                listener.Listen(10);

                while (true)
                {
                    Console.WriteLine("Waiting connection ... ");
                    Socket clientSocket = listener.Accept();

                    // Buffer untuk menyimpan data
                    byte[] bytes = new Byte[1024];
                    string data = null;

                    while (true)
                    {
                        int numByte = clientSocket.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, numByte);

                        if (data.IndexOf("<EOF>") > -1)
                            break;
                    }

                    Console.WriteLine("Text received -> {0} ", data);
                    byte[] message = Encoding.ASCII.GetBytes("Test Server");

                    // Mengirim pesan ke client
                    clientSocket.Send(message);

                    // Menutup koneksi dengan client
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
