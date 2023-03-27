using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectC
{
    public class Swipe : MonoBehaviour
    {
        GameControl control;

        private bool dragging = false;
        private Vector2 screenPos;
        private Vector3 worldPos;
        private Vector3 startPos;

        private float checkThreshold = 0.8f;
        private float distanceMoved;
        private string state = "Blank";

        private void Start()
        {
            control = GameObject.Find("GameControl").GetComponent<GameControl>();
            control.SendMessage("ChangeText", "");
            startPos = transform.position;
        }

        private void Update()
        {
            // When dragging stops
            if(dragging && Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                StopDrag();
                return;
            }

            // Get the touch porision
            if (Input.touchCount > 0)
            {
                screenPos = Input.GetTouch(0).position;
            } 
            else
            {
                return;
            }

            // Start dragging
            worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            if (dragging)
            {
                Drag();
            } else
            {
                // Starts dragging only if the gameobject's tag is Card
                if(Physics2D.Raycast(worldPos, Vector2.zero))
                {
                    RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
                    if (hit.collider.gameObject.tag == "Card" && Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        StartDrag();
                    }
                }
            }


            // When dragging goes beyond threshold
            if (distanceMoved > checkThreshold)
            {
                if (transform.localPosition.x > 0)
                {
                    // If the state isn't already right, 
                    // change it and the option shown on screen
                    if (state != "Right")
                    {
                        state = "Right";
                        control.SendMessage("ChangeText", state);
                    }
                }
                else
                {
                    // If the state isn't already left, 
                    // change it and the option shown on screen
                    if (state != "Left")
                    {
                        state = "Left";
                        control.SendMessage("ChangeText", state);
                    }
                }
            }
            else
            {
                // If the state isn't already none, 
                // change it and hide the text shown on screen
                if (state != "Blank")
                {
                    state = "Blank";
                    control.SendMessage("ChangeText", "");
                }
            }
        }

        void StartDrag()
        {
            dragging = true;
        }

        // Card follows the touch position
        void Drag()
        {
            transform.position = new Vector2(worldPos.x, worldPos.y);
            distanceMoved = Mathf.Abs(transform.localPosition.x - 0);

            // Resets the children too
            foreach (Transform child in transform)
            {
                child.localPosition = Vector2.zero;
                Vector3 promptPos = Vector3.zero;
                if (child.name != "bg")
                {
                    child.localPosition = promptPos;
                }
            }
        }

        // Resets the position to center
        void StopDrag()
        {
            // If the card was over the threshold when drag ended
            // Send a message that the swipe has happened
            if (distanceMoved > checkThreshold)
            {
                control.SendMessage("Swiped", state);
            }
            control.SendMessage("ChangeText", "");
            dragging = false;
            transform.position = startPos;
            foreach (Transform child in transform)
            {
                child.localPosition = Vector2.zero;
                Vector3 promptPos = Vector3.zero;
                if (child.name != "bg")
                {
                    child.localPosition = promptPos;
                }
            }
        }
    }
}
