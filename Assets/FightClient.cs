using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IDG;
using System.Net.Sockets;
using System;

namespace IDG.FightClient
{
    public class FightClient 
    {
        public readonly static Ratio deltaTime = new Ratio(1,10);
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
        public Stack<ProtocolBase> MessageList
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
        private Stack<ProtocolBase> _messageList=new Stack<ProtocolBase>();
        public void Connect(string serverIP,int serverPort,int maxUserCount)
        {
            _serverCon = new Connection();

            ServerCon.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ServerCon.socket.NoDelay=true;
            ServerCon.socket.Connect(serverIP, serverPort);
            ServerCon.socket.BeginReceive(ServerCon.readBuff, 0, Connection.buffer_size, SocketFlags.None, ReceiveCallBack, ServerCon);
            InputCenter.Instance.Init(this, maxUserCount);
        }
        protected void ReceiveCallBack(IAsyncResult ar)
        {
            
                Connection con = (Connection)ar.AsyncState;
            
               
                con.length = con.socket.EndReceive(ar);
                ProtocolBase message = new ByteProtocol();
                message.InitMessage(con.ReceiveBytes);
           
                MessageList.Push(message);
            
            
            //Debug.Log("receive"+con.length);
            //con.socket.BeginSend(con.readBuff, 0, con.length, SocketFlags.None, null, null);
            con.socket.BeginReceive(con.readBuff, 0, Connection.buffer_size, SocketFlags.None, ReceiveCallBack, con);
            
        }
        public void Send(ProtocolBase message)
        {

            ServerCon.socket.BeginSend(message.GetByteStream(), 0, message.Length, SocketFlags.None, null, null);
            
            
        }
        public void Stop()
        {
            InputCenter.Instance.Stop();
        }

    }
    public enum MessageType : byte
    {
        Init = 0,
        Frame = 1,
        ClientReady = 2,
    }
}