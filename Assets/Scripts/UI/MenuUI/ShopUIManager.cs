using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIManager : MonoBehaviour
{
	public GameObject shopBack;
	public Text block;

	MessageBox messageBox;

	void Awake()
	{
		messageBox = FindObjectOfType<MessageBox>();
	}

	void Update()
	{
		block.text = string.Format("{0:#,##0}", GameSetting.instance.block);
	}

	public void CloseBtn()
	{
		shopBack.SetActive(false);
	}

	public void BuyItemBtn()
	{
		messageBox.ShowMessageBox("Error", "지금은 구매할 수 없습니다.");
	}
}
