using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

namespace ProjectC
{
    public class GameControl : MonoBehaviour
    {
        private List<Branch> allBranches;
        private List<Branch> branches;
        [SerializeField] private TMP_Text cardText;
        [SerializeField] private TMP_Text optionText;
        int branchCount;
        int currBranch;
        string optionLeft, optionRight;
        bool endGame = false;

        // Start is called before the first frame update
        void Start()
        {
            allBranches = GetComponents<Branch>().ToList<Branch>();
            branches = new List<Branch>();
            foreach (Branch b in allBranches)
            {
                branches.Add(b);
            }
            RandomBranch();
        }

        // Displays current prompt and options on screen
        public void GetCurrentOptions(Branch b)
        {
            if (b.currNode == null)
            {
                Debug.Log("null branch");
            } else
            {
                optionLeft = b.currNode.OptionLeft;
                optionRight = b.currNode.OptionRight;
                cardText.text = b.currNode.Prompt;
            }
        }

        // Randomizes which branch to check next
        public void RandomBranch()
        {
            branchCount = branches.Count;
            if (branchCount <= 0)
            {
                endGame = true;
                currBranch = Random.Range(0, allBranches.Count);
                GetCurrentOptions(allBranches[currBranch]);
            }
            else
            {
                currBranch = Random.Range(0, branchCount);
                GetCurrentOptions(branches[currBranch]);
            }
        }

        // Shows the text depending on which side the card is being swiped to
        // Called from another script
        public void ChangeText(string state)
        {
            switch(state)
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

        // Happens when the swipe is done
        public void Swiped(string state)
        {
            bool left = (state == "Left");
            if (endGame)
            {
                // If we're in endgame, meaning that we don't have any choices left
                // we will cycle through the end cards until they run out
                // and then the game ends
                if (allBranches.Count - 1 > 0)
                {
                    allBranches.Remove(allBranches[currBranch]);
                    RandomBranch();
                }
                else
                {
                    GameEnd();
                }
            }
            else
            {
                // If we're not in endgame, we check if the next child node is the end card
                // and if it is, remove it from the list and save it for endgame
                branches[currBranch].GetNextNode(left);
                if (branches[currBranch].IsLastNode(branches[currBranch].currNode.Id))
                {
                    Debug.Log("Next node is the last node");
                    branches.Remove(branches[currBranch]);
                }

                RandomBranch();
            }
        }

        // Game ends
        public void GameEnd()
        {
            Debug.Log("Game ends");
        }
    }
}
