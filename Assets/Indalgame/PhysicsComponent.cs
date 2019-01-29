using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDG.FSClient;

namespace IDG
{
    public class PhysicsComponent
    {
        public bool enable=false;
     
        protected List<NetData> lastCollisonDatas = new List<NetData>();
        public List<NetData> collisonDatas=new List<NetData>();

        public void Init(Action<NetData> enter,Action<NetData> stay, Action<NetData> exit){
            OnPhysicsCheckEnter=enter;
            OnPhysicsCheckStay=stay;

            OnPhysicsCheckExit=exit;
        }
        public void Update(){
            if (enable)
            {
                foreach (var other in this.collisonDatas)
                {
                    if (!lastCollisonDatas.Contains(other))
                    {
                        OnPhysicsCheckEnter(other);
                    }
                    else
                    {
                        lastCollisonDatas.Remove(other);
                    }
                    //Debug.Log("1 "+this.collisonDatas+" "+this.collisonDatas.Count);
                    OnPhysicsCheckStay(other);
                }
                foreach (var other in lastCollisonDatas)
                {
                    OnPhysicsCheckExit(other);
                }
            }
            lastCollisonDatas.Clear();
            lastCollisonDatas.AddRange(collisonDatas);

            //  collisonDatas = ShapPhysics.CheckAll(this);
           
            
            collisonDatas.Clear();
        }

        public bool CheckCollision(NetData a)
        {
           
            foreach (var item in collisonDatas)
            {

                if (!item.isTrigger)
                {
                    return true;
                }
            }

            return false;
        }
        public Action<NetData> OnPhysicsCheckStay;
        public Action<NetData> OnPhysicsCheckEnter;
        public Action<NetData> OnPhysicsCheckExit;
    }
}