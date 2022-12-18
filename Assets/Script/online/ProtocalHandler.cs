using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;
using static UnityEngine.GraphicsBuffer;

namespace Client
{

    public class ProtocalHandler
    {
        public ClientCore clientCore;

        public delegate void Handler(string input);
        public Dictionary<string, Handler> handlers;

        public ProtocalHandler(ClientCore clientCore)
        {
            this.clientCore = clientCore;
            handlers = new Dictionary<string, Handler>()
            {
                {nameof(Chat), Chat },
                {nameof(RoomInfo), RoomInfo },
                {nameof(MoveCard), MoveCard },
                {nameof(RotateCard), RotateCard },
                {nameof(PartialSnapshoot), PartialSnapshoot },
                {nameof(SetDeskNum), SetDeskNum },
                {nameof(SetMarker), SetMarker },
                {nameof(UseDeck), UseDeck },

            };
        }

        public void Chat(string input)
        {
            clientCore.chat.AddLog(input);
        }

        public void UseDeck(string input)
        {
            string[] temp = input.Split('|');
            clientCore.cardPool.ImportDeck(int.Parse(temp[0]), temp);
        }

        public void RoomInfo(string input)
        {
            string[] temp = input.Split('|');
            int l = temp.Length;

            for (int i = 0; i < l-1; i++)
            {
                clientCore.roomStatus.nickNames[i] = temp[i];
            }

            clientCore.roomStatus.needRefresh = true;
            clientCore.playerIndex = int.Parse(temp[l-1]);
        }

        public void MoveCard(string input)
        {
            string[] temp = input.Split('|');
            int cardIndex = int.Parse(temp[0]);
            CardGroup orgin = clientCore.cardPool.cards[cardIndex].cardGroup;
            CardGroup target = clientCore.allCardGroup.cardGroups[temp[2]];
            orgin.RemoveCard(clientCore.cardPool.cards[cardIndex]);
            orgin.needRefresh = true;
            target.AddCard(clientCore.cardPool.cards[cardIndex], int.Parse(temp[1]));
            target.needRefresh = true;
        }

        public void RotateCard(string input)
        {
            string[] temp = input.Split('|');
            int cardIndex = int.Parse(temp[0]);
            clientCore.cardPool.cards[cardIndex].localRotation = int.Parse(temp[1]);
            clientCore.cardPool.cards[cardIndex].cardGroup.needRefresh = true;
        }

        public void PartialSnapshoot(string input)
        {
            string[] cards = input.Split('|');
            clientCore.allCardGroup.cardGroups[cards[0]].SnapshootRefresh(cards);
        }

        public void SetDeskNum(string input)
        {
            clientCore.deskOperation.life[0] = int.Parse(input.Split('|')[0]);
            clientCore.deskOperation.life[1] = int.Parse(input.Split('|')[1]);
            clientCore.deskOperation.needRefresh = true;
        }

        public void SetMarker(string input)
        {
            string[] temp = input.Split('|');
            int cardIndex = int.Parse(temp[0]);

            clientCore.cardPool.cards[cardIndex].SetMarker(temp[1]);
        }
    }
}

