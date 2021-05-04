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
