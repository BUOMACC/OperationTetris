using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
	// Score
	public Text scoreText;
	public Text scoreAddText;

	// Gravity Gage
	public Image gravImg;


	public void SetScoreText(string amount, string currentScore)
	{
		// 점수 증가량, 현재 점수
		scoreAddText.text = amount;
		scoreText.text = currentScore;
	}

	public void AddGravityGage(int amount)
	{

	}

	IEnumerator AddGravityGageCoroutine()
	{
		yield return null;
	}
}
