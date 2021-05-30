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

	public InputField input_ID;
	public InputField input_PW;

	private const string url = "leehy1235.cafe24.com/1.php";

	public void TryLogin()
	{
		StartCoroutine(LoginCoroutine(input_ID.text, input_PW.text));
	}

	IEnumerator LoginCoroutine(string id, string pw)
	{
		/* 로그인 성공시
		  OnLoginSuccess.Invoke(); 호출
		실패시 OnLoginFail.Invoke(); 호출
		*/
		WWWForm form = new WWWForm();
		form.AddField("id", id);
		form.AddField("pw", pw);

		UnityWebRequest req = UnityWebRequest.Post(url, form);

		// 응답까지 대기
		yield return req.SendWebRequest();

		if(!(req.isNetworkError || req.isHttpError))
		{
			string resultData = req.downloadHandler.text;
			string[] results = resultData.Split(',');

			Debug.Log(resultData);
			if (results[0].Equals("Success"))
			{
				Debug.Log("로그인 성공");
				GameSetting.instance.isOnline = true;
				GameSetting.instance.aID = id;
				GameSetting.instance.uID = int.Parse(results[1]);

				OnLoginSuccess.Invoke();
			}
			else
			{
				Debug.Log("로그인 실패");
				OnLoginFail.Invoke();
			}
		}
		else
		{
			// Error
			Debug.Log("연결 에러");
		}

	}

	public void TryRegister()
	{
		StartCoroutine(RegisterCoroutine(input_ID.text, input_PW.text));
	}

	IEnumerator RegisterCoroutine(string id, string pw)
	{
		/* 로그인 성공시
		  OnRegisterSuccess.Invoke(); 호출
		실패시 OnRegisterFail.Invoke(); 호출
		*/
		yield return null;
	}
}
