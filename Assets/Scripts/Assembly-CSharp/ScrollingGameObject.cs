using UnityEngine;

public class ScrollingGameObject : AbstractMonoBehaviour
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
	private float speed;
	[SerializeField]
	private int size;
	[SerializeField]
	private bool resetTransforms;
}
