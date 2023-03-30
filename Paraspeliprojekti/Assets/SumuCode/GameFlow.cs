using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ProjectC
{
    public class GameFlow : MonoBehaviour
    {
        private int currentNodeId = 1;
        private List<StoryNode> storyNodes;
        // private List<StoryNode> randomNodes;
        private List<int> continuedStoryIds, randomNodeIds, nextInStoryIds, endNodeIds;
        public string fileName;
        [SerializeField] private TMP_Text cardText, optionText, nameText;
        [SerializeField] private SpriteRenderer image;
        [SerializeField] private GameObject backgroundCard;
        private string optionLeft, optionRight;
        private bool endGame = false;

        Meters meters;

        // Start is called before the first frame update
        void Start()
        {
            meters = FindObjectOfType<Meters>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void RandomCard()
        {
            if (nextInStoryIds.Count > 0)
            {
                // if a story can be continued, randomly decide to continue it or not
                int chance = Random.Range(1, 5);
                if(chance <= 2)
                {
                    currentNodeId = RandomId(continuedStoryIds);
                    continuedStoryIds.Remove(currentNodeId);
                }
                else
                {
                    currentNodeId = RandomId(randomNodeIds);
                    randomNodeIds.Remove(currentNodeId);
                }
            }
            else
            {
                // if a story can't be continued, check if we're in endgame
                if (randomNodeIds.Count > 0)
                {
                    currentNodeId = RandomId(randomNodeIds);
                    randomNodeIds.Remove(currentNodeId);
                }
                else
                {
                    // if we're in endgame, loop end cards until they run out
                    endGame = true;
                    if (endNodeIds.Count > 0)
                    {
                        currentNodeId = RandomId(endNodeIds);
                    }
                }
            }
        }

        public int RandomId(List<int> list)
        {
            int randomNumber = Random.Range(0, list.Count);
            return list[randomNumber];
        }

        public void GetCurrentOption()
        {
            StoryNode currNode = GetNodeById(currentNodeId);

            optionLeft = currNode.OptionLeft;
            optionRight = currNode.OptionRight;
            cardText.text = currNode.Prompt;
            nameText.text = currNode.Name;

            // changes the pic but only if it's a different one than the previous
            if (image.sprite.name != currNode.ImageName)
            {
                Sprite cardImage = Resources.Load<Sprite>("Art/Character cards/" + currNode.ImageName);
                image.sprite = cardImage;
            }

            bool affectsHappy = (currNode.LeftHappy != 0 || currNode.RightHappy != 0);
            bool affectsMoney = (currNode.LeftMoney != 0 || currNode.RightMoney != 0);
            bool affectsEnergy = (currNode.LeftEnergy != 0 || currNode.RightEnergy != 0);
            meters.ShowIndicators(affectsHappy, affectsMoney, affectsEnergy);
        }

        public StoryNode GetNodeById(int id)
        {
            foreach (StoryNode sn in storyNodes)
            {
                if(sn.Id == id)
                {
                    return sn;
                }
            }

            return null;
        }

        public void LoadFromFile()
        {
            storyNodes = new List<StoryNode>();
            TextAsset textData = Resources.Load("Story/" + fileName) as TextAsset;
            string txt = textData.text;
            var lines = txt.Split("\n");

            foreach(var line in lines)
            {
                string[] parts = line.Split("\t");
                int id = int.Parse(parts[0].Trim());
                string cardName = parts[1];
                string imageName = parts[2];
                string description = parts[3];
                int leftId = int.Parse(parts[4].Trim());
                string leftOption = parts[5];
                int leftHappy = int.Parse(parts[6].Trim());
                int leftMoney = int.Parse(parts[7].Trim());
                int leftEnergy = int.Parse(parts[8].Trim());
                int rightId = int.Parse(parts[9].Trim());
                string rightOption = parts[10];
                int rightHappy = int.Parse(parts[11].Trim());
                int rightMoney = int.Parse(parts[12].Trim());
                int rightEnergy = int.Parse(parts[13].Trim());

                StoryNode node = new StoryNode
                {
                    Id = id,
                    Name = name,
                    ImageName = imageName,
                    Prompt = description,
                    OptionLeft = leftOption,
                    ChildLeft = leftId,
                    LeftHappy = leftHappy,
                    LeftMoney = leftMoney,
                    LeftEnergy = leftEnergy,
                    OptionRight = rightOption,
                    ChildRight = rightId,
                    RightHappy = rightHappy,
                    RightMoney = rightMoney,
                    RightEnergy = rightEnergy
                };

                // checks if the node is supposed to be random or not
                if(description.ToUpper().StartsWith("[RANDOM]"))
                {
                    // randomNodes.Add(node);
                    randomNodeIds.Add(id);
                }
                else
                {
                    // storyNodes.Add(node);
                    continuedStoryIds.Add(id);
                }

                storyNodes.Add(node);
            }
        }

        public void Swiped(string state)
        {
            bool left = (state == "Left");
            if (endGame)
            {
                // If we're in endgame, meaning that we don't have any choices left
                // we will cycle through the end cards until they run out
                // and then the game ends
                if (endNodeIds.Count - 1 > 0)
                {
                    if (endNodeIds.Count - 1 == 1)
                    {
                        backgroundCard.SetActive(false);
                    }

                    endNodeIds.Remove(currentNodeId);
                    /*
                    allBranches.Remove(allBranches[currBranch]);
                    RandomBranch();*/
                }
                else
                {
                    GameEnd();
                    return;
                }
            }
            else
            {
                // If we're not in endgame, we check if the next child node is the end card
                // and if it is, remove it from the list and save it for endgame
                StoryNode curr = GetNodeById(currentNodeId);

                if (left)
                {
                    meters.AddToMeters(curr.LeftHappy, curr.LeftMoney, curr.LeftEnergy);
                }
                else
                {
                    meters.AddToMeters(curr.RightHappy, curr.RightMoney, curr.RightEnergy);
                }
                /*
                branches[currBranch].GetNextNode(left);

                if (branches[currBranch].RandomBranch())
                {
                    if (branches[currBranch].NodeCount() <= 0)
                    {
                        allBranches.Remove(branches[currBranch]);
                        branches.Remove(branches[currBranch]);
                        Debug.Log("All randoms are played");
                        if (allBranches.Count <= 0)
                        {
                            GameEnd();
                            return;
                        }
                    }
                }
                else
                {
                    if (branches[currBranch].IsLastNode(branches[currBranch].currNode.Id))
                    {
                        Debug.Log("Next node is the last node");
                        branches.Remove(branches[currBranch]);
                    }
                }*/

                // RandomBranch();
            }
        }

        // Game ends
        public void GameEnd()
        {
            SceneLoader sceneLoader = GetComponent<SceneLoader>();
            sceneLoader.LoadScene("GameOver");
        }
    }
}
