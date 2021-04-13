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
	public float fallTime = 0.8f;
	private float previousTime;

	GameManager gm; // GameManager 게임의 흐름을 관리
	UIManager um;

	void Awake()
	{
		gm = FindObjectOfType<GameManager>(); // Get GameManager
		um = FindObjectOfType<UIManager>();
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
			if (HasLine(i))
			{
				if (ValidExpression(i) == "NoError") // 식 완성 체크
				{
					long score = CalcExpression(i);
					gm.AddScore(score);
				}
				else
					Debug.Log(ValidExpression(i));
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

	// 완성된 식인지 확인하는 함수 (i = y임)
	// TODO: 이후에 string 말고 bool을 반환타입으로 변경할 것임, 테스트와 오류검출을 위해 string을 반환타입으로 해두었음
	string ValidExpression(int i)
	{
		GameObject block;
		BlockData data;

		string frontChar = "";
		int operatorCnt = 0;
		int numCnt = 0;

		for (int j = 0; j < gm.width; j++)
		{
			// 블록 정보를 받아옴
			block = gm.grid[j, i].gameObject;
			data = block.GetComponent<BlockData>();

			// 1) 라인의 첫 번째에는 ×÷가 올 수 없고 끝부분에는 +-×÷가 올 수 없다. (×5+3-5+)
			if (j == 0)
			{
				if (data.blockValue == "*" || data.blockValue == "/")
					return "첫라인 Error";
			}
			// 2) 라인의 첫번째에 이미 +-연산자가 와있는 경우 두번째에는 어떤 연산자던 올 수 없다. (-+5...)
			else if(j == 1 && (data.blockValue == "+" || data.blockValue == "-" || data.blockValue == "*" || data.blockValue == "/"))
			{
				if(frontChar == "+" || frontChar == "-")
					return "0열에 +-가 와있으면 1열에는 연산자 불가";
			}
			else if (j == gm.width - 1) // 라인의 끝부분에는 +-×÷가 올 수 없다.
			{
				if (data.blockValue == "+" || data.blockValue == "-" || data.blockValue == "*" || data.blockValue == "/")
					return "끝라인 Error";
			}

			// 3) 사칙연산 기호 이후에 바로 ×÷가 다시 올 수 없다. (3××5...)
			if (frontChar == "*" || frontChar == "/" || frontChar == "+" || frontChar == "-")
			{
				if (data.blockValue == "*" || data.blockValue == "/") return "x/ 바로못옴"; // 앞에 이미 ×÷ 기호가 한번 온 경우 (flag = true를 의미)
			}

			// 4) 사칙연산이 두번 연속으로 사용된 경우 +- 기호를 다시 사용할 수 없다.
			if (data.blockValue == "+" || data.blockValue == "-")
			{
				if (operatorCnt >= 2) return "사칙연산 이미 두번연속으로 왔음";
			}

			// 이 블록의 정보가 연산자인 경우 연산자 카운트 +1 (연속), 아닌경우 초기화
			if (data.blockValue == "+" || data.blockValue == "-" || data.blockValue == "*" || data.blockValue == "/")
				operatorCnt++;
			else
			{
				operatorCnt = 0;
				numCnt++;
			}
			frontChar = data.blockValue;
		}
		// 5) 숫자로만 완성됐을 경우 점수를 적게줌
		if(numCnt >= gm.width)
		{
			return "숫자로만 완성함";
		}

		return "NoError";
	}

	// 식을 쪼개 배열에 담아주는 함수
	long CalcExpression(int i)
	{
		GameObject block;
		BlockData data;

		string expression = "";

		// 블록에 저장된 값을 문자열로 가져오는 작업
		for (int j = 0; j < gm.width; j++)
		{
			// 블록 정보를 받아옴
			block = gm.grid[j, i].gameObject;
			data = block.GetComponent<BlockData>();

			expression += data.blockValue;
		}

		// 식 계산
		DataTable dt = new DataTable();
		var v = dt.Compute(expression, "");

		if(v.ToString() == "Infinity") return 0; // 0으로 나눈 경우
		long res = System.Convert.ToInt64(v);

		// TODO: 이후에 지울것
		Debug.Log(expression + "계산결과: " + res);

		return res;
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
		int rnum = 0;
		SpriteRenderer sr;

		for (int i = 0; i < blockData.Length; i++)
		{
			rnum = Random.Range(0, 100);
			sr = blockData[i].go_blockValue.GetComponent<SpriteRenderer>();
			Debug.Log(blockData[i].chanceNum);

			if (rnum < blockData[i].chanceNum)	// chanceNum % 확률로 숫자만 나오도록 함
            {
				rnum = Random.Range(0, 10);
				sr.sprite = gm.sprites[rnum];
				blockData[i].blockValue = rnum.ToString();
			}
            else
            {
				rnum = Random.Range(10, 14);	// gm.sprites의 순서가 숫자 10개 다음 연산자이므로 10~13의 숫자만 나오게 함
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
