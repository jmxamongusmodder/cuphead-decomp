using UnityEngine;

public class BaronessLevelBackgroundChange : AbstractPausableComponent
{
	public enum B_Axis
	{
		X = 0,
		Y = 1,
	}

	public B_Axis b_axis;
	public float speed;
	[SerializeField]
	private bool isClouds;
	[SerializeField]
	protected bool b_negativeDirection;
	[SerializeField]
	protected int b_offset;
	[SerializeField]
	protected int b_count;
	[SerializeField]
	private BaronessLevelCastle baroness;
	[SerializeField]
	private OneTimeScrollingSprite sprite;
}
