using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IDG;
using IDG.FightClient;
public class Bullet : NetObject {
    public static int id=0;

    protected override void FrameUpdate()
    {
    
        net.Position +=net.forward* (net.deltaTime * 10f);
    }

    //   // Use this for initialization
    protected new void  Start()
    {
        base.Start();
        name = "bullet" + id++;
        net.isTrigger = true;
    }
    public override PoolType GetPoolType()
    {
        return PoolType.bullet;
    }
    // Update is called once per frame
    //   void Update () {

    //}
}
