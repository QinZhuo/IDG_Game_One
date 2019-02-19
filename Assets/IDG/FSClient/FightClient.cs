using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IDG;
using System.Net.Sockets;
using System;
using System.Linq;
namespace IDG.FSClient
{
    /// <summary>
    /// 【帧同步客户端】负责与【帧同步服务器】连接
    /// </summary>
    public class FSClient 
    {
        /// <summary>
        /// 帧同步时间间隔
        /// </summary>
        public readonly static FixedNumber deltaTime = new FixedNumber(0.1f);
        
        /// <summary>
        /// 服务器连接
        /// </summary>
        public Connection ServerCon
        {
            get
            {
                lock (_serverCon)
                {
                    return _serverCon;
                }
            }
        }
        /// <summary>
        /// 消息队列
        /// </summary>
        public Queue<ProtocolBase> MessageList
        {
            get
            {
                lock (_messageList)
                {
                    return _messageList;
                } 
            }
        }
        private Connection _serverCon;

        public InputCenter inputCenter;
        public NetObjectManager objectManager;

        public NetData localPlayer;
        public ShapPhysics physics;
        public object unityClient;
        private Queue<ProtocolBase> _messageList=new Queue<ProtocolBase>();
        /// <summary>
        /// 连接服务器函数
        /// </summary>
        /// <param name="serverIP">服务器IP地址</param>
        /// <param name="serverPort">服务器端口</param>
        /// <param name="maxUserCount">最大玩家数</param>
        public void Connect(string serverIP,int serverPort,int maxUserCount)
        {
            _serverCon = new Connection();
            ServerCon.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ServerCon.socket.NoDelay=true;
            ServerCon.socket.Connect(serverIP, serverPort);
            ServerCon.socket.BeginReceive(ServerCon.readBuff,0, ServerCon.BuffRemain, SocketFlags.None, ReceiveCallBack, ServerCon);
            inputCenter=new InputCenter();
            inputCenter.Init(this, maxUserCount);
            objectManager=new NetObjectManager(this);
            physics =new ShapPhysics();
            physics.Init();
        }
        Dictionary<Connection, byte[]> lastBytesList = new Dictionary<Connection, byte[]>();
        /// <summary>
        /// 数据接受回调函数
        /// </summary>
        protected void ReceiveCallBack(IAsyncResult ar)
        {
            
                Connection con = (Connection)ar.AsyncState;
            
               
                int length= con.socket.EndReceive(ar);
            con.length += length;
            Debug.Log("receive: "+length);
            //  Debug.Log(DateTime.Now.ToString()+":"+DateTime.Now.Millisecond+ "receive:" + length);
            ProcessData(con);
            con.socket.BeginReceive(con.readBuff, con.length, con.BuffRemain, SocketFlags.None, ReceiveCallBack, con);
            
        }
        /// <summary>
        /// 解析字节数据
        /// </summary>
        /// <param name="connection">要解析数据的连接</param>
        private void ProcessData(Connection connection)
        {

            if (connection.length < sizeof(Int32))
            {
                Debug.Log("获取不到信息大小重新接包解析：" + connection.length.ToString());
                return;
            }
            Array.Copy(connection.readBuff, connection.lenBytes, sizeof(Int32));
            connection.msgLength = BitConverter.ToInt32(connection.lenBytes, 0);
           
            if (connection.length < connection.msgLength + sizeof(Int32))
            {
                Debug.Log("信息大小不匹配重新接包解析：" + connection.length + ":" +(connection.msgLength+4).ToString());
                return;
            }
            //ServerDebug.Log("接收信息大小：" + connection.msgLength.ToString(), 1);
            // string str = Encoding.UTF8.GetString(connection.readBuff, sizeof(Int32), connection.length);
            
            ProtocolBase message = new ByteProtocol();
           // Debug.Log(DateTime.Now.ToString() + ":" + DateTime.Now.Millisecond+"接收消息大小:" + connection.msgLength);
            message.InitMessage(connection.ReceiveBytes);
            MessageList.Enqueue(message);
        //    Debug.Log("ProcessDataOver");
            //Send(connection, str);
            int count = connection.length - connection.msgLength - sizeof(Int32);
            Array.Copy(connection.readBuff, sizeof(Int32) + connection.msgLength, connection.readBuff, 0, count);
            connection.length = count;
            if (connection.length > 0)
            {
                ProcessData(connection);
            }
        }
        /// <summary>
        /// 发送字节
        /// </summary>
        /// <param name="bytes">发送内容</param>
        public void Send(byte[] bytes)
        {

            byte[] length = BitConverter.GetBytes(bytes.Length);
            byte[] temp = length.Concat(bytes).ToArray();
         //   Debug.Log("send" + temp.Length);
            ServerCon.socket.BeginSend(temp, 0, temp.Length, SocketFlags.None, null, null);
        }
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            inputCenter.Stop();
        }
        /// <summary>
        /// 解析消息并进行消息分发
        /// </summary>
        /// <param name="protocol">要解析的消息</param>
        public void ParseMessage(ProtocolBase protocol)
        {
            byte t = protocol.getByte();
  //          Debug.Log("parseMessage" + t);
            //Debug.Log(DateTime.Now.ToString() + ":" + DateTime.Now.Millisecond+"MessageType: " + (MessageType)t);
            switch ((MessageType)t)
            {

                case MessageType.Init:
                    ServerCon.clientId = protocol.getByte();
                
                    Debug.Log("clientID:" + ServerCon.clientId);
                    break;
                case MessageType.Frame:
                    inputCenter.ReceiveStep(protocol);
                    break;
                default:
                    Debug.LogError("消息解析错误 未解析类型" + t);
                    return;
            }
            if (protocol.Length > 0)
            {
                Debug.Log("剩余未解析" + protocol.Length);
                //ParseMessage(protocol);
            }
        }
    }
    public enum MessageType : byte
    {
        Init = 0,
        Frame = 1,
        ClientReady = 2,
    }
}