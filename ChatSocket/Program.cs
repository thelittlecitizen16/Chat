using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatSocket
{
    class Program
    {
        //public static Hashtable clientsList = new Hashtable();
        public static ConcurrentDictionary<Guid, TcpClient> clientsList = new ConcurrentDictionary<Guid, TcpClient>();

        static void Main(string[] args)
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];

            TcpListener listener = new TcpListener(ipAddr, 8888);
            listener.Start();
            Console.WriteLine("wait for first connection");

            try
            {
                while (true)
                {
                    TcpClient tcpClient = listener.AcceptTcpClient();
                    clientsList.TryAdd(Guid.NewGuid(), tcpClient);
                    Console.WriteLine("new connection from client");

                    HandleClient client = new HandleClient();
                    client.StartClient(tcpClient, clientsList);
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            } 
        }
    }
}
