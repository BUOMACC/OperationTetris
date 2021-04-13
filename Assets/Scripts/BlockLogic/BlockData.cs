using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockData : MonoBehaviour
{
	public GameObject go_blockValue; // 각 블록에 표시된 문자 오브젝트, 외부 호출시 사용
	public string blockValue; // 무엇이 적혔는지 <EX: 0이 적혔는지 5가 적혔는지..>
	public int chanceNum = 100;
}
