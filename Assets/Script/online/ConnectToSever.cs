using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;

namespace Client
{
    public class ConnectToSever : MonoBehaviour
    {
        [HideInInspector] public ClientCore clientCore;

        private Socket socket;
        private byte[] buffer = new byte[1024];
        private string remainData;

        // Start is called before the first frame update
        void Awake()
        {
            clientCore = GameObject.Find("Player").GetComponent<ClientCore>();
            remainData = "";
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SocketInit()
        {
            socket.Close();
            clientCore.playerIndex = 7;
            clientCore.roomStatus.needRefresh = true;
        }

        public void Connect(string IP, string nickName)
        {
            try
            {
                if (IP.Equals(""))
                {
                    IP = "127.0.0.1";
                }

                clientCore.playerIndex = 8;
                IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(IP), 6789);
                socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipe);
                StartReceive();
                Send(ToSever.JoinRoom(nickName)) ;
                Send(ToSever.Snapshoot()) ;
                clientCore.roomStatus.connect.interactable = false;
                clientCore.roomStatus.getSnapshoot.interactable = true;
            }
            catch
            {
                clientCore.chat.AddLog("无法连接至服务器");
                clientCore.playerIndex = 7;
            }
        }

        private void StartReceive()
        {
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback,  null);
        }

        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            if (clientCore.playerIndex != 7)
            {
                int len = socket.EndReceive(asyncResult);
                if (len > 0)
                {
                    string msg = remainData + Encoding.UTF8.GetString(buffer, 0, len);
                    string[] temp1 = msg.Split('#');
                    int l = temp1.Length;

                    for (int i = 0; i < l-1; i++)
                    {
                        string[] temp2 = temp1[i].Split('$'); 
                        try
                        {
                            clientCore.protocalHandler.handlers[temp2[0]](temp2[1]);
                        }
                        catch (Exception e)
                        {
                            Debug.Log("***" + e.ToString());
                        }
                        Debug.Log(temp1[i]);
                    }

                    remainData = temp1[l - 1];
                }
                StartReceive();
            }
        }

        public void Send(string input)
        {
            try
            {
                socket.Send(Encoding.UTF8.GetBytes(input + "#"));
            }
            catch
            {
                SocketInit();
                clientCore.chat.AddLog("与服务器断开连接，请重新连接");
            }
        }
    }
}
