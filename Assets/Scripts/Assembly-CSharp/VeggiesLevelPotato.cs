using UnityEngine;

public class VeggiesLevelPotato : LevelProperties.Veggies.Entity
{
	[SerializeField]
	private Transform gunRoot;
	[SerializeField]
	private VeggiesLevelSpit projectilePrefab;
	[SerializeField]
	private Effect spitEffect;
}
