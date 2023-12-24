using UnityEngine;

public class BatLevelBouncer : AbstractCollidableObject
{
	public enum SideHit
	{
		Top = 0,
		Bottom = 1,
		Left = 2,
		Right = 3,
		None = 4,
	}

	[SerializeField]
	private BatLevelPinkCore pinkPrefab;
	public SideHit lastSideHit;
}
