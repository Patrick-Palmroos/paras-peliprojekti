using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectC
{
    public class StoryControl : MonoBehaviour
    {
        public static string gameEndText = "Game over!";
        private static StoryControl storyControl;
        public static StartState state = StartState.NewGame;
        public static bool buttonControls = false;

        private const string LANGUAGE = "Language";
        private const string GAMEMODE = "Game mode";
        private static int language = 0; // 0 = finnish, 1 = english
        private static int gameMode = 0; // 0 = swipe, 1 = buttons

        public enum StartState
        {
            NewGame,
            LoadGame
        }

        // Start is called before the first frame update
        void Start()
        {
            DontDestroyOnLoad(gameObject);

            // prevents duplicates
            if (storyControl == null)
            {
                storyControl = this;
            }
            else
            {
                Destroy(gameObject);
            }

            language = PlayerPrefs.GetInt(LANGUAGE);
            gameMode = PlayerPrefs.GetInt(GAMEMODE);
        }

        public static void ChangeLanguage(bool finnish)
        {
            if (finnish)
            {
                language = 0;
            }
            else
            {
                language = 1;
            }

            PlayerPrefs.SetInt(LANGUAGE, language);
        }

        public static void ChangeGameMode (bool swipeOn)
        {
            if (swipeOn)
            {
                gameMode = 0;
            }
            else
            {
                gameMode = 1;
            }

            PlayerPrefs.SetInt(GAMEMODE, gameMode);
        }

        // returns true if the game is in finnish
        public static bool IsFinnish()
        {
            if (PlayerPrefs.HasKey(LANGUAGE))
                return PlayerPrefs.GetInt(LANGUAGE) == 0;

            return true;
        }

        // returns true if the game is in swipe mode
        public static bool IsSwipeMode()
        {
            if (PlayerPrefs.HasKey(GAMEMODE))
                return PlayerPrefs.GetInt(GAMEMODE) == 0;

            return true;
        }
    }
}
