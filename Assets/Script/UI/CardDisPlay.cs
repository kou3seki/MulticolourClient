using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Client;

namespace Client
{
    public class CardDisPlay : MonoBehaviour
    {
        [HideInInspector] public ClientCore clientCore;

        public Image displayImage;
        public Text displayText;
        public Text displayMarker;
        public Text[] displayColor;

        // Start is called before the first frame update
        void Awake()
        {
            clientCore = GameObject.Find("Player").GetComponent<ClientCore>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetDisplay(CardData cardData)
        {
            if (cardData.Image != null)
            {
                displayImage.sprite = cardData.Image;
                displayText.text = cardData.Describe;
            }

            if (cardData.BaseCost != null)
            {
                for (int i = 0; i < 5; i++)
                {
                    displayColor[i].text = cardData.BaseCost[i] switch
                    {
                        999 => "X",
                        1998 => "2X",
                        555 => "3/3",
                        222 => "-0-",
                        _ => cardData.BaseCost[i].ToString(),
                    };
                }
            }
        }

        public void SetDisplay(Card card)
        {
            SetDisplay(card.cardData);
            displayMarker.text = card.marker.text;
        }

        public void ClearDisplay()
        {
            displayImage.sprite = AllCardData.transparent;
            displayText.text = "";
            displayMarker.text = "";

            for (int i = 0; i < 5; i++)
            {
                displayColor[i].text = "";
            }
        }
    }
}

