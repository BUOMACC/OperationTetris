using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ChangeColor : MonoBehaviour
{
	public FastMobileBloom bloom;
	public float speed = 1.0f;

	int dir = 1;

	void Update()
	{
		bloom.intensity += speed * Time.deltaTime * dir;

		if (bloom.intensity >= 4) dir = -1;
		else if (bloom.intensity <= 0) dir = 1;
	}
}
