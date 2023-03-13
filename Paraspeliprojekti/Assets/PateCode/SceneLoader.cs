using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectC
{
    public class SceneLoader : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            StartCoroutine(SceneLoaderCoroutine(sceneName));
        }

        IEnumerator SceneLoaderCoroutine(string sceneName)
        {
            AsyncOperation asyncLoader = SceneManager.LoadSceneAsync(sceneName);
            while (!asyncLoader.isDone)
            {
                yield return null;
            }
        }
    }
}
