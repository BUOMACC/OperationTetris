﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System.Data;


public class GameManager : MonoBehaviour
{

	public long score = 0;

	public GameObject[] blocks;

	// Map Size
	[Header("Map Size")]
	public int width = 9;
	public int height = 20;

	// Grid Size
	public Transform[,] grid;

	[Header("GameSetting")]
	public GameSetting.Difficulty difficulty = GameSetting.Difficulty.Easy; // 난이도
	public GameSetting.Mode mode = GameSetting.Mode.Normal;
	public float destroyTime = 0.2f; // 블록 파괴시간 (1 = 1초)

	// Sprite List (숫자, 연산자 리스트)
	[Header("Number, Operator List")]
	public SpriteAtlas atlas;
	public string[] spritesName;
	public string[] operators = { "+", "-", "*", "/" };

	// 필요 Component
	private GameUIManager um;

	// 블록 배열
	private GameObject[] blockList = new GameObject[4];

	// 블록 교체(저장)
	private GameObject savedBlock;
	private GameObject savedBlocktemp;
	private bool blockChanged = false;

	void Awake()
	{
		// Grid Size 정의
		grid = new Transform[width, height];
		um = FindObjectOfType<GameUIManager>();
	}

	void Start()
	{
		InitGameSetting();
		InitNextBlock();
		NewTetrisBlock();
	}

	// 게임 시작시 정보를 받아 설정함
	private void InitGameSetting()
	{
		difficulty = GameSetting.instance.difficulty; // 난이도 설정
		mode = GameSetting.instance.mode; // 모드 설정
	}

	// 처음 실행시 다음 블록 3개를 스폰
	public void InitNextBlock()
	{
		for (int i = 1; i < 4; i++) // 3개의 블록 미리 생성 (다음으로 보여줄 블록 3개)
		{
			blockList[i] = Instantiate(blocks[Random.Range(0, blocks.Length)], transform.position, Quaternion.identity);
		}
	}

	// 테트리스 블록 스폰
	public void NewTetrisBlock()
	{
		for(int i=0; i<3; i++) // 1, 2, 3번을 앞으로 하나씩 땡겨옴
        {
			blockList[i] = blockList[i + 1];
        }
		blockList[3] = Instantiate(blocks[Random.Range(0, blocks.Length)], transform.position, Quaternion.identity); // 새로운 블록 하나 생성 (3번)

		SetBlockPosition();
	}

	// 테트리스 블록 위치 설정
	public void SetBlockPosition()
    {
		blockList[0].GetComponent<TetrisBlock>().SetPreviousTime(Time.time); // 현재 시간을 넣어줌으로써 바로 떨어지는 것을 방지
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

	public void setBlockChanged()
	{
		blockChanged = false;
	}

	// 테트리스 블록 저장
	public void saveBlock()
	{
		if (blockChanged == false)
		{
			blockList[0].GetComponent<TetrisBlock>().SetPreviousTime(Time.time); // AddToGrid 함수가 호출되지 않도록 조건을 일부러 틀리게 맞춰줌
			blockList[0].GetComponent<TetrisBlock>().enabled = false;
			if (savedBlock == null) // 처음 블록 저장을 했을 때
			{
				savedBlock = blockList[0];
				NewTetrisBlock();
			}
			else // 두 번째 이후
			{
				savedBlocktemp = savedBlock;
				savedBlock = blockList[0];
				blockList[0] = savedBlocktemp;
				SetBlockPosition();
			}

			// 저장한 블록 위치 설정, 회전 초기화

			if (savedBlock.tag.Equals("Block_B"))

			{
				savedBlock.transform.position = new Vector3(10.05f, 5.3f, 0); // 모양에 따라 위치 변경
			}
			else
			{
				savedBlock.transform.position = new Vector3(9.7f, 5.3f, 0);
			}
			savedBlock.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
			savedBlock.GetComponent<TetrisBlock>().ResetBlockRatation();

			blockChanged = true;
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
	public IEnumerator CheckForLines()
	{
		for (int y = height - 1; y >= 0; y--)
		{
			if (HasLine(y))
			{
				if (ValidExpression(y) == "NoError") // 식 완성 체크
				{
					long score = CalcExpression(y);
					AddScore(score);
					if(difficulty == GameSetting.Difficulty.Hard) // 하드 모드인 경우(식 완성이 된 경우에만 제거)
					{
						DeleteLine(y);
						yield return new WaitForSeconds(destroyTime);
						RowDown(y);
					}
				}

				if (difficulty == GameSetting.Difficulty.Easy) // 하드 모드가 아닌 경우(식이 완성되지 않아도 제거)
				{
					DeleteLine(y);
					yield return new WaitForSeconds(destroyTime);
					RowDown(y);
				}
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
			//Destroy(grid[x, y].gameObject);

			grid[x, y].GetComponent<BlockData>().FadeOutAnimation(destroyTime);
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
