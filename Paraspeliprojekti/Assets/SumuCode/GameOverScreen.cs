using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ProjectC
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private TMP_Text gameOverText;

        // Start is called before the first frame update
        void Start()
        {
            if (GameObject.FindObjectOfType<StoryControl>())
                gameOverText.text = StoryControl.gameEndText;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
