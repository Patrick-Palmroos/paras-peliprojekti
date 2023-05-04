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

            StoryControl.NormalEnding();
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

        public static string GameModeString()
        {
            return GAMEMODE;
        }

        public static void NoHappinessEnding()
        {
            if(IsFinnish())
            {
                gameEndText = 
                    "Työntekijöistäsi tuli niin onnettomia ja pahoinvoivia työpaikalla, että he irtisanoutuivat." +
                    "Koita pitää huoli siitä, että kaikilla työntekijöillä on hyvä olla työpaikalla.";
            }
            else
            {
                gameEndText =
                    "Your workers got so unhappy they resigned, and there are no workers left to continue the project." +
                    "Try to make sure that everyone's wellbeing is taken into consideration.";
            }
        }

        public static void NoMoneyEnding()
        {
            if (IsFinnish())
            {
                gameEndText =
                    "Kulutit liikaa projektin rahoja, eikä sinulla ole enää tarpeeksi varoja jatkaa projektia.";
            }
            else
            {
                gameEndText =
                    "You used too much of the project's money, and now you don't have the necessary funds to continue the project.";
            }
        }

        public static void NoEnergyEnding()
        {
            if (IsFinnish())
            {
                gameEndText =
                    "Työntekijät saivat liikaa töitä harteilleen, eivätkä he kykene enää jatkamaan projektin parissa.";
            }
            else
            {
                gameEndText =
                    "The workers had too much work on their plates, and they couldn't continue working with the project anymore.";
            }
        }

        public static void NormalEnding()
        {
            if (IsFinnish())
            {
                gameEndText = 
                    "Onneksi olkoon, projekti saatiin päätökseen onnistuneesti! \n" +
                    "Selvisit projektinvetäjän virasta kunnialla ja työtoverisi pysyivät tyytyväisinä.";
            }
            else
            {
                gameEndText = 
                    "Congratulations, the project is successfully behind you! \n" +
                    "You handled the title of a project manager with honor and your workmates stayed happy.";
            }
        }
    }
}
