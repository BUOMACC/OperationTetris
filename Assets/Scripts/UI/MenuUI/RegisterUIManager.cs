using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterUIManager : MonoBehaviour
{
	MainUIManager um;

	void Awake()
	{
		um = FindObjectOfType<MainUIManager>();
	}

	public void CloseBtn()
	{
		um.ShowLoginUI();
	}
}
