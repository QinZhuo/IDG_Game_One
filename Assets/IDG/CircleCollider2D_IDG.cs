using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace IDG.FSClient
{

  
    
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
