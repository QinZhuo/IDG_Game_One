using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IDG;
using IDG.FSClient;
public class ItemView : NetObjectView<ItemData> { 

}
public class ItemData : NetData
{
    public NetData user;

    public override void Init(FSClient client)
    {
        base.Init(client);
        rigibody.useCheckCallBack = true;
        isTrigger = true;

    }
    public override void Start()
    {
        Shap = new BoxShap(new FixedNumber(1), new FixedNumber(1));
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
            client.objectManager.Destory(this.view);
            //var gun = new GunBase();
            //gun.Init(20, this);
            //(other as PlayerData).AddGun(gun);
           (other as PlayerData).skillList.AddSkill(ItemManager.GetSkill((SkillId)client.random.Range(3)));

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
