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
	public float gravGageSpd = 3.0f;


	public void SetScoreText(string amount, string currentScore)
	{
		// 점수 증가량, 현재 점수
		scoreAddText.text = amount;
		scoreText.text = currentScore;
	}

	public void AddGravityGage(float amount)
	{
		StartCoroutine(AddGravityGageCoroutine(gravImg.fillAmount+amount));
	}

	IEnumerator AddGravityGageCoroutine(float end)
	{
		while(end-gravImg.fillAmount > 0.01f)
		{
			gravImg.fillAmount = Mathf.Lerp(gravImg.fillAmount, end, gravGageSpd * Time.deltaTime);
			yield return null;
		}
		gravImg.fillAmount = end;
	}
}
