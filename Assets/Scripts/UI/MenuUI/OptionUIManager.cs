using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionUIManager : MonoBehaviour
{

	public void ChangeValue_LimitFPS()
	{
		// 슬라이더 값을 가져와 GameSetting에 저장, GameSetting에 변수선언 필요
	}

	public void ChangeValue_BGM()
	{
		// 슬라이더 값을 가져와 저장
	}

	public void ChangeValue_SFX()
	{
		// 슬라이더 값을 가져와 저장
	}

	public void ChangeValue_Bloom()
	{
		/*
		 Element_Bloom 밑 Toggle 값에 따라 (체크박스 형식, if or switch로 체크)
		 Camera.main.GetComponent<FastMobileBloom>().enabled = 값;
		 */
	}

	public void ChangeValue_CameraShake()
	{
		// 미완
	}

	public void CloseBtn()
	{
		this.gameObject.SetActive(false);
	}
}
