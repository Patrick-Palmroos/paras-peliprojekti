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

        public bool playOnAwake;
        public bool loop;
        public string audioclipName;
        public AudioClip audioClip;
        [HideInInspector] public AudioSource audioSource;
    }
}
