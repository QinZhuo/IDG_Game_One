using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    class Tree4
    {
        public static int MaxDepth=5;
        public static int SplitSize = 5;
        public Tree4 LU;//leftUpTree
        public Tree4 RUT;
        public Tree4 LDT;
        public Tree4 RDT;

        bool isLeaf = false;
       // List<object>
    }
    class Tree4Child
    {
        Tree4 LeftUp;
        Tree4 LeftDown;
        Tree4 RightUp;
        Tree4 RightDown;
    }
    class Tree4Brother
    {
        Tree4 Left;
        Tree4 Right;
        Tree4 Up;
        Tree4 Down;
    }
    class Tree4Border
    {
        int Left;
        int Right;
        int Up;
        int Down;
    }
}
