using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTetrisBlock : MonoBehaviour
{
	public GameObject[] blocks;

    // Start is called before the first frame update
    void Start()
    {
		NewTetrisBlock();
	}

	public void NewTetrisBlock()
	{
		Instantiate(blocks[Random.Range(0, blocks.Length)], transform.position, Quaternion.identity);
	}
}
