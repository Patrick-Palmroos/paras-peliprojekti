using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ProjectC
{
    public class GameLanguageControl : MonoBehaviour
    {
        [SerializeField] private TMP_Text pauseTitle, saveGame, saved, options, backToMenu, closePause;
        [SerializeField] private TMP_Text sfx, music, backToOptions, controlsTitle;
        [SerializeField] private TMP_Dropdown controls;
        // Start is called before the first frame update
        void Start()
        {
            LanguageChanged();
        }

        public void LanguageChanged()
        {
            if (StoryControl.IsFinnish())
                ChangeToFinnish();
            else
                ChangeToEnglish();
        }

        private void ChangeToFinnish()
        {
            pauseTitle.text = "Tauko";
            saveGame.text = "Tallenna peli";
            saved.text = "Tallennettu";
            options.text = "Asetukset";
            backToMenu.text = "Takaisin päävalikkoon";
            closePause.text = "Jatka peliä";
            sfx.text = "Äänitehosteet";
            music.text = "Musiikki";
            backToOptions.text = "Takaisin";
            controlsTitle.text = "Pelimuoto:";
            controls.options[0].text = "Pyyhkäisy";
            controls.options[1].text = "Painikkeet";
            if (controls.value == 0)
                controls.gameObject.GetComponentInChildren<TMP_Text>().text = controls.options[0].text;
            else
                controls.gameObject.GetComponentInChildren<TMP_Text>().text = controls.options[1].text;
        }

        private void ChangeToEnglish()
        {
            pauseTitle.text = "Pause";
            saveGame.text = "Save game";
            saved.text = "Game saved";
            options.text = "Options";
            backToMenu.text = "Back to main menu";
            closePause.text = "Continue";
            sfx.text = "Sfx";
            music.text = "Music";
            backToOptions.text = "Back";
            controlsTitle.text = "Game mode:";
            controls.options[0].text = "Swipe";
            controls.options[1].text = "Buttons";
            if (controls.value == 0)
                controls.gameObject.GetComponentInChildren<TMP_Text>().text = controls.options[0].text;
            else
                controls.gameObject.GetComponentInChildren<TMP_Text>().text = controls.options[1].text;
        }
    }
}
