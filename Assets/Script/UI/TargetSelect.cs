using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Client;
using static Client.TargetSelect;

namespace Client
{

    public class TargetSelect : MonoBehaviour
    {
        [HideInInspector] public ClientCore clientCore;

        public GameObject panel;
        [HideInInspector]public Vector3 orginalLoc;

        public GameObject cardButtonPrefab;
        private CardButton[] cardRange;
        public Button cancel;

        // Start is called before the first frame update
        void Awake()
        {
            clientCore = GameObject.Find("Player").GetComponent<ClientCore>();
            panel.SetActive(true);
            orginalLoc = panel.transform.localPosition;
            SetVisible(false);
            cancel.onClick.AddListener(SelectCancel);

            cardRange = new CardButton[54];

            RectTransform rect = panel.transform as RectTransform;
            Vector3 interval = new Vector3(rect.rect.width / 9, rect.rect.height / 6, 0);

            for (int i = 0; i < cardRange.Length; i++)
            {
                cardRange[i] = Instantiate(cardButtonPrefab, panel.transform).GetComponent<CardButton>();
                cardRange[i].SetPosition(new Vector3((4 - i % 9) * interval.x, -(2.5f - i / 9) * interval.y, 0));
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void InitSelect(CardGroup cardGroup)
        {
            SetVisible(true);
            List<Card> cardsAfterSort = new List<Card>();
            bool isAdded = false;
            for (int i = 0; i < cardGroup.cardList.Count; i++)
            {
                for (int j = 0; j < cardsAfterSort.Count; j++)
                {
                    if (cardGroup.cardList[i].GetComponent<Card>().cardData.CardOrder < cardsAfterSort[j].GetComponent<Card>().cardData.CardOrder)
                    {
                        cardsAfterSort.Insert(j, cardGroup.cardList[i]);
                        isAdded = true;
                        break;
                    }
                }

                if (!isAdded)
                {
                    cardsAfterSort.Add(cardGroup.cardList[i]);
                }
                isAdded = false;
            }

            for(int i = 0; i < cardsAfterSort.Count; i++)
            {
                cardRange[i].ButtonRefresh(cardsAfterSort[i].GetComponent<Card>());
            }
        }

        public void SelectCancel()
        {
            foreach (CardButton card in cardRange)
            {
                card.ButtonClear();
            }
            SetVisible(false);
        }

        public void SetVisible(bool isVisible)
        {
            if (isVisible)
            {
                panel.transform.localPosition = orginalLoc;
            }
            else
            {
                panel.transform.position = ClientCore.cantSeeLoc;
            }
        }
    }
}
