using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ClientChat
{
    class Program
    {
        public static TcpClient client;
        static void Main(string[] args)
        {
            try
            {
                IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddr = ipHost.AddressList[0];

                client = new TcpClient();
                try
                {
                    client.Connect(ipAddr, 8888);

                    bool endConnection = false;
                    Thread ctThread = new Thread(getMessage);
                    ctThread.Start();

                    while (!endConnection)
                    {
                        

                        Console.WriteLine("enter message to server, if you wand to end connection enter: 0");
                        string message = Console.ReadLine();

                        if (message == "0")
                        {
                            endConnection = true;
                        }
                        else
                        {
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(message);

                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }

                    }
                }
                catch (ArgumentNullException ane)
                {

                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }

                catch (SocketException se)
                {

                    Console.WriteLine("SocketException : {0}", se.ToString());
                }

                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }

            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }
        }
        private static void getMessage()
        {
            while (true)
            {
                NetworkStream serverStream = client.GetStream();
                byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                int bytesRead = serverStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                string returndata = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
                if (returndata!="")
                {
                    Console.WriteLine("client gets data: " + returndata);
                    returndata = "";
                }   
            }
        }
    }
}
