using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDG
{
    /// <summary>
    /// 定点数数学类
    /// </summary>
    class MathFixed
    {
        protected static int tabCount = 18*4;
        /// <summary>
        /// sin值对应表
        /// </summary>
        protected static readonly List<FixedNumber> _m_SinTab = new List<FixedNumber>();
        public static readonly FixedNumber PI = new FixedNumber(3.14159265f);
        protected static FixedNumber GetSinTab(FixedNumber r)
        {
            
            FixedNumber i =new FixedNumber( r.ToInt());
            //UnityEngine.Debug.Log(i.ToInt());
            if (i.ToInt() == _m_SinTab.Count-1)
            {
                return _m_SinTab[(int)i.ToInt()];
            }
            else
            {
               // UnityEngine.Debug.Log(i.ToInt()+":"+ _m_SinTab[i.ToInt()]+":"+ Ratio.Lerp(_m_SinTab[i.ToInt()], _m_SinTab[(i + 1).ToInt()], r - i));
                return FixedNumber.Lerp(_m_SinTab[(int)i.ToInt()], _m_SinTab[(int)(i+1).ToInt()], r - i);
            }
            
        }
        public static FixedNumber GetAsinTab(FixedNumber sin)
        {
            MathFixed math = Instance;
            //UnityEngine.Debug.Log("GetAsinTab");
            for (int i = _m_SinTab.Count-1; i >=0; i--)
            {
               
                if (sin > _m_SinTab[i])
                {
                    if (i == _m_SinTab.Count-1)
                    {
                        return new FixedNumber(i) / (tabCount / 4) * (PI / 2);
                    }
                    else
                    {
                        //return new Ratio(i);
                        return FixedNumber.Lerp(new FixedNumber(i), new FixedNumber(i + 1), (sin-_m_SinTab[i])/(_m_SinTab[i+1] - _m_SinTab[i])) / (tabCount / 4) * (PI / 2);
                    }
                }
            }
            return new FixedNumber();
        }
        protected static MathFixed Instance
        {
            get
            {
                if (_m_instance == null)
                {
                    _m_instance = new MathFixed();
                    
                }
                return _m_instance;
            }
        }
        protected static MathFixed _m_instance;
        protected MathFixed()
        {
            if (_m_instance == null)
            {
                
                _m_SinTab.Add(new FixedNumber(0f));//0
                _m_SinTab.Add(new FixedNumber(0.08715f));
                _m_SinTab.Add(new FixedNumber(0.17364f));
                _m_SinTab.Add(new FixedNumber(0.25881f));
                _m_SinTab.Add(new FixedNumber(0.34202f));//20
                _m_SinTab.Add(new FixedNumber(0.42261f));
                _m_SinTab.Add(new FixedNumber(0.5f));

                _m_SinTab.Add(new FixedNumber(0.57357f));//35
                _m_SinTab.Add(new FixedNumber(0.64278f));
                _m_SinTab.Add(new FixedNumber(0.70710f));
                _m_SinTab.Add(new FixedNumber(0.76604f));
                _m_SinTab.Add(new FixedNumber(0.81915f));//55
                _m_SinTab.Add(new FixedNumber(0.86602f));//60

                _m_SinTab.Add(new FixedNumber(0.90630f));
                _m_SinTab.Add(new FixedNumber(0.93969f));
                _m_SinTab.Add(new FixedNumber(0.96592f));
                _m_SinTab.Add(new FixedNumber(0.98480f));//80
                _m_SinTab.Add(new FixedNumber(0.99619f));

                _m_SinTab.Add(new FixedNumber(1f));
               
               
            }
        }
        public static FixedNumber PiToAngel(FixedNumber pi)
        {
            return pi / PI * 180;
        }
        public static FixedNumber Asin(FixedNumber sin)
        {
            if (sin < -1 || sin > 1) { return new FixedNumber(); }
            if (sin >= 0)
            {
                return GetAsinTab(sin);
            }
            else
            {
                return -GetAsinTab(-sin);
            }
        }
        public static FixedNumber Sin(FixedNumber r)
        {
           
            MathFixed math= Instance;
            //int tabCount = SinTab.Count*4;
            FixedNumber result=new FixedNumber();
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
                r -= new FixedNumber(tabCount / 4);
                result = GetSinTab(new FixedNumber(tabCount / 4) - r);
            }
            else if (r >= tabCount / 2 && r < 3 * tabCount / 4)    // PI ~ 3/4*PI
            {
                r -= new FixedNumber(tabCount / 2);
                result = -GetSinTab(r);
            }
            else if (r >= 3 * tabCount / 4 && r < tabCount)      // 3/4*PI ~ 2*PI
            {
                r = new FixedNumber(tabCount) - r;
                result = -GetSinTab(r);
            }
            
            return result;
        }
        public static FixedNumber Abs(FixedNumber ratio)
        {
            return FixedNumber.Abs( ratio);
        }
        public static FixedNumber Sqrt(FixedNumber r)
        {
            return FixedNumber.Sqrt(r);
        }
        
        public static FixedNumber Cos(FixedNumber r)
        {
            return Sin(r + PI / 2);
        }
        public static FixedNumber SinAngle(FixedNumber angle)
        {
            return Sin(angle / 180 * PI);
        }
        public static FixedNumber CosAngle(FixedNumber angle)
        {
            return Cos(angle / 180 * PI);
        }
    }
}
