using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDG.FSClient;

namespace IDG
{
    public class CollisonInfo
    {
        public bool active = true;
        FixedNumber lastCheckTime = new FixedNumber(-1000);
        Dictionary<NetData, List<NetData>> checkList=new Dictionary<NetData, List<NetData>>();
        Dictionary<NetData, List<NetData>> lastList = new Dictionary<NetData, List<NetData>>();

      



        public Dictionary<NetData, List<NetData>> Check(Tree4 tree)
        {
            if (!active&&InputCenter.Time<=lastCheckTime ) return checkList;
            checkList.Clear();
            int count = tree.objs.Count;
            var objs = tree.objs;
            for (int i = 0; i < count; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    if (objs[i] != objs[j] && ShapPhysics.Check(objs[i], objs[j]))
                    {
                        if (checkList.ContainsKey(objs[i]))
                        {
                            checkList[objs[i]].Add(objs[j]);
                        }
                        else
                        {
                            checkList.Add(objs[i], new List<NetData>());
                            checkList[objs[i]].Add(objs[j]);
                        }
                        if (checkList.ContainsKey(objs[j]))
                        {
                            checkList[objs[j]].Add(objs[i]);
                        }
                        else
                        {
                            checkList.Add(objs[j], new List<NetData>());
                            checkList[objs[j]].Add(objs[i]);
                        }
                        
                    }
                }

            }
            lastCheckTime = InputCenter.Time;
            active = false;
            return checkList;
        }
        //    foreach (var obj1 in tree.objs)
        //    {
        //        foreach (var obj2 in tree.objs)
        //        {
        //            if (obj1 != obj2 && ShapPhysics.Check(obj1, obj2))
        //            {


        //            }
        //        }

        //    }
        //}
       

    }
    public class ShapPhysics
    {
        private static List<NetData> shaps = null;
        public static Tree4 tree = null;
        public static void Init()
        {
            if (shaps == null && tree == null)
            {
                shaps = new List<NetData>();
                tree = new Tree4();

            }
        }
        public static void Add(NetData obj)
        {
            shaps.Add(obj);
            tree.Add(obj);
        }
        public static void Remove(NetData obj)
        {
            shaps.Remove(obj);
            Tree4.Remove(obj);
        }
        //public static bool CheckCollision(NetData a)
        //{
        //   foreach (var item in CheckAll(a))
        //        {
                    
        //            if (!item.isTrigger)
        //            {
        //                return true;
        //            }
        //        }
                
           
        //    return false;
        //}
        public static List<NetData> CheckAll(NetData a)
        {
            List<NetData> others = new List<NetData>();
            foreach (Tree4 tree in a.trees)
            {
                //if (tree == null) UnityEngine.Debug.LogError("tree Is Null");
                //if (tree.collisonInfo == null) UnityEngine.Debug.LogError("collisonInfo Is Null");
                var objs = tree.collisonInfo.Check(tree);
                if (objs.ContainsKey(a))
                {
                    foreach (var obj in tree.collisonInfo.Check(tree)[a])
                    {
                        if (!others.Contains(obj))
                        {
                            others.Add(obj);
                        }
                    }
                    //others.AddRange(tree.collisonInfo.Check(tree)[a]);
                }
                
            }
            return others;
        }
        public static bool Check(NetData a, NetData b)
        {

            return Tree4.BoxCheck(a, b)&&GJKCheck(a.Shap, b.Shap);//;// xB&&yB;
        }
        
        public static bool GJKCheck(ShapBase a, ShapBase b)
        {
            Fixed2 direction = a.position - b.position;
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
        List<Fixed2> points = new List<Fixed2>();
        private Fixed2 d;
        public void Push(Fixed2 point)
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
        public Fixed2 GetA()
        {
            return points[points.Count - 1];
        }
        public Fixed2 GetB()
        {
            return points[points.Count - 2];
        }
        public Fixed2 GetC()
        {
            return points[points.Count - 3];
        }
        public bool ContainsOrigin()
        {
            Fixed2 A = GetA();
            Fixed2 AO = -A;
            Fixed2 B = GetB();
            Fixed2 AB = B - A;
            if (points.Count == 3)
            {
                Fixed2 C = GetC();

                Fixed2 AC = C - A;
                Fixed2 ABnormal = AC * AB * AB;
                Fixed2 ACnormal = AB * AC * AC;
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
        public Fixed2 GetDirection()
        {
            return d;
        }

    }
   
    public abstract class ShapBase
    {
        private FixedNumber left;
        private FixedNumber right;
        private FixedNumber up;
        private FixedNumber down;
        public FixedNumber height;// { get { return Ratio.AbsMax(up,down); } }
        public FixedNumber width;// { get { return Ratio.AbsMax(left, right); } }
        private Fixed2[] _points;
        public NetData netinfo;
        public Fixed2 position { get { if (netinfo != null) { return netinfo.Position; } else { return Fixed2.zero; } } }
        public FixedNumber rotation { get { if (netinfo != null) { return netinfo.Rotation; } else { return new FixedNumber(); } } }

        public Fixed2 GetPoint(int index)
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
        protected Fixed2[] Points
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
            width = FixedNumber.Max(FixedNumber.Abs( left), FixedNumber.Abs(right)) * 2;
            height = FixedNumber.Max(FixedNumber.Abs(up), FixedNumber.Abs(down)) * 2;
        }
        public Fixed2 Support(Fixed2 direction)
        {
            int index = 0;
            FixedNumber maxDot, t;
            Fixed2 p;
            p = GetPoint(index);
            maxDot = Fixed2.Dot(p, direction);
            for (; index < PointsCount; index++)
            {
                t = Fixed2.Dot(GetPoint(index), direction);
                //Debug.Log(_points[index] + "dot" + direction + "=" + t);
                if (t > maxDot)
                {
                    maxDot = t;
                    p = GetPoint(index);
                }
            }
            return p + position;
        }
        public static Fixed2 Support(ShapBase a, ShapBase b, Fixed2 direction)
        {
            Fixed2 p1 = a.Support(direction);
            Fixed2 p2 = b.Support(-direction);
            //Debug.Log("Support{ p1:" + p1 + "p2:" + p2 + "p3:" + (p1 - p2));
            return p1 - p2;
        }

    }

    //public class TwoWayDictionary<T>
    //{
    //    bool[,] map;
    //    Dictionary<T,int> index;
    //    public TwoWayDictionary(){
    //        index = new Dictionary<T, int>();
    //    }
    //    public void Reset(List<T> objs)
    //    {
    //        map = new bool[objs.Count, objs.Count];
    //        index.Clear();
    //        int i = 0;
    //        foreach (var obj in objs)
    //        {
    //            index.Add(obj, i);
    //            i++;
    //        }
    //    }
        
    //}
}
