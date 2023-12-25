using System;
using UnityEngine;

// Token: 0x0200067A RID: 1658
public class FlyingGenieLevelSpawnerPoint : AbstractProjectile
{
	// Token: 0x1700039D RID: 925
	// (get) Token: 0x060022F2 RID: 8946 RVA: 0x001482D4 File Offset: 0x001466D4
	protected override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060022F3 RID: 8947 RVA: 0x001482DC File Offset: 0x001466DC
	public FlyingGenieLevelSpawnerPoint Create(Vector2 pos, float rotation, LevelProperties.FlyingGenie.Bullets properties)
	{
		FlyingGenieLevelSpawnerPoint flyingGenieLevelSpawnerPoint = base.Create(pos, rotation) as FlyingGenieLevelSpawnerPoint;
		flyingGenieLevelSpawnerPoint.properties = properties;
		return flyingGenieLevelSpawnerPoint;
	}

	// Token: 0x060022F4 RID: 8948 RVA: 0x00148300 File Offset: 0x00146700
	public void Shoot()
	{
		this.effect.Create(this.root.transform.position);
		int num = UnityEngine.Random.Range(1, 4);
		BasicProjectile basicProjectile = this.projectile.Create(this.root.transform.position, base.transform.eulerAngles.z - 90f, this.properties.childSpeed);
		basicProjectile.GetComponent<Animator>().Play("Bullet_" + num);
	}

	// Token: 0x060022F5 RID: 8949 RVA: 0x00148391 File Offset: 0x00146791
	public void Dead()
	{
		this.Die();
		this.StopAllCoroutines();
	}

	// Token: 0x060022F6 RID: 8950 RVA: 0x0014839F File Offset: 0x0014679F
	protected override void Die()
	{
		this.StopAllCoroutines();
	}

	// Token: 0x060022F7 RID: 8951 RVA: 0x001483A7 File Offset: 0x001467A7
	protected override void RandomizeVariant()
	{
	}

	// Token: 0x04002B8D RID: 11149
	[SerializeField]
	private Effect effect;

	// Token: 0x04002B8E RID: 11150
	[SerializeField]
	private BasicProjectile projectile;

	// Token: 0x04002B8F RID: 11151
	[SerializeField]
	private Transform root;

	// Token: 0x04002B90 RID: 11152
	private LevelProperties.FlyingGenie.Bullets properties;
}
