using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IDG;
using IDG.FSClient;
public class ZombieShow : NetObjectView<ZombieData> {
    
}


public class ZombieData : HealthData
{
    // protected GunBase gun;
   
    public override void Start()
    {
        this.tag = "Player";
        
     
       // gun = new GunBase();
       // gun.Init(2, this);
    }
    protected override void FrameUpdate()
    {
      
        
        // Debug.Log("FrameUpdate"+ Position+":"+ Input.GetJoyStickDirection(FrameKey.MoveKey));
    }
    protected override void Die()
    {
        base.Die();
        NetObjectManager.Destory<ZombieData>(show);
    }
    public override string PrefabPath()
    {
        return "Prefabs/Player";
    }
}

