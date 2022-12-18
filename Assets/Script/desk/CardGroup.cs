using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;
using UnityEngine.UI;
using Unity.Mathematics;

namespace Client
{
    public class CardGroup
    {
        public const float CardWidth = 4.8f;
        public const float CardThickness = 0.05f;
        public const float maxRefreshTime = 0.5f;
        [HideInInspector] public ClientCore clientCore;
        [HideInInspector] public DeskArea deskArea;

        public string deskAreaName;

        public bool isPile;
        public bool isVisible;
        public int lapDirection;
        public Quaternion localRotation;
        private Vector3 centerPosition;
        private float width;
        private int maxNum;
        private Vector3 interval;

        public List<Card> cardList;
        public float waitRefresh;
        public bool needRefresh;

        public CardGroup(string deskAreaName, bool isPile, bool isVisible, Quaternion localRotation, int lapDirection)
        {
            clientCore = GameObject.Find("Player").GetComponent<ClientCore>();
            this.deskAreaName = deskAreaName;
            this.isPile = isPile;
            this.isVisible = isVisible;
            this.localRotation = localRotation;
            cardList = new List<Card>();
            this.lapDirection = lapDirection;   
        }

        public void DeskAreaRefresh(DeskArea deskArea)
        {
            RectTransform rect = deskArea.transform as RectTransform;
            centerPosition = rect.anchoredPosition3D;
            if (!isPile)
            {
                width = rect.rect.width;
                maxNum = (int)(width / CardWidth + 1);
            }

            this.deskArea = deskArea;
            IntervalRefresh();
        }

        public void AddCard(Card card, int selectLoc)
        {
            card.cardGroup = this;
            card.localRotation = 0;
            card.SetVisible(isVisible);

            if (selectLoc < 0)
            {
                cardList.Add(card);
            }
            else
            {
                cardList.Insert(selectLoc, card);
            }
        }

        public void RemoveCard(Card card)
        {
            cardList.Remove(card);
            card.cardGroup = null;
        }

        public void RemoveAllCard()
        {
            foreach (Card card in cardList)
            {
                card.cardGroup = null;
            }
            cardList.Clear();
        }

        public void CardPositionRefresh()
        {
            IntervalRefresh();
            for (int cardNum = 0; cardNum < cardList.Count; cardNum++)
            {
                Card card = cardList[cardNum];

                if (isPile)
                {
                    card.SetMove(centerPosition - interval * cardNum, localRotation, 0, false);
                }
                else
                {
                    card.SetMove(centerPosition -  lapDirection * interval * ((cardList.Count - 1) / 2f - cardNum), localRotation, false);
                }

                card.SetLayer("Desk", cardNum);
            }

            waitRefresh = -1;
        }

        public void SnapshootRefresh(string[] snapshoot)
        {
            RemoveAllCard();

            for (int i = 1; i < snapshoot.Length; i++)
            {
                string[] cardInfos = snapshoot[i].Split('&');
                int cardIndex = int.Parse(cardInfos[0]);
                AddCard(clientCore.cardPool.cards[cardIndex], -1);
                clientCore.cardPool.cards[cardIndex].localRotation = int.Parse(cardInfos[1]);
                clientCore.cardPool.cards[cardIndex].SetMarker(cardInfos[2]);
            }

            needRefresh = true;
        }

        public void GroupRefresh()
        {
            if (waitRefresh >= 0)
            {
                waitRefresh -= Time.deltaTime;
                if (waitRefresh < 0)
                {
                    CardPositionRefresh();
                }
            }

            if (needRefresh)
            {
                CardPositionRefresh();
                needRefresh = false;
            }
        }

        private void IntervalRefresh()
        {
            interval = isPile ? new Vector3(0, 0, CardThickness):
                (cardList.Count > maxNum ? new Vector3((width - CardWidth) / (cardList.Count - 1), 0, 0) : new Vector3(CardWidth, 0, 0));
        }
    }
}
