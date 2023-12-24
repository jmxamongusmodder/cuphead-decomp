using System;
using UnityEngine;

[Serializable]
public class TranslationElement
{
	[SerializeField]
	private int m_ID;
	[SerializeField]
	private int m_Depth;
	[SerializeField]
	public string key;
	[SerializeField]
	public Localization.Categories category;
	[SerializeField]
	public string description;
	[SerializeField]
	public Localization.Translation[] translations;
	[SerializeField]
	public Localization.Translation[] translationsCuphead;
	[SerializeField]
	public Localization.Translation[] translationsMugman;
	public bool enabled;
}
