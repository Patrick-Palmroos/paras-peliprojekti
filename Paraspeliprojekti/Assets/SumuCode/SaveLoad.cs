using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace ProjectC
{
    public class SaveLoad : MonoBehaviour
    {
        Branch[] branches;
        Meters meters;
        GameFlow flowControl;
        ButtonControls buttonControls;

        private const string SAVEEXISTS = "Save exists";
        private const string BUTTONCONTROLS = "Button controls";

        private void Start()
        {
            branches = GetComponents<Branch>();
            meters = GetComponent<Meters>();
            flowControl = GetComponent<GameFlow>();
            buttonControls = GetComponent<ButtonControls>();
        }

        public void SaveGame()
        {
            Debug.Log("Saving");
            PlayerPrefs.SetInt(SAVEEXISTS, 1);
            GameData data = new GameData();
            data.currentNodeId = flowControl.currentNodeId;
            data.happiness = meters.GetHappiness();
            data.money = meters.GetMoney();
            data.energy = meters.GetEnergy();
            data.randomNodeIds = flowControl.randomNodeIds;
            data.nextInStoryIds = flowControl.nextInStoryIds;
            data.lowMoneyIds = flowControl.lowMoneyIds;
            data.lowHappinessIds = flowControl.lowHappinessIds;
            data.lowEnergyIds = flowControl.lowEnergyIds;
            data.endNodeIds = flowControl.endNodeIds;

            // does the saving
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file;
            file = File.Create(Application.persistentDataPath + "/save.dat");
            bf.Serialize(file, data);
            file.Close();
        }

        public void LoadGame()
        {
            Debug.Log("Loading");
            if (File.Exists(Application.persistentDataPath + "/save.dat"))
            {
                //loads file
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
                GameData data = (GameData)bf.Deserialize(file);
                file.Close();

                // sets the data 
                flowControl.currentNodeId = data.currentNodeId;
                meters.SetMeters(data.happiness, data.energy, data.money);
                flowControl.randomNodeIds = data.randomNodeIds;
                flowControl.nextInStoryIds = data.nextInStoryIds;
                flowControl.lowMoneyIds = data.lowMoneyIds;
                flowControl.lowHappinessIds = data.lowHappinessIds;
                flowControl.lowEnergyIds = data.lowEnergyIds;
                flowControl.endNodeIds = data.endNodeIds;
            }
            else
            {
                Debug.Log("File does not exist");
            }

            if(PlayerPrefs.HasKey(BUTTONCONTROLS))
            {
                buttonControls.ActivateButtonControls(PlayerPrefs.GetInt(BUTTONCONTROLS) == 1);
            }

            flowControl.GameLoaded();
        }
    }

    [System.Serializable]
    class GameData
    {
        public int happiness, money, energy;
        public int currentNodeId;
        public List<int> randomNodeIds, nextInStoryIds, endNodeIds;
        public List<int> lowMoneyIds, lowEnergyIds, lowHappinessIds;
    }

}
