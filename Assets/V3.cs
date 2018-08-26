using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace IDG
{
    public struct V3
    {
        public Ratio x
        {
            get;
            private set;
        }
        public Ratio y
        {
            get;
            private set;
        }
        public Ratio z
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
        public V3(int x=0, int y=0,int z=0)
        {
            this.x = new Ratio(x, 1);
            this.y = new Ratio(y, 1);
            this.z = new Ratio(z, 1);

        }
        public V3(float x, float y,float z)
        {

            this.x = new Ratio((int)x, 1);
            this.y = new Ratio((int)y, 1);
            this.z = new Ratio((int)z, 1);

        }
        public V3(Ratio x, Ratio y ,Ratio z)
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
        public static V3 operator +(V3 a, V3 b)
        {
            return new V3(a.x + b.x, a.y + b.y,a.z+b.z);
        }
        public static V3 operator -(V3 a, V3 b)
        {
            return new V3(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        //public static V3 operator *(V3 a, Ratio b)
        //{
        //    return new V3(a.x * b, a.y * b, a.z * b);
        //}
        //public static V3 operator *(Ratio a, V3 b)
        //{
        //    return new V3(a*b.x,  a* b.y, a * b.z);
        //}
        public static V3 left = new V3(-1, 0);
        public static V3 right = new V3(1, 0);
        public static V3 up = new V3(0, 1);
        public static V3 down = new V3(0, -1);
        public static V3 zero = new V3(0, 0);
        //public static V3 operator +(V3 v3,Ratio ratio)
        //{

        //}
        public Ratio Dot(V3 b)
        {
            return Dot(this, b);
        }
        public static Ratio Dot(V3 a,V3 b)
        {
            return a.x*b.x+b.y*a.y;
        }

        public static V3 operator -(V3 a)
        {
            return new V3(-a.x, -a.y,-a.z);
        }
        public static V2 operator *(V3 a, V2 b)
        {
            return new V2(-a.z * b.y, a.z * b.x);
        }
        public override string ToString()
        {
            return "{" + x.ToString() + "," + y.ToString() + "}";// + ":" + ToVector3().ToString();
        }
    }
}
