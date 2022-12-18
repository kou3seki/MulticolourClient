using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Client;
using Unity.Mathematics;
using Random = UnityEngine.Random;

namespace Client
{

    public class AllCardGroup : MonoBehaviour
    {
        [HideInInspector] public ClientCore clientCore; 

        public GameObject deskArea;
        [HideInInspector] public Dictionary<string, CardGroup> cardGroups = new Dictionary<string, CardGroup>();

        // Start is called before the first frame update
        void Awake()
        {
            clientCore = GameObject.Find("Player").GetComponent<ClientCore>();
            GroupLoad();
        }

        void Start()
        {
            deskArea.SetActive(true);
            DeskAreaMatch();
            deskArea.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            foreach (CardGroup group in cardGroups.Values)
            {
                group.GroupRefresh();
            }
        }

        private void GroupLoad()
        {
            cardGroups.Add("Palette0", new CardGroup("Palette0", false, true, Quaternion.identity, -1));
            cardGroups.Add("Battlefield10", new CardGroup("Battlefield10", false, true, Quaternion.identity, -1));
            cardGroups.Add("Battlefield20", new CardGroup("Battlefield20", false, true, Quaternion.identity, -1));
            cardGroups.Add("Hand0", new CardGroup("Hand0", false, false, Quaternion.Euler(new Vector3(-45, 0, 0)), -1));
            cardGroups.Add("Deck0", new CardGroup("Deck0", true, false, Quaternion.Euler(new Vector3(180, 0, 0)), -1));
            cardGroups.Add("Cemetry0", new CardGroup("Cemetry0", true, true, Quaternion.identity, -1));
            cardGroups.Add("Heroine0", new CardGroup("Heroine0", false, true, Quaternion.identity, -1));
            cardGroups.Add("Out0", new CardGroup("Out0", true, true, Quaternion.identity, -1));

            cardGroups.Add("Palette1", new CardGroup("Palette1", false, true, Quaternion.Euler(new Vector3(0, 0, 180)), 1));
            cardGroups.Add("Battlefield11", new CardGroup( "Battlefield11",  false, true, Quaternion.Euler(new Vector3(0, 0, 180)), 1));
            cardGroups.Add("Battlefield21", new CardGroup("Battlefield21",  false, true, Quaternion.Euler(new Vector3(0, 0, 180)), 1));
            cardGroups.Add("Hand1", new CardGroup("Hand1", false, false, Quaternion.Euler(new Vector3(45, 0, 180)), 1));
            cardGroups.Add("Deck1", new CardGroup("Deck1", true, false, Quaternion.Euler(new Vector3(180, 0, 180)), 1));
            cardGroups.Add("Cemetry1", new CardGroup("Cemetry1", true, true, Quaternion.Euler(new Vector3(0, 0, 180)), 1));
            cardGroups.Add("Heroine1", new CardGroup("Heroine1", false, true, Quaternion.Euler(new Vector3(0, 0, 180)), 1));
            cardGroups.Add("Out1", new CardGroup("Out1", true, true, Quaternion.Euler(new Vector3(0, 0, 180)), 1));

            cardGroups.Add("Display", new CardGroup("Display", false, true, Quaternion.Euler(new Vector3(0, 0, 90)), 1));
        }

        private void DeskAreaMatch()
        {
            foreach (CardGroup cardGroup in cardGroups.Values)
            {
                cardGroup.DeskAreaRefresh(clientCore.allDeskArea.deskAreas[cardGroup.deskAreaName]);
            }
        }
    }
}
