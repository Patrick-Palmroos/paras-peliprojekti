using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectC
{
    public class AnimationController : MonoBehaviour
    {
        GameObject fade;
        Animator anim;

        private void Awake() {
            fade = GameObject.Find("Fade");
            fade.SetActive(false);
            anim = fade.GetComponent<Animator>();
        }

        public void FadeIn() {
            fade.SetActive(true);
            anim.Play("Fade_In");
        }
    }
}
