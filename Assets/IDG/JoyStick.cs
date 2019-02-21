using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using IDG;
namespace IDG.MobileInput
{

    /// <summary>
    /// 摇杆UI实现
    /// </summary>
    public class JoyStick : MonoBehaviour,IDragHandler,IEndDragHandler,IBeginDragHandler
    {
        public RectTransform backTransform;
        public RectTransform moveObj;
        protected float maxScale;
        Coroutine coroutine;
        protected bool isDown;
        protected bool onDrag;
        protected Fixed2 dir = Fixed2.zero;
        public KeyNum key;
        public Action BeginMove;
        public Action<Fixed2> OnMove;
        public Action<Fixed2> EndMove;
        public bool moveToPointDownPos=false;
        public CanvasGroup group;
        public bool useKey = false;
        public Fixed2 Direction()
        {
            return dir;
         
        }
        public Vector3 GetVector3()
        {
            return (moveObj.position - backTransform.position).normalized;
        }
        protected KeyNum KeyValue()
        {
            
            return isDown ? key :0 ;
            
        }
        public JoyStickKey GetInfo()
        {
            return new JoyStickKey(KeyValue(), Direction());
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            isDown = true;
            onDrag = true;
            if(moveToPointDownPos) backTransform.position = eventData.position;
            Vector3 tmp = GetVector3();
            dir=new Fixed2(tmp.x, tmp.y);
            if (BeginMove != null)
            {
                BeginMove();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 movePos = eventData.position;
           
            movePos = movePos - backTransform.position;
          
            if (movePos.magnitude > maxScale)
            {

                movePos = movePos.normalized * maxScale;
            }
            else
            {
                
            }
            moveObj.position = backTransform.position + movePos;

            // direction = new V2(movePos.x, movePos.y);
            Vector3 tmp = GetVector3();
            dir = new Fixed2(tmp.x, tmp.y);
            if (OnMove!=null)
            {
                OnMove(Direction());
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //throw new System.NotImplementedException();
            moveObj.localPosition = Vector3.zero;
            backTransform.localPosition = Vector3.zero;
            if (EndMove!=null)
            {
                EndMove(Direction());
            }
            //direction = new V2();
            isDown = false;
            onDrag = false;
        }

        // Use this for initialization
        void Awake()
        {
            maxScale = backTransform.rect.width/2;
            if (useKey)
            {
                group.alpha = 0;
                group.blocksRaycasts = false;
            }
            else
            {
                group.blocksRaycasts = true;
            }
        }

       
    }
}
