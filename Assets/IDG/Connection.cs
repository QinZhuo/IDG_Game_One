using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
namespace IDG
{
    /// <summary>
    /// 连接数据类
    /// 保存Sokcet及其传输接收数据所需缓存
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// 连接ID
        /// </summary>
        public int clientId=-1;
        /// <summary>
        /// 读取buffer长度
        /// </summary>
        public readonly static int buffer_size = 1024;
        /// <summary>
        /// 读取buffer
        /// </summary>
        public byte[] readBuff = new byte[buffer_size];
        /// <summary>
        /// 数据长度（字节）
        /// </summary>
        public byte[] lenBytes = new byte[4];
        /// <summary>
        /// 数据长度（整形）
        /// </summary>
        public int msgLength = 0;
        /// <summary>
        /// 连接用套接字Socket
        /// </summary>
        public Socket socket;
        /// <summary>
        /// 已接受长度
        /// </summary>
        public int length=0;
        /// <summary>
        /// 缓存
        /// </summary>
        protected byte[] tempBuff;
        /// <summary>
        /// 剩余字节数
        /// </summary>
        public int BuffRemain { get { return buffer_size - length; } }
        /// <summary>
        /// 接收的字节
        /// </summary>
        public byte[] ReceiveBytes { get { tempBuff = new byte[msgLength]; Array.Copy(readBuff,4, tempBuff,0, msgLength); return tempBuff; } }
    }
}
