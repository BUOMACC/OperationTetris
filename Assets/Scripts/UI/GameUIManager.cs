using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
	public Text scoreText;
	public Text scoreAddText;


	public void SetScoreText(string amount, string currentScore)
	{
		// 점수 증가량, 현재 점수
		scoreAddText.text = amount;
		scoreText.text = currentScore;
	}
}
