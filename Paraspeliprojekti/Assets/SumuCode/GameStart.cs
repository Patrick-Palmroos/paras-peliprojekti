using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ProjectC
{
    public class GameStart : MonoBehaviour
    {
        [SerializeField] private GameObject helpScreen, startText, meters, indicators, buttonIndicators;
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
                    startText.GetComponent<TMP_Text>().text = "PROJEKTI ON ALKANUT \n \n" +
                        "Valitse kortin alle tulevilla nappuloilla";
                }

                pause.DisableCard();
            }
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
                    if (!swipeControls)
                    {
                        GetComponent<ButtonControls>().ActivateButtonControls(true);
                    }

                    break;
            }
        }
    }
}
