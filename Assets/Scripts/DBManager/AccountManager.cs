using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AccountManager : MonoBehaviour
{
	public UnityEvent OnLoginSuccess;
	public UnityEvent OnLoginFail;
	public UnityEvent OnRegisterSuccess;
	public UnityEvent OnRegisterFail;

	[Header("Login Component")]
	public InputField login_inputID;
	public InputField login_inputPW;

	[Header("Register Component")]
	public InputField reg_inputID;
	public InputField reg_inputPW;
	public InputField reg_inputName;
	public InputField reg_inputEmail;

	private MessageBox messageBox;
	private MainUIManager um;


	private const string url = "leehy1235.cafe24.com/";

	void Awake()
	{
		messageBox = FindObjectOfType<MessageBox>();
		um = FindObjectOfType<MainUIManager>();
	}

	public void TryLogin()
	{
		if (login_inputID.text.Equals("") || login_inputPW.text.Equals(""))
		{
			messageBox.ShowMessageBox("로그인", "아이디와 패스워드를 입력하세요!");
			return;
		}

		um.ShowWaitUI();
		StartCoroutine(LoginCoroutine(login_inputID.text, login_inputPW.text));
	}

	IEnumerator LoginCoroutine(string ID, string PW)
	{
		/* 로그인 성공시
		  OnLoginSuccess.Invoke(); 호출
		실패시 OnLoginFail.Invoke(); 호출
		*/

		WWWForm form = new WWWForm();
		form.AddField("ID", ID);
		form.AddField("PW", PW);

		UnityWebRequest req = UnityWebRequest.Post(url + "Login.php", form);

		// 응답까지 대기
		yield return req.SendWebRequest();

		if (!(req.isNetworkError || req.isHttpError))
		{
			string resultData = req.downloadHandler.text;
			// ,로 나누어 값을 쪼갬 (UID, 성공리턴값 등을 구분하기 위해서)
			string[] results = resultData.Split(',');

			if (results[0].Equals("Success"))
			{
				messageBox.ShowMessageBox("로그인", "로그인 성공!");
				GameSetting.instance.isOnline = true;
				GameSetting.instance.uID = int.Parse(results[1]);
				GameSetting.instance.nickName = results[2];
				GameSetting.instance.block = int.Parse(results[3]);
				GameSetting.instance.level = int.Parse(results[4]);
				GameSetting.instance.exp = int.Parse(results[5]);
				GameSetting.instance.exp_Max = int.Parse(results[6]);
				GameSetting.instance.puzzle_Stage = int.Parse(results[7]);
				GameSetting.instance.normal_Easy = long.Parse(results[8]);
				GameSetting.instance.normal_Hard = long.Parse(results[9]);
				GameSetting.instance.timeAttack_Easy = long.Parse(results[10]);
				GameSetting.instance.timeAttack_Hard = long.Parse(results[11]);

				OnLoginSuccess.Invoke();
			}
			else
			{
				messageBox.ShowMessageBox("로그인", "아이디 또는 패스워드를 확인해주세요.");
				OnLoginFail.Invoke();
			}
		}
		else
		{
			// Error
			messageBox.ShowMessageBox("Error", "연결 오류");
		}

	}

	public void TryRegister()
	{
		if (reg_inputID.text.Equals("") || reg_inputPW.text.Equals("") || reg_inputName.Equals(""))
		{
			messageBox.ShowMessageBox("회원가입", "아이디, 패스워드, 닉네임은 필수 입력 사항입니다!");
			return;
		}

		if (reg_inputID.text.Contains(".") || reg_inputID.text.Contains(",") ||
			reg_inputPW.text.Contains(".") || reg_inputPW.text.Contains(",") ||
			reg_inputName.text.Contains(".") || reg_inputName.text.Contains(",") ||
			reg_inputEmail.text.Contains(","))
		{
			messageBox.ShowMessageBox("회원가입", "사용할 수 없는 기호가 포함되어 있습니다!");
			return;
		}

		um.ShowWaitUI();
		StartCoroutine(RegisterCoroutine(reg_inputID.text, reg_inputPW.text, reg_inputName.text, reg_inputEmail.text));
	}

	IEnumerator RegisterCoroutine(string ID, string PW, string NAME, string EMAIL)
	{
		/* 로그인 성공시
		  OnRegisterSuccess.Invoke(); 호출
		실패시 OnRegisterFail.Invoke(); 호출
		*/
		WWWForm form = new WWWForm();
		form.AddField("ID", ID);
		form.AddField("PW", PW);
		form.AddField("NAME", NAME);
		form.AddField("EMAIL", EMAIL);

		UnityWebRequest req = UnityWebRequest.Post(url + "Register.php", form);

		// 응답까지 대기
		yield return req.SendWebRequest();

		if (!(req.isNetworkError || req.isHttpError))
		{
			string resultData = req.downloadHandler.text;

			switch (resultData)
			{
				case "Success":
					messageBox.ShowMessageBox("회원가입", "회원가입이 성공적으로 처리되었습니다.");
					OnRegisterSuccess.Invoke();

					// Clear Input Field
					reg_inputID.text = "";
					reg_inputPW.text = "";
					reg_inputName.text = "";
					reg_inputEmail.text = "";

					break;

				case "AlreadyName":
					messageBox.ShowMessageBox("회원가입", "이미 존재하는 닉네임입니다!");
					OnRegisterFail.Invoke();
					break;

				case "AlreadyID":
					messageBox.ShowMessageBox("회원가입", "이미 존재하는 아이디입니다!");
					OnRegisterFail.Invoke();
					break;
			}
		}
		else
		{
			// Error
			messageBox.ShowMessageBox("Error", "연결 오류");
		}
	}


	public void GetRanking(string MODE, RankingElement[] elements)
	{
		StartCoroutine(GetRanking_Coroutine(MODE, elements));
	}

	IEnumerator GetRanking_Coroutine(string MODE, RankingElement[] elements)
	{
		WWWForm form = new WWWForm();
		form.AddField("MODE", MODE);

		UnityWebRequest req = UnityWebRequest.Post(url + "Ranking.php", form);

		// 응답까지 대기
		yield return req.SendWebRequest();

		if (!(req.isNetworkError || req.isHttpError))
		{
			string resultData = req.downloadHandler.text;
			string[] results = resultData.Split('.');

			switch (results[0])
			{
				case "Success":
					for (int i = 1; i < results.Length; i++)
					{
						elements[i - 1].SetText(results[i]);
					}
					break;

				case "NoData":
					messageBox.ShowMessageBox("랭킹", "조회된 데이터가 없습니다.");
					break;
			}
		}
		else
		{
			// Error
			messageBox.ShowMessageBox("Error", "연결 오류");
		}
	}

	public void TrySaveData()
	{
		StartCoroutine(TrySaveData_Coroutine());
	}

	IEnumerator TrySaveData_Coroutine()
	{
		WWWForm form = new WWWForm();
		form.AddField("UID", GameSetting.instance.uID);
		form.AddField("NAME", GameSetting.instance.nickName);
		form.AddField("BLOCK", GameSetting.instance.block);
		form.AddField("LEVEL", GameSetting.instance.level);
		form.AddField("EXP", GameSetting.instance.exp);
		form.AddField("EXP_MAX", GameSetting.instance.exp_Max);
		form.AddField("PUZZLE_STAGE", GameSetting.instance.puzzle_Stage);
		form.AddField("NORMAL_EASY", GameSetting.instance.normal_Easy.ToString());
		form.AddField("NORMAL_HARD", GameSetting.instance.normal_Hard.ToString());
		form.AddField("TIMEATTACK_EASY", GameSetting.instance.timeAttack_Easy.ToString());
		form.AddField("TIMEATTACK_HARD", GameSetting.instance.timeAttack_Hard.ToString());

		UnityWebRequest req = UnityWebRequest.Post(url + "SaveData.php", form);

		// 응답까지 대기
		yield return req.SendWebRequest();
	}

	public void TryChangeName(string NEWNAME)
	{
		StartCoroutine(ChangeNameCoroutine(NEWNAME));
	}

	IEnumerator ChangeNameCoroutine(string NEWNAME)
	{

		WWWForm form = new WWWForm();
		form.AddField("NAME", GameSetting.instance.nickName);
		form.AddField("NEWNAME", NEWNAME);
		form.AddField("BLOCK", GameSetting.instance.block);

		UnityWebRequest req = UnityWebRequest.Post(url + "ChangeName.php", form);

		// 응답까지 대기
		yield return req.SendWebRequest();

		if (!(req.isNetworkError || req.isHttpError))
		{
			string resultData = req.downloadHandler.text;

			if(resultData.Equals("Success"))
			{
				messageBox.ShowMessageBox("알림", "닉네임을 성공적으로 변경했습니다.");
				GameSetting.instance.nickName = NEWNAME;
				GameSetting.instance.block -= 300;
			}
			else
			{
				messageBox.ShowMessageBox("알림", "이미 존재하는 닉네임입니다!");
			}
		}
		else
		{
			// Error
			messageBox.ShowMessageBox("Error", "연결 오류");
		}
	}


}
