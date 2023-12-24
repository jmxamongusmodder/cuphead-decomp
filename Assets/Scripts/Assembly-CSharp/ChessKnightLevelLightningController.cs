using UnityEngine;

public class ChessKnightLevelLightningController : AbstractMonoBehaviour
{
	[SerializeField]
	private MinMax lightningDelayRange;
	[SerializeField]
	private SpriteRenderer rend;
	[SerializeField]
	private Renderer glowTexture;
	[SerializeField]
	private float[] glowIntensity;
}
