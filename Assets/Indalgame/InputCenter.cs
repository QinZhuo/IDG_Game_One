﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Timers;
using UnityEngine;
namespace IDG.FSClient {

    public class InputCenter 
    {
        protected Timer timer;
        
        private static InputCenter instance;
        protected FixedNumber _time;
        protected FSClient client;
        protected FrameKey sendKey;
      //  protected Fixed2[] sendFixed2;

        public Action frameUpdate;
        public static FixedNumber Time
        {
            get { return instance. _time; }
        }
        // public void ResetKey()
        // {
        //     sendKey.Reset();   
        // }
       
        //protected List<Func<FrameKey>> frameKeys;
        protected Dictionary<KeyNum,JoyStickKey> joySticks;
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
                
                _m_inputs[i].ReceiveStep(protocol);
                
            }
           
           
            //   Debug.Log("当前帧：[" + _m_clientStep + "]" );
            for (; _m_clientStep < _m_serverStep; _m_clientStep++)
            {
                if (frameUpdate != null)
                {
                   
                    frameUpdate();
                    Tree4.CheckTree();
                }
                this._time += FSClient.deltaTime;
                

            }
          
            //Debug.Log("当前帧：[" + _m_clientStep + "]");
        }
      
        public void Init(FSClient client,int maxClient)
        {
            this.client = client;
            timer = new Timer(FSClient.deltaTime.ToFloat()*1000);
            timer.AutoReset = true;
            timer.Elapsed += SendClientFrame;
            timer.Enabled = true;
            _m_serverStep = 0;
            _m_clientStep = 0;
            _m_inputs = new InputUnit[maxClient+1];
        
            joySticks = new Dictionary<KeyNum,JoyStickKey>();
           // sendFixed2 =null;
            sendKey=new FrameKey();
            for (int i = 0; i < maxClient+1; i++)
            {
                _m_inputs[i] = new InputUnit();
                _m_inputs[i].Init();
            }
        }
        public void SetKey(bool down,KeyNum mask)
        {
           sendKey.SetKey(down,mask);
        }
        public void SetJoyStick(KeyNum mask,JoyStickKey joy){
            sendKey.SetKey(joy.key,mask);
            if(joySticks.ContainsKey(mask)){
                joySticks[mask]=joy;
            }else
            {
                joySticks.Add(mask,joy);
            }
        }
        // public void AddKey(Func<FrameKey> func)
        // {
        //     frameKeys.Add(func);
        // }
        // public void AddJoyStick(FrameKey key, JoyStickKey func)
        // {
        //     JoyIndex.Add(key, joySticks.Count);
        //     joySticks.Add(func);
        //     sendFixed2 = new Fixed2[joySticks.Count];
        // }
        protected void SendClientFrame(object sender, ElapsedEventArgs e)
        {
            if (client.ServerCon.clientId < 0) return;
            ProtocolBase protocol = new ByteProtocol();
            protocol.push((byte)MessageType.Frame);
            protocol.push((byte)client.ServerCon.clientId);
           
            foreach (var bt in sendKey.GetBytes())
            {
                  protocol.push(bt);
            }
           
           
            
            protocol.push((byte)joySticks.Count);
                Debug.LogError("len"+joySticks.Count);
            foreach (var joy in joySticks)
            {
              Debug.LogError("key"+joy.Key);
                protocol.push((byte)joy.Key);
                protocol.push(joy.Value.direction);
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

        
        private FrameKey keyList;
        protected Dictionary<KeyNum,JoyStickKey> joySticks;
        public void Init()
        {
            keyList =new FrameKey();

            joySticks = new Dictionary<KeyNum, JoyStickKey>();


        }
        public void ReceiveStep(ProtocolBase message)
        {
            
            keyList.Parse(message);
            //Debug.Log(keyList[InputCenter.Instance.ServerStepIndex]);
            byte len=message.getByte();

        
            for(byte i=0;i<len;i++){
               
                JoyStickKey joy=new JoyStickKey((KeyNum)message.getByte(),message.getV2()) ;
                if(joySticks.ContainsKey(joy.key)){
                    joySticks[joy.key]=joy;
                }else
                {
                    joySticks.Add(joy.key,joy);
                }
            }

        }
        public bool GetKey(KeyNum key)
        {
            return keyList.GetKey(key);
        }
        public bool GetKeyDown(KeyNum key)
        {
            return keyList.GetKeyDown(key);
        }
        public bool GetKeyUp(KeyNum key)
        {
            return keyList.GetKeyUp(key);
        }
      
        public Fixed2 GetJoyStickDirection(KeyNum key)
        {
            if(joySticks.ContainsKey(key)){
                 return joySticks[key].direction;
            }else
            {
                Debug.LogError("未同步的摇杆！！！");
                return Fixed2.zero;
            }
           
            
        }
        public void InitFrame()
        {
            //keyList[InputCenter.Instance.ClientStepIndex].Reset();
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
