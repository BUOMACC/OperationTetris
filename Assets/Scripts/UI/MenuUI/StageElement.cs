using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageElement : MonoBehaviour
{
	public GameObject stageIcon;
	public GameObject stageText;
	public int stageLevel = 1;

	SelectMapUIManager um;

	void Awake()
	{
		um = FindObjectOfType<SelectMapUIManager>();
	}

	void OnEnable()
	{
		stageText.GetComponent<Text>().text = stageLevel.ToString();
		if (stageLevel <= GameSetting.instance.puzzle_Stage)
		{
			stageIcon.SetActive(false);
			stageText.SetActive(true);
		}
		else
		{
			stageIcon.SetActive(true);
			stageText.SetActive(false);
		}
	}

	public void BtnClick()
	{
		um.PlayBtn_Puzzle(stageLevel);
	}

}
