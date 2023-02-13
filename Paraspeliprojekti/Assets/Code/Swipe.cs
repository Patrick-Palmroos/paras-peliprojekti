using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProjectC
{
    public class Swipe : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Vector2 startPos;
        private float distanceMoved;
        private double threshold = 0.4 * Screen.width;
        private bool swipeLeft;
        GameObject control;

        public void Awake()
        {
            control = GameObject.FindGameObjectWithTag("GlobalControl");
            StartCoroutine(FadeCard());
        }

        public void ChangeContent(string text, bool affectHappy, bool affectMoney, bool affectEnergy)
        {
            gameObject.GetComponentInChildren<Text>().text = text;
            Image happyIndicator = transform.Find("indicators").Find("happy").GetComponent<Image>();
            Image moneyIndicator = transform.Find("indicators").Find("money").GetComponent<Image>();
            Image energyIndicator = transform.Find("indicators").Find("energy").GetComponent<Image>();
            happyIndicator.enabled = affectHappy;
            moneyIndicator.enabled = affectMoney;
            energyIndicator.enabled = affectEnergy;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            startPos = transform.localPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.localPosition = new Vector2(transform.localPosition.x + eventData.delta.x, transform.localPosition.y);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            distanceMoved = Mathf.Abs(transform.localPosition.x - startPos.x);
            if (distanceMoved < threshold)
            {
                transform.localPosition = startPos;
            }
            else
            {
                if (transform.localPosition.x > startPos.x)
                {
                    swipeLeft = false;
                }
                else
                {
                    swipeLeft = true;
                }

                // swipe has happened

                StartCoroutine(SwipeCard());
            }
        }

        private IEnumerator FadeCard()
        {
            Color transparent = new Color(1, 1, 1, 0);
            GetComponent<Image>().color = transparent;
            float alpha = GetComponent<Image>().color.a;
            for (float t = 0f; t < 1f; t += Time.deltaTime / 0.5f)
            {
                Color alphaChange = new Color(1, 1, 1, Mathf.Lerp(alpha, 1f, t));
                GetComponent<Image>().color = alphaChange;
                yield return null;
            }
        }

        private IEnumerator SwipeCard()
        {
            Color transparent = new Color(1, 1, 1, 0);
            float destPos;
            float time = 0;
            float halfSize = GetComponent<RectTransform>().rect.width / 2;
            float leftSide = transform.localPosition.x - Screen.width - halfSize;
            float rightSide = transform.localPosition.x + Screen.width + halfSize;
            while (GetComponent<Image>().color != new Color(1, 1, 1, 0))
            {
                time += Time.deltaTime;
                if (swipeLeft)
                {
                    destPos = leftSide;
                }
                else
                {
                    destPos = rightSide;
                }
                transform.localPosition = new Vector2(Mathf.SmoothStep(transform.localPosition.x, destPos, 4 * time), transform.localPosition.y);
                GetComponent<Image>().color = new Color(1, 1, 1, Mathf.SmoothStep(1, 0, 4 * time));
                yield return null;
            }
            Destroy(gameObject);
        }
    }

}
