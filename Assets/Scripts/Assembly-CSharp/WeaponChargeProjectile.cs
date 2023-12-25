using System;
using UnityEngine;

// Token: 0x02000A71 RID: 2673
public class WeaponChargeProjectile : BasicProjectile
{
	// Token: 0x06003FD8 RID: 16344 RVA: 0x0022D584 File Offset: 0x0022B984
	protected override void Die()
	{
		if (this.fullyCharged)
		{
			Vector2 vector = MathUtils.AngleToDirection(base.transform.eulerAngles.z) * 75f;
			base.transform.AddPosition(vector.x, vector.y, 0f);
		}
		base.transform.eulerAngles = new Vector3(0f, 0f, (float)UnityEngine.Random.Range(0, 360));
		base.Die();
		if (this.fullyCharged)
		{
			AudioManager.Play("player_weapon_charge_full_impact");
			this.emitAudioFromObject.Add("player_weapon_charge_full_impact");
		}
	}

	// Token: 0x040046B2 RID: 18098
	public bool fullyCharged;
}
