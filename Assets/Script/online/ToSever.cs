using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;

namespace Client
{

    public class ToSever
    {

        public static string JoinRoom(string nickname)
        {
            return "IntoRoom$" + (nickname.Equals("") ? "ÄäÃû" : nickname);
        }

        public static string MoveCard(int cardIndex, int order, string target)
        {
            return "MoveCard$" + cardIndex +"|" + order + "|" + target;
        }

        public static string RotateCard(int cardIndex, int angle)
        {
            return "RotateCard$" + cardIndex + "|" + angle;
        }

        public static string SetMarker(int cardIndex, string marker)
        {
            return "SetMarker$" + cardIndex + "|" + marker;
        }

        public static string SetDeskNum(int[] deskNum)
        {
            string output = "SetDeskNum$";

            foreach (int num in deskNum)
            {
                output += (num + "|");
            }

            return output;
        }

        public static string UseDeck(DeckInEditor deckInEditor)
        {
            string output = deckInEditor.heroineEditor.cardData.CardOrder.ToString();

            foreach (Card card in deckInEditor.deck)
            {
                output += ("|" + card.GetComponent<Card>().cardData.CardOrder);
            }

            return "UseDeck$" + output;
        }

        public static string Snapshoot()
        {
            return "Snapshoot$" + 0;
        }

        public static string ChangeSeat(int i)
        {
            return "ChangeSeat$" + i;
        }
    }
}
