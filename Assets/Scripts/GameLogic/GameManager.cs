using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

public class GameManager : MonoBehaviour
{
	public long score = 0;

	public GameObject[] blocks;

	// Map Size
	public int width = 10;
	public int height = 20;

	// Grid Size
	public Transform[,] grid;

	// Sprite List (숫자, 연산자 리스트)
	public Sprite[] sprites;
	public string[] operators = { "+", "-", "*", "/" };

	// 필요 Component
	public GameUIManager um;

	// 블록 배열
	private GameObject[] blockList = new GameObject[4];

	void Awake()
	{
		// Grid Size 정의
		grid = new Transform[width, height];
		um = FindObjectOfType<GameUIManager>();
	}

	void Start()
	{
		for(int i=1; i<4; i++) // 3개의 블록 미리 생성 (다음으로 보여줄 블록 3개)
        {
			blockList[i] = Instantiate(blocks[Random.Range(0, blocks.Length)], transform.position, Quaternion.identity);
		}

		NewTetrisBlock();
	}

	// 테트리스 블록 스폰
	public void NewTetrisBlock()
	{
		for(int i=0; i<3; i++) // 1, 2, 3번을 앞으로 하나씩 땡겨옴
        {
			blockList[i] = blockList[i + 1];
        }
		blockList[3] = Instantiate(blocks[Random.Range(0, blocks.Length)], transform.position, Quaternion.identity); // 새로운 블록 하나 생성 (3번)

		setBlockPosition();
	}

	//테트리스 블록 위치 설정
	public void setBlockPosition()
    {
		blockList[0].GetComponent<TetrisBlock>().setPreviousTime(Time.time); // 현재 시간을 넣어줌으로써 바로 떨어지는 것을 방지
		blockList[0].transform.position = new Vector3(3, 18, 0);
		blockList[0].transform.localScale = new Vector3(1, 1, 1);
		blockList[0].GetComponent<TetrisBlock>().enabled = true;

		for (int i=1; i<4; i++) // 블록 모양 구분 이름 말고 다른 방법이 좋을거같음
		{
			if (blockList[i].tag.Equals("Block_B")) 
				
			{
				blockList[i].transform.position = new Vector3(10.05f, 16.3f - (i - 1) * 3f, 0); // 모양에 따라 위치 변경
			}
			else
			{
				blockList[i].transform.position = new Vector3(9.7f, 16.3f - (i - 1) * 3f, 0);
			}
			blockList[i].transform.localScale = new Vector3(0.7f, 0.7f, 0.7f); // 미리보기는 크기 줄임
		}
	}

	// 점수 추가
	public void AddScore(long amount)
	{
		score += amount;
		um.SetScoreText(string.Format("{0:#,##0}", amount), string.Format("{0:#,##0}", score));
	}


// GameGrid Logic
	// 라인 체크
	public void CheckForLines()
	{
		for (int y = height - 1; y >= 0; y--)
		{
			if (HasLine(y))
			{
				if (ValidExpression(y) == "NoError") // 식 완성 체크
				{
					long score = CalcExpression(y);
					AddScore(score);
				}
				else
					Debug.Log(ValidExpression(y));
				DeleteLine(y);
				RowDown(y);
			}
		}
	}

	// y
	bool HasLine(int y)
	{
		for (int x = 0; x < width; x++)
		{
			if (grid[x, y] == null)
				return false;
		}
		return true;
	}

	void DeleteLine(int y)
	{
		for (int x = 0; x < width; x++)
		{
			Destroy(grid[x, y].gameObject);
			grid[x, y] = null;
		}
	}

	void RowDown(int i)
	{
		for (int y = i; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				if (grid[x, y] != null)
				{
					grid[x, y - 1] = grid[x, y];
					grid[x, y] = null;
					grid[x, y - 1].transform.position -= new Vector3(0, 1, 0);
				}
			}
		}
	}





	// 완성된 식인지 확인하는 함수 (i = y임)
	// TODO: 이후에 string 말고 bool을 반환타입으로 변경할 것임, 테스트와 오류검출을 위해 string을 반환타입으로 해두었음
	string ValidExpression(int y)
	{
		GameObject block;
		BlockData data;

		string frontChar = "";
		int operatorCnt = 0;
		int numCnt = 0;

		for (int x = 0; x < width; x++)
		{
			// 블록 정보를 받아옴
			block = grid[x, y].gameObject;
			data = block.GetComponent<BlockData>();

			// 1) 라인의 첫 번째에는 ×÷가 올 수 없고 끝부분에는 +-×÷가 올 수 없다. (×5+3-5+)
			if (x == 0)
			{
				if (data.blockValue == "*" || data.blockValue == "/")
					return "첫라인 Error";
			}
			// 2) 라인의 첫번째에 이미 +-연산자가 와있는 경우 두번째에는 어떤 연산자던 올 수 없다. (-+5...)
			else if (x == 1 && (data.blockValue == "+" || data.blockValue == "-" || data.blockValue == "*" || data.blockValue == "/"))
			{
				if (frontChar == "+" || frontChar == "-")
					return "0열에 +-가 와있으면 1열에는 연산자 불가";
			}
			else if (x == width - 1) // 라인의 끝부분에는 +-×÷가 올 수 없다.
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
		if (numCnt >= width)
		{
			return "숫자로만 완성함";
		}

		return "NoError";
	}

	// 식을 계산하는 함수
	long CalcExpression(int y)
	{
		GameObject block;
		BlockData data;

		string expression = "";

		// 블록에 저장된 값을 문자열로 가져오는 작업
		for (int x = 0; x < width; x++)
		{
			// 블록 정보를 받아옴
			block = grid[x, y].gameObject;
			data = block.GetComponent<BlockData>();

			expression += data.blockValue;
		}

		// 식 계산
		DataTable dt = new DataTable();
		var v = dt.Compute(expression, "");

		if (v.ToString() == "Infinity") return 0; // 0으로 나눈 경우
		long res = System.Convert.ToInt64(v);

		// TODO: 이후에 지울것
		Debug.Log(expression + "계산결과: " + res);

		return res;
	}
}
