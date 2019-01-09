using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IDG;

namespace IDG.FightClient
{
    public class GunBase:ItemBase
    {   
        protected Ratio firingInterval;
        protected Ratio lastTime;
        protected GunSetting gunSetting;
        protected Ratio timer;
        public void Init(float firingRate,NetData User)
        {
            this.firingInterval = new Ratio(1/firingRate);
            lastTime = Ratio.Zero;
            gunSetting = DataManager.Instance.gunManager.gun;
            this.user = User;
            timer = Ratio.Zero;
        }
        public void Fire(V2 position, Ratio rotation)
        {
            var t = InputCenter.Time - lastTime;
            if (t > gunSetting.fireRate)
            {


                timer+= (gunSetting.recoilTime+gunSetting.fireRate - t);
                if (timer <= 0)
                {
                    timer =new Ratio(0);
                }
                Ratio rote =new Ratio( gunSetting.recoilFrouceCurve.Evaluate(timer.ToFloat() )-1)*gunSetting.recoilScale;
                
                lastTime = InputCenter.Time;
                ShootBullet(position,rotation+ rote);
            }
            else
            {

            }
        }
        protected virtual void ShootBullet(V2 position, Ratio rotation)
        {
            Bullet data = new Bullet();
            data.user = this.user;
            data.Init();
            data.Reset(position, rotation);
            NetData.Instantiate<Bullet>(data);
        } 
    }
}
