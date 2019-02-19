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
        Shap = new CircleShap(new FixedNumber(0.5f), 8);

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
        client.objectManager.Destory(view);
    }
    public override string PrefabPath()
    {
        return "Prefabs/Zombie";
    }
}

