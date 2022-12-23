using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickeableNonButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	[SerializeField]
	private UnityEvent onClick = new UnityEvent();
	[SerializeField]
	private UnityEvent onHover = new UnityEvent();
	[SerializeField]
	private UnityEvent onNotHover = new UnityEvent();

	public void OnPointerClick(PointerEventData eventData)
	{
		onClick.Invoke();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		onHover.Invoke();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		onNotHover.Invoke();
	}
}
