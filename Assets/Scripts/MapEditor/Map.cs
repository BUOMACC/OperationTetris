using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map", menuName = "Puzzle/Create New Map")]
public class Map : ScriptableObject
{
	public GridData[] gridDatas;
}

[System.Serializable]
public class GridData
{
	public Vector2 pos;
	public string blockValue = "0";
	public Color blockColor = Color.white;
}