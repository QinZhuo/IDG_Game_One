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
        protected Ratio _time;
        protected FightClient client;
        protected FrameKey sendKey;
        protected V2[] sendV2;
        protected Dictionary<FrameKey, int> JoyIndex;
        public Action frameUpdate;
        public static Ratio Time
        {
            get { return instance. _time; }
        }
        public void ResetKey()
        {
             FrameKey key=0;
             
             foreach (var keyFunc in frameKeys)
             { 
             key |= keyFunc();
             }
            for (int i = 0; i < joySticks.Count; i++)
            {
                sendV2[i] = joySticks[i].direction();
                key |= joySticks[i].frameKey();
                
            }
            sendKey = key;
        }
        public int JoyStickIndex(FrameKey frameKey)
        {
            return JoyIndex[frameKey];
        }
        protected List<Func<FrameKey>> frameKeys;
        protected List<JoyStickKey> joySticks;
        protected int _m_serverStep;
        protected int _m_clientStep;
        public int ClientStepIndex { get { return _m_clientStep %MaxFramBufferCount; } }
        public int ServerStepIndex { get { return _m_serverStep % MaxFramBufferCount; } }
        public readonly static int MaxFramBufferCount = 10;
        protected InputUnit[] _m_inputs;
        public InputUnit this[int index]
        {
            get
            {
                if (index < 0)
                {
                    return this._m_inputs[this._m_inputs.Length - 1];
                }
                else
                {
                    return _m_inputs[index];
                }
            }
        }
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
            Debug.Log("分布解析:" + protocol.Length);
            for (int i = 0; i < length; i++)
            {
                
                _m_inputs[i].ReceiveStep(protocol,joySticks.Count);
                
            }
           
           
            //   Debug.Log("当前帧：[" + _m_clientStep + "]" );
            for (; _m_clientStep < _m_serverStep; _m_clientStep++)
            {
                if (frameUpdate != null) frameUpdate();
                for (int i = 0; i < length; i++)
                {
                    _m_inputs[i].InitFrame();
                    this._time += FightClient.deltaTime;
                }

            }
          
            //Debug.Log("当前帧：[" + _m_clientStep + "]");
        }
      
        public void Init(FightClient client,int maxClient)
        {
            this.client = client;
            timer = new Timer(50);
            timer.AutoReset = true;
            timer.Elapsed += SendClientFrame;
            timer.Enabled = true;
            _m_serverStep = 0;
            _m_clientStep = 0;
            _m_inputs = new InputUnit[maxClient+1];
            frameKeys = new List<Func<FrameKey>>();
            JoyIndex = new Dictionary<FrameKey, int>();
            joySticks = new List<JoyStickKey>();
            sendV2 =null;
            for (int i = 0; i < maxClient+1; i++)
            {
                _m_inputs[i] = new InputUnit();
                _m_inputs[i].Init();
            }
        }
        //public void SetKey(FrameKey key)
        //{
        //    sendKey|= key;
        //}
        public void AddKey(Func<FrameKey> func)
        {
            frameKeys.Add(func);
        }
        public void AddJoyStick(FrameKey key, JoyStickKey func)
        {
            JoyIndex.Add(key, joySticks.Count);
            joySticks.Add(func);
            sendV2 = new V2[joySticks.Count];
        }
        protected void SendClientFrame(object sender, ElapsedEventArgs e)
        {
            if (client.ServerCon.clientId < 0) return;
            ProtocolBase protocol = new ByteProtocol();
            protocol.push((byte)MessageType.Frame);
            protocol.push((byte)client.ServerCon.clientId);
            protocol.push((byte)sendKey);
            if (sendV2!=null)
            {
                foreach (var direction in sendV2)
                {
                    protocol.push(direction);
                }
            }
            client.Send(protocol.GetByteStream());
        }
        public void Stop()
        {
            timer.Stop();
        }
        
    }
    public class InputUnit
    {

        
        private FrameKey[] keyList;
        V2[] directions;
        public void Init()
        {
            keyList = new FrameKey[InputCenter.MaxFramBufferCount];
            directions = new V2[0];


        }
        public void ReceiveStep(ProtocolBase message,int length)
        {
            keyList[InputCenter.Instance.ServerStepIndex] = (FrameKey)message.getByte();
            //Debug.Log(keyList[InputCenter.Instance.ServerStepIndex]);
            lock (directions)
            {
                directions = new V2[length];

                for (int n = 0; n < length; n++)
                {

                    directions[n] = message.getV2();


                }
            }

        }
        public bool GetKey(FrameKey key)
        {
            return (keyList[InputCenter.Instance.ClientStepIndex] & key) == key;
        }
        public bool GetJoyStick(FrameKey key)
        {
            return (keyList[InputCenter.Instance.ClientStepIndex] & key) == key;
        }
        public V2 GetJoyStickDirection(FrameKey key)
        {
            lock (directions)
            {
                return directions[InputCenter.Instance.JoyStickIndex(key)];
            }
        }
        public void InitFrame()
        {
            keyList[InputCenter.Instance.ClientStepIndex] = 0;
        }
       
        public Action framUpdate
        {
            set {
                InputCenter.Instance.frameUpdate=value;
            }
            get
            {
                return InputCenter.Instance.frameUpdate;
            }
        }
    }
    //public delegate void FrameUpdate();
}
