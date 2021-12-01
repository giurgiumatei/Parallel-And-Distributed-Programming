using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Lab4.Domain;

namespace lab4.Domain
{
    public class AsyncSession
    {
        public static int Port { get; set; } = 80;
        public static List<string> Hosts { get; set; }



        private static void Start(object idObject)
        {
            var id = (int)idObject;

            StartClient(Hosts[id], id);
        }

        private static async void StartClient(string hostName, int id)
        {
            var ipAddress = GetHostIpAddress(hostName);
            var remoteEndpoint = CreateEndPoint(ipAddress);

            var client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            var connection = new Connection
            {
                Socket = client,
                HostName = hostName.Split('/')[0],
                EndPoint = hostName.Contains("/") ? hostName.Substring(hostName.IndexOf("/", StringComparison.Ordinal)) : "/",
                IpEndPoint = remoteEndpoint,
                Id = id
            };

            await Connect(connection);

            await Send(connection,
                HttpProtocolParser.GetRequestString(connection.HostName, connection.EndPoint)); // request data from the server

            await Receive(connection);

            Console.WriteLine("Connection {0} > Content length is:{1}", connection.Id, HttpProtocolParser.GetContentLength(connection.ResponseContent.ToString()));

            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

        private static async Task Connect(Connection state)
        {
            state.Socket.BeginConnect(state.IpEndPoint, ConnectCallback, state);

            await Task.FromResult<object>(state.ConnectDone.WaitOne());
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            var resultSocket = (Connection)ar.AsyncState;
            var clientSocket = resultSocket.Socket;
            var clientId = resultSocket.Id;
            var hostname = resultSocket.HostName;

            clientSocket.EndConnect(ar);

            Console.WriteLine("Connection {0} > Socket connected to {1} ({2})", clientId, hostname, clientSocket.RemoteEndPoint);

            resultSocket.ConnectDone.Set();
        }

        private static async Task Send(Connection state, string data)
        {
            var byteData = Encoding.ASCII.GetBytes(data);

            state.Socket.BeginSend(byteData, 0, byteData.Length, 0, SendCallback, state);

            await Task.FromResult<object>(state.SendDone.WaitOne());
        }

        private static void SendCallback(IAsyncResult result)
        {
            var resultSocket = (Connection)result.AsyncState;
            var clientSocket = resultSocket.Socket;
            var clientId = resultSocket.Id;

            var bytesSent = clientSocket.EndSend(result); // complete sending the data to the server  

            Console.WriteLine("Connection {0} > Sent {1} bytes to server.", clientId, bytesSent);

            resultSocket.SendDone.Set(); // signal that all bytes have been sent
        }

        private static async Task Receive(Connection connection)
        {
            // receive data
            connection.Socket.BeginReceive(connection.Buffer, 0, Connection.BufferSize, 0, ReceiveCallback, connection);

            await Task.FromResult<object>(connection.ReceiveDone.WaitOne());
        }

        private static void ReceiveCallback(IAsyncResult result)
        {
            // retrieve the details from the connection information wrapper
            var resultSocket = (Connection)result.AsyncState;
            var clientSocket = resultSocket.Socket;

            try
            {
                // read data from the remote device.  
                var bytesRead = clientSocket.EndReceive(result);

                // get from the buffer, a number of characters <= to the buffer size, and store it in the responseContent
                resultSocket.ResponseContent += Encoding.ASCII.GetString(resultSocket.Buffer, 0, bytesRead);

                // if the response header has not been fully obtained, get the next chunk of data
                if (!HttpProtocolParser.ResponseHeaderObtained(resultSocket.ResponseContent))
                {
                    clientSocket.BeginReceive(resultSocket.Buffer, 0, Connection.BufferSize, 0, ReceiveCallback, resultSocket);
                }
                else
                {
                    string output = Encoding.Default.GetString(resultSocket.Buffer);
                    Console.WriteLine(output);
                    resultSocket.ReceiveDone.Set(); // signal that all bytes have been received       
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

        public static void Run(List<string> hostnames)
        {
            var id = 0;
            Hosts = hostnames;
            var tasks = new List<Task>();

            hostnames.ForEach(_ =>
            {
                tasks.Add(Task.Factory.StartNew(Start, id));
                id++;
            });

            Task.WaitAll(tasks.ToArray());
        }
    }
}