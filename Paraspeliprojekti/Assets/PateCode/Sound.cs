using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectC
{
    [System.Serializable]
    public class Sound
    {
        public enum SoundType {
            sfx, music
        }
        public SoundType soundType;

        public bool loop;
        public bool onAwake;
        public string audioclipName;
        public AudioClip audioClip;
        [HideInInspector] public AudioSource audioSource;
    }
}
