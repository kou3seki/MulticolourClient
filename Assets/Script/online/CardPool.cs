using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;

namespace Client
{
    public class CardPool : MonoBehaviour
    {
        public static Vector3 CardInitLoc = new Vector3(0, 0, -100);
        public static int CardPoolCap = 60;
        [HideInInspector] public ClientCore clientCore;
        public GameObject cardPrefab;

        public Card[] cards;

        // Start is called before the first frame update
        void Awake()
        {
            clientCore = GameObject.Find("Player").GetComponent<ClientCore>();
            CardPoolInit();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void CardPoolInit()
        {
            cards = new Card[2*CardPoolCap + 1];
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i] = CreateCard(i);
            }

            cards[2 * CardPoolCap] = CreateCard(2 * CardPoolCap);
            cards[2 * CardPoolCap].CardRefresh(AllCardData.sweetpotato);
        }

        public Card CreateCard(int index)
        {
            GameObject cardModel = Instantiate(cardPrefab, CardInitLoc, Quaternion.identity);
            Card card = cardModel.GetComponent<Card>();
            card.uniqueID = index;

            return card;
        }

        public void CardPoolClear(int player)
        {
            for (int i = 0; i < CardPoolCap; i++)
            {
                cards[i + CardPoolCap * player].CardClear();
            }
        }

        public void ImportDeck(int player, string[] deck)
        {
            CardPoolClear(player);
            
            for (int i = 1; i < deck.Length; i++)
            {
                if (!deck[i].Equals(""))
                {
                    cards[i + CardPoolCap * player -1].CardRefresh(AllCardData.allCardDatas_list[int.Parse(deck[i])]);
                }
            }
        }
    }
}
