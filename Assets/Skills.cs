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
   
           
   
    public override void Use()
    {
        base.Use();
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
        bullet.Init();
        bullet.Reset(position, rotation);
        NetObjectManager.Instantiate<Bullet>(bullet);
      //  UnityEngine.Debug.LogError("bullet" + rotation);
    }
}

