using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AccountManager : MonoBehaviour
{
	public UnityEvent OnLoginSuccess;
	public UnityEvent OnLoginFail;
	public UnityEvent OnRegisterSuccess;
	public UnityEvent OnRegisterFail;

	public void TryLogin(string id, string pw)
	{
		StartCoroutine(LoginCoroutine(id, pw));
	}

	IEnumerator LoginCoroutine(string id, string pw)
	{
		/* 로그인 성공시
		  OnLoginSuccess.Invoke(); 호출
		실패시 OnLoginFail.Invoke(); 호출
		*/
		yield return null;
	}

	public void TryRegister(string id, string pw)
	{
		StartCoroutine(RegisterCoroutine(id, pw));
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
