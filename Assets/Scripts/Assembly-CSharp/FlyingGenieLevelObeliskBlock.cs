using System;
using UnityEngine;

// Token: 0x02000675 RID: 1653
public class FlyingGenieLevelObeliskBlock : AbstractProjectile
{
	// Token: 0x060022CD RID: 8909 RVA: 0x00146EA8 File Offset: 0x001452A8
	public void Init(Vector3 pos, LevelProperties.FlyingGenie.Obelisk properties)
	{
		base.transform.position = pos;
		this.properties = properties;
		this.rootPos = new Vector3(base.GetComponent<Renderer>().bounds.size.x / 2f + 10f, 0f, 0f);
	}

	// Token: 0x060022CE RID: 8910 RVA: 0x00146F04 File Offset: 0x00145304
	protected override void Start()
	{
		base.Start();
		this.darkSprite.sortingOrder = base.GetComponent<SpriteRenderer>().sortingOrder + 1;
	}

	// Token: 0x060022CF RID: 8911 RVA: 0x00146F24 File Offset: 0x00145324
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060022D0 RID: 8912 RVA: 0x00146F42 File Offset: 0x00145342
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060022D1 RID: 8913 RVA: 0x00146F60 File Offset: 0x00145360
	public void ShootRegular(float angle)
	{
		this.projectile.Create(base.transform.position + this.rootPos, angle, this.properties.obeliskShootSpeed);
	}

	// Token: 0x060022D2 RID: 8914 RVA: 0x00146F95 File Offset: 0x00145395
	public void ShootPink(float angle)
	{
		this.pinkProjectile.Create(base.transform.position + this.rootPos, angle, this.properties.obeliskShootSpeed);
	}

	// Token: 0x04002B6F RID: 11119
	[SerializeField]
	private BasicProjectile projectile;

	// Token: 0x04002B70 RID: 11120
	[SerializeField]
	private BasicProjectile pinkProjectile;

	// Token: 0x04002B71 RID: 11121
	[SerializeField]
	private SpriteRenderer darkSprite;

	// Token: 0x04002B72 RID: 11122
	private LevelProperties.FlyingGenie.Obelisk properties;

	// Token: 0x04002B73 RID: 11123
	private Vector3 rootPos;
}
