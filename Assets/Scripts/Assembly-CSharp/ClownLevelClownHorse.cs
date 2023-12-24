using UnityEngine;

public class ClownLevelClownHorse : LevelProperties.Clown.Entity
{
	[SerializeField]
	private GameObject clownHorseBody;
	[SerializeField]
	private GameObject clownHorseHead;
	[SerializeField]
	private ClownLevelClownSwing clownSwing;
	[SerializeField]
	private Transform projectileRoot;
	[SerializeField]
	private ClownLevelHorseshoe regularHorseshoe;
	[SerializeField]
	private ClownLevelHorseshoe pinkHorseshoe;
	[SerializeField]
	private Effect spitFxPrefabA;
	[SerializeField]
	private Effect spitFxPrefabB;
	[SerializeField]
	private Transform spitFxRoot;
}
