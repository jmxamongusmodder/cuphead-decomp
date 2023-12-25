using System;
using UnityEngine;

// Token: 0x02000A80 RID: 2688
public class WeaponPeashotExProjectile : AbstractProjectile
{
	// Token: 0x06004046 RID: 16454 RVA: 0x00230B14 File Offset: 0x0022EF14
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead)
		{
			return;
		}
		if (this.timeUntilUnfreeze > 0f)
		{
			this.timeUntilUnfreeze -= CupheadTime.FixedDelta;
			this.currentSpeed = 0f;
		}
		else
		{
			this.currentSpeed = this.moveSpeed;
		}
		Vector2 vector = MathUtils.AngleToDirection(base.transform.eulerAngles.z) * this.currentSpeed;
		base.transform.AddPosition(vector.x * CupheadTime.FixedDelta, vector.y * CupheadTime.FixedDelta, 0f);
	}

	// Token: 0x06004047 RID: 16455 RVA: 0x00230BC0 File Offset: 0x0022EFC0
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		float num = this.damageDealer.DealDamage(hit);
		this.totalDamage += num;
		if (this.totalDamage > this.maxDamage)
		{
			this.Die();
		}
		if (num > 0f)
		{
			this.hitFXPrefab.Create(this.hitFxRoot.position);
			AudioManager.Play("player_ex_impact_hit");
			this.emitAudioFromObject.Add("player_ex_impact_hit");
			this.timeUntilUnfreeze = this.hitFreezeTime;
		}
	}

	// Token: 0x06004048 RID: 16456 RVA: 0x00230C4F File Offset: 0x0022F04F
	protected override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		if (hit.tag == "Parry")
		{
			return;
		}
		base.OnCollisionOther(hit, phase);
	}

	// Token: 0x04004715 RID: 18197
	[SerializeField]
	private Effect hitFXPrefab;

	// Token: 0x04004716 RID: 18198
	[SerializeField]
	private Transform hitFxRoot;

	// Token: 0x04004717 RID: 18199
	private float timeUntilUnfreeze;

	// Token: 0x04004718 RID: 18200
	public float moveSpeed;

	// Token: 0x04004719 RID: 18201
	public float hitFreezeTime;

	// Token: 0x0400471A RID: 18202
	private float totalDamage;

	// Token: 0x0400471B RID: 18203
	private float currentSpeed;

	// Token: 0x0400471C RID: 18204
	public float maxDamage;
}
