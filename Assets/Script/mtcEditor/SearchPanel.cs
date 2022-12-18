using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Client;
using System.Linq;

namespace Client
{
    public class SearchPanel : MonoBehaviour
    {
        [HideInInspector] public DeckEditorCore deckEditorCore;

        public InputField cardName;
        public Button resultUp;
        public Button resultDown;
        public GameObject buttonPrefab;
        public GameObject resultPanel;
        private AddCardButton[] addCardButtons;
        private CardData[] result;
        private int displayIndex;

        public Button[] colorFilter;
        private Text[] colorFilterX;
        private bool[] colorFilterOpen;

        // Start is called before the first frame update
        void Awake()
        {
            cardName.onValueChanged.AddListener(Search);

            resultUp.onClick.AddListener(IndexMinus);
            resultDown.onClick.AddListener(IndexAdd);

            addCardButtons = new AddCardButton[19];
            for (int i = 0; i < addCardButtons.Length; i++)
            {
                GameObject addCardButton = Instantiate(buttonPrefab, resultPanel.transform);
                addCardButton.transform.localPosition += new Vector3(0, 270 - 30 * i, 0);
                addCardButtons[i] = addCardButton.GetComponent<AddCardButton>();
                addCardButtons[i].deckEditorCore = deckEditorCore;
            }

            colorFilterX = new Text[5];
            colorFilterOpen = new bool[5];
            for (int i = 0; i < 5; i++)
            {
                colorFilterX[i] = colorFilter[i].GetComponentInChildren<Text>();
                colorFilterOpen[i] = false;
            }
            colorFilter[0].onClick.AddListener(delegate { ColorFilter(0); });
            colorFilter[1].onClick.AddListener(delegate { ColorFilter(1); });
            colorFilter[2].onClick.AddListener(delegate { ColorFilter(2); });
            colorFilter[3].onClick.AddListener(delegate { ColorFilter(3); });
            colorFilter[4].onClick.AddListener(delegate { ColorFilter(4); });

            result = AllCardData.allCardDatas.Values.ToArray<CardData>();
            displayIndex = 0;
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ColorFilter(int i)
        {
            if (colorFilterOpen[i])
            {
                colorFilterOpen[i] = false;
                colorFilterX[i].text = "";
            }
            else
            {
                colorFilterOpen[i] = true;
                colorFilterX[i].text = "¡Ì";
            }
            Search("");
        }

        public void DisplayRefresh()
        {
            if(result != null)
            {
                for (int i = 0; i < addCardButtons.Length; i++)
                {
                    if (displayIndex + i > result.Length - 1)
                    {
                        addCardButtons[i].ButtonRefresh();
                        resultDown.interactable = false;
                    }
                    else
                    {
                        addCardButtons[i].ButtonRefresh(result[displayIndex + i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < addCardButtons.Length; i++)
                {
                    addCardButtons[i].ButtonRefresh();
                    resultDown.interactable = false;
                }
            }

            if (displayIndex == 0) resultUp.interactable = false;
            if (result == null || displayIndex + addCardButtons.Length >= result.Length) resultDown.interactable = false;
        }

        public void IndexAdd()
        {
            displayIndex++;
            DisplayRefresh();
            resultUp.interactable = true;
        }

        public void IndexMinus()
        {
            displayIndex--;
            DisplayRefresh();
            resultDown.interactable = true;
        }

        public void Search(string keywords)
        {
            //string keywords = cardName.text;
            resultUp.interactable = true;
            resultDown.interactable = true;
            displayIndex = 0;

            if (keywords.Equals("") && !deckEditorCore.deckInEditor.heroineEditor.heroineActive
                && !colorFilterOpen[0] && !colorFilterOpen[1] && !colorFilterOpen[2] && !colorFilterOpen[3] && !colorFilterOpen[4])
            {
                this.result = AllCardData.allCardDatas.Values.ToArray<CardData>();
            }
            else
            {
                List<CardData> result = new List<CardData>();

                foreach (CardData target in AllCardData.allCardDatas.Values)
                {
                    bool flag = target.NameC.Contains(keywords);
                    if (!flag)
                    {
                        foreach (string tag in target.Tags)
                        {
                            if (tag.Equals(keywords))
                            {
                                flag = true;
                            }
                        }
                    }

                    if (!flag)
                    {
                        flag = target.Describe.Contains(keywords);
                    }

                    if (deckEditorCore.deckInEditor.heroineEditor.heroineActive && flag)
                    {
                        flag = false;
                        foreach (string tag in target.Tags)
                        {
                            if (tag.Equals("×Ô»ú"))
                            {
                                flag = true;
                            }
                        }
                    }

                    if (flag && (colorFilterOpen[0] || colorFilterOpen[1] || colorFilterOpen[2] || colorFilterOpen[3] || colorFilterOpen[4]))
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            if(target.BaseCost[i] !=0 && !colorFilterOpen[i] || target.BaseCost[i] == 0 && colorFilterOpen[i])
                            {
                                flag = false;
                            }
                        }
                    }


                    if (flag)
                    {
                        result.Add(target);
                    }
                }

                if (result.Count == 0)
                {
                    this.result = null;
                }
                else
                {
                    this.result = result.ToArray();
                }
            }

            DisplayRefresh();
        }
    }
}


