using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;
using System;

namespace Client
{
    public class AllDeskArea : MonoBehaviour
    {
        [HideInInspector] public ClientCore clientCore;

        public GameObject deskArea;
        [HideInInspector] public Dictionary<string, DeskArea> deskAreas = new Dictionary<string, DeskArea>();

        // Start is called before the first frame update
        void Awake()
        {
            clientCore = GameObject.Find("Player").GetComponent<ClientCore>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void MoveCard(Card card, CardGroup targetCardGroup)
        {
            if (clientCore.playerIndex <= 1)
            {
                clientCore.connectToSever.Send(ToSever.MoveCard(card.uniqueID, -1, targetCardGroup.deskAreaName));
            }
        }
    }
}

