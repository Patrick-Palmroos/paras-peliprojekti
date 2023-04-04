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

        // Sumu's additions
        [SerializeField] Button loadButton;
        [SerializeField] TMP_Dropdown language, gameMode;


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

            // disables load button if there is nothing to load
            if(!PlayerPrefs.HasKey("Save exists"))
                loadButton.interactable = false;

            // changes the values if they're not default
            if (StoryControl.IsFinnish() == false)
                language.value = 1;
            if (StoryControl.IsSwipeMode() == false)
                gameMode.value = 1;

            // adds listeners to dropdowns
            language.onValueChanged.AddListener(delegate { LanguageChanged(language); });
            gameMode.onValueChanged.AddListener(delegate { GameModeChanged(language); });
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
            // tells the story control that a save should be loaded
            StoryControl.state = StoryControl.StartState.LoadGame;
            StartCoroutine(NewGameButton());
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


        // -------------------
        // Sumu's part starts here
        // -------------------
        private void LanguageChanged(TMP_Dropdown languageOptions)
        {
            StoryControl.ChangeLanguage(languageOptions.value == 0);
        }

        private void GameModeChanged(TMP_Dropdown gameModeOptions)
        {
            StoryControl.ChangeGameMode(gameModeOptions.value == 0);
        }
    }
}
