using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

public class GameManager : MonoBehaviour
{
	public GameObject[] blocks;

	// Map Size
	public int width = 10;
	public int height = 20;

	// Grid Size
	public Transform[,] grid;

	// Sprite List (숫자, 연산자 리스트)
	public Sprite[] sprites;
	public string[] operators = { "+", "-", "*", "/" };

	void Awake()
	{
		// Grid Size 정의
		grid = new Transform[width, height];
	}

	void Start()
    {
		NewTetrisBlock();
	}

	public void NewTetrisBlock()
	{
		Instantiate(blocks[Random.Range(0, blocks.Length)], transform.position, Quaternion.identity);
	}
}
