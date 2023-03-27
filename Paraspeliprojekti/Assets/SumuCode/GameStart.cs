using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectC
{
    public class GameStart : MonoBehaviour
    {
        [SerializeField] private GameObject helpScreen, meters, indicators;

        // Start is called before the first frame update
        void Start()
        {
            meters.SetActive(false);
            indicators.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
