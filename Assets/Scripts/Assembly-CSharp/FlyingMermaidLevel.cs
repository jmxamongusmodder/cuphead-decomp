using System;
using UnityEngine;

public class FlyingMermaidLevel : Level
{
	[Serializable]
	public class Prefabs
	{
	}

	[SerializeField]
	private FlyingMermaidLevelMermaid mermaid;
	[SerializeField]
	private Prefabs prefabs;
	[SerializeField]
	private FlyingMermaidLevelMerdusa merdusa;
	[SerializeField]
	private FlyingMermaidLevelMerdusaHead merdusaHead;
	[SerializeField]
	private Sprite _bossPortraitMain;
	[SerializeField]
	private Sprite _bossPortraitMerdusa;
	[SerializeField]
	private Sprite _bossPortraitHead;
	[SerializeField]
	private string _bossQuoteMain;
	[SerializeField]
	private string _bossQuoteMerdusa;
	[SerializeField]
	private string _bossQuoteHead;
}
