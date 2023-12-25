using System;
using UnityEngine;

// Token: 0x02000A65 RID: 2661
public class WeaponAccuracyProjectile : BasicProjectile
{
	// Token: 0x06003F7F RID: 16255 RVA: 0x0022B17C File Offset: 0x0022957C
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		this.hitEnemy = true;
	}

	// Token: 0x06003F80 RID: 16256 RVA: 0x0022B18D File Offset: 0x0022958D
	protected override void OnDestroy()
	{
		if (this.EnemyDeath != null)
		{
			this.EnemyDeath(this.hitEnemy);
		}
		base.OnDestroy();
	}

	// Token: 0x06003F81 RID: 16257 RVA: 0x0022B1B1 File Offset: 0x002295B1
	protected override void Die()
	{
		base.Die();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003F82 RID: 16258 RVA: 0x0022B1C4 File Offset: 0x002295C4
	public void SetSize(float size)
	{
		base.transform.SetScale(new float?(size), new float?(size), null);
	}

	// Token: 0x04004674 RID: 18036
	public WeaponAccuracyProjectile.OnEnemyDeath EnemyDeath;

	// Token: 0x04004675 RID: 18037
	private bool hitEnemy;

	// Token: 0x02000A66 RID: 2662
	// (Invoke) Token: 0x06003F84 RID: 16260
	public delegate void OnEnemyDeath(bool hitEnemy);
}
