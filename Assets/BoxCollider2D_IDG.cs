using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace IDG.FightClient
{


   
    public class BoxCollider2D_IDG : Collider2DBase_IDG
    {
        public int x;
        public int y;

        public override ShapBase GetShap()
        {
            return new BoxShap(new Ratio(x), new Ratio(y));
        }
        
       

        // Update is called once per frame
        void Update()
        {

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
}
