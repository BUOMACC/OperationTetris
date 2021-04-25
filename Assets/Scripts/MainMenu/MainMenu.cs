using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    void Awake()
    {
		// 게임 실행시 초기세팅
		Screen.SetResolution(720, 1280, true);
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}
