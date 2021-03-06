﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Linq;

namespace ChatSocket
{
    public class HandleClient
    {
        TcpClient clientSocket;
        ConcurrentDictionary<Guid, TcpClient> ClientsList;

        public void StartClient(TcpClient inClientSocket, ConcurrentDictionary<Guid, TcpClient> clientsList)
        {
            clientSocket = inClientSocket;
            ClientsList = clientsList;

            Thread ctThread = new Thread(DoChat);
            ctThread.Start();
        }

        private void DoChat()
        {
            while (true)
            {
                try
                {
                    NetworkStream nwStream = clientSocket.GetStream();
                    byte[] buffer = new byte[clientSocket.ReceiveBufferSize];

                    int bytesRead = nwStream.Read(buffer, 0, clientSocket.ReceiveBufferSize);

                    string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Received and Sending back: " + dataReceived);

                   //TODO: check if send client and remove ClientsList.Where(c=>c.Value != )
                    foreach (var client in ClientsList)
                    {
                        NetworkStream clientStream = client.Value.GetStream();
                        clientStream.Write(buffer, 0, bytesRead);
                    }
                }
                catch (SocketException)
                {
                    clientSocket.Close();
                }
                catch (ObjectDisposedException)
                {
                    clientSocket.Close();
                }
                catch (Exception)
                {
                    clientSocket.Close();
                }
            }
        }
    }
}
