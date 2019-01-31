using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDG.FSClient;
namespace IDG
{
    public  class ComponentBase 
    {
        public NetData data;
        public ComponentBase()
        {

        }
        public virtual void Init()
        {

        }
        public void InitNetData(NetData data)
        {
            this.data = data;
            Init();
        }
        public virtual  void Update()
        {

        }
      
    }
   
}
