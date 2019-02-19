using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace IDG
{
    /// <summary>
    /// 定点数二维向量
    /// </summary>
    [System.Serializable]
    public struct Fixed2
    {
        public FixedNumber x;
        public FixedNumber y;
       // public static float rotateOffset = 2 * Mathf.PI / 360;

        //public V3()
        //{
        //    this.x = new Ratio(0);
        //    this.y = new Ratio(0);
        //    this.z = new Ratio(0);
        //}
        
        
        public Fixed2(float x, float y)
        {

            this.x = new FixedNumber(x);
            this.y = new FixedNumber(y);

        }
        public Fixed2(FixedNumber x, FixedNumber y)
        {
            this.x = x;
            this.y = y;

        }
        public Vector3 ToVector3()
        {
            return new Vector3(x.ToFloat(), 0, y.ToFloat());
        }
        public static Fixed2 GetV2(FixedNumber x, FixedNumber y)
        {
            return new Fixed2(x, y);
        }
        public static Fixed2 operator +(Fixed2 a, Fixed2 b)
        {
            return new Fixed2(a.x + b.x, a.y + b.y);
        }
        public static Fixed2 operator -(Fixed2 a, Fixed2 b)
        {
            return new Fixed2(a.x - b.x, a.y - b.y);
        }
        public static Fixed2 operator *(Fixed2 a, FixedNumber b)
        {
            return new Fixed2(a.x * b, a.y * b);
        }
        public Fixed2 Rotate(FixedNumber value)
        {
            
            FixedNumber tx, ty;
            tx = MathFixed.CosAngle(value) * x - y * MathFixed.SinAngle(value);
            ty = MathFixed.CosAngle(value) * y + x * MathFixed.SinAngle(value);
           // Debug.Log("f:" + f + "sin90" + Mathf.Sin(90) + "cos90" + (Math.Cos(90)));
            //1,0   tx=1*0-0  ty
            return new Fixed2(tx, ty);
        }
        
        public FixedNumber ToRotation()
        {
            if (x == 0 && y == 0)
            {
                return new FixedNumber();
            }
            FixedNumber sin = this.normalized.y;
            if (this.x >= 0)
            {
                return MathFixed.Asin(sin)/MathFixed.PI*180;
            }
            else
            {
                return MathFixed.Asin(-sin) / MathFixed.PI * 180+180;
            }
        }
        public static Fixed2 Parse(FixedNumber ratio)
        {
            return new Fixed2(MathFixed.CosAngle(ratio), MathFixed.SinAngle(ratio) );
        }
        public Fixed2 normalized
        {

            get {
                if (x == 0 && y == 0)
                {
                    return new Fixed2();
                }
                FixedNumber n =((x *x) + (y * y)).Sqrt();
             //   Debug.Log("N" + ((x * x) + (y * y)).Sqrt());
                return new Fixed2(x/n,y/n);

            }
        }
        //public static V2 operator *(Ratio a, V2 b)
        //{
        //    return new V2(a*b.x,  a* b.y);
        //}
        public static Fixed2 left = new Fixed2(-1, 0);
        public static Fixed2 right = new Fixed2(1, 0);
        public static Fixed2 up = new Fixed2(0, 1);
        public static Fixed2 down = new Fixed2(0, -1);
        public static Fixed2 zero = new Fixed2(0, 0);
        //public static V3 operator +(V3 v3,Ratio ratio)
        //{

        //}
        public FixedNumber Dot(Fixed2 b)
        {
            return Dot(this, b);
        }
        public static FixedNumber Dot(Fixed2 a,Fixed2 b)
        {
            return a.x*b.x+b.y*a.y;
        }

        public static Fixed2 operator -(Fixed2 a)
        {
            return new Fixed2(-a.x, -a.y);
        }
        public static Fixed3 operator *(Fixed2 a, Fixed2 b)
        {
            return new Fixed3(new FixedNumber(),new FixedNumber(),  a.x * b.y - a.y * b.x);
        }
        public static bool operator ==(Fixed2 a, Fixed2 b)
        {
            return a.x == b.x && a.y == b.y;
        }
        public static bool operator !=(Fixed2 a, Fixed2 b)
        {
            return a.x != b.x || a.y != b.y;
        }
        public override string ToString()
        {
            return "{" + x.ToString() + "," + y.ToString() + "}";// + ":" + ToVector3().ToString();
        }

    }
}
