using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIManager : MonoBehaviour
{
	public GameObject shopBack;
	public Text block;

	[Header("ChangeName Dialog")]
	public GameObject cn_Back;
	public InputField cn_Rename;

	MainUIManager um;
	AccountManager am;
	MessageBox messageBox;

	void Awake()
	{
		um = FindObjectOfType<MainUIManager>();
		am = FindObjectOfType<AccountManager>();
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

	public void CloseBtn_ChangeNameDialog()
	{
		cn_Back.SetActive(false);
	}

	public void BuyItemBtn()
	{
		messageBox.ShowMessageBox("Error", "지금은 구매할 수 없습니다.");
	}


	// 다이얼로그를 띄움 -> 닉네임 특수기호 필터 -> 구매 완료
	public void BuyItemBtn_ChangeName()
	{
		// Offline Mode Filter
		if(!GameSetting.instance.isOnline)
		{
			messageBox.ShowMessageBox("Error", "오프라인 모드에서는 구매할 수 없습니다.");
			return;
		}

		// 구매 다이얼로그 띄움
		cn_Back.SetActive(true);
	}

	public void Btn_ChangeNameOK()
	{
		cn_Back.SetActive(false);

		// 특수기호 필터
		if (cn_Rename.text.Contains(".") || cn_Rename.text.Contains(",") || cn_Rename.text == "")
		{
			messageBox.ShowMessageBox("알림", "사용할 수 없는 기호가 포함되어 있습니다!");
		}

		// 구매 로직
		if (GameSetting.instance.block >= 300)
		{
			GameSetting.instance.block -= 300;
			am.TryChangeName(cn_Rename.text);
		}
		else
		{
			messageBox.ShowMessageBox("상점", "구매하기 위한 블록이 부족합니다.");
		}
	}
}
