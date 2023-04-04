using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectC
{
    public class VelocitySwipe : MonoBehaviour
    {
        Swipe swipeScr;
        Vector3 lastMousePos;
        public float interval, threshhold;
        bool track;
        string state;

        private void Start()
        {
            swipeScr = gameObject.GetComponent<Swipe>();
        }

        private void Update()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                track = true;
                StartCoroutine(checkPosInIntervals());
            }

            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                track = false;
                float currPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x;
                float swipeDelta = lastMousePos.x - currPos;
                if (swipeDelta > 0)
                {
                    state = "left";
                } else
                {
                    state = "right";
                }
                Debug.Log(swipeDelta);
                if (Mathf.Abs(swipeDelta) > threshhold)
                {
                    swipeScr.VelocitySwipeTrue(state);
                }
            }
        }

        IEnumerator checkPosInIntervals()
        {
            while (track)
            {
                lastMousePos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                yield return new WaitForSeconds(interval);
            }
        }
    }
}
