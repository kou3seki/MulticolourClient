using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Client;

namespace Client
{

    public class CardOpeartion : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        ClientCore clientCore;
        Card card;

        // Start is called before the first frame update
        void Awake()
        {
            clientCore = GameObject.Find("Player").GetComponent<ClientCore>();
            card = GetComponent<Card>();
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (card.cardGroup != null)
            {
                clientCore.cardControl.pointedCard = card;
            }

            if (card.cardGroup == null || card.cardGroup.isVisible)
            {
                clientCore.cardDisPlay.SetDisplay(card);
            }
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (card.cardGroup != null)
            {
                clientCore.cardControl.pointedCard = null;
            }

            clientCore.cardDisPlay.ClearDisplay();
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if(card.cardGroup != null)
            {
                if(eventData.button == PointerEventData.InputButton.Left)
                {
                    if (card.cardGroup.isPile && card.cardGroup.deskAreaName != ("Deck" + (1 - clientCore.playerIndex)))
                    {
                        clientCore.targetSelect.InitSelect(card.cardGroup);
                    }
                }

                if (eventData.button == PointerEventData.InputButton.Right)
                {
                    clientCore.markerInput.CreateInput(card);
                }
            }
            else
            {
                if (eventData.button == PointerEventData.InputButton.Right)
                {
                    clientCore.deckEditorCore.deckInEditor.RemoveCard(card);
                }

                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    clientCore.deckEditorCore.deckInEditor.CreateCard(card.cardData);
                }
            }
        }
    }
}

