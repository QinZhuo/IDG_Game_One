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
        public static int SplitSize =10;
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
        public  List<Tree4> activeTreeList=new List<Tree4>();

        public  void CheckTree()
        {
          //  Debug.LogErrorFormat("activeTreeListCount : {0}",activeTreeList.Count);
            foreach (var tree in activeTreeList)
            {
                tree.DebugActiveTree();
                Check(tree);
                 
            }
            activeTreeList.Clear();
        }

        public void DebugActiveTree(){
            Color c=Color.magenta;
            c.a=0.4f;
            if(collisonInfo.active){    
                for (int x = -1; x <= 1; x++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        UnityEngine.Debug.DrawRay(border.center.ToVector3(),new Vector3(x,0,z)*border.size.ToFloat()*0.5f,c,0.2f);
                    }
                }    
            }
        }
        public List<NetData> CheckShap(ShapBase shap)
        {
            List<NetData> objs=new List<NetData>();
           var treeList=GetInTress(shap);
           foreach (var tree in treeList)
           {
                for (int i = 0; i < tree.objs.Count; i++)
                {
                    if (ShapPhysics.Check(shap, tree.objs[i].Shap)){
                        objs.Add(tree.objs[i]);
                    }
                }
           }
            return objs;
        }
        public List<Tree4> GetInTress(ShapBase shap)
        {
            List<Tree4> treeList = new List<Tree4>();
            if (child == null)
            {
                if (IsIn(shap))
                {
                    treeList.Add(this);
                }
            }
            else
            {
                treeList.AddRange(child.GetInTrees(shap));
            }
            return treeList;
        }
        static void Check(Tree4 tree)
        {
            if (!tree.collisonInfo.active ) return ;
            
            int count = tree.objs.Count;
            var objs = tree.objs;
            for (int i = 0; i < count; i++)
            {
                for (int j = i + 1; j < count; j++)
                {
                    if (objs[i] != objs[j]&&(objs[i].rigibody.useCheck||objs[j].rigibody.useCheck)&& ShapPhysics.Check(objs[i].Shap, objs[j].Shap))
                    {
                        objs[i].rigibody.collisonDatas.Add(objs[j]);
                        objs[j].rigibody.collisonDatas.Add(objs[i]);
                    }
                }

            }
            //lastCheckTime = InputCenter.Time;
            tree.collisonInfo.active = false;
           
        }


        public Tree4()
        {
            objs = new List<NetData>(SplitSize+1);
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
            if (!IsIn(obj.Shap)) return;
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
        
        public void SetActive(NetData obj)
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
            if (!IsIn(obj.Shap))
            {
                DisLink(obj);
            }
            brother.Add(obj);
            //root.Add(obj);
        }
        public bool IsIn(ShapBase shap)
        {
            if(( (border.center.x- shap.position.x).Abs()<=(border.size+shap.width/2))
                &&
                ((border.center.y - shap.position.y).Abs() <= (border.size + shap.height/2))
                )
            {
                return true;
            }
            return false;
        }
    }
    /// <summary>
    /// 四叉树子节点类
    /// </summary>
    public class Tree4Child
    {
        public Tree4 LeftUp{ get { return trees[(int)Pos.LeftUp]; } }
        public Tree4 LeftDown { get { return trees[(int)Pos.LeftDown]; } }
        public Tree4 RightUp { get { return trees[(int)Pos.RightUp]; } }
        public Tree4 RightDown { get { return trees[(int)Pos.RightDown]; } }

        private Tree4[] trees;
        /// <summary>
        /// 初始化节点信息
        /// </summary>
        /// <param name="depth">当前深度</param>
        /// <param name="border">边界类对象</param>
        /// <param name="brother">邻居类对象</param>
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

        public List<Tree4> GetInTrees(ShapBase shap)
        {
            List<Tree4> treeList = new List<Tree4>();
            foreach (var t in trees)
            {
                treeList.AddRange(t.GetInTress(shap));
            }
            return treeList;
        }
        /// <summary>
        /// 向子节点添加对象
        /// </summary>
        /// <param name="obj">添加的对象</param>
        public void Add(NetData obj)
        {
            for (int i = 0; i < 4; i++)
            {
                trees[i].Add(obj);
            }
        }
        
    }
    /// <summary>
    /// 四叉树邻居类
    /// </summary>
    public class Tree4Brother
    {
        public Tree4 Left { get { return brothers[(int)Dir.Left]; } }
        public Tree4 Right { get { return brothers[(int)Dir.Right]; } }
        public Tree4 Up { get { return brothers[(int)Dir.Up]; } }
        public Tree4 Down { get { return brothers[(int)Dir.Down]; } }
        private Tree4[] brothers;

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

        
        /// <summary>
        /// 向邻居节点添加对象
        /// </summary>
        /// <param name="obj">移动的对象</param>
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
    /// <summary>
    /// 方向枚举
    /// </summary>
    enum Dir:byte
    {
        Left=0,
        Right=1,
        Up=2,
        Down=3,
    }
    /// <summary>
    /// 方位枚举
    /// </summary>
    enum Pos: byte
    {
        LeftUp=0,
        LeftDown=1,
        RightUp=2,
        RightDown=3,
    }
    /// <summary>
    /// 四叉树边界类
    /// </summary>
    public class Tree4Border
    {
        /// <summary>
        /// 正方形边长
        /// </summary>
        public FixedNumber size;
        /// <summary>
        /// 中心点位置
        /// </summary>
        public Fixed2 center;

        public Tree4Border(Fixed2 center,FixedNumber size)
        {
            this.center = center;
            this.size = size;
        }
       

        /// <summary>
        /// 分裂节点 生成四个子节点
        /// </summary>
        /// <returns>四个子节点边界</returns>
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
    
}
