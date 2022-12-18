using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Client;
using System;

namespace Client
{

    public class DeskArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [HideInInspector] public ClientCore clientCore;

        public string areaName;
        public string cardGroupName;

        public CardGroup cardGroup;

        // Start is called before the first frame update
        void Awake()
        {
            clientCore = GameObject.Find("Player").GetComponent<ClientCore>();
            clientCore.allDeskArea.deskAreas.Add(areaName, this);
        }

        void Start()
        {
            cardGroup = clientCore.allCardGroup.cardGroups[cardGroupName];
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            clientCore.cardControl.areaTarget = this;
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            clientCore.cardControl.areaTarget = null;
        }
    }
}
