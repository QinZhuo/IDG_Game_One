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
    /// 帧按键处理类
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
        public bool GetKey(KeyNum key){ 
          
            if((midKey&key)==key||(finalKey&key)==key){
                return true;
            }
            return false;
        }
        public bool GetKeyDown(KeyNum key){ 
            if(((lastKey&key)!=key&&(midKey&key)==key)||((midKey&key)!=key&&(finalKey&key)==key)){
                return true;  
            }
            return false;
        }
        public bool GetKeyUp(KeyNum key){ 
            if(((lastKey&key)==key&&(midKey&key)!=key)||((midKey&key)==key&&(finalKey&key)!=key)){
                return true;  
            }
            return false;
        }

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
        public Byte[] GetBytes(){
            var r= new byte[]{(byte)midKey,(byte)finalKey};
            Reset();
            return r;
        } 

        public void Parse(ProtocolBase message){
            Reset();
            midKey=(KeyNum) message.getByte();
            finalKey=(KeyNum) message.getByte();
           //s UnityEngine.Debug.LogError("lastKey ["+lastKey+"] midKey ["+midKey+"] finalKey["+finalKey+"]");
        }
    }
    public struct JoyStickKey
    {
        public KeyNum key;
        public Fixed2 direction;
        public JoyStickKey(KeyNum key, Fixed2 direction)
        {
            this.key = key;
            this.direction = direction;
        }
        
    }
}
