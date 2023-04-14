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
                    "Opeta t‰ss‰ miten peli‰ pelataan";
                }
                else
                {
                    startText.GetComponent<TMP_Text>().text = "PROJEKTI ON ALKANUT \n \n" +
                        "Valitse kortin alle tulevilla nappuloilla";
                }

                meters.GetComponent<TMP_Text>().text = "Teht‰v‰si on pit‰‰ n‰m‰ mittarit tasapainossa p‰‰st‰m‰tt‰ mit‰‰n nollaan";
                indicators.GetComponent<TMP_Text>().text = "N‰m‰ merkit kertovat, mihin mittareihin kortin valinta tulee mahdollisesti vaikuttamaan";
                buttonIndicators.GetComponent<TMP_Text>().text = "N‰m‰ merkit kertovat, mihin mittareihin kortin valinta tulee mahdollisesti vaikuttamaan";
            }
            else
            {
                if (swipeControls)
                {
                    startText.GetComponent<TMP_Text>().text = "PROJECT HAS BEGUN \n \n" +
                    "Teach how to play here";
                }
                else
                {
                    startText.GetComponent<TMP_Text>().text = "PROJECT HAS BEGUN \n \n" +
                        "Chooce by using the buttons down below";
                }
                meters.GetComponent<TMP_Text>().text = "Your mission is to keep these meters in balance without letting them reach 0";
                indicators.GetComponent<TMP_Text>().text = "These squares indicate which meters the choice will possibly affect";
                buttonIndicators.GetComponent<TMP_Text>().text = "These squares indicate which meters the choice will possibly affect";
            }
        }
    }
}
