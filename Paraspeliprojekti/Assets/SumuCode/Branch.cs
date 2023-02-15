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
        [SerializeField] string filePath;
        public StoryNode currNode;

        public void Awake()
        {
            LoadFromFile(filePath);
            currNode = RootNode;
        }

        // Moves on to the child node
        // Called from another script
        public StoryNode GetNextNode(bool left)
        {
            StoryNode childNode;
            if(left)
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

        // Loads from file
        public void LoadFromFile(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] parts = line.Split('\t');

                    int id = int.Parse(parts[0]);
                    string description = parts[1];
                    int leftId = int.Parse(parts[2]);
                    string leftOption = parts[3];
                    int leftHappy = int.Parse(parts[4]);
                    int leftMoney = int.Parse(parts[5]);
                    int leftEnergy = int.Parse(parts[6]);
                    int rightId = int.Parse(parts[7]);
                    string rightOption = parts[8];
                    int rightHappy = int.Parse(parts[9]);
                    int rightMoney = int.Parse(parts[10]);
                    int rightEnergy = int.Parse(parts[11]);
                    StoryNode node = new StoryNode
                    {
                        Id = id,
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
            }

            RootNode = GetNode(0);
        }

        // Finds a node by its ID
        private StoryNode GetNode(int id)
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
            StoryNode sn = GetNode(id);
            if(sn.ChildLeft == -1 && sn.ChildRight == -1)
            {
                return true;
            }
            return false;
        }
    }
}
