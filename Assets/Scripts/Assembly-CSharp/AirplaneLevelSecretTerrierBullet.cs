using System;
using UnityEngine;

// Token: 0x020004C4 RID: 1220
public class AirplaneLevelSecretTerrierBullet : AbstractProjectile
{
	// Token: 0x06001480 RID: 5248 RVA: 0x000B7DE4 File Offset: 0x000B61E4
	public AirplaneLevelSecretTerrierBullet Create(Vector3 pos, Vector3 targetPos, LevelProperties.Airplane.SecretTerriers props, Vector3 scale)
	{
		AirplaneLevelSecretTerrierBullet airplaneLevelSecretTerrierBullet = base.Create() as AirplaneLevelSecretTerrierBullet;
		airplaneLevelSecretTerrierBullet.speed = props.dogBulletArcSpeed;
		airplaneLevelSecretTerrierBullet.arcHeight = props.dogBulletArcHeight;
		airplaneLevelSecretTerrierBullet.splitAngle = props.dogBulletSplitAngle;
		airplaneLevelSecretTerrierBullet.splitSpeed = props.dogBulletSplitSpeed;
		airplaneLevelSecretTerrierBullet.willSplit = props.dogBulletWillSplit;
		airplaneLevelSecretTerrierBullet.hp = props.dogBulletHealth;
		airplaneLevelSecretTerrierBullet.transform.position = pos;
		airplaneLevelSecretTerrierBullet.posTimer = 0f;
		airplaneLevelSecretTerrierBullet.startPos = pos;
		airplaneLevelSecretTerrierBullet.destPos = targetPos;
		airplaneLevelSecretTerrierBullet.lastPos = airplaneLevelSecretTerrierBullet.startPos;
		airplaneLevelSecretTerrierBullet.transform.localScale = scale;
		airplaneLevelSecretTerrierBullet.damageReceiver = airplaneLevelSecretTerrierBullet.GetComponent<DamageReceiver>();
		airplaneLevelSecretTerrierBullet.damageReceiver.OnDamageTaken += airplaneLevelSecretTerrierBullet.OnDamageTaken;
		return airplaneLevelSecretTerrierBullet;
	}

	// Token: 0x06001481 RID: 5249 RVA: 0x000B7EA7 File Offset: 0x000B62A7
	public override void SetParryable(bool parryable)
	{
		base.SetParryable(parryable);
	}

	// Token: 0x06001482 RID: 5250 RVA: 0x000B7EB0 File Offset: 0x000B62B0
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.exploded)
		{
			return;
		}
		if (this.posTimer < 1f)
		{
			this.lastPos = base.transform.position;
			base.transform.position = Vector3.Lerp(this.startPos, this.destPos, this.posTimer) + Vector3.up * Mathf.Sin(this.posTimer * 3.1415927f) * this.arcHeight;
			this.posTimer += this.speed * CupheadTime.FixedDelta;
		}
		else
		{
			base.transform.position += this.destPos - this.lastPos;
			this.lastPos += Vector3.up * this.arcHeight * CupheadTime.FixedDelta * 0.25f;
		}
	}

	// Token: 0x06001483 RID: 5251 RVA: 0x000B7FB7 File Offset: 0x000B63B7
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001484 RID: 5252 RVA: 0x000B7FD8 File Offset: 0x000B63D8
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.exploded)
		{
			return;
		}
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			this.exploded = true;
			this.coll.enabled = false;
			base.animator.Play((!Rand.Bool()) ? "ExplodeB" : "ExplodeA");
			AudioManager.Play("sfx_dlc_dogfight_ps_terrier_pineappleexplode");
		}
	}

	// Token: 0x06001485 RID: 5253 RVA: 0x000B8058 File Offset: 0x000B6458
	private void AniEvent_SpawnShrapnel()
	{
		this.splitBulletPrefab.Create(base.transform.position, MathUtils.DirectionToAngle(Vector3.right), this.splitSpeed);
		this.splitBulletPrefab.Create(base.transform.position, MathUtils.DirectionToAngle(Vector3.right) - this.splitAngle, this.splitSpeed);
		this.splitBulletPrefab.Create(base.transform.position, MathUtils.DirectionToAngle(Vector3.right) + this.splitAngle, this.splitSpeed);
	}

	// Token: 0x06001486 RID: 5254 RVA: 0x000B8106 File Offset: 0x000B6506
	private void AniEvent_EndExplosion()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06001487 RID: 5255 RVA: 0x000B8113 File Offset: 0x000B6513
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			this.damageDealer.DealDamage(hit);
		}
		AudioManager.Play("sfx_dlc_dogfight_ps_terrier_pineapplehitplayer");
	}

	// Token: 0x04001DD1 RID: 7633
	private float speed;

	// Token: 0x04001DD2 RID: 7634
	private float arcHeight;

	// Token: 0x04001DD3 RID: 7635
	private float splitAngle;

	// Token: 0x04001DD4 RID: 7636
	private float splitSpeed;

	// Token: 0x04001DD5 RID: 7637
	private float hp;

	// Token: 0x04001DD6 RID: 7638
	private bool willSplit;

	// Token: 0x04001DD7 RID: 7639
	private Vector3 startPos;

	// Token: 0x04001DD8 RID: 7640
	private Vector3 destPos;

	// Token: 0x04001DD9 RID: 7641
	private Vector3 lastPos;

	// Token: 0x04001DDA RID: 7642
	private float posTimer;

	// Token: 0x04001DDB RID: 7643
	private bool exploded;

	// Token: 0x04001DDC RID: 7644
	[SerializeField]
	private CircleCollider2D coll;

	// Token: 0x04001DDD RID: 7645
	[SerializeField]
	private BasicProjectile splitBulletPrefab;

	// Token: 0x04001DDE RID: 7646
	private DamageReceiver damageReceiver;
}
