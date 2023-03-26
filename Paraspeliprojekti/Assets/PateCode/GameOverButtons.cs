using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectC
{
    public class GameOverButtons : MonoBehaviour
    {
        SceneLoader loader;
        AnimationController animCtrl;
        SoundManager soundManager;

        private void Awake()
        {
            loader = gameObject.GetComponent<SceneLoader>();
            animCtrl = gameObject.GetComponent<AnimationController>();
            soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        }

        public void MainMenuWrapper()
        {
            StartCoroutine(MainMenu());
        }

        IEnumerator MainMenu()
        {
            animCtrl.FadeIn();
            yield return new WaitForSeconds(0.4f);
            soundManager.StopGroup(Sound.SoundType.music);
            loader.LoadScene("Menu");
        }
    }
}
