using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Client;

namespace Client
{
    public class Card : MonoBehaviour
    {
        [HideInInspector] public ClientCore clientCore;
        CardMove cardMove;

        public int uniqueID;
        public bool isActive;
        public CardData cardData;

        public string cardName;
        public Image cardImage;
        public Text marker;
        Canvas layer;

        Sprite imageBuffer;
        bool needRefresh;
        string markBuffer;

        public int localRotation;
        public CardGroup cardGroup;

        // Start is called before the first frame update
        void Awake()
        {
            clientCore = GameObject.Find("Player").GetComponent<ClientCore>();
            cardMove = GetComponent<CardMove>();
            layer = GetComponentInChildren<Canvas>();

            isActive = false;
        }

        // Update is called once per frame
        void Update()
        {
            CardRefresh();
        }

        public void CardRefresh(CardData cardData)
        {
            this.cardData = cardData;
            cardName = cardData.NameC;
            imageBuffer = cardData.Image;
            isActive = true;
            needRefresh = true;
        }

        public void CardClear()
        {
            cardData = AllCardData.derivant;
            cardName = "";
            imageBuffer = AllCardData.filledSqure;
            needRefresh = true;
            isActive = false;
            needRefresh = true;

            if (cardGroup != null)
            {
                cardGroup.RemoveCard(this);
            }
        }

        public void SetMarker(string s)
        {
            markBuffer = s;
            needRefresh = true;
        }

        public void SetVisible(bool visible)
        {
            if (!visible)
            {
                imageBuffer = AllCardData.back;
            }
            else
            {
                imageBuffer = cardData.Image;
            }
            needRefresh = true;
        }

        public void CardRefresh()
        {
            if (needRefresh)
            {
                marker.text = markBuffer;
                cardImage.sprite = imageBuffer;
                if (!isActive && cardGroup == null)
                {
                    gameObject.transform.position = CardPool.CardInitLoc;
                }
                needRefresh = false;
            }
        }

        public void SetMove(Vector3 targetPosition, Quaternion targetAngle, bool atOnce)
        {
            SetMove(targetPosition, targetAngle, localRotation, atOnce);
        }

        public void SetMove(Vector3 targetPosition, Quaternion targetAngle, int localRotation, bool atOnce)
        {
            if (atOnce)
            {
                cardMove.DirectedMove(targetPosition, targetAngle, localRotation);
            }
            else
            {
                cardMove.SetPositionAndRotation(targetPosition, targetAngle, localRotation);
            }
        }

        public void SetLayer(string layer, int order)
        {
            this.layer.sortingLayerName = layer;
            this.layer.sortingOrder = order;
        }
    }
}