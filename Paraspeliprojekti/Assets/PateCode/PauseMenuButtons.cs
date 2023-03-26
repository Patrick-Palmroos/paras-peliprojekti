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
        }
        private void Start()
        {
            music.value = soundManager.GetVolume("music");
            musicSlider.text = ((int)(music.value * 100)).ToString();
            sfx.value = soundManager.GetVolume("sfx");
            sfxSlider.text = ((int)(sfx.value * 100)).ToString();
            soundManager.PlayAudio("Game Music");
        }

        //turns main menu buttons off and options menu on by calling the coroutine below
        public void OptionsMenu()
        {
            mainButtons.SetActive(false);
            optionsButtons.SetActive(true);
        }

        //turns options menu buttons off and main menu on by calling the coroutine below
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
            mainButtons.SetActive(true);
        }

        public void ClosePauseMenu()
        {
            mainButtons.SetActive(false);
        }

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
    }
}
