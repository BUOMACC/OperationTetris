using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class Stage
{
	public string name;
	public double targetScore;
	public GameObject[] nextBlocks;
	public Map map;
}

public class PuzzleMode : MonoBehaviour
{
	public GameObject block;
	public Stage[] stages;
}
