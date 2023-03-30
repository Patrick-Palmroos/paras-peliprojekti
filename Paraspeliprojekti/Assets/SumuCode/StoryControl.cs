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
        }
    }
}
