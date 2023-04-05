using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ProjectC
{
    public class EndingLanguageControl : MonoBehaviour
    {
        [SerializeField] private TMP_Text backToMenu;

        // Start is called before the first frame update
        void Start()
        {
            ChangeLanguage(StoryControl.IsFinnish());
        }

        void ChangeLanguage(bool finnish)
        {
            if(finnish)
            {
                backToMenu.text = "Takaisin p‰‰valikkoon";
            }
            else
            {
                backToMenu.text = "Back to main menu";
            }
        }
    }
}
