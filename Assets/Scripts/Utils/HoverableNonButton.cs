using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoverableNonButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private UnityEvent onHover = new UnityEvent();
	[SerializeField]
	private UnityEvent onNotHover = new UnityEvent();

	public void OnPointerEnter(PointerEventData eventData)
	{
		onHover.Invoke();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		onNotHover.Invoke();
	}
}
