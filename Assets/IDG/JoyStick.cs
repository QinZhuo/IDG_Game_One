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
        public RectTransform moveObj;
        float maxScale;
        Coroutine coroutine;
        bool isDown;
        Fixed2 dir = Fixed2.zero;
        public KeyNum key;
        public Action BeginMove;
        public Action<Fixed2> OnMove;
        public Action<Fixed2> EndMove;
        public bool useKey=false;
        public KeyCode up;
        public KeyCode down;
        public KeyCode left;
        public KeyCode right;
        private void Update()
        {
            if (!useKey|| isDown) return;
            Vector3 pos = new Vector3();
            if (Input.GetKey(left))
            {
                pos.x -= 1;
            }
            if (Input.GetKey(right))
            {
                pos.x += 1;
            }
            if (Input.GetKey(down))
            {
                pos.y -= 1;
            }
            if (Input.GetKey(up))
            {
                pos.y += 1;
            }
            moveObj.transform.position = transform.position + pos.normalized * maxScale;
            Vector3 tmp = GetVector3();
          
            dir = new Fixed2(tmp.x, tmp.y);
        }
        public Fixed2 Direction()
        {
            return dir;
         
        }
        public Vector3 GetVector3()
        {
            return (moveObj.position - transform.position).normalized;
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
           
            movePos = movePos - transform.position;
          
            if (movePos.magnitude > maxScale)
            {

                movePos = movePos.normalized * maxScale;
            }
            else
            {
                
            }
            moveObj.position = transform.position + movePos;

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
            if (EndMove!=null)
            {
                EndMove(Direction());
            }
            //direction = new V2();
            isDown = false;
        }

        // Use this for initialization
        void Awake()
        {
            maxScale = (transform as RectTransform).rect.width/2;
        }

       
    }
}
