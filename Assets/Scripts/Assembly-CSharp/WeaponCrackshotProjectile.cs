using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A75 RID: 2677
public class WeaponCrackshotProjectile : BasicProjectile
{
	// Token: 0x06003FF6 RID: 16374 RVA: 0x0022E52C File Offset: 0x0022C92C
	protected override void Start()
	{
		base.Start();
		base.animator.Play(this.variant.ToString(), 0, UnityEngine.Random.Range(0f, 1f));
		this.damageDealer.isDLCWeapon = true;
		AudioManager.Play("player_weapon_crackshot_shoot");
		this.emitAudioFromObject.Add("player_weapon_crackshot_shoot");
	}

	// Token: 0x06003FF7 RID: 16375 RVA: 0x0022E591 File Offset: 0x0022C991
	protected override void OnDieDistance()
	{
		if (base.dead)
		{
			return;
		}
		this.Die();
		base.animator.SetTrigger("OnDistanceDie");
	}

	// Token: 0x06003FF8 RID: 16376 RVA: 0x0022E5B8 File Offset: 0x0022C9B8
	protected override void Die()
	{
		this.move = false;
		base.Die();
		this.coll.enabled = false;
		if (base.animator.GetCurrentAnimatorStateInfo(0).IsTag("Comet"))
		{
			base.animator.Play((!Rand.Bool()) ? "ImpactCometB" : "ImpactCometA");
		}
		else
		{
			base.animator.Play((!Rand.Bool()) ? "ImpactSmallB" : "ImpactSmallA");
		}
	}

	// Token: 0x06003FF9 RID: 16377 RVA: 0x0022E649 File Offset: 0x0022CA49
	private void _OnDieAnimComplete()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003FFA RID: 16378 RVA: 0x0022E658 File Offset: 0x0022CA58
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!this.cracked && base.distance > WeaponProperties.LevelWeaponCrackshot.Basic.crackDistance && !base.dead)
		{
			this.cracked = true;
			this.crackFX.Create(base.transform.position);
			base.animator.SetBool("IsB", this.useBComet);
			base.animator.Play((!Rand.Bool()) ? "CometStartA" : "CometStartB");
			AudioManager.Play("player_weapon_crackshot_shootfast");
			this.emitAudioFromObject.Add("player_weapon_crackshot_shootfast");
			this.damageDealer.SetDamage(WeaponProperties.LevelWeaponCrackshot.Basic.crackedDamage);
			this.Speed = WeaponProperties.LevelWeaponCrackshot.Basic.crackedSpeed;
			this.FindTarget();
			if (this.target != null)
			{
				if (Vector3.Angle(base.transform.right, this.target.bounds.center - base.transform.position) > this.maxAngleRange)
				{
					if (Mathf.Abs(base.transform.right.y) < 0.05f)
					{
						base.transform.eulerAngles = new Vector3(0f, 0f, (base.transform.eulerAngles.z <= 90f) ? (MathUtils.DirectionToAngle(base.transform.right) + this.maxAngleRange) : (MathUtils.DirectionToAngle(base.transform.right) - this.maxAngleRange));
					}
					else
					{
						base.transform.eulerAngles = new Vector3(0f, 0f, MathUtils.DirectionToAngle(base.transform.right) + this.maxAngleRange);
					}
				}
				else
				{
					base.transform.eulerAngles = new Vector3(0f, 0f, MathUtils.DirectionToAngle(this.target.bounds.center - base.transform.position));
				}
			}
		}
	}

	// Token: 0x06003FFB RID: 16379 RVA: 0x0022E890 File Offset: 0x0022CC90
	public void FindTarget()
	{
		this.target = this.findBestTarget(AbstractProjectile.FindOverlapScreenDamageReceivers());
	}

	// Token: 0x06003FFC RID: 16380 RVA: 0x0022E8A4 File Offset: 0x0022CCA4
	private Collider2D findBestTarget(IEnumerable<DamageReceiver> damageReceivers)
	{
		float num = float.MaxValue;
		float num2 = float.MaxValue;
		Collider2D collider2D = null;
		Collider2D collider2D2 = null;
		Vector2 vector = base.transform.position;
		foreach (DamageReceiver damageReceiver in damageReceivers)
		{
			if (damageReceiver.gameObject.activeInHierarchy && damageReceiver.enabled && damageReceiver.type == DamageReceiver.Type.Enemy)
			{
				foreach (Collider2D collider2D3 in damageReceiver.GetComponents<Collider2D>())
				{
					if (collider2D3.isActiveAndEnabled && CupheadLevelCamera.Current.ContainsPoint(collider2D3.bounds.center, collider2D3.bounds.size / 2f))
					{
						float sqrMagnitude = (vector - collider2D3.bounds.center).sqrMagnitude;
						if (sqrMagnitude < num2)
						{
							num2 = sqrMagnitude;
							collider2D2 = collider2D3;
						}
						if (sqrMagnitude < num && Vector3.Angle(base.transform.right, collider2D3.bounds.center - vector) < this.maxAngleRange)
						{
							num = sqrMagnitude;
							collider2D = collider2D3;
						}
					}
				}
				foreach (DamageReceiverChild damageReceiverChild in damageReceiver.GetComponentsInChildren<DamageReceiverChild>())
				{
					foreach (Collider2D collider2D4 in damageReceiverChild.GetComponents<Collider2D>())
					{
						if (collider2D4.isActiveAndEnabled && CupheadLevelCamera.Current.ContainsPoint(collider2D4.bounds.center, collider2D4.bounds.size / 2f))
						{
							float sqrMagnitude2 = (vector - collider2D4.bounds.center).sqrMagnitude;
							if (sqrMagnitude2 < num2)
							{
								num2 = sqrMagnitude2;
								collider2D2 = collider2D4;
							}
							if (sqrMagnitude2 < num && Vector3.Angle(base.transform.right, collider2D4.bounds.center - vector) < this.maxAngleRange)
							{
								num = sqrMagnitude2;
								collider2D = collider2D4;
							}
						}
					}
				}
			}
		}
		return (!(collider2D == null)) ? collider2D : collider2D2;
	}

	// Token: 0x040046C6 RID: 18118
	private Collider2D target;

	// Token: 0x040046C7 RID: 18119
	private bool cracked;

	// Token: 0x040046C8 RID: 18120
	public float maxAngleRange;

	// Token: 0x040046C9 RID: 18121
	public int variant;

	// Token: 0x040046CA RID: 18122
	public bool useBComet;

	// Token: 0x040046CB RID: 18123
	[SerializeField]
	private Collider2D coll;

	// Token: 0x040046CC RID: 18124
	[SerializeField]
	private Effect crackFX;
}
