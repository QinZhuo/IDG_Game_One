using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDG.FightClient;
namespace IDG
{
    public class ShapPhysics
    {
        private static List<NetInfo> shaps = null;
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
            foreach (Tree4 tree in a.trees)
            {
                foreach (var item in tree.objs)
                {
                    if (item != a && Check(a, item))
                    {

                        return true;
                    }
                }
                
            }
            return false;
        }
        protected static bool Check(NetInfo a, NetInfo b)
        {

            return Tree4.BoxCheck(a, b)&&GJKCheck(a.Shap, b.Shap);//;// xB&&yB;
        }
        
        public static bool GJKCheck(ShapBase a, ShapBase b)
        {
            V2 direction = a.position - b.position;
            //V2 A = ;
            Simplex s = new Simplex();
            s.Push(ShapBase.Support(a, b, direction));
            direction = -direction;

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
        List<V2> points = new List<V2>();
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
                d = AB * AO * AB;
            }
            return false;
        }
        public V2 GetDirection()
        {
            return d;
        }

    }
    public class BoxShap : ShapBase
    {

        public BoxShap(Ratio x, Ratio y)
        {
            V2[] v2s = new V2[4];
            v2s[0] = new V2(x / 2, y / 2);
            v2s[1] = new V2(-x / 2, y / 2);
            v2s[2] = new V2(x / 2, -y / 2);
            v2s[3] = new V2(-x / 2, -y / 2);
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

        public V2 GetPoint(int index)
        {
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
                ResetSize();

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
            Ratio maxDot, t;
            V2 p;
            p = GetPoint(index);
            maxDot = V2.Dot(p, direction);
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
            return p + position;
        }
        public static V2 Support(ShapBase a, ShapBase b, V2 direction)
        {
            V2 p1 = a.Support(direction);
            V2 p2 = b.Support(-direction);
            //Debug.Log("Support{ p1:" + p1 + "p2:" + p2 + "p3:" + (p1 - p2));
            return p1 - p2;
        }

    }
}
