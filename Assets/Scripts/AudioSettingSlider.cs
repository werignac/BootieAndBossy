using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Slider))]
public class AudioSettingSlider : MonoBehaviour
{
	private Slider slider;

	[SerializeField]
	private UnityEvent<Action<float>> query;

	[SerializeField]
	private UnityEvent<float> onSliderChange = new UnityEvent<float>();

	private void Awake()
	{
		slider = GetComponent<Slider>();
		QueryAudioValue();
	}

	private void QueryAudioValue()
	{
		this.query.Invoke(this.ReceiveAudioValue);
	}

	private void ReceiveAudioValue(float newValue)
	{
		slider.value = newValue;
	}

	public void ChangeAudioValue(float newValue)
	{
		this.onSliderChange.Invoke(newValue);
	}


}
