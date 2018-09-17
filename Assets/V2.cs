using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace IDG
{
    public struct V2
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
       // public static float rotateOffset = 2 * Mathf.PI / 360;

        //public V3()
        //{
        //    this.x = new Ratio(0);
        //    this.y = new Ratio(0);
        //    this.z = new Ratio(0);
        //}
        public V2(int x=0, int y=0)
        {
            this.x = new Ratio(x, 1);
            this.y = new Ratio(y, 1);

        }
        public V2(float x, float y)
        {

            this.x = new Ratio((int)x, 1);
            this.y = new Ratio((int)y, 1);

        }
        public V2(Ratio x, Ratio y)
        {
            this.x = x;
            this.y = y;

        }
        public Vector3 ToVector3()
        {
            return new Vector3(x.ToFloat(), 0, y.ToFloat());
        }
        public static V2 GetV2(Ratio x, Ratio y)
        {
            return new V2(x, y);
        }
        public static V2 operator +(V2 a, V2 b)
        {
            return new V2(a.x + b.x, a.y + b.y);
        }
        public static V2 operator -(V2 a, V2 b)
        {
            return new V2(a.x - b.x, a.y - b.y);
        }
        public static V2 operator *(V2 a, Ratio b)
        {
            return new V2(a.x * b, a.y * b);
        }
        public V2 Rotate(Ratio value)
        {
            
            Ratio tx, ty;
            tx = MathR.CosAngle(value) * x - y * MathR.SinAngle(value);
            ty = MathR.CosAngle(value) * y + x * MathR.SinAngle(value);
           // Debug.Log("f:" + f + "sin90" + Mathf.Sin(90) + "cos90" + (Math.Cos(90)));
            //1,0   tx=1*0-0  ty
            return new V2(tx, ty);
        }
        
        public Ratio ToRotation()
        {
            if (x == 0 && y == 0)
            {
                return new Ratio();
            }
            Ratio sin = this.normalized.y;
            if (this.x > 0)
            {
                return MathR.Asin(sin)/MathR.PI*180;
            }
            else
            {
                return MathR.Asin(-sin) / MathR.PI * 180+180;
            }
        }
        public V2 normalized
        {

            get {
                if (x == 0 && y == 0)
                {
                    return new V2();
                }
                Ratio n = ((x *x) + (y * y)).Sqrt();
                return new V2(x/n,y/n);

            }
        }
        //public static V2 operator *(Ratio a, V2 b)
        //{
        //    return new V2(a*b.x,  a* b.y);
        //}
        public static V2 left = new V2(-1, 0);
        public static V2 right = new V2(1, 0);
        public static V2 up = new V2(0, 1);
        public static V2 down = new V2(0, -1);
        public static V2 zero = new V2(0, 0);
        //public static V3 operator +(V3 v3,Ratio ratio)
        //{

        //}
        public Ratio Dot(V2 b)
        {
            return Dot(this, b);
        }
        public static Ratio Dot(V2 a,V2 b)
        {
            return a.x*b.x+b.y*a.y;
        }

        public static V2 operator -(V2 a)
        {
            return new V2(-a.x, -a.y);
        }
        public static V3 operator *(V2 a, V2 b)
        {
            return new V3(new Ratio(),new Ratio(),  a.x * b.y - a.y * b.x);
        }
        public override string ToString()
        {
            return "{" + x.ToString() + "," + y.ToString() + "}";// + ":" + ToVector3().ToString();
        }

    }
}
