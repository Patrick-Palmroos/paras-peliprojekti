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

                if (sound.playOnAwake) {
                    sound.audioSource.Play();
                }

                if (sound.loop)
                {
                    sound.audioSource.loop = true;
                }
            }
        }

        public void PlayAudio(string clipName) {
            Sound playSound = Array.Find(soundArr, s => s.audioclipName == clipName);
            playSound.audioSource.Play();
        }

        public void UpdateMixer(float v, string mixerName)
        {
            sfxGroup.audioMixer.SetFloat(mixerName, Mathf.Log10(v) * 20);
        }
    }
}
