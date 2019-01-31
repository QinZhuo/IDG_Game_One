﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Timers;
using UnityEngine;
namespace IDG.FSClient {
    /// <summary>
    /// 帧同步逻辑帧键盘输入处理中心
    /// </summary>
    public class InputCenter 
    {
        /// <summary>
        /// 计时器
        /// </summary>
        protected Timer timer;
        /// <summary>
        /// 单例
        /// </summary>
        private static InputCenter instance;
        /// <summary>
        /// 当前游戏时间 帧*帧间隔
        /// </summary>
        protected FixedNumber _time;
        /// <summary>
        /// 帧同步客户端
        /// </summary>
        protected FSClient client;
        /// <summary>
        /// 发送的按键信息
        /// </summary>
        protected FrameKey sendKey;
      //  protected Fixed2[] sendFixed2;
        /// <summary>
        /// 每帧调用函数
        /// </summary>
        public Action frameUpdate;
        public static FixedNumber Time
        {
            get { return instance. _time; }
        }
        /// <summary>
        /// 发送的摇杆操作信息
        /// </summary>
        protected Dictionary<KeyNum,JoyStickKey> joySticks;
        /// <summary>
        /// 服务器当前帧 进程
        /// </summary>
        protected int _m_serverStep;
        /// <summary>
        /// 本地客户端当前 进程
        /// </summary>
        protected int _m_clientStep;
        /// <summary>
        /// 多个客户端输入分配
        /// </summary>
        protected InputUnit[] _m_inputs;

        public static bool IsLocalId(int id)
        {
            return id == Instance.client.ServerCon.clientId;
        }
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
        /// <summary>
        /// 接收帧消息 并解析消息
        /// </summary>
        /// <param name="protocol">服务器发过来的帧信息</param>
        public void ReceiveStep(ProtocolBase protocol)
        {
            //获取玩家客户端个数
            int length = protocol.getByte();
            
            
            _m_serverStep++;
            Debug.Log("分布解析:" + protocol.Length);
            for (int i = 0; i < length; i++)
            {
                //解析各个玩家输入
                _m_inputs[i].ReceiveStep(protocol);
                
            }
           
           
            //执行帧调用函数
            for (; _m_clientStep < _m_serverStep; _m_clientStep++)
            {
                if (frameUpdate != null)
                {
                   
                    frameUpdate();
                    Tree4.CheckTree();
                }
                this._time += FSClient.deltaTime;
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="client">帧同步客户端对象</param>
        /// <param name="maxClient">最大客户端数</param>
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

            sendKey=new FrameKey();
            for (int i = 0; i < maxClient+1; i++)
            {
                _m_inputs[i] = new InputUnit();
                _m_inputs[i].Init();
            }
        }
        /// <summary>
        /// 设置按键操作
        /// </summary>
        /// <param name="down">是否按下</param>
        /// <param name="mask">当前按键</param>
        public void SetKey(bool down,KeyNum mask)
        {
           sendKey.SetKey(down,mask);
        }
        /// <summary>
        /// 设置按键操作
        /// </summary>
        /// <param name="mask">当前按键</param>
        /// <param name="joy">是否按下</param>
        public void SetJoyStick(KeyNum mask,JoyStickKey joy){
            sendKey.SetKey(joy.key,mask);
            if(joySticks.ContainsKey(mask)){
                joySticks[mask]=joy;
            }else
            {
                joySticks.Add(mask,joy);
            }
        }

        /// <summary>
        /// 发送本地客户端帧信息
        /// </summary>
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
               // Debug.LogError("len"+joySticks.Count);
            foreach (var joy in joySticks)
            {
            //  Debug.LogError("key"+joy.Key);
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
    /// <summary>
    /// 客户端输入解析类
    /// </summary>
    public class InputUnit
    {

        /// <summary>
        /// 帧按键信息
        /// </summary>
        private FrameKey frameKey;
        /// <summary>
        /// 帧摇杆信息
        /// </summary>
        protected Dictionary<KeyNum,JoyStickKey> joySticks;
        public void Init()
        {
            frameKey =new FrameKey();
            joySticks = new Dictionary<KeyNum, JoyStickKey>();
        }
        /// <summary>
        /// 解析帧信息
        /// </summary>
        /// <param name="message">消息</param>
        public void ReceiveStep(ProtocolBase message)
        {
            
            frameKey.Parse(message);
            //Debug.Log(keyList[InputCenter.Instance.ServerStepIndex]);
            byte len=message.getByte();

        
            for(byte i=0;i<len;i++){
               
                JoyStickKey joy=new JoyStickKey((KeyNum)(message.getByte()),message.getV2()) ;
                
                if(joySticks.ContainsKey(joy.key)){
                    
                    joySticks[joy.key]=joy;
                }else
                {
                    joySticks.Add(joy.key,joy);
                }
            }

        }
        /// <summary>
        /// 检测按键当前帧是否处于按下按下状态
        /// </summary>
        /// <param name="key">按键</param>
        public bool GetKey(KeyNum key)
        {
            return frameKey.GetKey(key);
        }
        /// <summary>
        /// 检测按键当前帧是否有按下操作
        /// </summary>
        /// <param name="key">按键</param>
        public bool GetKeyDown(KeyNum key)
        {
            return frameKey.GetKeyDown(key);
        }
        /// <summary>
        /// 检测按键当前帧是否有抬起操作
        /// </summary>
        /// <param name="key">按键</param>
        public bool GetKeyUp(KeyNum key)
        {
            return frameKey.GetKeyUp(key);
        }
        /// <summary>
        /// 获取按键对应的摇杆方向
        /// </summary>
        /// <param name="key">摇杆对应按键</param>
        /// <returns>摇杆方向</returns>
        public Fixed2 GetJoyStickDirection(KeyNum key)
        {
            if(joySticks.ContainsKey(key)){
                 return joySticks[key].direction;
            }else
            {
                
                string test="|||";
                foreach (var joy in joySticks)
                {
                    test+="["+joy.Key+"]";
                }
                Debug.LogError("未同步的摇杆！！！["+key+"]"+test);
                
                return Fixed2.zero;
            }
           
            
        }
        /// <summary>
        /// 帧调用函数
        /// </summary>
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

}
