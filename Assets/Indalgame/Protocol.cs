using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDG
{
    /// <summary>
    /// 传输协议基类
    /// </summary>
    public abstract class ProtocolBase
    {
        public abstract ProtocolBase InitMessage(byte[] bytes);
        public abstract int Length { get; }
        public abstract byte[] GetByteStream();
        public abstract void push(Int32 int32);
        public abstract void push(Int64 int64);
        public abstract void push(UInt16 uint16);
        public abstract void push(Byte uint8);
      
        public abstract void push(Boolean boolean);
        public abstract void push(FixedNumber ratio);
        public abstract void push(Fixed2 v2);

        public abstract void push(String str);
        public abstract void push(Byte[] bytes);
        //public abstract bool InitNext(byte[] bytes);


        public abstract Int32 getInt32();
        public abstract Int64 getInt64();
        public abstract UInt16 getUInt16();
        public abstract Byte getByte();
        public abstract Boolean getBoolean();
        public abstract FixedNumber getRatio();
        public abstract Fixed2 getV2();
        public abstract String getString();
        public abstract Byte[] getLastBytes();
    }
    /// <summary>
    /// 字节流传输协议实现
    /// </summary>
    public class ByteProtocol : ProtocolBase
    {
        protected List<Byte> byteList = new List<byte>();
        protected byte[] bytes;
        protected int index = 0;
        protected int lastOffset = 0;
        protected UInt16 strLength = 0;
        protected byte[] tempBytes;
        public override int Length { get { return bytes.Length - (index + lastOffset); } }
        public override byte[] GetByteStream()
        {
            //  push(byteList.Count);
          
            // bytes = byteList.ToArray();
            // bytes = byteList.ToArray();

            return byteList.ToArray();
        }

        public override int getInt32()
        {
            index += lastOffset;
            lastOffset = 4;
            return BitConverter.ToInt32(bytes, index);
        }

        public override long getInt64()
        {
            index += lastOffset;
            lastOffset = 8;
            return BitConverter.ToInt64(bytes, index);
        }

        public override FixedNumber getRatio()
        {
            FixedNumber r = new FixedNumber();
            r.SetValue(getInt64());
            return r;
        }

        public override string getString()
        {
            strLength = getUInt16();
            index += lastOffset;
            lastOffset = strLength;

            return Encoding.Unicode.GetString(bytes, index, strLength);
        }

        public override ushort getUInt16()
        {
            index += lastOffset;
            lastOffset = 2;
            return BitConverter.ToUInt16(bytes, index);
        }

        public override ProtocolBase InitMessage(byte[] bytes)
        {
            this.bytes = bytes;
            index = 0;
            strLength = 0;
            lastOffset = 0;
            return this;
        }

        public override void push(int int32)
        {
            byteList.AddRange(BitConverter.GetBytes(int32));
        }

        public override void push(long int64)
        {
            byteList.AddRange(BitConverter.GetBytes(int64));
        }

        public override void push(ushort uint16)
        {
            byteList.AddRange(BitConverter.GetBytes(uint16));
        }

        public override void push(byte uint8)
        {
            byteList.Add(uint8);
        }

        public override void push(bool boolean)
        {
            byteList.Add(boolean ? (byte)1 : (byte)0);
        }

        public override bool getBoolean()
        {
            return getByte() == 1;
        }

        public override byte getByte()
        {
            index += lastOffset;
            lastOffset = 1;
            return bytes[index];
        }


        public override void push(string str)
        {
            tempBytes = Encoding.Unicode.GetBytes(str);

            strLength = (UInt16)tempBytes.Length;
            push(strLength);
            byteList.AddRange(tempBytes);
        }

        public override void push(FixedNumber ratio)
        {
            push(ratio.GetValue());
          
        }
        public override void push(Fixed2 v2)
        {
            push(v2.x.GetValue());
            push(v2.y.GetValue());
        }

        public override Fixed2 getV2()
        {
            FixedNumber r1 = getRatio(), r2 = getRatio();
            return new Fixed2(r1, r2);
        }

        public override void push(byte[] bytes)
        {
            byteList.AddRange(bytes);
        }

        public override byte[] getLastBytes()
        {
            index += lastOffset;
            lastOffset = bytes.Length - index;
            byte[] temp = new byte[lastOffset];
            Array.Copy(bytes, index, temp, 0, lastOffset);
            return temp;
        }
    }
}
