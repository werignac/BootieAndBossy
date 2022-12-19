using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace werignac.Displays
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class UITextDisplay : MonoBehaviour
	{
		private TextMeshProUGUI text;

		private void Start()
		{
			text = GetComponent<TextMeshProUGUI>();
		}

		public void Display(string toDisplay)
		{
			if (text == null)
				text = GetComponent<TextMeshProUGUI>();

			text.text = toDisplay;
		}

		public void Display(long toDisplay)
		{
			Display(toDisplay.ToString());
		}
	}
}
