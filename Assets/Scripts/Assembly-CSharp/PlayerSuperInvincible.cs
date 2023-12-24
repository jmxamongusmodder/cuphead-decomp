using UnityEngine;

public class PlayerSuperInvincible : AbstractPlayerSuper
{
	[SerializeField]
	private Material superMaterial;
	[SerializeField]
	private Effect sparkle;
	[SerializeField]
	private float sparkleSpawnTime;
	[SerializeField]
	private Vector3 shadowOffset;
	[SerializeField]
	private GameObject shadowCuphead;
	[SerializeField]
	private GameObject shadowMugman;
}
