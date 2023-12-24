using UnityEngine;

public class ClownLevelRiders : AbstractCollidableObject
{
	[SerializeField]
	private SpriteRenderer backSeat;
	[SerializeField]
	private SpriteRenderer backRider;
	public bool inFront;
}
