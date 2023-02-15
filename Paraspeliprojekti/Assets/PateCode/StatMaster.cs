using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ProjectC
{
    public class StatMaster : MonoBehaviour
    {
        int money, productivity, morale, happiness;
        GameObject[] statUI;

        private void Awake()
        {
            statUI = GameObject.FindGameObjectsWithTag("stat");
            statUIUpdater();
        }
        void statUIUpdater()
        {
            foreach (GameObject stat in statUI)
            {
                switch (stat.name)
                {
                    case "Money":
                        stat.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = money.ToString();
                        break;
                    case "Productivity":
                        stat.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = productivity.ToString();
                        break;
                    case "Morale":
                        stat.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = morale.ToString();
                        break;
                    case "Happiness":
                        stat.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = happiness.ToString();
                        break;
                }
            }
        }
        public void applier(int money, int productivity, int morale, int happiness)
        {
            this.money += money;
            this.productivity += productivity;
            this.morale += morale;
            this.happiness += happiness;

            statUIUpdater();
        }
    }
}
