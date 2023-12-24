using UnityEngine;

public class CircusPlatformingLevelPoleHandler : AbstractPausableComponent
{
	[SerializeField]
	private int poleBotCount;
	[SerializeField]
	private Transform poleRoot;
	[SerializeField]
	private CircusPlatformingLevelPoleBot poleBot;
}
