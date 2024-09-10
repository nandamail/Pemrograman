using System;
using System.Net;
using System.Threading;

namespace UriBuilderDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Menampilkan nama host lokal
            string localHostName = Dns.GetHostName();
            Console.WriteLine($"Nama Host Lokal: {localHostName}\n");

            // Daftar domain yang biasa Anda buka
            string[] domains = { "google.com", "youtube.com", "wikipedia.org" };

            foreach (var domain in domains)
            {
                try
                {
                    // Mendapatkan daftar alamat IP untuk domain
                    IPAddress[] addresses = Dns.GetHostAddresses(domain);
                    Console.WriteLine($"Daftar IP Address untuk domain: {domain}");
                    foreach (var ip in addresses)
                    {
                        Console.WriteLine($"IP Address: {ip}");
                    }
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Gagal mendapatkan alamat IP untuk domain {domain}: {ex.Message}");
                }

                // Tidur selama 2 detik sebelum lanjut ke domain berikutnya
                Thread.Sleep(2000);
            }
        }
    }
}