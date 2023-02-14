using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectC
{
    public class Swipe : MonoBehaviour
    {
        [SerializeField] private InputAction press, screenPos;
        GameControl control;
        private float checkThreshold = 1.5f;
        private float distanceMoved;
        private Vector3 startPos;
        private Vector3 currScreenPos;
        private BoxCollider2D coll;
        Camera camera;
        private string state;
        private bool isDragging;
        private bool isPressed {
            get
            {
                Vector3 wp = camera.ScreenToWorldPoint(currScreenPos);
                Vector2 touchPos = new Vector2(wp.x, wp.y);
                if(coll == Physics2D.OverlapPoint(touchPos))
                {
                    return true;
                }
                return false;
            }
        }

        private void Awake()
        {
            control = GameObject.Find("GameControl").GetComponent<GameControl>();
            startPos = transform.position;
            camera = Camera.main;
            coll = gameObject.GetComponent<BoxCollider2D>();
            screenPos.Enable();
            press.Enable();
            screenPos.performed += ctx => { currScreenPos = ctx.ReadValue<Vector2>(); };
            press.performed += _ => { if(isPressed) StartCoroutine(Drag()); };
            press.canceled += _ => { isDragging = false; };
        }

        private void Update()
        {
            if (distanceMoved > checkThreshold)
            {
                if (transform.localPosition.x > startPos.x)
                {
                    // right
                    if (state != "Right")
                    {
                        state = "Right";
                        control.SendMessage("ChangeText", state);
                    }
                } 
                else
                {
                    // left
                    if (state != "Left")
                    {
                        state = "Left";
                        control.SendMessage("ChangeText", state);
                    }
                }
            } else
            {
                if(state != "Blank")
                {
                    state = "Blank";
                    control.SendMessage("ChangeText", "");
                }
            }
        }

        private IEnumerator Drag()
        {
            isDragging = true;
            while(isDragging)
            {
                distanceMoved = Mathf.Abs(transform.localPosition.x - startPos.x);
                Vector3 newPos = camera.ScreenToWorldPoint(currScreenPos);
                newPos.z = transform.position.z;
                transform.position = newPos;
                yield return null;
            }

            // sends swipe message
            if (distanceMoved > checkThreshold)
            {
                control.SendMessage("Swiped", state);
            }
            distanceMoved = 0;
            // resets the children
            foreach (Transform child in transform)
            {
                child.localPosition = startPos;
            }
            transform.position = startPos;
        }
    }
}
