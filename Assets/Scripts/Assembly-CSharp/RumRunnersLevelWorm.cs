using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007A1 RID: 1953
public class RumRunnersLevelWorm : LevelProperties.RumRunners.Entity
{
	// Token: 0x17000401 RID: 1025
	// (get) Token: 0x06002B99 RID: 11161 RVA: 0x001966BA File Offset: 0x00194ABA
	// (set) Token: 0x06002B9A RID: 11162 RVA: 0x001966C2 File Offset: 0x00194AC2
	public bool introDrop { get; set; }

	// Token: 0x17000402 RID: 1026
	// (get) Token: 0x06002B9B RID: 11163 RVA: 0x001966CB File Offset: 0x00194ACB
	// (set) Token: 0x06002B9C RID: 11164 RVA: 0x001966D3 File Offset: 0x00194AD3
	public bool isDead { get; private set; }

	// Token: 0x06002B9D RID: 11165 RVA: 0x001966DC File Offset: 0x00194ADC
	protected override void Awake()
	{
		base.Awake();
		this.boxCollider = base.GetComponent<BoxCollider2D>();
		this.boxCollider.enabled = false;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06002B9E RID: 11166 RVA: 0x00196735 File Offset: 0x00194B35
	public override void LevelInit(LevelProperties.RumRunners properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x06002B9F RID: 11167 RVA: 0x00196740 File Offset: 0x00194B40
	public void Setup()
	{
		base.gameObject.SetActive(true);
		this.phonographPos = base.transform.position;
		Vector3 position = this.laserGroup1.transform.parent.position;
		Vector3 position2 = new Vector3(base.transform.position.x, 720f);
		base.transform.position = position2;
		this.diamond.transform.position = this.phonographPos;
		this.laserGroup1.transform.parent.position = position;
	}

	// Token: 0x06002BA0 RID: 11168 RVA: 0x001967D8 File Offset: 0x00194BD8
	public void StartWorm(float introDamage)
	{
		this.bossMaxHealth = base.properties.CurrentHealth;
		if (introDamage > 0f)
		{
			base.properties.DealDamage(introDamage * base.properties.CurrentState.worm.introDamageMultiplier);
			this.GetNewSpeed();
		}
		this.diamond.transform.parent = null;
		this.laserGroup1.transform.parent.parent = null;
		this.speed = base.properties.CurrentState.worm.rotationSpeedRange.min;
		base.StartCoroutine(this.bugIntro_cr());
	}

	// Token: 0x06002BA1 RID: 11169 RVA: 0x00196880 File Offset: 0x00194C80
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (!this.canTakeDamage)
		{
			return;
		}
		base.properties.DealDamage(info.damage);
		this.GetNewSpeed();
		if (Level.Current.mode == Level.Mode.Easy && !this.isDead && base.properties.CurrentHealth <= 0f)
		{
			this.StartDeath();
		}
	}

	// Token: 0x06002BA2 RID: 11170 RVA: 0x001968E5 File Offset: 0x00194CE5
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06002BA3 RID: 11171 RVA: 0x001968FC File Offset: 0x00194CFC
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002BA4 RID: 11172 RVA: 0x00196914 File Offset: 0x00194D14
	public void StartBarrels()
	{
		this.runnersCoroutine = base.StartCoroutine(this.spawnRunners_cr());
	}

	// Token: 0x06002BA5 RID: 11173 RVA: 0x00196928 File Offset: 0x00194D28
	private void AniEvent_StartBug()
	{
		base.StartCoroutine(this.revealLaser_cr());
		this.lasersChangeCoroutine = base.StartCoroutine(this.lasersChangeDir_cr());
		this.lasersTurnOnCoroutine = base.StartCoroutine(this.lasersTurnOn_cr());
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002BA6 RID: 11174 RVA: 0x00196968 File Offset: 0x00194D68
	private IEnumerator bugIntro_cr()
	{
		this.boxCollider.enabled = true;
		this.canTakeDamage = true;
		YieldInstruction wait = new WaitForFixedUpdate();
		Vector3 startPos = new Vector3(base.transform.position.x, 720f);
		Vector3 endPos = this.phonographPos;
		this.offscreenPos = startPos;
		base.transform.position = startPos;
		base.animator.Play(0, 0, 0.2f);
		float elapsedTime = 0f;
		bool canDrop = false;
		while (!canDrop)
		{
			if (this.introDrop)
			{
				float normalizedTime = base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
				canDrop = (normalizedTime >= 0.9f || normalizedTime <= 0.1f);
			}
			elapsedTime += CupheadTime.FixedDelta;
			base.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / 1.65f);
			yield return wait;
		}
		base.animator.SetTrigger("Continue");
		elapsedTime = 0f;
		bool dustSpawned = false;
		float dropPosition = base.transform.position.y;
		while (elapsedTime < 0.6f)
		{
			elapsedTime += CupheadTime.FixedDelta;
			float t = elapsedTime / 0.6f;
			Vector3 position = base.transform.position;
			float startPosition = dropPosition;
			if (t >= 0.36363637f)
			{
				startPosition = (dropPosition - endPos.y) * 0.6f + endPos.y;
			}
			position.y = EaseUtils.EaseOutBounce(startPosition, endPos.y, t);
			base.transform.position = position;
			int shadowIndex = Mathf.Clamp(Mathf.RoundToInt(t * 10f), 0, this.dropShadowSprites.Length - 1);
			this.fakePhonographShadowRenderer.sprite = this.dropShadowSprites[shadowIndex];
			this.fakePhonographShadowRenderer.transform.position = endPos;
			if (!dustSpawned && t >= 0.36363637f)
			{
				this.dropDustEffect.Create(endPos);
				dustSpawned = true;
				CupheadLevelCamera.Current.Shake(10f, 0.3f, false);
				this.diamond.GetComponent<Collider2D>().enabled = true;
			}
			yield return wait;
		}
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForNormalizedTime(this, 1f, "IntroEnd1", 0, true, false, true);
		this.fakePhonographShadowRenderer.sprite = null;
		base.animator.Play("IntroEnd2");
		this.diamond.animator.Play("Slack", 0);
		yield return this.diamond.animator.WaitForNormalizedTime(this, 1f, "Slack", 0, true, false, true);
		this.diamond.animator.Play("Loop", 0);
		this.diamond.animator.Play("Idle", 1);
		yield break;
	}

	// Token: 0x06002BA7 RID: 11175 RVA: 0x00196984 File Offset: 0x00194D84
	private IEnumerator revealLaser_cr()
	{
		this.diamond.StartSparkle();
		this.laserGroup1.Begin();
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		this.laserGroup2.Begin();
		yield break;
	}

	// Token: 0x06002BA8 RID: 11176 RVA: 0x001969A0 File Offset: 0x00194DA0
	private IEnumerator lasersChangeDir_cr()
	{
		LevelProperties.RumRunners.Worm p = base.properties.CurrentState.worm;
		string[] directionPattern = p.directionAttackString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int directionIndex = UnityEngine.Random.Range(0, directionPattern.Length);
		this.groupSpeed1 = this.speed;
		this.groupSpeed2 = -this.speed;
		base.StartCoroutine(this.lasersRotate_cr());
		while (!this.isDead)
		{
			Parser.IntTryParse(directionPattern[directionIndex], out this.direction);
			if (this.direction == 1)
			{
				this.groupSpeed1 = this.speed;
				this.groupSpeed2 = -this.speed;
			}
			else
			{
				this.groupSpeed1 = -this.speed;
				this.groupSpeed2 = this.speed;
			}
			yield return CupheadTime.WaitForSeconds(this, p.directionTime);
			directionIndex = (directionIndex + 1) % directionPattern.Length;
		}
		yield break;
	}

	// Token: 0x06002BA9 RID: 11177 RVA: 0x001969BC File Offset: 0x00194DBC
	private IEnumerator lasersTurnOn_cr()
	{
		LevelProperties.RumRunners.Worm p = base.properties.CurrentState.worm;
		this.MusicSnapshot_StartGreenBeam();
		while (!this.isDead)
		{
			RumRunnersLevelLaser currentLaser = (!Rand.Bool()) ? this.laserGroup2 : this.laserGroup1;
			yield return CupheadTime.WaitForSeconds(this, p.attackOffDurationRange.RandomFloat());
			yield return null;
			if (currentLaser != null)
			{
				currentLaser.Warning();
				this.MusicSnapshot_StartYellowBeam();
			}
			yield return CupheadTime.WaitForSeconds(this, p.warningDuration);
			yield return null;
			if (currentLaser != null)
			{
				this.lasersAttack(currentLaser);
				this.audioWarble.HandleWarble();
				this.MusicSnapshot_StartRedBeam();
			}
			yield return CupheadTime.WaitForSeconds(this, p.attackOnDurationRange.RandomFloat());
			yield return null;
			if (currentLaser != null)
			{
				this.lasersEndAttack(currentLaser);
				this.MusicSnapshot_StartGreenBeam();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002BAA RID: 11178 RVA: 0x001969D8 File Offset: 0x00194DD8
	private IEnumerator lasersRotate_cr()
	{
		for (;;)
		{
			if (this.laserGroup1 != null)
			{
				this.laserGroup1.transform.Rotate(Vector3.forward * this.groupSpeed1 * CupheadTime.Delta);
			}
			if (this.laserGroup2 != null)
			{
				this.laserGroup2.transform.Rotate(Vector3.forward * this.groupSpeed2 * CupheadTime.Delta);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002BAB RID: 11179 RVA: 0x001969F4 File Offset: 0x00194DF4
	public void GetNewSpeed()
	{
		MinMax rotationSpeedRange = base.properties.CurrentState.worm.rotationSpeedRange;
		float num = base.properties.CurrentHealth / this.bossMaxHealth;
		float num2 = 1f - num;
		this.speed = rotationSpeedRange.min + rotationSpeedRange.max * num2;
		if (this.direction == 1)
		{
			this.groupSpeed1 = this.speed;
			this.groupSpeed2 = -this.speed;
		}
		else
		{
			this.groupSpeed1 = -this.speed;
			this.groupSpeed2 = this.speed;
		}
	}

	// Token: 0x06002BAC RID: 11180 RVA: 0x00196A8C File Offset: 0x00194E8C
	private void endLasers()
	{
		base.StopCoroutine(this.lasersTurnOnCoroutine);
		base.StopCoroutine(this.lasersChangeCoroutine);
		this.laserGroup1.CancelWarning();
		this.laserGroup2.CancelWarning();
		this.lasersEndAttack(this.laserGroup1);
		this.lasersEndAttack(this.laserGroup2);
		this.laserGroup1.End();
		this.laserGroup2.End();
		this.diamond.EndSparkle();
	}

	// Token: 0x06002BAD RID: 11181 RVA: 0x00196B00 File Offset: 0x00194F00
	private void lasersAttack(RumRunnersLevelLaser laserGroup)
	{
		laserGroup.Attack();
		this.diamond.SetAttack(true);
	}

	// Token: 0x06002BAE RID: 11182 RVA: 0x00196B14 File Offset: 0x00194F14
	private void lasersEndAttack(RumRunnersLevelLaser laserGroup)
	{
		laserGroup.EndAttack();
		this.diamond.SetAttack(false);
	}

	// Token: 0x06002BAF RID: 11183 RVA: 0x00196B28 File Offset: 0x00194F28
	private IEnumerator spawnRunners_cr()
	{
		LevelProperties.RumRunners.Barrels p = base.properties.CurrentState.barrels;
		float topDirection = (float)Rand.PosOrNeg();
		bool bottom = false;
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		if (player == null)
		{
			player = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		}
		if (player != null)
		{
			Vector3 position = player.transform.position;
			if ((position.x > 0f && topDirection < 0f) || (position.x < 0f && topDirection > 0f))
			{
				bottom = true;
			}
		}
		PatternString barrelDelayPattern = new PatternString(p.barrelDelayString, true);
		PatternString parryString = new PatternString(p.barrelParryString, true);
		while (base.properties.CurrentState.stateName != LevelProperties.RumRunners.States.Anteater)
		{
			bool isCop = (!bottom) ? this.topBarrelCop : this.bottomBarrelCop;
			this.bottomBarrelCop = ((!bottom) ? this.bottomBarrelCop : (!this.bottomBarrelCop));
			this.topBarrelCop = ((!bottom) ? (!this.topBarrelCop) : this.topBarrelCop);
			RumRunnersLevelBarrel r = this.barrelPrefab.InstantiatePrefab<RumRunnersLevelBarrel>();
			bool parryable = !isCop && parryString.PopLetter() == 'P';
			Vector3 spawnPos = (!bottom) ? this.runnerSpawnPointTop.position : this.runnerSpawnPointBottom.position;
			float direction = topDirection * (float)((!bottom) ? 1 : -1);
			spawnPos.x *= direction;
			r.LevelInit(base.properties);
			r.Initialize(direction, spawnPos, this, parryable, isCop);
			bottom = !bottom;
			float delayTime = barrelDelayPattern.PopFloat();
			yield return CupheadTime.WaitForSeconds(this, delayTime);
		}
		yield break;
	}

	// Token: 0x06002BB0 RID: 11184 RVA: 0x00196B44 File Offset: 0x00194F44
	private IEnumerator move_cr()
	{
		bool movingOut = true;
		float time = base.properties.CurrentState.worm.moveTime;
		Vector3 startPos = new Vector3(-base.properties.CurrentState.worm.moveDistance / 2f, base.transform.position.y);
		Vector3 endPos = new Vector3(base.properties.CurrentState.worm.moveDistance / 2f, base.transform.position.y);
		float t = time / 2f;
		bool kick = false;
		bool endMove = false;
		this.AnimationEvent_SFX_RUMRUN_BugGirl_Tapdance();
		SpriteRenderer spriteRenderer = base.GetComponent<SpriteRenderer>();
		int initialSortingOrder = spriteRenderer.sortingOrder;
		spriteRenderer.sortingOrder = 10;
		YieldInstruction waitInstruction = new WaitForFixedUpdate();
		bool initialLoop = true;
		while (!endMove)
		{
			float start = (!movingOut) ? endPos.x : startPos.x;
			float end = (!movingOut) ? startPos.x : endPos.x;
			if (initialLoop)
			{
				start = base.transform.position.x;
				t = 0f;
				time /= 2f;
			}
			while (t < time && !this.isDead)
			{
				t += CupheadTime.FixedDelta;
				float val = t / time;
				Vector3 position = base.transform.position;
				position.x = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, end, val);
				position.y = RumRunnersLevel.GroundWalkingPosY(position, this.boxCollider, RumRunnersLevelWorm.PositionYOffset, RumRunnersLevelWorm.PositionYRayLength);
				base.transform.position = position;
				yield return waitInstruction;
				if (!kick && val > 0.7f)
				{
					kick = true;
					string trigger = (end <= 0f) ? "KickLeft" : "KickRight";
					base.animator.SetTrigger(trigger);
				}
			}
			if (this.isDead)
			{
				endMove = true;
				base.StartCoroutine(this.defeat_cr());
			}
			else
			{
				movingOut = !movingOut;
				kick = false;
				t = 0f;
				base.transform.SetPosition(new float?(end), null, null);
				if (initialLoop)
				{
					time *= 2f;
					initialLoop = false;
				}
			}
			yield return null;
		}
		base.StartCoroutine(this.deathMove_cr(initialSortingOrder));
		yield break;
	}

	// Token: 0x06002BB1 RID: 11185 RVA: 0x00196B60 File Offset: 0x00194F60
	private IEnumerator defeat_cr()
	{
		base.animator.SetBool("EasyMode", Level.Current.mode == Level.Mode.Easy);
		base.animator.Play("Defeat");
		base.transform.localScale = new Vector3(Mathf.Sign(base.transform.position.x), 1f);
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		this.diamond.animator.Play("Defeat");
		yield break;
	}

	// Token: 0x06002BB2 RID: 11186 RVA: 0x00196B7C File Offset: 0x00194F7C
	private IEnumerator deathMove_cr(int initialSortingOrder)
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float FALL_SPEED = (Level.Current.mode != Level.Mode.Easy) ? 400f : 200f;
		bool flipped = base.transform.position.x < 0f;
		float POS_X = 612f * Mathf.Sign(base.transform.position.x);
		float fallTime = (Mathf.Abs(POS_X) - Mathf.Abs(base.transform.position.x)) / FALL_SPEED;
		float startPos = base.transform.position.x;
		float endpos = POS_X;
		float elapsedTime = 0f;
		while (elapsedTime < fallTime)
		{
			elapsedTime += CupheadTime.FixedDelta;
			Vector3 position = base.transform.position;
			position.x = Mathf.Lerp(startPos, endpos, elapsedTime / fallTime);
			position.y = RumRunnersLevel.GroundWalkingPosY(position, this.boxCollider, RumRunnersLevelWorm.PositionYOffset, RumRunnersLevelWorm.PositionYRayLength);
			base.transform.position = position;
			yield return wait;
		}
		base.animator.SetTrigger("Continue");
		if (Level.Current.mode == Level.Mode.Easy)
		{
			this.StopAllCoroutines();
			yield break;
		}
		yield return base.animator.WaitForAnimationToEnd(this, "Fall", false, true);
		this.canTakeDamage = false;
		base.GetComponent<HitFlash>().disabled = true;
		base.GetComponent<SpriteRenderer>().sortingOrder = initialSortingOrder;
		elapsedTime = 0f;
		startPos = base.transform.position.x;
		float targetPositionX = (!flipped) ? this.phonographPos.x : -105f;
		base.animator.enabled = false;
		while (elapsedTime < 2f)
		{
			Vector2 position2 = base.transform.position;
			position2.x = Mathf.Lerp(startPos, targetPositionX, elapsedTime / 2f);
			position2.y = RumRunnersLevel.GroundWalkingPosY(position2, this.boxCollider, RumRunnersLevelWorm.PositionYOffset, RumRunnersLevelWorm.PositionYRayLength);
			base.transform.position = position2;
			yield return null;
			base.animator.Update(CupheadTime.Delta);
			elapsedTime = base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
		}
		base.transform.SetPosition(new float?(targetPositionX), null, null);
		base.animator.enabled = true;
		base.animator.SetBool("Flipped", flipped);
		base.animator.SetTrigger("End");
		string animationName = (!flipped) ? "Jump" : "JumpFlipped";
		yield return base.animator.WaitForNormalizedTime(this, 1f, animationName, 0, true, false, true);
		animationName = ((!flipped) ? "JumpSquish" : "JumpSquishFlipped");
		base.animator.Play(animationName);
		this.diamond.animator.Play((!flipped) ? "DefeatSquish" : "DefeatSquishFlipped");
		yield return base.animator.WaitForNormalizedTime(this, 1f, animationName, 0, true, false, true);
		base.transform.SetPosition(new float?(this.phonographPos.x * Mathf.Sign(base.transform.position.x) + ((!flipped) ? 0f : -6f)), null, null);
		base.animator.Play("Wave");
		this.diamond.GetComponent<Collider2D>().enabled = false;
		elapsedTime = 0f;
		Vector3 start = base.transform.position;
		Vector3 targetPosition = new Vector3(base.transform.position.x, this.offscreenPos.y);
		this.diamond.transform.parent = base.transform;
		base.StartCoroutine(this.exitShadow_cr());
		while (elapsedTime < 0.866f)
		{
			elapsedTime += CupheadTime.FixedDelta;
			base.transform.position = Vector3.Lerp(start, targetPosition, elapsedTime / 0.866f);
			yield return wait;
		}
		this.StopAllCoroutines();
		this.diamond.Die();
		UnityEngine.Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x06002BB3 RID: 11187 RVA: 0x00196BA0 File Offset: 0x00194FA0
	private IEnumerator exitShadow_cr()
	{
		this.realPhonographShadowRenderer.enabled = false;
		Vector3 position = this.realPhonographShadowRenderer.transform.position;
		float accumulator = 0f;
		int index = 4;
		while (index >= 0)
		{
			this.fakePhonographShadowRenderer.sprite = this.dropShadowSprites[index];
			this.fakePhonographShadowRenderer.transform.position = position;
			yield return null;
			accumulator += CupheadTime.Delta;
			if (accumulator > 0.041666668f)
			{
				accumulator -= 0.041666668f;
				index--;
			}
		}
		this.fakePhonographShadowRenderer.sprite = null;
		yield break;
	}

	// Token: 0x06002BB4 RID: 11188 RVA: 0x00196BBC File Offset: 0x00194FBC
	public void StartDeath()
	{
		if (this.isDead)
		{
			return;
		}
		this.AnimationEvent_SFX_RUMRUN_BugGirl_Tapdance_Stop();
		this.SFX_RUMRUN_BugGirl_DieFalltoGround();
		this.MusicSnapshot_RevertToDefault();
		this.isDead = true;
		this.endLasers();
		base.StopCoroutine(this.runnersCoroutine);
		if (Level.Current.mode == Level.Mode.Easy)
		{
			base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		}
	}

	// Token: 0x06002BB5 RID: 11189 RVA: 0x00196C1C File Offset: 0x0019501C
	protected virtual void MusicSnapshot_StartGreenBeam()
	{
		AudioManager.SnapshotTransition(new string[]
		{
			"RumRunners_GreenBeam",
			"Unpaused",
			"Unpaused_1920s"
		}, new float[]
		{
			1f,
			0f,
			0f
		}, 0.5f);
	}

	// Token: 0x06002BB6 RID: 11190 RVA: 0x00196C74 File Offset: 0x00195074
	protected virtual void MusicSnapshot_StartYellowBeam()
	{
		AudioManager.SnapshotTransition(new string[]
		{
			"RumRunners_YellowBeam",
			"Unpaused",
			"Unpaused_1920s"
		}, new float[]
		{
			1f,
			0f,
			0f
		}, 0.5f);
	}

	// Token: 0x06002BB7 RID: 11191 RVA: 0x00196CCC File Offset: 0x001950CC
	protected virtual void MusicSnapshot_StartRedBeam()
	{
		AudioManager.SnapshotTransition(new string[]
		{
			"RumRunners_RedBeam",
			"RumRunners_GreenBeam",
			"Unpaused_1920s"
		}, new float[]
		{
			1f,
			0f,
			0f
		}, 0.16f);
	}

	// Token: 0x06002BB8 RID: 11192 RVA: 0x00196D24 File Offset: 0x00195124
	protected virtual void MusicSnapshot_RevertToDefault()
	{
		string[] array = new string[2];
		array[0] = "RumRunners_RedBeam";
		if (SettingsData.Data.vintageAudioEnabled)
		{
			array[1] = "Unpaused_1920s";
		}
		else
		{
			array[1] = "Unpaused";
		}
		AudioManager.SnapshotTransition(array, new float[]
		{
			0f,
			1f
		}, 3f);
	}

	// Token: 0x06002BB9 RID: 11193 RVA: 0x00196D88 File Offset: 0x00195188
	private IEnumerator StartRedBeamMusicSnapshotWait_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 3f);
		yield break;
	}

	// Token: 0x06002BBA RID: 11194 RVA: 0x00196DA3 File Offset: 0x001951A3
	private void SFX_RUMRUN_BugGirl_DieFalltoGround()
	{
		AudioManager.Play("sfx_DLC_RUMRUN_P2_BugGirl_DieFalltoGround");
		this.emitAudioFromObject.Add("sfx_DLC_RUMRUN_P2_BugGirl_DieFalltoGround");
	}

	// Token: 0x06002BBB RID: 11195 RVA: 0x00196DBF File Offset: 0x001951BF
	private void AnimationEvent_SFX_RUMRUN_BugGirl_DismountJumpLand()
	{
		AudioManager.Play("sfx_DLC_RUMRUN_P2_BugGirl_DismountJumpLand");
		this.emitAudioFromObject.Add("sfx_DLC_RUMRUN_P2_BugGirl_DismountJumpLand");
	}

	// Token: 0x06002BBC RID: 11196 RVA: 0x00196DDB File Offset: 0x001951DB
	private void AnimationEvent_SFX_RUMRUN_BugGirl_ExitWinch()
	{
		AudioManager.Play("sfx_DLC_RUMRUN_P2_BugGirl_ExitWinch");
		this.emitAudioFromObject.Add("sfx_DLC_RUMRUN_P2_BugGirl_ExitWinch");
	}

	// Token: 0x06002BBD RID: 11197 RVA: 0x00196DF7 File Offset: 0x001951F7
	private void AnimationEvent_SFX_RUMRUN_BugGirl_Tapdance()
	{
		AudioManager.PlayLoop("sfx_dlc_rumrun_p2_buggirl_tapdance");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_p2_buggirl_tapdance");
	}

	// Token: 0x06002BBE RID: 11198 RVA: 0x00196E13 File Offset: 0x00195213
	private void AnimationEvent_SFX_RUMRUN_BugGirl_Tapdance_Stop()
	{
		AudioManager.Stop("sfx_dlc_rumrun_p2_buggirl_tapdance");
	}

	// Token: 0x06002BBF RID: 11199 RVA: 0x00196E1F File Offset: 0x0019521F
	private void AnimationEvent_SFX_RUMRUN_BugGirl_VocalDismountLaugh()
	{
		AudioManager.Play("sfx_DLC_RUMRUN_P2_BugGirl_VocalDismountLaugh");
		this.emitAudioFromObject.Add("sfx_DLC_RUMRUN_P2_BugGirl_VocalDismountLaugh");
	}

	// Token: 0x06002BC0 RID: 11200 RVA: 0x00196E3B File Offset: 0x0019523B
	private void AnimationEvent_SFX_RUMRUN_BugGirl_VocalExcited()
	{
		AudioManager.Play("sfx_DLC_RUMRUN_P2_BugGirl_VocalExcited");
		this.emitAudioFromObject.Add("sfx_DLC_RUMRUN_P2_BugGirl_VocalExcited");
	}

	// Token: 0x0400344F RID: 13391
	private static readonly float PositionYOffset = 20f;

	// Token: 0x04003450 RID: 13392
	private static readonly float PositionYRayLength = 250f;

	// Token: 0x04003451 RID: 13393
	[SerializeField]
	private Sprite[] dropShadowSprites;

	// Token: 0x04003452 RID: 13394
	[SerializeField]
	private SpriteRenderer fakePhonographShadowRenderer;

	// Token: 0x04003453 RID: 13395
	[SerializeField]
	private SpriteRenderer realPhonographShadowRenderer;

	// Token: 0x04003454 RID: 13396
	[SerializeField]
	private Effect dropDustEffect;

	// Token: 0x04003455 RID: 13397
	[SerializeField]
	private RumRunnersLevelLaser laserGroup1;

	// Token: 0x04003456 RID: 13398
	[SerializeField]
	private RumRunnersLevelLaser laserGroup2;

	// Token: 0x04003457 RID: 13399
	[SerializeField]
	private RumRunnersLevelDiamond diamond;

	// Token: 0x04003458 RID: 13400
	[SerializeField]
	private RumRunnersLevelBarrel barrelPrefab;

	// Token: 0x04003459 RID: 13401
	[SerializeField]
	private Transform runnerSpawnPointTop;

	// Token: 0x0400345A RID: 13402
	[SerializeField]
	private Transform runnerSpawnPointBottom;

	// Token: 0x0400345B RID: 13403
	[SerializeField]
	private AudioWarble audioWarble;

	// Token: 0x0400345C RID: 13404
	private DamageDealer damageDealer;

	// Token: 0x0400345D RID: 13405
	private DamageReceiver damageReceiver;

	// Token: 0x0400345E RID: 13406
	private bool canTakeDamage;

	// Token: 0x0400345F RID: 13407
	private Vector3 phonographPos;

	// Token: 0x04003460 RID: 13408
	private Vector3 offscreenPos;

	// Token: 0x04003461 RID: 13409
	private float speed;

	// Token: 0x04003462 RID: 13410
	private float groupSpeed1;

	// Token: 0x04003463 RID: 13411
	private float groupSpeed2;

	// Token: 0x04003464 RID: 13412
	private float bossMaxHealth;

	// Token: 0x04003465 RID: 13413
	private bool topBarrelCop;

	// Token: 0x04003466 RID: 13414
	private bool bottomBarrelCop;

	// Token: 0x04003467 RID: 13415
	private int direction;

	// Token: 0x04003468 RID: 13416
	private Coroutine runnersCoroutine;

	// Token: 0x04003469 RID: 13417
	private Coroutine lasersChangeCoroutine;

	// Token: 0x0400346A RID: 13418
	private Coroutine lasersTurnOnCoroutine;

	// Token: 0x0400346B RID: 13419
	private BoxCollider2D boxCollider;
}
