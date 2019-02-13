using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IDG;
using IDG.FSClient;
public class ItemView : NetObjectView<ItemData> { 
    // Update is called once per frame
    //   void Update () {

    //}
    public SkillId skillId;
}
public class ItemData : NetData
{
    public NetData user;

    public override void Init()
    {
        base.Init();
        physics.enable = true;
        isTrigger = true;

    }
    protected override void FrameUpdate()
    {
        
        
    }
    public override void OnPhysicsCheckStay(NetData other)
    {

        if (other.tag == "Player" && other != user)
        {
            UnityEngine.Debug.Log("Stay触发Bullet！！！！");
        
        }
    }
    public override void OnPhysicsCheckEnter(NetData other)
    {
        if (other.tag == "Player" && other != user)
        {
            UnityEngine.Debug.Log("Enter触发Bullet！！！！");
            NetObjectManager.Destory<ItemData>(this.view);
            //var gun = new GunBase();
            //gun.Init(20, this);
            //(other as PlayerData).AddGun(gun);
           (other as PlayerData).skillList.AddSkill(ItemManager.GetSkill((this.view as ItemView).skillId));

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
        return "Prefabs/ItemView";
    }
}
