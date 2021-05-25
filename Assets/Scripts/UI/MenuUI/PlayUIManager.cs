using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayUIManager : MonoBehaviour
{
	public string gameSceneName;
	public float waitTime = 3.0f;

	MainUIManager um;

	void Awake()
	{
		um = FindObjectOfType<MainUIManager>();
	}

	public void PlayBtn()
	{
		um.ShowWaitUI();
		StartCoroutine(LoadAsyncGameSceneCoroutine()); // 비동기 게임씬 로딩
	}

	IEnumerator LoadAsyncGameSceneCoroutine()
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(gameSceneName);
		operation.allowSceneActivation = false;

		yield return new WaitForSeconds(waitTime);

		while(!operation.isDone)
		{
			yield return null;
			if(operation.progress >= 0.9f)
				operation.allowSceneActivation = true;
		}
	}

	public void OptionBtn()
    {
		um.ShowOptionUI();
    }

	public void QuitBtn()
	{
		Application.Quit();
	}
}
