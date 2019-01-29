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
}
