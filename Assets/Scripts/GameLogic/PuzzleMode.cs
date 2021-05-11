using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GridData
{
	public Vector2 pos;
	public string blockValue = "0";
	public Color blockColor = Color.white;
}

[System.Serializable]
public class Stage
{
	public string name;
	public GameObject[] nextBlocks;
	public GridData[] gridDatas;
}

public class PuzzleMode : MonoBehaviour
{
	public Stage[] stages;
	public GameObject block;
}
