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
        public static V2 operator *(V2 a, Ratio b)
        {
            return new V2(a.x * b, a.y * b);
        }
        public static V2 left = new V2(-1, 0);
        public static V2 right = new V2(1, 0);
        public static V2 up = new V2(0, 1);
        public static V2 down = new V2(0, -1);

        //public static V3 operator +(V3 v3,Ratio ratio)
        //{

        //}
        public override string ToString()
        {
            return "{" + x.ToString() + ",0," + y.ToString() + "}" + ":" + ToVector3().ToString();
        }
    }
}
