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
        public readonly static int buffer_size = 1024*8;
        public byte[] readBuff = new byte[buffer_size];
        public byte[] lenBytes = new byte[4];
        public int msgLength = 0;
        public Socket socket;
        public int length=0;
        protected byte[] tempBuff;
        public int BuffRemain { get { return buffer_size - length; } }
        public byte[] ReceiveBytes { get { tempBuff = new byte[msgLength]; Array.Copy(readBuff,4, tempBuff,0, msgLength); return tempBuff; } }
    }
}
