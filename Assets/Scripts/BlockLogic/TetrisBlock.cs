using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

public class TetrisBlock : MonoBehaviour
{
	[Header("블록 데이터")]
	public BlockData[] blockData; // 블록 데이터 정보 (블록마다 저장된 숫자, 연산자..)
	//public int chanceNum = 90; // 숫자가 나올 확률 (90이면 90%)

	public Vector3 rotationPoint;
	private float previousTime;

	GameManager gm; // GameManager 게임의 흐름을 관리
	GameUIManager um;

	void Awake()
	{
		gm = FindObjectOfType<GameManager>(); // Get GameManager
		um = FindObjectOfType<GameUIManager>();

		SetRandomBlockValue(); // 블록 생성시 블록마다 값을 줌
		this.enabled = false; // 맵으로 이동될 때까지 스크립트를 비활성화함
	}

	void Update()
    {
		// Left / Right Move
        if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			transform.position += new Vector3(-1, 0, 0);
			if(!ValidMove())
				transform.position -= new Vector3(-1, 0, 0);
		}
		else if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			transform.position += new Vector3(1, 0, 0);
			if (!ValidMove())
				transform.position -= new Vector3(1, 0, 0);
		}
		else if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			// Block Rotate
			RotateBlock(90);
			if (!ValidMove())
			{
				RotateBlock(-90);
			}
		}
		else if (Input.GetKeyDown(KeyCode.Space)) // Block Save
		{
			gm.SaveBlock();
		}

		// Down / Fast Down
		if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? gm.currentFallTime / 10 : gm.currentFallTime))
		{
			transform.position += new Vector3(0, -1, 0);
			if (!ValidMove())
			{
				transform.position -= new Vector3(0, -1, 0);
				AddToGrid();
				StartCoroutine(gm.CheckForLines());

				this.enabled = false;
				gm.NewTetrisBlock();

				gm.SetBlockChanged(); // 블록을 한 번만 바꿀 수 있게 해놓은 제한을 풀어줌
			}
			previousTime = Time.time;
		}
    }

	public void SetPreviousTime(float n) // 블록 생성시 바로 떨어지는 문제를 해결하기 위함
    {
		previousTime = n;
    }

	void RotateBlock(float angle)
	{
		// 블록 자체 회전
		transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), angle);
		// 회전시 값 부분은 월드포지션 기준 (0, 0, 0) 회전값을 로컬 포지션에 적용
		// 블록 자체가 회전되어도 표시된 숫자나 연산자는 똑바로 있어야 하기 때문
		for (int i = 0; i < blockData.Length; i++)
		{
			GameObject block = blockData[i].go_blockValue;
			block.transform.rotation = Quaternion.identity;
		}
	}

	public void ResetBlockRatation()
	{
		// 블록 자체 회전
		transform.rotation = Quaternion.identity;
		// 회전시 값 부분은 월드포지션 기준 (0, 0, 0) 회전값을 로컬 포지션에 적용
		// 블록 자체가 회전되어도 표시된 숫자나 연산자는 똑바로 있어야 하기 때문
		for (int i = 0; i < blockData.Length; i++)
		{
			GameObject block = blockData[i].go_blockValue;
			block.transform.rotation = Quaternion.identity;
		}
	}

	void AddToGrid()
	{
		foreach (Transform children in transform)
		{
			int roundedX = Mathf.RoundToInt(children.transform.position.x);
			int roundedY = Mathf.RoundToInt(children.transform.position.y);

			GameManager.grid[roundedX, roundedY] = children;
		}
	}

	/* (원본)
	// 블록마다 값을 랜덤하게 설정하기
	void SetBlockValue()
	{
		int rnum = 0;
		for (int i = 0; i < blockData.Length; i++)
		{
			rnum = Random.Range(0, 14); // 0이상 15미만의 값을 뽑아냄
			SpriteRenderer sr = blockData[i].go_blockValue.GetComponent<SpriteRenderer>();

			// Sprite 설정, 블록이 가진 값 설정 (0~9인 경우 그냥 넣고 10이상인 경우 operators값을 넣음)
			// rnum - 10하면 rnum이 10일때 operators의 0번째를 참조할 수 있으므로 -10을 한다
			sr.sprite = gm.sprites[rnum];
			blockData[i].blockValue = (rnum < 10) ? rnum.ToString() : gm.operators[rnum - 10];
		}
	}
	*/
	void SetRandomBlockValue()
	{
		int rnum = 0;
		string spriteName = "";
		SpriteRenderer sr;

		for (int i = 0; i < blockData.Length; i++)
		{
			// 값이 없는 경우에만 할당
			if(blockData[i].blockValue == "")
			{
				rnum = Random.Range(0, 100);
				sr = blockData[i].go_blockValue.GetComponent<SpriteRenderer>();

				if (rnum < blockData[i].chanceNum)  // chanceNum % 확률로 숫자만 나오도록 함
				{
					rnum = Random.Range(1, 10);
					spriteName = gm.spritesName[rnum];
					sr.sprite = gm.atlas.GetSprite(spriteName);
					blockData[i].blockValue = rnum.ToString();
				}
				else
				{
					rnum = Random.Range(10, 14); // gm.sprites의 순서가 숫자 10개 다음 연산자이므로 9~12의 숫자만 나오게 함
					spriteName = gm.spritesName[rnum];
					sr.sprite = gm.atlas.GetSprite(spriteName);
					blockData[i].blockValue = gm.operators[rnum - 10];  // rnum - 10을 하면 0~3까지 숫자이므로 operator 참조가 편리
				}
			}
		}
	}

	bool ValidMove()
	{
		foreach (Transform children in transform)
		{
			int roundedX = Mathf.RoundToInt(children.transform.position.x);
			int roundedY = Mathf.RoundToInt(children.transform.position.y);

			if (roundedX < 0 || roundedX >= gm.width || roundedY < 0 || roundedY >= gm.height)
			{
				return false;
			}

			if (GameManager.grid[roundedX, roundedY] != null)
			{
				return false;
			}
		}
		return true;
	}




	/* 짜놓고 사용안하는 함수 (혹시몰라 내버려둠)
	     식을 쪼개 배열에 담아주는 함수
	string[] SplitExpression(int i)
	{
		GameObject block;
		BlockData data;

		string expression = "";
		string[] nums = new string[10]; // 10개까지 저장 가능
		int index = 0; // nums의 인덱스

		// 블록에 저장된 값을 문자열로 가져오는 작업
		for (int j = 0; j < gm.width; j++)
		{
			// 블록 정보를 받아옴
			block = gm.grid[j, i].gameObject;
			data = block.GetComponent<BlockData>();

			expression += data.blockValue;
		}

		// 문자열로 저장된 식을 쪼개기
		for (int s = 0; s < expression.Length; s++)
		{
			// 라인의 첫부분이 아니고 연산자를 만나면 연산자를 저장하고 다음 인덱스로 넘김
			if (s != 0 &&
				(expression[s] == '+' || expression[s] == '-' || expression[s] == '*' || expression[s] == '/'))
			{
				index = index + 1;
				nums[index] = expression[s].ToString();
				index = index + 1;
				continue;
			}
			nums[index] += expression[s];
		}

		return nums;
	}
	*/
}
