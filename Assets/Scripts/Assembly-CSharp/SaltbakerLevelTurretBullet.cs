using System;
using UnityEngine;

// Token: 0x020007DA RID: 2010
public class SaltbakerLevelTurretBullet : BasicProjectile
{
	// Token: 0x06002DE3 RID: 11747 RVA: 0x001B0F34 File Offset: 0x001AF334
	public SaltbakerLevelTurretBullet Create(Vector3 pos, float rotation, float speed, SaltbakerLevelSaltbaker parent)
	{
		SaltbakerLevelTurretBullet saltbakerLevelTurretBullet = base.Create(pos, rotation, speed) as SaltbakerLevelTurretBullet;
		saltbakerLevelTurretBullet.parent = parent;
		saltbakerLevelTurretBullet.animator.Play((!Rand.Bool()) ? "B" : "A");
		saltbakerLevelTurretBullet.animator.Update(0f);
		return saltbakerLevelTurretBullet;
	}

	// Token: 0x06002DE4 RID: 11748 RVA: 0x001B0F92 File Offset: 0x001AF392
	protected override void Start()
	{
		base.Start();
		this.parent.OnDeathEvent += this.Die;
	}

	// Token: 0x06002DE5 RID: 11749 RVA: 0x001B0FB2 File Offset: 0x001AF3B2
	protected override void OnDestroy()
	{
		this.parent.OnDeathEvent -= this.Die;
		base.OnDestroy();
	}

	// Token: 0x04003667 RID: 13927
	private SaltbakerLevelSaltbaker parent;
}
