using UnityEngine;

public class ClownLevelDucks : AbstractProjectile
{
	public bool isBombDuck;
	[SerializeField]
	private Effect explosionPrefab;
	[SerializeField]
	private Effect smokePrefab;
	[SerializeField]
	private Effect sparkPrefab;
	[SerializeField]
	private SpriteDeathParts[] deathParts;
	[SerializeField]
	private Transform bomb;
	[SerializeField]
	private GameObject body;
}
