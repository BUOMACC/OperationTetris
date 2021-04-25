using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginUIManager : MonoBehaviour
{
	public float waitTime = 3.0f;
	MainUIManager um;

	void Awake()
	{
		um = FindObjectOfType<MainUIManager>();
	}

	public void PlayOffline()
	{
		StartCoroutine(PlayOfflineCoroutine());
	}

	IEnumerator PlayOfflineCoroutine()
	{
		um.ShowWaitUI();
		yield return new WaitForSeconds(waitTime);
		um.ShowPlayUI();
	}
}
