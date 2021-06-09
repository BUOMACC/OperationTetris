using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetting : MonoBehaviour
{
	public static GameSetting instance;

	private OptionUIManager om;
	private AudioManager am;

	private int frameLimit;
	private int bgm;
	private int sfx;
	private bool bloom = true;
	private bool camShake = true;

	void Start()
    {
		frameLimit = PlayerPrefs.GetInt("frameLimit", 60);
		bgm = PlayerPrefs.GetInt("bgm", 100);
		sfx = PlayerPrefs.GetInt("sfx", 100);
		bloom = (PlayerPrefs.GetInt("bloom", 1) == 1) ? true : false;
		camShake =( PlayerPrefs.GetInt("camShake", 1) == 1) ? true : false;
		LoadOptionValues();
	}

	public void LoadOptionValues()
    {
		Application.targetFrameRate = frameLimit;
		AudioManager.instance.setBGMVolume(bgm);
		AudioManager.instance.setSFXVolume(sfx);
		Camera.main.GetComponent<FastMobileBloom>().enabled = bloom;
		// TODO: camera shake setting load
	}

	public void setFrameLimit(int frameLimit)
	{
		this.frameLimit = frameLimit;
	}

	public void setBGM(int bgm)
    {
		this.bgm = bgm;
    }

	public void setSFX(int sfx)
    {
		this.sfx = sfx;
    }


	// Difficulty
	public enum Difficulty
	{
		Easy,
		Hard
	}
	public Difficulty difficulty = Difficulty.Easy;

	// GameMode
	public enum Mode
	{
		Normal, // 기본
		TimeAttack, // 시간제한
		Puzzle // 퍼즐
	}
	public Mode mode = Mode.Normal;

	// Login Data
	public bool isOnline = false;
	// 로그인시 uID와 aID가 매치되어야함 (uID = 고유 아이디, 번호
	//									aID = 계정 아이디)
	public int uID = 0;
	public string aID = "";
	public string uName = "";


	void Awake()
	{
		#region Singleton
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			Destroy(this);
		}

		#endregion // 세팅은 하나만 존재하도록 싱글톤 사용
	}
}
