using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000616 RID: 1558
public class FlyingBirdLevelBird : LevelProperties.FlyingBird.Entity
{
	// Token: 0x1700037A RID: 890
	// (get) Token: 0x06001F7E RID: 8062 RVA: 0x001210BC File Offset: 0x0011F4BC
	// (set) Token: 0x06001F7F RID: 8063 RVA: 0x001210C4 File Offset: 0x0011F4C4
	public FlyingBirdLevelBird.State state { get; private set; }

	// Token: 0x1700037B RID: 891
	// (get) Token: 0x06001F80 RID: 8064 RVA: 0x001210CD File Offset: 0x0011F4CD
	// (set) Token: 0x06001F81 RID: 8065 RVA: 0x001210D5 File Offset: 0x0011F4D5
	public bool floating { get; private set; }

	// Token: 0x06001F82 RID: 8066 RVA: 0x001210E0 File Offset: 0x0011F4E0
	protected override void Awake()
	{
		base.Awake();
		this.state = FlyingBirdLevelBird.State.Init;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.heart.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
		this.heart.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.houseCollider = new GameObject("House Collider").transform;
		BoxCollider2D boxCollider2D = this.houseCollider.gameObject.AddComponent<BoxCollider2D>();
		boxCollider2D.isTrigger = true;
		boxCollider2D.offset = new Vector2(60f, -50f);
		boxCollider2D.size = new Vector2(400f, 300f);
		CollisionChild collisionChild = this.houseCollider.gameObject.AddComponent<CollisionChild>();
		collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
	}

	// Token: 0x06001F83 RID: 8067 RVA: 0x001211DC File Offset: 0x0011F5DC
	private void Start()
	{
		this.featherPrefab.CreatePool(150);
		this.heart.gameObject.SetActive(false);
		this.feathersFirstTime = true;
	}

	// Token: 0x06001F84 RID: 8068 RVA: 0x00121206 File Offset: 0x0011F606
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.damageReceiver != null)
		{
			this.damageReceiver.OnDamageTaken -= this.OnDamageTaken;
		}
		this.featherPrefab = null;
	}

	// Token: 0x06001F85 RID: 8069 RVA: 0x00121240 File Offset: 0x0011F640
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.state == FlyingBirdLevelBird.State.Dead || this.state == FlyingBirdLevelBird.State.Dying)
		{
			return;
		}
		base.properties.DealDamage(info.damage);
		if (base.properties.CurrentHealth <= 0f)
		{
			this.BirdDie();
		}
	}

	// Token: 0x06001F86 RID: 8070 RVA: 0x00121292 File Offset: 0x0011F692
	private void Update()
	{
		if (this.houseCollider != null)
		{
			this.houseCollider.position = base.transform.position;
		}
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001F87 RID: 8071 RVA: 0x001212D1 File Offset: 0x0011F6D1
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001F88 RID: 8072 RVA: 0x001212FC File Offset: 0x0011F6FC
	public override void LevelInit(LevelProperties.FlyingBird properties)
	{
		base.LevelInit(properties);
		properties.OnStateChange += this.OnStateChange;
		this.floating = false;
		base.StartCoroutine(this.float_cr());
		this.garbageIndex = UnityEngine.Random.Range(0, properties.CurrentState.garbage.garbageTypeString.Length);
	}

	// Token: 0x06001F89 RID: 8073 RVA: 0x00121354 File Offset: 0x0011F754
	private void OnStateChange()
	{
		if (base.properties.CurrentState.stateName == LevelProperties.FlyingBird.States.Whistle)
		{
			base.StartCoroutine(this.whistle_cr());
			if (this.patternCoroutine != null)
			{
				base.StopCoroutine(this.patternCoroutine);
			}
			this.patternCoroutine = base.StartCoroutine(this.whistle_cr());
		}
	}

	// Token: 0x06001F8A RID: 8074 RVA: 0x001213B0 File Offset: 0x0011F7B0
	public void IntroContinue()
	{
		Animator component = base.GetComponent<Animator>();
		component.SetTrigger("Continue");
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001F8B RID: 8075 RVA: 0x001213DC File Offset: 0x0011F7DC
	private void SfxIntroA()
	{
		AudioManager.Play("level_flying_bird_intro_a");
	}

	// Token: 0x06001F8C RID: 8076 RVA: 0x001213E8 File Offset: 0x0011F7E8
	private void SfxIntroB()
	{
		AudioManager.Play("level_flying_bird_intro_b");
	}

	// Token: 0x06001F8D RID: 8077 RVA: 0x001213F4 File Offset: 0x0011F7F4
	private void OnIntroAnimComplete()
	{
		this.introEnded = true;
	}

	// Token: 0x06001F8E RID: 8078 RVA: 0x00121400 File Offset: 0x0011F800
	private IEnumerator intro_cr()
	{
		while (!this.introEnded)
		{
			yield return null;
		}
		this.floating = true;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.floating.attackInitialDelayRange.RandomFloat());
		this.state = FlyingBirdLevelBird.State.Idle;
		yield break;
	}

	// Token: 0x06001F8F RID: 8079 RVA: 0x0012141B File Offset: 0x0011F81B
	private void IdleNoBlink()
	{
		this.blinks++;
		if (this.blinks >= this.maxBlinks)
		{
			base.animator.SetBool("Blink", true);
		}
	}

	// Token: 0x06001F90 RID: 8080 RVA: 0x0012144D File Offset: 0x0011F84D
	private void Blink()
	{
		this.blinks = 0;
		this.maxBlinks = UnityEngine.Random.Range(2, 5);
		base.animator.SetBool("Blink", false);
	}

	// Token: 0x06001F91 RID: 8081 RVA: 0x00121474 File Offset: 0x0011F874
	private IEnumerator whistle_cr()
	{
		this.state = FlyingBirdLevelBird.State.Whistle;
		this.floating = false;
		Animator animator = base.GetComponent<Animator>();
		animator.Play("Whistle");
		AudioManager.Play("level_flying_bird_whistle");
		yield return animator.WaitForAnimationToEnd(this, "Whistle", false, true);
		this.state = FlyingBirdLevelBird.State.Idle;
		this.floating = true;
		yield break;
	}

	// Token: 0x06001F92 RID: 8082 RVA: 0x00121490 File Offset: 0x0011F890
	private IEnumerator float_cr()
	{
		bool goUp = Rand.Bool();
		for (;;)
		{
			if (goUp)
			{
				yield return base.StartCoroutine(this.floatTo_cr(base.properties.CurrentState.floating.top, base.properties.CurrentState.floating.time));
			}
			else
			{
				yield return base.StartCoroutine(this.floatTo_cr(base.properties.CurrentState.floating.bottom, base.properties.CurrentState.floating.time));
			}
			goUp = !goUp;
		}
		yield break;
	}

	// Token: 0x06001F93 RID: 8083 RVA: 0x001214AC File Offset: 0x0011F8AC
	private IEnumerator floatTo_cr(float end, float time)
	{
		float t = 0f;
		float start = base.transform.position.y;
		while (t < time)
		{
			if (!this.floating)
			{
				while (!this.floating)
				{
					yield return null;
				}
			}
			float val = t / time;
			base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, end, val)), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetPosition(null, new float?(end), null);
		yield break;
	}

	// Token: 0x06001F94 RID: 8084 RVA: 0x001214D5 File Offset: 0x0011F8D5
	public void StartFeathers()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.feathers_cr());
	}

	// Token: 0x06001F95 RID: 8085 RVA: 0x00121500 File Offset: 0x0011F900
	private void FireFeathers(int count, float offset, bool parryable)
	{
		parryable = false;
		for (int i = 0; i < count; i++)
		{
			float num = 360f * ((float)i / (float)count);
			this.featherPrefab.Spawn(base.transform.position, Quaternion.Euler(new Vector3(0f, 0f, offset + num - 180f))).Init(base.properties.CurrentState.feathers.speed).SetParryable(parryable);
		}
	}

	// Token: 0x06001F96 RID: 8086 RVA: 0x00121584 File Offset: 0x0011F984
	private IEnumerator feathers_cr()
	{
		this.state = FlyingBirdLevelBird.State.Feathers;
		this.floating = false;
		Animator animator = base.GetComponent<Animator>();
		animator.Play("Feathers_Start");
		AudioManager.Play("level_flyingbird_feathers_start");
		this.emitAudioFromObject.Add("level_flyingbird_feathers_start");
		yield return animator.WaitForAnimationToEnd(this, "Feathers_Start", false, true);
		LevelProperties.FlyingBird.Feathers featherProperties = base.properties.CurrentState.feathers;
		KeyValue[] pattern = KeyValue.ListFromString(featherProperties.pattern[UnityEngine.Random.Range(0, featherProperties.pattern.Length)], new char[]
		{
			'P',
			'D'
		});
		AudioManager.PlayLoop("level_flyingbird_feathers_loop");
		this.emitAudioFromObject.Add("level_flyingbird_feathers_loop");
		for (int i = 0; i < pattern.Length; i++)
		{
			float offset = 0f;
			bool parryable = false;
			if (pattern[i].key == "P")
			{
				int p = 0;
				while ((float)p < pattern[i].value)
				{
					this.FireFeathers(featherProperties.count, offset, parryable);
					parryable = !parryable;
					offset += featherProperties.offset;
					yield return CupheadTime.WaitForSeconds(this, this.feathersFirstTime ? featherProperties.initalShotDelay : featherProperties.shotDelay);
					this.feathersFirstTime = false;
					p++;
				}
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, pattern[i].value);
			}
			yield return null;
		}
		AudioManager.Stop("level_flyingbird_feathers_loop");
		this.floating = true;
		animator.Play("Feathers_End");
		AudioManager.Play("level_flyingbird_feathers_hesitate");
		this.emitAudioFromObject.Add("level_flyingbird_feathers_hesitate");
		yield return CupheadTime.WaitForSeconds(this, featherProperties.hesitate);
		animator.Play("Feathers_Hesitate_End");
		AudioManager.Stop("level_flyingbird_feathers_hesitate");
		yield return animator.WaitForAnimationToEnd(this, "Feathers_Hesitate_End", false, true);
		this.state = FlyingBirdLevelBird.State.Idle;
		yield break;
	}

	// Token: 0x06001F97 RID: 8087 RVA: 0x0012159F File Offset: 0x0011F99F
	public void StartEggs()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.eggs_cr());
	}

	// Token: 0x06001F98 RID: 8088 RVA: 0x001215CA File Offset: 0x0011F9CA
	private void FireEgg()
	{
		this.eggPrefab.Create(base.properties.CurrentState.feathers.speed, this.eggRoot.position);
	}

	// Token: 0x06001F99 RID: 8089 RVA: 0x001215FD File Offset: 0x0011F9FD
	private void SoundFireEggThroaty()
	{
		AudioManager.Play("level_flying_bird_spit_throaty");
		this.emitAudioFromObject.Add("level_flying_bird_spit_throaty");
	}

	// Token: 0x06001F9A RID: 8090 RVA: 0x00121619 File Offset: 0x0011FA19
	private void SoundFireEggProjectile()
	{
		AudioManager.Play("level_flying_bird_spit");
		this.emitAudioFromObject.Add("level_flying_bird_spit");
	}

	// Token: 0x06001F9B RID: 8091 RVA: 0x00121638 File Offset: 0x0011FA38
	private IEnumerator eggs_cr()
	{
		this.floating = true;
		this.state = FlyingBirdLevelBird.State.Eggs;
		Animator animator = base.GetComponent<Animator>();
		LevelProperties.FlyingBird.Eggs eggProperties = base.properties.CurrentState.eggs;
		KeyValue[] pattern = KeyValue.ListFromString(eggProperties.pattern[UnityEngine.Random.Range(0, eggProperties.pattern.Length)], new char[]
		{
			'P',
			'D'
		});
		for (int i = 0; i < pattern.Length; i++)
		{
			if (pattern[i].key == "P")
			{
				int p = 0;
				while ((float)p < pattern[i].value)
				{
					yield return CupheadTime.WaitForSeconds(this, eggProperties.shotDelay);
					animator.Play("Spit");
					p++;
				}
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, pattern[i].value);
			}
			yield return null;
		}
		yield return animator.WaitForAnimationToEnd(this, "Spit", false, true);
		yield return CupheadTime.WaitForSeconds(this, eggProperties.hesitate);
		this.state = FlyingBirdLevelBird.State.Idle;
		yield break;
	}

	// Token: 0x06001F9C RID: 8092 RVA: 0x00121653 File Offset: 0x0011FA53
	public void StartLasers()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.lasers_cr());
	}

	// Token: 0x06001F9D RID: 8093 RVA: 0x00121680 File Offset: 0x0011FA80
	private void FireLasers()
	{
		AudioManager.Play("level_flyingbird_bird_laser_fire");
		this.emitAudioFromObject.Add("level_flyingbird_bird_laser_fire");
		this.laserEffect.Create(this.laserRoots[0].position);
		foreach (Transform transform in this.laserRoots)
		{
			this.laserPrefab.Create(transform.position, -transform.eulerAngles.z, -base.properties.CurrentState.lasers.speed);
		}
	}

	// Token: 0x06001F9E RID: 8094 RVA: 0x0012171B File Offset: 0x0011FB1B
	private void LasersAnimEnded()
	{
		this.state = FlyingBirdLevelBird.State.LasersEnding;
	}

	// Token: 0x06001F9F RID: 8095 RVA: 0x00121724 File Offset: 0x0011FB24
	private IEnumerator lasers_cr()
	{
		Animator animator = base.GetComponent<Animator>();
		this.state = FlyingBirdLevelBird.State.Lasers;
		this.floating = false;
		LevelProperties.FlyingBird.Lasers properties = base.properties.CurrentState.lasers;
		animator.SetTrigger("StartLasers");
		while (this.state == FlyingBirdLevelBird.State.Lasers)
		{
			yield return null;
		}
		this.floating = true;
		yield return CupheadTime.WaitForSeconds(this, properties.hesitate);
		this.state = FlyingBirdLevelBird.State.Idle;
		yield break;
	}

	// Token: 0x06001FA0 RID: 8096 RVA: 0x0012173F File Offset: 0x0011FB3F
	private void LasersSFX()
	{
		AudioManager.Play("level_flyingbird_bird_lasers");
		this.emitAudioFromObject.Add("level_flyingbird_bird_lasers");
	}

	// Token: 0x06001FA1 RID: 8097 RVA: 0x0012175C File Offset: 0x0011FB5C
	public void BirdFall()
	{
		this.state = FlyingBirdLevelBird.State.Dying;
		this.houseCollider.gameObject.SetActive(false);
		AudioManager.Stop("level_flyingbird_feathers_loop");
		base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		base.GetComponent<CircleCollider2D>().enabled = false;
		this.StopAllCoroutines();
		base.animator.Play("Death");
		base.StartCoroutine(this.die_cr());
		this.nurses.Die();
	}

	// Token: 0x06001FA2 RID: 8098 RVA: 0x001217D0 File Offset: 0x0011FBD0
	public void BirdDie()
	{
		this.nurses.Die();
		this.nurses.nurses[0].gameObject.SetActive(false);
		this.nurses.nurses[1].gameObject.SetActive(false);
		base.GetComponent<Collider2D>().enabled = false;
		base.StopCoroutine(this.checkHeart_cr());
		base.StopCoroutine(this.garbage_cr());
		this.nurses.animator.SetTrigger("Die");
		base.animator.Play("Stretcher_Death");
		AudioManager.PlayLoop("level_flyingbird_stretcher_death");
		this.emitAudioFromObject.Add("level_flyingbird_stretcher_death");
		foreach (BoxCollider2D boxCollider2D in base.GetComponentsInChildren<BoxCollider2D>())
		{
			boxCollider2D.enabled = false;
		}
		foreach (CircleCollider2D circleCollider2D in base.GetComponentsInChildren<CircleCollider2D>())
		{
			circleCollider2D.enabled = false;
		}
	}

	// Token: 0x06001FA3 RID: 8099 RVA: 0x001218CF File Offset: 0x0011FCCF
	private void OnDeathComplete()
	{
		this.StopAllCoroutines();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06001FA4 RID: 8100 RVA: 0x001218E3 File Offset: 0x0011FCE3
	private void DeathSfx()
	{
	}

	// Token: 0x06001FA5 RID: 8101 RVA: 0x001218E5 File Offset: 0x0011FCE5
	private void OnDeathExploded()
	{
		base.GetComponent<LevelBossDeathExploder>().StopExplosions();
		this.smallBird.StartPattern(base.transform.position);
	}

	// Token: 0x06001FA6 RID: 8102 RVA: 0x00121910 File Offset: 0x0011FD10
	private IEnumerator die_cr()
	{
		Animator animator = base.GetComponent<Animator>();
		while (base.transform.position.y > 100f || base.transform.position.y < 0f)
		{
			yield return null;
		}
		this.floating = false;
		animator.Play("Death");
		this.deathEffectFront.Create(this.deathEffectsRoot.position);
		this.deathEffectBack.Create(this.deathEffectsRoot.position);
		yield break;
	}

	// Token: 0x06001FA7 RID: 8103 RVA: 0x0012192C File Offset: 0x0011FD2C
	public void OnBossRevival()
	{
		this.state = FlyingBirdLevelBird.State.Reviving;
		this.houseCollider.gameObject.SetActive(true);
		UnityEngine.Object.Destroy(this.deathParts);
		base.gameObject.SetActive(true);
		base.animator.Play("Revived");
		this.nurses.animator.SetTrigger("StartNurses");
		base.GetComponent<CircleCollider2D>().enabled = true;
		base.GetComponent<HitFlash>().StopAllCoroutines();
		base.GetComponent<HitFlash>().SetColor(0f);
		base.transform.SetPosition(new float?((float)Level.Current.Right + 250f), new float?((float)(Level.Current.Ground - 150)), null);
		base.StartCoroutine(this.revival_cr());
		this.heart.InitHeart(base.properties);
	}

	// Token: 0x06001FA8 RID: 8104 RVA: 0x00121A14 File Offset: 0x0011FE14
	private IEnumerator revival_cr()
	{
		float end = (float)Level.Current.Ground + 250f;
		yield return base.StartCoroutine(this.move_to_position_cr(base.transform.position.y, end, base.properties.CurrentState.floating.time, EaseUtils.EaseType.easeInOutSine));
		this.state = FlyingBirdLevelBird.State.Revived;
		this.nurses.InitNurse(base.properties.CurrentState.nurses);
		yield break;
	}

	// Token: 0x06001FA9 RID: 8105 RVA: 0x00121A30 File Offset: 0x0011FE30
	private IEnumerator move_to_position_cr(float start, float end, float time, EaseUtils.EaseType ease)
	{
		base.transform.SetPosition(null, new float?(start), null);
		float startX = base.transform.position.x;
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			base.transform.SetPosition(new float?(EaseUtils.Ease(ease, startX, 0f, val)), new float?(EaseUtils.Ease(ease, start, end, val)), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetPosition(new float?(0f), new float?(end), null);
		yield return null;
		base.StartCoroutine(this.stretcherMove_cr());
		yield break;
	}

	// Token: 0x06001FAA RID: 8106 RVA: 0x00121A68 File Offset: 0x0011FE68
	private IEnumerator stretcherMove_cr()
	{
		bool movingRight = Rand.Bool();
		float time = base.properties.CurrentState.bigBird.speedXTime;
		float end = 0f;
		do
		{
			if (this.state != FlyingBirdLevelBird.State.Heart)
			{
				float t = 0f;
				float start = base.transform.position.x;
				if (movingRight)
				{
					end = 290f;
				}
				else
				{
					end = -240f;
				}
				while (t < time)
				{
					if (this.state != FlyingBirdLevelBird.State.Heart)
					{
						float value = t / time;
						base.transform.SetPosition(new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, end, value)), null, null);
						t += CupheadTime.Delta;
					}
					yield return null;
				}
				base.transform.SetPosition(new float?(end), null, null);
				movingRight = !movingRight;
			}
			yield return null;
		}
		while (base.properties.CurrentHealth > 0f);
		yield break;
	}

	// Token: 0x06001FAB RID: 8107 RVA: 0x00121A83 File Offset: 0x0011FE83
	public void StartGarbageOne()
	{
		base.StartCoroutine(this.garbage_cr());
	}

	// Token: 0x06001FAC RID: 8108 RVA: 0x00121A94 File Offset: 0x0011FE94
	private IEnumerator garbage_cr()
	{
		this.state = FlyingBirdLevelBird.State.Garbage;
		base.animator.SetBool("OnGarbage", true);
		yield return base.animator.WaitForAnimationToStart(this, "Garbage_Start", false);
		AudioManager.Play("level_flyingbird_stretcher_garbage_start");
		this.emitAudioFromObject.Add("level_flyingbird_stretcher_garbage_start");
		float garbageSpeed = base.properties.CurrentState.garbage.speedX;
		float garbageCounter = 0f;
		GameObject chosenPrefab = null;
		yield return base.animator.WaitForAnimationToEnd(this, "Garbage_Start", false, true);
		int maxShotIndex = UnityEngine.Random.Range(0, base.properties.CurrentState.garbage.shotCount.Split(new char[]
		{
			','
		}).Length);
		int maxShot = Parser.IntParse(base.properties.CurrentState.garbage.shotCount.Split(new char[]
		{
			','
		})[maxShotIndex]);
		while (garbageCounter < (float)maxShot)
		{
			string[] garbageTypes = base.properties.CurrentState.garbage.garbageTypeString[this.garbageIndex].Split(new char[]
			{
				','
			});
			if (garbageTypes[this.typeIndex][0] == 'P')
			{
				chosenPrefab = this.bootPinkPrefab;
			}
			else if (garbageTypes[this.typeIndex][0] == 'B')
			{
				chosenPrefab = this.bootPrefab;
			}
			else if (garbageTypes[this.typeIndex][0] == 'F')
			{
				chosenPrefab = this.fishPrefab;
			}
			else if (garbageTypes[this.typeIndex][0] == 'A')
			{
				chosenPrefab = this.applePrefab;
			}
			else
			{
				global::Debug.LogError("Invalid garbage type string.", null);
			}
			yield return base.animator.WaitForAnimationToEnd(this, "Garbage", false, true);
			AudioManager.Play("level_flyingbird_stretcher_garbage");
			this.emitAudioFromObject.Add("level_flyingbird_stretcher_garbage");
			GameObject garbage = UnityEngine.Object.Instantiate<GameObject>(chosenPrefab, this.garbageRoot.transform.position, Quaternion.identity);
			garbage.GetComponent<BasicProjectile>().Speed = 1f;
			garbage.transform.localScale = Vector3.one * base.properties.CurrentState.garbage.shotSize;
			base.StartCoroutine(this.multiShotGarbage_cr(garbageSpeed, garbage));
			garbageSpeed += base.properties.CurrentState.garbage.speedXIncreaser;
			garbageCounter += 1f;
			if (this.typeIndex < garbageTypes.Length - 1)
			{
				this.typeIndex++;
			}
			else
			{
				this.garbageIndex = (this.garbageIndex + 1) % base.properties.CurrentState.garbage.garbageTypeString.Length;
				this.typeIndex = 0;
			}
			if (garbageCounter < (float)maxShot)
			{
				yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.garbage.shotDelay);
			}
		}
		garbageCounter = 0f;
		base.animator.SetTrigger("Continue");
		base.animator.SetBool("OnGarbage", false);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.garbage.hesitate.RandomFloat());
		this.state = FlyingBirdLevelBird.State.Revived;
		yield break;
	}

	// Token: 0x06001FAD RID: 8109 RVA: 0x00121AB0 File Offset: 0x0011FEB0
	private IEnumerator multiShotGarbage_cr(float speedX, GameObject proj)
	{
		bool isFalling = false;
		float pct = 1f;
		Vector3 velocity = new Vector3(-speedX, base.properties.CurrentState.garbage.speedY);
		while (proj != null)
		{
			if (proj.transform.position.y > (float)Level.Current.Ground - 200f)
			{
				if (isFalling)
				{
					pct -= CupheadTime.Delta * 4f;
					if (pct < -1f)
					{
						pct = -1f;
					}
				}
				velocity.y = base.properties.CurrentState.garbage.speedY * pct;
				proj.transform.position += velocity * CupheadTime.FixedDelta;
				if (proj.transform.position.y >= base.properties.CurrentState.garbage.maxHeight)
				{
					isFalling = true;
				}
				yield return null;
			}
			yield return null;
		}
		UnityEngine.Object.Destroy(proj);
		yield break;
	}

	// Token: 0x06001FAE RID: 8110 RVA: 0x00121AD9 File Offset: 0x0011FED9
	public void StartHeartAttack()
	{
		this.state = FlyingBirdLevelBird.State.HeartTrans;
		base.animator.SetBool("OnRegurgitate", true);
		AudioManager.Play("level_flyingbird_stretcher_regurgitate_start");
		this.emitAudioFromObject.Add("level_flyingbird_stretcher_regurgitate_start");
	}

	// Token: 0x06001FAF RID: 8111 RVA: 0x00121B0E File Offset: 0x0011FF0E
	private void OpenHeart()
	{
		this.state = FlyingBirdLevelBird.State.Heart;
		this.heart.StartHeartAttack();
		base.GetComponent<DamageReceiver>().enabled = false;
		this.heartSpitFX.SetActive(true);
		base.StartCoroutine(this.checkHeart_cr());
	}

	// Token: 0x06001FB0 RID: 8112 RVA: 0x00121B48 File Offset: 0x0011FF48
	private IEnumerator checkHeart_cr()
	{
		while (this.heart.gameObject.activeSelf)
		{
			yield return null;
		}
		base.animator.SetBool("OnRegurgitate", false);
		AudioManager.Play("level_flyingbird_stretcher_regurgitate_end");
		this.emitAudioFromObject.Add("level_flyingbird_stretcher_regurgitate_end");
		base.GetComponent<DamageReceiver>().enabled = true;
		yield return base.animator.WaitForAnimationToEnd(this, "Regurgitate_End", false, true);
		this.state = FlyingBirdLevelBird.State.HeartTrans;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.heart.hesitate.RandomFloat());
		this.heartSpitFX.SetActive(false);
		this.state = FlyingBirdLevelBird.State.Revived;
		yield break;
	}

	// Token: 0x06001FB1 RID: 8113 RVA: 0x00121B64 File Offset: 0x0011FF64
	private void NursesHeartHeight()
	{
		foreach (Transform transform in this.nurses.nurses)
		{
			transform.transform.localPosition = new Vector3(transform.transform.localPosition.x, transform.transform.localPosition.y + 8f);
		}
	}

	// Token: 0x06001FB2 RID: 8114 RVA: 0x00121BD4 File Offset: 0x0011FFD4
	private void NursesGarbageHeight()
	{
		foreach (Transform transform in this.nurses.nurses)
		{
			transform.transform.localPosition = new Vector3(transform.transform.localPosition.x, transform.transform.localPosition.y + 6f);
		}
	}

	// Token: 0x06001FB3 RID: 8115 RVA: 0x00121C44 File Offset: 0x00120044
	private void NursesReset()
	{
		foreach (Transform transform in this.nurses.nurses)
		{
			transform.transform.localPosition = Vector3.zero;
		}
	}

	// Token: 0x0400280F RID: 10255
	[SerializeField]
	private GameObject bootPrefab;

	// Token: 0x04002810 RID: 10256
	[SerializeField]
	private GameObject bootPinkPrefab;

	// Token: 0x04002811 RID: 10257
	[SerializeField]
	private GameObject fishPrefab;

	// Token: 0x04002812 RID: 10258
	[SerializeField]
	private GameObject applePrefab;

	// Token: 0x04002813 RID: 10259
	[Space(10f)]
	[SerializeField]
	private FlyingBirdLevelSmallBird smallBird;

	// Token: 0x04002814 RID: 10260
	[Space(10f)]
	[SerializeField]
	private FlyingBirdLevelBirdFeather featherPrefab;

	// Token: 0x04002815 RID: 10261
	[Space(10f)]
	[SerializeField]
	private Transform eggRoot;

	// Token: 0x04002816 RID: 10262
	[SerializeField]
	private FlyingBirdLevelBirdEgg eggPrefab;

	// Token: 0x04002817 RID: 10263
	[SerializeField]
	private GameObject deathParts;

	// Token: 0x04002818 RID: 10264
	[Space(10f)]
	[SerializeField]
	private Transform nurse1Root;

	// Token: 0x04002819 RID: 10265
	[SerializeField]
	private Transform nurse2Root;

	// Token: 0x0400281A RID: 10266
	[SerializeField]
	private Transform garbageRoot;

	// Token: 0x0400281B RID: 10267
	[SerializeField]
	private FlyingBirdLevelHeart heart;

	// Token: 0x0400281C RID: 10268
	[SerializeField]
	private GameObject heartSpitFX;

	// Token: 0x0400281D RID: 10269
	[SerializeField]
	private FlyingBirdLevelNurses nurses;

	// Token: 0x0400281E RID: 10270
	[SerializeField]
	private GameObject head;

	// Token: 0x0400281F RID: 10271
	private DamageDealer damageDealer;

	// Token: 0x04002820 RID: 10272
	private DamageReceiver damageReceiver;

	// Token: 0x04002821 RID: 10273
	private bool introEnded;

	// Token: 0x04002822 RID: 10274
	private bool feathersFirstTime;

	// Token: 0x04002823 RID: 10275
	private Transform houseCollider;

	// Token: 0x04002824 RID: 10276
	private Coroutine patternCoroutine;

	// Token: 0x04002825 RID: 10277
	private int garbageIndex;

	// Token: 0x04002826 RID: 10278
	private int typeIndex;

	// Token: 0x04002827 RID: 10279
	private int blinks;

	// Token: 0x04002828 RID: 10280
	private int maxBlinks = 6;

	// Token: 0x04002829 RID: 10281
	[Space(10f)]
	[SerializeField]
	private Transform[] laserRoots;

	// Token: 0x0400282A RID: 10282
	[SerializeField]
	private BasicProjectile laserPrefab;

	// Token: 0x0400282B RID: 10283
	[SerializeField]
	private Effect laserEffect;

	// Token: 0x0400282C RID: 10284
	[Space(10f)]
	[SerializeField]
	private Transform deathEffectsRoot;

	// Token: 0x0400282D RID: 10285
	[SerializeField]
	private Effect deathEffectFront;

	// Token: 0x0400282E RID: 10286
	[SerializeField]
	private Effect deathEffectBack;

	// Token: 0x02000617 RID: 1559
	public enum State
	{
		// Token: 0x04002830 RID: 10288
		Init,
		// Token: 0x04002831 RID: 10289
		Idle,
		// Token: 0x04002832 RID: 10290
		Feathers,
		// Token: 0x04002833 RID: 10291
		Eggs,
		// Token: 0x04002834 RID: 10292
		Dying,
		// Token: 0x04002835 RID: 10293
		Dead,
		// Token: 0x04002836 RID: 10294
		Whistle,
		// Token: 0x04002837 RID: 10295
		Lasers,
		// Token: 0x04002838 RID: 10296
		LasersEnding,
		// Token: 0x04002839 RID: 10297
		Reviving,
		// Token: 0x0400283A RID: 10298
		Revived,
		// Token: 0x0400283B RID: 10299
		Garbage,
		// Token: 0x0400283C RID: 10300
		Heart,
		// Token: 0x0400283D RID: 10301
		HeartTrans
	}
}
