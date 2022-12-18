using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Client;

namespace Client
{
    public class DeckInEditor : MonoBehaviour
    {
        public const int Xcount = 9;
        public const int Ycount = 6;
        [HideInInspector] public DeckEditorCore deckEditorCore;

        public GameObject point1;//¿¨ÅÆ²Î¿¼µã¼ÆËã
        public GameObject point2;
        public Vector3 interval;

        public Button heroineButton;
        public Button closeEditor;
        [HideInInspector] public HeroineEditor heroineEditor;

        [HideInInspector] public List<Card> deck;
        [HideInInspector] public Dictionary<string, int> remainCard;

        // Start is called before the first frame update
        void Awake()
        {
            SetInterval();

            heroineEditor = heroineButton.GetComponent<HeroineEditor>();
            heroineEditor.deckEditorCore = deckEditorCore;

            deck = new List<Card>();
            remainCard = new Dictionary<string, int>();
            RemainCardInit();

            closeEditor.onClick.AddListener(OnEditorClose);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void SetInterval()
        {
            interval = new Vector3((point2.transform.position.x - point1.transform.position.x )/( Xcount - 1 ),
                (point2.transform.position.y - point1.transform.position.y) /( Ycount - 1),
                (point2.transform.position.z - point1.transform.position.z) / (Ycount - 1));
        }

        private void RemainCardInit()
        {
            if(remainCard.Count == 0)
            {
                foreach (string name in AllCardData.allCardDatas.Keys)
                {
                    remainCard.Add(name, AllCardData.allCardDatas[name].MaxNum);
                }
            }
            else
            {
                foreach (string name in AllCardData.allCardDatas.Keys)
                {
                    remainCard[name] = AllCardData.allCardDatas[name].MaxNum;
                }
            }
        }

        public void RemainCardSet(List<Card> cards, CardData heroine)
        {
            RemainCardInit();
            if(heroine.NameC != null)
            {
                remainCard[heroine.NameC] -= 4;
            }

            foreach (Card card in cards)
            {
                remainCard[card.cardData.NameC] --;
            }
        }

        public void CoordinateUpdate(int start)
        {
            for (int i = start;i < deck.Count;i++)
            {
                if(i < 7)
                {
                    deck[i].SetMove(point1.transform.position + new Vector3((i + 2) * interval.x, 0, 0), point1.transform.rotation, false);
                }
                else if(i < 14)
                {
                    deck[i].SetMove(point1.transform.position + new Vector3((i - 5) * interval.x, interval.y, interval.z ), point1.transform.rotation, false);
                }
                else
                {
                    deck[i].SetMove(point1.transform.position + new Vector3((i - 14)%Xcount * interval.x, ((i - 14)/Xcount+2) * interval.y, 
                        ((i - 14) / Xcount + 2) * interval.z), point1.transform.rotation, false);
                }
            }
        }

        public void CanUseJudge()
        {
            if (deck.Count > 0 && heroineEditor.cardData.NameC != "" && deckEditorCore.clientCore.playerIndex <=1)
            {
                deckEditorCore.deckStore.deckConfirm.interactable = true;
            }
            else
            {
                deckEditorCore.deckStore.deckConfirm.interactable = false;
            }
        }

        public void AddCard(Card cardAdd)
        {
            int index = 0;
            foreach (Card card in deck)
            {
                if (cardAdd.GetComponent<Card>().cardData.CardOrder < card.GetComponent<Card>().cardData.CardOrder) break;
                else index++;
            }
            deck.Insert(index, cardAdd);
            CoordinateUpdate(index);
        }
        
        public void CreateCard(CardData cardData)
        {
            if (deck.Count < 50 && remainCard[cardData.NameC] >0)
            {
                Card newCard = deckEditorCore.IdleCard();
                newCard.GetComponent<Card>().CardRefresh(cardData);
                Card cardAction = newCard.GetComponent<Card>();
                cardAction.SetMove(point2.transform.position, Quaternion.Euler(new Vector3(-45, 0, 0)), true);
                remainCard[cardData.NameC]--;
                deckEditorCore.searchPanel.DisplayRefresh();
                AddCard(newCard);
            }
        }

        public void RemoveCard(Card needRemove)
        {
            remainCard[needRemove.GetComponent<Card>().cardData.NameC]++;
            deckEditorCore.searchPanel.DisplayRefresh();
            deck.Remove(needRemove);
            needRemove.CardClear();
            CoordinateUpdate(0);
        }

        public void DeckClear()
        {
            for (int i = 0; i < deck.Count; i++)
            {
                deck[i].CardClear();
            }

            deck = new List<Card>();
            heroineEditor.HeroineClear();
            RemainCardInit();
            deckEditorCore.searchPanel.DisplayRefresh();
            CoordinateUpdate(0);
        }

        public void UseDeck()
        {
            deckEditorCore.clientCore.connectToSever.Send(ToSever.UseDeck(this)) ;
            deckEditorCore.deckStore.ExportDeck();
            OnEditorClose();
        }

        public void OnEditorClose()
        {
            DeckClear();
            deckEditorCore.SetVisible(false, 0);
        }
    }
}

