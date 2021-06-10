using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIManager : MonoBehaviour
{
	public GameObject waitUI;
	public GameObject loginUI;
	public GameObject registerUI;
	public GameObject playUI;
	public GameObject optionUI;
	public GameObject selectMapUI;

	public string gameSceneName = "Game";
	public float waitTime = 3.0f;

	AccountManager am;

	void Awake()
	{
		am = FindObjectOfType<AccountManager>();
	}

	void Start()
	{
		AudioManager.instance.PlayBGM("MainBGM");
		if(GameSetting.instance.session) // 세션 유지중이라면 로그인 스킵
		{
			ShowPlayUI();
			if(GameSetting.instance.isOnline) // 온라인 모드라면 데이터 저장
			{
				am.TrySaveData();
			}
		}
	}

	public void ShowWaitUI()
	{
		loginUI.SetActive(false);
		registerUI.SetActive(false);
		playUI.SetActive(false);

		waitUI.SetActive(true);
	}

	public void ShowRegisterUI()
	{
		loginUI.SetActive(false);
		waitUI.SetActive(false);

		registerUI.SetActive(true);
	}

	public void ShowLoginUI()
	{
		registerUI.SetActive(false);
		waitUI.SetActive(false);

		loginUI.SetActive(true);
	}

	public void ShowPlayUI()
	{
		waitUI.SetActive(false);
		loginUI.SetActive(false);
		playUI.SetActive(true);
		GameSetting.instance.session = true;
	}

	public void ShowOptionUI()
	{
		optionUI.SetActive(true);
	}

	public void ShowSelectMapUI()
	{
		selectMapUI.SetActive(true);
	}
}
