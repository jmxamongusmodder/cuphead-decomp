using UnityEngine;

public class SpriteDeathPartsDLC : SpriteDeathParts
{
	[SerializeField]
	private bool progressiveBlur;
	[SerializeField]
	private float blurIncreaseSpeed;
	[SerializeField]
	private bool progressiveDim;
	[SerializeField]
	private float dimIncreaseSpeed;
	[SerializeField]
	private SpriteRenderer rend;
}
