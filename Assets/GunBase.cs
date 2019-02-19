using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IDG;

namespace IDG.FSClient
{
    public class GunBase:ItemBase
    {   
        protected FixedNumber firingInterval;
        protected FixedNumber lastTime;
       // protected GunSetting gunSetting;
        protected FixedNumber timer;
        public void Init(float firingRate,NetData User)
        {
            this.firingInterval = new FixedNumber(1/firingRate);
            lastTime = FixedNumber.Zero;
           // gunSetting = DataManager.Instance.gunManager.gun;
            this.user = User;
            timer = FixedNumber.Zero;
        }
        public void Fire(NetData user, FixedNumber rotation)
        {
            var t =  user.client.inputCenter.Time - lastTime;
            if (t > 0.1f)
            {
             
                FixedNumber rote =new FixedNumber(0);
                
                lastTime = user.client.inputCenter.Time;
                ShootBullet(user.transform.Position,rotation+ rote);
            }
            else
            {

            }
        }
        protected virtual void ShootBullet(Fixed2 position, FixedNumber rotation)
        {
            Bullet data = new Bullet();
            data.user = this.user;
            data.Init(user.client);
            data.Reset(position, rotation);
            user.client.objectManager.Instantiate(data);
        } 
    }
}
