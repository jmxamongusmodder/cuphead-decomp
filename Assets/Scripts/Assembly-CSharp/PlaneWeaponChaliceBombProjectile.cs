using System;
using UnityEngine;

// Token: 0x02000ABB RID: 2747
public class PlaneWeaponChaliceBombProjectile : AbstractProjectile
{
	// Token: 0x060041FB RID: 16891 RVA: 0x0023A4BC File Offset: 0x002388BC
	protected override void Start()
	{
		base.Start();
		base.transform.SetScale(new float?(this.size), new float?(this.size), null);
		AudioManager.Play("plane_shmup_bomb_fire");
		this.emitAudioFromObject.Add("plane_shmup_bomb_fire");
	}

	// Token: 0x060041FC RID: 16892 RVA: 0x0023A514 File Offset: 0x00238914
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead)
		{
			return;
		}
		this.velocity.y = this.velocity.y - this.gravity * CupheadTime.FixedDelta;
		base.transform.position += this.velocity * CupheadTime.FixedDelta;
	}

	// Token: 0x060041FD RID: 16893 RVA: 0x0023A57C File Offset: 0x0023897C
	private void DealDamage(GameObject hit)
	{
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x060041FE RID: 16894 RVA: 0x0023A58B File Offset: 0x0023898B
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		this.DealDamage(hit);
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x060041FF RID: 16895 RVA: 0x0023A59C File Offset: 0x0023899C
	protected override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		if (hit.tag != "Parry")
		{
			base.OnCollisionOther(hit, phase);
		}
	}

	// Token: 0x06004200 RID: 16896 RVA: 0x0023A5BC File Offset: 0x002389BC
	protected override void Die()
	{
		base.Die();
		base.GetComponent<SpriteRenderer>().enabled = false;
		AudioManager.Play("plane_shmup_bomb_explosion");
		this.emitAudioFromObject.Add("plane_shmup_bomb_explosion");
		this.explosion.Create(base.transform.position, this.damageExplosion, base.DamageMultiplier, this.explosionSize);
		this.explosion.animator.Play((!this.isA) ? "B" : "A");
	}

	// Token: 0x06004201 RID: 16897 RVA: 0x0023A64C File Offset: 0x00238A4C
	public void SetAnimation(bool isA)
	{
		this.isA = isA;
		base.animator.Play((!isA) ? "B" : "A");
	}

	// Token: 0x04004863 RID: 18531
	[SerializeField]
	private PlaneWeaponBombExplosion explosion;

	// Token: 0x04004864 RID: 18532
	public float explosionSize;

	// Token: 0x04004865 RID: 18533
	public float gravity;

	// Token: 0x04004866 RID: 18534
	public float damageExplosion;

	// Token: 0x04004867 RID: 18535
	public float size;

	// Token: 0x04004868 RID: 18536
	private bool isA;

	// Token: 0x04004869 RID: 18537
	public Vector2 velocity;
}
