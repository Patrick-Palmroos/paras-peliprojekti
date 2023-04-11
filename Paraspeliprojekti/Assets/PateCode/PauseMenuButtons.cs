using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectC
{
    public class PauseMenuButtons : MonoBehaviour
    {
        [SerializeField] string MainMenuScene;
        AnimationController animCtrl;
        GameObject optionsButtons;
        SoundManager soundManager;
        [HideInInspector] public float sfxVolume, musicVolume;
        [SerializeField] TextMeshProUGUI sfxSlider, musicSlider;
        Slider sfx, music;
        [SerializeField] Swipe swipe;
        [SerializeField] TextMeshProUGUI optionText;
        bool cardEnabled = true;

        SaveLoad loader;
        ButtonControls buttonControlScript;
        [SerializeField] TMP_Dropdown gameMode;
        [SerializeField] GameObject darken;
        [SerializeField] GameObject savedConfirmation;

        // new pause UI
        [SerializeField] private GameObject saveBtn, settingsBtn, menuBtn, settingsScreen;
        [SerializeField] private RectTransform background;
        Vector3 saveBtnPos, settingsBtnPos, menuBtnPos;
        Vector2 backgroundOpenSize, backgroundClosedSize;

        bool animOn = false;
        bool menuOpen = false;
        float duration = 0.3f;

        private string sfxVolumeName = "Sfx Volume";
        private string musicVolumeName = "Music Volume";

        private void Awake()
        {
            sfx = GameObject.FindGameObjectWithTag("sfx").GetComponent<Slider>();
            music = GameObject.FindGameObjectWithTag("music").GetComponent<Slider>();
            soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
            animCtrl = gameObject.GetComponent<AnimationController>();
            optionsButtons = GameObject.Find("OptionContainer");
            optionsButtons.SetActive(false);

            loader = FindObjectOfType<SaveLoad>();
            buttonControlScript = FindObjectOfType<ButtonControls>();
        }
        private void Start()
        {
            soundManager.PlayAudio("Game Music");

            // on default, sets the volume sliders to full volume
            if (!PlayerPrefs.HasKey(sfxVolumeName))
                PlayerPrefs.SetFloat(sfxVolumeName, 1);
            if (!PlayerPrefs.HasKey(musicVolumeName))
                PlayerPrefs.SetFloat(musicVolumeName, 1);

            music.value = PlayerPrefs.GetFloat(musicVolumeName);
            sfx.value = PlayerPrefs.GetFloat(sfxVolumeName);
            soundManager.UpdateMixer(music.value, musicVolumeName);
            soundManager.UpdateMixer(sfx.value, sfxVolumeName);

            musicSlider.text = ((int)(music.value * 100)).ToString();
            sfxSlider.text = ((int)(sfx.value * 100)).ToString();

            // dropdown menu 
            if (StoryControl.IsSwipeMode() == false)
                gameMode.value = 1;

            gameMode.onValueChanged.AddListener(delegate { GameModeChanged(gameMode); });
            darken.SetActive(false);

            sfx.value = PlayerPrefs.GetFloat(sfxVolumeName);
            music.value = PlayerPrefs.GetFloat(musicVolumeName);

            // new pause UI initiation
            saveBtnPos = saveBtn.transform.localPosition;
            settingsBtnPos = settingsBtn.transform.localPosition;
            menuBtnPos = menuBtn.transform.localPosition;
            saveBtn.transform.localPosition = Vector3.zero;
            settingsBtn.transform.localPosition = Vector3.zero;
            menuBtn.transform.localPosition = Vector3.zero;
            backgroundOpenSize = background.sizeDelta;
            backgroundClosedSize = new Vector2(110, 110);
            background.sizeDelta = backgroundClosedSize;

            saveBtn.SetActive(false);
            settingsBtn.SetActive(false);
            menuBtn.SetActive(false);
        }

        //turns on options buttons
        public void OptionsMenu()
        {
            if (cardEnabled)
            {
                DisableCard();
            }

            StartCoroutine(CloseMenu());
            optionsButtons.SetActive(true);
            darken.SetActive(true);
        }

        //Returns the game to main menu
        public void MainMenu()
        {
            StartCoroutine(MainMenuCoroutine());
        }

        public void CloseOptions()
        {
            if (StoryControl.IsSwipeMode())
                EnableCard();

            optionsButtons.SetActive(false);
            darken.SetActive(false);
        }

        //coroutine that loads main menu
        IEnumerator MainMenuCoroutine()
        {
            animCtrl.FadeIn();
            yield return new WaitForSeconds(0.4f);
            soundManager.StopGroup(Sound.SoundType.music);
            this.gameObject.GetComponent<SceneLoader>().LoadScene(MainMenuScene);
        }
        //used to change sfx volume
        public void SfxSlider(float v)
        {
            sfxVolume = v;
            sfxSlider.text = ((int)(v * 100)).ToString();
            soundManager.UpdateMixer(v, sfxVolumeName);
            PlayerPrefs.SetFloat(sfxVolumeName, v);
        }
        //used to change music volume
        public void MusicSlider(float v)
        {
            musicVolume = v;
            musicSlider.text = ((int)(v * 100)).ToString();
            soundManager.UpdateMixer(v, musicVolumeName);
            PlayerPrefs.SetFloat(musicVolumeName, v);
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


        // --- NEW UI STUFF ---

        public void ToggleMenu()
        {
            if (animOn)
                return;

            animOn = true;
            if (menuOpen)
                StartCoroutine(CloseMenu());
            else
                StartCoroutine(OpenMenu());
        }

        public void SaveGame()
        {
            loader.SaveGame();
            StartCoroutine(GameSaved());
            savedConfirmation.GetComponent<Animator>().SetTrigger("saved");
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

        IEnumerator OpenMenu()
        {
            StartCoroutine(MoveMenuItems(saveBtn, saveBtnPos));
            StartCoroutine(MoveMenuItems(settingsBtn, settingsBtnPos));
            StartCoroutine(MoveMenuItems(menuBtn, menuBtnPos));
            Coroutine bg = StartCoroutine(ChangeBackgroundSize(backgroundOpenSize));
            yield return bg;
            animOn = false;
            menuOpen = true;
        }

        IEnumerator CloseMenu()
        {
            StartCoroutine(MoveMenuItems(saveBtn, Vector3.zero));
            StartCoroutine(MoveMenuItems(settingsBtn, Vector3.zero));
            StartCoroutine(MoveMenuItems(menuBtn, Vector3.zero));
            Coroutine bg = StartCoroutine(ChangeBackgroundSize(backgroundClosedSize));
            yield return bg;
            animOn = false;
            menuOpen = false;
        }

        IEnumerator MoveMenuItems(GameObject menuItem, Vector3 endPos)
        {
            if (!menuItem.activeInHierarchy)
                menuItem.SetActive(true);

            float timer = 0;
            Vector3 startPos = menuItem.transform.localPosition;

            // moves the menu item to a desired position
            while (timer < duration)
            {
                timer += Time.deltaTime;
                menuItem.transform.localPosition = Vector3.Lerp(startPos, endPos, (timer / duration));
                yield return new WaitForEndOfFrame();
            }

            menuItem.transform.localPosition = endPos;

            if (endPos == Vector3.zero)
                menuItem.SetActive(false);
        }

        IEnumerator ChangeBackgroundSize(Vector2 endSize)
        {
            float timer = 0;
            Vector2 startSize = background.sizeDelta;

            // changes the background size to a desired size
            while (timer < duration)
            {
                timer += Time.deltaTime;
                background.sizeDelta = Vector2.Lerp(startSize, endSize, (timer / duration));
                yield return new WaitForEndOfFrame();
            }

            background.sizeDelta = endSize;
        }

        IEnumerator GameSaved()
        {
            float timer = 0;
            float spinDuration = 0.4f;
            float startRotation = 0;
            float endRotation = 360;
            
            while (timer < spinDuration)
            {
                timer += Time.deltaTime;
                float spin = Mathf.Lerp(startRotation, endRotation, (timer / spinDuration));
                saveBtn.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, spin);
                yield return null;
            }
        }
    }
}
