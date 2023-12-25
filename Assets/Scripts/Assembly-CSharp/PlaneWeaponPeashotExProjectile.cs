using System;
using UnityEngine;

// Token: 0x02000AC1 RID: 2753
public class PlaneWeaponPeashotExProjectile : AbstractProjectile
{
	// Token: 0x06004220 RID: 16928 RVA: 0x0023B8BC File Offset: 0x00239CBC
	public void Init()
	{
		this.Cuphead.enabled = ((this.PlayerId == PlayerId.PlayerOne && !PlayerManager.player1IsMugman) || (this.PlayerId == PlayerId.PlayerTwo && PlayerManager.player1IsMugman));
		this.Mugman.enabled = ((this.PlayerId == PlayerId.PlayerOne && PlayerManager.player1IsMugman) || (this.PlayerId == PlayerId.PlayerTwo && !PlayerManager.player1IsMugman));
	}

	// Token: 0x06004221 RID: 16929 RVA: 0x0023B93C File Offset: 0x00239D3C
	protected override void OnDealDamage(float damage, DamageReceiver receiver, DamageDealer damageDealer)
	{
		base.OnDealDamage(damage, receiver, damageDealer);
		this.chompFxPrefab.Create(this.chompFxRoot.position);
		this.state = PlaneWeaponPeashotExProjectile.State.Frozen;
		this.speed = 0f;
		this.timeSinceFrozen = 0f;
		AudioManager.Play("player_plane_weapon_ex_chomp");
		this.emitAudioFromObject.Add("player_plane_weapon_ex_chomp");
	}

	// Token: 0x06004222 RID: 16930 RVA: 0x0023B9A0 File Offset: 0x00239DA0
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead)
		{
			return;
		}
		PlaneWeaponPeashotExProjectile.State state = this.state;
		if (state != PlaneWeaponPeashotExProjectile.State.Idle)
		{
			if (state == PlaneWeaponPeashotExProjectile.State.Frozen)
			{
				this.timeSinceFrozen += CupheadTime.FixedDelta;
				if (this.timeSinceFrozen > this.FreezeTime)
				{
					this.state = PlaneWeaponPeashotExProjectile.State.Idle;
					this.speed = this.MaxSpeed;
				}
			}
		}
		else
		{
			this.speed = Mathf.Min(this.MaxSpeed, this.speed + this.Acceleration * CupheadTime.FixedDelta);
		}
		base.transform.AddPosition(this.speed * CupheadTime.FixedDelta, 0f, 0f);
	}

	// Token: 0x06004223 RID: 16931 RVA: 0x0023BA5D File Offset: 0x00239E5D
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		this.DealDamage(hit);
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x06004224 RID: 16932 RVA: 0x0023BA6E File Offset: 0x00239E6E
	protected override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		if (hit.tag == "Parry")
		{
			return;
		}
		base.OnCollisionOther(hit, phase);
	}

	// Token: 0x06004225 RID: 16933 RVA: 0x0023BA8E File Offset: 0x00239E8E
	private void DealDamage(GameObject hit)
	{
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06004226 RID: 16934 RVA: 0x0023BA9D File Offset: 0x00239E9D
	public override void OnLevelEnd()
	{
	}

	// Token: 0x04004896 RID: 18582
	public float MaxSpeed;

	// Token: 0x04004897 RID: 18583
	public float Acceleration;

	// Token: 0x04004898 RID: 18584
	public float FreezeTime;

	// Token: 0x04004899 RID: 18585
	[SerializeField]
	private Effect chompFxPrefab;

	// Token: 0x0400489A RID: 18586
	[SerializeField]
	private Transform chompFxRoot;

	// Token: 0x0400489B RID: 18587
	[SerializeField]
	private SpriteRenderer Cuphead;

	// Token: 0x0400489C RID: 18588
	[SerializeField]
	private SpriteRenderer Mugman;

	// Token: 0x0400489D RID: 18589
	private PlaneWeaponPeashotExProjectile.State state;

	// Token: 0x0400489E RID: 18590
	private float timeSinceFrozen;

	// Token: 0x0400489F RID: 18591
	public float speed;

	// Token: 0x02000AC2 RID: 2754
	public enum State
	{
		// Token: 0x040048A1 RID: 18593
		Idle,
		// Token: 0x040048A2 RID: 18594
		Frozen
	}
}
