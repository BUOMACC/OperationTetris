using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetting : MonoBehaviour
{
	public static GameSetting instance;

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
