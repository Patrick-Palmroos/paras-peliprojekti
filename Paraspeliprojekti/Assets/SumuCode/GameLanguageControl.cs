using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ProjectC
{
    public class GameLanguageControl : MonoBehaviour
    {
        [SerializeField] private TMP_Text saveConfirmation;
        [SerializeField] private TMP_Text sfx, music, closeOptions, controlsTitle;
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
            saveConfirmation.text = "Tallennettu";
            sfx.text = "Äänitehosteet";
            music.text = "Musiikki";
            closeOptions.text = "Sulje";
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
            saveConfirmation.text = "Game saved";
            sfx.text = "Sfx";
            music.text = "Music";
            closeOptions.text = "Close";
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
