using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectC
{
    public class ChoiceScr : MonoBehaviour
    {
        [SerializeField] bool hasContinuation, hasLongTermEffects;
        [SerializeField] GameObject leftSwipePref, rightSwipePref;
        [SerializeField] int[] rightSwipeStats = new int[4];
        [SerializeField] int[] leftSwipeStats = new int[4];
        StatMaster stats;

        private void Awake()
        {
            stats = GameObject.Find("Stats").GetComponent<StatMaster>();
        }
        //false for left and true for right
        public void selectedChoice(bool rightOrLeft)
        {
            if (hasContinuation)
            {
                switch (rightOrLeft)
                {
                    case true:
                        Instantiate(rightSwipePref, Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0)),
                            Quaternion.identity, transform.parent.parent);
                        stats.applier(rightSwipeStats[0], rightSwipeStats[1], rightSwipeStats[2], rightSwipeStats[3]);
                        break;
                    case false:
                        Instantiate(leftSwipePref, Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0)),
                            Quaternion.identity, transform.parent.parent);
                        stats.applier(leftSwipeStats[0], leftSwipeStats[1], leftSwipeStats[2], leftSwipeStats[3]);
                        break;
                }
                Destroy(transform.parent.gameObject);
            }

            if (hasLongTermEffects)
            {
                //figure it out when it comes to it.
            }
        }
    }
}
