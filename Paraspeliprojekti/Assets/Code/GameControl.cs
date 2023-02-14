using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ProjectC
{
    public class GameControl : MonoBehaviour
    {
        private Branch[] branches;
        [SerializeField] GameObject card;
        [SerializeField] private TMP_Text optionText;
        int branchCount;
        int currBranch;
        string optionLeft, optionRight;

        // Start is called before the first frame update
        void Start()
        {
            branches = GetComponents<Branch>();
            branchCount = branches.Length;
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

        public void Right()
        {
            Debug.Log("Swipe right");
        }

        public void Left()
        {
            Debug.Log("Swipe Left");
        }

    }
}
