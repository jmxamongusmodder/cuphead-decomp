using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000AB6 RID: 2742
public class PlaneWeaponBombExProjectile : AbstractProjectile
{
	// Token: 0x170005C3 RID: 1475
	// (get) Token: 0x060041DC RID: 16860 RVA: 0x002398E8 File Offset: 0x00237CE8
	protected override float DestroyLifetime
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x060041DD RID: 16861 RVA: 0x002398F0 File Offset: 0x00237CF0
	public void Init()
	{
		this.Cuphead.enabled = ((this.PlayerId == PlayerId.PlayerOne && !PlayerManager.player1IsMugman) || (this.PlayerId == PlayerId.PlayerTwo && PlayerManager.player1IsMugman));
		this.Mugman.enabled = ((this.PlayerId == PlayerId.PlayerOne && PlayerManager.player1IsMugman) || (this.PlayerId == PlayerId.PlayerTwo && !PlayerManager.player1IsMugman));
		base.StartCoroutine(this.trail_cr());
	}

	// Token: 0x060041DE RID: 16862 RVA: 0x0023997B File Offset: 0x00237D7B
	protected override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		if (hit.tag == "Parry")
		{
			return;
		}
		base.OnCollisionOther(hit, phase);
	}

	// Token: 0x060041DF RID: 16863 RVA: 0x0023999B File Offset: 0x00237D9B
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		this.DealDamage(hit);
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x060041E0 RID: 16864 RVA: 0x002399AC File Offset: 0x00237DAC
	private void DealDamage(GameObject hit)
	{
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x060041E1 RID: 16865 RVA: 0x002399BB File Offset: 0x00237DBB
	protected override void Die()
	{
		this.move = false;
		base.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f));
		base.Die();
	}

	// Token: 0x060041E2 RID: 16866 RVA: 0x002399F4 File Offset: 0x00237DF4
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!this.move)
		{
			return;
		}
		this.t += CupheadTime.FixedDelta;
		if (this.target != null && this.target.gameObject.activeInHierarchy && this.target.isActiveAndEnabled && this.t < WeaponProperties.LevelWeaponHoming.Basic.maxHomingTime)
		{
			float num;
			for (num = MathUtils.DirectionToAngle(this.target.bounds.center - base.transform.position); num > this.rotation + 180f; num -= 360f)
			{
			}
			while (num < this.rotation - 180f)
			{
				num += 360f;
			}
			float num2 = this.rotationSpeed.min;
			if (this.t > this.timeBeforeEaseRotationSpeed + this.rotationSpeedEaseTime)
			{
				num2 = this.rotationSpeed.max;
			}
			else if (this.t > this.timeBeforeEaseRotationSpeed)
			{
				num2 = this.rotationSpeed.GetFloatAt((this.t - this.timeBeforeEaseRotationSpeed) / this.rotationSpeedEaseTime);
			}
			if (Mathf.Abs(num - this.rotation) < num2 * CupheadTime.FixedDelta)
			{
				this.rotation = num;
			}
			else if (num > this.rotation)
			{
				this.rotation += num2 * CupheadTime.FixedDelta;
			}
			else
			{
				this.rotation -= num2 * CupheadTime.FixedDelta;
			}
		}
		Vector3 a = MathUtils.AngleToDirection(this.rotation);
		base.transform.position += a * this.speed * CupheadTime.FixedDelta;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.rotation + this.spriteRotation));
		if (!CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(this.destroyPadding, this.destroyPadding)))
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060041E3 RID: 16867 RVA: 0x00239C44 File Offset: 0x00238044
	public void FindTarget()
	{
		float num = float.MaxValue;
		Collider2D collider2D = null;
		Vector2 a = base.transform.position + this.speed * (this.timeBeforeEaseRotationSpeed + this.rotationSpeedEaseTime * 0.75f) * MathUtils.AngleToDirection(this.rotation);
		foreach (DamageReceiver damageReceiver in UnityEngine.Object.FindObjectsOfType<DamageReceiver>())
		{
			if (damageReceiver.gameObject.activeInHierarchy && damageReceiver.type == DamageReceiver.Type.Enemy)
			{
				foreach (Collider2D collider2D2 in damageReceiver.GetComponents<Collider2D>())
				{
					if (collider2D2.isActiveAndEnabled && CupheadLevelCamera.Current.ContainsPoint(collider2D2.bounds.center, collider2D2.bounds.size / 2f))
					{
						float sqrMagnitude = (a - collider2D2.bounds.center).sqrMagnitude;
						if (sqrMagnitude < num)
						{
							num = sqrMagnitude;
							collider2D = collider2D2;
						}
					}
				}
				foreach (DamageReceiverChild damageReceiverChild in damageReceiver.GetComponentsInChildren<DamageReceiverChild>())
				{
					foreach (Collider2D collider2D3 in damageReceiverChild.GetComponents<Collider2D>())
					{
						if (collider2D3.isActiveAndEnabled && CupheadLevelCamera.Current.ContainsPoint(collider2D3.bounds.center, collider2D3.bounds.size / 2f))
						{
							float sqrMagnitude2 = (a - collider2D3.bounds.center).sqrMagnitude;
							if (sqrMagnitude2 < num)
							{
								num = sqrMagnitude2;
								collider2D = collider2D3;
							}
						}
					}
				}
			}
		}
		this.target = collider2D;
	}

	// Token: 0x060041E4 RID: 16868 RVA: 0x00239E68 File Offset: 0x00238268
	private IEnumerator trail_cr()
	{
		while (!base.dead)
		{
			yield return CupheadTime.WaitForSeconds(this, this.trailDelay);
			if (base.dead)
			{
				yield break;
			}
			this.trailFxPrefab.Create(this.trailFxRoot.position + MathUtils.RandomPointInUnitCircle() * this.trailFxMaxOffset);
		}
		yield break;
	}

	// Token: 0x060041E5 RID: 16869 RVA: 0x00239E83 File Offset: 0x00238283
	public override void OnLevelEnd()
	{
	}

	// Token: 0x0400483C RID: 18492
	[SerializeField]
	private float spriteRotation;

	// Token: 0x0400483D RID: 18493
	[SerializeField]
	private Effect trailFxPrefab;

	// Token: 0x0400483E RID: 18494
	[SerializeField]
	private Transform trailFxRoot;

	// Token: 0x0400483F RID: 18495
	[SerializeField]
	private float trailFxMaxOffset;

	// Token: 0x04004840 RID: 18496
	[SerializeField]
	private float trailDelay;

	// Token: 0x04004841 RID: 18497
	[SerializeField]
	private float destroyPadding;

	// Token: 0x04004842 RID: 18498
	[SerializeField]
	private SpriteRenderer Cuphead;

	// Token: 0x04004843 RID: 18499
	[SerializeField]
	private SpriteRenderer Mugman;

	// Token: 0x04004844 RID: 18500
	public float speed;

	// Token: 0x04004845 RID: 18501
	public MinMax rotationSpeed;

	// Token: 0x04004846 RID: 18502
	public float timeBeforeEaseRotationSpeed;

	// Token: 0x04004847 RID: 18503
	public float rotationSpeedEaseTime;

	// Token: 0x04004848 RID: 18504
	public float rotation;

	// Token: 0x04004849 RID: 18505
	private Vector2 velocity;

	// Token: 0x0400484A RID: 18506
	private float t;

	// Token: 0x0400484B RID: 18507
	private bool move = true;

	// Token: 0x0400484C RID: 18508
	private Collider2D target;

	// Token: 0x0400484D RID: 18509
	private AbstractPlayerController player;
}
