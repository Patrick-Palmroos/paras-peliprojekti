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

        public void Start()
        {
            LoadFromFile("Assets/Story/branchTest.tsv");
            PlayStory(RootNode);
        }

        private void PlayStory(StoryNode node)
        {
            ShowMessage(node.Prompt);

            // If this node has no children, the story has ended
            if (node.ChildLeft == -1 && node.ChildRight == -1)
            {
                Debug.Log("no children");
                return;
            }


            string choice = ShowChoice(node.OptionLeft, node.OptionRight);
            StoryNode nextNode;
            if (choice == node.OptionLeft)
            {
                nextNode = GetNode(node.ChildLeft);
            }
            else
            {
                nextNode = GetNode(node.ChildRight);
            }

            // Play the story from the selected child node
            PlayStory(nextNode);
        }

        // this currently returns optionLeft every time, wip
        private string ShowChoice(string optionLeft, string optionRight)
        {
            return optionLeft;
        }

        private void ShowMessage(string message)
        {
            // This function would normally display the message to the player, but in this example we'll just print it to the console.
            Debug.Log("message: " + message);
        }

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
                    int rightId = int.Parse(parts[4]);
                    string rightOption = parts[5];
                    StoryNode node = new StoryNode
                    {
                        Id = id,
                        Prompt = description,
                        OptionLeft = leftOption,
                        ChildLeft = leftId,
                        OptionRight = rightOption,
                        ChildRight = rightId
                    };

                    nodes[id] = node;
                }
            }

            RootNode = GetNode(0);
        }

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
    }

    public class StoryNode
    {
        public int Id { get; set; }
        public string Prompt { get; set; }
        public string OptionLeft { get; set; }
        public string OptionRight { get; set; }
        public int ChildLeft { get; set; }
        public int ChildRight { get; set; }
    }
}
