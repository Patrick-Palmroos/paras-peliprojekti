using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ProjectC
{
    public class GameFlow : MonoBehaviour
    {
        [HideInInspector] public int currentNodeId = 1;
        private List<StoryNode> storyNodes;
        [HideInInspector] public List<int> randomNodeIds, nextInStoryIds, endNodeIds;
        [HideInInspector] public List<int> lowMoneyIds, lowEnergyIds, lowHappinessIds;
        public string fileName;
        [SerializeField] private TMP_Text cardText, optionText, nameText;
        [SerializeField] private SpriteRenderer image;
        [SerializeField] private GameObject backgroundCard;
        private string optionLeft, optionRight;
        private bool endGame = false;

        Meters meters;
        SaveLoad loader;
        private const string RANDOMNODE = "[RANDOM]";
        private const string LOWMONEY = "[LOWMONEY]";
        private const string LOWENERGY = "[LOWENERGY]";
        private const string LOWHAPPINESS = "[LOWHAPPINESS]";

        private int lowThreshold = 20;

        // Start is called before the first frame update
        void Start()
        {
            meters = FindObjectOfType<Meters>();
            loader = FindObjectOfType<SaveLoad>();
            LoadFromFile();

            if (StoryControl.state == StoryControl.StartState.LoadGame)
            {
                loader.LoadGame();
            }
            else
            {
                CheckStatus();
            }
        }

        public void CheckStatus()
        {

            // Checks if any status is low before giving completely random cards
            if (meters.GetHappiness() < lowThreshold && lowHappinessIds.Count > 0)
            {
                // LOW HAPPINESS
                currentNodeId = RandomId(lowHappinessIds);
                lowHappinessIds.Remove(currentNodeId);
            } 
            else if (meters.GetMoney() < lowThreshold && lowMoneyIds.Count > 0)
            {
                // LOW MONEY
                currentNodeId = RandomId(lowMoneyIds);
                lowMoneyIds.Remove(currentNodeId);
            }
            else if (meters.GetEnergy() < lowThreshold && lowEnergyIds.Count > 0)
            {
                // LOW ENERGY
                currentNodeId = RandomId(lowEnergyIds);
                lowEnergyIds.Remove(currentNodeId);
            }
            else
            {
                RandomCard();
            }
            GetCurrentOptions();
        }

        // Gives a random card that hasn't been played yet
        public void RandomCard()
        {
            if (nextInStoryIds.Count > 0)
            {
                // If a story can be continued, first check if random cards also exist
                if (randomNodeIds.Count > 0)
                {
                    // there is a chance a story card will still be played
                    int chance = Random.Range(1, 5);
                    if (chance <= 2)
                    {
                        // PLAY STORY CARD
                        currentNodeId = RandomId(nextInStoryIds);
                        nextInStoryIds.Remove(currentNodeId);
                    }
                    else
                    {
                        // PLAY RANDOM CARD
                        currentNodeId = RandomId(randomNodeIds);
                        randomNodeIds.Remove(currentNodeId);
                    }
                }
                else
                {
                    // There is no random cards so the story must be continued
                    currentNodeId = RandomId(nextInStoryIds);
                    nextInStoryIds.Remove(currentNodeId);
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
                    else
                    {
                        GameEnd();
                    }
                }
            }
        }

        public int RandomId(List<int> list)
        {
            int randomNumber = Random.Range(0, list.Count);
            return list[randomNumber];
        }

        // Displays current options and texts on screen
        public void GetCurrentOptions()
        {
            StoryNode currNode = GetNodeById(currentNodeId);

            optionLeft = currNode.OptionLeft;
            optionRight = currNode.OptionRight;
            cardText.text = currNode.Prompt;
            nameText.text = currNode.Name;
            Debug.Log(currNode.Name);

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

        // Reads the text file
        public void LoadFromFile()
        {
            storyNodes = new List<StoryNode>();
            nextInStoryIds = new List<int>();
            endNodeIds = new List<int>();
            randomNodeIds = new List<int>();
            lowEnergyIds = new List<int>();
            lowHappinessIds = new List<int>();
            lowMoneyIds = new List<int>();

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
                    Name = cardName,
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
                if(description.StartsWith(RANDOMNODE))
                {
                    node.Prompt = description.Replace(RANDOMNODE, "");           
                    randomNodeIds.Add(id);
                }
                else if (description.StartsWith(LOWENERGY))
                {
                    node.Prompt = description.Replace(LOWENERGY, ""); 
                    lowEnergyIds.Add(id);
                }
                else if (description.StartsWith(LOWHAPPINESS))
                {
                    node.Prompt = description.Replace(LOWHAPPINESS, "");
                    lowHappinessIds.Add(id);
                }
                else if (description.StartsWith(LOWMONEY))
                {
                    node.Prompt = description.Replace(LOWMONEY, "");
                    lowMoneyIds.Add(id);
                }

                storyNodes.Add(node);
            }

            Debug.Log("Game loaded");
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
                    int leftId = GetNodeById(currentNodeId).ChildLeft;
                    if (leftId == 0)
                    {

                    }
                    else if (GetNodeById(leftId).ChildLeft == -1)
                    {
                        endNodeIds.Add(leftId);
                    }
                    else if (leftId > 1)
                    {
                        nextInStoryIds.Add(leftId);
                    }
                }
                else
                {
                    meters.AddToMeters(curr.RightHappy, curr.RightMoney, curr.RightEnergy);
                    int rightId = GetNodeById(currentNodeId).ChildRight;
                    if(rightId == 0)
                    {

                    }
                    else if (GetNodeById(rightId).ChildRight == -1)
                    {
                        endNodeIds.Add(rightId);
                    }
                    else if (rightId > 1)
                    {
                        nextInStoryIds.Add(rightId);
                    }
                }
            }
            CheckStatus();
        }

        // Shows the text depending on which side the card is being swiped to
        // Called from another script
        public void ChangeText(string state)
        {
            switch (state)
            {
                case "Right":
                    optionText.text = optionRight;
                    break;
                case "Left":
                    optionText.text = optionLeft;
                    break;
                case "":
                    optionText.text = "";
                    break;
            }
        }

        public string GetOption(bool left)
        {
            if(left)
            {
                return GetNodeById(currentNodeId).OptionLeft;
            }
            else
            {
                return GetNodeById(currentNodeId).OptionRight;
            }
        }

        // Game ends
        public void GameEnd()
        {
            SceneLoader sceneLoader = GetComponent<SceneLoader>();
            sceneLoader.LoadScene("GameOver");
        }

        public void GameLoaded()
        {
            CheckStatus();
        }
    }
}
