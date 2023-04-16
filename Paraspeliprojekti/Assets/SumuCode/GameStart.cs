using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ProjectC
{
    public class GameStart : MonoBehaviour
    {
        [SerializeField] private GameObject helpScreen, startText, meters, indicators, buttonIndicators;
        [SerializeField] private GameObject card;
        private HelpPhase phase = HelpPhase.Start;
        PauseMenuButtons pause;
        SaveLoad loader;
        bool swipeControls;
        
        enum HelpPhase
        {
            Start, 
            Meters,
            Indicators,
            Close
        }

        // Start is called before the first frame update
        void Start()
        {
            pause = FindObjectOfType<PauseMenuButtons>();
            loader = GetComponent<SaveLoad>();
            swipeControls = StoryControl.IsSwipeMode();
            print(swipeControls);
            meters.SetActive(false);
            indicators.SetActive(false);
            buttonIndicators.SetActive(false);
            if(StoryControl.state == StoryControl.StartState.LoadGame)
            {
                helpScreen.SetActive(false);
            }
            else
            {
                if (!swipeControls)
                {
                    GetComponent<ButtonControls>().ButtonControlTutorialPositions();
                }

                pause.DisableCard();
            }

            ChangeLanguage();
        }

        public void TapScreen()
        {
            phase++;
            switch(phase)
            {
                case HelpPhase.Meters:
                    startText.SetActive(false);
                    meters.SetActive(true);
                    break;

                case HelpPhase.Indicators:
                    meters.SetActive(false);
                    if (swipeControls)
                        indicators.SetActive(true);
                    else
                        buttonIndicators.SetActive(true);
                    break;

                case HelpPhase.Close:
                    helpScreen.SetActive(false);
                    pause.EnableCard();

                    if(PlayerPrefs.GetInt(StoryControl.GameModeString()) == 0)
                        card.GetComponent<Animation>().Play("shake");

                    if (!swipeControls)
                    {
                        GetComponent<ButtonControls>().ActivateButtonControls(true);
                    }

                    break;
            }
        }

        private void ChangeLanguage()
        {
            if(StoryControl.IsFinnish())
            {
                if (swipeControls)
                {
                    startText.GetComponent<TMP_Text>().text = "PROJEKTI ON ALKANUT \n \n" +
                    "Sinä olet projektinvetäjä. Sinulle esitetään työpaikalla tapahtuvia tilanteita kortti kerrallaan, ja tehtäväsi on valita paras toimenpide kussakin tilanteessa. \n" +
                    "Tee valinta vetämällä kortti vasemmalle tai oikealle. Vastausvaihtoehdot tulevat näkyviin korttia liikuttamalla.";
                }
                else
                {
                    startText.GetComponent<TMP_Text>().text = "PROJEKTI ON ALKANUT \n \n" +
                    "Sinä olet projektinvetäjä. Sinulle esitetään työpaikalla tapahtuvia tilanteita kortti kerrallaan, ja tehtäväsi on valita paras toimenpide kussakin tilanteessa. \n" +
                    "Tee valinta painamalla kortin alle ilmestyviä nappuloita.";
                }

                meters.GetComponent<TMP_Text>().text = "Tehtävänäsi on myös pitää nämä mittarit tasapainossa päästämättä mitään niistä nollaan.";
                indicators.GetComponent<TMP_Text>().text = "Nämä merkit kertovat, mihin mittareihin kortin valinta tulee mahdollisesti vaikuttamaan";
                buttonIndicators.GetComponent<TMP_Text>().text = "Nämä merkit kertovat, mihin mittareihin kortin valinta tulee mahdollisesti vaikuttamaan";
            }
            else
            {
                if (swipeControls)
                {
                    startText.GetComponent<TMP_Text>().text = "THE PROJECT HAS BEGUN \n \n" +
                    "You are the project manager. You will be presented with situations that happens in the workplace, one card at a time. Your mission is to choose the best option in each scenario. \n" +
                    "Make a choice by swiping the card to either left or right. The options will appear when you move the card.";
                }
                else
                {
                    startText.GetComponent<TMP_Text>().text = "PROJECT HAS BEGUN \n \n" +
                    "You are the project manager. You will be presented with situations that happens in the workplace, one card at a time. Your mission is to choose the best option in each scenario. \n" +
                    "Make a choice by tapping the buttons that will appear under the card.";
                }
                meters.GetComponent<TMP_Text>().text = "Another mission of yours is to keep these meters in balance without letting any of them go to zero.";
                indicators.GetComponent<TMP_Text>().text = "These squares indicate which meters the choice will possibly affect";
                buttonIndicators.GetComponent<TMP_Text>().text = "These squares indicate which meters the choice will possibly affect";
            }
        }
    }
}
