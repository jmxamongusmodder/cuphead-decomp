using UnityEngine;

public class MouseLevelSawBlade : AbstractCollidableObject
{
	[SerializeField]
	private float initX;
	[SerializeField]
	private float idleX;
	[SerializeField]
	private float attackMinX;
	[SerializeField]
	private float attackMaxX;
	[SerializeField]
	private Transform blade;
	[SerializeField]
	private float rotateSpeed;
	[SerializeField]
	private int sawId;
	[SerializeField]
	private int stickId;
}
