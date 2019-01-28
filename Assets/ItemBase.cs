using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IDG;

namespace IDG.FSClient
{
    public class ItemBase
    {
        protected string itemName="物体";
        protected int itemId = -1;
        private static int ITEM_ID_NUM=0;
        protected NetData user;
        public ItemBase()
        {
            itemId = ITEM_ID_NUM++;
            itemName = "物体" + itemId;
        }
        
    }
}
