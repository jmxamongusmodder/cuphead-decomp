using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000692 RID: 1682
public class FlyingMermaidLevelMermaid : LevelProperties.FlyingMermaid.Entity
{
	// Token: 0x06002390 RID: 9104 RVA: 0x0014DCDC File Offset: 0x0014C0DC
	public FlyingMermaidLevelMermaid()
	{
		FlyingMermaidLevelMermaid.FishPossibility[] array = new FlyingMermaidLevelMermaid.FishPossibility[2];
		array[0] = FlyingMermaidLevelMermaid.FishPossibility.Homer;
		this.fishPattern = array;
		this.maxBlinks = 3;
		base..ctor();
	}

	// Token: 0x170003A3 RID: 931
	// (get) Token: 0x06002391 RID: 9105 RVA: 0x0014DD0F File Offset: 0x0014C10F
	// (set) Token: 0x06002392 RID: 9106 RVA: 0x0014DD17 File Offset: 0x0014C117
	public FlyingMermaidLevelMermaid.State state { get; private set; }

	// Token: 0x06002393 RID: 9107 RVA: 0x0014DD20 File Offset: 0x0014C120
	protected override void Awake()
	{
		base.Awake();
		this.summonPattern.Shuffle<FlyingMermaidLevelMermaid.SummonPossibility>();
		this.fishPattern.Shuffle<FlyingMermaidLevelMermaid.FishPossibility>();
		base.StartCoroutine(this.intro_cr());
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		CollisionChild collisionChild = this.blockingColliders.gameObject.AddComponent<CollisionChild>();
		collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
	}

	// Token: 0x06002394 RID: 9108 RVA: 0x0014DDA8 File Offset: 0x0014C1A8
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06002395 RID: 9109 RVA: 0x0014DDBC File Offset: 0x0014C1BC
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		if (!this.stopMoving)
		{
			if (this.introEnded && !this.transformationStarting)
			{
				float num = Mathf.Max(PlayerManager.GetNext().center.x, PlayerManager.GetNext().center.x);
				if (num > base.transform.position.x)
				{
					this.Position(true);
				}
				else
				{
					this.Position(false);
				}
			}
			else if (this.transformationStarting)
			{
				this.Position(false);
			}
		}
	}

	// Token: 0x06002396 RID: 9110 RVA: 0x0014DE6D File Offset: 0x0014C26D
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002397 RID: 9111 RVA: 0x0014DE8C File Offset: 0x0014C28C
	public override void LevelInit(LevelProperties.FlyingMermaid properties)
	{
		base.LevelInit(properties);
		this.initFishPatternIndices();
		this.spreadFishPinkPattern = properties.CurrentState.spreadshotFish.spreadshotPinkString.Split(new char[]
		{
			','
		});
		this.spreadFishPinkIndex = UnityEngine.Random.Range(0, this.spreadFishPinkPattern.Length);
	}

	// Token: 0x06002398 RID: 9112 RVA: 0x0014DEE0 File Offset: 0x0014C2E0
	private void Position(bool closeGap)
	{
		this.walkDuration = (float)((!this.transformationStarting) ? 4 : 2);
		if (closeGap)
		{
			float x = this.walkingPositions[0].position.x;
			float x2 = this.walkingPositions[1].position.x;
			this.Move(x, x2, this.walkDuration, 1);
		}
		else
		{
			float x = this.walkingPositions[1].position.x;
			float x2 = this.walkingPositions[0].position.x;
			this.Move(x, x2, this.walkDuration, -1);
		}
	}

	// Token: 0x06002399 RID: 9113 RVA: 0x0014DF8C File Offset: 0x0014C38C
	private void Move(float startPosition, float endPosition, float duration, int direction)
	{
		this.walkTime += CupheadTime.Delta * (float)direction;
		if (direction < 0)
		{
			if (this.walkTime <= 0f)
			{
				this.walkTime = 0f;
			}
		}
		else if (this.walkTime >= duration)
		{
			this.walkTime = duration;
		}
		this.walkPCT = this.walkTime / duration;
		if (this.walkPCT >= 1f)
		{
			this.walkPCT = 1f;
		}
		if (direction < 0)
		{
			this.walkPCT = 1f - this.walkPCT;
		}
		base.transform.SetPosition(new float?(startPosition + (endPosition - startPosition) * this.walkPCT), null, null);
	}

	// Token: 0x0600239A RID: 9114 RVA: 0x0014E060 File Offset: 0x0014C460
	private void PlayIntroSound()
	{
		AudioManager.Play("level_mermaid_intro");
		this.emitAudioFromObject.Add("level_mermaid_intro");
	}

	// Token: 0x0600239B RID: 9115 RVA: 0x0014E07C File Offset: 0x0014C47C
	private IEnumerator intro_cr()
	{
		float t = 0f;
		base.transform.SetPosition(null, new float?(this.startUnderwaterY), null);
		yield return CupheadTime.WaitForSeconds(this, this.introRiseTime * 0.5f);
		base.StartCoroutine(this.spawn_splash_cr());
		while (t < this.introRiseTime * 0.5f)
		{
			t += CupheadTime.Delta;
			base.transform.SetPosition(null, new float?(Mathf.Lerp(this.startUnderwaterY, this.regularY, t / (this.introRiseTime * 0.5f))), null);
			yield return null;
		}
		base.transform.SetPosition(null, new float?(this.regularY), null);
		while (!this.introEnded)
		{
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.state = FlyingMermaidLevelMermaid.State.Idle;
		yield break;
	}

	// Token: 0x0600239C RID: 9116 RVA: 0x0014E098 File Offset: 0x0014C498
	private IEnumerator spawn_splash_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.35f);
		FlyingMermaidLevelSplashManager.Instance.SpawnMegaSplashMedium(base.gameObject, -50f, true, -200f);
		yield return null;
		yield break;
	}

	// Token: 0x0600239D RID: 9117 RVA: 0x0014E0B4 File Offset: 0x0014C4B4
	public void IntroContinue()
	{
		Animator component = base.GetComponent<Animator>();
		component.SetTrigger("Continue");
		this.state = FlyingMermaidLevelMermaid.State.Intro;
	}

	// Token: 0x0600239E RID: 9118 RVA: 0x0014E0DA File Offset: 0x0014C4DA
	private void OnIntroAnimComplete()
	{
		this.introEnded = true;
	}

	// Token: 0x0600239F RID: 9119 RVA: 0x0014E0E4 File Offset: 0x0014C4E4
	private void BlinkMaybe()
	{
		this.blinks++;
		if (this.blinks >= this.maxBlinks)
		{
			this.blinks = 0;
			this.maxBlinks = UnityEngine.Random.Range(2, 5);
			this.blinkOverlaySprite.enabled = true;
		}
		else
		{
			this.blinkOverlaySprite.enabled = false;
		}
	}

	// Token: 0x060023A0 RID: 9120 RVA: 0x0014E141 File Offset: 0x0014C541
	public void StartYell()
	{
		this.state = FlyingMermaidLevelMermaid.State.Yell;
		base.StartCoroutine(this.yell_cr());
	}

	// Token: 0x060023A1 RID: 9121 RVA: 0x0014E158 File Offset: 0x0014C558
	private IEnumerator yell_cr()
	{
		LevelProperties.FlyingMermaid.Yell p = base.properties.CurrentState.yell;
		string[] pattern = p.patternString.GetRandom<string>().Split(new char[]
		{
			','
		});
		base.animator.SetTrigger("StartYell");
		base.animator.SetBool("Repeat", true);
		yield return base.animator.WaitForAnimationToEnd(this, "Yell_Start", false, true);
		float waitTime = p.anticipateInitialHold;
		for (int i = 0; i < pattern.Length; i++)
		{
			if (pattern[i][0] == 'D')
			{
				Parser.FloatTryParse(pattern[i].Substring(1), out waitTime);
			}
			else
			{
				int repeatTimes = 0;
				Parser.IntTryParse(pattern[i].Substring(1), out repeatTimes);
				for (int j = 0; j < repeatTimes; j++)
				{
					yield return CupheadTime.WaitForSeconds(this, waitTime);
					base.animator.SetTrigger("Continue");
					yield return base.animator.WaitForAnimationToEnd(this, "Yell_Anticipation_End", false, true);
					this.FireProjectiles();
					this.yellEffect.Create(this.yellFxRoot.position);
					yield return CupheadTime.WaitForSeconds(this, p.mouthHold);
					base.animator.SetTrigger("Continue");
					waitTime = p.anticipateHold;
					if (i < pattern.Length - 1 || j < repeatTimes - 1)
					{
						yield return base.animator.WaitForAnimationToEnd(this, "Yell_Back", false, true);
					}
				}
			}
		}
		base.animator.SetBool("Repeat", false);
		yield return base.animator.WaitForAnimationToEnd(this, "Yell_End", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.hesitateAfterAttack);
		this.state = FlyingMermaidLevelMermaid.State.Idle;
		yield break;
	}

	// Token: 0x060023A2 RID: 9122 RVA: 0x0014E174 File Offset: 0x0014C574
	private void FireProjectiles()
	{
		LevelProperties.FlyingMermaid.Yell yell = base.properties.CurrentState.yell;
		AbstractPlayerController next = PlayerManager.GetNext();
		for (int i = 0; i < yell.numBullets; i++)
		{
			float floatAt = yell.spreadAngle.GetFloatAt((float)i / ((float)yell.numBullets - 1f));
			FlyingMermaidLevelYellProjectile flyingMermaidLevelYellProjectile = this.yellProjectilePrefab.Create(this.projectileRoot.position, yell.bulletSpeed, floatAt, next);
			flyingMermaidLevelYellProjectile.animator.SetInteger("Variant", i);
		}
	}

	// Token: 0x060023A3 RID: 9123 RVA: 0x0014E202 File Offset: 0x0014C602
	public void StartSummon()
	{
		this.state = FlyingMermaidLevelMermaid.State.Summon;
		base.StartCoroutine(this.summon_cr());
	}

	// Token: 0x060023A4 RID: 9124 RVA: 0x0014E218 File Offset: 0x0014C618
	private IEnumerator summon_cr()
	{
		LevelProperties.FlyingMermaid.Summon p = base.properties.CurrentState.summon;
		base.animator.SetBool("Summon", true);
		yield return base.animator.WaitForAnimationToEnd(this, "Summon_Start", false, true);
		AudioManager.Play("level_mermaid_summon_loop_start");
		yield return CupheadTime.WaitForSeconds(this, p.holdBeforeCreature);
		FlyingMermaidLevelMermaid.SummonPossibility summon = this.nextSummon();
		AudioManager.Play("level_mermaid_summon_loop");
		if (summon != FlyingMermaidLevelMermaid.SummonPossibility.Seahorse)
		{
			if (summon != FlyingMermaidLevelMermaid.SummonPossibility.Pufferfish)
			{
				if (summon == FlyingMermaidLevelMermaid.SummonPossibility.Turtle)
				{
					this.SummonTurtle();
				}
			}
			else
			{
				AudioManager.Play("level_mermaid_merdusa_puffer_fish_bubble_up");
				base.StartCoroutine(this.summonPufferFish_cr());
			}
		}
		else
		{
			this.SummonSeahorse();
		}
		yield return CupheadTime.WaitForSeconds(this, p.holdAfterCreature);
		AudioManager.Stop("level_mermaid_summon_loop");
		AudioManager.Play("level_mermaid_summon_loop_end");
		base.animator.SetBool("Summon", false);
		yield return base.animator.WaitForAnimationToEnd(this, "Summon_End", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.hesitateAfterAttack);
		this.state = FlyingMermaidLevelMermaid.State.Idle;
		yield break;
	}

	// Token: 0x060023A5 RID: 9125 RVA: 0x0014E233 File Offset: 0x0014C633
	private FlyingMermaidLevelMermaid.SummonPossibility nextSummon()
	{
		this.summonIndex = (this.summonIndex + 1) % this.summonPattern.Length;
		return this.summonPattern[this.summonIndex];
	}

	// Token: 0x060023A6 RID: 9126 RVA: 0x0014E25C File Offset: 0x0014C65C
	private IEnumerator summonPufferFish_cr()
	{
		LevelProperties.FlyingMermaid.Pufferfish p = base.properties.CurrentState.pufferfish;
		string[] pattern = p.spawnString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int i = UnityEngine.Random.Range(0, pattern.Length);
		float t = 0f;
		float waitTime = 0f;
		int spawnsUntilPinkPufferfish = p.pinkPufferSpawnRange.RandomInt();
		while (t < p.spawnDuration && !this.stopPufferfish)
		{
			if (pattern[i][0] == 'D')
			{
				Parser.FloatTryParse(pattern[i].Substring(1), out waitTime);
			}
			else
			{
				if (waitTime > 0f)
				{
					yield return CupheadTime.WaitForSeconds(this, waitTime);
					t += waitTime;
				}
				string[] spawnLocations = pattern[i].Split(new char[]
				{
					'-'
				});
				foreach (string s in spawnLocations)
				{
					float x = 0f;
					Parser.FloatTryParse(s, out x);
					spawnsUntilPinkPufferfish--;
					FlyingMermaidLevelPufferfish prefab;
					if (spawnsUntilPinkPufferfish == 0)
					{
						spawnsUntilPinkPufferfish = p.pinkPufferSpawnRange.RandomInt();
						prefab = this.pinkPufferfishPrefab;
					}
					else
					{
						prefab = this.pufferfishPrefabs[UnityEngine.Random.Range(0, this.pufferfishPrefabs.Length)];
					}
					base.StartCoroutine(this.summon_pufferfish_cr(prefab, x));
				}
				waitTime = p.delay;
			}
			i = (i + 1) % pattern.Length;
		}
		yield break;
	}

	// Token: 0x060023A7 RID: 9127 RVA: 0x0014E278 File Offset: 0x0014C678
	private void SummonSeahorse()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		FlyingMermaidLevelSeahorse flyingMermaidLevelSeahorse = UnityEngine.Object.Instantiate<FlyingMermaidLevelSeahorse>(this.seahorsePrefab);
		Vector2 v = flyingMermaidLevelSeahorse.transform.position;
		v.x = next.transform.position.x;
		flyingMermaidLevelSeahorse.transform.position = v;
		flyingMermaidLevelSeahorse.Init(base.properties.CurrentState.seahorse);
		GroundHomingMovement component = flyingMermaidLevelSeahorse.GetComponent<GroundHomingMovement>();
		component.TrackingPlayer = next;
	}

	// Token: 0x060023A8 RID: 9128 RVA: 0x0014E2FC File Offset: 0x0014C6FC
	private void SummonTurtle()
	{
		FlyingMermaidLevelTurtle flyingMermaidLevelTurtle = UnityEngine.Object.Instantiate<FlyingMermaidLevelTurtle>(this.turtlePrefab);
		Vector2 v = flyingMermaidLevelTurtle.transform.position;
		v.x = (float)Level.Current.Left + base.properties.CurrentState.turtle.appearPosition.RandomFloat();
		flyingMermaidLevelTurtle.transform.position = v;
		flyingMermaidLevelTurtle.Init(base.properties.CurrentState.turtle);
	}

	// Token: 0x060023A9 RID: 9129 RVA: 0x0014E37C File Offset: 0x0014C77C
	private IEnumerator summon_pufferfish_cr(FlyingMermaidLevelPufferfish prefab, float x)
	{
		yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0f, 0.15f));
		FlyingMermaidLevelPufferfish pufferfish = UnityEngine.Object.Instantiate<FlyingMermaidLevelPufferfish>(prefab);
		Vector2 position = pufferfish.transform.position;
		position.x = x + (float)Level.Current.Left;
		pufferfish.transform.position = position;
		pufferfish.Init(base.properties.CurrentState.pufferfish);
		yield break;
	}

	// Token: 0x060023AA RID: 9130 RVA: 0x0014E3A5 File Offset: 0x0014C7A5
	public void StartFish()
	{
		base.StartCoroutine(this.fish_cr());
	}

	// Token: 0x060023AB RID: 9131 RVA: 0x0014E3B4 File Offset: 0x0014C7B4
	private void PlayMermaidTuckdownSound()
	{
		AudioManager.Play("level_mermaid_tuckdown_laugh");
		this.emitAudioFromObject.Add("level_mermaid_tuckdown_laugh");
	}

	// Token: 0x060023AC RID: 9132 RVA: 0x0014E3D0 File Offset: 0x0014C7D0
	private IEnumerator fish_cr()
	{
		this.state = FlyingMermaidLevelMermaid.State.Fish;
		base.animator.SetTrigger("StartFish");
		yield return base.animator.WaitForAnimationToEnd(this, "Tuckdown_Start", false, true);
		float t = 0f;
		FlyingMermaidLevelSplashManager.Instance.SpawnMegaSplashLarge(base.gameObject, 0f, false, 0f);
		while (t < this.tuckdownMoveTime)
		{
			t += CupheadTime.Delta;
			base.transform.SetPosition(null, new float?(Mathf.Lerp(this.regularY, this.fishUnderwaterY, t / this.tuckdownMoveTime)), null);
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, this.tuckdownWaitTime);
		this.fish = this.nextFish();
		this.spreadshotFishSprite.enabled = (this.fish == FlyingMermaidLevelMermaid.FishPossibility.Spreadshot);
		this.spreadshotFishOverlaySprite.enabled = (this.fish == FlyingMermaidLevelMermaid.FishPossibility.Spreadshot);
		this.spinnerFishSprite.enabled = (this.fish == FlyingMermaidLevelMermaid.FishPossibility.Spinner);
		this.spinnerFishOverlaySprite.enabled = (this.fish == FlyingMermaidLevelMermaid.FishPossibility.Spinner);
		this.homerFishSprite.enabled = (this.fish == FlyingMermaidLevelMermaid.FishPossibility.Homer);
		this.homerFishOverlaySprite.enabled = (this.fish == FlyingMermaidLevelMermaid.FishPossibility.Homer);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Tuckdown_Loop", false, true);
		t = 0f;
		FlyingMermaidLevelSplashManager.Instance.SpawnMegaSplashLarge(base.gameObject, 50f, true, 0f);
		while (t < this.tuckdownRiseTime)
		{
			t += CupheadTime.Delta;
			base.transform.SetPosition(null, new float?(Mathf.Lerp(this.fishUnderwaterY, this.regularY, t / this.tuckdownRiseTime)), null);
			yield return null;
		}
		base.animator.SetBool("Repeat", true);
		string[] pattern = this.nextFishPatternString().Split(new char[]
		{
			','
		});
		float waitTime = base.properties.CurrentState.fish.delayBeforeFirstAttack;
		for (int i = 0; i < pattern.Length; i++)
		{
			if (pattern[i][0] == 'D')
			{
				Parser.FloatTryParse(pattern[i].Substring(1), out waitTime);
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, waitTime);
				base.animator.SetTrigger("Continue");
				yield return base.animator.WaitForAnimationToEnd(this, "Fish_Attack_Start", false, true);
				this.doFishAttack(pattern[i]);
				if (i < pattern.Length - 1)
				{
					yield return base.animator.WaitForAnimationToEnd(this, "Fish_Attack_Repeat", false, true);
					waitTime = this.waitTimeBetweenFishAttacks();
				}
			}
		}
		base.animator.SetBool("Repeat", false);
		yield return base.animator.WaitForAnimationToEnd(this, "Fish_Attack", false, true);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.fish.delayBeforeFly);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Fish_Launch", false, true);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.fish.hesitateAfterAttack);
		this.state = FlyingMermaidLevelMermaid.State.Idle;
		yield break;
	}

	// Token: 0x060023AD RID: 9133 RVA: 0x0014E3EB File Offset: 0x0014C7EB
	private FlyingMermaidLevelMermaid.FishPossibility nextFish()
	{
		this.fishIndex = (this.fishIndex + 1) % this.fishPattern.Length;
		return this.fishPattern[this.fishIndex];
	}

	// Token: 0x060023AE RID: 9134 RVA: 0x0014E414 File Offset: 0x0014C814
	private void initFishPatternIndices()
	{
		this.spreadshotPatternIndex = UnityEngine.Random.Range(0, base.properties.CurrentState.spreadshotFish.shootString.Length);
		this.spinnerPatternIndex = UnityEngine.Random.Range(0, base.properties.CurrentState.spinnerFish.shootString.Length);
		this.homerPatternIndex = UnityEngine.Random.Range(0, base.properties.CurrentState.homerFish.shootString.Length);
	}

	// Token: 0x060023AF RID: 9135 RVA: 0x0014E48C File Offset: 0x0014C88C
	private string nextFishPatternString()
	{
		switch (this.fish)
		{
		case FlyingMermaidLevelMermaid.FishPossibility.Spreadshot:
			this.spreadshotPatternIndex = (this.spreadshotPatternIndex + 1) % base.properties.CurrentState.spreadshotFish.shootString.Length;
			return base.properties.CurrentState.spreadshotFish.shootString[this.spreadshotPatternIndex];
		case FlyingMermaidLevelMermaid.FishPossibility.Spinner:
			this.spinnerPatternIndex = (this.spinnerPatternIndex + 1) % base.properties.CurrentState.spinnerFish.shootString.Length;
			return base.properties.CurrentState.spinnerFish.shootString[this.spinnerPatternIndex];
		case FlyingMermaidLevelMermaid.FishPossibility.Homer:
			this.homerPatternIndex = (this.homerPatternIndex + 1) % base.properties.CurrentState.homerFish.shootString.Length;
			return base.properties.CurrentState.homerFish.shootString[this.homerPatternIndex];
		default:
			return string.Empty;
		}
	}

	// Token: 0x060023B0 RID: 9136 RVA: 0x0014E588 File Offset: 0x0014C988
	private float waitTimeBetweenFishAttacks()
	{
		switch (this.fish)
		{
		case FlyingMermaidLevelMermaid.FishPossibility.Spreadshot:
			return base.properties.CurrentState.spreadshotFish.attackDelay;
		case FlyingMermaidLevelMermaid.FishPossibility.Spinner:
			return base.properties.CurrentState.spinnerFish.attackDelay;
		case FlyingMermaidLevelMermaid.FishPossibility.Homer:
			return base.properties.CurrentState.homerFish.attackDelay;
		default:
			return 0f;
		}
	}

	// Token: 0x060023B1 RID: 9137 RVA: 0x0014E5FC File Offset: 0x0014C9FC
	private void doFishAttack(string attackString)
	{
		AudioManager.Play("level_mermaid_fish_attack");
		this.emitAudioFromObject.Add("level_mermaid_fish_attack");
		FlyingMermaidLevelMermaid.FishPossibility fishPossibility = this.fish;
		if (fishPossibility != FlyingMermaidLevelMermaid.FishPossibility.Spreadshot)
		{
			if (fishPossibility != FlyingMermaidLevelMermaid.FishPossibility.Spinner)
			{
				if (fishPossibility == FlyingMermaidLevelMermaid.FishPossibility.Homer)
				{
					this.fishHomer();
				}
			}
			else
			{
				this.fishSpinner();
			}
		}
		else
		{
			this.fishSpreadshot(attackString);
		}
	}

	// Token: 0x060023B2 RID: 9138 RVA: 0x0014E668 File Offset: 0x0014CA68
	private void fishSpreadshot(string attackString)
	{
		int num = 0;
		Parser.IntTryParse(attackString.Substring(1), out num);
		num--;
		string[] array = base.properties.CurrentState.spreadshotFish.spreadVariableGroups[num].Split(new char[]
		{
			','
		});
		float speed = 0f;
		int num2 = 0;
		MinMax minMax = new MinMax(0f, 0f);
		foreach (string text in array)
		{
			if (text[0] == 'S')
			{
				Parser.FloatTryParse(text.Substring(1), out speed);
			}
			else if (text[0] == 'N')
			{
				Parser.IntTryParse(text.Substring(1), out num2);
			}
			else
			{
				string[] array3 = text.Split(new char[]
				{
					'-'
				});
				Parser.FloatTryParse(array3[0], out minMax.min);
				Parser.FloatTryParse(array3[1], out minMax.max);
			}
		}
		for (int j = 0; j < num2; j++)
		{
			float floatAt = minMax.GetFloatAt((float)j / ((float)num2 - 1f));
			BasicProjectile basicProjectile = this.fishSpreadshotBulletPrefab.Create(this.fishProjectileRoot.position, floatAt, speed);
			basicProjectile.animator.SetInteger("Variant", j % 2);
			basicProjectile.SetParryable(this.spreadFishPinkPattern[this.spreadFishPinkIndex][0] == 'P');
			this.spreadFishPinkIndex = (this.spreadFishPinkIndex + 1) % this.spreadFishPinkPattern.Length;
		}
	}

	// Token: 0x060023B3 RID: 9139 RVA: 0x0014E804 File Offset: 0x0014CC04
	private void fishSpinner()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		Vector2 direction = next.transform.position - this.fishProjectileRoot.position;
		direction.Normalize();
		if (next.transform.position.x > this.fishProjectileRoot.transform.position.x)
		{
			direction = MathUtils.AngleToDirection(90f);
		}
		this.fishSpinnerBulletPrefab.Create(this.fishProjectileRoot.position, direction, base.properties.CurrentState.spinnerFish);
	}

	// Token: 0x060023B4 RID: 9140 RVA: 0x0014E8A8 File Offset: 0x0014CCA8
	private void fishHomer()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		Vector2 direction = next.transform.position - this.fishProjectileRoot.position;
		float rotation = MathUtils.DirectionToAngle(direction) + UnityEngine.Random.Range(-15f, 15f);
		LevelProperties.FlyingMermaid.HomerFish homerFish = base.properties.CurrentState.homerFish;
		if (next.transform.position.x > this.fishProjectileRoot.transform.position.x)
		{
			rotation = 90f;
		}
		this.fishHomerBulletPrefab.Create(this.fishProjectileRoot.position, rotation, next, homerFish);
	}

	// Token: 0x060023B5 RID: 9141 RVA: 0x0014E958 File Offset: 0x0014CD58
	public void LaunchFish()
	{
		FlyingMermaidLevelFish flyingMermaidLevelFish = null;
		FlyingMermaidLevelMermaid.FishPossibility fishPossibility = this.fish;
		if (fishPossibility != FlyingMermaidLevelMermaid.FishPossibility.Spreadshot)
		{
			if (fishPossibility != FlyingMermaidLevelMermaid.FishPossibility.Spinner)
			{
				if (fishPossibility == FlyingMermaidLevelMermaid.FishPossibility.Homer)
				{
					flyingMermaidLevelFish = this.homerFishPrefab;
				}
			}
			else
			{
				flyingMermaidLevelFish = this.spinnerFishPrefab;
			}
		}
		else
		{
			flyingMermaidLevelFish = this.spreadshotFishPrefab;
		}
		flyingMermaidLevelFish.Create(this.fishLaunchRoot.position, base.properties.CurrentState.fish);
	}

	// Token: 0x060023B6 RID: 9142 RVA: 0x0014E9D2 File Offset: 0x0014CDD2
	private void OnFishSpitFx()
	{
		this.FishSpitEffectPrefab.Create(this.fishProjectileRoot.position);
	}

	// Token: 0x060023B7 RID: 9143 RVA: 0x0014E9EB File Offset: 0x0014CDEB
	public void StartTransform()
	{
		this.transformationStarting = true;
		base.StartCoroutine(this.transform_cr());
	}

	// Token: 0x060023B8 RID: 9144 RVA: 0x0014EA04 File Offset: 0x0014CE04
	private IEnumerator transform_cr()
	{
		while (base.transform.position.x != this.walkingPositions[0].position.x)
		{
			yield return null;
		}
		this.stopMoving = true;
		float startX = base.transform.position.x;
		float t = 0f;
		while (t < this.transformMoveTime)
		{
			t += CupheadTime.Delta;
			base.transform.SetPosition(new float?(Mathf.Lerp(startX, startX - this.transformMoveX, t / this.transformMoveTime)), null, null);
			yield return null;
		}
		base.animator.SetTrigger("Transform");
		if (this.state == FlyingMermaidLevelMermaid.State.Summon)
		{
			yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
			this.stopPufferfish = true;
		}
		if (this.state == FlyingMermaidLevelMermaid.State.Idle)
		{
			this.stopPufferfish = true;
		}
		this.state = FlyingMermaidLevelMermaid.State.Transform;
		yield return base.animator.WaitForAnimationToStart(this, "Transform", false);
		AudioManager.Play("level_mermaid_transform");
		((FlyingMermaidLevel)Level.Current).MerdusaTransformStarted = true;
		this.stopPufferfish = true;
		yield return base.animator.WaitForAnimationToEnd(this, "Transform", false, true);
		t = 0f;
		while (t < this.eelSinkTime)
		{
			t += CupheadTime.Delta;
			base.transform.SetPosition(null, new float?(Mathf.Lerp(this.regularY, this.eelUnderwaterY, t / this.eelSinkTime)), null);
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060023B9 RID: 9145 RVA: 0x0014EA20 File Offset: 0x0014CE20
	public void DisableColliders()
	{
		Collider2D[] components = base.GetComponents<Collider2D>();
		foreach (Collider2D collider2D in components)
		{
			collider2D.enabled = false;
		}
		this.blockingColliders.gameObject.SetActive(false);
	}

	// Token: 0x060023BA RID: 9146 RVA: 0x0014EA66 File Offset: 0x0014CE66
	public void SpawnMerdusa()
	{
		this.merdusa.StartIntro(base.transform.position);
	}

	// Token: 0x060023BB RID: 9147 RVA: 0x0014EA83 File Offset: 0x0014CE83
	private void RightSplash()
	{
		this.splashRight.Create(this.splashRoot.transform.position);
	}

	// Token: 0x060023BC RID: 9148 RVA: 0x0014EAA1 File Offset: 0x0014CEA1
	private void LeftSplash()
	{
		this.splashLeft.Create(this.splashRoot.transform.position);
	}

	// Token: 0x060023BD RID: 9149 RVA: 0x0014EABF File Offset: 0x0014CEBF
	private void SoundMermaidFishLaunch()
	{
		AudioManager.Play("level_mermaid_fish_launch");
		this.emitAudioFromObject.Add("level_mermaid_fish_launch");
	}

	// Token: 0x060023BE RID: 9150 RVA: 0x0014EADB File Offset: 0x0014CEDB
	private void SoundMermaidAttackYellStart()
	{
		AudioManager.Play("level_mermaid_yell_start");
		this.emitAudioFromObject.Add("level_mermaid_yell_start");
	}

	// Token: 0x060023BF RID: 9151 RVA: 0x0014EAF7 File Offset: 0x0014CEF7
	private void SoundMermaidAttackYell()
	{
		AudioManager.Play("level_mermaid_yell_attack");
		this.emitAudioFromObject.Add("level_mermaid_yell_attack");
	}

	// Token: 0x04002C38 RID: 11320
	[SerializeField]
	private Transform[] walkingPositions;

	// Token: 0x04002C39 RID: 11321
	[SerializeField]
	private float introRiseTime;

	// Token: 0x04002C3A RID: 11322
	[SerializeField]
	private float tuckdownMoveTime;

	// Token: 0x04002C3B RID: 11323
	[SerializeField]
	private float tuckdownWaitTime;

	// Token: 0x04002C3C RID: 11324
	[SerializeField]
	private float tuckdownRiseTime;

	// Token: 0x04002C3D RID: 11325
	[SerializeField]
	private float regularY;

	// Token: 0x04002C3E RID: 11326
	[SerializeField]
	private float startUnderwaterY;

	// Token: 0x04002C3F RID: 11327
	[SerializeField]
	private float fishUnderwaterY;

	// Token: 0x04002C40 RID: 11328
	[SerializeField]
	private float transformMoveTime;

	// Token: 0x04002C41 RID: 11329
	[SerializeField]
	private float transformMoveX;

	// Token: 0x04002C42 RID: 11330
	[SerializeField]
	private float eelSinkTime;

	// Token: 0x04002C43 RID: 11331
	[SerializeField]
	private float eelUnderwaterY;

	// Token: 0x04002C44 RID: 11332
	[SerializeField]
	private FlyingMermaidLevelYellProjectile yellProjectilePrefab;

	// Token: 0x04002C45 RID: 11333
	[SerializeField]
	private FlyingMermaidLevelSeahorse seahorsePrefab;

	// Token: 0x04002C46 RID: 11334
	[SerializeField]
	private Effect FishSpitEffectPrefab;

	// Token: 0x04002C47 RID: 11335
	private bool introEnded;

	// Token: 0x04002C48 RID: 11336
	private DamageDealer damageDealer;

	// Token: 0x04002C49 RID: 11337
	private DamageReceiver damageReceiver;

	// Token: 0x04002C4A RID: 11338
	[SerializeField]
	private Transform projectileRoot;

	// Token: 0x04002C4B RID: 11339
	[SerializeField]
	private Transform yellFxRoot;

	// Token: 0x04002C4C RID: 11340
	[SerializeField]
	private FlyingMermaidLevelPufferfish[] pufferfishPrefabs;

	// Token: 0x04002C4D RID: 11341
	[SerializeField]
	private FlyingMermaidLevelPufferfish pinkPufferfishPrefab;

	// Token: 0x04002C4E RID: 11342
	[SerializeField]
	private FlyingMermaidLevelTurtle turtlePrefab;

	// Token: 0x04002C4F RID: 11343
	[SerializeField]
	private SpriteRenderer blinkOverlaySprite;

	// Token: 0x04002C50 RID: 11344
	[SerializeField]
	private SpriteRenderer spreadshotFishSprite;

	// Token: 0x04002C51 RID: 11345
	[SerializeField]
	private SpriteRenderer spinnerFishSprite;

	// Token: 0x04002C52 RID: 11346
	[SerializeField]
	private SpriteRenderer homerFishSprite;

	// Token: 0x04002C53 RID: 11347
	[SerializeField]
	private SpriteRenderer spreadshotFishOverlaySprite;

	// Token: 0x04002C54 RID: 11348
	[SerializeField]
	private SpriteRenderer spinnerFishOverlaySprite;

	// Token: 0x04002C55 RID: 11349
	[SerializeField]
	private SpriteRenderer homerFishOverlaySprite;

	// Token: 0x04002C56 RID: 11350
	[SerializeField]
	private FlyingMermaidLevelFish spreadshotFishPrefab;

	// Token: 0x04002C57 RID: 11351
	[SerializeField]
	private FlyingMermaidLevelFish spinnerFishPrefab;

	// Token: 0x04002C58 RID: 11352
	[SerializeField]
	private FlyingMermaidLevelFish homerFishPrefab;

	// Token: 0x04002C59 RID: 11353
	[SerializeField]
	private BasicProjectile fishSpreadshotBulletPrefab;

	// Token: 0x04002C5A RID: 11354
	[SerializeField]
	private FlyingMermaidLevelFishSpinner fishSpinnerBulletPrefab;

	// Token: 0x04002C5B RID: 11355
	[SerializeField]
	private FlyingMermaidLevelHomingProjectile fishHomerBulletPrefab;

	// Token: 0x04002C5C RID: 11356
	[SerializeField]
	private Transform fishLaunchRoot;

	// Token: 0x04002C5D RID: 11357
	[SerializeField]
	private Transform fishProjectileRoot;

	// Token: 0x04002C5E RID: 11358
	[SerializeField]
	private FlyingMermaidLevelMerdusa merdusa;

	// Token: 0x04002C5F RID: 11359
	[SerializeField]
	private Transform blockingColliders;

	// Token: 0x04002C60 RID: 11360
	[SerializeField]
	private Effect splashRight;

	// Token: 0x04002C61 RID: 11361
	[SerializeField]
	private Effect splashLeft;

	// Token: 0x04002C62 RID: 11362
	[SerializeField]
	private Transform splashRoot;

	// Token: 0x04002C63 RID: 11363
	[SerializeField]
	private Effect yellEffect;

	// Token: 0x04002C64 RID: 11364
	private FlyingMermaidLevelMermaid.SummonPossibility[] summonPattern = new FlyingMermaidLevelMermaid.SummonPossibility[]
	{
		FlyingMermaidLevelMermaid.SummonPossibility.Seahorse,
		FlyingMermaidLevelMermaid.SummonPossibility.Pufferfish,
		FlyingMermaidLevelMermaid.SummonPossibility.Turtle
	};

	// Token: 0x04002C65 RID: 11365
	private FlyingMermaidLevelMermaid.FishPossibility[] fishPattern;

	// Token: 0x04002C66 RID: 11366
	private int summonIndex;

	// Token: 0x04002C67 RID: 11367
	private int fishIndex;

	// Token: 0x04002C68 RID: 11368
	private int spreadshotPatternIndex;

	// Token: 0x04002C69 RID: 11369
	private int spinnerPatternIndex;

	// Token: 0x04002C6A RID: 11370
	private int homerPatternIndex;

	// Token: 0x04002C6B RID: 11371
	private bool stopPufferfish;

	// Token: 0x04002C6C RID: 11372
	private bool transformationStarting;

	// Token: 0x04002C6D RID: 11373
	private bool stopMoving;

	// Token: 0x04002C6E RID: 11374
	private float walkPCT;

	// Token: 0x04002C6F RID: 11375
	private float walkTime;

	// Token: 0x04002C70 RID: 11376
	private float walkDuration;

	// Token: 0x04002C71 RID: 11377
	private string[] spreadFishPinkPattern;

	// Token: 0x04002C72 RID: 11378
	private int spreadFishPinkIndex;

	// Token: 0x04002C73 RID: 11379
	private int blinks;

	// Token: 0x04002C74 RID: 11380
	private int maxBlinks;

	// Token: 0x04002C75 RID: 11381
	private FlyingMermaidLevelMermaid.FishPossibility fish;

	// Token: 0x02000693 RID: 1683
	public enum State
	{
		// Token: 0x04002C77 RID: 11383
		Intro,
		// Token: 0x04002C78 RID: 11384
		Idle,
		// Token: 0x04002C79 RID: 11385
		Yell,
		// Token: 0x04002C7A RID: 11386
		Summon,
		// Token: 0x04002C7B RID: 11387
		Fish,
		// Token: 0x04002C7C RID: 11388
		Transform
	}

	// Token: 0x02000694 RID: 1684
	public enum SummonPossibility
	{
		// Token: 0x04002C7E RID: 11390
		Seahorse,
		// Token: 0x04002C7F RID: 11391
		Pufferfish,
		// Token: 0x04002C80 RID: 11392
		Turtle
	}

	// Token: 0x02000695 RID: 1685
	public enum FishPossibility
	{
		// Token: 0x04002C82 RID: 11394
		Spreadshot,
		// Token: 0x04002C83 RID: 11395
		Spinner,
		// Token: 0x04002C84 RID: 11396
		Homer
	}
}
