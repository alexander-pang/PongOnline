using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class ScoreTrigger : NetworkBehaviour
{
	[SyncVar(hook = nameof(UpdateLeftScore))]
	private int leftScore;

	[SyncVar(hook = nameof(UpdateRightScore))]
	private int rightScore;

	public GameObject leftScoreText;
	public GameObject rightScoreText;

    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    void Start()
	{
		leftScore = 0;
		rightScore = 0;
	}

	public void LeftScores()
	{
		UpdateLeftScore(leftScore, ++leftScore);
	}

	public void RightScores()
	{
        UpdateRightScore(rightScore, ++rightScore);
	}

	public void UpdateLeftScore(int oldValue, int newValue)
	{
		leftScoreText.GetComponent<Text>().text = newValue.ToString();
    }

	public void UpdateRightScore(int oldValue, int newValue)
	{
		leftScoreText.GetComponent<Hud>().rightScore.text = newValue.ToString();
	}

	public int GetLeftScore()
	{
		return leftScore;
	}

	public int GetRightScore()
	{
		return rightScore;
	}
}
