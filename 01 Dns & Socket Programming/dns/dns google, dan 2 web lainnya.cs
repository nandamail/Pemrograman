using System;
using System.Net;
using System.Threading;

namespace UriBuilderDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Daftar domain yang biasa penulis buka
            string[] domains = { "google.com", "quora.com", "facebook.com" };

            foreach (var domain in domains)
            {
                try
                {
                    var domainEntry = Dns.GetHostEntry(domain);
                    Console.WriteLine($"Informasi DNS untuk domain: {domain}");
                    Console.WriteLine($"Host Name: {domainEntry.HostName}");
                    foreach (var ip in domainEntry.AddressList)
                    {
                        Console.WriteLine($"IP Address: {ip}");
                    }
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Gagal mendapatkan informasi DNS untuk domain {domain}: {ex.Message}");
                }

                // Jeda 2 detik sebelum lanjut ke domain berikutnya
                Thread.Sleep(2000);
            }
        }
    }
}