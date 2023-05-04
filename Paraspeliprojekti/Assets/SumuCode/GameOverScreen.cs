using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ProjectC
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private TMP_Text gameOverText;
        [SerializeField] private GameObject passedBackground, failedBackground, whitePanel;

        // Start is called before the first frame update
        void Start()
        {
            if (GameObject.FindObjectOfType<StoryControl>())
                gameOverText.text = StoryControl.gameEndText;

            if (gameOverText.text.StartsWith("Onneksi olkoon") || gameOverText.text.StartsWith("Congratulations"))
            {
                passedBackground.SetActive(true);
                failedBackground.SetActive(false);
                whitePanel.SetActive(true);
            }
            else
            {
                passedBackground.SetActive(false);
                failedBackground.SetActive(true);
                whitePanel.SetActive(false);
            }
        }
    }
}
