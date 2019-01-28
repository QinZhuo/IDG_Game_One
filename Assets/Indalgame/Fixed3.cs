using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace IDG
{
    public struct Fixed3
    {
        public FixedNumber x
        {
            get;
            private set;
        }
        public FixedNumber y
        {
            get;
            private set;
        }
        public FixedNumber z
        {
            get;
            private set;
        }

        //public V3()
        //{
        //    this.x = new Ratio(0);
        //    this.y = new Ratio(0);
        //    this.z = new Ratio(0);
        //}
        public Fixed3(int x=0, int y=0,int z=0)
        {
            this.x = new FixedNumber(x);
            this.y = new FixedNumber(y);
            this.z = new FixedNumber(z);

        }
        public Fixed3(float x, float y,float z)
        {

            this.x = new FixedNumber(x);
            this.y = new FixedNumber(y);
            this.z = new FixedNumber(z);

        }
        public Fixed3(FixedNumber x, FixedNumber y ,FixedNumber z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Vector3 ToVector3()
        {
            return new Vector3(x.ToFloat(), 0, y.ToFloat());
        }
        //public static V3 GetV3(Ratio x, Ratio y)
        //{
        //    return new V3(x, y);
        //}
        public static Fixed3 operator +(Fixed3 a, Fixed3 b)
        {
            return new Fixed3(a.x + b.x, a.y + b.y,a.z+b.z);
        }
        public static Fixed3 operator -(Fixed3 a, Fixed3 b)
        {
            return new Fixed3(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        //public static V3 operator *(V3 a, Ratio b)
        //{
        //    return new V3(a.x * b, a.y * b, a.z * b);
        //}
        //public static V3 operator *(Ratio a, V3 b)
        //{
        //    return new V3(a*b.x,  a* b.y, a * b.z);
        //}
        public static Fixed3 left = new Fixed3(-1, 0);
        public static Fixed3 right = new Fixed3(1, 0);
        public static Fixed3 up = new Fixed3(0, 1);
        public static Fixed3 down = new Fixed3(0, -1);
        public static Fixed3 zero = new Fixed3(0, 0);
        //public static V3 operator +(V3 v3,Ratio ratio)
        //{

        //}
        public FixedNumber Dot(Fixed3 b)
        {
            return Dot(this, b);
        }
        public static FixedNumber Dot(Fixed3 a,Fixed3 b)
        {
            return a.x*b.x+b.y*a.y;
        }

        public static Fixed3 operator -(Fixed3 a)
        {
            return new Fixed3(-a.x, -a.y,-a.z);
        }
        public static Fixed2 operator *(Fixed3 a, Fixed2 b)
        {
            return new Fixed2(-a.z * b.y, a.z * b.x);
        }
        public override string ToString()
        {
            return "{" + x.ToString() + "," + y.ToString() + "}";// + ":" + ToVector3().ToString();
        }
    }
}
