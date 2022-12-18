using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Client;
using UnityEngine.UIElements;

namespace Client
{
    public class MarkerInput : MonoBehaviour
    {
        [HideInInspector] public ClientCore clientCore;

        public InputField markerInput;
        private Card card;
        public bool selected;

        // Start is called before the first frame update
        void Awake()
        {
            clientCore = GameObject.Find("Player").GetComponent<ClientCore>();
            markerInput.onValueChanged.AddListener(OnValueChanged);
            markerInput.onEndEdit.AddListener(OnEndEdit);
        }

        // Update is called once per frame
        void Update()
        {
            if (selected)
            {
                Confirm();
            }
        }

        public void CreateInput(Card card)
        {
            this.card = card;
            selected = true;
            markerInput.text = card.marker.text;
            markerInput.ActivateInputField();
        }

        private void OnValueChanged(string s)
        {
            card.marker.text = s;
        }

        private void OnEndEdit(string s)
        {
            selected = false;
            clientCore.connectToSever.Send(ToSever.SetMarker(card.uniqueID, s));
            markerInput.text = "";
        }

        private void Confirm()
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                selected = false;
                markerInput.DeactivateInputField();
            }
        }
    }
}

