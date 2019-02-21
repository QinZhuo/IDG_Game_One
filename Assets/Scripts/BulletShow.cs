using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IDG;
using IDG.FSClient;
public class BulletShow : NetObjectView<Bullet> {
    public static int id=0;

    

  
 

    
    // Update is called once per frame
    //   void Update () {

    //}
}
public class Bullet : NetData
{
    public NetData user;
    public FixedNumber startTime;
    public override void Init(FSClient client)
    {
        base.Init(client);
        rigibody.useCheckCallBack = true;
        isTrigger = true;
        startTime = client.inputCenter.Time;
    }
    public override void Start()
    {
        Shap = new BoxShap(new FixedNumber(2), new FixedNumber(0.3f));
    }
    protected override void FrameUpdate()
    {
        
        transform.Position += transform.forward * (deltaTime * 10f);
        // Debug.Log("bullet" + Position);
        if (client.inputCenter.Time - startTime > 3)
        {
            client.objectManager.Destory(this.view);
        }
    }
    public override void OnPhysicsCheckStay(NetData other)
    {

        if (other.tag == "Player" && other != user)
        {
            UnityEngine.Debug.Log("Stay触发Bullet！！！！");
            //Destory<Bullet>(this.show);
        }
    }
    public override void OnPhysicsCheckEnter(NetData other)
    {
        if (other is HealthData && other != user)
        {
            UnityEngine.Debug.Log("Enter触发Bullet！！！！");
             client.objectManager.Destory(this.view);
            (other as HealthData).GetHurt(new FixedNumber(10));
        }
    }
    public override void OnPhysicsCheckExit(NetData other)
    {
        if (other.tag=="Player" && other != user)
        {
            UnityEngine.Debug.Log("Exit触发Bullet！！！！");
            //Destory<Bullet>(this.show);
        }
    }
    public override string PrefabPath()
    {
        return "Prefabs/Bullet";
    }
}
