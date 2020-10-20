using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System;

namespace Mirror.Examples.Pong
{
    public class ChatterBehaviour : NetworkBehaviour
    {
        [SerializeField] private Text chatText = null;
        [SerializeField] private GameObject canvas = null;
        [SerializeField] private InputField inputField = null;
        
        private static event Action<string> OnMessage;

        public override void OnStartAuthority()
        {
            canvas.SetActive(true);

            OnMessage += HandleNewMessage;
        }

        [ClientCallback]
        private void OnDestroy()
        {
            if (!hasAuthority){ return; }

            OnMessage -= HandleNewMessage;
        }

        private void HandleNewMessage(string message)
        {
            chatText.text += message;
        }

        [Client]
        public void Send()
        {
            if (!Input.GetKeyDown(KeyCode.Return)) {return;}
            if (string.IsNullOrWhiteSpace(inputField.text)) {return;}
            
            CmdSendMessage(inputField.text);
            
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