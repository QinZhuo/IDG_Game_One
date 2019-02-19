using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDG;
using IDG.FSClient;

class SkillShoots:SkillBase
{
    public override void Init()
    {
        key = KeyNum.Skill1;
        time = new FixedNumber(0.7f);
        timer = new FixedNumber(0);
    }
   
           
   
    public override void UseOver()
    {
        base.UseOver();
       // UnityEngine.Debug.LogError("bulletUse" + data.Input.GetJoyStickDirection(key).ToRotation());
        for (int i = -30; i <= 30; i += 5)
        {
            ShootBullet(data.transform.Position,  data.Input.GetJoyStickDirection(key).ToRotation() + i);
        }
    }
    protected void ShootBullet(Fixed2 position, FixedNumber rotation)
    {
        Bullet bullet = new Bullet();
        bullet.user = data;
        bullet.Init(data.client);
        bullet.Reset(position, rotation);
        data.client.objectManager.Instantiate(bullet);
      //  UnityEngine.Debug.LogError("bullet" + rotation);
    }
}

class SkillGun:SkillBase
{
    GunBase gun;
    public override void Init()
    {
        key = KeyNum.Skill1;
        time = new FixedNumber(0.7f);
        timer = new FixedNumber(0);
        gun = new GunBase();
        gun.Init(20, data);
    
    }
   
    
   
    public override void StayUse()
    {
        var rot=data.Input.GetJoyStickDirection(key);
        if(gun!=null)  gun.Fire(data,rot.ToRotation() );  
     
    }
    protected void ShootBullet(Fixed2 position, FixedNumber rotation)
    {
        Bullet bullet = new Bullet();
        bullet.user = data;
        bullet.Init(data .client);
        bullet.Reset(position, rotation);
        data.client.objectManager.Instantiate(bullet);
      //  UnityEngine.Debug.LogError("bullet" + rotation);
    }
}
