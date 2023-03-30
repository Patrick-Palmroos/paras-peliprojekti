using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectC
{
    public class GameStart : MonoBehaviour
    {
        [SerializeField] private GameObject helpScreen, startText, meters, indicators;
        private HelpPhase phase = HelpPhase.Start;
        PauseMenuButtons pause;
        SaveLoad loader;
        // GameControl control;
        
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
            // control = GetComponent<GameControl>();
            meters.SetActive(false);
            indicators.SetActive(false);
            if(StoryControl.state == StoryControl.StartState.LoadGame)
            {
                helpScreen.SetActive(false);
            }
            else
            {
                pause.DisableCard();
            }
        }

        // Update is called once per frame
        void Update()
        {
        
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
                    indicators.SetActive(true);
                    break;

                case HelpPhase.Close:
                    helpScreen.SetActive(false);
                    pause.EnableCard();
                    break;
            }
        }
    }
}
