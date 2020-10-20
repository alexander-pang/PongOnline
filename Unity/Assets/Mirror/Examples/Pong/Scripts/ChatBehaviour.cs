﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using System;

namespace Mirror.Examples.Pong
{
    public class ChatBehaviour : NetworkBehaviour
    {
        [SerializeField] private GameObject chatUI = null;
        [SerializeField] private TMP_Text TMP_Text = null;
        [SerializeField] private TMP_InputField inputField = null;
        
        private static event Action<string> OnMessage;

        public override void OnStartAuthority()
        {
            chatUI.SetActive(true);

            OnMessage += HandleNewMessage;
        }

        [ClientCallback]
        private void OnDestroy()
        {
            if (!hasAuthority){ return; }

            OnMessage -= HandleNewMessage;
        }

        private void HandleNewMessage()
        {
            chatText.text += OnMessage;
        }

        [Client]
        public void Send(string message)
        {
            if (!Input.GetKeyDown(KeyCode.Return)) {return;}
            if (string.IsNullOrWhiteSpace(message)) {return;}
            
            CmdSendMessage(message);
            
            inputField.text = string.Empty;
        }

        [Command]
        private void CmdSendMessage(string message)
        {
            RcpHandleMessage($"[{connectionToClient.connectionId}]: {message}");
        }

        [ClientRpc]
        private void RpcHandleMessage(string message)
        {
            OnMessage?.Invoke($"\n{message}");
        }
    }
}