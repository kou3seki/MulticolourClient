using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Client;
using System.Text;
using System;
using System.Linq;

namespace Client
{
    public class DeckStore : MonoBehaviour
    {
        [HideInInspector] public DeckEditorCore deckEditorCore;
        private DeckInEditor deckInEditor;
        
        [HideInInspector] public string[] deckData;
        [HideInInspector] public int currentDeck; 

        public Button deckConfirm;
        public Button deckClear;
        public Button deckSave;
        public Button deckCopy;
        public Button deckPaste;
        public Button[] deckImport;

        public Text deckPasteWarning;

        // Start is called before the first frame update
        void Awake()
        {
            deckInEditor = deckEditorCore.deckInEditor;
            currentDeck = 0;

            deckSave.onClick.AddListener(ExportDeck);
            deckClear.onClick.AddListener(deckInEditor.DeckClear);
            deckConfirm.onClick.AddListener(deckInEditor.UseDeck);
            deckCopy.onClick.AddListener(CopyDeck);
            deckPaste.onClick.AddListener(PasteDeck);

            deckImport[0].onClick.AddListener(delegate { ImportDeck(0); });
            deckImport[1].onClick.AddListener(delegate { ImportDeck(1); });
            deckImport[2].onClick.AddListener(delegate { ImportDeck(2); });
            deckImport[3].onClick.AddListener(delegate { ImportDeck(3); });
            deckImport[4].onClick.AddListener(delegate { ImportDeck(4); });
        }

        // Update is called once per frame
        void Update()
        {

        }

        public string DeckSerialization()
        {
            StringBuilder result = new StringBuilder();

            result.Append("自机：" + deckInEditor.heroineEditor.cardData.NameC + "\r\n");

            int num = 1;
            string previousName = "";
            string currentName = "";
            for (int i = 0; i< deckInEditor.deck.Count; i++)
            {
                currentName = deckInEditor.deck[i].GetComponent<Card>().cardData.NameC;
                if (currentName.Equals(previousName))
                {
                    num++;
                }
                else
                {
                    if (!previousName.Equals(""))
                    {
                        result.Append(previousName + "*" + num + "\r\n");
                        num = 1;
                    }
                }

                previousName = currentName;
            }

            if (!currentName.Equals(""))
            {
                result.Append(currentName + "*" + num + "\r\n");
            }

            return result.ToString();
        }

        public void ExportDeck()
        {
            File.WriteAllText(Application.dataPath + "/Resources/CardInfo/deck" + currentDeck + ".txt", DeckSerialization());
        }

        public void CopyDeck()
        {
            GUIUtility.systemCopyBuffer = DeckSerialization();
        }

        public void DeckDeserialization()
        {
            if (deckData.Length > 0 && !deckData[0].Equals("自机：") && !deckData[0].Equals(""))
            {
                deckInEditor.heroineEditor.CreateHeroine(AllCardData.allCardDatas[deckData[0].Substring(3)]);
            }

            if (deckData.Length > 1)
            {
                for (int i = 1; i < deckData.Length; i++)
                {
                    if (!deckData[i].Equals(""))
                    {
                        string[] temp = deckData[i].Split('*');
                        int num = int.Parse(temp[1]);
                        for (int j = 0; j < num; j++)
                        {
                            deckInEditor.CreateCard(AllCardData.allCardDatas[temp[0]]);
                        }
                    }
                }
                deckInEditor.CoordinateUpdate(0);
            }

            deckInEditor.RemainCardSet(deckInEditor.deck, deckInEditor.heroineEditor.cardData);
            deckEditorCore.searchPanel.Search("");
        }

        public void ImportDeck(int order)
        {
            deckInEditor.DeckClear();
            if (deckInEditor.heroineEditor.heroineActive)
            {
                deckInEditor.heroineEditor.HeroineSwitch();
            }
            currentDeck = order;

            for (int i = 1; i < 6; i++)
            {
                deckImport[i - 1].GetComponentInChildren<Text>().text = i.ToString();
            }
            deckImport[order].GetComponentInChildren<Text>().text = "-";


            deckData = File.ReadAllLines(Application.dataPath + "/Resources/CardInfo/deck" + order + ".txt");
            try
            {
                DeckDeserialization();
                deckPasteWarning.text = "";
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                deckPasteWarning.text = "卡组文件损坏";
            }
        }

        public void PasteDeck()
        {
            try
            {
                deckInEditor.DeckClear();
                deckData = GUIUtility.systemCopyBuffer.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                DeckDeserialization();
                deckPasteWarning.text = "";
            }
            catch(Exception e)
            {
                Debug.Log(e.ToString());
                deckInEditor.DeckClear();
                deckPasteWarning.text = "卡组序列无效";
            }
        }
    }
}

