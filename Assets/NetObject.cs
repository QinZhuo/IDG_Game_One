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
            transform.rotation = Quaternion.Euler(0, -net.Rotation.ToFloat(), 0);
        }
        // Update is called once per frame
        void Update()
        {
            LerpNetPos(Time.deltaTime*10);
        }
        private void OnDrawGizmos()
        {
            if (net.Shap == null) return;
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(net.Shap.position.ToVector3(), new Vector3(net.Width.ToFloat(), 0, net.Height.ToFloat()));
            for (int i = 0; i < net.Shap.PointsCount; i++)
            
            {
                Gizmos.DrawCube((net.Shap.GetPoint(i)+ net.Position).ToVector3(), Vector3.one*0.1f);
            }
        }
    }
    [System.Serializable]
    public class NetInfo
    {
        public string name;
        private V2 _position=new V2();
        private V2 _lastPos=new V2();
        private Ratio _lastRota = new Ratio();
        public Ratio Width
        {
            get
            {
                return Shap.width;
            }
        }
        public Ratio Height
        {
            get
            {
                return Shap.height;
            }
        }
        public List<Tree4> trees = new List<Tree4>();
        private Ratio _rotation = new Ratio();
        public V2 Position
        {
            get
            {
                return _position;
            }
            set
            {
                if (Shap == null)
                {
                    _position = value;
                    _lastPos = _position;
                }
                else
                {
                    _position = value;
                    if (!ShapPhysics.Check(this))
                    {
                        _lastPos = _position;
                        Tree4.Move(this);
                    }
                    else
                    {
                        _position = _lastPos;
                    }
                }

            }
        }
        //public Ratio Left
        //{
        //    get
        //    {
        //        return  _shap.left + _previewPos.x;
        //    }
        //}
        //public Ratio Right
        //{
        //    get
        //    {
        //        return _shap.right + _previewPos.x;
        //    }
        //}
        //public Ratio Up
        //{
        //    get
        //    {
        //        return _shap.up + _previewPos.y;
        //    }
        //}
        //public Ratio Down
        //{
        //    get
        //    {
        //        return _shap.down + _previewPos.y;
        //    }
        //}
        public Ratio Rotation
        {
            get
            {
                    return _rotation;
            }
            set
            {
                if (Shap==null)
                {
                    _rotation = value % 360;
                    _lastRota = _rotation;
                }
                else
                {
                    _rotation = value % 360;
                    if (!ShapPhysics.Check(this))
                    {
                        _lastRota = _rotation;
                        Shap.ResetSize();
                       // Debug.Log("rotation" + _rotation);
                        Tree4 .Move(this);  
                    }
                    else
                    {
                        _rotation = _lastRota ;
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

                if (_shap != null)
                {
                    ShapPhysics.Remove(this);
                }
                if (value != null)
                {
                    
                    _shap = value;
                    _shap.netinfo = this;
                    ShapPhysics.Add(this);
                    
                }
                else
                {
                    _shap = value;
                    
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

    //public class TestShap : ShapBase
    //{

    //    public TestShap(bool isA)
    //    {
           
    //        V2[] v2s;
    //        if (isA)
    //        {
               
    //            v2s = new V2[3];
    //            v2s[0] = new V2(4, 5);
    //            v2s[1] = new V2(4, 11);
    //            v2s[2] = new V2(9, 9);
    //        }
    //        else
    //        {
    //            v2s = new V2[4];
    //            v2s[0] = new V2(7, 3);
    //            v2s[1] = new V2(5, 7);
    //            v2s[2] = new V2(12, 7);
    //            v2s[3] = new V2(10, 2);
    //        }
            
    //        Points = v2s;
    //    }

    //}
    
    
}