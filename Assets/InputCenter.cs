using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Timers;
using UnityEngine;
namespace IDG.FightClient {

    public class InputCenter 
    {
        protected Timer timer;
        
        private static InputCenter instance;
        protected FightClient client;
        protected FrameKey sendKey;
        protected int _m_serverStep;
        protected int _m_clientStep;
        public int ClientStepIndex { get { return _m_clientStep %MaxFramBufferCount; } }
        public int ServerStepIndex { get { return _m_serverStep % MaxFramBufferCount; } }
        public readonly static int MaxFramBufferCount = 1000000;
        public InputUnit[] inputs;
        public static InputCenter Instance {
            get { if (instance == null) { instance = new InputCenter(); }
                return instance;
            }
           
        }
        public void ReceiveStep(ProtocolBase protocol)
        {
            int length = protocol.getByte();
            //int clientId= protocol.getByte();
            
            _m_serverStep++;
            Debug.Log("当前帧：[" + _m_serverStep + "]");
            for (int i = 0; i < length; i++)
            {
                inputs[i].ReceiveStep(protocol.getByte());
                
            }
            
            for (; _m_clientStep < _m_serverStep; _m_clientStep++)
            {
                for (int i = 0; i < length; i++)
                {
                    inputs[i].NextFrame();
                }
                
            }
            //Debug.Log("当前帧：[" + _m_clientStep + "]");
        }
        public void Init(FightClient client,int maxClient)
        {
            this.client = client;
            timer = new Timer(100);
            timer.AutoReset = true;
            timer.Elapsed += SendClientFrame;
            timer.Enabled = true;
            _m_serverStep = 0;
            _m_clientStep = 0;
            inputs = new InputUnit[maxClient];
            for (int i = 0; i < maxClient; i++)
            {
                inputs[i] = new InputUnit();
                inputs[i].Init();
            }
        }
        public void SetKey(FrameKey key)
        {
            sendKey|= key;
        }
        protected void SendClientFrame(object sender, ElapsedEventArgs e)
        {
            if (client.ServerCon.clientId < 0) return;
            ProtocolBase protocol = new ByteProtocol();
            protocol.push((byte)MessageType.Frame);
            protocol.push((byte)client.ServerCon.clientId);
            protocol.push((byte)sendKey);
            client.Send(protocol.GetByteStream());
            
            sendKey = 0;
        }
        public void Stop()
        {
            timer.Stop();
        }
        
    }
    public class InputUnit
    {

        
        private FrameKey[] keyList;
        
        public void Init()
        {
            keyList = new FrameKey[InputCenter.MaxFramBufferCount];
           
        }
        public void ReceiveStep(byte value)
        {
            keyList[InputCenter.Instance.ServerStepIndex] = (FrameKey)value;
        }
        public bool GetKey(FrameKey key)
        {
            return (keyList[InputCenter.Instance.ClientStepIndex] & key) == key;
        }

        private void InitFrame()
        {
            keyList[InputCenter.Instance.ClientStepIndex] = 0;
        }
        public void NextFrame()
        {
            if (framUpdate != null) framUpdate();
            InitFrame();
        }
        public event FrameUpdate framUpdate;
    }
    public delegate void FrameUpdate();
}
