using System;
using UnityEngine;

// Token: 0x020004C8 RID: 1224
public class AirplaneLevelTurretBullet : AbstractProjectile
{
	// Token: 0x060014B7 RID: 5303 RVA: 0x000BA108 File Offset: 0x000B8508
	public AirplaneLevelTurretBullet Create(Vector2 pos, Vector2 velocity, float gravity)
	{
		AirplaneLevelTurretBullet airplaneLevelTurretBullet = base.Create() as AirplaneLevelTurretBullet;
		airplaneLevelTurretBullet.velocity = velocity;
		airplaneLevelTurretBullet.transform.position = pos;
		airplaneLevelTurretBullet.gravity = gravity;
		return airplaneLevelTurretBullet;
	}

	// Token: 0x060014B8 RID: 5304 RVA: 0x000BA144 File Offset: 0x000B8544
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
		this.velocity.y = this.velocity.y - this.gravity * CupheadTime.FixedDelta;
	}

	// Token: 0x060014B9 RID: 5305 RVA: 0x000BA1A7 File Offset: 0x000B85A7
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060014BA RID: 5306 RVA: 0x000BA1C5 File Offset: 0x000B85C5
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			this.damageDealer.DealDamage(hit);
			AudioManager.Play("sfx_dlc_dogfight_p1_terrierplane_baseball_impact");
		}
	}

	// Token: 0x04001E27 RID: 7719
	private Vector2 velocity;

	// Token: 0x04001E28 RID: 7720
	private float gravity;
}
