using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDG.FSClient;

namespace IDG
{
    /// <summary>
    /// 物理碰撞信息
    /// </summary>
    public class CollisonInfo
    {
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool active = false;
        /// <summary>
        /// 上次碰撞检测时间
        /// </summary>
        //FixedNumber lastCheckTime = new FixedNumber(-1000);
        /// <summary>
        /// 碰撞列表
        /// </summary>
       // Dictionary<NetData, List<NetData>> checkList=new Dictionary<NetData, List<NetData>>();
        /// <summary>
        /// 碰撞检测
        /// </summary>
        /// <param name="tree">检测的节点</param>
        /// <returns>碰撞的对象</returns>
        //public Dictionary<NetData, List<NetData>> Check(Tree4 tree,FixedNumber time)
        //{
        //    if (!active&&time<=lastCheckTime ) return checkList;
        //    checkList.Clear();
        //    int count = tree.objs.Count;
        //    var objs = tree.objs;
        //    for (int i = 0; i < count; i++)
        //    {
        //        for (int j = i + 1; j < count; j++)
        //        {
        //            if (objs[i] != objs[j] && ShapPhysics.Check(objs[i], objs[j]))
        //            {
        //                if (checkList.ContainsKey(objs[i]))
        //                {
        //                    checkList[objs[i]].Add(objs[j]);
        //                }
        //                else
        //                {
        //                    checkList.Add(objs[i], new List<NetData>());
        //                    checkList[objs[i]].Add(objs[j]);
        //                }
        //                if (checkList.ContainsKey(objs[j]))
        //                {
        //                    checkList[objs[j]].Add(objs[i]);
        //                }
        //                else
        //                {
        //                    checkList.Add(objs[j], new List<NetData>());
        //                    checkList[objs[j]].Add(objs[i]);
        //                }
                        
        //            }
        //        }

        //    }
        //    lastCheckTime = time;
        //    active = false;
        //    return checkList;
        //}
    }
    /// <summary>
    /// 物理检测
    /// </summary>
    public class ShapPhysics
    {
        /// <summary>
        /// 图形列表
        /// </summary>
        private List<NetData> shaps = null;
        /// <summary>
        /// 空间分割四叉树对象
        /// </summary>
        public Tree4 tree = null;
        public void Init()
        {
            if (shaps == null && tree == null)
            {
                shaps = new List<NetData>();
                tree = new Tree4();
            }
        }
        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="obj">游戏对象</param>
        public void Add(NetData obj)
        {
            shaps.Add(obj);
            tree.Add(obj);
        }
        /// <summary>
        /// 移除对象
        /// </summary>
        /// <param name="obj">游戏对象</param>
        public void Remove(NetData obj)
        {
            shaps.Remove(obj);
            Tree4.Remove(obj);
        }

        
        //GJK算法原理
        //两个物体进行明可夫斯基差操作 得到的新图形形状包含原点则这两个图形的是相交的
        /// <summary>
        /// 立即检测两个物体是否发生细节碰撞 
        /// 先包围盒检测（粗略） 后GJK碰撞检测（细节）
        /// </summary>
        /// <param name="a">检测对象a</param>
        /// <param name="b">检测对象b</param>
        /// <returns>是否碰撞</returns>
        public static bool Check(ShapBase a, ShapBase b)
        {
            return BoxCheck(a, b)&&GJKCheck(a, b);//;// xB&&yB;
        }
        public List<NetData> OverlapShap(ShapBase shap,Fixed2 position)
        {
            shap.position = position;
            return OverlapShap(shap);
        }
        public List<NetData> OverlapShap(ShapBase shap)
        {
            return tree.CheckShap(shap);
        }
        /// <summary>
        /// GJK算法的多边形碰撞检测
        /// </summary>
        /// <param name="a">形状a</param>
        /// <param name="b">形状b</param>
        /// <returns>是否碰撞</returns>
        public static bool GJKCheck(ShapBase a, ShapBase b)
        {
            Fixed2 direction = a.position - b.position;
          
            Simplex s = new Simplex();
            s.Push(ShapBase.Support(a, b, direction));
            direction = -direction;
            while (true)        //迭代
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
             

            }
        }

        public static bool BoxCheck(ShapBase objA, ShapBase objB)
        {
            if (FixedNumber.Abs((objA.position.x - objB.position.x)) < (objA.width + objB.width) / 2
                &&
                FixedNumber.Abs((objA.position.y - objB.position.y)) < (objA.height + objB.height) / 2
                )
            {
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 单纯形（Simplex）
    /// 2阶单纯形，2维空间，2 + 1 = 3个顶点
    /// 所以就是一个平面（2维空间）上的一个三角形 
    /// </summary>
    class Simplex
    {
        List<Fixed2> points = new List<Fixed2>();
        private Fixed2 d;
        public void Push(Fixed2 point)
        {
            points.Add(point);
        }
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
        /// <summary>
        /// 检测是否包含原点 包含原点就发生了碰撞
        /// </summary>
        /// <returns>是否包含原点</returns>
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
                d = AB * AO * AB;
            }
            return false;
        }
        /// <summary>
        /// 获取下一步迭代的方向
        /// </summary>
        /// <returns>迭代方向</returns>
        public Fixed2 GetDirection()
        {
            return d;
        }

    }
    /// <summary>
    /// 形状基类
    /// </summary>
    [System.Serializable]
    public class ShapBase
    {
        private FixedNumber left;
        private FixedNumber right;
        private FixedNumber up;
        private FixedNumber down;
        public FixedNumber height;// { get { return Ratio.AbsMax(up,down); } }
        public FixedNumber width;// { get { return Ratio.AbsMax(left, right); } }
        protected Fixed2[] _points;
        public NetData data;
        public Fixed2 position { get { if (data != null) { return data.transform.Position; } else { return _position; } }
            set
            {
                _position = value;
            }
        }
        public FixedNumber rotation { get { if (data != null) { return data.transform.Rotation; } else { return new FixedNumber(); } } }
        protected Fixed2 _position=Fixed2.zero;
        
        public ShapBase()
        {

        }
        public ShapBase(Fixed2[] points)
        {
            _points = points;
            ResetSize();
        }
    
        public Fixed2 GetPoint(int index)
        {
            return _points[index].Rotate(rotation);
        }
        public Fixed2[] GetPoints()
        {
            return _points;
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
        /// <summary>
        /// 根据旋转计算新的包围盒大小 与 点的位置
        /// </summary>
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
        /// <summary>
        /// 给定两个凸体 给定迭代方向 该函数返回这两个凸体明可夫斯基差形状中的一个点
        /// </summary>
        /// <param name="direction">迭代方向</param>
        /// <returns>点</returns>
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
        /// <summary>
        /// 给定方向 给定两个凸体 该函数返回这两个凸体明可夫斯基差形状中的一个点
        /// </summary>
        /// <param name="a">形状a</param>
        /// <param name="b">形状b</param>
        /// <param name="direction">迭代方向</param>
        /// <returns>指定方向内的一点</returns>
        public static Fixed2 Support(ShapBase a, ShapBase b, Fixed2 direction)
        {
            Fixed2 p1 = a.Support(direction);
            Fixed2 p2 = b.Support(-direction);
            //Debug.Log("Support{ p1:" + p1 + "p2:" + p2 + "p3:" + (p1 - p2));
            return p1 - p2;
        }

    }

  
}
