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
       
        public void Init(float firingRate,NetData User)
        {
            this.firingInterval = new Ratio(1/firingRate);
            lastTime = new Ratio(-1000);
            this.user = User;
        }
        public void Fire(V2 position, Ratio rotation)
        {
            if(InputCenter.Time> lastTime + firingInterval)
            {
                lastTime = InputCenter.Time;
                ShootBullet(position,rotation);
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
