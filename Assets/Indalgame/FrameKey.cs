using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace IDG{
    /// <summary>
    /// 按键值枚举
    /// </summary>
    public enum KeyNum : byte
    {
        Left = 2,//a 0b01
        Right = 4,//d 0b10
        Up = 8, //w  0b
        Down = 16,//s
        MoveKey=32,
        Attack=64,
    }
    /// <summary>
    /// 帧按键信息处理类
    /// </summary>
    public class FrameKey
    {
        /// <summary>
        /// 上一逻辑帧按键值
        /// </summary>
        KeyNum lastKey;
        /// <summary>
        /// 当前帧按键值
        /// </summary>
        KeyNum midKey;
        /// <summary>
        /// 记录当前帧是否有与midKey不同的值  如一帧内进行了点击与抬起
        /// </summary>
        KeyNum finalKey;
        public FrameKey(){
            lastKey=(KeyNum)0;
            midKey=(KeyNum)0;
            finalKey=(KeyNum)0;
        }
        /// <summary>
        /// 按键状态判断
        /// </summary>
        /// <param name="key">按键</param>
        public bool GetKey(KeyNum key){ 
          
            if((midKey&key)==key||(finalKey&key)==key){
                return true;
            }
            return false;
        }
        /// <summary>
        /// 按键按下操作判断
        /// </summary>
        /// <param name="key">按键</param>
        public bool GetKeyDown(KeyNum key){ 
            if(((lastKey&key)!=key&&(midKey&key)==key)||((midKey&key)!=key&&(finalKey&key)==key)){
                return true;  
            }
            return false;
        }
        /// <summary>
        /// 按键抬起操作判断
        /// </summary>
        /// <param name="key">按键</param>
        public bool GetKeyUp(KeyNum key){ 
            if(((lastKey&key)==key&&(midKey&key)!=key)||((midKey&key)==key&&(finalKey&key)!=key)){
                return true;  
            }
            return false;
        }
        /// <summary>
        /// 重置操作信息
        /// </summary>
        protected void Reset(){
            lastKey=finalKey;
            midKey=finalKey;
        }
        public void SetKey(bool down,KeyNum mask){
            KeyNum key=down?mask:0;
            SetKey(key,mask);
        }
        public void SetKey(KeyNum key,KeyNum mask){
            if((mask&lastKey)!=key){
                midKey=(midKey&~mask)|key;
                finalKey=midKey;
            }else if((mask&midKey)!=key){
                finalKey=(finalKey&~mask)|key;
            }
        }
        /// <summary>
        /// 转换为传输用byte信息
        /// </summary>
        /// <returns>byte信息</returns>
        public Byte[] GetBytes(){
            var r= new byte[]{(byte)midKey,(byte)finalKey};
            Reset();
            return r;
        } 
        /// <summary>
        /// 解析byte信息为按键数据
        /// </summary>
        /// <param name="message">消息</param>
        public void Parse(ProtocolBase message){
            Reset();
            midKey=(KeyNum) message.getByte();
            finalKey=(KeyNum) message.getByte();
        }
    }
    /// <summary>
    /// 摇杆信息
    /// </summary>
    public struct JoyStickKey
    {
        /// <summary>
        /// 摇杆对应按键或者摇杆当前状态
        /// </summary>
        public KeyNum key;
        /// <summary>
        /// 摇杆方向
        /// </summary>
        public Fixed2 direction;
        public JoyStickKey(KeyNum key, Fixed2 direction)
        {
            this.key = key;
            this.direction = direction;
        }
        
    }
}
