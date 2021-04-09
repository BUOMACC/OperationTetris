using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
	[Header("블록 데이터")]
	public BlockData[] blockData; // 블록 데이터 정보 (블록마다 저장된 숫자, 연산자..)

	public Vector3 rotationPoint;
	public float fallTime = 0.8f;
	private float previousTime;

	GameManager gm; // GameManager 게임의 흐름을 관리

	void Awake()
	{
		gm = FindObjectOfType<GameManager>(); // Get GameManager
	}

	void Start()
	{
		SetBlockValue(); // 블록 생성시 블록마다 값을 줌
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

		// Down / Fast Down
		if(Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
		{
			transform.position += new Vector3(0, -1, 0);
			if (!ValidMove())
			{
				transform.position -= new Vector3(0, -1, 0);
				AddToGrid();
				CheckForLines();

				this.enabled = false;
				gm.NewTetrisBlock();
			}
			previousTime = Time.time;
		}
    }

	void CheckForLines()
	{
		for(int i = gm.height-1; i >= 0; i--)
		{
			if(HasLine(i))
			{
				DeleteLine(i);
				RowDown(i);
			}
		}
	}

	bool HasLine(int i)
	{
		for(int j = 0; j < gm.width; j++)
		{
			if (gm.grid[j, i] == null)
				return false;
		}
		return true;
	}

	void DeleteLine(int i)
	{
		for (int j = 0; j < gm.width; j++)
		{
			Destroy(gm.grid[j, i].gameObject);
			gm.grid[j, i] = null;
		}
	}

	void RowDown(int i)
	{
		for(int y = i; y < gm.height; y++)
		{
			for(int j = 0; j < gm.width; j++)
			{
				if(gm.grid[j, y] != null)
				{
					gm.grid[j, y - 1] = gm.grid[j, y];
					gm.grid[j, y] = null;
					gm.grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
				}
			}
		}
	}

	void AddToGrid()
	{
		foreach (Transform children in transform)
		{
			int roundedX = Mathf.RoundToInt(children.transform.position.x);
			int roundedY = Mathf.RoundToInt(children.transform.position.y);

			gm.grid[roundedX, roundedY] = children;
		}
	}

	void RotateBlock(float angle)
	{
		// 블록 자체 회전
		transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), angle);
		// 회전시 값 부분은 월드포지션 기준 (0, 0, 0) 회전값을 로컬 포지션에 적용
		// 블록 자체가 회전되어도 표시된 숫자나 연산자는 똑바로 있어야 하기 때문
		for(int i = 0; i < blockData.Length; i++)
		{
			GameObject block = blockData[i].go_blockValue;
			block.transform.rotation = Quaternion.identity;
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
	void SetBlockValue()
	{
		int chanceNum = 90;	// 숫자가 나올 확률 (90이면 90%)
		int rnum = 0;
		for (int i = 0; i < blockData.Length; i++)
		{
			rnum = Random.Range(0, 100);
			if(rnum < chanceNum)	// chanceNum % 확률로 숫자만 나오도록 함
            {
				rnum = Random.Range(0, 10);
				SpriteRenderer sr = blockData[i].go_blockValue.GetComponent<SpriteRenderer>();
				sr.sprite = gm.sprites[rnum];
				blockData[i].blockValue = rnum.ToString();
			}
            else
            {
				rnum = Random.Range(10, 14);	// gm.sprites의 순서가 숫자 10개 다음 연산자이므로 10~13의 숫자만 나오게 함
				SpriteRenderer sr = blockData[i].go_blockValue.GetComponent<SpriteRenderer>();
				sr.sprite = gm.sprites[rnum];
				blockData[i].blockValue = gm.operators[rnum - 10];	// rnum - 10을 하면 0~3까지 숫자이므로 operator 참조가 편리
			}
		}
	}

	bool ValidMove()
	{
		foreach (Transform children in transform)
		{
			int roundedX = Mathf.RoundToInt(children.transform.position.x);
			int roundedY = Mathf.RoundToInt(children.transform.position.y);

			if(roundedX < 0 || roundedX >= gm.width || roundedY < 0 || roundedY >= gm.height)
			{
				return false;
			}

			if(gm.grid[roundedX,roundedY] != null)
			{
				return false;
			}
		}
		return true;
	}
}
