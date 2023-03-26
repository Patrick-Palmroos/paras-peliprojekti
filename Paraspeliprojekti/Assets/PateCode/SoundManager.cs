using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace ProjectC
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup musicGroup;
        [SerializeField] private AudioMixerGroup sfxGroup;
        float sfxVol = 100f, musicVol = 100f;
        [SerializeField] private Sound[] soundArr;

        private void Awake()
        {
            foreach (Sound sound in soundArr) {
                sound.audioSource = gameObject.AddComponent<AudioSource>();
                sound.audioSource.clip = sound.audioClip;

                switch (sound.soundType) {
                    case Sound.SoundType.music:
                        sound.audioSource.outputAudioMixerGroup = musicGroup;
                        break;
                    case Sound.SoundType.sfx:
                        sound.audioSource.outputAudioMixerGroup = sfxGroup;
                        break;
                }

                if (sound.loop)
                {
                    sound.audioSource.loop = true;
                }
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            GameObject[] tempArr = GameObject.FindGameObjectsWithTag("SoundManager");
            if (tempArr.Length > 1)
            {
                if (tempArr[0] != this.gameObject) {
                    Destroy(tempArr[0].gameObject);
                } else
                {
                    Destroy(tempArr[1].gameObject);
                }
            }
        }

        public void PlayAudio(string clipName) {
            Sound playSound = Array.Find(soundArr, s => s.audioclipName == clipName);
            playSound.audioSource.Play();
        }

        public void UpdateMixer(float v, string mixerName)
        {
            if (mixerName == "Sfx Volume")
            {
                sfxGroup.audioMixer.SetFloat(mixerName, Mathf.Log10(v) * 20);
                sfxVol = v;
            } else
            {
                musicGroup.audioMixer.SetFloat(mixerName, Mathf.Log10(v) * 20);
                musicVol = v;
            }
        }

        public float GetVolume(string name)
        {
            float temp = 0;
            if (name == "sfx")
            {
                temp = sfxVol;
            } else
            {
                temp = musicVol;
            }

            return temp;
        }

        public void StopAudio(string clipName)
        {
            Sound stopSound = Array.Find(soundArr, s => s.audioclipName == clipName);
            stopSound.audioSource.Stop();
        }

        public void StopGroup(Sound.SoundType type)
        {
            foreach (Sound sound in soundArr)
            {
                if (sound.soundType == type)
                {
                    sound.audioSource.Stop();
                }
            }
        }
    }
}
