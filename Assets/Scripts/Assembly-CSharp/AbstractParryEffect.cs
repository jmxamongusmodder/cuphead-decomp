using UnityEngine;

public class AbstractParryEffect : Effect
{
	[SerializeField]
	private GameObject sprites;
	[SerializeField]
	private Effect spark;
	[SerializeField]
	private ParryAttackSpark parryAttack;
}
