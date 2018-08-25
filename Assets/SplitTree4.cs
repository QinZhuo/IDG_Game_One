using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using IDG.FightClient;
namespace IDG
{
    class Tree4
    {
        public static int MaxSize = 100;
        public static int MaxDepth=5;
        public static int SplitSize = 5;
        public Tree4Child child;
        public Tree4Border border;
        public Tree4Brother brother;
        public List<NetInfo> objs;
       
        public int depth;
        //bool isLeaf = false;
        public int size;
        public Tree4()
        {
            objs = new List<NetInfo>(SplitSize+1);
            
            size = MaxSize / 2;
            border = new Tree4Border(new V2(), MaxSize / 2);
            depth = 0;
        }
        public Tree4(Tree4Border border)
        {

        }
        public void Split()
        {
            child = new Tree4Child(border, brother);
            Debug.Log(1);
        }
        public void Add(NetInfo obj)
        {
            if (child == null)
            {
                objs.Add(obj);
                if (objs.Count > SplitSize)
                {
                    Split();
                }
            }
            else
            {
                child.Add(obj);
            }
         
        }
        public void AddChild(NetInfo obj)
        {
            if (obj.Right>border.center.x)
            {
                if (obj.Down > border.center.y)
                {
                    child.LeftUp.Add(obj);
                }
                else if(obj.Down < border.center.y)
                {

                }
            }
        }
        //public Tree4()
        //{
        //    objs = new List<object>(SplitSize);
        //}
        // List<object>
    }
    class Tree4Child
    {
        public Tree4 LeftUp{ get { return trees[(int)Pos.LeftUp]; } }
        public Tree4 LeftDown { get { return trees[(int)Pos.LeftDown]; } }
        public Tree4 RightUp { get { return trees[(int)Pos.RightUp]; } }
        public Tree4 RightDown { get { return trees[(int)Pos.RightDown]; } }

        private Tree4[] trees;
        public Tree4Child(Tree4Border border,Tree4Brother brother)
        {
            Tree4Border[] borders= border.Split();
            for (int i = 0; i < 4; i++)
            {
                trees[i] = new Tree4(borders[i]);
            }
            trees[(int)Pos.LeftUp].brother = new Tree4Brother(brother.Left,RightUp,brother.Up,LeftDown);
            trees[(int)Pos.LeftDown].brother = new Tree4Brother(brother.Left, RightDown,LeftUp, brother.Down);
            trees[(int)Pos.RightUp].brother = new Tree4Brother(LeftUp, brother.Right, brother.Up, RightDown);
            trees[(int)Pos.RightDown].brother = new Tree4Brother(LeftDown, brother.Right, RightUp, brother.Down);
        }
        
    }
    class Tree4Brother
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
    }
    enum Dir:byte
    {
        Left=0,
        Right=1,
        Up=2,
        Down=4,
    }
    enum Pos: byte
    {
        LeftUp=0,
        LeftDown=1,
        RightUp=2,
        RightDown=0,
    }
    class Tree4Border
    {
        public Ratio Left { get { return borders[(int)Dir.Left]; } }
        public Ratio Right { get { return borders[(int)Dir.Right]; } }
        public Ratio Up { get { return borders[(int)Dir.Up]; } }
        public Ratio Down { get { return borders[(int)Dir.Down]; } }

        //public Tree4Border(Ratio Left,Ratio Right, Ratio Up,Ratio Down)
        //{
        public V2 center;
        //}
        Ratio[] borders;
        public Tree4Border(V2 center,int size)
        {
            borders = new Ratio[4];
            this.center = center;
            borders[(int)Dir.Left] = center.x - size;
            borders[(int)Dir.Right] = center.x + size;
            borders[(int)Dir.Up] = center.y + size;
            borders[(int)Dir.Down] = center.y - size;
        }
        public Tree4Border(Ratio left, Ratio right, Ratio up,Ratio down)
        {
            borders = new Ratio[4];
            borders[(int)Dir.Left] = left;
            borders[(int)Dir.Right] = right;
            borders[(int)Dir.Up] = up;
            borders[(int)Dir.Down] = down;
        }
        
        public Tree4Border[] Split()
        {
            Tree4Border[] bs = new Tree4Border[4];
            bs[(int)Pos.LeftUp] = new Tree4Border(Left, center.x, Up, center.y);
            bs[(int)Pos.LeftDown] = new Tree4Border(Left, center.x, center.y,Down);
            bs[(int)Pos.RightUp] = new Tree4Border(center.x, Right, Up, center.y);
            bs[(int)Pos.RightDown] = new Tree4Border(center.x,Right , center.y, Down);
            return bs;
        }
    }
    //enum Tree4NodeType
    //{
        
    //}
    
}
