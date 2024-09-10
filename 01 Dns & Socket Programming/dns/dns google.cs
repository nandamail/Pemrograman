using System;
using System.Net;
using System.Threading;

namespace UrBuilderDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var domainEntry = Dns.GetHostEntry("google.com");
            Console.WriteLine(domainEntry.HostName);
            foreach (var ip in domainEntry.AddressList)
            {
                Console.WriteLine(ip);
            }
            Thread.Sleep(10000);
        }
    }
}