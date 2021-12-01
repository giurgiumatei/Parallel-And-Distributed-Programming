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
    public class Connection
    {
        public int Id { get; set; }
        public string HostName { get; set; }
        public string EndPoint { get; set; }
        public IPEndPoint IpEndPoint { get; set; }
        public ManualResetEvent ConnectDone { get; set; } = new (false);
        public ManualResetEvent SendDone { get; set; } = new (false);
        public ManualResetEvent ReceiveDone { get; set; } = new (false);
        public Socket Socket { get; set; }
        public byte[] Buffer { get; set; } = new byte[BufferSize];
        public static int BufferSize { get; set; } = 1024 * 10;
        public string ResponseContent { get; set; }


    }

}
