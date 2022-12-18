using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Client;

namespace Client
{

    public class DeskOperation : MonoBehaviour
    {
        [HideInInspector] public ClientCore clientCore;

        public GameObject deskOpeartion;

        public Button drawCard;
        public Button drawCost;
        public Button display;
        public Button shuffle;

        public Button reset;
        public Button[] lifeSet;
        public Button randomGen;

        public Text[] deckNums;
        public bool needRefresh;
        public int[] life;


        // Start is called before the first frame update
        void Awake()
        {
            clientCore = GameObject.Find("Player").GetComponent<ClientCore>();
            drawCard.onClick.AddListener(delegate { MoveCard("Hand" + clientCore.playerIndex); } );
            drawCost.onClick.AddListener(delegate { MoveCard("Palette" + clientCore.playerIndex); });
            display.onClick.AddListener(delegate { MoveCard("Battlefield2" + clientCore.playerIndex); });
            shuffle.onClick.AddListener(delegate { clientCore.connectToSever.Send("Shuffle$0"); });
            reset.onClick.AddListener(ResetCard);

            lifeSet[0].onClick.AddListener(delegate { life[clientCore.playerIndex]++; SetDeskNum(); });
            lifeSet[1].onClick.AddListener(delegate { life[clientCore.playerIndex]--; SetDeskNum(); });
            lifeSet[2].onClick.AddListener(delegate { life[clientCore.playerIndex] = 20; SetDeskNum(); });
            randomGen.onClick.AddListener(delegate {clientCore.connectToSever.Send("Chat$得到随机数" + Random.Range(1, 20)); });

            life = new int[2] {20, 20};
        }

        // Update is called once per frame
        void Update()
        {
            if (needRefresh)
            {
                for (int i = 0; i < 2; i++)
                {
                    deckNums[i].text = life[i].ToString();
                }
                needRefresh = false;
            }
        }

        public Card GetFirstCard(string cardGroupName)
        {
            CardGroup cardGroup = clientCore.allCardGroup.cardGroups[cardGroupName];
            if(cardGroup.cardList.Count != 0)
            {
                return cardGroup.cardList[cardGroup.cardList.Count-1].GetComponent<Card>();
            }
            else
            {
                return null;
            }
        }

        public void MoveCard(string cardGroupName)
        {
            Card temp = GetFirstCard("Deck" + clientCore.playerIndex);
            if (temp != null)
            {
                clientCore.connectToSever.Send(ToSever.MoveCard(temp.uniqueID, -1, cardGroupName)) ;
            }
        }

        public void SetInteractible(bool can)
        {
            drawCard.interactable = can;
            drawCost.interactable = can;
            display.interactable = can;
            shuffle.interactable = can;
            reset.interactable = can;
            randomGen.interactable = can;
            for(int i = 0; i < 3; i++)
            {
                lifeSet[i].interactable = can;
            }
        }

        public void SetDeskNum()
        {
            clientCore.connectToSever.Send(ToSever.SetDeskNum(life));
        }

        public void ResetCard()
        {
            foreach (Card card in clientCore.allCardGroup.cardGroups["Palette" + clientCore.playerIndex].cardList)
            {
                if (card.localRotation != 0)
                {
                    clientCore.connectToSever.Send(ToSever.RotateCard(card.uniqueID, 0));
                }
            }

            foreach (Card card in clientCore.allCardGroup.cardGroups["Battlefield1" + clientCore.playerIndex].cardList)
            {
                if (card.localRotation != 0)
                {
                    if (card.cardData.BaseValue != null || card.cardData.Tags.Contains("道具"))
                    {
                        clientCore.connectToSever.Send(ToSever.RotateCard(card.uniqueID, 0));
                    }
                }
            }

            foreach (Card card in clientCore.allCardGroup.cardGroups["Battlefield2" + clientCore.playerIndex].cardList)
            {
                if (card.localRotation != 0)
                {
                    if (card.cardData.BaseValue != null || card.cardData.Tags.Contains("道具"))
                    {
                        clientCore.connectToSever.Send(ToSever.RotateCard(card.uniqueID, 0));
                    }
                }
            }
        }
    }
}

