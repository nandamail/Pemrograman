using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SocketServerDemo
{
    [Serializable]
    public class CustomData
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int port = 11111;
            if (args.Length > 0)
            {
                int.TryParse(args[0], out port);
            }

            Console.WriteLine($"Server running on port: {port}");
            ExecuteServer(port);
        }

        public static void ExecuteServer(int port)
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, port);

            Socket listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                while (true)
                {
                    Console.WriteLine("Waiting connection ... ");
                    Socket clientSocket = listener.Accept();

                    byte[] bytes = new byte[1024];
                    int numByte = clientSocket.Receive(bytes);

                    // Deserialize the received byte array to an object
                    CustomData data = (CustomData)ByteArrayToObject(bytes);

                    Console.WriteLine($"Received Object: Id = {data.Id}, Name = {data.Name}");
                    byte[] message = Encoding.ASCII.GetBytes("Server received the object");

                    clientSocket.Send(message);
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static object ByteArrayToObject(byte[] arrBytes)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                ms.Write(arrBytes, 0, arrBytes.Length);
                ms.Seek(0, SeekOrigin.Begin);
                return bf.Deserialize(ms);
            }
        }
    }
}
