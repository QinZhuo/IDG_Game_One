using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IDG;
namespace IDG.FightClient
{
    public class NetObject : MonoBehaviour
    {
        
        public NetInfo net;
        // Use this for initialization

      
        protected void LerpNetPos(float timer)
        {
            transform.position = Vector3.Lerp(transform.position, net.Position.ToVector3(), timer);
        }
        // Update is called once per frame
        void Update()
        {
            LerpNetPos(Time.deltaTime*10);
        }
    }
    [System.Serializable]
    public class NetInfo
    {
        private V2 _position;
        private V2 _previewPos;
        public Ratio Left
        {
            get
            {
                return  Shap.left + _previewPos.x;
            }
        }
        public Ratio Right
        {
            get
            {
                return Shap.right + _previewPos.x;
            }
        }
        public Ratio Up
        {
            get
            {
                return Shap.up + _previewPos.y;
            }
        }
        public Ratio Down
        {
            get
            {
                return Shap.down + _previewPos.y;
            }
        }
        public V2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                if (Shap==null)
                {
                    _position = value;
                    _previewPos = _position;
                }
                else
                {
                    _previewPos = value;
                    if (!ShapPhysics.Check(this))
                    {
                        _position = value;
                    }
                    else
                    {
                        Debug.Log("碰撞" + this.ClientId);
                        _previewPos = _position;
                    }
                }
               
            }
        }
        public int ClientId;
        private ShapBase _shap;
        public ShapBase Shap
        {
            get
            {
                return _shap;
            }
            set
            {
                if (value != null)
                {
                    ShapPhysics.shaps.Add(this);
                }
                else
                {
                    ShapPhysics.shaps.Remove(this);
                }
                _shap = value;
            }
        }
        public Ratio deltaTime
        {
            get
            {
                return FightClient.deltaTime;
            }
        }
        public InputUnit Input
        {
            get
            {
                return InputCenter.Instance.inputs[this.ClientId];
            }
        }
       
    }
    //[System.Serializable]
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
    public V2(int x,int y)
        {
            this.x = new Ratio(x,1);
            this.y = new Ratio(y,1);
           
        }
        public V2(Ratio x, Ratio y)
        {
            this.x =x;
            this.y =y;
           
        }
        public Vector3 ToVector3()
        {
            return new Vector3(x.ToFloat(),0, y.ToFloat());
        }
        public static V2 GetV2(Ratio x,Ratio y)
        {
            return new V2(x, y);
        }
        public static V2 operator +(V2 a,V2 b)
        {
            return new V2(a.x + b.x, a.y + b.y);
        }
        public static V2 operator *(V2 a, Ratio b)
        {
            return new V2(a.x *b, a.y * b);
        }
        public static V2 left = new V2(-1, 0);
        public static V2 right = new V2(1, 0);
        public static V2 up = new V2(0,1);
        public static V2 down = new V2(0, -1);

        //public static V3 operator +(V3 v3,Ratio ratio)
        //{

        //}
        public override string ToString()
        {
            return "{" + x.ToString() +",0," + y.ToString() + "}"+":"+ToVector3().ToString();
        }
    }

    public class ShapPhysics
    {
        public static List<NetInfo> shaps=new List<NetInfo>();

        public static bool Check(NetInfo a)
        {
            foreach (NetInfo item in shaps)
            {

                if (item!=a&&Check(a, item))
                {
                    return true;
                }
            }
            
            return false;
        }
        public static bool Check(NetInfo a,NetInfo b)
        {
            bool xB=false, yB = false;
            if (a.Position.x < b.Position.x)
            {
                if (a.Right > b.Left)
                {
                    xB = true;
                }
            }
            else if (a.Position.x > b.Position.x )
            {
                if (b.Right >a.Left)
                {
                    xB = true;
                }
            }
            else
            {
                xB = true;
            }
            if (a.Position.y < b.Position.y )
            {
                if ( a.Up > b.Down)
                {
                    yB = true;
                }
            }
            else if (a.Position.y > b.Position.y)
            {
                if (b.Up > a.Down)
                {
                    yB = true;
                }
            }
            else
            {
                yB = true;
            }
            return xB&&yB;
        }
    }

    public class BoxShap:ShapBase
    {
        
        public BoxShap(Ratio r)
        {
            V2[] v2s = new V2[4];
            v2s[0] = new V2(r, r);
            v2s[1] = new V2(-r, r);
            v2s[2] = new V2(r, -r);
            v2s[3] = new V2(-r, -r);
            Points = v2s;
        }
    }
    public class ShapBase
    {
        public Ratio left;
        public Ratio right;
        public Ratio up;
        public Ratio down;
        private V2[] _points;
        public V2[] Points
        {
            get
            {
                return _points;
            }
            set
            {
                left = value[0].x;
                right = value[0].x;
                up = value[0].y;
                down = value[0].y;
                for (int i = 0; i < value.Length; i++)
                {
                    if ( value[i].x < left)
                    {
                        left = value[i].x;
                    }
                    if (value[i].x > right)
                    {
                        right = value[i].x;
                    }
                    if (value[i].y < down)
                    {
                        down = value[i].y;
                    }
                    if (value[i].y > up)
                    {
                        up = value[i].y;
                    }
                }
                _points = value;
            }
        }
        
    }
    
}