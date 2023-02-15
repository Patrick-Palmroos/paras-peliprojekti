using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectC
{
    public class SwipeScr : MonoBehaviour
    {
        float choiceThreshhold = 130, moveAmpunt;
        BoxCollider2D coll;
        public GameObject[] prompts;
        ChoiceScr choice;
        Vector2 ogPos;

        private void Awake()
        {
            coll = gameObject.GetComponent<BoxCollider2D>();
            ogPos = transform.position;
            prompts = GameObject.FindGameObjectsWithTag("prompt");
            disablePrompts();
            choice = gameObject.GetComponent<ChoiceScr>();
        }
        private void Update()
        {
            //if button is pressed and is on the card, this will move the card and display the prompts.
            if (Input.GetKey(KeyCode.Mouse0) && clickPos())
            {
                Vector2 mousePos = Input.mousePosition;
                transform.position = new Vector2(Input.mousePosition.x, transform.position.y);
                moveAmpunt = ogPos.x - mousePos.x;
                if (moveAmpunt < -40)
                {
                    selectPrompt(true).SetActive(true);
                } else if (moveAmpunt > 40)
                {
                    selectPrompt(false).SetActive(true);
                } else
                {
                    disablePrompts();
                }
            } 
            //once the key is let go the following code will check if the threshhold for the choice is fulfilled
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if (moveAmpunt > choiceThreshhold)
                {
                    disablePrompts();
                    choice.selectedChoice(false);
                } else if (moveAmpunt < -choiceThreshhold)
                {
                    disablePrompts();
                    choice.selectedChoice(true);
                } else
                {
                    disablePrompts();
                    transform.position = ogPos;
                    moveAmpunt = 0;
                }
            }
        }

        bool clickPos()
        {
            Vector2 clickDownPos = Input.mousePosition;
            LayerMask layerMask = LayerMask.GetMask("Card");
            return Physics2D.Raycast(clickDownPos, Vector2.down, Mathf.Infinity, layerMask);
        }

        void disablePrompts()
        {
            foreach (GameObject prompt in prompts)
            {
                prompt.SetActive(false);
            }
        }

        GameObject selectPrompt(bool lOrR)
        {
            GameObject temp = null;
            foreach (GameObject prompt in prompts)
            {
                if (lOrR && prompt.name == "RightPrompt")
                {
                    temp = prompt;
                } else if (!lOrR && prompt.name == "LeftPrompt")
                {
                    temp = prompt;
                }
            }

            return temp;
        }
    }
}
