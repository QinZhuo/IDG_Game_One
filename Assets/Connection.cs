using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
namespace IDG
{
    public class Connection
    {
        public int clientId=-1;
        public readonly static int buffer_size = 1024;
        public byte[] readBuff = new byte[1024];
        public Socket socket;
        public int length;
        protected byte[] tempBuff;
        public byte[] ReceiveBytes { get { tempBuff = new byte[length]; Array.Copy(readBuff, tempBuff, length); return tempBuff; } }
    }
}
