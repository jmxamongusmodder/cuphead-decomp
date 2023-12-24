using UnityEngine;

public class FlyingBlimpLevelFadeBackground : ScrollingSprite
{
	public bool fadeOriginal;
	[SerializeField]
	private FlyingBlimpLevelMoonLady moonLady;
	[SerializeField]
	private Transform replacementSprite;
}
