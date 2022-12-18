using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Client;

namespace Client
{
    public class ClientCore : MonoBehaviour
    {
        public static Vector3 cantSeeLoc = new Vector3(5000, 0, 0);
        [HideInInspector] public DeckEditorCore deckEditorCore;

        [HideInInspector] public AllDeskArea allDeskArea;
        [HideInInspector] public AllCardGroup allCardGroup;
        [HideInInspector] public CardControl cardControl;

        [HideInInspector] public CardDisPlay cardDisPlay;
        [HideInInspector] public Chat chat;
        [HideInInspector] public TargetSelect targetSelect;
        [HideInInspector] public RoomStatus roomStatus;
        [HideInInspector] public DeskOperation deskOperation;
        [HideInInspector] public MarkerInput markerInput;

        [HideInInspector] public CardPool cardPool;
        [HideInInspector] public ConnectToSever connectToSever;
        [HideInInspector] public ProtocalHandler protocalHandler;

        public Camera mainCamera;
        public int playerIndex;

        public GameObject ui;
        public GameObject desk;
        public GameObject online;

        // Start is called before the first frame update
        void Awake()
        {
            Screen.SetResolution(1920, 1080, true);
            deckEditorCore = GetComponent<DeckEditorCore>();

            allDeskArea = desk.GetComponent<AllDeskArea>();
            allCardGroup = desk.GetComponent<AllCardGroup>();
            cardControl = desk.GetComponent<CardControl>();

            cardDisPlay = ui.GetComponent<CardDisPlay>();
            chat = ui.GetComponent<Chat>();
            targetSelect = ui.GetComponent<TargetSelect>();
            roomStatus = ui.GetComponent<RoomStatus>();
            deskOperation = ui.GetComponent<DeskOperation>();
            markerInput = ui.GetComponent<MarkerInput>();

            cardPool = online.GetComponent<CardPool>();
            connectToSever = online.GetComponent<ConnectToSever>();
            protocalHandler = new ProtocalHandler(this);
            playerIndex = 7;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}


