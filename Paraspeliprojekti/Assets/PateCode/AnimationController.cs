using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace ProjectC
{
    public class AnimationController : MonoBehaviour
    {
        GameObject fade;
        Animator fadeAnim, blindsAnim;

        private void Awake() {
            fade = GameObject.Find("Fade");
            fadeAnim = fade.GetComponent<Animator>();
            try
            {
                blindsAnim = GameObject.Find("Blinds").GetComponent<Animator>();
            } catch(NullReferenceException e) { }
        }

        private void Start()
        {
            StartCoroutine(FadeOut());
        }

        public void FadeIn() {
            fade.SetActive(true);
            fadeAnim.Play("Fade_In");
        }

        public void CloseBlinds()
        {
            blindsAnim.Play("Blinds_Close");
        }

        public void OpenBlinds()
        {
            blindsAnim.Play("Blinds_OpenAnim");
        }

        IEnumerator FadeOut()
        {
            yield return new WaitForSeconds(0.1f);
            fadeAnim.Play("Fade_Out");
            yield return new WaitForSeconds(0.4f);
            fade.SetActive(false);
        }
    }
}
