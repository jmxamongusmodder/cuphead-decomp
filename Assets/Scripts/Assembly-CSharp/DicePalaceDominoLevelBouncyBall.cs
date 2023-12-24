using UnityEngine;

public class DicePalaceDominoLevelBouncyBall : AbstractProjectile
{
	[SerializeField]
	private Effect hitEffectPrefab;
	[SerializeField]
	private Effect explosion;
	[SerializeField]
	private Transform[] toRotate;
}
