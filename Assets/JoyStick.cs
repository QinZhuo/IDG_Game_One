using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using IDG;
namespace IDG.MobileInput
{

    
    public class JoyStick : MonoBehaviour,IDragHandler,IEndDragHandler,IBeginDragHandler
    {
        public RectTransform moveObj;
       // V2 direction;
        float maxScale;
        Coroutine coroutine;
        bool isDown;
        public FrameKey frameKey;
        public Action BeginMove;
        public Action<V2> OnMove;
        public Action<V2> EndMove;
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
        }
        public V2 Direction()
        {
            //Debug.Log("position " + (moveObj.position - transform.position) + ":" + direction.normalized);
            // Debug.Log("normalized " + +":"+direction.normalized);
            Vector3 tmp = (moveObj.position - transform.position).normalized;
            return new V2(tmp.x,tmp.y);
        }
        public FrameKey key()
        {
            
            return isDown ? frameKey :0 ;
            
        }
        public JoyStickKey GetInfo()
        {
            return new JoyStickKey(key, Direction);
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            //if (coroutine != null)
            //{
            //    StopCoroutine(coroutine);
            //    coroutine = null;
            //}
            isDown = true;
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
