using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;

namespace Client
{
    public class DeckEditorCore : MonoBehaviour
    {
        [HideInInspector] public ClientCore clientCore;

        public GameObject deckEditor;
        [HideInInspector] public RectTransform rectTransform;
        [HideInInspector] public Vector3 orginalLoc;

        [HideInInspector] public DeckInEditor deckInEditor;
        [HideInInspector] public SearchPanel searchPanel;
        [HideInInspector] public DeckStore deckStore;

        [HideInInspector] public Card[] cardsInEditor;

        // Start is called before the first frame update
        void Awake()
        {
            clientCore = GetComponent<ClientCore>();

            deckInEditor = deckEditor.GetComponent<DeckInEditor>();
            deckInEditor.deckEditorCore = this;
            searchPanel = deckEditor.GetComponent<SearchPanel>();
            searchPanel.deckEditorCore = this;
            deckStore = deckEditor.GetComponent<DeckStore>();
            deckStore.deckEditorCore = this;

            deckEditor.SetActive(true);
            rectTransform = deckEditor.transform as RectTransform;
            orginalLoc = rectTransform.anchoredPosition3D;
            SetVisible(false, 0);
        }

        void Start()
        {
            cardsInEditor = new Card[50];
            for (int i = 0; i < cardsInEditor.Length; i++)
            {
                cardsInEditor[i] = clientCore.cardPool.CreateCard(i);
                cardsInEditor[i].gameObject.transform.localScale = new Vector3(0.92f, 1.3f, 0.02f);
                cardsInEditor[i].SetLayer("UI", 1);
            }
        }

        // Update is called once per frame
        void Update()
        {
            deckInEditor.CanUseJudge();
        }

        public Card IdleCard()
        {
            foreach (Card card in cardsInEditor)
            {
                if (!card.isActive)
                {
                    return card;
                }
            }
            return null;
        }

        public void SetVisible(bool isVisible, int player)
        {
            if (isVisible)
            {
                if (player == 0)
                {
                    rectTransform.anchoredPosition3D = orginalLoc;
                    deckInEditor.SetInterval();
                }
                else
                {
                    rectTransform.anchoredPosition3D = new Vector3(174, 0, 0);
                    deckInEditor.SetInterval();
                }
            }
            else
            {
                deckEditor.transform.position = ClientCore.cantSeeLoc;
            }
        }
    }
}


