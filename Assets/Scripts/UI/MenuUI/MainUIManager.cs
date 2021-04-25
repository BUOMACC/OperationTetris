using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIManager : MonoBehaviour
{
	public GameObject waitUI;
	public GameObject loginUI;
	public GameObject playUI;

	void Start()
	{

	}

	public void ShowWaitUI()
	{
		loginUI.SetActive(false);
		playUI.SetActive(false);

		waitUI.SetActive(true);
	}

	public void ShowPlayUI()
	{
		waitUI.SetActive(false);

		playUI.SetActive(true);
	}
}
