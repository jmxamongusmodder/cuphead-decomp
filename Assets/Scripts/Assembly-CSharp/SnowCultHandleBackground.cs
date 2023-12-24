using UnityEngine;

public class SnowCultHandleBackground : AbstractPausableComponent
{
	[SerializeField]
	private SpriteRenderer[] fadeRenderers;
	[SerializeField]
	private float[] fadePeriod;
	[SerializeField]
	private float[] fadeOffset;
	[SerializeField]
	private float[] fadeMin;
	[SerializeField]
	private float[] fadeMax;
	[SerializeField]
	private Animator[] candles;
	[SerializeField]
	private Animator glimmer;
	[SerializeField]
	private Animator[] sparkles;
}
