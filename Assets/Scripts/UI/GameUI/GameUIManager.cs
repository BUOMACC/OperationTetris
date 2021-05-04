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
	public Image gravGageImg;
	public GameObject gravIcon;
	public float gravGageSpd = 3.0f;


	public void SetScoreText(string amount, string currentScore)
	{
		// 점수 증가량, 현재 점수
		scoreAddText.text = amount;
		scoreText.text = currentScore;
	}

	public void SetGravityGage(float gage)
	{
		StartCoroutine(SetGravityGageCoroutine(gage));
	}

	IEnumerator SetGravityGageCoroutine(float gage)
	{
		while(Mathf.Abs(gage - gravGageImg.fillAmount) > 0.01f)
		{
			gravGageImg.fillAmount = Mathf.Lerp(gravGageImg.fillAmount, gage, gravGageSpd * Time.deltaTime);
			yield return null;
		}
		gravGageImg.fillAmount = gage;
	}
}
