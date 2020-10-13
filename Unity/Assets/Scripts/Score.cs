using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Score : NetworkBehaviour
{

    [SyncVar]
    private int leftScore;
    [SyncVar]
    private int rightScore;

    public delegate void LeftChangedDelegate(int leftScore);
    [SyncEvent]
    public event LeftChangedDelegate EventLeftChanged;

    #region Server
    [Server]
    private void SetLeftScore(int score)
    {
        leftScore = score;
        EventLeftChanged?.Invoke(leftScore);
    }

    public override void OnStartServer() => SetLeftScore(0);
    [Command]
    private void LeftScores() => SetLeftScore(leftScore++);

    #endregion

    #region Client
    [ClientCallback]
    #endregion

    #region Client
    private void Update()
    {
        if(!hasAuthority) { return; }

    }
    #endregion
}
