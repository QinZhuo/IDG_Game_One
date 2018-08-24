using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IDG;
using IDG.FightClient;
public class Cube : NetObject {
   // public NetInfo net;
    private void Start()
    {
        net.Position = new V2(transform.position.x, transform.position.z);

        if (net.ClientId >= 0)
        {
           
            net.Input.framUpdate += FrameUpdate;
        }
       
        
        net.Shap = new BoxShap(new Ratio(1,2));
    }
    //float last;
    protected void FrameUpdate()
    {
        Debug.Log("frameTime:" + Time.time);
       // last = Time.time;
        if (net.Input.GetKey(FrameKey.Left))
        {
            net.Position+= (V2.left*net.deltaTime);
        }
        if (net.Input.GetKey(FrameKey.Up))
        {
            net.Position += (V2.up * net.deltaTime);
        }
        if (net.Input.GetKey(FrameKey.Right))
        {
            net.Position += (V2.right * net.deltaTime);
        }
        if (net.Input.GetKey(FrameKey.Down))
        {
            net.Position += (V2.down * net.deltaTime);
        }
        Debug.Log(net.Position);
    }

    // Use this for initialization
   
	
	// Update is called once per frame
	//void Update () {
		
	//}

}
