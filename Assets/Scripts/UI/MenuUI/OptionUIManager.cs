using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUIManager : MonoBehaviour
{
	public static OptionUIManager instance;

	private GameSetting gs;

	public Slider frameLimitSlider;
	public Slider bgmSlider;
	public Slider sfxSlider;
	public Toggle bloomToggle;
	public Toggle cameraShakeToggle;

	public Text fpsText;
	public Text bgmText;
	public Text sfxText;

	void Update()
	{
		ChangeValue_LimitFPS();
		ChangeValue_BGM();
		ChangeValue_SFX();
		ChangeValue_Bloom();
		ChangeValue_CameraShake();
	}

	public void ChangeValue_LimitFPS()
	{
		GameSetting.instance.setFrameLimit((int)frameLimitSlider.value);
		PlayerPrefs.SetInt("frameLimit", (int)frameLimitSlider.value);
		fpsText.text = "" + frameLimitSlider.value;
	}

	public void ChangeValue_BGM()
	{
		GameSetting.instance.setBGM((int)bgmSlider.value);
		PlayerPrefs.SetInt("bgm", (int)bgmSlider.value);
		bgmText.text = "" + bgmSlider.value;
	}

	public void ChangeValue_SFX()
	{
		GameSetting.instance.setSFX((int)sfxSlider.value);
		PlayerPrefs.SetInt("sfx", (int)sfxSlider.value);
		sfxText.text = "" + sfxSlider.value;
	}

	public void ChangeValue_Bloom()
	{
		if (bloomToggle.isOn == true)
		{
			Camera.main.GetComponent<FastMobileBloom>().enabled = true;
		}
        else
        {
			Camera.main.GetComponent<FastMobileBloom>().enabled = false;
		}
	}

	public void ChangeValue_CameraShake()
	{
		// 미완, if (cameraShakeToggle.isOn == true) ~~~~
	}

	public void CloseBtn()
	{
		this.gameObject.SetActive(false);
	}
}
