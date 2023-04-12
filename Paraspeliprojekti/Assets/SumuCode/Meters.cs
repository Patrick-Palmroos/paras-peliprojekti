using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectC
{
    public class Meters : MonoBehaviour
    {
        [SerializeField] Image happyMeter, moneyMeter, energyMeter;
        [SerializeField] Image affectHappy, affectMoney, affectEnergy;
        [SerializeField] private int happy, money, energy;
        int factor = 100;
        private float meterSpeed = 3f;
        const string outOfHappiness = "unhappy";
        const string outOfMoney = "no money";
        const string outOfEnergy = "burnout";

        SceneLoader loader;

        // Start is called before the first frame update
        void Start()
        {
            loader = GetComponent<SceneLoader>();
        }

        // Update is called once per frame
        void Update()
        {
            // Adjusts the meters if the values are changed
            if(ValuesChanged())
            {
                Adjust(happyMeter, happyMeter.fillAmount, ConvertToFillAmount(happy));
                Adjust(moneyMeter, moneyMeter.fillAmount, ConvertToFillAmount(money));
                Adjust(energyMeter, energyMeter.fillAmount, ConvertToFillAmount(energy));
            }

            /*
            // GAME ENDS
            if (happy <= 0)
            {
                GameEnd(outOfHappiness);
            }

            if (money <= 0)
            {
                GameEnd(outOfMoney);
            }

            if(energy <= 0)
            {
                GameEnd(outOfEnergy);
            }
            */
        }

        // Shows the indicators if the choice can affect that value
        public void ShowIndicators(bool affectsHappiness, bool affectsMoney, bool affectsEnergy)
        {
            affectHappy.enabled = affectsHappiness;
            affectMoney.enabled = affectsMoney;
            affectEnergy.enabled = affectsEnergy;
        }

        // Returns true if the value has changed, false if not
        private bool ValuesChanged()
        {
            if(happyMeter.fillAmount != ConvertToFillAmount(happy))
            {
                return true;
            }

            if(moneyMeter.fillAmount != ConvertToFillAmount(money))
            {
                return true;
            }

            if(energyMeter.fillAmount != ConvertToFillAmount(energy))
            {
                return true;
            }

            return false;
        }

        // Adds points to the meters
        public void AddToMeters(int addHappy, int addMoney, int addEnergy)
        {
            happy += addHappy;
            money += addMoney;
            energy += addEnergy;

            if (happy > 100)
                happy = 100;
            if (money > 100)
                money = 100;
            if (energy > 100)
                energy = 100;
        }

        // Converts the values to fill amount for the image
        private float ConvertToFillAmount(int value)
        {
            return (float)value / factor;
        }

        // Smoothly adjusts the meters
        private void Adjust(Image meter, float from, float to)
        {
            meter.fillAmount = Mathf.Lerp(from, to, Time.deltaTime * meterSpeed);
        }

        /*
        private void GameEnd(string state)
        {
            switch(state)
            {
                case outOfHappiness:
                    if (StoryControl.IsFinnish())
                    {
                        StoryControl.gameEndText = "Onnellisuus loppui!";
                    }
                    else
                    {
                        StoryControl.gameEndText = "Ran out of happiness!";
                    }

                    loader.LoadScene("GameOver");
                    break;
                case outOfMoney:
                    if (StoryControl.IsFinnish())
                    {
                        StoryControl.gameEndText = "Rahat loppuivat!";
                    }
                    else
                    {
                        StoryControl.gameEndText = "Ran out of money";
                    }

                    loader.LoadScene("GameOver");
                    break;
                case outOfEnergy:
                    if (StoryControl.IsFinnish())
                    {
                        StoryControl.gameEndText = "Energiat loppuivat!";
                    }
                    else
                    {
                        StoryControl.gameEndText = "Ran out of energy";
                    }

                    loader.LoadScene("GameOver");
                    break;
            }
        }*/

        public int GetHappiness()
        {
            return happy;
        }

        public int GetEnergy()
        {
            return energy;
        }

        public int GetMoney()
        {
            return money;
        }

        public void SetMeters(int happiness, int energy, int money)
        {
            this.happy = happiness;
            this.energy = energy;
            this.money = money;
        }

        public bool GameWillEnd(int affectHappiness, int affectMoney, int affectEnergy)
        {
            if (happy + affectHappiness <= 0)
            {
                if (StoryControl.IsFinnish())
                {
                    StoryControl.gameEndText = "Onnellisuus loppui!";
                }
                else
                {
                    StoryControl.gameEndText = "Ran out of happiness!";
                }
                return true;
            }

            if (money + affectMoney <= 0)
            {
                if (StoryControl.IsFinnish())
                {
                    StoryControl.gameEndText = "Rahat loppuivat!";
                }
                else
                {
                    StoryControl.gameEndText = "Ran out of money";
                }
                return true;
            }

            if (energy + affectEnergy <= 0)
            {
                if (StoryControl.IsFinnish())
                {
                    StoryControl.gameEndText = "Energiat loppuivat!";
                }
                else
                {
                    StoryControl.gameEndText = "Ran out of energy";
                }
                return true;
            }

            return false;
        }
    }
}
