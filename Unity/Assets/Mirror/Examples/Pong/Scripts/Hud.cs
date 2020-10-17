using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

namespace Mirror.Examples.Pong
{
    public class Hud : NetworkBehaviour
    {
        public Text leftScore;
        public Text rightScore;
        public Text leftWin;
        public Text rightWin;
        public static Hud instance { get; set; }

        //public Hud(Text ls, Text rs, Text lw, Text rw)
        //{
        //    leftScore = ls;
        //    rightScore = rs;
        //    leftWin = lw;
        //    rightWin = rw;
        //}
        public void Awake()
        {
            instance = this;
        }
    }
}