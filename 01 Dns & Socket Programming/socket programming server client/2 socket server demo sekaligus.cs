// Menambahkan parameter untuk menentukan port secara dinamis
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
        // Mengikat dan mendengarkan koneksi
        listener.Bind(localEndPoint);
        listener.Listen(10);

        while (true)
        {
            Console.WriteLine("Waiting connection ... ");
            Socket clientSocket = listener.Accept();

            // Proses komunikasi dengan client...
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.ToString());
    }
}

// Di Main Method, bisa dijalankan dengan port berbeda
static void Main(string[] args)
{
    // Menjalankan server pertama pada port 11111
    ExecuteServer(11111);

    // Jika ingin menjalankan server kedua, gunakan port yang berbeda
    // ExecuteServer(11112);
}