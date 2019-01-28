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
        protected GunSetting gunSetting;
        protected FixedNumber timer;
        public void Init(float firingRate,NetData User)
        {
            this.firingInterval = new FixedNumber(1/firingRate);
            lastTime = FixedNumber.Zero;
            gunSetting = DataManager.Instance.gunManager.gun;
            this.user = User;
            timer = FixedNumber.Zero;
        }
        public void Fire(Fixed2 position, FixedNumber rotation)
        {
            var t = InputCenter.Time - lastTime;
            if (t > gunSetting.fireRate)
            {


                timer+= (gunSetting.recoilTime+gunSetting.fireRate - t);
                if (timer <= 0)
                {
                    timer =new FixedNumber(0);
                }
                FixedNumber rote =new FixedNumber( gunSetting.recoilFrouceCurve.Evaluate(timer.ToFloat() )-1)*gunSetting.recoilScale;
                
                lastTime = InputCenter.Time;
                ShootBullet(position,rotation+ rote);
            }
            else
            {

            }
        }
        protected virtual void ShootBullet(Fixed2 position, FixedNumber rotation)
        {
            Bullet data = new Bullet();
            data.user = this.user;
            data.Init();
            data.Reset(position, rotation);
            NetData.Instantiate<Bullet>(data);
        } 
    }
}
