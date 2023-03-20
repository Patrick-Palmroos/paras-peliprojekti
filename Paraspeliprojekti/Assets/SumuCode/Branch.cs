using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace ProjectC
{
    public class Branch : MonoBehaviour
    {
        private Dictionary<int, StoryNode> nodes = new Dictionary<int, StoryNode>();
        public StoryNode RootNode { get; private set; }
        [SerializeField] public string fileName;
        public StoryNode currNode;
        [SerializeField] private bool isRandom;

        public void Awake()
        {
            // LoadFromFile(filePath);
            // currNode = RootNode;
        }

        // Moves on to the child node
        // Called from another script
        public StoryNode GetNextNode(bool left)
        {
            if (!isRandom)
            {
                StoryNode childNode;
                if (left)
                {
                    childNode = GetNode(currNode.ChildLeft);
                }
                else
                {
                    childNode = GetNode(currNode.ChildRight);
                }
                currNode = childNode;
                return childNode;
            }
            else
            {
                List<StoryNode> remainingNodes = new List<StoryNode>(nodes.Values);
                int randomIndex = Random.Range(0, remainingNodes.Count);
                StoryNode randomNode = remainingNodes[randomIndex];
                nodes.Remove(randomNode.Id);
                Debug.Log(nodes.Count + " nodes remaining");
                currNode = randomNode;
                return randomNode;
            }
        }

        // Loads from file
        public void LoadFromFile()
        {
            TextAsset textData = Resources.Load("Story/" + fileName) as TextAsset;
            string txt = textData.text;
            var lines = txt.Split("\n");
            foreach (var line in lines)
            {
                string[] parts = line.Split('\t');

                int id = int.Parse(parts[0].Trim());
                string name = parts[1];
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

                nodes[id] = node;
            }

            RootNode = GetNode(0);
        }

        // Finds a node by its ID
        public StoryNode GetNode(int id)
        {
            if (nodes.TryGetValue(id, out StoryNode node))
            {
                return node;
            }
            else
            {
                return null;
            }
        }

        // Checks by ID if this node is the last child
        public bool IsLastNode(int id)
        {
            if (!isRandom)
            {
                StoryNode sn = GetNode(id);
                if (sn.ChildLeft == -1 && sn.ChildRight == -1)
                {
                    return true;
                }
                return false;
            }
            else
            {
                if(nodes.Count > 1)
                {
                    return false;
                }
                return true;
            }
        }

        public bool RandomBranch()
        {
            return isRandom;
        }

        public int NodeCount()
        {
            return nodes.Count;
        }

        public List<int> RemainingNodes()
        {
            List<int> remainingNodes = new List<int>(nodes.Keys);
            return remainingNodes;
        }

        public void SetRemainingNodes(List<int> nodesRemaining)
        {

        }
    }
}
