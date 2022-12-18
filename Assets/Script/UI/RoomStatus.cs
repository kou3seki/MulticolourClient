using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Client;

namespace Client
{

    public class RoomStatus : MonoBehaviour
    {
        public const  int roomCap = 7;
        [HideInInspector] public ClientCore clientCore;

        public GameObject room;

        public Button[] player;
        private Text[] playerName;
        public InputField ipInput;
        public InputField nameInput;

        public Button connect;
        public Button switchCamera;
        public Button openEditor;
        public Camera mainCamera;
        public Button getSnapshoot;
        public int currentView;

        public string[] nickNames;
        public int[] flag;
        public bool needRefresh;

        private float operateCooling;
        public float maxCooling;

        // Start is called before the first frame update
        void Awake()
        {
            clientCore = GameObject.Find("Player").GetComponent<ClientCore>();

            openEditor.onClick.AddListener(OpenEditor);
            switchCamera.onClick.AddListener(SwitchCamera);
            connect.onClick.AddListener(delegate { clientCore.connectToSever.Connect(ipInput.text, nameInput.text); });
            getSnapshoot.onClick.AddListener(delegate { clientCore.connectToSever.Send(ToSever.Snapshoot()); operateCooling = maxCooling; });

            playerName = new Text[roomCap];
            for (int i = 0; i < roomCap; i++)
            {
                playerName[i] = player[i].GetComponentInChildren<Text>();
            }

            nickNames = new string[roomCap];
            flag = new int[roomCap];
        }

        void Start()
        {
            player[0].onClick.AddListener(delegate { clientCore.connectToSever.Send(ToSever.ChangeSeat(0)); operateCooling = maxCooling; });
            player[1].onClick.AddListener(delegate { clientCore.connectToSever.Send(ToSever.ChangeSeat(1)); operateCooling = maxCooling; });
            player[2].onClick.AddListener(delegate { clientCore.connectToSever.Send(ToSever.ChangeSeat(2)); operateCooling = maxCooling; });
            player[3].onClick.AddListener(delegate { clientCore.connectToSever.Send(ToSever.ChangeSeat(3)); operateCooling = maxCooling; });
            player[4].onClick.AddListener(delegate { clientCore.connectToSever.Send(ToSever.ChangeSeat(4)); operateCooling = maxCooling; });
            player[5].onClick.AddListener(delegate { clientCore.connectToSever.Send(ToSever.ChangeSeat(5)); operateCooling = maxCooling; });
            player[6].onClick.AddListener(delegate { clientCore.connectToSever.Send(ToSever.ChangeSeat(6)); operateCooling = maxCooling; });
        }

        // Update is called once per frame
        void Update()
        {
            RoomRefresh();
        }

        public void RoomRefresh()
        {
            if (needRefresh)
            {
                for (int i = 0; i < nickNames.Length; i++)
                {
                    if (clientCore.playerIndex < roomCap)
                    {
                        if (nickNames[i].Equals(""))
                        {
                            player[i].interactable = true;

                            if (i <= 1)
                            {
                                playerName[i].text = "加入对局";
                            }
                            else
                            {
                                playerName[i].text = "加入观战";
                            }
                        }
                        else
                        {
                            player[i].interactable = false;
                            playerName[i].text = nickNames[i];
                        }
                    }
                    else
                    {
                        nickNames[i] = "";
                        player[i].interactable = false;
                        playerName[i].text = "";
                        connect.interactable = true;
                        getSnapshoot.interactable = false;
                    }
                }


                if (clientCore.playerIndex <= 1)
                {
                    clientCore.deskOperation.SetInteractible(true);
                    if (currentView != clientCore.playerIndex && currentView != clientCore.playerIndex + 2)
                    {
                        if(currentView ==0 || currentView == 2)
                        {
                            currentView--;
                        }
                        else
                        {
                            currentView -= 3;
                        }
                        SwitchCamera();
                    }
                    clientCore.allCardGroup.cardGroups["Hand" + clientCore.playerIndex].isVisible = true;
                    clientCore.allCardGroup.cardGroups["Hand" + (1-clientCore.playerIndex)].isVisible = false;
                }
                else
                {
                    clientCore.deskOperation.SetInteractible(false);
                    clientCore.allCardGroup.cardGroups["Hand0"].isVisible = true;
                    clientCore.allCardGroup.cardGroups["Hand1"].isVisible = true;
                }

                if (clientCore.playerIndex < 7)
                {
                    clientCore.connectToSever.Send(ToSever.Snapshoot());
                }
                needRefresh = false;
            }

            if (operateCooling >= 0)
            {
                if(operateCooling == maxCooling)
                {
                    for (int i = 0; i < roomCap; i++)
                    {
                        player[i].interactable = false;
                    }
                    getSnapshoot.interactable=false;
                }
                operateCooling -= Time.deltaTime;
                if(operateCooling < 0)
                {
                    needRefresh = true;
                    getSnapshoot.interactable = true;
                }
            }
        }

        public void OpenEditor()
        {
            clientCore.deckEditorCore.SetVisible(true, currentView);
            clientCore.deckEditorCore.deckStore.ImportDeck(0);
            clientCore.targetSelect.SelectCancel();
        }

        public void SwitchCamera()
        {
            if(clientCore.playerIndex <= 1)
            {
                currentView+=2;
            }
            else
            {
                currentView++;
            }

            if(currentView >= 4)
            {
                currentView -=4;
            }

            switch (currentView)
            {
                case 0:
                    {
                        mainCamera.transform.SetPositionAndRotation(new Vector3(-6, -38, -28), Quaternion.Euler(new Vector3(-45, 0, 0)));
                        clientCore.allCardGroup.cardGroups["Hand0"].localRotation = Quaternion.Euler(new Vector3(-45, 0, 0));
                        clientCore.allCardGroup.cardGroups["Hand0"].CardPositionRefresh();
                        clientCore.allCardGroup.cardGroups["Hand1"].localRotation = Quaternion.Euler(new Vector3(45, 0, 180));
                        clientCore.allCardGroup.cardGroups["Hand1"].CardPositionRefresh();
                        break;
                    }
                case 1:
                    {
                        mainCamera.transform.SetPositionAndRotation(new Vector3(6, 38, -28), Quaternion.Euler(new Vector3(45, 0, 180)));
                        clientCore.allCardGroup.cardGroups["Hand0"].localRotation = Quaternion.Euler(new Vector3(-45, 0, 0));
                        clientCore.allCardGroup.cardGroups["Hand0"].CardPositionRefresh();
                        clientCore.allCardGroup.cardGroups["Hand1"].localRotation = Quaternion.Euler(new Vector3(45, 0, 180));
                        clientCore.allCardGroup.cardGroups["Hand1"].CardPositionRefresh();
                        break;
                    }
                case 2:
                    {
                        mainCamera.transform.SetPositionAndRotation(new Vector3(-6, 0, -53), Quaternion.Euler(new Vector3(0, 0, 0)));
                        clientCore.allCardGroup.cardGroups["Hand0"].localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        clientCore.allCardGroup.cardGroups["Hand0"].CardPositionRefresh();
                        clientCore.allCardGroup.cardGroups["Hand1"].localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
                        clientCore.allCardGroup.cardGroups["Hand1"].CardPositionRefresh();
                        break;
                    }
                default:
                    {
                        mainCamera.transform.SetPositionAndRotation(new Vector3(6, 0, -53), Quaternion.Euler(new Vector3(0, 0, 180)));
                        clientCore.allCardGroup.cardGroups["Hand0"].localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        clientCore.allCardGroup.cardGroups["Hand0"].CardPositionRefresh();
                        clientCore.allCardGroup.cardGroups["Hand1"].localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
                        clientCore.allCardGroup.cardGroups["Hand1"].CardPositionRefresh();
                        break;
                    }
            }
        }
    }
}
