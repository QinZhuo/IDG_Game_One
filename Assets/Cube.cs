using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IDG;
using IDG.FightClient;
public class Cube : NetObject {
    // public NetInfo net;
    static int cubuid=1;
    
    private void Awake()
    {
        ShapPhysics.Init();
        //Debug.Log("init");
       // ShapBase a = new TestShap(true);
       // ShapBase b = new TestShap(false);
        //Debug.Log("testShap Sport:" + ShapBase.Support(a, b, new V2(1, 0)));
        //Debug.Log("testShap Sport:" + ShapBase.Support(a, b, new V2(-1, 0)));
        //Debug.Log("testShap Sport:" + ShapBase.Support(a, b, new V2(0, 1)));
        //Debug.Log("testShap Sport:" + ShapBase.Support(a, b, new V2(0, -1)));
        //ShapPhysics.GJKCheck(a, b);
        //Debug.Log("ABxAOxAB2:" + (new V2(-12, -4) * new V2(8, 2)) * new V2(-12, -4));
    }
    private void Start()
    {
        name = "cube" + cubuid++;
        //ShapPhysics.tree.Add(net);
        net.Position = new V2(transform.position.x, transform.position.z);
        if(net.Shap==null) net.Shap = new BoxShap(new Ratio(1),new Ratio(1));
        net.name = name;
        if (net.ClientId >= 0)
        {
           
            net.Input.framUpdate += FrameUpdate;
        }

        
    }
    //float last;
    protected void FrameUpdate()
    {
        // Debug.Log("frameTime:" + Time.time);
        // last = Time.time;
        //V2 move = new V2();
        V2 move = net.Input.GetJoyStickDirection(FrameKey.MoveKey);
       // Debug.Log(move);
        //if (net.Input.GetKey(FrameKey.Left))
        //{
        //    move += (V2.left);
        //}
        //if (net.Input.GetKey(FrameKey.Up))
        //{

        //    move += (V2.up);
        //    // net.Rotation += new Ratio(10) ;
        //}
        //if (net.Input.GetKey(FrameKey.Right))
        //{

        //    move += (V2.right);
        //}
        //if (net.Input.GetKey(FrameKey.Down))
        //{
        //    //net.Position -= (V2.up * net.deltaTime);
        //    move += (V2.down);
        //}

        net.Position += move * net.deltaTime;
        if (move.x != 0 || move.y != 0)
        {
            net.Rotation = move.ToRotation();
        }
        //Debug.Log(net.Position);
    }

    // Use this for initialization
   
	
	// Update is called once per frame
	//void Update () {
		
	//}

}
