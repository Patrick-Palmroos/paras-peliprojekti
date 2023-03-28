using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectC
{
    public class SaveLoad : MonoBehaviour
    {
        Branch[] branches;
        Meters meters;
        GameControl control;

        private const string HAPPINESS = "Happiness";
        private const string ENERGY = "Energy";
        private const string MONEY = "Money";
        private const string CURRENTBRANCH = "Current branch";
        private const string SAVEEXISTS = "Save exists";

        private void Start()
        {
            branches = GetComponents<Branch>();
            meters = GetComponent<Meters>();
            control = GetComponent<GameControl>();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                PlayerPrefs.DeleteAll();
            }
        }

        public void SaveData()
        {
            foreach (Branch b in branches)
            {
                if (!b.RandomBranch())
                {
                    string branchName = b.fileName;
                    int id = b.currNode.Id;
                    PlayerPrefs.SetInt(branchName, id);
                    print(branchName + " " + id);
                }
                else
                {
                    List<int> nodesRemaining = b.RemainingNodes();
                    int nodeCount = nodesRemaining.Count;
                    string branchName = b.fileName;
                    PlayerPrefs.SetInt(branchName, nodeCount);
                    int index = 0;
                    foreach (int i in nodesRemaining)
                    {
                        PlayerPrefs.SetInt(branchName + index.ToString(), i);
                        print(branchName + index.ToString());
                        index++;
                    }
                }
            }

            PlayerPrefs.SetInt(HAPPINESS, meters.GetHappiness());
            PlayerPrefs.SetInt(ENERGY, meters.GetEnergy());
            PlayerPrefs.SetInt(MONEY, meters.GetMoney());
            PlayerPrefs.SetInt(CURRENTBRANCH, control.GetCurrBranch());
            PlayerPrefs.SetInt(SAVEEXISTS, 1);
        }

        public void LoadData()
        {
            foreach(Branch b in branches)
            {
                string branchName = b.fileName;
                if(!b.RandomBranch())
                {
                    b.currNode = b.GetNode(PlayerPrefs.GetInt(branchName));
                }
                else
                {
                    List<int> remainingNodesList = new List<int>();
                    int nodeCount = PlayerPrefs.GetInt(branchName);
                    for(int i = 0; i < nodeCount; i++)
                    {
                        int remainingNodeId = PlayerPrefs.GetInt(branchName + i);
                        remainingNodesList.Add(remainingNodeId);
                        print(remainingNodeId);
                    }
                }
            }

            int happiness = PlayerPrefs.GetInt(HAPPINESS);
            int energy = PlayerPrefs.GetInt(ENERGY);
            int money = PlayerPrefs.GetInt(MONEY);
            meters.SetMeters(happiness, energy, money);
            print(happiness + " " + energy + " " + money);
            int currentBranch = PlayerPrefs.GetInt(CURRENTBRANCH);
            control.GameLoaded(currentBranch);
        }
    }
}
