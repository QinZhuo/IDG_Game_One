using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using IDG.FightClient;
namespace IDG
{
    public class Tree4
    {
        public static int MaxSize = 100;
        public static int MaxDepth=5;
        public static int SplitSize = 5;
        public Tree4Child child;
        public Tree4Border border;
        public Tree4Brother brother;
        public List<NetInfo> objs;
        public static Tree4 root;
        public int depth;
        //bool isLeaf = false;
       // public int size;
        public Tree4()
        {
            objs = new List<NetInfo>(SplitSize+1);
            root = this;
            //size = MaxSize;
            border = new Tree4Border(new V2(0,0), new Ratio(MaxSize,1));
            brother = new Tree4Brother();
            depth = 0;
        }
        public Tree4(int depth, Tree4Border border)
        {
            this.depth = depth;
            objs = new List<NetInfo>(SplitSize + 1);
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
        public void Add(NetInfo obj)
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
        private void Link(NetInfo obj)
        {
            objs.Add(obj);
            obj.trees.Add(this);
        }
        private void DisLink(NetInfo obj)
        {
            objs.Remove(obj);
            obj.trees.Remove(this);
        }
        public static bool BoxCheck(NetInfo objA,NetInfo objB)
        {
            if ((objA.Position.x-objB.Position.x).Abs()<(objA.Width+objB.Width)/2
                &&
                (objA.Position.y - objB.Position.y).Abs() < (objA.Height + objB.Height) / 2
                )
            {
                return true;
            }
            return false;
        }
        public static void Move(NetInfo obj)
        {
            Tree4[] trees = obj.trees.ToArray();
            foreach (var item in trees)
            {
                item.SubMove(obj);
                
            }
        }
        public void SubMove(NetInfo obj)
        {
            if (!IsIn(obj))
            {
                DisLink(obj);
            }
            brother.Add(obj);
            //root.Add(obj);
        }
        public bool IsIn(NetInfo obj)
        {
            if(((border.center.x-obj.Position.x).Abs()<(border.size+obj.Width/2))
                &&
                ((border.center.y - obj.Position.y).Abs() < (border.size + obj.Height/2))
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
        public void Add(NetInfo obj)
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
        public void Add(NetInfo obj)
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

        public Ratio size;
        public V2 center;
        
        //Ratio[] borders;
        public Tree4Border(V2 center,Ratio size)
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
            bs[(int)Pos.LeftUp] = new Tree4Border(new V2(center.x-size / 2 , size/2+center.y),size/2);
            bs[(int)Pos.LeftDown] = new Tree4Border(new V2(center.x - size / 2,  center.y- size / 2), size / 2);
            bs[(int)Pos.RightUp] = new Tree4Border(new V2(center.x + size / 2, center.y + size / 2), size / 2);
            bs[(int)Pos.RightDown] = new Tree4Border(new V2(center.x + size / 2, center.y - size / 2), size / 2);
            return bs;
        }
    }
    //enum Tree4NodeType
    //{
        
    //}
    
}
