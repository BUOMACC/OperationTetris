using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectMapUIManager : MonoBehaviour
{
	public GameObject selectMapBack;
	public GameObject tab_Normal;
	public GameObject tab_TimeAttack;
	public GameObject tab_Puzzle;

	public GameObject tab_Ranking;
	public RankingElement rankingElement;
	public Text text_rankEasy;
	public Text text_rankHard;
	public RankingElement[] element_Easy;
	public RankingElement[] element_Hard;

	MainUIManager um;
	AccountManager am;
	MessageBox messageBox;

	void Awake()
	{
		um = FindObjectOfType<MainUIManager>();
		am = FindObjectOfType<AccountManager>();
		messageBox = FindObjectOfType<MessageBox>();
	}

	public void Select_Normal()
	{
		tab_TimeAttack.SetActive(false);
		tab_Puzzle.SetActive(false);
		tab_Ranking.SetActive(false);

		tab_Normal.SetActive(true);
	}

	public void Select_TimeAttack()
	{
		tab_Normal.SetActive(false);
		tab_Puzzle.SetActive(false);
		tab_Ranking.SetActive(false);

		tab_TimeAttack.SetActive(true);
	}

	public void Select_Puzzle()
	{
		tab_TimeAttack.SetActive(false);
		tab_Normal.SetActive(false);
		tab_Ranking.SetActive(false);

		tab_Puzzle.SetActive(true);
	}

	public void Select_Ranking(string mode)
	{
		// 인터넷에 연결된 경우만 랭킹 조회가능
			tab_TimeAttack.SetActive(false);
			tab_Normal.SetActive(false);
			tab_Puzzle.SetActive(false);

			tab_Ranking.SetActive(true);

			if (mode.Contains("NORMAL"))
			{
				text_rankEasy.text = "기본 - Easy (Top 10)";
				text_rankHard.text = "기본 - Hard (Top 10)";

				for (int i = 0; i < 10; i++)
				{
					element_Easy[i].SetText("");
					element_Hard[i].SetText("");
				}

				am.GetRanking("NORMAL_EASY", element_Easy);
				am.GetRanking("NORMAL_HARD", element_Hard);
			}
			else if (mode.Contains("TIMEATTACK"))
			{
				text_rankEasy.text = "타임어택 - Easy (Top 10)";
				text_rankHard.text = "타임어택 - Hard (Top 10)";

				for (int i = 0; i < 10; i++)
				{
					element_Easy[i].SetText("");
					element_Hard[i].SetText("");
				}

				am.GetRanking("TIMEATTACK_EASY", element_Easy);
				am.GetRanking("TIMEATTACK_HARD", element_Hard);
			}
		}
	}

	public void CloseBtn()
	{
		selectMapBack.SetActive(false);
	}

	public void PlayBtn_NormalEasy()
	{
		GameSetting.instance.mode = GameSetting.Mode.Normal;
		GameSetting.instance.difficulty = GameSetting.Difficulty.Easy;

		GamePlay();
	}

	public void PlayBtn_NormalHard()
	{
		GameSetting.instance.mode = GameSetting.Mode.Normal;
		GameSetting.instance.difficulty = GameSetting.Difficulty.Hard;

		GamePlay();
	}

	public void PlayBtn_TimeAttackEasy()
	{
		GameSetting.instance.mode = GameSetting.Mode.TimeAttack;
		GameSetting.instance.difficulty = GameSetting.Difficulty.Easy;

		GamePlay();
	}

	public void PlayBtn_TimeAttackHard()
	{
		GameSetting.instance.mode = GameSetting.Mode.TimeAttack;
		GameSetting.instance.difficulty = GameSetting.Difficulty.Hard;

		GamePlay();
	}

	public void PlayBtn_Puzzle(int level)
	{
		GameSetting.instance.mode = GameSetting.Mode.Puzzle;
		GameSetting.instance.difficulty = GameSetting.Difficulty.Easy;
		GameSetting.instance.puzzleLevel = level;

		GamePlay();
	}

	public void GamePlay()
	{
		CloseBtn();
		um.ShowWaitUI();
		StartCoroutine(LoadAsyncGameSceneCoroutine()); // 비동기 게임씬 로딩
	}

	IEnumerator LoadAsyncGameSceneCoroutine()
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(um.gameSceneName);
		operation.allowSceneActivation = false;

		yield return new WaitForSeconds(um.waitTime);

		while (!operation.isDone)
		{
			yield return null;
			if (operation.progress >= 0.9f)
				operation.allowSceneActivation = true;
		}
	}
}
