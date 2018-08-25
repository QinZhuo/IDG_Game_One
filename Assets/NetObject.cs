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
        private V2 _position=new V2();
        private V2 _previewPos=new V2();
        public Ratio Left
        {
            get
            {
                return  _shap.left + _previewPos.x;
            }
        }
        public Ratio Right
        {
            get
            {
                return _shap.right + _previewPos.x;
            }
        }
        public Ratio Up
        {
            get
            {
                return _shap.up + _previewPos.y;
            }
        }
        public Ratio Down
        {
            get
            {
                return _shap.down + _previewPos.y;
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
        public int ClientId=-1;
        private ShapBase _shap;//=new BoxShap(new Ratio(1,2));
        public ShapBase Shap
        {
            get
            {
                return _shap;
            }
            set
            {
                //ShapPhysics.Add(this);
                //_shap = new BoxShap(new Ratio(1, 2));
                _shap = value;
                if (value != null)
                {
                    ShapPhysics.Add(this);
                }
                else
                {
                   // ShapPhysics.remove(this);
                }
                
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
    

    public class ShapPhysics
    {
        private static List<NetInfo> shaps=null;
        public static Tree4 tree = null;
        public static void Init()
        {
            if (shaps == null && tree == null)
            {
                shaps = new List<NetInfo>();
                tree = new Tree4();
            }
        }
        public static void Add(NetInfo obj)
        {
            shaps.Add(obj);
            tree.Add(obj);
        }
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
    public abstract class ShapBase
    {
        public Ratio left;
        public Ratio right;
        public Ratio up;
        public Ratio down;
        private V2[] _points;
        public V2[] Points
        {
            //get
            //{
            //    return _points;
            //}
            set
            {
                _points = value;
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
                
            }
        }
        
    }
    
}