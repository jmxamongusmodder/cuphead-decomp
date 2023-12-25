using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000626 RID: 1574
public class FlyingBirdLevelSmallBird : LevelProperties.FlyingBird.Entity
{
	// Token: 0x1700037D RID: 893
	// (get) Token: 0x06002001 RID: 8193 RVA: 0x0012602C File Offset: 0x0012442C
	// (set) Token: 0x06002002 RID: 8194 RVA: 0x00126034 File Offset: 0x00124434
	public FlyingBirdLevelSmallBird.State state { get; private set; }

	// Token: 0x1700037E RID: 894
	// (get) Token: 0x06002003 RID: 8195 RVA: 0x0012603D File Offset: 0x0012443D
	// (set) Token: 0x06002004 RID: 8196 RVA: 0x00126045 File Offset: 0x00124445
	public FlyingBirdLevelSmallBird.Direction direction { get; private set; }

	// Token: 0x06002005 RID: 8197 RVA: 0x00126050 File Offset: 0x00124450
	private void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = this.sprite.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.collisionChild = this.sprite.GetComponent<CollisionChild>();
		this.collisionChild.OnPlayerCollision += this.OnPlayerCollision;
		this.aim = new GameObject("Aim").transform;
		this.aim.SetParent(this.bulletRoot);
		this.aim.ResetLocalTransforms();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06002006 RID: 8198 RVA: 0x001260F5 File Offset: 0x001244F5
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		this.PositionEggs();
	}

	// Token: 0x06002007 RID: 8199 RVA: 0x00126113 File Offset: 0x00124513
	public override void LevelInit(LevelProperties.FlyingBird properties)
	{
		base.LevelInit(properties);
		if (Level.Current.mode == Level.Mode.Easy)
		{
			properties.OnBossDeath += this.OnBossDeath;
		}
	}

	// Token: 0x06002008 RID: 8200 RVA: 0x00126140 File Offset: 0x00124540
	private void OnBossDeath()
	{
		if (Level.Current.mode == Level.Mode.Easy)
		{
			base.properties.OnBossDeath -= this.OnBossDeath;
		}
		this.sprite.GetComponent<Collider2D>().enabled = false;
		base.properties.OnStateChange -= this.OnBossDeath;
		this.StopAllCoroutines();
		this.sprite.transform.ResetLocalTransforms();
		base.animator.Play("Death");
		AudioManager.Play("level_flyingbird_small_bird_death_cry");
		this.emitAudioFromObject.Add("level_flyingbird_small_bird_death_cry");
		AudioManager.Stop("level_flyingbird_small_bird_rotating_eggs_loop");
		foreach (FlyingBirdLevelSmallBirdEgg flyingBirdLevelSmallBirdEgg in this.eggs)
		{
			flyingBirdLevelSmallBirdEgg.Explode();
		}
		if (Level.Current.mode != Level.Mode.Easy)
		{
			this.sprite.GetComponent<LevelBossDeathExploder>().StartExplosion();
			base.StartCoroutine(this.leave_cr());
		}
	}

	// Token: 0x06002009 RID: 8201 RVA: 0x00126260 File Offset: 0x00124660
	private void OnPlayerCollision(GameObject hit, CollisionPhase phase)
	{
		if (this.state == FlyingBirdLevelSmallBird.State.Dead)
		{
			return;
		}
		if (this.damageDealer != null)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600200A RID: 8202 RVA: 0x00126287 File Offset: 0x00124687
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.state == FlyingBirdLevelSmallBird.State.Dead)
		{
			return;
		}
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x0600200B RID: 8203 RVA: 0x001262A8 File Offset: 0x001246A8
	public void StartPattern(Vector2 pos)
	{
		base.properties.OnStateChange += this.OnBossDeath;
		if (this.state != FlyingBirdLevelSmallBird.State.Init)
		{
			return;
		}
		this.state = FlyingBirdLevelSmallBird.State.Starting;
		base.transform.position = pos;
		base.gameObject.SetActive(true);
		base.StartCoroutine(this.float_cr());
		base.StartCoroutine(this.start_cr());
	}

	// Token: 0x0600200C RID: 8204 RVA: 0x00126316 File Offset: 0x00124716
	private void TurnComplete()
	{
	}

	// Token: 0x0600200D RID: 8205 RVA: 0x00126318 File Offset: 0x00124718
	private void PositionEggs()
	{
		if (this.eggs == null || this.eggs.Count < 1)
		{
			return;
		}
		foreach (FlyingBirdLevelSmallBirdEgg flyingBirdLevelSmallBirdEgg in this.eggs)
		{
			flyingBirdLevelSmallBirdEgg.transform.localPosition = Vector3.zero;
		}
	}

	// Token: 0x0600200E RID: 8206 RVA: 0x0012639C File Offset: 0x0012479C
	private IEnumerator start_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		base.StartCoroutine(this.eggs_cr());
		yield return CupheadTime.WaitForSeconds(this, 1f);
		base.StartCoroutine(this.moveX_cr());
		base.StartCoroutine(this.moveY_cr());
		base.StartCoroutine(this.shooting_cr());
		yield break;
	}

	// Token: 0x0600200F RID: 8207 RVA: 0x001263B8 File Offset: 0x001247B8
	private IEnumerator float_cr()
	{
		yield return this.sprite.TweenLocalPositionY(0f, 10f, 1f, EaseUtils.EaseType.easeOutSine);
		for (;;)
		{
			yield return this.sprite.TweenLocalPositionY(10f, -10f, 1f, EaseUtils.EaseType.easeInOutSine);
			yield return this.sprite.TweenLocalPositionY(-10f, 10f, 1f, EaseUtils.EaseType.easeInOutSine);
		}
		yield break;
	}

	// Token: 0x06002010 RID: 8208 RVA: 0x001263D4 File Offset: 0x001247D4
	private IEnumerator leave_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		SpriteRenderer renderer = this.sprite.GetComponent<SpriteRenderer>();
		float end = (float)((base.transform.position.x <= 0f) ? Level.Current.Left : Level.Current.Right);
		end += renderer.bounds.size.x / 2f * Mathf.Sign(base.transform.position.x);
		base.StartCoroutine(this.tweenX_cr(base.transform.position.x, end, base.properties.CurrentState.smallBird.leaveTime, EaseUtils.EaseType.easeInOutSine));
		this.sprite.GetComponent<Collider2D>().enabled = false;
		while (this.state != FlyingBirdLevelSmallBird.State.Dead)
		{
			yield return null;
		}
		UnityEngine.Object.Destroy(this.sprite.GetComponent<LevelBossDeathExploder>());
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06002011 RID: 8209 RVA: 0x001263F0 File Offset: 0x001247F0
	private void ShootProjectile()
	{
		this.aim.LookAt2D(PlayerManager.Current.center);
		this.bulletPrefab.Create(this.bulletRoot.position, this.aim.eulerAngles.z + 180f, -base.properties.CurrentState.smallBird.shotSpeed).SetParryable(true);
	}

	// Token: 0x06002012 RID: 8210 RVA: 0x00126464 File Offset: 0x00124864
	private IEnumerator shooting_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.smallBird.shotDelay);
			AbstractPlayerController target = PlayerManager.GetNext();
			if (this.direction == FlyingBirdLevelSmallBird.Direction.Left)
			{
				if (target.center.x > base.transform.position.x)
				{
					yield return this.Turn(FlyingBirdLevelSmallBird.Direction.Right);
				}
			}
			else if (target.center.x < base.transform.position.x)
			{
				yield return this.Turn(FlyingBirdLevelSmallBird.Direction.Left);
			}
			FlyingBirdLevelSmallBird.State lastState = this.state;
			this.state = FlyingBirdLevelSmallBird.State.Shooting;
			base.animator.SetTrigger("Shoot");
			base.animator.WaitForAnimationToEnd(this, "Shoot", false, true);
			this.state = lastState;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002013 RID: 8211 RVA: 0x0012647F File Offset: 0x0012487F
	private Coroutine Turn(FlyingBirdLevelSmallBird.Direction d)
	{
		return base.StartCoroutine(this.turn_cr(d));
	}

	// Token: 0x06002014 RID: 8212 RVA: 0x00126490 File Offset: 0x00124890
	private IEnumerator turn_cr(FlyingBirdLevelSmallBird.Direction d)
	{
		if (this.direction != d)
		{
			this.sprite.transform.SetScale(new float?((float)d), null, null);
			this.direction = d;
			base.animator.Play("Turn");
			yield return base.animator.WaitForAnimationToEnd(this, "Turn", false, true);
		}
		yield break;
	}

	// Token: 0x06002015 RID: 8213 RVA: 0x001264B4 File Offset: 0x001248B4
	private IEnumerator eggs_cr()
	{
		int count = base.properties.CurrentState.smallBird.eggCount;
		this.eggs = new List<FlyingBirdLevelSmallBirdEgg>();
		this.eggContainer = new GameObject("Eggs").transform;
		this.eggContainer.SetParent(base.transform);
		this.eggContainer.ResetLocalTransforms();
		this.eggContainer.SetLocalPosition(null, new float?(-65f), null);
		for (int i = 0; i < count; i++)
		{
			float value = (float)i / (float)count * 360f;
			FlyingBirdLevelSmallBirdEgg flyingBirdLevelSmallBirdEgg = this.eggPrefab.InstantiatePrefab<FlyingBirdLevelSmallBirdEgg>();
			flyingBirdLevelSmallBirdEgg.SetParent(this.eggContainer, base.properties);
			flyingBirdLevelSmallBirdEgg.container.SetEulerAngles(new float?(0f), new float?(0f), new float?(value));
			this.eggs.Add(flyingBirdLevelSmallBirdEgg);
		}
		AudioManager.PlayLoop("level_flyingbird_small_bird_rotating_eggs_loop");
		this.emitAudioFromObject.Add("level_flyingbird_small_bird_rotating_eggs_loop");
		for (;;)
		{
			this.eggContainer.AddLocalEulerAngles(0f, 0f, base.properties.CurrentState.smallBird.eggRotationSpeed * CupheadTime.Delta);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002016 RID: 8214 RVA: 0x001264D0 File Offset: 0x001248D0
	private IEnumerator moveX_cr()
	{
		this.direction = FlyingBirdLevelSmallBird.Direction.Left;
		for (;;)
		{
			float minX = base.properties.CurrentState.smallBird.minX;
			this.state = FlyingBirdLevelSmallBird.State.Left;
			yield return base.StartCoroutine(this.tweenX_cr(base.transform.position.x, minX, base.properties.CurrentState.smallBird.timeX, EaseUtils.EaseType.easeInOutSine));
			yield return this.Turn(FlyingBirdLevelSmallBird.Direction.Right);
			this.state = FlyingBirdLevelSmallBird.State.Right;
			yield return base.StartCoroutine(this.tweenX_cr(minX, 520f, base.properties.CurrentState.smallBird.timeX, EaseUtils.EaseType.easeInOutSine));
			yield return this.Turn(FlyingBirdLevelSmallBird.Direction.Left);
		}
		yield break;
	}

	// Token: 0x06002017 RID: 8215 RVA: 0x001264EC File Offset: 0x001248EC
	private IEnumerator tweenX_cr(float start, float end, float time, EaseUtils.EaseType ease)
	{
		base.transform.SetPosition(new float?(start), null, null);
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			base.transform.SetPosition(new float?(EaseUtils.Ease(ease, start, end, val)), null, null);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.state = FlyingBirdLevelSmallBird.State.Dead;
		base.transform.SetPosition(new float?(end), null, null);
		yield return null;
		yield break;
	}

	// Token: 0x06002018 RID: 8216 RVA: 0x00126524 File Offset: 0x00124924
	private IEnumerator moveY_cr()
	{
		if (Rand.Bool())
		{
			yield return base.StartCoroutine(this.tweenY_cr(base.transform.position.y, 260f, base.properties.CurrentState.smallBird.timeY, EaseUtils.EaseType.easeInOutSine));
		}
		else
		{
			float currentDist = -230f - base.transform.position.y;
			float normalDist = 490f;
			float time = base.properties.CurrentState.smallBird.timeY - currentDist / normalDist;
			yield return base.StartCoroutine(this.tweenY_cr(base.transform.position.y, -230f, time, EaseUtils.EaseType.easeInOutSine));
			yield return base.StartCoroutine(this.tweenY_cr(-230f, 260f, base.properties.CurrentState.smallBird.timeY, EaseUtils.EaseType.easeInOutSine));
		}
		for (;;)
		{
			yield return base.StartCoroutine(this.tweenY_cr(260f, -230f, base.properties.CurrentState.smallBird.timeY, EaseUtils.EaseType.easeInOutSine));
			yield return base.StartCoroutine(this.tweenY_cr(-230f, 260f, base.properties.CurrentState.smallBird.timeY, EaseUtils.EaseType.easeInOutSine));
		}
		yield break;
	}

	// Token: 0x06002019 RID: 8217 RVA: 0x00126540 File Offset: 0x00124940
	private IEnumerator tweenY_cr(float start, float end, float time, EaseUtils.EaseType ease)
	{
		base.transform.SetPosition(null, new float?(start), null);
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			base.transform.SetPosition(null, new float?(EaseUtils.Ease(ease, start, end, val)), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetPosition(null, new float?(end), null);
		yield return null;
		yield break;
	}

	// Token: 0x0600201A RID: 8218 RVA: 0x00126578 File Offset: 0x00124978
	private void SmallLaserShootSFX()
	{
		AudioManager.Play("level_flyingbird_small_bird_shoot");
		this.emitAudioFromObject.Add("level_flyingbird_small_bird_shoot");
	}

	// Token: 0x0400288A RID: 10378
	[SerializeField]
	private FlyingBirdLevelSmallBirdSprite sprite;

	// Token: 0x0400288B RID: 10379
	private CollisionChild collisionChild;

	// Token: 0x0400288C RID: 10380
	private DamageReceiver damageReceiver;

	// Token: 0x0400288D RID: 10381
	private DamageDealer damageDealer;

	// Token: 0x0400288E RID: 10382
	[Space(10f)]
	[SerializeField]
	private FlyingBirdLevelSmallBirdEgg eggPrefab;

	// Token: 0x0400288F RID: 10383
	[Space(10f)]
	[SerializeField]
	private BasicProjectile bulletPrefab;

	// Token: 0x04002890 RID: 10384
	[SerializeField]
	private Transform bulletRoot;

	// Token: 0x04002893 RID: 10387
	private Transform aim;

	// Token: 0x04002894 RID: 10388
	private Transform eggContainer;

	// Token: 0x04002895 RID: 10389
	private List<FlyingBirdLevelSmallBirdEgg> eggs;

	// Token: 0x02000627 RID: 1575
	public enum State
	{
		// Token: 0x04002897 RID: 10391
		Init,
		// Token: 0x04002898 RID: 10392
		Starting,
		// Token: 0x04002899 RID: 10393
		Right,
		// Token: 0x0400289A RID: 10394
		Left,
		// Token: 0x0400289B RID: 10395
		Shooting,
		// Token: 0x0400289C RID: 10396
		Dead
	}

	// Token: 0x02000628 RID: 1576
	public enum Direction
	{
		// Token: 0x0400289E RID: 10398
		Right = -1,
		// Token: 0x0400289F RID: 10399
		Left = 1
	}
}
