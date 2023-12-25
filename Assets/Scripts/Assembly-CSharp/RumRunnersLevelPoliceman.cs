using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200079A RID: 1946
public class RumRunnersLevelPoliceman : AbstractCollidableObject
{
	// Token: 0x170003FC RID: 1020
	// (get) Token: 0x06002B3C RID: 11068 RVA: 0x00192D69 File Offset: 0x00191169
	// (set) Token: 0x06002B3D RID: 11069 RVA: 0x00192D71 File Offset: 0x00191171
	public bool isActive { get; set; }

	// Token: 0x06002B3E RID: 11070 RVA: 0x00192D7C File Offset: 0x0019117C
	private void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.scaleX = base.transform.localScale.x;
		this.collider = base.GetComponent<Collider2D>();
		this.collider.enabled = false;
	}

	// Token: 0x06002B3F RID: 11071 RVA: 0x00192DE8 File Offset: 0x001911E8
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002B40 RID: 11072 RVA: 0x00192E00 File Offset: 0x00191200
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002B41 RID: 11073 RVA: 0x00192E20 File Offset: 0x00191220
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp <= 0f && this.deathCoroutine == null)
		{
			Level.Current.RegisterMinionKilled();
			this.StopAllCoroutines();
			this.deathCoroutine = base.StartCoroutine(this.death_cr());
		}
	}

	// Token: 0x06002B42 RID: 11074 RVA: 0x00192E7D File Offset: 0x0019127D
	public void SetProperties(LevelProperties.RumRunners.Spider properties, RumRunnersLevelSpider spider)
	{
		this.properties = properties;
		this.spider = spider;
	}

	// Token: 0x06002B43 RID: 11075 RVA: 0x00192E90 File Offset: 0x00191290
	public void CopAppear(Vector3 appearPos, bool isPink, bool goingLeft)
	{
		if (this.deathCoroutine != null)
		{
			return;
		}
		Vector3 b = this.spawnPositionOffset;
		b.x *= (float)((!goingLeft) ? 1 : -1);
		base.transform.position = appearPos + b;
		this.isPink = isPink;
		this.hp = this.properties.copHealth;
		base.StartCoroutine(this.shooting_cr());
		base.transform.SetScale(new float?((!goingLeft) ? this.scaleX : (-this.scaleX)), null, null);
		this.isActive = true;
	}

	// Token: 0x06002B44 RID: 11076 RVA: 0x00192F4C File Offset: 0x0019134C
	private IEnumerator shooting_cr()
	{
		this.collider.enabled = true;
		this.gunSmokeRenderer.enabled = !this.isPink;
		this.gunSmokeParryRenderer.enabled = this.isPink;
		this.lastShootDirection = this.calculateDirection();
		string animatorParameter;
		string stateBaseName;
		if (this.lastShootDirection == RumRunnersLevelPoliceman.Direction.Down)
		{
			animatorParameter = "ShootingDown";
			stateBaseName = "ShootDown";
			this.currentBulletOrigin = this.bulletOriginDown;
		}
		else if (this.lastShootDirection == RumRunnersLevelPoliceman.Direction.Up)
		{
			animatorParameter = "ShootingUp";
			stateBaseName = "ShootUp";
			this.currentBulletOrigin = this.bulletOriginUp;
		}
		else
		{
			animatorParameter = "Shooting";
			stateBaseName = "ShootStraight";
			this.currentBulletOrigin = this.bulletOriginStraight;
		}
		Coroutine alignmentCoroutine = base.StartCoroutine(this.align_cr());
		base.animator.SetBool(animatorParameter, true);
		yield return base.animator.WaitForAnimationToStart(this, stateBaseName + "Hold", false);
		yield return CupheadTime.WaitForSeconds(this, this.properties.copAttackWarning);
		base.animator.SetTrigger("Shoot");
		yield return base.animator.WaitForAnimationToStart(this, stateBaseName + "ExitHold", false);
		yield return CupheadTime.WaitForSeconds(this, this.properties.copExitDelay);
		base.animator.SetTrigger("ShootExit");
		yield return base.animator.WaitForAnimationToStart(this, "ShootExit", false);
		yield return null;
		while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
		{
			yield return null;
		}
		this.collider.enabled = false;
		base.transform.position = new Vector3(0f, 1000f);
		base.animator.SetBool(animatorParameter, false);
		this.isActive = false;
		base.StopCoroutine(alignmentCoroutine);
		yield break;
	}

	// Token: 0x06002B45 RID: 11077 RVA: 0x00192F68 File Offset: 0x00191368
	private IEnumerator align_cr()
	{
		YieldInstruction waitInstruction = new WaitForFixedUpdate();
		for (;;)
		{
			yield return waitInstruction;
			base.transform.SetPosition(null, new float?(RumRunnersLevel.GroundWalkingPosY(base.transform.position, this.collider, 0f, 200f)), null);
		}
		yield break;
	}

	// Token: 0x06002B46 RID: 11078 RVA: 0x00192F84 File Offset: 0x00191384
	private IEnumerator death_cr()
	{
		this.SFX_RUMRUN_Police_DiePoof();
		Vector3 puffPosition = this.collider.bounds.center;
		base.animator.SetBool("Shooting", false);
		base.animator.SetBool("ShootingUp", false);
		base.animator.SetBool("ShootingDown", false);
		this.isActive = false;
		this.collider.enabled = false;
		base.transform.position = puffPosition;
		base.animator.SetBool("Die", true);
		yield return base.animator.WaitForAnimationToStart(this, "Death", false);
		while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
		{
			yield return null;
		}
		base.transform.position = new Vector3(0f, 1000f);
		base.animator.SetBool("Die", false);
		this.deathCoroutine = null;
		yield break;
	}

	// Token: 0x06002B47 RID: 11079 RVA: 0x00192FA0 File Offset: 0x001913A0
	private void animationEvent_SpawnBullet()
	{
		Vector3 v = this.spider.transform.position - base.transform.position;
		RumRunnersLevelPoliceBullet rumRunnersLevelPoliceBullet = (RumRunnersLevelPoliceBullet)this.regularBullet.Create(this.currentBulletOrigin.transform.position, MathUtils.DirectionToAngle(v), this.properties.copBulletSpeed);
		rumRunnersLevelPoliceBullet.spiderDamage = this.properties.copBulletBossDamage;
		rumRunnersLevelPoliceBullet.direction = this.lastShootDirection;
		rumRunnersLevelPoliceBullet.SetParryable(this.isPink);
		rumRunnersLevelPoliceBullet.GetComponent<SpriteRenderer>().flipY = (Mathf.Sign(v.x) < 0f);
	}

	// Token: 0x06002B48 RID: 11080 RVA: 0x00193051 File Offset: 0x00191451
	private void animationEvent_ExitDisappeared()
	{
		this.collider.enabled = false;
	}

	// Token: 0x06002B49 RID: 11081 RVA: 0x00193060 File Offset: 0x00191460
	private RumRunnersLevelPoliceman.Direction calculateDirection()
	{
		if (base.transform.position.y - this.spider.transform.position.y > RumRunnersLevelPoliceman.SpiderYDistanceThreshold)
		{
			return RumRunnersLevelPoliceman.Direction.Down;
		}
		if (this.spider.transform.position.y - base.transform.position.y > RumRunnersLevelPoliceman.SpiderYDistanceThreshold)
		{
			return RumRunnersLevelPoliceman.Direction.Up;
		}
		return RumRunnersLevelPoliceman.Direction.Straight;
	}

	// Token: 0x06002B4A RID: 11082 RVA: 0x001930DE File Offset: 0x001914DE
	private void AnimationEvent_SFX_RUMRUN_Police_GunShoot()
	{
		AudioManager.Play("sfx_dlc_rumrun_policegun_shoot");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_policegun_shoot");
	}

	// Token: 0x06002B4B RID: 11083 RVA: 0x001930FA File Offset: 0x001914FA
	private void SFX_RUMRUN_Police_DiePoof()
	{
		AudioManager.Play("sfx_dlc_rumrun_lackey_poof");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_lackey_poof");
		AudioManager.Stop("sfx_dlc_rumrun_policegun_shoot");
	}

	// Token: 0x040033EB RID: 13291
	private static readonly float SpiderYDistanceThreshold = 100f;

	// Token: 0x040033EC RID: 13292
	[SerializeField]
	private RumRunnersLevelPoliceBullet regularBullet;

	// Token: 0x040033ED RID: 13293
	[SerializeField]
	private Transform bulletOriginStraight;

	// Token: 0x040033EE RID: 13294
	[SerializeField]
	private Transform bulletOriginUp;

	// Token: 0x040033EF RID: 13295
	[SerializeField]
	private Transform bulletOriginDown;

	// Token: 0x040033F0 RID: 13296
	[SerializeField]
	private Vector2 spawnPositionOffset;

	// Token: 0x040033F1 RID: 13297
	[SerializeField]
	private SpriteRenderer gunSmokeRenderer;

	// Token: 0x040033F2 RID: 13298
	[SerializeField]
	private SpriteRenderer gunSmokeParryRenderer;

	// Token: 0x040033F3 RID: 13299
	private LevelProperties.RumRunners.Spider properties;

	// Token: 0x040033F4 RID: 13300
	private RumRunnersLevelSpider spider;

	// Token: 0x040033F5 RID: 13301
	private DamageDealer damageDealer;

	// Token: 0x040033F6 RID: 13302
	private DamageReceiver damageReceiver;

	// Token: 0x040033F7 RID: 13303
	private bool isPink;

	// Token: 0x040033F8 RID: 13304
	private float hp;

	// Token: 0x040033F9 RID: 13305
	private float scaleX;

	// Token: 0x040033FA RID: 13306
	private Collider2D collider;

	// Token: 0x040033FB RID: 13307
	private Transform currentBulletOrigin;

	// Token: 0x040033FC RID: 13308
	private Coroutine deathCoroutine;

	// Token: 0x040033FD RID: 13309
	private RumRunnersLevelPoliceman.Direction lastShootDirection;

	// Token: 0x0200079B RID: 1947
	public enum Direction
	{
		// Token: 0x04003400 RID: 13312
		Straight,
		// Token: 0x04003401 RID: 13313
		Up,
		// Token: 0x04003402 RID: 13314
		Down
	}
}
