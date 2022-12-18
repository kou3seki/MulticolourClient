using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Client;
using UnityEngine.EventSystems;

namespace Client
{

    public class CardButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [HideInInspector] public ClientCore clientCore;

        private Image image;

        private Card card;
        private bool isActive;

        // Start is called before the first frame update
        void Awake()
        {
            clientCore = GameObject.Find("Player").GetComponent<ClientCore>();
            image = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetPosition(Vector3 position)
        {
            gameObject.transform.localPosition = position;
        }

        public void ButtonRefresh(Card card)
        {
            this.card = card;
            image.sprite = card.cardData.Image;
            isActive = true;
        }

        public void ButtonClear()
        {
            image.sprite = AllCardData.transparent;
            card = null;
            isActive = false;
        }

        private void CreateDrag()
        {
            if(card != null)
            {
                clientCore.cardControl.CreateDrag(card);
                clientCore.targetSelect.SelectCancel();
            }
        }


        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (isActive)
            {
                clientCore.cardDisPlay.SetDisplay(card.cardData);
            }
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (isActive)
            {
                clientCore.cardDisPlay.ClearDisplay();
            }
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            CreateDrag();
        }
    }
}
