using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectC
{
    public class LanguageSetup : MonoBehaviour
    {
        void Awake()
        {
            if(PlayerPrefs.HasKey("Language"))
            {
                FindObjectOfType<SceneLoader>().LoadScene("Menu");
            }
        }
    }
}
