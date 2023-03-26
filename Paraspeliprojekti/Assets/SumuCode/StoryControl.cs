using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectC
{
    public class StoryControl : MonoBehaviour
    {
        public static string gameEndText = "Game over!";

        // Start is called before the first frame update
        void Start()
        {
            DontDestroyOnLoad(this);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
