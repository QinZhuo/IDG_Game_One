using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace IDG.FSClient
{


   
    public class BoxCollider2D_IDG : Collider2DBase_IDG
    {
        public float x;
        public float y;

        public override ShapBase GetShap()
        {
            return new BoxShap(new FixedNumber(x), new FixedNumber(y));
        }
        
     
    }
    public class BoxShap : ShapBase
    {

        public BoxShap(FixedNumber x, FixedNumber y)
        {
            Fixed2[] v2s = new Fixed2[4];
            v2s[0] = new Fixed2(x / 2, y / 2);
            v2s[1] = new Fixed2(-x / 2, y / 2);
            v2s[2] = new Fixed2(x / 2, -y / 2);
            v2s[3] = new Fixed2(-x / 2, -y / 2);
            Points = v2s;
        }
    }
}
