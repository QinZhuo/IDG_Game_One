using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace IDG.FightClient
{


    [RequireComponent(typeof(NetObject))]
    public class BoxCollider2D_IDG : MonoBehaviour
    {
        public int x;
        public int y;
        // Use this for initialization
        void Start()
        {
            GetComponent<NetObject>().net.Shap = new BoxShap(new Ratio(x), new Ratio(y));
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
