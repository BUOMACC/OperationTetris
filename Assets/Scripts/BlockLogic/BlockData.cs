using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockData : MonoBehaviour
{
	public GameObject go_blockValue; // 각 블록에 표시된 문자 오브젝트, 외부 호출시 사용
	public string blockValue; // 무엇이 적혔는지 <EX: 0이 적혔는지 5가 적혔는지..>
	public int chanceNum = 100;

	public SpriteRenderer blockRenderer; // 블록 이미지의 스프라이트 렌더러
	public SpriteRenderer markRenderer; // 블록에 표시된 마킹(숫자, 기호..)의 스프라이트 렌더러


	void Awake()
	{
		blockRenderer = GetComponent<SpriteRenderer>();
		markRenderer = transform.Find("Mark").GetComponent<SpriteRenderer>();
	}

	public void FadeOutAnimation(float destroyTime)
	{
		StartCoroutine(FadeOutAnimationCoroutine(destroyTime));
	}

	IEnumerator FadeOutAnimationCoroutine(float destroyTime)
	{
		Color blockColor = blockRenderer.color;
		Color markColor = markRenderer.color;

		// alpha 값이 0보다 큰 동안 반복(보이는 동안 반복)
		while (blockColor.a > 0)
		{
			// Time.deltaTime 과 파괴시간(destroyTime)을 나누어 누적시킴
			blockColor.a -= Time.deltaTime / destroyTime;
			markColor.a -= Time.deltaTime / destroyTime;

			blockRenderer.color = blockColor;
			markRenderer.color = markColor;

			yield return null;
		}

		this.gameObject.SetActive(false);
	}
}
