using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public Text scoreText;

	public void SetScoreText(string s)
	{
		scoreText.text = s;
	}
}
