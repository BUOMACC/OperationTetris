using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetting : MonoBehaviour
{
	public static GameSetting instance;

	private OptionUIManager om;
	private AudioManager am;

	[Header("Game Data")]

	private int frameLimit;
	private int bgm;
	private int sfx;
	private bool bloom = true;
	private bool camShake = true;

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
	public int puzzleLevel = 1;
	public float puzzleFallTime = 999999.0f;

	// Login Data
	[Header("User Data")]
	public bool session = false; // 세션 유지
	public bool isOnline = false;
	public int uID = 0;
	public string nickName = "";
	public int block = 0;
	public int level = 1;
	public int exp = 0;
	public int exp_Max = 100;
	public int puzzle_Stage = 1;
	public long normal_Easy = 0;
	public long normal_Hard = 0;
	public long timeAttack_Easy = 0;
	public long timeAttack_Hard = 0;


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

	void Start()
	{
		frameLimit = PlayerPrefs.GetInt("frameLimit", 60);
		bgm = PlayerPrefs.GetInt("bgm", 100);
		sfx = PlayerPrefs.GetInt("sfx", 100);
		bloom = (PlayerPrefs.GetInt("bloom", 1) == 1) ? true : false;
		camShake = (PlayerPrefs.GetInt("camShake", 1) == 1) ? true : false;
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

	public void AddExp(int amount)
	{
		exp += amount;
		if(exp >= exp_Max)
		{
			exp = exp - exp_Max;
			level += 1;
		}
	}
}
