using System;
using UnityEngine;

public class AbstractProjectile : AbstractCollidableObject
{
	[Serializable]
	public class CollisionProperties
	{
		public bool Walls;
		public bool Ceiling;
		public bool Ground;
		public bool Enemies;
		public bool EnemyProjectiles;
		public bool Player;
		public bool PlayerProjectiles;
		public bool Other;
	}

	[SerializeField]
	private bool _canParry;
	public float Damage;
	public float DamageRate;
	public PlayerId PlayerId;
	public DamageDealer.DamageTypesManager DamagesType;
	public CollisionProperties CollisionDeath;
}
