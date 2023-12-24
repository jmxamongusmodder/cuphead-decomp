using System;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class AbstractUIInteractionDialogue : AbstractMonoBehaviour
{
	[Serializable]
	public class Properties
	{
		public string text;
		public string subtext;
		public AbstractUIInteractionDialogue.AnimationType animationType;
		public CupheadButton button;
	}

	public enum AnimationType
	{
		Full = 0,
		Individual = 1,
	}

	[SerializeField]
	protected Text uiText;
	[SerializeField]
	protected TextMeshProUGUI tmpText;
	[SerializeField]
	protected CupheadGlyph glyph;
	[SerializeField]
	protected RectTransform back;
}
