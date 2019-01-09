using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IDG;
using IDG.FightClient;
public class BulletShow : NetObjectShow<Bullet> {
    public static int id=0;

    

  
 

    
    // Update is called once per frame
    //   void Update () {

    //}
}
public class Bullet : NetData
{
    public NetData user;
    public Ratio startTime;
    public override void Init()
    {
        base.Init();
        usePhysicsCheck = true;
        isTrigger = true;
        startTime = InputCenter.Time;
    }
    protected override void FrameUpdate()
    {
        
        Position += forward * (deltaTime * 10f);
        // Debug.Log("bullet" + Position);
        if (InputCenter.Time - startTime > 3)
        {
            Destory<Bullet>(this.show);
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
        if (other.tag == "Player" && other != user)
        {
            UnityEngine.Debug.Log("Enter触发Bullet！！！！");
            Destory<Bullet>(this.show);
            (other as HealthData).GetHurt(new Ratio(10));
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
    protected override string PrefabPath()
    {
        return "Prefabs/Bullet";
    }
}
