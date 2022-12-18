using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Client;

namespace Client
{
    public class Chat : MonoBehaviour
    {
        [HideInInspector] public ClientCore clientCore;

        public Text actionLog;
        private Queue<string> actions = new Queue<string>();

        private bool needRefresh;

        // Start is called before the first frame update
        void Awake()
        {
            clientCore = GameObject.Find("Player").GetComponent<ClientCore>();
        }

        // Update is called once per frame
        void Update()
        { 
            if (needRefresh)
            {
                LogRefresh();
            }
        }


        public void AddLog(string input)
        {
            actions.Enqueue(input);
            if (actions.Count >  13)
            {
                actions.Dequeue();
            }
            needRefresh = true;
        }

        public void LogRefresh()
        {
            string output = "";
            foreach (string log in actions)
            {
                output += (log + "\n");
            }
            actionLog.text = output;
            needRefresh = false;
        }
    }
}
