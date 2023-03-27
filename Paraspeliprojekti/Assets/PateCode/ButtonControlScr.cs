using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectC
{
    public class ButtonControlScr : MonoBehaviour
    {
        [SerializeField] string newGameScene;
        AnimationController animCtrl;
        GameObject mainButtons;
        GameObject optionsButtons;
        GameObject exitCheck;
        SoundManager soundManager;
        [HideInInspector] public float sfxVolume;
        [HideInInspector] public float musicVolume;
        [SerializeField] TextMeshProUGUI sfxSlider;
        [SerializeField] TextMeshProUGUI musicSlider;
        Slider sfx, music;

        private void Awake()
        {
            sfx = GameObject.FindGameObjectWithTag("sfx").GetComponent<Slider>();
            music = GameObject.FindGameObjectWithTag("music").GetComponent<Slider>();
            soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
            animCtrl = gameObject.GetComponent<AnimationController>();
            optionsButtons = GameObject.Find("OptionContainer");
            mainButtons = GameObject.Find("MainContainer");
            exitCheck = GameObject.Find("ExitCheck");
            optionsButtons.SetActive(false);
            exitCheck.SetActive(false);
        }

        private void Start()
        {
            music.value = soundManager.GetVolume("music");
            musicSlider.text = ((int)(music.value * 100)).ToString();
            sfx.value = soundManager.GetVolume("sfx");
            sfxSlider.text = ((int)(sfx.value * 100)).ToString();
            soundManager.PlayAudio("Menu Music");
        }
        //Unity UI button cant start a couroutine so a wrapper is used.
        public void NewGameWrapper() {
            StartCoroutine(NewGameButton());
        }
        //loads new game scene asynchronatically.
        IEnumerator NewGameButton() {
            animCtrl.FadeIn();
            yield return new WaitForSeconds(0.4f);
            soundManager.StopGroup(Sound.SoundType.music);
            this.gameObject.GetComponent<SceneLoader>().LoadScene(newGameScene);
        }
        //turns main menu buttons off and options menu on by calling the coroutine below
        public void OptionsMenu()
        {
            StartCoroutine(OptionsMenuCoroutine());
        }

        IEnumerator OptionsMenuCoroutine()
        {
            mainButtons.SetActive(false);
            animCtrl.CloseBlinds();
            yield return new WaitForSeconds(0.7f);
            optionsButtons.SetActive(true);
        }
        //turns options menu buttons off and main menu on by calling the coroutine below
        public void MainMenu()
        {
            StartCoroutine(MainMenuCoroutine());
        }

        IEnumerator MainMenuCoroutine()
        {
            optionsButtons.SetActive(false);
            animCtrl.OpenBlinds();
            yield return new WaitForSeconds(0.5f);
            mainButtons.SetActive(true);
        }
        //loads the new game
        public void LoadGame()
        {
            Debug.Log("Load Game");
        }
        //truns check both on and off
        public void ExitButton()
        {
            if (exitCheck.activeInHierarchy == true)
            {
                exitCheck.SetActive(false);
            } else
            {
                exitCheck.SetActive(true);
            }
        }
        //closes the application
        public void ExitGame()
        {
            Application.Quit();
        }
        //used to change sfx volume
        public void SfxSlider(float v)
        {
            sfxVolume = v;
            sfxSlider.text = ((int)(v * 100)).ToString();
            soundManager.UpdateMixer(v, "Sfx Volume");
        }
        //used to change music volume
        public void MusicSlider(float v)
        {
            musicVolume = v;
            musicSlider.text = ((int)(v * 100)).ToString();
            soundManager.UpdateMixer(v, "Music Volume");
        }
    }
}
