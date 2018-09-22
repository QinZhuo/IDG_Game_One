using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDG
{
    class MathR
    {
        protected static int tabCount = 18*4;
        protected static readonly List<Ratio> _m_SinTab = new List<Ratio>();
        public static readonly Ratio PI = new Ratio(3.14159265f);
        protected static Ratio GetSinTab(Ratio r)
        {
            
            Ratio i =new Ratio( r.ToInt());
            //UnityEngine.Debug.Log(i.ToInt());
            if (i.ToInt() == _m_SinTab.Count-1)
            {
                return _m_SinTab[(int)i.ToInt()];
            }
            else
            {
               // UnityEngine.Debug.Log(i.ToInt()+":"+ _m_SinTab[i.ToInt()]+":"+ Ratio.Lerp(_m_SinTab[i.ToInt()], _m_SinTab[(i + 1).ToInt()], r - i));
                return Ratio.Lerp(_m_SinTab[(int)i.ToInt()], _m_SinTab[(int)(i+1).ToInt()], r - i);
            }
            
        }
        public static Ratio GetAsinTab(Ratio sin)
        {
            MathR math = Instance;
            //UnityEngine.Debug.Log("GetAsinTab");
            for (int i = _m_SinTab.Count-1; i >=0; i--)
            {
               
                if (sin > _m_SinTab[i])
                {
                    if (i == _m_SinTab.Count-1)
                    {
                        return new Ratio(i) / (tabCount / 4) * (PI / 2);
                    }
                    else
                    {
                        //return new Ratio(i);
                        return Ratio.Lerp(new Ratio(i), new Ratio(i + 1), (sin-_m_SinTab[i])/(_m_SinTab[i+1] - _m_SinTab[i])) / (tabCount / 4) * (PI / 2);
                    }
                }
            }
            return new Ratio();
        }
        protected static MathR Instance
        {
            get
            {
                if (_m_instance == null)
                {
                    _m_instance = new MathR();
                    
                }
                return _m_instance;
            }
        }
        protected static MathR _m_instance;
        protected MathR()
        {
            if (_m_instance == null)
            {
                
                _m_SinTab.Add(new Ratio(0f));//0
                _m_SinTab.Add(new Ratio(0.08715f));
                _m_SinTab.Add(new Ratio(0.17364f));
                _m_SinTab.Add(new Ratio(0.25881f));
                _m_SinTab.Add(new Ratio(0.34202f));//20
                _m_SinTab.Add(new Ratio(0.42261f));
                _m_SinTab.Add(new Ratio(0.5f));

                _m_SinTab.Add(new Ratio(0.57357f));//35
                _m_SinTab.Add(new Ratio(0.64278f));
                _m_SinTab.Add(new Ratio(0.70710f));
                _m_SinTab.Add(new Ratio(0.76604f));
                _m_SinTab.Add(new Ratio(0.81915f));//55
                _m_SinTab.Add(new Ratio(0.86602f));//60

                _m_SinTab.Add(new Ratio(0.90630f));
                _m_SinTab.Add(new Ratio(0.93969f));
                _m_SinTab.Add(new Ratio(0.96592f));
                _m_SinTab.Add(new Ratio(0.98480f));//80
                _m_SinTab.Add(new Ratio(0.99619f));

                _m_SinTab.Add(new Ratio(1f));
               
               
            }
        }
        public static Ratio PiToAngel(Ratio pi)
        {
            return pi / PI * 180;
        }
        public static Ratio Asin(Ratio sin)
        {
            if (sin < -1 || sin > 1) { return new Ratio(); }
            if (sin >= 0)
            {
                return GetAsinTab(sin);
            }
            else
            {
                return -GetAsinTab(-sin);
            }
        }
        public static Ratio Sin(Ratio r)
        {
           
            MathR math= Instance;
            //int tabCount = SinTab.Count*4;
            Ratio result=new Ratio();
            r = (r * tabCount / 2 / PI);
            //int n = r.ToInt();
            while (r < 0)
            {
                r += tabCount;
            }
            while (r > tabCount)
            {
                r -= tabCount;
            }
            if (r >= 0 && r <= tabCount / 4)                // 0 ~ PI/2
            {
                result = GetSinTab(r);
            }
            else if (r > tabCount / 4 && r < tabCount / 2)       // PI/2 ~ PI
            {
                r -= new Ratio(tabCount / 4);
                result = GetSinTab(new Ratio(tabCount / 4) - r);
            }
            else if (r >= tabCount / 2 && r < 3 * tabCount / 4)    // PI ~ 3/4*PI
            {
                r -= new Ratio(tabCount / 2);
                result = -GetSinTab(r);
            }
            else if (r >= 3 * tabCount / 4 && r < tabCount)      // 3/4*PI ~ 2*PI
            {
                r = new Ratio(tabCount) - r;
                result = -GetSinTab(r);
            }
            
            return result;
        }
        public static Ratio Abs(Ratio ratio)
        {
            return Ratio.Abs( ratio);
        }
        public static Ratio Sqrt(Ratio r)
        {
            return Ratio.Sqrt(r);
        }
        
        public static Ratio Cos(Ratio r)
        {
            return Sin(r + PI / 2);
        }
        public static Ratio SinAngle(Ratio angle)
        {
            return Sin(angle / 180 * PI);
        }
        public static Ratio CosAngle(Ratio angle)
        {
            return Cos(angle / 180 * PI);
        }
    }
}
