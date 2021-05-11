using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

// 그리드에 저장된 블록
[System.Serializable]
public class PuzzleGridData
{
	public Vector2 position;
	public string value;
}

// 스폰될 블록
[System.Serializable]
public class PuzzleBlockData
{
	// C: Cube    I: 긴 막대    L  ...
	public string shape;
}

[System.Serializable]
public class CustomMap
{
	public string name;
	public string desc;
	public double targetScore;
	public List<PuzzleGridData> gridDatas;
	public List<PuzzleBlockData> blockDatas;
}


public class MapEditor : MonoBehaviour
{
	public CustomMap customMap;

	public GameObject block;
	public InputField input_X;
	public InputField input_Y;
	public Dropdown blockValue;

	public InputField mapName;
	public InputField mapDesc;

	[Header("Save Setting")]
	public string FileName = "CustomMap01.json";

	GameManager gm;

	void Awake()
	{
		gm = FindObjectOfType<GameManager>();
	}

	public void CreateBlockBtn()
	{
		int posX = int.Parse(input_X.text);
		int posY = int.Parse(input_Y.text);
		string value;

		// 이미 배치되어 있는경우 취소
		for(int i = 0; i < customMap.gridDatas.Count; i++)
		{
			if (customMap.gridDatas[i].position == new Vector2(posX, posY))
				return;
		}

		if(blockValue.value <= 8)
			value = (blockValue.value + 1).ToString();
		else
			value = gm.operators[(blockValue.value - 9)].ToString();

		// 블록 설치
		PlaceBlock(new Vector2(posX, posY), value);

		// 블록 배치정보 등록
		PuzzleGridData newData = new PuzzleGridData();
		newData.position = new Vector2(posX, posY);
		newData.value = value;
		customMap.gridDatas.Add(newData);
	}

	public void DeleteBlockBtn()
	{
		int posX = int.Parse(input_X.text);
		int posY = int.Parse(input_Y.text);

		// 그 좌표에 배치된 블록이 있다면 삭제
		for (int i = 0; i < customMap.gridDatas.Count; i++)
		{
			if (customMap.gridDatas[i].position == new Vector2(posX, posY))
			{
				GameObject deleteBlock = GameObject.Find("(" + posX + ", " + posY + ")");
				Destroy(deleteBlock);
				customMap.gridDatas.RemoveAt(i);
			}
		}
	}

	public void SaveDataBtn()
	{
		customMap.name = mapName.text;
		customMap.desc = mapDesc.text;

		string saveJson = JsonUtility.ToJson(customMap);
		string filePath = Application.dataPath + "/Saves/";
		File.WriteAllText(filePath + FileName, saveJson);
	}

	public void LoadDataBtn()
	{
		string loadJson = File.ReadAllText(Application.dataPath + "/Saves/" + FileName);
		CustomMap loadData = JsonUtility.FromJson<CustomMap>(loadJson);

		if (!CheckSaveMap(loadData))
		{
			Debug.Log("잘못된 파일입니다.");
			return;
		}


		for(int i = 0; i < loadData.gridDatas.Count; i++)
		{
			Vector2 pos = loadData.gridDatas[i].position;
			string value = loadData.gridDatas[i].value;

			PlaceBlock(pos, value);
		}
	}



	private bool CheckSaveMap(CustomMap map)
	{
		for (int i = 0; i < map.gridDatas.Count; i++)
		{
			if (map.gridDatas[i].position.x > gm.width ||
				map.gridDatas[i].position.y > gm.height)
			{
				return false;
			}

			if (map.gridDatas[i].value == "+" || map.gridDatas[i].value == "-" ||
				map.gridDatas[i].value == "*" || map.gridDatas[i].value == "/" ||
				map.gridDatas[i].value == "1" || map.gridDatas[i].value == "2" ||
				map.gridDatas[i].value == "3" || map.gridDatas[i].value == "4" ||
				map.gridDatas[i].value == "5" || map.gridDatas[i].value == "6" ||
				map.gridDatas[i].value == "7" || map.gridDatas[i].value == "8" ||
				map.gridDatas[i].value == "9")
			{
				continue;
			}
			else
			{
				return false;
			}
		}
		return true;
	}

	private void PlaceBlock(Vector2 pos, string value)
	{
		GameObject spawnedBlock = Instantiate(block, pos, Quaternion.identity);
		spawnedBlock.name = "(" + pos.x + ", " + pos.y + ")";
		BlockData data = spawnedBlock.GetComponentInChildren<BlockData>();
		SetBlockValue(data, value);
	}

	private void SetBlockValue(BlockData data, string value)
	{
		int spriteIndex = 0;
		if (value != "+" && value != "-" && value != "*" && value != "/")
			spriteIndex = int.Parse(value);
		else
		{
			// 연산자 기호 Sprite Name 위치 찾기
			for (int i = 0; i < gm.operators.Length; i++)
			{
				if (value == gm.operators[i])
				{
					spriteIndex = 10 + i;
					break;
				}
			}
		}

		data.blockValue = value;

		string spriteName = gm.spritesName[spriteIndex];
		data.markRenderer.sprite = gm.atlas.GetSprite(spriteName);
	}
}
