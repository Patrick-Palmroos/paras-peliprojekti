using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectC
{
    public class AnimationController : MonoBehaviour
    {
        GameObject fade;
        Animator fadeAnim, blindsAnim;

        private void Awake() {
            fade = GameObject.Find("Fade");
            fade.SetActive(false);
            blindsAnim = GameObject.Find("Blinds").GetComponent<Animator>();
            fadeAnim = fade.GetComponent<Animator>();
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
    }
}
