using System;
using UnityEngine;

// Token: 0x02000908 RID: 2312
public class PlatformingLevelParryObject : ParrySwitch
{
	// Token: 0x06003642 RID: 13890 RVA: 0x001F7B23 File Offset: 0x001F5F23
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06003643 RID: 13891 RVA: 0x001F7B36 File Offset: 0x001F5F36
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06003644 RID: 13892 RVA: 0x001F7B4E File Offset: 0x001F5F4E
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.hurtsPlayer && this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x04003E3A RID: 15930
	public bool hurtsPlayer;

	// Token: 0x04003E3B RID: 15931
	private DamageDealer damageDealer;
}
