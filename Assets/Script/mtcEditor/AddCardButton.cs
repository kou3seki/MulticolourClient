using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Client;

namespace Client
{
    public class AddCardButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IScrollHandler
    {
        [HideInInspector] public DeckEditorCore deckEditorCore;
        [HideInInspector] public DeckInEditor deckInEditor;

        public CardData cardData;

        private Button button;
        private Text text;

        // Start is called before the first frame update
        void Awake()
        {
            button = GetComponent<Button>();
            text = GetComponentInChildren<Text>();
            button.onClick.AddListener(AddCard);
        }

        void Start()
        {
            deckInEditor = deckEditorCore.deckInEditor;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddCard()
        {
            if (deckInEditor.heroineEditor.heroineActive)
            {
                deckInEditor.heroineEditor.CreateHeroine(cardData);
            }
            else
            {
                deckInEditor.CreateCard(cardData);
            }
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            DisplayRefresh();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            deckEditorCore.clientCore.cardDisPlay.ClearDisplay();
        }

        void IScrollHandler.OnScroll(PointerEventData eventData)
        {
            if (eventData.scrollDelta.y < 0 && deckEditorCore.searchPanel.resultDown.interactable)
            {
                deckEditorCore.searchPanel.IndexAdd();
            }

            if (eventData.scrollDelta.y > 0 && deckEditorCore.searchPanel.resultUp.interactable)
            {
                deckEditorCore.searchPanel.IndexMinus();
            }

            DisplayRefresh();
        }

        public void ButtonRefresh(CardData cardData)
        {
            this.cardData = cardData;
            text.text = deckInEditor.remainCard[cardData.NameC] +  "*" + cardData.NameC;

            if (deckInEditor.remainCard[cardData.NameC] < 1 || (deckInEditor.heroineEditor.heroineActive && deckInEditor.remainCard[cardData.NameC] < 4))
            {
                button.interactable = false;
            }
            else
            {
                button.interactable = true;
            }
        }

        public void ButtonRefresh()
        {
            text.text = "";
            cardData = new CardData();
            button.interactable = false;
        }

        private void DisplayRefresh()
        {
            if (cardData.Image != null)
            {
                deckEditorCore.clientCore.cardDisPlay.SetDisplay(cardData);
            }
        }
    }
}

