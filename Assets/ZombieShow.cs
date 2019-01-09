using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IDG;
using IDG.FightClient;
public class ZombieShow : NetObjectShow<ZombieData> {
    
}

public class EnemyTarget : NetData
{
    protected NetData _m_enemy;
    public NetData Enemy
    {
        get
        {
            return _m_enemy;
        }
    }
    public void SetTargetFindMode(Ratio r)
    {
        Shap = new CircleShap(r,8);
        usePhysicsCheck = true;
        isTrigger = true;
        base.Init();
        base.Start();
    }
    protected override void FrameUpdate()
    {
        
    }
    public override void OnPhysicsCheckEnter(NetData other)
    {
        if (other.tag == "Player"&&other!=parent)
        {
            _m_enemy = other;
        }
    }
    protected override string PrefabPath()
    {
        return "";
    }
}
public class ZombieData : HealthData
{
    // protected GunBase gun;
    public EnemyTarget target;
    public override void Start()
    {
        this.tag = "Player";
        
        target = new EnemyTarget();
        target.parent = this;
        target.SetTargetFindMode(new Ratio(3));
       // gun = new GunBase();
       // gun.Init(2, this);
    }
    protected override void FrameUpdate()
    {
        if (target.Enemy != null)
        {
            var dir = target.Enemy.Position - Position;
            Position += dir * deltaTime*new Ratio(0.3f);
           // Debug.Log("zombiePos" + Position+"dir"+dir);
        }
        
        // Debug.Log("FrameUpdate"+ Position+":"+ Input.GetJoyStickDirection(FrameKey.MoveKey));
    }
    protected override void Die()
    {
        base.Die();
        target.Destory();
        Destory<ZombieData>(show);
    }
    protected override string PrefabPath()
    {
        return "Prefabs/Player";
    }
}

