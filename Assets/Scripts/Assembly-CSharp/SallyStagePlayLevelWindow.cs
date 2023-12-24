using UnityEngine;

public class SallyStagePlayLevelWindow : AbstractCollidableObject
{
	[SerializeField]
	private Transform projectileRoot;
	[SerializeField]
	private SallyStagePlayLevelWindowProjectile rulerPink;
	[SerializeField]
	private SallyStagePlayLevelWindowProjectile ruler;
	[SerializeField]
	private SallyStagePlayLevelWindowProjectile bottle;
	[SerializeField]
	private SpriteRenderer[] nunPink;
	public int windowNum;
}
