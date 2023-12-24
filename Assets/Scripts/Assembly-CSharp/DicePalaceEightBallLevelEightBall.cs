using UnityEngine;
using System.Collections.Generic;

public class DicePalaceEightBallLevelEightBall : LevelProperties.DicePalaceEightBall.Entity
{
	[SerializeField]
	private Effect attackEffect;
	[SerializeField]
	private Effect projectileEffect;
	[SerializeField]
	private Transform root;
	[SerializeField]
	private BasicProjectile projectile;
	[SerializeField]
	private BasicProjectile pinkProjectile;
	[SerializeField]
	private List<DicePalaceEightBallLevelPoolBall> balls;
}
