using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectC
{
    public class PauseMenuButtons : MonoBehaviour
    {
        [SerializeField] string MaineMenuScene;
        AnimationController animCtrl;
        GameObject mainButtons;
        GameObject optionsButtons;
        SoundManager soundManager;
        [HideInInspector] public float sfxVolume;
        [HideInInspector] public float musicVolume;
        [SerializeField] TextMeshProUGUI sfxSlider;
        [SerializeField] TextMeshProUGUI musicSlider;
        Slider sfx, music;
        [SerializeField] Swipe swipe;
        [SerializeField] TextMeshProUGUI optionText;
        bool cardEnabled = true;

        SaveLoad loader;
        [SerializeField] TMP_Dropdown gameMode;
        ButtonControls buttonControlScript;

        private void Awake()
        {
            sfx = GameObject.FindGameObjectWithTag("sfx").GetComponent<Slider>();
            music = GameObject.FindGameObjectWithTag("music").GetComponent<Slider>();
            soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
            animCtrl = gameObject.GetComponent<AnimationController>();
            optionsButtons = GameObject.Find("OptionContainer");
            mainButtons = GameObject.Find("MainContainer");
            optionsButtons.SetActive(false);
            mainButtons.SetActive(false);

            loader = FindObjectOfType<SaveLoad>();
            buttonControlScript = FindObjectOfType<ButtonControls>();
        }
        private void Start()
        {
            music.value = soundManager.GetVolume("music");
            musicSlider.text = ((int)(music.value * 100)).ToString();
            sfx.value = soundManager.GetVolume("sfx");
            sfxSlider.text = ((int)(sfx.value * 100)).ToString();
            soundManager.PlayAudio("Game Music");

            // dropdown menu 
            if (StoryControl.IsSwipeMode() == false)
                gameMode.value = 1;

            gameMode.onValueChanged.AddListener(delegate { GameModeChanged(gameMode); });
        }

        //turns on options buttons
        public void OptionsMenu()
        {
            mainButtons.SetActive(false);
            optionsButtons.SetActive(true);
        }

        //Returns the game to main menu
        public void MainMenu()
        {
            StartCoroutine(MainMenuCoroutine());
        }

        public void PauseMenu()
        {
            if (optionsButtons.activeInHierarchy == true)
            {
                optionsButtons.SetActive(false);
            }
            if (cardEnabled)
            {
                DisableCard();
            }
            mainButtons.SetActive(true);
        }
        //closes pause menu and returns to the game
        public void ClosePauseMenu()
        {
            if (StoryControl.IsSwipeMode())
                EnableCard();
            mainButtons.SetActive(false);
        }
        //coroutine that loads main menu
        IEnumerator MainMenuCoroutine()
        {
            animCtrl.FadeIn();
            yield return new WaitForSeconds(0.4f);
            soundManager.StopGroup(Sound.SoundType.music);
            this.gameObject.GetComponent<SceneLoader>().LoadScene(MaineMenuScene);
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
        //disables the games card since Time.timeScale didnt like me today
        public void DisableCard()
        {
            cardEnabled = false;
            swipe.enabled = false;
            optionText.enabled = false;
        }
        //enables the games card since Time.timeScale didnt like me today
        public void EnableCard()
        {
            cardEnabled = true;
            swipe.enabled = true;
            optionText.enabled = true;
        }
        //============>>>>>>> SAVE GAME @SUMU <<<<<<<<<<<<===================
        public void SaveGame()
        {
            loader.SaveGame();
            Debug.Log("Game saved");
        }

        public void GameModeChanged(TMP_Dropdown gameModeOptions)
        {
            StoryControl.ChangeGameMode(gameModeOptions.value == 0);
            buttonControlScript.ActivateButtonControls(!StoryControl.IsSwipeMode());
            if (StoryControl.IsSwipeMode())
            {
                DisableCard();
            }
        }
    }
}
