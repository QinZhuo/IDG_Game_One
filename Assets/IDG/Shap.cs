using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace IDG.FSClient
{
    public class CircleShap : ShapBase
    {
        public CircleShap(FixedNumber r, int num)
        {

            FixedNumber t360 = new FixedNumber(360);
            FixedNumber tmp = t360 / num;
            Fixed2[] v2s = new Fixed2[num];
            int i = 0;
            for (FixedNumber tr = new FixedNumber(0); tr < t360 && i < num; tr += tmp, i++)
            {
                v2s[i] = Fixed2.Parse(tr) * r;
            }

            Points = v2s;
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
    public abstract class Collider2DBase_IDG:MonoBehaviour
    {
        public abstract ShapBase GetShap();
    }
}