using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200063B RID: 1595
public class FlyingBlimpLevelMoonLady : LevelProperties.FlyingBlimp.Entity
{
	// Token: 0x17000386 RID: 902
	// (get) Token: 0x060020B9 RID: 8377 RVA: 0x0012E349 File Offset: 0x0012C749
	// (set) Token: 0x060020BA RID: 8378 RVA: 0x0012E351 File Offset: 0x0012C751
	public FlyingBlimpLevelMoonLady.State state { get; private set; }

	// Token: 0x060020BB RID: 8379 RVA: 0x0012E35C File Offset: 0x0012C75C
	protected override void Awake()
	{
		base.Awake();
		this.childColliders = base.gameObject.GetComponentsInChildren<CollisionChild>();
		this.changeStarted = false;
		Vector3 vector = new Vector3(1f, 1f, 1f);
		if (Rand.Bool())
		{
			vector.y = -vector.y;
			this.smoke.transform.position = this.smokeFlippedPos.transform.position;
		}
		this.smoke.transform.SetScale(new float?(1f), new float?(vector.y), new float?(1f));
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.GetComponent<Collider2D>().enabled = false;
		foreach (CollisionChild collisionChild in this.childColliders)
		{
			collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
			collisionChild.GetComponent<Collider2D>().enabled = false;
		}
	}

	// Token: 0x060020BC RID: 8380 RVA: 0x0012E480 File Offset: 0x0012C880
	public void StartIntro()
	{
		base.GetComponent<Collider2D>().enabled = true;
		base.transform.position = this.transformSpawnPoint.position;
		base.animator.SetTrigger("To A");
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x060020BD RID: 8381 RVA: 0x0012E4CC File Offset: 0x0012C8CC
	public override void LevelInit(LevelProperties.FlyingBlimp properties)
	{
		base.LevelInit(properties);
		this.state = FlyingBlimpLevelMoonLady.State.Unspawned;
	}

	// Token: 0x060020BE RID: 8382 RVA: 0x0012E4DC File Offset: 0x0012C8DC
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.state != FlyingBlimpLevelMoonLady.State.Morph)
		{
			if (base.properties.CurrentState.uFO.invincibility)
			{
				if (info.damage < base.properties.CurrentHealth)
				{
					base.properties.DealDamage(info.damage);
				}
				else if (this.state == FlyingBlimpLevelMoonLady.State.Idle)
				{
					base.properties.DealDamage(info.damage);
				}
			}
			else
			{
				base.properties.DealDamage(info.damage);
			}
		}
		if (base.properties.CurrentHealth <= 0f && this.state != FlyingBlimpLevelMoonLady.State.Death)
		{
			this.StartDeath();
		}
	}

	// Token: 0x060020BF RID: 8383 RVA: 0x0012E598 File Offset: 0x0012C998
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		if (PauseManager.state == PauseManager.State.Paused)
		{
			this.pedal.Pause();
			this.gears.Pause();
		}
		else
		{
			this.pedal.UnPause();
			this.gears.UnPause();
		}
	}

	// Token: 0x060020C0 RID: 8384 RVA: 0x0012E5F7 File Offset: 0x0012C9F7
	public override void OnLevelEnd()
	{
		base.OnLevelEnd();
		this.pedal.Stop();
		this.gears.Stop();
	}

	// Token: 0x060020C1 RID: 8385 RVA: 0x0012E615 File Offset: 0x0012CA15
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060020C2 RID: 8386 RVA: 0x0012E634 File Offset: 0x0012CA34
	private IEnumerator intro_cr()
	{
		AudioManager.Play("level_flying_blimp_transform_moon");
		this.state = FlyingBlimpLevelMoonLady.State.Morph;
		LevelProperties.FlyingBlimp.Morph p = base.properties.CurrentState.morph;
		PlanePlayerController playerOne = PlayerManager.GetPlayer<PlanePlayerController>(PlayerId.PlayerOne);
		PlanePlayerController playerTwo = PlayerManager.GetPlayer<PlanePlayerController>(PlayerId.PlayerTwo);
		if (playerOne != null && playerOne.isActiveAndEnabled)
		{
			playerOne.animationController.SetColorOverTime(this.dimColor.GetComponent<SpriteRenderer>().color, 15f);
		}
		if (playerTwo != null && playerTwo.isActiveAndEnabled)
		{
			playerTwo.animationController.SetColorOverTime(this.dimColor.GetComponent<SpriteRenderer>().color, 15f);
		}
		yield return null;
		while (base.transform.position != this.transformMorphEndPoint.position)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.transformMorphEndPoint.position, 300f * CupheadTime.Delta);
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
		}
		yield return CupheadTime.WaitForSeconds(this, p.crazyAHold);
		this.pedal.Stop();
		base.animator.SetTrigger("To B");
		yield return CupheadTime.WaitForSeconds(this, p.crazyBHold);
		base.animator.SetTrigger("End");
		base.StartCoroutine(this.stars_cr());
		yield return base.animator.WaitForAnimationToEnd(this, "Morph_End", false, true);
		this.state = FlyingBlimpLevelMoonLady.State.Idle;
		foreach (CollisionChild collisionChild in this.childColliders)
		{
			collisionChild.GetComponent<Collider2D>().enabled = true;
		}
		Level.Current.SetBounds(null, new int?(Level.Current.Right - 250), null, null);
		base.StartCoroutine(this.ufo_attack_handler_cr());
		yield return null;
		yield break;
	}

	// Token: 0x060020C3 RID: 8387 RVA: 0x0012E650 File Offset: 0x0012CA50
	private void SpawnStar(FlyingBlimpLevelStars prefab, Vector2 startPoint)
	{
		if (prefab != null)
		{
			Vector2 pos = prefab.transform.position;
			pos.y = 360f - startPoint.y;
			pos.x = 640f;
			prefab.Create(pos, base.properties.CurrentState.stars);
		}
	}

	// Token: 0x060020C4 RID: 8388 RVA: 0x0012E6B4 File Offset: 0x0012CAB4
	private IEnumerator stars_cr()
	{
		LevelProperties.FlyingBlimp.Stars p = base.properties.CurrentState.stars;
		string[] positionPattern = p.positionString.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] typePattern = p.typeString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int i = UnityEngine.Random.Range(0, typePattern.Length);
		float t = 0f;
		int place = UnityEngine.Random.Range(0, positionPattern.Length);
		float waitTime = 0f;
		Vector2 spawnPos = Vector2.zero;
		for (;;)
		{
			for (int j = place; j < positionPattern.Length; j++)
			{
				if (waitTime > 0f)
				{
					yield return CupheadTime.WaitForSeconds(this, waitTime);
				}
				if (positionPattern[j][0] == 'D')
				{
					Parser.FloatTryParse(positionPattern[j].Substring(1), out waitTime);
				}
				else
				{
					string[] array = positionPattern[j].Split(new char[]
					{
						'-'
					});
					foreach (string s in array)
					{
						float y = 0f;
						Parser.FloatTryParse(s, out y);
						FlyingBlimpLevelStars prefab = null;
						if (typePattern[i][0] == 'A')
						{
							prefab = this.starPrefabA;
						}
						else if (typePattern[i][0] == 'B')
						{
							prefab = this.starPrefabB;
						}
						else if (typePattern[i][0] == 'C')
						{
							prefab = this.starPrefabC;
						}
						else if (typePattern[i][0] == 'P')
						{
							prefab = this.starPrefabPink;
						}
						Parser.FloatTryParse(positionPattern[j].Substring(1), out waitTime);
						spawnPos.y = y;
						if (this.state != FlyingBlimpLevelMoonLady.State.Death)
						{
							this.SpawnStar(prefab, spawnPos);
						}
						i = (i + 1) % typePattern.Length;
					}
					waitTime = p.delay;
				}
				t += waitTime;
				j %= positionPattern.Length;
				place = 0;
			}
		}
		yield break;
	}

	// Token: 0x060020C5 RID: 8389 RVA: 0x0012E6D0 File Offset: 0x0012CAD0
	private IEnumerator ufo_attack_handler_cr()
	{
		this.state = FlyingBlimpLevelMoonLady.State.Idle;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.uFO.moonWaitForNextATK);
		base.StartCoroutine(this.ufo_cr());
		yield return null;
		yield break;
	}

	// Token: 0x060020C6 RID: 8390 RVA: 0x0012E6EB File Offset: 0x0012CAEB
	private void SmokeEffect()
	{
		base.animator.Play("Moon_Smoke");
	}

	// Token: 0x060020C7 RID: 8391 RVA: 0x0012E700 File Offset: 0x0012CB00
	private void SpawnUFO(FlyingBlimpLevelUFO prefab)
	{
		LevelProperties.FlyingBlimp.UFO uFO = base.properties.CurrentState.uFO;
		FlyingBlimpLevelUFO flyingBlimpLevelUFO = UnityEngine.Object.Instantiate<FlyingBlimpLevelUFO>(prefab);
		flyingBlimpLevelUFO.Init(this.ufoStartPoint.position, this.ufoMidPoint.position, this.ufoStopPoint.position, uFO.UFOSpeed, uFO.UFOHP, uFO);
	}

	// Token: 0x060020C8 RID: 8392 RVA: 0x0012E768 File Offset: 0x0012CB68
	private IEnumerator ufo_cr()
	{
		this.state = FlyingBlimpLevelMoonLady.State.Attack;
		float volume = 0.1f;
		LevelProperties.FlyingBlimp.UFO p = base.properties.CurrentState.uFO;
		string[] typePattern = p.UFOString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int index = UnityEngine.Random.Range(0, typePattern.Length);
		AudioManager.Play("level_flying_blimp_moon_anticipation");
		base.animator.SetTrigger("To ATK");
		yield return CupheadTime.WaitForSeconds(this, p.moonATKAnticipation);
		this.gears.Play();
		this.gears.volume = volume;
		AudioManager.Play("level_flying_blimp_moon_face_extend");
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Moon_Attack", false, true);
		this.time = 0f;
		this.startTimer = true;
		base.StartCoroutine(this.timer_cr());
		while (this.time < p.moonATKDuration)
		{
			this.pedal.volume = volume;
			if (volume < 1f)
			{
				volume += 0.1f;
			}
			if (this.state != FlyingBlimpLevelMoonLady.State.Death)
			{
				if (typePattern[index][0] == 'A')
				{
					this.SpawnUFO(this.ufoPrefabA);
				}
				else if (typePattern[index][0] == 'B')
				{
					this.SpawnUFO(this.ufoPrefabB);
				}
			}
			yield return CupheadTime.WaitForSeconds(this, p.UFODelay);
			if (index < typePattern.Length - 1)
			{
				index++;
			}
			else
			{
				index = 0;
			}
		}
		this.pedal.volume = 1f;
		base.animator.SetTrigger("End");
		this.startTimer = false;
		this.gears.Stop();
		AudioManager.Play("level_flying_blimp_moon_gears_idle");
		yield return base.animator.WaitForAnimationToEnd(this, "Moon_Attack_To_Idle", false, true);
		base.StartCoroutine(this.ufo_attack_handler_cr());
		yield break;
	}

	// Token: 0x060020C9 RID: 8393 RVA: 0x0012E784 File Offset: 0x0012CB84
	private IEnumerator timer_cr()
	{
		while (this.startTimer)
		{
			this.time += CupheadTime.Delta;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060020CA RID: 8394 RVA: 0x0012E79F File Offset: 0x0012CB9F
	public void StartDeath()
	{
		this.state = FlyingBlimpLevelMoonLady.State.Death;
		base.StartCoroutine(this.die_cr());
	}

	// Token: 0x060020CB RID: 8395 RVA: 0x0012E7B8 File Offset: 0x0012CBB8
	private IEnumerator die_cr()
	{
		base.animator.SetTrigger("Death");
		base.GetComponent<Collider2D>().enabled = false;
		yield return null;
		yield break;
	}

	// Token: 0x04002945 RID: 10565
	public bool changeStarted;

	// Token: 0x04002947 RID: 10567
	[SerializeField]
	private GameObject smoke;

	// Token: 0x04002948 RID: 10568
	[SerializeField]
	private Transform smokeFlippedPos;

	// Token: 0x04002949 RID: 10569
	[SerializeField]
	private AudioSource pedal;

	// Token: 0x0400294A RID: 10570
	[SerializeField]
	private AudioSource gears;

	// Token: 0x0400294B RID: 10571
	[SerializeField]
	private FlyingBlimpLevelUFO ufoPrefabA;

	// Token: 0x0400294C RID: 10572
	[SerializeField]
	private FlyingBlimpLevelUFO ufoPrefabB;

	// Token: 0x0400294D RID: 10573
	[SerializeField]
	private Transform ufoStartPoint;

	// Token: 0x0400294E RID: 10574
	[SerializeField]
	private Transform ufoMidPoint;

	// Token: 0x0400294F RID: 10575
	[SerializeField]
	private Transform ufoStopPoint;

	// Token: 0x04002950 RID: 10576
	[SerializeField]
	private Transform dimColor;

	// Token: 0x04002951 RID: 10577
	[SerializeField]
	private Transform transformSpawnPoint;

	// Token: 0x04002952 RID: 10578
	[SerializeField]
	private Transform transformMorphEndPoint;

	// Token: 0x04002953 RID: 10579
	[SerializeField]
	private FlyingBlimpLevelStars starPrefabA;

	// Token: 0x04002954 RID: 10580
	[SerializeField]
	private FlyingBlimpLevelStars starPrefabB;

	// Token: 0x04002955 RID: 10581
	[SerializeField]
	private FlyingBlimpLevelStars starPrefabC;

	// Token: 0x04002956 RID: 10582
	[SerializeField]
	private FlyingBlimpLevelStars starPrefabPink;

	// Token: 0x04002957 RID: 10583
	private DamageDealer damageDealer;

	// Token: 0x04002958 RID: 10584
	private DamageReceiver damageReceiver;

	// Token: 0x04002959 RID: 10585
	private CollisionChild[] childColliders;

	// Token: 0x0400295A RID: 10586
	private float time;

	// Token: 0x0400295B RID: 10587
	private bool startTimer;

	// Token: 0x0200063C RID: 1596
	public enum State
	{
		// Token: 0x0400295D RID: 10589
		Unspawned,
		// Token: 0x0400295E RID: 10590
		Morph,
		// Token: 0x0400295F RID: 10591
		Idle,
		// Token: 0x04002960 RID: 10592
		Attack,
		// Token: 0x04002961 RID: 10593
		Death
	}
}
