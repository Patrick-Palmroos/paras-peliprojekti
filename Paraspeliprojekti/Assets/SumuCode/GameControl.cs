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

        // Start is called before the first frame update
        void Start()
        {
            allBranches = GetComponents<Branch>().ToList<Branch>();
            branches = allBranches;
            branchCount = branches.Count;
            RandomBranch();
            GetCurrentOptions();
            Debug.Log(branchCount + " branches");
        }

        public void GetCurrentOptions()
        {
            Debug.Log(currBranch);
            if (branches[currBranch].currNode == null)
            {
                Debug.Log("null");
            } else
            {
                Debug.Log("Not null");
                optionLeft = branches[currBranch].currNode.OptionLeft;
                optionRight = branches[currBranch].currNode.OptionRight;
            }
            cardText.text = branches[currBranch].currNode.Prompt;
        }

        public void RandomBranch()
        {
            currBranch = Random.Range(0, branchCount);
        }

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

        public void Swiped(string state)
        {
            bool left = (state == "Left");
            if(left)
            {
                Debug.Log("swiped left");
                if (branches[currBranch].IsLastNode(branches[currBranch].currNode.ChildLeft))
                {
                    Debug.Log("Next story node is the last one");
                }
            } 
            else
            {
                Debug.Log("swiped right");
                if (branches[currBranch].IsLastNode(branches[currBranch].currNode.ChildRight))
                {
                    Debug.Log("Next story node is the last one");
                }
            }

            branches[currBranch].GetNextNode(left);
            RandomBranch();
            GetCurrentOptions();
        }

    }
}
