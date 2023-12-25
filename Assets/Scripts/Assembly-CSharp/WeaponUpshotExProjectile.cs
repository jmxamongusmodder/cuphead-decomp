using System;
using UnityEngine;

// Token: 0x02000A8A RID: 2698
public class WeaponUpshotExProjectile : AbstractProjectile
{
	// Token: 0x0600407D RID: 16509 RVA: 0x00231B61 File Offset: 0x0022FF61
	protected override void OnDieDistance()
	{
	}

	// Token: 0x17000598 RID: 1432
	// (get) Token: 0x0600407E RID: 16510 RVA: 0x00231B63 File Offset: 0x0022FF63
	protected override float DestroyLifetime
	{
		get
		{
			return 4f;
		}
	}

	// Token: 0x17000599 RID: 1433
	// (get) Token: 0x0600407F RID: 16511 RVA: 0x00231B6A File Offset: 0x0022FF6A
	protected override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06004080 RID: 16512 RVA: 0x00231B70 File Offset: 0x0022FF70
	protected override void Start()
	{
		base.Start();
		this.damageDealer.SetDamage(WeaponProperties.LevelWeaponUpshot.Ex.damage);
		this.damageDealer.SetRate(WeaponProperties.LevelWeaponUpshot.Ex.damageRate);
		this.damageDealer.isDLCWeapon = true;
		this.angle = MathUtils.DirectionToAngle(base.transform.right);
		base.transform.position += base.transform.right * 120f;
		base.transform.localScale = new Vector3((float)((base.transform.eulerAngles.z <= 90f || base.transform.eulerAngles.z >= 270f) ? 1 : -1), 1f);
		this.endScale = base.transform.localScale;
		base.transform.localScale *= 0.5f;
		this.startScale = base.transform.localScale;
		base.transform.eulerAngles = Vector3.zero;
		this.startPos = base.transform.position;
		base.animator.Play("EX", 0, UnityEngine.Random.Range(0f, 1f));
		this.trailPositions = new Vector2[6];
		for (int i = 0; i < this.trailPositions.Length; i++)
		{
			this.trailPositions[i] = base.transform.position;
		}
	}

	// Token: 0x06004081 RID: 16513 RVA: 0x00231D10 File Offset: 0x00230110
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
			return;
		}
		this.time += CupheadTime.FixedDelta;
		this.angle += Mathf.Lerp(WeaponProperties.LevelWeaponUpshot.Ex.minRotationSpeed, WeaponProperties.LevelWeaponUpshot.Ex.maxRotationSpeed, this.time / WeaponProperties.LevelWeaponUpshot.Ex.rotationRampTime) * CupheadTime.FixedDelta * this.rotateDir;
		this.radius += Mathf.Lerp(WeaponProperties.LevelWeaponUpshot.Ex.minRadiusSpeed, WeaponProperties.LevelWeaponUpshot.Ex.maxRadiusSpeed, this.time / WeaponProperties.LevelWeaponUpshot.Ex.radiusRampTime) * CupheadTime.FixedDelta;
		base.transform.position = this.startPos + MathUtils.AngleToDirection(this.angle) * this.radius;
		float num = Mathf.Round(this.time * 24f) / 24f;
		base.transform.localScale = Vector3.Lerp(this.startScale, this.endScale, num * 5f);
		num *= 0.2f;
		this.trail1.color = new Color(1f, 1f, 1f, 0.5f - num);
		this.trail2.color = new Color(1f, 1f, 1f, 0.25f - num);
		this.UpdateTrails();
	}

	// Token: 0x06004082 RID: 16514 RVA: 0x00231E90 File Offset: 0x00230290
	private void UpdateTrails()
	{
		int num = this.currentPositionIndex - 2;
		if (num < 0)
		{
			num += this.trailPositions.Length;
		}
		int num2 = this.currentPositionIndex - 5;
		if (num2 < 0)
		{
			num2 += this.trailPositions.Length;
		}
		this.trail1.transform.position = this.trailPositions[num];
		this.trail2.transform.position = this.trailPositions[num2];
		this.currentPositionIndex = (this.currentPositionIndex + 1) % this.trailPositions.Length;
		this.trailPositions[this.currentPositionIndex] = base.transform.position;
	}

	// Token: 0x06004083 RID: 16515 RVA: 0x00231F5C File Offset: 0x0023035C
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		float num = this.damageDealer.DealDamage(hit);
		this.totalDamage += num;
		if (this.totalDamage > WeaponProperties.LevelWeaponUpshot.Ex.maxDamage)
		{
			this.Die();
		}
		if (num > 0f)
		{
			this.hitFXPrefab.Create(Vector3.Lerp(base.transform.position, hit.transform.position, 0.5f) + UnityEngine.Random.insideUnitCircle * 20f);
			AudioManager.Play("player_ex_impact_hit");
			this.emitAudioFromObject.Add("player_ex_impact_hit");
			this.timeUntilUnfreeze = WeaponProperties.LevelWeaponUpshot.Ex.freezeTime;
		}
	}

	// Token: 0x06004084 RID: 16516 RVA: 0x00232017 File Offset: 0x00230417
	protected override void Die()
	{
		base.Die();
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.Play("Die");
	}

	// Token: 0x0400473B RID: 18235
	private float timeUntilUnfreeze;

	// Token: 0x0400473C RID: 18236
	private float totalDamage;

	// Token: 0x0400473D RID: 18237
	private float angle;

	// Token: 0x0400473E RID: 18238
	private float time;

	// Token: 0x0400473F RID: 18239
	private float radius;

	// Token: 0x04004740 RID: 18240
	private Vector3 startPos;

	// Token: 0x04004741 RID: 18241
	public float rotateDir;

	// Token: 0x04004742 RID: 18242
	private Vector3 startScale;

	// Token: 0x04004743 RID: 18243
	private Vector3 endScale;

	// Token: 0x04004744 RID: 18244
	private Vector2[] trailPositions;

	// Token: 0x04004745 RID: 18245
	private int currentPositionIndex;

	// Token: 0x04004746 RID: 18246
	[SerializeField]
	private SpriteRenderer trail1;

	// Token: 0x04004747 RID: 18247
	[SerializeField]
	private SpriteRenderer trail2;

	// Token: 0x04004748 RID: 18248
	private const int trailFrameDelay = 3;

	// Token: 0x04004749 RID: 18249
	[SerializeField]
	private Effect hitFXPrefab;
}
