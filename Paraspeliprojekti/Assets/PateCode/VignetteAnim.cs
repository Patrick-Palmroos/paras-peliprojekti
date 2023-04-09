using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ProjectC
{
    public class VignetteAnim : MonoBehaviour
    {
        Volume volume;
        Vignette vig;
        public float multiplier, colorMultiplier, resetMult;
        float xPos, intensity;
        bool resetVignette;
        float distance;

        private void Start()
        {
            volume = GameObject.Find("Global Volume").GetComponent<Volume>();
            volume.profile.TryGet<Vignette>(out vig);
        }

        public void MoveVignette(float amount)
        {
            distance = amount;
        }

        public void ResetVignette()
        {
            resetVignette = true;
        }

        private void FixedUpdate()
        {
            if (resetVignette)
            {
                if (distance > 0)
                {
                    distance -= Time.deltaTime * resetMult;
                } else if (distance < 0)
                {
                    distance += Time.deltaTime * resetMult;
                } 
                else
                {
                    distance = 0;
                    resetVignette = false;
                }
            }

            xPos = 0.5f + distance * multiplier;
            vig.center.value = new Vector2(xPos, 0.8f);
            intensity = Mathf.Abs(distance) * colorMultiplier;
            vig.intensity.value = intensity * colorMultiplier;
        }
    }
}
