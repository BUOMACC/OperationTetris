using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIManager : MonoBehaviour
{
	public GameObject waitUI;
	public GameObject loginUI;
	public GameObject playUI;
	public GameObject optionUI;

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

	public void ShowOptionUI()
	{
		optionUI.SetActive(true);
	}
}
