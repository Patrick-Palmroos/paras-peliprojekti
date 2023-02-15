using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectC
{
    // WIP NOT TESTED
    public class Meters : MonoBehaviour
    {
        [SerializeField] Image happyMeter, moneyMeter, energyMeter;
        [SerializeField] Image affectHappy, affectMoney, affectEnergy;
        int tempHappy, tempMoney, tempEnergy;
        int currHappy, currMoney, currEnergy;
        int factor = 100;
        private float meterSpeed = 0.5f;
        bool changeFill = false;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.H))
            {
                AddToMeters(1, 10, 20);
            }

            if(Input.GetKeyDown(KeyCode.J))
            {
                AddToMeters(-1, -10, -20);
            }

            if(changeFill)
            {
                Adjust();
            }
        }

        public void AddToMeters(int changeHappy, int changeMoney, int changeEnergy)
        {
            currHappy = tempHappy + changeHappy;
            currMoney = tempMoney + changeMoney;
            currEnergy = tempEnergy + changeEnergy;
            changeFill = true;
        }

        public void Adjust()
        {
            happyMeter.fillAmount = Mathf.Lerp((float)(tempHappy / factor), (float)(currHappy / factor), Time.deltaTime * meterSpeed);
            moneyMeter.fillAmount = Mathf.Lerp((float)(tempMoney / factor), (float)(currMoney / factor), Time.deltaTime * meterSpeed);
            energyMeter.fillAmount = Mathf.Lerp((float)(tempEnergy / factor), (float)(currEnergy / factor), Time.deltaTime * meterSpeed);
            tempHappy = (int)(happyMeter.fillAmount * factor);
            tempMoney = (int)(moneyMeter.fillAmount * factor);
            tempEnergy = (int)(energyMeter.fillAmount * factor);

            if (tempHappy == currHappy && tempMoney == currMoney && tempEnergy == currEnergy)
            {
                changeFill = false;
            }
        }

        private void AffectMeter(Image meter, string meterName)
        {
            float tempFill = 0f;
            float currFill = 0f; ;
            switch(meterName)
            {
                case "happy":
                    tempFill = tempHappy;
                    currFill = currHappy;
                    break;
                case "money":
                    tempFill = tempMoney;
                    currFill = currMoney;
                    break;
                case "energy":
                    tempFill = tempEnergy;
                    currFill = currEnergy;
                    break;
            }
            tempFill = (float)(tempFill / 100);
            currFill = (float)(currFill / 100);
            meter.fillAmount = Mathf.Lerp(tempFill, currFill, Time.deltaTime * meterSpeed);
        }
    }
}
