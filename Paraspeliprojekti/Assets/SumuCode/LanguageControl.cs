using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ProjectC
{
    public class LanguageControl : MonoBehaviour
    {
        [SerializeField] private TMP_Text newGame, load, options, exit;
        [SerializeField] private TMP_Text sfx, music, back;
        [SerializeField] private TMP_Dropdown language, controls;
        [SerializeField] private TMP_Text youSure, yes, no;

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
            newGame.text = "Uusi peli";
            load.text = "Lataa peli";
            options.text = "Asetukset";
            exit.text = "Lopeta peli";
            sfx.text = "Äänitehosteet";
            music.text = "Musiikki";
            back.text = "Takaisin";
            controls.options[0].text = "Pyyhkäisy";
            controls.options[1].text = "Painikkeet";
            if (controls.value == 0)
                controls.gameObject.GetComponentInChildren<TMP_Text>().text = controls.options[0].text;
            else
                controls.gameObject.GetComponentInChildren<TMP_Text>().text = controls.options[1].text;
            youSure.text = "Oletko varma, että tahdot lopettaa?";
            yes.text = "Kyllä";
            no.text = "Ei";
        }

        private void ChangeToEnglish()
        {
            newGame.text = "New game";
            load.text = "Load";
            options.text = "Options";
            exit.text = "Exit";
            sfx.text = "Sfx";
            music.text = "Music";
            back.text = "Back";
            controls.options[0].text = "Swipe";
            controls.options[1].text = "Buttons";
            if (controls.value == 0)
                controls.gameObject.GetComponentInChildren<TMP_Text>().text = controls.options[0].text;
            else
                controls.gameObject.GetComponentInChildren<TMP_Text>().text = controls.options[1].text;
            youSure.text = "Are you sure you want to exit?";
            yes.text = "Yes";
            no.text = "No";
        }
    }
}
