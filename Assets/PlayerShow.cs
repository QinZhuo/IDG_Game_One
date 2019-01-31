using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IDG;
using IDG.FSClient;
public class PlayerShow : NetObjectView<PlayerData> {
    // public NetInfo net;
    public int clientId = -1;
    static int playerUnitId = 1;
    public NetObjectView<PlayerData> netPrefab;
    public Animator anim;
    private new void Start()
    {
        base.Start();
        data.ClientId = clientId;
        anim.SetInteger("WeaponType", 1);
    }
    
    //float last;



    // Use this for initialization


    // Update is called once per frame
    //void Update () {

    //}
}
public abstract class HealthData : NetData
{
    protected FixedNumber _m_Hp=new FixedNumber(100);
    protected bool isDead=false;
    public virtual void GetHurt(FixedNumber atk)
    {
        if (!isDead)
        {
            _m_Hp -= atk;
            Debug.Log(this.name + " GetHurt "+atk+" Hp:"+_m_Hp);
            if (_m_Hp <= 0)
            {
                Die();
            }
        }
        
    }
    protected virtual void Die()
    {
        isDead = true;
        Debug.Log(this.name + "dead!!!");
    }
}
public class PlayerData: HealthData
{
    protected GunBase gun;
    public SkillList skillList;
    public override void Start()
    {
        this.tag = "Player";
        skillList= AddCommponent<SkillList>();
       
        
        
     
        if (IsLocalPlayer)
        {
            FightClientForUnity3D.Instance.playerData = this;
        }

    }
    protected override void FrameUpdate()
    {

       Fixed2 move = Input.GetJoyStickDirection(IDG.KeyNum.MoveKey);

        transform.Position += move * deltaTime;
        if (move.x != 0 || move.y != 0)
        {
            transform.Rotation = move.ToRotation();
        }
        if (Input.GetKeyUp(IDG.KeyNum.Attack))
        {
           if(gun!=null)  gun.Fire(transform.Position + transform.forward, transform.Rotation);
        
        }
  
    }
    public void AddGun(GunBase gun)
    {
        this.gun = gun;
    }

    public override string PrefabPath()
    {
        return "Prefabs/Player";
    }
}

