using UnityEngine;

public class FlyingGenieLevelBomb : AbstractProjectile
{
	public enum BombType
	{
		Regular = 0,
		Diagonal = 1,
		PlusSized = 2,
	}

	public BombType bombType;
	public bool readyToDetonate;
	[SerializeField]
	private GameObject[] explosionBeams;
}
