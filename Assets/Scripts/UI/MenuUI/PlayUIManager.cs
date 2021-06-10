using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayUIManager : MonoBehaviour
{
	public Text text_Name;
	public Text text_Exp;
	public Image img_ExpBar;

	MainUIManager um;
	OptionUIManager om;

	void Awake()
	{
		um = FindObjectOfType<MainUIManager>();
		om = FindObjectOfType<OptionUIManager>();
	}

	void Update()
	{
		text_Name.text = GameSetting.instance.nickName;
		text_Exp.text = GameSetting.instance.exp + " / " + GameSetting.instance.exp_Max + " (Lv." + GameSetting.instance.level + ")";
		img_ExpBar.fillAmount = (float)GameSetting.instance.exp / GameSetting.instance.exp_Max;
	}

	public void PlayBtn()
	{
		um.selectMapUI.SetActive(true);
	}

	public void OptionBtn()
    {
		um.ShowOptionUI();
		om.LoadOptionValues();
	}

	public void QuitBtn()
	{
		Application.Quit();
	}
}
