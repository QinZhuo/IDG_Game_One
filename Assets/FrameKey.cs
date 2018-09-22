using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace IDG{
    public enum FrameKey : byte
    {
        Left = 2,//a 0b01
        Right = 4,//d 0b10
        Up = 8, //w  0b
        Down = 16,//s
        MoveKey=32,
        Attack=64,
    }
    public struct JoyStickKey
    {
        public Func<FrameKey> frameKey;
        public Func<V2> direction;
        public JoyStickKey(Func<FrameKey> frameKey, Func<V2> direction)
        {
            this.frameKey = frameKey;
            this.direction = direction;
        }
    }
}
