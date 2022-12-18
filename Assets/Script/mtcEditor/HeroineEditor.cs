using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Client;

namespace Client
{
    public class HeroineEditor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [HideInInspector] public DeckEditorCore deckEditorCore;

        public CardData cardData;

        public bool heroineActive;
        private Text text;
        private Button button;
        private Image image;
        private float t;

        // Start is called before the first frame update
        void Awake()
        {
            button = GetComponent<Button>();
            text = GetComponentInChildren<Text>();
            image = GetComponent<Image>();
            button.onClick.AddListener(HeroineSwitch);
        }

        // Update is called once per frame
        void Update()
        {
            HeroineColorSwitch();
        }

        public void HeroineColorSwitch()
        {
            t += Time.deltaTime;
            byte colorValue = ((byte)(127 * Mathf.Sin(t) + 127));
            text.color = new Color32(colorValue, colorValue, colorValue, 255);
        }

        public void HeroineSwitch()
        {
            if (heroineActive)
            {
                heroineActive = false;
                text.text = "点击选择自机";
                deckEditorCore.searchPanel.Search("");
            }
            else
            {
                heroineActive = true;
                text.text = "右侧选择自机\n再次点击确认";
                deckEditorCore.searchPanel.Search("");
            }
        }

        public void CreateHeroine(CardData cardData)
        {
            if(this.cardData.NameC != null)
            {
                deckEditorCore.deckInEditor.remainCard[this.cardData.NameC] = 4;
            }

            this.cardData = cardData;
            image.sprite = cardData.Image;
            deckEditorCore.deckInEditor.remainCard[cardData.NameC] = 0;
            deckEditorCore.searchPanel.DisplayRefresh();
            deckEditorCore.deckInEditor.CoordinateUpdate(deckEditorCore.deckInEditor.deck.Count);
        }

        public void HeroineClear()
        {
            cardData = new CardData();
            GetComponent<Image>().sprite = AllCardData.rect;
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            if (cardData.Image != null)
            {
                deckEditorCore.clientCore.cardDisPlay.SetDisplay(cardData);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            deckEditorCore.clientCore.cardDisPlay.ClearDisplay();
        }
    }
}

