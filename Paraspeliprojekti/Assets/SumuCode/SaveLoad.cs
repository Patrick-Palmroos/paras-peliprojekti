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

        private void Start()
        {
            branches = GetComponents<Branch>();
            meters = GetComponent<Meters>();
            control = GetComponent<GameControl>();
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

            PlayerPrefs.SetInt("Happiness", meters.GetHappiness());
            PlayerPrefs.SetInt("Energy", meters.GetEnergy());
            PlayerPrefs.SetInt("Money", meters.GetMoney());
            PlayerPrefs.SetInt("Current branch", control.GetCurrBranch());
            print("Saved");
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

            int happiness = PlayerPrefs.GetInt("Happiness");
            int energy = PlayerPrefs.GetInt("Energy");
            int money = PlayerPrefs.GetInt("Money");
            meters.SetMeters(happiness, energy, money);
            print(happiness + " " + energy + " " + money);
            int currentBranch = PlayerPrefs.GetInt("Current branch");
            control.GameLoaded(currentBranch);
            print("Loaded");
        }
    }
}
