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
    public ShapBase findShap;
    public override void Start()
    {
        this.tag = "Zombie";
        Shap = new CircleShap(new FixedNumber(0.5f), 8);
        findShap = new CircleShap(new FixedNumber(3), 10);
        // gun = new GunBase();
         rigibody.useCheck=true;
        // gun.Init(2, this);
    }
    protected override void FrameUpdate()
    {
        // var others = client.physics.OverlapShap(findShap, transform.Position);

        // foreach (var other in others)
        // {
        //     if (other.tag == "Player")
        //     {
        //         transform.Position += ((other.transform.Position - transform.Position).normalized) * new FixedNumber(0.1f);
        //         break;
        //     }
        // }

    
      
        
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

