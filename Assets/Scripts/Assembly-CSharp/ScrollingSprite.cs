using UnityEngine;

public class ScrollingSprite : AbstractPausableComponent
{
	public enum Axis
	{
		X = 0,
		Y = 1,
	}

	public Axis axis;
	[SerializeField]
	protected bool negativeDirection;
	[SerializeField]
	private bool onLeft;
	[SerializeField]
	private bool isRotated;
	public float speed;
	[SerializeField]
	public float offset;
	[SerializeField]
	private int count;
}
