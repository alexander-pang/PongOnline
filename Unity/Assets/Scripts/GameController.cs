using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameController : NetworkBehaviour
{
    public static GameController instance { get; set; }

    [SerializeField] private int leftScore;
    [SerializeField] private int rightScore;

    public void Awake()
    {
        instance = this;
    }

    public void Score(NetworkManagerPong.Side side)
    {
        if (side == NetworkManagerPong.Side.Left)
        {
            leftScore++;
        }
        else if (side == NetworkManagerPong.Side.Right)
        {
            rightScore++;
        }
    }
}
