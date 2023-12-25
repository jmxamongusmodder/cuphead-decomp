using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A6E RID: 2670
public class WeaponBouncerProjectile : AbstractProjectile
{
	// Token: 0x17000580 RID: 1408
	// (get) Token: 0x06003FBD RID: 16317 RVA: 0x0022CA41 File Offset: 0x0022AE41
	protected override float DestroyLifetime
	{
		get
		{
			return 1000f;
		}
	}

	// Token: 0x06003FBE RID: 16318 RVA: 0x0022CA48 File Offset: 0x0022AE48
	protected override void Start()
	{
		base.Start();
		if (this.isEx)
		{
			this.damageDealer.SetDamageSource(DamageDealer.DamageSource.Ex);
			base.StartCoroutine(this.trail_cr());
		}
		else
		{
			switch (UnityEngine.Random.Range(0, 4))
			{
			case 0:
				base.animator.Play("A", 0, UnityEngine.Random.Range(0f, 1f));
				break;
			case 1:
				base.animator.Play("B", 0, UnityEngine.Random.Range(0f, 1f));
				break;
			case 2:
				base.animator.Play("C", 0, UnityEngine.Random.Range(0f, 1f));
				break;
			case 3:
				base.animator.Play("D", 0, UnityEngine.Random.Range(0f, 1f));
				break;
			}
		}
	}

	// Token: 0x06003FBF RID: 16319 RVA: 0x0022CB3C File Offset: 0x0022AF3C
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.firstUpdateNew)
		{
			this.firstUpdateNew = false;
			if (this.velocity.y < 0f)
			{
				return;
			}
		}
		if (base.dead)
		{
			return;
		}
		this.UpdateInAir();
		if (!this.isEx && !CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(150f, 100f)) && !CupheadLevelCamera.Current.ContainsPoint(new Vector3(base.transform.position.x, base.transform.position.y - 300f, 0f), new Vector2(150f, 100f)))
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06003FC0 RID: 16320 RVA: 0x0022CC28 File Offset: 0x0022B028
	private void UpdateInAir()
	{
		if (this.timeUntilUnfreeze > 0f)
		{
			this.timeUntilUnfreeze -= CupheadTime.FixedDelta;
			base.transform.position += new Vector3(this.velocity.x * CupheadTime.FixedDelta, 0f, 0f);
		}
		else
		{
			this.velocity.y = this.velocity.y - this.gravity * CupheadTime.FixedDelta;
			base.transform.position += this.velocity * CupheadTime.FixedDelta;
		}
	}

	// Token: 0x06003FC1 RID: 16321 RVA: 0x0022CCDC File Offset: 0x0022B0DC
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		LevelPlatform component = hit.GetComponent<LevelPlatform>();
		if ((component == null || !component.canFallThrough) && this.velocity.y < 0f)
		{
			this.HitGround(hit);
		}
	}

	// Token: 0x06003FC2 RID: 16322 RVA: 0x0022CD2C File Offset: 0x0022B12C
	protected override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionOther(hit, phase);
		LevelPlatform component = hit.GetComponent<LevelPlatform>();
		if (component != null && !component.canFallThrough && this.velocity.y < 0f)
		{
			this.HitGround(hit);
		}
	}

	// Token: 0x06003FC3 RID: 16323 RVA: 0x0022CD7B File Offset: 0x0022B17B
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		if (!this.isEx)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x06003FC4 RID: 16324 RVA: 0x0022CDA0 File Offset: 0x0022B1A0
	private void HitGround(GameObject hit)
	{
		float num = this.velocity.magnitude * WeaponProperties.LevelWeaponBouncer.Basic.bounceRatio - WeaponProperties.LevelWeaponBouncer.Basic.bounceSpeedDampening;
		if (num <= 0f || this.numBounces >= WeaponProperties.LevelWeaponBouncer.Basic.numBounces || this.isEx)
		{
			this.Die();
		}
		else
		{
			this.velocity = this.velocity.normalized * num;
			this.velocity.y = this.velocity.y * -1f;
			this.numBounces++;
			this.timeUntilUnfreeze = 0.041666668f;
			base.animator.SetTrigger((!Rand.Bool()) ? "Bounce_B" : "Bounce_A");
		}
	}

	// Token: 0x06003FC5 RID: 16325 RVA: 0x0022CE64 File Offset: 0x0022B264
	private IEnumerator trail_cr()
	{
		while (!base.dead)
		{
			yield return CupheadTime.WaitForSeconds(this, this.trailDelay);
			if (base.dead)
			{
				yield break;
			}
			this.trailFxPrefab.Create(base.transform.position + MathUtils.RandomPointInUnitCircle() * this.trailFxMaxOffset);
		}
		yield break;
	}

	// Token: 0x06003FC6 RID: 16326 RVA: 0x0022CE80 File Offset: 0x0022B280
	protected override void Die()
	{
		base.Die();
		if (this.isEx)
		{
			WeaponArcProjectileExplosion weaponArcProjectileExplosion = this.exExplosion.Create(base.transform.position, this.Damage, base.DamageMultiplier, this.PlayerId);
			weaponArcProjectileExplosion.DamageDealer.SetDamageSource(DamageDealer.DamageSource.Ex);
			MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
			meterScoreTracker.Add(weaponArcProjectileExplosion.DamageDealer);
			AudioManager.Play("player_weapon_bouncer_ex_explosion");
			this.emitAudioFromObject.Add("player_weapon_bouncer_ex_explosion");
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			base.transform.SetEulerAngles(null, null, new float?((float)UnityEngine.Random.Range(0, 360)));
		}
	}

	// Token: 0x0400469B RID: 18075
	[SerializeField]
	private bool isEx;

	// Token: 0x0400469C RID: 18076
	[SerializeField]
	private WeaponArcProjectileExplosion exExplosion;

	// Token: 0x0400469D RID: 18077
	[SerializeField]
	private Effect trailFxPrefab;

	// Token: 0x0400469E RID: 18078
	[SerializeField]
	private float trailFxMaxOffset;

	// Token: 0x0400469F RID: 18079
	[SerializeField]
	private float trailDelay;

	// Token: 0x040046A0 RID: 18080
	public float gravity;

	// Token: 0x040046A1 RID: 18081
	public Vector2 velocity;

	// Token: 0x040046A2 RID: 18082
	public WeaponBouncer weapon;

	// Token: 0x040046A3 RID: 18083
	public float bounceRatio;

	// Token: 0x040046A4 RID: 18084
	public float bounceSpeedDampening;

	// Token: 0x040046A5 RID: 18085
	private float timeUntilUnfreeze;

	// Token: 0x040046A6 RID: 18086
	private const float bounceFreezeTime = 0.041666668f;

	// Token: 0x040046A7 RID: 18087
	private int numBounces;

	// Token: 0x040046A8 RID: 18088
	private bool firstUpdateNew = true;
}
