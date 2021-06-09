using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
	// Score
	[Header("Score")]
	public Text scoreText;
	public Text scoreAddText;

	// Gravity Gage
	[Header("Gravity")]
	public Image gravGageImg;
	public GameObject gravIcon;
	public float gravGageSpd = 3.0f;

	// Game Over
	[Header("GameOver")]
	public GameObject gameOverUI;
	public GameObject infoText;
	public GameObject okBtn;
	public Text endScoreText;
	public Text endLineClearText;

	// Game Clear
	[Header("GameClaer")]
	public GameObject gameClearUI;
	public GameObject gameClearOkBtn;
	public Text gameClearEndScoreText;
	public Text gameClearEndLineClearText;

	// TimeAttack Mode
	[Header("TimeAttack Mode")]
	public Text limitTimeText;

	// Puzzle Mode
	[Header("Puzzle Mode")]
	public GameObject targetScore;
	public Text targetScoreText;

	// Control UI
	[Header("Control UI (Mobile)")]
	public Button block_MoveLeft;
	public Button block_MoveRight;
	public Button block_Rotate;
	public Button block_keep;
	public GameObject block_MoveDown;




	public void SetPuzzleUI()
    {
		targetScore.SetActive(true);
    }

	public void SetLimitTimeText(double limitTime)
	{
		limitTimeText.text = "남은시간 : " + limitTime + "초";
	}

	public void SetTargetScore(double targetScore)
    {
		targetScoreText.text = "" + targetScore;
    }


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

	public void ShowGameOverUI(bool show, double endScore)
	{
		gameOverUI.SetActive(show);
		if(!GameSetting.instance.isOnline)
		{
			infoText.SetActive(true);
		}
		StartCoroutine(SetEndScoreTextCoroutine(endScore));
	}

	public void ShowGameClearUI(bool show, double endScore)
    {
		gameClearUI.SetActive(show);
		StartCoroutine(SetClearScoreTextCoroutine(endScore));
	}

	IEnumerator SetEndScoreTextCoroutine(double endScore)
	{
		double startScore = 0;

		if (endScore >= -1000 && endScore <= 1000)
		{
			endScoreText.text = string.Format("{0:#,##0}", endScore);
		}
		else
		{
			while (startScore != endScore)
			{
				startScore += endScore / 30;
				if (startScore >= endScore) startScore = endScore;
				endScoreText.text = string.Format("{0:#,##0}", startScore);
				yield return new WaitForSeconds(0.03f);
			}
		}
	}

	IEnumerator SetClearScoreTextCoroutine(double endScore)
	{
		double startScore = 0;

		if (endScore >= -1000 && endScore <= 1000)
		{
			gameClearEndScoreText.text = string.Format("{0:#,##0}", endScore);
		}
		else
		{
			while (startScore != endScore)
			{
				startScore += endScore / 30;
				if (startScore >= endScore) startScore = endScore;
				gameClearEndScoreText.text = string.Format("{0:#,##0}", startScore);
				yield return new WaitForSeconds(0.03f);
			}
		}
	}

	public void GameOverOkBtn()
	{
		okBtn.SetActive(false);
		// TODO: 스코어 랭킹등록 로직

		SceneManager.LoadScene(0);
	}
}
