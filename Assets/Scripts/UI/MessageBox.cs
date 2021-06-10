using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
	public GameObject panel;
	public GameObject back;

	public Text title;
	public Text desc;

	public void ShowMessageBox(string title, string desc)
	{
		this.title.text = title;
		this.desc.text = desc;
		panel.SetActive(true);
		back.SetActive(true);
	}

	public void CloseMessageBox()
	{
		panel.SetActive(false);
		back.SetActive(false);
	}
}
