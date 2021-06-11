using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionUIManager : MonoBehaviour
{
	public static OptionUIManager instance;

	private GameSetting gs;
	public GameObject optionBack;

	public Slider frameLimitSlider;
	public Slider bgmSlider;
	public Slider sfxSlider;
	public Toggle bloomToggle;
	public Toggle cameraShakeToggle;

	public Text fpsText;
	public Text bgmText;
	public Text sfxText;

	public void LoadOptionValues()
    {
		frameLimitSlider.value = PlayerPrefs.GetInt("frameLimit", 60);
		fpsText.text = "" + PlayerPrefs.GetInt("frameLimit", 60);

		bgmSlider.value = PlayerPrefs.GetInt("bgm", 100);
		bgmText.text = "" + PlayerPrefs.GetInt("bgm", 100);

		sfxSlider.value = PlayerPrefs.GetInt("sfx", 100);
		sfxText.text = "" + PlayerPrefs.GetInt("sfx", 100);

		bloomToggle.isOn = (PlayerPrefs.GetInt("bloom", 1) == 1) ? true : false;
		cameraShakeToggle.isOn = (PlayerPrefs.GetInt("camShake", 1) == 1) ? true : false;
	}

	public void ChangeValue_LimitFPS()
	{
		GameSetting.instance.setFrameLimit((int)frameLimitSlider.value);
		PlayerPrefs.SetInt("frameLimit", (int)frameLimitSlider.value);
		Application.targetFrameRate = (int)frameLimitSlider.value;
		fpsText.text = "" + frameLimitSlider.value;
	}

	public void ChangeValue_BGM()
	{
		GameSetting.instance.setBGM((int)bgmSlider.value);
		PlayerPrefs.SetInt("bgm", (int)bgmSlider.value);
		AudioManager.instance.setBGMVolume((int)bgmSlider.value);
		bgmText.text = "" + bgmSlider.value;
	}

	public void ChangeValue_SFX()
	{
		GameSetting.instance.setSFX((int)sfxSlider.value);
		PlayerPrefs.SetInt("sfx", (int)sfxSlider.value);
		AudioManager.instance.setSFXVolume((int)sfxSlider.value);
		sfxText.text = "" + sfxSlider.value;
	}

	public void ChangeValue_Bloom()
	{
		if (bloomToggle.isOn == true)
		{
			Camera.main.GetComponent<FastMobileBloom>().enabled = true;
			PlayerPrefs.SetInt("bloom", (bloomToggle.isOn) ? 1 : 0);
		}
        else
        {
			Camera.main.GetComponent<FastMobileBloom>().enabled = false;
			PlayerPrefs.SetInt("bloom", (bloomToggle.isOn) ? 1 : 0);
		}
	}

	public void ChangeValue_CameraShake()
	{
		if (cameraShakeToggle.isOn == true)
        {
			GameSetting.instance.camShake = true;
			PlayerPrefs.SetInt("camShake", (cameraShakeToggle.isOn) ? 1 : 0);
		}
		else
        {
			GameSetting.instance.camShake = false;
			PlayerPrefs.SetInt("camShake", (cameraShakeToggle.isOn) ? 1 : 0);
		}
	}

	public void CloseBtn()
	{
		optionBack.SetActive(false);
	}
}
