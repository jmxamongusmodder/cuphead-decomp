using UnityEngine;

public class MapPlayerDust : Effect
{
	[SerializeField]
	private MinMax scaleRange;
	[SerializeField]
	private MinMax opacityRange;
	[SerializeField]
	private Vector3 offset;
	[SerializeField]
	private SpriteRenderer spriteRenderer;
}
