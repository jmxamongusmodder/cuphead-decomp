using System;
using UnityEngine;

// Token: 0x02000AB7 RID: 2743
public class PlaneWeaponBombProjectile : AbstractProjectile
{
	// Token: 0x060041E7 RID: 16871 RVA: 0x00239F98 File Offset: 0x00238398
	protected override void Start()
	{
		base.Start();
		base.transform.SetScale(new float?(this.bulletSize), new float?(this.bulletSize), null);
		AudioManager.Play("plane_shmup_bomb_fire");
		this.emitAudioFromObject.Add("plane_shmup_bomb_fire");
	}

	// Token: 0x060041E8 RID: 16872 RVA: 0x00239FF0 File Offset: 0x002383F0
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead)
		{
			return;
		}
		if (this.shootsUp)
		{
			this.velocity.y = this.velocity.y + this.gravity * CupheadTime.FixedDelta;
			base.transform.position += this.velocity * CupheadTime.FixedDelta;
		}
		else
		{
			this.velocity.y = this.velocity.y - this.gravity * CupheadTime.FixedDelta;
			base.transform.position += this.velocity * CupheadTime.FixedDelta;
		}
	}

	// Token: 0x060041E9 RID: 16873 RVA: 0x0023A0B1 File Offset: 0x002384B1
	private void DealDamage(GameObject hit)
	{
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x060041EA RID: 16874 RVA: 0x0023A0C0 File Offset: 0x002384C0
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		this.DealDamage(hit);
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x060041EB RID: 16875 RVA: 0x0023A0D1 File Offset: 0x002384D1
	protected override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		if (hit.tag != "Parry")
		{
			base.OnCollisionOther(hit, phase);
		}
	}

	// Token: 0x060041EC RID: 16876 RVA: 0x0023A0F0 File Offset: 0x002384F0
	protected override void Die()
	{
		base.Die();
		base.GetComponent<SpriteRenderer>().enabled = false;
		AudioManager.Play("plane_shmup_bomb_explosion");
		this.emitAudioFromObject.Add("plane_shmup_bomb_explosion");
		this.explosion.Create(base.transform.position, this.Damage, base.DamageMultiplier, this.explosionSize);
	}

	// Token: 0x060041ED RID: 16877 RVA: 0x0023A156 File Offset: 0x00238556
	public void SetAnimation(PlayerId player)
	{
		base.animator.Play(((player != PlayerId.PlayerOne || PlayerManager.player1IsMugman) && (player != PlayerId.PlayerTwo || !PlayerManager.player1IsMugman)) ? "Bomb_MM" : "Bomb_CH");
	}

	// Token: 0x0400484E RID: 18510
	[SerializeField]
	private PlaneWeaponBombExplosion explosion;

	// Token: 0x0400484F RID: 18511
	public bool shootsUp;

	// Token: 0x04004850 RID: 18512
	public float explosionSize;

	// Token: 0x04004851 RID: 18513
	public float bulletSize;

	// Token: 0x04004852 RID: 18514
	public float gravity;

	// Token: 0x04004853 RID: 18515
	public Vector2 velocity;
}
