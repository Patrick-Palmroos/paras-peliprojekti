using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectC
{
    public class Swipe : MonoBehaviour
    {
        GameFlow flowControl;
        ButtonControls buttonControls;
        [SerializeField] GameObject bgImage;
        private float zeroToOne = 1;
        private bool swiped = false;
        private bool velocitySwipe = false;
        public float turnMultiplier;
        private bool cardAnim = false;
        private bool dragging = false;
        private Vector2 screenPos;
        private Vector2 dragStartPos;
        private Vector3 worldPos;
        private Vector3 startPos;

        private float checkThreshold = 0.9f;
        private float distanceMoved;
        private float turnDistanceMoved;
        private string state = "Blank";
        private string velocityState = "";
        private Rigidbody2D rb;

        private void Start()
        {
            flowControl = FindObjectOfType<GameFlow>();
            buttonControls = FindObjectOfType<ButtonControls>();
            flowControl.SendMessage("ChangeText", "");
            startPos = transform.position;
            rb = gameObject.GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            // When dragging stops
            if(dragging && Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if (velocitySwipe)
                {
                    StartCoroutine(StopDrag(0.9f, velocityState));
                }
                else
                {
                    StartCoroutine(StopDrag(checkThreshold, state));
                }
                return;
            }
            //returns the card to its original position
            if (!dragging && transform.position != startPos && !swiped) {
                Vector2 returnPos = new Vector2(Mathf.Lerp(transform.position.x, startPos.x, 0.15f),
                    Mathf.Lerp(transform.position.y, startPos.y, 0.15f));
                transform.position = returnPos;
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
                if (Physics2D.Raycast(worldPos, Vector2.zero))
                {
                    RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
                    if (hit.collider.gameObject.tag == "Card" && Input.GetTouch(0).phase == TouchPhase.Began 
                        && !cardAnim)
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
                        flowControl.SendMessage("ChangeText", state);
                    }
                }
                else
                {
                    // If the state isn't already left, 
                    // change it and the option shown on screen
                    if (state != "Left")
                    {
                        state = "Left";
                        flowControl.SendMessage("ChangeText", state);
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
                    flowControl.SendMessage("ChangeText", "");
                }
            }
        }

        private void FixedUpdate()
        {
            //cards distance from point 0;
            turnDistanceMoved = transform.localPosition.x - 0;
            //swipe animation
            if (swiped)
            {
                switch (state)
                {
                    case "Right":
                        transform.position = new Vector2(Mathf.Lerp(transform.position.x,
                            transform.position.x + 10, 0.05f), transform.position.y);
                        break;
                    case "Left":
                        transform.position = new Vector2(Mathf.Lerp(transform.position.x,
                            transform.position.x - 10, 0.05f), transform.position.y);
                        break;
                    default:
                        break;
                }
            }
            if (cardAnim) {
                if (swiped) {
                    if (zeroToOne > 0) {
                        zeroToOne -= Time.deltaTime * 3.7f;
                        bgImage.transform.localScale = new Vector2(zeroToOne, 1);
                    }
                    else {
                        zeroToOne = 0;
                        bgImage.transform.localScale = new Vector2(zeroToOne, 1);
                    }
                } else {
                    if (zeroToOne < 1) {
                        zeroToOne += Time.deltaTime * 3.7f;
                        transform.localScale = new Vector2(zeroToOne, 1);
                    }
                    else {
                        zeroToOne = 1;
                        transform.localScale = new Vector2(zeroToOne, 1);
                    }
                }
            }

            float turnAngle = turnDistanceMoved * turnMultiplier * -1f;
            gameObject.transform.eulerAngles = new Vector3(
                gameObject.transform.eulerAngles.x,
                gameObject.transform.eulerAngles.y,
                turnAngle);
        }

        void StartDrag()
        {
            dragging = true;
            dragStartPos = worldPos;
        }

        // Card follows the touch position
        void Drag()
        {
            // starts dragging from where touch starts
            Vector2 newPos = new Vector2(dragStartPos.x - worldPos.x, (dragStartPos.y - worldPos.y) + 0.79f);
            transform.position = new Vector2(Mathf.Lerp(0, -newPos.x, 1f), 
                Mathf.Lerp(-0.79f, -newPos.y, 0.2f));

            distanceMoved = Mathf.Abs(turnDistanceMoved);
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
        IEnumerator StopDrag(float threshold, string currState)
        {
            // If the card was over the threshold when drag ended
            // Send a message that the swipe has happened and do animations
            dragging = false;
            flowControl.SendMessage("ChangeText", "");
            if (distanceMoved > threshold)
            {
                if (velocitySwipe)
                {
                    velocitySwipe = false;
                }
                swiped = true;
                zeroToOne = 1f;
                cardAnim = true;
                yield return new WaitForSeconds(0.3f);

                flowControl.SendMessage("Swiped", currState);
                gameObject.transform.localScale = new Vector2(0, 1);
                transform.position = startPos;
                swiped = false;
                yield return new WaitForSeconds(0.3f);
                cardAnim = false;
            }
            gameObject.transform.eulerAngles = new Vector3(
                gameObject.transform.eulerAngles.x,
                gameObject.transform.eulerAngles.y,
                0);
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

        public void AnimationStopped()
        {
            buttonControls.ChangeTexts();
        }

        public void VelocitySwipeTrue(string leftOrRight)
        {
            if (dragging)
            {
                velocitySwipe = true;
                velocityState = leftOrRight;
            }
        }
    }
}
