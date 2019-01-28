using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using IDG.FSClient;
namespace IDG
{
    /// <summary>
    /// 四叉树 用于空间分割 优化2D碰撞检测速度
    /// </summary>
    public class Tree4
    {
        public static int MaxSize = 100;
        /// <summary>
        /// 四叉树最大深度
        /// </summary>
        public static int MaxDepth=5;
        /// <summary>
        /// 空间中物体数多余该值时进行空间分割
        /// </summary>
        public static int SplitSize = 5;
        /// <summary>
        /// 孩子节点
        /// </summary>
        public Tree4Child child;
        /// <summary>
        /// 边界
        /// </summary>
        public Tree4Border border;
        /// <summary>
        /// 邻居节点
        /// </summary>
        public Tree4Brother brother;
        /// <summary>
        /// 空间中物体列表
        /// </summary>
        public List<NetData> objs;
        /// <summary>
        /// 树根
        /// </summary>
        public static Tree4 root;
        /// <summary>
        /// 碰撞信息
        /// </summary>
        public CollisonInfo collisonInfo;
        /// <summary>
        /// 当前节点深度
        /// </summary>
        public int depth;
        /// <summary>
        /// 被激活的树节点列表
        /// </summary>
        public static List<Tree4> activeTreeList=new List<Tree4>();
        //bool isLeaf = false;
       // public int size;
        public static void CheckTree()
        {
            foreach (var tree in activeTreeList)
            {
                Check(tree);
            }
        }

        public static void Check(Tree4 tree)
        {
            if (!tree.collisonInfo.active ) return ;
            
            int count = tree.objs.Count;
            var objs = tree.objs;
            for (int i = 0; i < count; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    if (objs[i] != objs[j] && ShapPhysics.Check(objs[i], objs[j]))
                    {
                        objs[i].collisonDatas.Add(objs[j]);
                        objs[j].collisonDatas.Add(objs[i]);
                    }
                }

            }
            //lastCheckTime = InputCenter.Time;
            tree.collisonInfo.active = false;
           
        }


        public Tree4()
        {
            objs = new List<NetData>(SplitSize+1);
            root = this;
            //size = MaxSize;
            border = new Tree4Border(new Fixed2(0,0), new FixedNumber(MaxSize));
            brother = new Tree4Brother();
            collisonInfo = new CollisonInfo();
            depth = 0;
        }
        public Tree4(int depth, Tree4Border border)
        {
            this.depth = depth;
            objs = new List<NetData>(SplitSize + 1);
            collisonInfo = new CollisonInfo();
            this.border = border;
        }
        public void Split()
        {
            child = new Tree4Child(depth+1,border, brother);
            while (objs.Count>0)
            {
                child.Add(objs[0]);
                //AddChild(objs[0]);
                DisLink(objs[0]);
            }
            //Debug.Log(1);
        }
        public void Add(NetData obj)
        {
            if (!IsIn(obj)) return;
            if (objs.Contains(obj)) return;
            if (child == null)
            {
                Link(obj);
                if (objs.Count > SplitSize&&depth<=MaxDepth)
                {
                    Split();
                } 
            }
            else
            {
                child.Add(obj);
            }
        }
        private void Link(NetData obj)
        {
            
            objs.Add(obj);
            obj.trees.Add(this);
        }
        public static void Remove(NetData obj)
        {
            Tree4[] trees = obj.trees.ToArray();
            foreach (var item in trees)
            {
                item.DisLink(obj);
            }
        }
        private void DisLink(NetData obj)
        {
            if (objs.Contains(obj))
            {
                objs.Remove(obj);
            }
            if (obj.trees.Contains(this))
            {
                obj.trees.Remove(this);
            }
        }
        public static bool BoxCheck(NetData objA,NetData objB)
        {
            if (FixedNumber.Abs( (objA.Position.x-objB.Position.x))<(objA.Width+objB.Width)/2
                &&
                FixedNumber.Abs((objA.Position.y - objB.Position.y)) < (objA.Height + objB.Height) / 2
                )
            {
                return true;
            }
            return false;
        }
        public static void SetActive(NetData obj)
        {
            foreach (var item in obj.trees)
            {
                item.collisonInfo.active = true;
                activeTreeList.Add(item);
            }
        }
        public static void Move(NetData obj)
        {
            
            Tree4[] trees = obj.trees.ToArray();
            foreach (var item in trees)
            {
                item.SubMove(obj);
               
            }
        }
        public void SubMove(NetData obj)
        {
            if (!IsIn(obj))
            {
                DisLink(obj);
            }
            brother.Add(obj);
            //root.Add(obj);
        }
        public bool IsIn(NetData obj)
        {
            if(( (border.center.x-obj.Position.x).Abs()<=(border.size+obj.Width/2))
                &&
                ((border.center.y - obj.Position.y).Abs() <= (border.size + obj.Height/2))
                )
            {
                return true;
            }
            return false;
        }
        //public void AddChild(NetInfo obj)
        //{
        //    child.LeftUp.Add(obj);
        //    child.LeftDown.Add(obj);
        //    child.RightUp.Add(obj);
        //    child.RightDown.Add(obj);
        //    //if (obj.Left <= border.center.x)
        //    //{
        //    //    if (obj.Up >= border.center.y)
        //    //    {
        //    //        child.LeftUp.Add(obj);
        //    //    }
        //    //}

        //    //if (obj.Left <= border.center.x)
        //    //{
        //    //    if (obj.Down <= border.center.y)
        //    //    {
        //    //        child.LeftDown.Add(obj);
        //    //    }
        //    //}
        //    //if (obj.Right >= border.center.x)
        //    //{
        //    //    if (obj.Up >= border.center.y)
        //    //    {
        //    //        child.RightUp.Add(obj);
        //    //    }
        //    //}
        //    //if (obj.Right >= border.center.x)
        //    //{
        //    //    if (obj.Down <= border.center.y)
        //    //    {
        //    //        child.RightDown.Add(obj);
        //    //    }
        //    //}



        //}
        //public Tree4()
        //{
        //    objs = new List<object>(SplitSize);
        //}
        // List<object>
    }
    public class Tree4Child
    {
        public Tree4 LeftUp{ get { return trees[(int)Pos.LeftUp]; } }
        public Tree4 LeftDown { get { return trees[(int)Pos.LeftDown]; } }
        public Tree4 RightUp { get { return trees[(int)Pos.RightUp]; } }
        public Tree4 RightDown { get { return trees[(int)Pos.RightDown]; } }

        private Tree4[] trees;
        public Tree4Child(int depth,Tree4Border border,Tree4Brother brother)
        {
            trees = new Tree4[4];
            Tree4Border[] borders= border.Split();
            for (int i = 0; i < 4; i++)
            {
                trees[i] = new Tree4(depth,borders[i]);
            }
            trees[(int)Pos.LeftUp].brother = new Tree4Brother(brother.Left,RightUp,brother.Up,LeftDown);
            trees[(int)Pos.LeftDown].brother = new Tree4Brother(brother.Left, RightDown,LeftUp, brother.Down);
            trees[(int)Pos.RightUp].brother = new Tree4Brother(LeftUp, brother.Right, brother.Up, RightDown);
            trees[(int)Pos.RightDown].brother = new Tree4Brother(LeftDown, brother.Right, RightUp, brother.Down);
        }
        public void Add(NetData obj)
        {
            for (int i = 0; i < 4; i++)
            {
                trees[i].Add(obj);
            }
        }
        
    }
    public class Tree4Brother
    {
        public Tree4 Left { get { return brothers[(int)Dir.Left]; } }
        public Tree4 Right { get { return brothers[(int)Dir.Right]; } }
        public Tree4 Up { get { return brothers[(int)Dir.Up]; } }
        public Tree4 Down { get { return brothers[(int)Dir.Down]; } }
        private Tree4[] brothers;
        public Tree4Brother[] Split()
        {
            Tree4Brother[] brothers = new Tree4Brother[4];
            //foreach (var item in collection)
            //{

            //}
            return brothers;
        }
        public Tree4Brother()
        {
            brothers = new Tree4[4];
        }
        public Tree4Brother(Tree4 left, Tree4 right, Tree4 up, Tree4 down)
        {
            brothers = new Tree4[4];
            brothers[(int)Dir.Left] = left;
            brothers[(int)Dir.Right] = right;
            brothers[(int)Dir.Up] = up;
            brothers[(int)Dir.Down] = down;
        }
        public void Add(NetData obj)
        {
         
            for (int i = 0; i < 4; i++)
            {
                if (brothers[i] != null)
                {
                    brothers[i].Add(obj);
                }
             
                
            }
        }
    }
    enum Dir:byte
    {
        Left=0,
        Right=1,
        Up=2,
        Down=3,
    }
    enum Pos: byte
    {
        LeftUp=0,
        LeftDown=1,
        RightUp=2,
        RightDown=3,
    }
    public class Tree4Border
    {
        //public Ratio Left { get { return borders[(int)Dir.Left]; } }
        //public Ratio Right { get { return borders[(int)Dir.Right]; } }
        //public Ratio Up { get { return borders[(int)Dir.Up]; } }
        //public Ratio Down { get { return borders[(int)Dir.Down]; } }

        public FixedNumber size;
        public Fixed2 center;
        
        //Ratio[] borders;
        public Tree4Border(Fixed2 center,FixedNumber size)
        {
            //borders = new Ratio[4];
            this.center = center;
            this.size = size;
            //borders[(int)Dir.Left] = center.x - size;
            //borders[(int)Dir.Right] = center.x + size;
            //borders[(int)Dir.Up] = center.y + size;
            //borders[(int)Dir.Down] = center.y - size;
        }
        //public Tree4Border(Ratio left, Ratio right, Ratio up,Ratio down)
        //{
        //    borders = new Ratio[4];
        //    borders[(int)Dir.Left] = left;
        //    borders[(int)Dir.Right] = right;
        //    borders[(int)Dir.Up] = up;
        //    borders[(int)Dir.Down] = down;
        //    center = new V2((left + right) / 2, (up+down) / 2);
        //}
        
        public Tree4Border[] Split()
        {
            Tree4Border[] bs = new Tree4Border[4];
            bs[(int)Pos.LeftUp] = new Tree4Border(new Fixed2(center.x-size / 2 , size/2+center.y),size/2);
            bs[(int)Pos.LeftDown] = new Tree4Border(new Fixed2(center.x - size / 2,  center.y- size / 2), size / 2);
            bs[(int)Pos.RightUp] = new Tree4Border(new Fixed2(center.x + size / 2, center.y + size / 2), size / 2);
            bs[(int)Pos.RightDown] = new Tree4Border(new Fixed2(center.x + size / 2, center.y - size / 2), size / 2);
            return bs;
        }
    }
    //enum Tree4NodeType
    //{
        
    //}
    
}
