using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace IDG
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
    public class RayShap : ShapBase
    {
        public static RayShap GetRay(Fixed2 origin, Fixed2 direction,FixedNumber length)
        {
            var shap = new RayShap(direction.normalized*length);
            shap._position = origin;
            return shap;
        }
        public RayShap ResetDirection(Fixed2 origin, Fixed2 direction,FixedNumber length)
        {
            position = origin;
            _points[1] = direction * length;
            ResetSize();
            return this;
        }

        public RayShap(Fixed2 direction)
        {
            Fixed2[] v2s = new Fixed2[2];
            v2s[0] = Fixed2.zero;
            v2s[1] = direction;
            Points = v2s;
        }
    }
}