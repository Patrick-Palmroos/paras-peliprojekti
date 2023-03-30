using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ProjectC
{
    public class ButtonControls : MonoBehaviour
    {
        [SerializeField] Button leftButton, rightButton;
        [SerializeField] GameObject card, cardBackground, indicators;
        Animator cardAnimator;
        Vector3 originalPos;
        Vector3 buttonPos;
        Vector3 indicatorOriginalPos;
        Vector3 indicatorPos;
        bool left;

        GameFlow flowControl;

        private const string BUTTONCONTROLS = "Button controls";

        // Start is called before the first frame update
        void Start()
        {
            flowControl = FindObjectOfType<GameFlow>();
            cardAnimator = card.GetComponent<Animator>();
            cardAnimator.enabled = false;
            originalPos = card.transform.position;
            buttonPos = new Vector3(0, -0.5f, 0);
            indicatorOriginalPos = indicators.transform.position;
            indicatorPos = new Vector3(0, -625, 0);
            leftButton.gameObject.SetActive(false);
            rightButton.gameObject.SetActive(false);
        }

        public void ActivateButtonControls(bool activate)
        {
            StoryControl.buttonControls = activate;
            leftButton.gameObject.SetActive(activate);
            rightButton.gameObject.SetActive(activate);
            card.GetComponent<Swipe>().enabled = !activate;

            if(activate)
            {
                // changes locations of certain objects
                card.transform.position = buttonPos;
                cardBackground.transform.position = buttonPos;
                indicators.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
                indicators.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
                indicators.GetComponent<RectTransform>().anchoredPosition = indicatorPos;
                cardAnimator.enabled = true;
            }
            else
            {
                // sets all back to their original locations
                card.transform.position = originalPos;
                cardBackground.transform.position = originalPos;
                indicators.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0);
                indicators.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0);
                indicators.transform.position = indicatorOriginalPos;
                cardAnimator.enabled = false;
            }
        }

        // chooses options from buttons
        public void ChooseOption(bool left)
        {
            this.left = left;
            if(left)
            {
                cardAnimator.SetTrigger("leftSwipe");
            }
            else
            {
                cardAnimator.SetTrigger("rightSwipe");
            }

            leftButton.interactable = false;
            rightButton.interactable = false;
        }

        public void ChangeTexts()
        {
            if(left)
            {
                flowControl.SendMessage("Swiped", "Left");
            }
            else
            {
                flowControl.SendMessage("Swiped", "Right");
            }
            leftButton.interactable = true;
            rightButton.interactable = true;
            leftButton.transform.Find("Text").GetComponent<TMP_Text>().text = flowControl.GetOption(true);
            rightButton.transform.Find("Text").GetComponent<TMP_Text>().text = flowControl.GetOption(false);
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.B))
            {
                ActivateButtonControls(!StoryControl.buttonControls);
                leftButton.transform.Find("Text").GetComponent<TMP_Text>().text = flowControl.GetOption(true);
                rightButton.transform.Find("Text").GetComponent<TMP_Text>().text = flowControl.GetOption(false);
            }
        }
    }
}