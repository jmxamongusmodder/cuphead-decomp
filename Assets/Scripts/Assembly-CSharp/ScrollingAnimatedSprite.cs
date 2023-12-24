using UnityEngine;

public class ScrollingAnimatedSprite : AbstractPausableComponent
{
	public enum Axis
	{
		X = 0,
		Y = 1,
	}

	public Axis axis;
	[SerializeField]
	private bool negativeDirection;
	[SerializeField]
	public float speed;
	[SerializeField]
	private int offset;
	[SerializeField]
	private int count;
}
