using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Client;

namespace Client
{

    public class CardControl : MonoBehaviour
    {
        [HideInInspector]public ClientCore clientCore;

        [HideInInspector] public DeskArea areaTarget;
        [HideInInspector] public Card pointedCard;
        private Card cardInDrag;

        public GameObject movePlatform;

        // Start is called before the first frame update
        void Awake()
        {
            clientCore = GameObject.Find("Player").GetComponent<ClientCore>();
        }

        // Update is called once per frame
        void Update()
        {
            CardRotation();
            CardDrag();
        }

        private void CardDrag()
        {
            if (Input.GetMouseButtonDown(0) && pointedCard != null)
            {
                if (!pointedCard.cardGroup.isPile)
                {
                    CreateDrag(pointedCard);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                ConfirmDrag();
            }

            DragCardMove();
        }

        public void CreateDrag(Card card)
        {
            cardInDrag = card;
            clientCore.allCardGroup.deskArea.SetActive(true);
            cardInDrag.SetLayer("Desk", 100);
        }

        public void ConfirmDrag()
        {
            if (cardInDrag != null)
            {
                if (areaTarget != null)
                {
                    clientCore.allDeskArea.MoveCard(cardInDrag, areaTarget.cardGroup);
                }

                cardInDrag.cardGroup.waitRefresh = CardGroup.maxRefreshTime;
                clientCore.allCardGroup.deskArea.SetActive(false);
                cardInDrag = null;
            }
        }

        public void DragCardMove()
        {
            if (cardInDrag != null)
            {
                RectTransform platform = movePlatform.transform as RectTransform;
                RectTransformUtility.ScreenPointToWorldPointInRectangle(platform, Input.mousePosition, clientCore.mainCamera, out Vector3 mousePos);
                cardInDrag.GetComponent<Card>().SetMove(mousePos, cardInDrag.transform.rotation, true);
            }
        }

        private void CardRotation()
        {
            if (Input.GetKeyDown(KeyCode.Q) && !clientCore.markerInput.selected)
            {
                if (pointedCard != null && !pointedCard.cardGroup.isPile)
                {
                    clientCore.connectToSever.Send(ToSever.RotateCard(pointedCard.uniqueID, pointedCard.localRotation +90));
                }
            }

            if (Input.GetKeyDown(KeyCode.E) && !clientCore.markerInput.selected)
            {
                if (pointedCard != null && !pointedCard.cardGroup.isPile)
                {
                    clientCore.connectToSever.Send(ToSever.RotateCard(pointedCard.uniqueID, pointedCard.localRotation - 90));
                }
            }

            if (Input.GetKeyDown(KeyCode.W) && !clientCore.markerInput.selected)
            {
                if (pointedCard != null && !pointedCard.cardGroup.isPile)
                {
                    clientCore.connectToSever.Send(ToSever.MoveCard(pointedCard.uniqueID, 0, "Deck" + clientCore.playerIndex));
                }
            }
        }
    }
}

