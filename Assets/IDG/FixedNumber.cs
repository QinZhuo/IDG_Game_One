using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDG;



namespace IDG
{
    /// <summary>
    /// 定点数 使用Int64实现
    /// </summary>
    public struct FixedNumber
    {
        /// <summary>
        /// 小数占用位数
        /// </summary>
        public static int Fix_Fracbits = 16;
        /// <summary>
        /// 0
        /// </summary>
        public static FixedNumber Zero = new FixedNumber(0);
        internal Int64 m_Bits;
        public FixedNumber(int x)
        {
            m_Bits = (x << Fix_Fracbits);
        }
        public FixedNumber(float x)
        {
            m_Bits = (Int64)((x) * (1 << Fix_Fracbits));
            //x*(((Int64)(1)<<Fix_Fracbits))
        }
        public FixedNumber(Int64 x)
        {
            m_Bits = ((x) * (1 << Fix_Fracbits));
        }
        public Int64 GetValue()
        {
            return m_Bits;
        }
        public FixedNumber SetValue(Int64 i)
        {
            m_Bits = i;
            return this;
        }
        public static FixedNumber Lerp(FixedNumber a,FixedNumber b,float t)
        {
            return a + (b - a) * t;
        }
        public static FixedNumber Lerp(FixedNumber a, FixedNumber b, FixedNumber t)
        {
            return a + (b - a) * t;
        }
        public FixedNumber Abs()
        {
           
            return FixedNumber.Abs(this);
        }
        public FixedNumber Sqrt()
        {
            return FixedNumber.Sqrt(this);
        }
        //******************* +  **************************
        public static FixedNumber operator +(FixedNumber p1, FixedNumber p2)
        {
            FixedNumber tmp;
            tmp.m_Bits = p1.m_Bits + p2.m_Bits;
            return tmp;
        }
        public static FixedNumber operator +(FixedNumber p1, int p2)
        {
            FixedNumber tmp;
            tmp.m_Bits = p1.m_Bits + (Int64)(p2 << Fix_Fracbits);
            return tmp;
        }
        public static FixedNumber operator +(int p1, FixedNumber p2)
        {
            return p2 + p1;
        }
        public static FixedNumber operator +(FixedNumber p1, Int64 p2)
        {
            FixedNumber tmp;
            tmp.m_Bits = p1.m_Bits + p2 << Fix_Fracbits;
            return tmp;
        }
        public static FixedNumber operator +(Int64 p1, FixedNumber p2)
        {
            return p2 + p1;
        }

        public static FixedNumber operator +(FixedNumber p1, float p2)
        {
            FixedNumber tmp;
            tmp.m_Bits = p1.m_Bits + (Int64)(p2 * (1 << Fix_Fracbits));
            return tmp;
        }
        public static FixedNumber operator +(float p1, FixedNumber p2)
        {
            FixedNumber tmp = p2 + p1;
            return tmp;
        }
        //*******************  -  **************************
        public static FixedNumber operator -(FixedNumber p1, FixedNumber p2)
        {
            FixedNumber tmp;
            tmp.m_Bits = p1.m_Bits - p2.m_Bits;
            return tmp;
        }

        public static FixedNumber operator -(FixedNumber p1, int p2)
        {
            FixedNumber tmp;
            tmp.m_Bits = p1.m_Bits - (Int64)(p2 << Fix_Fracbits);
            return tmp;
        }

        public static FixedNumber operator -(int p1, FixedNumber p2)
        {
            FixedNumber tmp;
            tmp.m_Bits = (p1 << Fix_Fracbits) - p2.m_Bits;
            return tmp;
        }
        public static FixedNumber operator -(FixedNumber p1, Int64 p2)
        {
            FixedNumber tmp;
            tmp.m_Bits = p1.m_Bits - (p2 << Fix_Fracbits);
            return tmp;
        }
        public static FixedNumber operator -(Int64 p1, FixedNumber p2)
        {
            FixedNumber tmp;
            tmp.m_Bits = (p1 << Fix_Fracbits) - p2.m_Bits;
            return tmp;
        }

        public static FixedNumber operator -(float p1, FixedNumber p2)
        {
            FixedNumber tmp;
            tmp.m_Bits = (Int64)(p1 * (1 << Fix_Fracbits)) - p2.m_Bits;
            return tmp;
        }
        public static FixedNumber operator -(FixedNumber p1, float p2)
        {
            FixedNumber tmp;
            tmp.m_Bits = p1.m_Bits - (Int64)(p2 * (1 << Fix_Fracbits));
            return tmp;
        }

        //******************* * **************************
        public static FixedNumber operator *(FixedNumber p1, FixedNumber p2)
        {
            FixedNumber tmp;
            tmp.m_Bits = ((p1.m_Bits) * (p2.m_Bits)) >> (Fix_Fracbits);
            return tmp;
        }

        public static FixedNumber operator *(int p1, FixedNumber p2)
        {
            FixedNumber tmp;
            tmp.m_Bits = p1 * p2.m_Bits;
            return tmp;
        }
        public static FixedNumber operator *(FixedNumber p1, int p2)
        {
            return p2 * p1;
        }
        public static FixedNumber operator *(FixedNumber p1, float p2)
        {
            FixedNumber tmp;
            tmp.m_Bits = (Int64)(p1.m_Bits * p2);
            return tmp;
        }
        public static FixedNumber operator *(float p1, FixedNumber p2)
        {
            FixedNumber tmp;
            tmp.m_Bits = (Int64)(p1 * p2.m_Bits);
            return tmp;
        }
        //******************* / **************************
        public static FixedNumber operator /(FixedNumber p1, FixedNumber p2)
        {
            FixedNumber tmp;
            if (p2 == FixedNumber.Zero)
            {
                UnityEngine.Debug.LogError("/0");
                tmp.m_Bits = Zero.m_Bits;
            }
            else
            {
                tmp.m_Bits = (p1.m_Bits) * (1 << Fix_Fracbits) / (p2.m_Bits);
            }
            return tmp;
        }
        public static FixedNumber operator /(FixedNumber p1, int p2)
        {
            FixedNumber tmp;
            if (p2 == 0)
            {
                UnityEngine.Debug.LogError("/0");
                tmp.m_Bits = Zero.m_Bits;
            }
            else
            {
                tmp.m_Bits = p1.m_Bits / (p2);
            }
            return tmp;
        }
        public static FixedNumber operator %(FixedNumber p1, int p2)
        {
            FixedNumber tmp;
            if (p2 == 0)
            {
                UnityEngine.Debug.LogError("/0");
                tmp.m_Bits = Zero.m_Bits;
            }
            else
            {
                tmp.m_Bits =( p1.m_Bits % (p2 << Fix_Fracbits));
            }
            return tmp;
        }
        public static FixedNumber operator /(int p1, FixedNumber p2)
        {
            FixedNumber tmp;
            if (p2 == Zero)
            {
                UnityEngine.Debug.LogError("/0");
                tmp.m_Bits = Zero.m_Bits;
            }
            else
            {
                Int64 tmp2 = ((Int64)p1 << Fix_Fracbits << Fix_Fracbits);
                tmp.m_Bits = tmp2 / (p2.m_Bits);
            }
            return tmp;
        }
        public static FixedNumber operator /(FixedNumber p1, Int64 p2)
        {
            FixedNumber tmp;
            if (p2 == 0)
            {
                UnityEngine.Debug.LogError("/0");
                tmp.m_Bits = Zero.m_Bits;
            }
            else
            {
                tmp.m_Bits = p1.m_Bits / (p2);
            }
            return tmp;
        }
        public static FixedNumber operator /(Int64 p1, FixedNumber p2)
        {
            FixedNumber tmp;
            if (p2 == Zero)
            {
                UnityEngine.Debug.LogError("/0");
                tmp.m_Bits = Zero.m_Bits;
            }
            else
            {
                if (p1 > Int32.MaxValue || p1 < Int32.MinValue)
                {
                    tmp.m_Bits = 0;
                    return tmp;
                }
                tmp.m_Bits = (p1 << Fix_Fracbits) / (p2.m_Bits);
            }
            return tmp;
        }
        public static FixedNumber operator /(float p1, FixedNumber p2)
        {
            FixedNumber tmp;
            if (p2 == Zero)
            {
                UnityEngine.Debug.LogError("/0");
                tmp.m_Bits = Zero.m_Bits;
            }
            else
            {
                Int64 tmp1 = (Int64)p1 * ((Int64)1 << Fix_Fracbits << Fix_Fracbits);
                tmp.m_Bits = (tmp1) / (p2.m_Bits);
            }
            return tmp;
        }
        public static FixedNumber operator /(FixedNumber p1, float p2)
        {
            FixedNumber tmp;
            if (p2 > -0.000001f && p2 < 0.000001f)
            {
                UnityEngine.Debug.LogError("/0");
                tmp.m_Bits = Zero.m_Bits;
            }
            else
            {
                tmp.m_Bits = (p1.m_Bits << Fix_Fracbits) / ((Int64)(p2 * (1 << Fix_Fracbits)));
            }
            return tmp;
        }
        public static FixedNumber Sqrt(FixedNumber p1)
        {
            FixedNumber tmp;
            Int64 ltmp = p1.m_Bits * (1 << Fix_Fracbits);
            tmp.m_Bits = (Int64)Math.Sqrt(ltmp);
            return tmp;
        }
        public static bool operator >(FixedNumber p1, FixedNumber p2)
        {
            return (p1.m_Bits > p2.m_Bits) ? true : false;
        }
        public static bool operator <(FixedNumber p1, FixedNumber p2)
        {
            return (p1.m_Bits < p2.m_Bits) ? true : false;
        }
        public static bool operator <=(FixedNumber p1, FixedNumber p2)
        {
            return (p1.m_Bits <= p2.m_Bits) ? true : false;
        }
        public static bool operator >=(FixedNumber p1, FixedNumber p2)
        {
            return (p1.m_Bits >= p2.m_Bits) ? true : false;
        }
        public static bool operator !=(FixedNumber p1, FixedNumber p2)
        {
            return (p1.m_Bits != p2.m_Bits) ? true : false;
        }
        public static bool operator ==(FixedNumber p1, FixedNumber p2)
        {
            return (p1.m_Bits == p2.m_Bits) ? true : false;
        }

        public static bool Equals(FixedNumber p1, FixedNumber p2)
        {
            return (p1.m_Bits == p2.m_Bits) ? true : false;
        }

        public bool Equals(FixedNumber right)
        {
            if (m_Bits == right.m_Bits)
            {
                return true;
            }
            return false;
        }

        public static bool operator >(FixedNumber p1, float p2)
        {
            return (p1.m_Bits > (p2 * (1 << Fix_Fracbits))) ? true : false;
        }
        public static bool operator <(FixedNumber p1, float p2)
        {
            return (p1.m_Bits < (p2 * (1 << Fix_Fracbits))) ? true : false;
        }
        public static bool operator <=(FixedNumber p1, float p2)
        {
            return (p1.m_Bits <= p2 * (1 << Fix_Fracbits)) ? true : false;
        }
        public static bool operator >=(FixedNumber p1, float p2)
        {
            return (p1.m_Bits >= p2 * (1 << Fix_Fracbits)) ? true : false;
        }
        public static bool operator !=(FixedNumber p1, float p2)
        {
            return (p1.m_Bits != p2 * (1 << Fix_Fracbits)) ? true : false;
        }
        public static bool operator ==(FixedNumber p1, float p2)
        {
            return (p1.m_Bits == p2 * (1 << Fix_Fracbits)) ? true : false;
        }

        //public static FPoint Cos(FPoint p1)
        //{
        //    return FP.TrigonometricFunction.Cos(p1);
        //}
        //public static FPoint Sin(FPoint p1)
        //{
        //    return FP.TrigonometricFunction.Sin(p1);
        //}

        public static FixedNumber Max()
        {
            FixedNumber tmp;
            tmp.m_Bits = Int64.MaxValue;
            return tmp;
        }

        public static FixedNumber Max(FixedNumber p1, FixedNumber p2)
        {
            return p1.m_Bits > p2.m_Bits ? p1 : p2;
        }
        public static FixedNumber Min(FixedNumber p1, FixedNumber p2)
        {
            return p1.m_Bits < p2.m_Bits ? p1 : p2;
        }

        public static FixedNumber Precision()
        {
            FixedNumber tmp;
            tmp.m_Bits = 1;
            return tmp;
        }

        public static FixedNumber MaxValue()
        {
            FixedNumber tmp;
            tmp.m_Bits = Int64.MaxValue;
            return tmp;
        }
        public static FixedNumber Abs(FixedNumber P1)
        {
            FixedNumber tmp;
            tmp.m_Bits = Math.Abs(P1.m_Bits);
            return tmp;
        }
        public static FixedNumber operator -(FixedNumber p1)
        {
            FixedNumber tmp;
            tmp.m_Bits = -p1.m_Bits;
            return tmp;
        }

        public float ToFloat()
        {
            return m_Bits / (float)(1 << Fix_Fracbits);
        }
        public UnityEngine.Quaternion ToUnityRotation()
        {
            return UnityEngine.Quaternion.Euler(0, -this.ToFloat(), 0);
        }
        public int ToInt()
        {
            return (int)(m_Bits >> (Fix_Fracbits));
        }
        public override string ToString()
        {
            double tmp = (double)m_Bits / (double)(1 << Fix_Fracbits);
            return tmp.ToString();
        }
    }
}

// namespace IDG.Ratio2
// {
//     public struct Ratio
//     {

//         private static readonly int precision = 100;
//         private int _ratio;
//         public static Ratio one = new Ratio(precision);
//         public static Ratio zero = new Ratio(0);
//         public static Ratio max = new Ratio(int.MaxValue);
//         public static Ratio min = new Ratio(int.MinValue);
//         public int ToPrecisionInt()
//         {
//             return _ratio;
//         }
//         public void SetPrecisionInt(int r)
//         {
//             _ratio = r; 
//         }
//         public static Ratio Max(Ratio a,Ratio b)
//         {
//             return a > b ? a : b;
//         }
        
//         public Ratio Abs()
//         {
//             return new Ratio(Math.Abs(_ratio));
//         }
//         public static Ratio Lerp(Ratio a, Ratio b, Ratio t)
//         {
//             return a + (b - a) * t;
//         }
//         public Ratio(int up, int down)
//         {
//             _ratio = (up * precision) / down;
//         }
//         public int ToInt()
//         {
//             return _ratio / precision;
//         }
//         public Ratio(float up)
//         {
//             _ratio = UnityEngine.Mathf.FloorToInt(up * precision);
            
//         }
        
//         public Ratio(double up)
//         {
//             _ratio = (int)Math.Floor((up * precision));

//         }
//         private Ratio(int precisionRatio)
//         {
//             _ratio = precisionRatio;

//         }
        

//         public static Ratio operator +(Ratio a, Ratio b)
//         {
//             return new Ratio(a._ratio + b._ratio);
//         }
//         public static Ratio operator +(Ratio a, int b)
//         {
//             return new Ratio(a._ratio + b*precision);
//         }
//         public static Ratio operator -(Ratio a, Ratio b)
//         {
//             return new Ratio(a._ratio - b._ratio);
//         }
//         public static Ratio operator -(Ratio a, int b)
//         {
//             return new Ratio(a._ratio - b * precision);
//         }
//         public static Ratio operator *(Ratio a, Ratio b)
//         {
//             Ratio r = new Ratio(a._ratio * b._ratio / precision);
         
//             return r;
//         }
//         public static Ratio operator *(Ratio a, int b)
//         {
           
//             return new Ratio(a._ratio * b );
//         }

//         public static Ratio operator /(Ratio a, Ratio b)
//         {
          
//             Ratio r= new Ratio(a._ratio * precision / b._ratio );
//             if (a != Ratio.zero && r == zero)
//             {
//                 //throw new Exception("Ratio超出最低表示范围" + a + "/" + b);

               

//             }
//             if (a != Ratio.zero && r == zero)
//             {
//                 r = new Ratio(1);
//             }
//             return r;
//         }
//         //public static Ratio operator *(Ratio a, float b)
//         //{
//         //    return new Ratio(a._ratio *UnityEngine.Mathf.Floor(b));
//         //}
//         public static Ratio operator /(Ratio a, int b)
//         {
//            // UnityEngine.Debug.Log("[Ratio] " + a.ToString() + "/ [int] " + b + "=" + new Ratio(a._ratio / b));
//             return new Ratio(a._ratio/ b);
//         }
//         public static Ratio operator %(Ratio a, int b)
//         {
//             return new Ratio(a._ratio% (b*precision));
//         }
//         public static bool operator >(Ratio a, Ratio b)
//         {
//             return a._ratio > b._ratio;
//         }
//         public static bool operator <(Ratio a, Ratio b)
//         {
//             return a._ratio < b._ratio;
//         }
//         public static bool operator <(Ratio a, int b)
//         {
//             return a._ratio < b*precision;
//         }
//         public static bool operator >(Ratio a, int b)
//         {
//             return a._ratio > b * precision;
//         }
//         public static bool operator <=(Ratio a, int b)
//         {
//             return a._ratio <= b * precision;
//         }
//         public static bool operator >=(Ratio a, int b)
//         {
//             return a._ratio >= b * precision;
//         }
//         public static bool operator >=(Ratio a, Ratio b)
//         {
//             return a._ratio >= b._ratio;
//         }
//         public static bool operator <=(Ratio a, Ratio b)
//         {
//             return a._ratio <= b._ratio;
//         }
//         public static bool operator ==(Ratio a, Ratio b)
//         {
//             return a._ratio == b._ratio;
//         }
//         public static bool operator ==(Ratio a, int b)
//         {
//             return a._ratio == b * precision;
//         }
//         public static bool operator !=(Ratio a, int b)
//         {
//             return a._ratio != b * precision;
//         }
//         public static bool operator !=(Ratio a, Ratio b)
//         {
//             return a._ratio != b._ratio;
//         }
//         public static Ratio operator -(Ratio a)
//         {
//             return new Ratio(-a._ratio);
//         }
//         public float ToFloat()
//         {
//             return _ratio * 1.0f / precision;
//         }
//         public double ToDouble()
//         {
//             return _ratio * 1.0 / precision;
//         }
//         public override string ToString()
//         {
//             return ToFloat().ToString();
//         }
//         public override int GetHashCode()
//         {
//             return base.GetHashCode();
//         }
    
//         public override bool Equals(object obj)
//         {
//             return base.Equals(obj);
//         }
//         public Ratio Sqrt()
//         {
//             return new Ratio(Math.Sqrt(ToDouble()));
//         }
//         //public Ratio SQR(Ratio a)
//         //{
//         //    Ratio x = a, y = new Ratio(0f), z = new Ratio(1);
//         //    while (MathR.Abs(x - y) > z)
//         //    {
//         //        y = x;
//         //        x = new Ratio(1,2) * (x + a / x);
//         //    }
//         //    return x;
//         //}
        

//     }

    
// }
// namespace IDG.TrueRatio
// {
//     public struct Ratio
//     {
//         private int _u;//分子
//         public int u
//         {
//             get { return _u; }
//             private set { _u = value; }
//         }
//         private int _d;//分母 
//         public int d
//         {
//             get { return _d; }
//             private set
//             {
//                 if (value == 0) { throw new InvalidOperationException("分母不能为零"); }
//                 else if (value > 0) { _d = value; }
//                 else { _d = -value; u = -u; }
//             }
//         }
//         //private static Ratio temp = new Ratio(0);
//         //public static Ratio Add(Ratio a,Ratio b)
//         //{
//         //    temp.Reset(0);
//         //    temp.Add(a);
//         //    temp.Add(b);
//         //    return temp;
//         //}
//         //public static Ratio GetRatio(int up, int down = 1)
//         //{
//         //    return new Ratio(up, down);
//         //}
//         public Ratio(int up, int down = 1)
//         {
//             _u = 0;
//             _d = 1;
//             u = up;
//             d = down;
//             Reduction();
//         }
//         private void Reset(int up, int down = 1)
//         {
//             u = up;
//             d = down;
//             Reduction();
//         }
//         public override string ToString()
//         {
//             return u + "/" + d;
//         }
//         private void Reduction()//约分
//         {
//             int gcd = GCD(u, d);
//             u /= gcd;
//             d /= gcd;
//         }
//         private int GCD(int a, int b)
//         {
//             int temp = 1;
//             while (b != 0)
//             {
//                 temp = a % b;
//                 a = b;
//                 b = temp;
//             }
//             return a;
//         }
//         public static Ratio operator +(Ratio a, Ratio b)
//         {
//             return new Ratio(a.u * b.d + b.u * a.d, a.d * b.d);
//         }
//         public static Ratio operator -(Ratio a, Ratio b)
//         {
//             return new Ratio(a.u * b.d - b.u * a.d, a.d * b.d);
//         }
//         public static Ratio operator *(Ratio a, Ratio b)
//         {
//             return new Ratio(a.u * b.u, a.d * b.d);
//         }

//         public static Ratio operator /(Ratio a, Ratio b)
//         {
//             return a * !b;
//         }
//         public static Ratio operator !(Ratio a)
//         {
//             return new Ratio(a.d, a.u);
//         }
//         public float ToFloat()
//         {
//             return u * 1.0f / d;
//         }

//     }
// }
