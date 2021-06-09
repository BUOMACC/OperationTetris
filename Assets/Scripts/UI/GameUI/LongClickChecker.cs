using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class LongClickChecker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public bool clicked = false;

	public void OnPointerDown(PointerEventData eventData)
	{
		clicked = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		clicked = false;
	}
}
