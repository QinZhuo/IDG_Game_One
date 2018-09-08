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
                    _rotation = value;
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

    public class TestShap : ShapBase
    {

        public TestShap(bool isA)
        {
           
            V2[] v2s;
            if (isA)
            {
               
                v2s = new V2[3];
                v2s[0] = new V2(4, 5);
                v2s[1] = new V2(4, 11);
                v2s[2] = new V2(9, 9);
            }
            else
            {
                v2s = new V2[4];
                v2s[0] = new V2(7, 3);
                v2s[1] = new V2(5, 7);
                v2s[2] = new V2(12, 7);
                v2s[3] = new V2(10, 2);
            }
            
            Points = v2s;
        }

    }
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
        public static void Remove(NetInfo obj)
        {
            shaps.Remove(obj);
            Tree4.Remove(obj);
        }
        public static bool Check(NetInfo a)
        {
            foreach (NetInfo item in shaps)
            {

                if (item!=a&&Check(a, item))
                {
                    Debug.Log("碰撞" + item.name);
                    return true;
                }
            }
            
            return false;
        }
        public static bool Check(NetInfo a,NetInfo b)
        {

            return GJKCheck(a.Shap, b.Shap);//Tree4.BoxCheck(a, b);// xB&&yB;
        }
        public static bool GJKCheck(ShapBase a, ShapBase b)
        {
            V2 direction =  a.position - b.position;
            //V2 A = ;
            Simplex s = new Simplex();
            s.Push(ShapBase.Support(a, b, direction));
            direction =-direction;
            
            while (true)
            {

                s.Push(ShapBase.Support(a, b, direction));
                if (s.GetA().Dot(direction) < 0)
                {
                    return false;
                }
                else
                {
                    if (s.ContainsOrigin())
                    {
                        return true;
                    }
                    else
                    {
                        direction = s.GetDirection();
                    }
                }
                //s.Push(A);

            }
            //return false;
        }
    }
    class Simplex
    {
        List<V2> points=new List<V2>();
        private V2 d;
        public void Push(V2 point)
        {
            points.Add(point);
        }
        //public V2 Pop()
        //{
        //    V2 point = points[0];
        //    points.RemoveAt(0);
        //    return point;
        //}
        //public V2 Peek()
        //{
        //    return points[0];
        //}
        public V2 GetA()
        {
            return points[points.Count - 1];
        }
        public V2 GetB()
        {
            return points[points.Count - 2];
        }
        public V2 GetC()
        {
            return points[points.Count - 3];
        }
        public bool ContainsOrigin()
        {
            V2 A = GetA();
            V2 AO = -A;
            V2 B = GetB();
            V2 AB = B - A;
            if (points.Count == 3)
            {
                V2 C = GetC();

                V2 AC = C - A;
                V2 ABnormal = AC * AB * AB;
                V2 ACnormal = AB * AC * AC;
               // Debug.Log("A" + A + "B" + B + "C" + C);
                if (ABnormal.Dot(AO) > 0)
                {
                    points.Remove(C);
                    d = ABnormal;
                }
                else
                {
                    if (ACnormal.Dot(AO) > 0)
                    {
                        points.Remove(B);
                        d = ACnormal;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                //Debug.Log("A" + A + "B" + B);
                d =AB*AO*AB;
            }
            return false;
        }
        public V2 GetDirection()
        {
            return d;
        }
        
    }
    public class BoxShap:ShapBase
    {
        
        public BoxShap(Ratio x,Ratio y)
        {
            V2[] v2s = new V2[4];
            v2s[0] = new V2(x/2, y/2);
            v2s[1] = new V2(-x/2, y/2);
            v2s[2] = new V2(x/2, -y/2);
            v2s[3] = new V2(-x/2, -y/2);
            Points = v2s;
        }
       
    }
    public abstract class ShapBase
    {
        private Ratio left;
        private Ratio right;
        private Ratio up;
        private Ratio down;
        public Ratio height;// { get { return Ratio.AbsMax(up,down); } }
        public Ratio width;// { get { return Ratio.AbsMax(left, right); } }
        private V2[] _points;
        public NetInfo netinfo;
        public V2 position { get { if (netinfo != null) { return netinfo.Position; } else { return V2.zero; } } }
        public Ratio rotation { get { if (netinfo != null) { return netinfo.Rotation; } else { return new Ratio(); } } }

        public V2 GetPoint(int index){
            return _points[index].Rotate(rotation);
        }
        public int PointsCount
        {
            get
            {
                return _points.Length;
            }
        }
        protected V2[] Points
        {
            set
            {
                _points = value;

            }
        }
        public void ResetSize()
        {
            left = _points[0].Rotate(rotation).x;
            right = _points[0].Rotate(rotation).x;
            up = _points[0].Rotate(rotation).y;
            down = _points[0].Rotate(rotation).y;
            for (int i = 0; i < _points.Length; i++)
            {
                if (_points[i].Rotate(rotation).x < left)
                {
                    left = _points[i].Rotate(rotation).x;
                }
                if (_points[i].Rotate(rotation).x > right)
                {
                    right = _points[i].Rotate(rotation).x;
                }
                if (_points[i].Rotate(rotation).y < down)
                {
                    down = _points[i].Rotate(rotation).y;
                }
                if (_points[i].Rotate(rotation).y > up)
                {
                    up = _points[i].Rotate(rotation).y;
                }
            }
            width = Ratio.Max(left.Abs(), right.Abs()) * 2;
            height = Ratio.Max(up.Abs(), down.Abs()) * 2;
        }
        public V2 Support(V2 direction)
        {
            int index = 0;
            Ratio maxDot,t;
            V2 p;
            p = GetPoint(index);
            maxDot=V2.Dot(p, direction);
            for (; index < PointsCount; index++)
            {
                t = V2.Dot(GetPoint(index), direction);
                //Debug.Log(_points[index] + "dot" + direction + "=" + t);
                if (t > maxDot)
                {
                    maxDot = t;
                    p = GetPoint(index);
                }
            }
            return p+position;
        }
        public static V2 Support(ShapBase a,ShapBase b, V2 direction)
        {
            V2 p1 = a.Support(direction);
            V2 p2= b.Support(-direction);
            //Debug.Log("Support{ p1:" + p1 + "p2:" + p2 + "p3:" + (p1 - p2));
            return p1 - p2;
        }

    }
    
}