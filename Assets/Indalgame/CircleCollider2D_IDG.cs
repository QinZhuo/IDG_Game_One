using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace IDG.FSClient
{

    public abstract class Collider2DBase_IDG:MonoBehaviour
    {
        public abstract ShapBase GetShap();
    }
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
    public class CircleCollider2D_IDG : Collider2DBase_IDG
    {

        public float r=1;
        public int num=8;

        public override ShapBase GetShap()
        {
            return new CircleShap(new FixedNumber(r), num);
        }

        // Use this for initialization
        //void Start()
        //{
        //    GetComponent<NetObjectShow>().data.Shap = new CircleShap(new Ratio(r), num);
        //}

        // Update is called once per frame

        
    }
}
