using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab4.Domain
{
    public class CallbackSession
    {
        public static int Port { get; set; } = 80;

        private static void StartClient(string hostName, int id)
        {
            var ipAddress = GetHostIpAddress(hostName);
            var remoteEndpoint = CreateEndPoint(ipAddress);

            var clientSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            var connection = new Connection
            {
                Socket = clientSocket,
                HostName = hostName.Split('/')[0],
                EndPoint = hostName.Contains("/") ? hostName.Substring(hostName.IndexOf("/", StringComparison.Ordinal)) : "/",
                IpEndPoint = remoteEndpoint,
                Id = id
            };

            connection.Socket.BeginConnect(connection.IpEndPoint, Connected, connection);
        }

        private static void Connected(IAsyncResult result)
        {
            var resultSocket = (Connection) result.AsyncState;
            var clientSocket = resultSocket.Socket;
            var clientId = resultSocket.Id;
            var hostname = resultSocket.HostName;

            clientSocket.EndConnect(result);
            Console.WriteLine($"Connection {clientId} > Socket connected to {hostname} ({clientSocket.RemoteEndPoint})");

            var byteData =
                Encoding.ASCII.GetBytes(
                    HttpProtocolParser.GetRequestString(resultSocket.HostName, resultSocket.EndPoint));

            resultSocket.Socket.BeginSend(byteData, 0, byteData.Length, 0, Sent, resultSocket);
        }

        private static void Sent(IAsyncResult result)
        {
            var resultSocket = (Connection) result.AsyncState;
            var clientSocket = resultSocket.Socket;
            var clientId = resultSocket.Id;

            // send data to server
            var bytesSent = clientSocket.EndSend(result);
            Console.WriteLine($"Connection {clientId} > Sent {bytesSent} bytes to server.");

            // server response (data)
            resultSocket.Socket.BeginReceive(resultSocket.Buffer, 0, Connection.BufferSize, 0, Receiving, resultSocket);
        }

        private static void Receiving(IAsyncResult ar)
        {
            // get answer details
            var resultSocket = (Connection) ar.AsyncState;
            var clientSocket = resultSocket.Socket;

            try
            {
                var bytesRead = clientSocket.EndReceive(ar); // read response data

                resultSocket.ResponseContent += Encoding.ASCII.GetString(resultSocket.Buffer, 0, bytesRead);

                // if the response header has not been fully obtained, get the next chunk of data
                if (!HttpProtocolParser.ResponseHeaderObtained(resultSocket.ResponseContent))
                {
                    clientSocket.BeginReceive(resultSocket.Buffer, 0, Connection.BufferSize, 0, Receiving,
                        resultSocket);
                }
                else
                {
                    var contentLength = HttpProtocolParser.GetContentLength(resultSocket.ResponseContent);
                    Console.WriteLine($"Content length is: {contentLength}");
                    var output = Encoding.Default.GetString(resultSocket.Buffer);
                    Console.WriteLine(output);
                    clientSocket.Shutdown(SocketShutdown.Both); // free socket
                    clientSocket.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static IPAddress GetHostIpAddress(string hostName) =>
            Dns.GetHostEntry(hostName.Split('/')[0]).AddressList[0];
        private static IPEndPoint CreateEndPoint(IPAddress ipAddress) => new(ipAddress, Port);

        public static void Run(List<string> hostNames)
        {
            var id = 1;

            hostNames.ForEach(hostName =>
            {
                StartClient(hostName, id);
                Thread.Sleep(1000);
                id++;
            });
            
        }
    }
}