using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200057F RID: 1407
public class DevilLevelSittingDevil : LevelProperties.Devil.Entity
{
	// Token: 0x06001ABE RID: 6846 RVA: 0x000F539C File Offset: 0x000F379C
	protected override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.leftWallPositionX = this.leftWall.transform.position.x;
		this.rightWallPositionX = this.rightWall.transform.position.x;
	}

	// Token: 0x06001ABF RID: 6847 RVA: 0x000F540E File Offset: 0x000F380E
	private void Start()
	{
		this.dragonPos = this.dragonHead.transform.position;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001AC0 RID: 6848 RVA: 0x000F5434 File Offset: 0x000F3834
	public override void LevelInit(LevelProperties.Devil properties)
	{
		base.LevelInit(properties);
		this.isSpiderAttackNext = Rand.Bool();
		this.spiderOffsets = properties.CurrentState.spider.positionOffset.Split(new char[]
		{
			','
		});
		this.spiderOffsetIndex = UnityEngine.Random.Range(0, this.spiderOffsets.Length);
		this.pitchforkPattern = properties.CurrentState.pitchfork.patternString.RandomChoice<string>().Split(new char[]
		{
			','
		});
		this.pitchforkPatternIndex = UnityEngine.Random.Range(0, this.pitchforkPattern.Length);
		this.pitchforkTwoFlameWheelSpawner = new DevilLevelPitchforkProjectileSpawner(2, properties.CurrentState.pitchforkTwoFlameWheel.angleOffset);
		this.pitchforkThreeFlameJumperSpawner = new DevilLevelPitchforkProjectileSpawner(3, properties.CurrentState.pitchforkThreeFlameJumper.angleOffset);
		this.pitchforkFourFlameBouncerSpawner = new DevilLevelPitchforkProjectileSpawner(4, properties.CurrentState.pitchforkFourFlameBouncer.angleOffset);
		this.pitchforkFiveFlameSpinnerSpawner = new DevilLevelPitchforkProjectileSpawner(4, properties.CurrentState.pitchforkFiveFlameSpinner.angleOffset);
		this.pitchforkSixFlameRingSpawner = new DevilLevelPitchforkProjectileSpawner(6, properties.CurrentState.pitchforkSixFlameRing.angleOffset);
		if (Level.CurrentMode == Level.Mode.Easy)
		{
			properties.OnBossDeath += this.DeathEasy;
		}
	}

	// Token: 0x06001AC1 RID: 6849 RVA: 0x000F5574 File Offset: 0x000F3974
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.demonPrefab = null;
		this.wheelProjectilePrefab = null;
		this.wheelOrbitingProjectilePrefab = null;
		this.jumpingProjectilePrefab = null;
		this.bouncingProjectilePrefab = null;
		this.spinnerOrbitingProjectilePrefab = null;
		this.spinnerProjectilePrefab = null;
		this.ringProjectilePrefab = null;
	}

	// Token: 0x06001AC2 RID: 6850 RVA: 0x000F55B4 File Offset: 0x000F39B4
	private IEnumerator intro_cr()
	{
		this.state = DevilLevelSittingDevil.State.Intro;
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", false, true);
		this.state = DevilLevelSittingDevil.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06001AC3 RID: 6851 RVA: 0x000F55CF File Offset: 0x000F39CF
	public void StartDemons()
	{
		base.StartCoroutine(this.demon_cr());
	}

	// Token: 0x06001AC4 RID: 6852 RVA: 0x000F55E0 File Offset: 0x000F39E0
	private IEnumerator demon_cr()
	{
		bool fromLeft = Rand.Bool();
		bool playedFirstSound = false;
		yield return CupheadTime.WaitForSeconds(this, 3f);
		while (!this.endPH1)
		{
			yield return null;
			if (playedFirstSound)
			{
				AudioManager.Play("devil_small_flame_imp_spawn");
				this.emitAudioFromObject.Add("devil_small_flame_imp_spawn");
			}
			else
			{
				AudioManager.Play("devil_small_flame_imp_first_spawn");
				this.emitAudioFromObject.Add("devil_small_flame_imp_first_spawn");
				playedFirstSound = true;
			}
			DevilLevelDemon demon = this.demonPrefab.Create((!fromLeft) ? this.rightDemonPeek.position : this.leftDemonPeek.position, (float)((!fromLeft) ? -1 : 1), base.properties.CurrentState.demons.speed, base.properties.CurrentState.demons.hp, this);
			if (fromLeft)
			{
				demon.JumpRoot = this.leftDemonJumpRoot.position;
				demon.RunRoot = this.leftDemonRunRoot.position;
				demon.PillarDestination = this.leftDemonPillar.position;
				demon.FrontSpawn = this.leftDemonFront.position;
			}
			else
			{
				demon.JumpRoot = this.rightDemonJumpRoot.position;
				demon.RunRoot = this.rightDemonRunRoot.position;
				demon.PillarDestination = this.rightDemonPillar.position;
				demon.FrontSpawn = this.rightDemonFront.position;
			}
			fromLeft = !fromLeft;
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.demons.delay);
		}
		yield break;
	}

	// Token: 0x06001AC5 RID: 6853 RVA: 0x000F55FB File Offset: 0x000F39FB
	public void StartClap()
	{
		this.state = DevilLevelSittingDevil.State.Clap;
		base.StartCoroutine(this.clap_cr());
	}

	// Token: 0x06001AC6 RID: 6854 RVA: 0x000F5614 File Offset: 0x000F3A14
	private IEnumerator clap_cr()
	{
		LevelProperties.Devil.Clap p = base.properties.CurrentState.clap;
		base.animator.SetBool("StartRam", true);
		yield return base.animator.WaitForAnimationToEnd(this, "Ram_Start", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.delay.RandomFloat());
		foreach (DevilLevelDevilArm devilLevelDevilArm in this.arms)
		{
			devilLevelDevilArm.Attack(p.speed);
		}
		while (this.arms[0].state != DevilLevelDevilArm.State.Idle)
		{
			yield return null;
		}
		base.animator.SetBool("StartRam", false);
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = DevilLevelSittingDevil.State.Idle;
		yield break;
	}

	// Token: 0x06001AC7 RID: 6855 RVA: 0x000F5630 File Offset: 0x000F3A30
	public void StartHead()
	{
		this.state = DevilLevelSittingDevil.State.Head;
		if (this.isSpiderAttackNext)
		{
			base.StartCoroutine(this.spider_cr());
		}
		else
		{
			base.StartCoroutine(this.dragon_cr());
		}
		this.isSpiderAttackNext = !this.isSpiderAttackNext;
	}

	// Token: 0x06001AC8 RID: 6856 RVA: 0x000F5680 File Offset: 0x000F3A80
	private IEnumerator spider_cr()
	{
		base.animator.SetBool("StartSpider", true);
		yield return base.animator.WaitForAnimationToStart(this, "Spider_Start", false);
		AudioManager.Play("devil_spider_head_intro");
		this.emitAudioFromObject.Add("devil_spider_head_intro");
		yield return base.animator.WaitForAnimationToEnd(this, "Spider_Start", false, true);
		LevelProperties.Devil.Spider p = base.properties.CurrentState.spider;
		int numAttacks = p.numAttacks.RandomInt();
		for (int i = 0; i < numAttacks; i++)
		{
			yield return CupheadTime.WaitForSeconds(this, p.entranceDelay.RandomFloat());
			this.spiderOffsetIndex = (this.spiderOffsetIndex + 1) % this.spiderOffsets.Length;
			float offset = 0f;
			Parser.FloatTryParse(this.spiderOffsets[this.spiderOffsetIndex], out offset);
			this.spiderHead.Attack(Mathf.Clamp(PlayerManager.GetNext().center.x + offset, -620f, 620f), p.downSpeed, p.upSpeed);
			while (this.spiderHead.state != DevilLevelSpiderHead.State.Idle)
			{
				yield return null;
			}
		}
		base.animator.SetBool("StartSpider", false);
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = DevilLevelSittingDevil.State.Idle;
		yield break;
	}

	// Token: 0x06001AC9 RID: 6857 RVA: 0x000F569C File Offset: 0x000F3A9C
	private IEnumerator dragon_cr()
	{
		base.animator.SetBool("StartDragon", true);
		bool isLeft = Rand.Bool();
		base.animator.SetBool("IsLeft", isLeft);
		this.dragonHead.Attack(this, isLeft);
		LevelProperties.Devil.Dragon p = base.properties.CurrentState.dragon;
		while (this.dragonHead.state != DevilLevelDragonHead.State.Idle)
		{
			yield return null;
		}
		base.animator.SetBool("StartDragon", false);
		yield return base.animator.WaitForAnimationToEnd(this, "Morph_End", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = DevilLevelSittingDevil.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06001ACA RID: 6858 RVA: 0x000F56B7 File Offset: 0x000F3AB7
	private void DragonStop()
	{
		base.animator.SetTrigger("Continue");
		this.dragonHead.state = DevilLevelDragonHead.State.Stopped;
	}

	// Token: 0x06001ACB RID: 6859 RVA: 0x000F56D5 File Offset: 0x000F3AD5
	private void DragonReverse()
	{
		base.animator.SetTrigger("OnDragonEnd");
	}

	// Token: 0x06001ACC RID: 6860 RVA: 0x000F56E7 File Offset: 0x000F3AE7
	private void ResetPosition()
	{
		this.dragonHead.SetPosition(this.dragonPos);
	}

	// Token: 0x06001ACD RID: 6861 RVA: 0x000F56FA File Offset: 0x000F3AFA
	public void StartPitchfork()
	{
		this.state = DevilLevelSittingDevil.State.Pitchfork;
		base.animator.SetBool("StartTrident", true);
	}

	// Token: 0x06001ACE RID: 6862 RVA: 0x000F5714 File Offset: 0x000F3B14
	private void SpawnProjectiles()
	{
		this.StartTridentHeadSFX();
		this.pitchforkPatternIndex = (this.pitchforkPatternIndex + 1) % this.pitchforkPattern.Length;
		int num = 0;
		Parser.IntTryParse(this.pitchforkPattern[this.pitchforkPatternIndex], out num);
		AudioManager.Play("devil_generic_projectile_start");
		this.emitAudioFromObject.Add("devil_generic_projectile_start");
		switch (num)
		{
		case 2:
			base.StartCoroutine(this.pitchforkTwoFlameWheel_cr());
			break;
		case 3:
			base.StartCoroutine(this.pitchforkThreeFlameJumper_cr());
			break;
		case 4:
			base.StartCoroutine(this.pitchforkFourFlameBouncer_cr());
			break;
		case 5:
			base.StartCoroutine(this.pitchforkFiveFlameSpinner_cr());
			break;
		case 6:
			base.StartCoroutine(this.pitchforkSixFlameRing_cr());
			break;
		}
	}

	// Token: 0x06001ACF RID: 6863 RVA: 0x000F57EC File Offset: 0x000F3BEC
	private Vector2 getPitchforkFiringPos(float angle)
	{
		return new Vector2(0f, base.properties.CurrentState.pitchfork.spawnCenterY) + MathUtils.AngleToDirection(angle) * base.properties.CurrentState.pitchfork.spawnRadius;
	}

	// Token: 0x06001AD0 RID: 6864 RVA: 0x000F583D File Offset: 0x000F3C3D
	private void StartParts()
	{
		base.animator.Play("Trident_Body", 2);
		base.animator.Play("Trident_Attack", 3);
	}

	// Token: 0x06001AD1 RID: 6865 RVA: 0x000F5861 File Offset: 0x000F3C61
	private void StopParts()
	{
		base.animator.SetBool("StartTrident", false);
	}

	// Token: 0x06001AD2 RID: 6866 RVA: 0x000F5874 File Offset: 0x000F3C74
	private IEnumerator pitchforkTwoFlameWheel_cr()
	{
		LevelProperties.Devil.PitchforkTwoFlameWheel p = base.properties.CurrentState.pitchforkTwoFlameWheel;
		List<DevilLevelPitchforkWheelProjectile> projectiles = new List<DevilLevelPitchforkWheelProjectile>();
		bool flipDelays = Rand.Bool();
		foreach (float angle in this.pitchforkTwoFlameWheelSpawner.getSpawnAngles())
		{
			bool flag = projectiles.Count == 0;
			if (flipDelays)
			{
				flag = !flag;
			}
			float attackDelay = (!flag) ? p.secondAttackDelay : p.initialtAttackDelay;
			DevilLevelPitchforkWheelProjectile devilLevelPitchforkWheelProjectile = this.wheelProjectilePrefab.Create(this.getPitchforkFiringPos(angle), attackDelay, p.movementSpeed, this);
			this.wheelOrbitingProjectilePrefab.Create(devilLevelPitchforkWheelProjectile, 90f, (float)Rand.PosOrNeg() * p.rotationSpeed, 100f, this);
			projectiles.Add(devilLevelPitchforkWheelProjectile);
		}
		bool allProjectilesFinished = false;
		while (!allProjectilesFinished)
		{
			allProjectilesFinished = true;
			foreach (DevilLevelPitchforkWheelProjectile devilLevelPitchforkWheelProjectile2 in projectiles)
			{
				if (devilLevelPitchforkWheelProjectile2 != null && devilLevelPitchforkWheelProjectile2.state != DevilLevelPitchforkWheelProjectile.State.Returning)
				{
					allProjectilesFinished = false;
				}
			}
			yield return null;
		}
		AudioManager.Play("devil_generic_projectile_stop");
		this.emitAudioFromObject.Add("devil_generic_projectile_stop");
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = DevilLevelSittingDevil.State.Idle;
		yield break;
	}

	// Token: 0x06001AD3 RID: 6867 RVA: 0x000F5890 File Offset: 0x000F3C90
	private IEnumerator pitchforkThreeFlameJumper_cr()
	{
		LevelProperties.Devil.PitchforkThreeFlameJumper p = base.properties.CurrentState.pitchforkThreeFlameJumper;
		List<DevilLevelPitchforkJumpingProjectile> projectiles = new List<DevilLevelPitchforkJumpingProjectile>();
		foreach (float angle in this.pitchforkThreeFlameJumperSpawner.getSpawnAngles())
		{
			projectiles.Add(this.jumpingProjectilePrefab.Create(this.getPitchforkFiringPos(angle), p.launchAngle, p.launchSpeed, p.gravity, p.numJumps, this));
		}
		projectiles.Shuffle<DevilLevelPitchforkJumpingProjectile>();
		float delay = p.initialAttackDelay.RandomFloat();
		for (int i = 0; i < p.numJumps; i++)
		{
			foreach (DevilLevelPitchforkJumpingProjectile projectile in projectiles)
			{
				yield return CupheadTime.WaitForSeconds(this, delay);
				projectile.Jump();
				delay = p.jumpDelay;
			}
		}
		AudioManager.Play("devil_generic_projectile_stop");
		this.emitAudioFromObject.Add("devil_generic_projectile_stop");
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = DevilLevelSittingDevil.State.Idle;
		yield break;
	}

	// Token: 0x06001AD4 RID: 6868 RVA: 0x000F58AC File Offset: 0x000F3CAC
	private IEnumerator pitchforkFourFlameBouncer_cr()
	{
		LevelProperties.Devil.PitchforkFourFlameBouncer p = base.properties.CurrentState.pitchforkFourFlameBouncer;
		List<DevilLevelPitchforkBouncingProjectile> projectiles = new List<DevilLevelPitchforkBouncingProjectile>();
		float delay = p.initialAttackDelay.RandomFloat();
		foreach (float angle in this.pitchforkFourFlameBouncerSpawner.getSpawnAngles())
		{
			projectiles.Add(this.bouncingProjectilePrefab.Create(this.getPitchforkFiringPos(angle), delay, p.speed, angle, p.numBounces, this, base.properties.CurrentState.pitchfork.dormantDuration));
		}
		projectiles[UnityEngine.Random.Range(0, projectiles.Count)].SetParryable(true);
		bool allProjectilesFinished = false;
		while (!allProjectilesFinished)
		{
			allProjectilesFinished = true;
			foreach (DevilLevelPitchforkBouncingProjectile devilLevelPitchforkBouncingProjectile in projectiles)
			{
				if (devilLevelPitchforkBouncingProjectile.BouncesRemaining > 0)
				{
					allProjectilesFinished = false;
				}
			}
			yield return null;
		}
		AudioManager.Play("devil_generic_projectile_stop");
		this.emitAudioFromObject.Add("devil_generic_projectile_stop");
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = DevilLevelSittingDevil.State.Idle;
		yield break;
	}

	// Token: 0x06001AD5 RID: 6869 RVA: 0x000F58C8 File Offset: 0x000F3CC8
	private IEnumerator pitchforkFiveFlameSpinner_cr()
	{
		LevelProperties.Devil.PitchforkFiveFlameSpinner p = base.properties.CurrentState.pitchforkFiveFlameSpinner;
		DevilLevelPitchforkSpinnerProjectile centerProjectile = this.spinnerProjectilePrefab.Create(new Vector2(0f, base.properties.CurrentState.pitchfork.spawnCenterY), p.maxSpeed, p.acceleration, p.attackDuration, this, base.properties.CurrentState.pitchfork.dormantDuration);
		float rotationSpeed = (float)Rand.PosOrNeg() * p.rotationSpeed;
		foreach (float angle in this.pitchforkFiveFlameSpinnerSpawner.getSpawnAngles())
		{
			this.spinnerOrbitingProjectilePrefab.Create(centerProjectile, angle, rotationSpeed, base.properties.CurrentState.pitchfork.spawnRadius, this, base.properties.CurrentState.pitchfork.dormantDuration);
		}
		AudioManager.Play("devil_generic_projectile_stop");
		this.emitAudioFromObject.Add("devil_generic_projectile_stop");
		yield return CupheadTime.WaitForSeconds(this, p.attackDuration + p.hesitate);
		this.state = DevilLevelSittingDevil.State.Idle;
		yield break;
	}

	// Token: 0x06001AD6 RID: 6870 RVA: 0x000F58E4 File Offset: 0x000F3CE4
	private IEnumerator pitchforkSixFlameRing_cr()
	{
		LevelProperties.Devil.PitchforkSixFlameRing p = base.properties.CurrentState.pitchforkSixFlameRing;
		List<DevilLevelPitchforkRingProjectile> projectiles = new List<DevilLevelPitchforkRingProjectile>();
		foreach (float angle in this.pitchforkSixFlameRingSpawner.getSpawnAngles())
		{
			projectiles.Add(this.ringProjectilePrefab.Create(this.getPitchforkFiringPos(angle), p.speed, p.groundDuration, this, base.properties.CurrentState.pitchfork.dormantDuration));
		}
		projectiles[UnityEngine.Random.Range(0, projectiles.Count)].SetParryable(true);
		yield return CupheadTime.WaitForSeconds(this, p.initialAttackDelay.RandomFloat());
		projectiles[0].Attack();
		projectiles.RemoveAt(0);
		if (Rand.Bool())
		{
			projectiles.Reverse();
		}
		foreach (DevilLevelPitchforkRingProjectile projectile in projectiles)
		{
			yield return CupheadTime.WaitForSeconds(this, p.attackDelay);
			projectile.Attack();
		}
		AudioManager.Play("devil_generic_projectile_stop");
		this.emitAudioFromObject.Add("devil_generic_projectile_stop");
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = DevilLevelSittingDevil.State.Idle;
		yield break;
	}

	// Token: 0x06001AD7 RID: 6871 RVA: 0x000F5900 File Offset: 0x000F3D00
	private void DeathEasy()
	{
		if (Level.Current.mode == Level.Mode.Easy)
		{
			base.properties.OnBossDeath -= this.DeathEasy;
			base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		}
		base.animator.Play("DeathEasy");
	}

	// Token: 0x06001AD8 RID: 6872 RVA: 0x000F594E File Offset: 0x000F3D4E
	public void StartTransform()
	{
		this.endPH1 = true;
		this.state = DevilLevelSittingDevil.State.EndPhase1;
		base.animator.SetTrigger("OnPhase2");
		base.StartCoroutine(this.on_phase_2_cr());
	}

	// Token: 0x06001AD9 RID: 6873 RVA: 0x000F597C File Offset: 0x000F3D7C
	private IEnumerator on_phase_2_cr()
	{
		yield return base.animator.WaitForAnimationToStart(this, "Death_Start", false);
		if (this.OnPhase1Death != null)
		{
			this.OnPhase1Death();
		}
		yield return base.animator.WaitForAnimationToStart(this, "Death_Hole", false);
		this.middleGround.SetActive(false);
		base.StartCoroutine(this.move_fire_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06001ADA RID: 6874 RVA: 0x000F5998 File Offset: 0x000F3D98
	private IEnumerator move_fire_cr()
	{
		AudioManager.PlayLoop("devil_fire_wall");
		this.emitAudioFromObject.Add("devil_fire_wall");
		while (!this.endFire)
		{
			if (this.leftWall.transform.position.x >= -200f)
			{
				break;
			}
			this.leftWall.transform.position += Vector3.right * base.properties.CurrentState.firewall.firewallSpeed * CupheadTime.FixedDelta;
			if (this.rightWall.transform.position.x <= 200f)
			{
				break;
			}
			this.rightWall.transform.position += Vector3.left * base.properties.CurrentState.firewall.firewallSpeed * CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06001ADB RID: 6875 RVA: 0x000F59B3 File Offset: 0x000F3DB3
	public void RemoveFire()
	{
		base.StartCoroutine(this.remove_fire_cr());
	}

	// Token: 0x06001ADC RID: 6876 RVA: 0x000F59C4 File Offset: 0x000F3DC4
	private IEnumerator remove_fire_cr()
	{
		this.endFire = true;
		float t = 0f;
		float time = 1f;
		float startLeftPos = this.leftWall.transform.position.x;
		float startRightPos = this.rightWall.transform.position.x;
		while (t < time)
		{
			t += CupheadTime.Delta;
			float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
			this.leftWall.transform.SetPosition(new float?(Mathf.Lerp(startLeftPos, this.leftWallPositionX, val)), null, null);
			this.rightWall.transform.SetPosition(new float?(Mathf.Lerp(startRightPos, this.rightWallPositionX, val)), null, null);
			yield return null;
		}
		this.leftWall.transform.SetPosition(new float?(this.leftWallPositionX), null, null);
		this.rightWall.transform.SetPosition(new float?(this.rightWallPositionX), null, null);
		AudioManager.FadeSFXVolume("devil_fire_wall", 0f, 1f);
		yield return CupheadTime.WaitForSeconds(this, 1f);
		AudioManager.Stop("devil_fire_wall");
		yield break;
	}

	// Token: 0x06001ADD RID: 6877 RVA: 0x000F59DF File Offset: 0x000F3DDF
	private void TridentStartSFX()
	{
		AudioManager.Play("devil_trident_start");
		this.emitAudioFromObject.Add("devil_trident_start");
	}

	// Token: 0x06001ADE RID: 6878 RVA: 0x000F59FB File Offset: 0x000F3DFB
	private void TridentEndSFX()
	{
		AudioManager.Play("devil_trident_end");
		this.emitAudioFromObject.Add("devil_trident_end");
	}

	// Token: 0x06001ADF RID: 6879 RVA: 0x000F5A17 File Offset: 0x000F3E17
	private void TridentAttackSFX()
	{
		AudioManager.Play("devil_trident_attack");
		this.emitAudioFromObject.Add("devil_trident_attack");
	}

	// Token: 0x06001AE0 RID: 6880 RVA: 0x000F5A33 File Offset: 0x000F3E33
	private void SpiderMorphEndSFX()
	{
		AudioManager.Play("devil_spider_morph_end");
		this.emitAudioFromObject.Add("devil_spider_morph_end");
	}

	// Token: 0x06001AE1 RID: 6881 RVA: 0x000F5A4F File Offset: 0x000F3E4F
	private void DevilPhase1DeathSFX()
	{
		AudioManager.Play("devil_phase_1_death_start");
		this.emitAudioFromObject.Add("devil_phase_1_death_start");
	}

	// Token: 0x06001AE2 RID: 6882 RVA: 0x000F5A6B File Offset: 0x000F3E6B
	private void DragonMorphEndSFX()
	{
		AudioManager.Play("devil_dragon_morph_end");
		this.emitAudioFromObject.Add("devil_dragon_morph_end");
	}

	// Token: 0x06001AE3 RID: 6883 RVA: 0x000F5A87 File Offset: 0x000F3E87
	private void HandclapSnakeSFX()
	{
		AudioManager.Play("devil_dragon_start");
		this.emitAudioFromObject.Add("devil_dragon_start");
	}

	// Token: 0x06001AE4 RID: 6884 RVA: 0x000F5AA3 File Offset: 0x000F3EA3
	private void IntroPupilsSFX()
	{
		AudioManager.Play("devil_intro_pupils");
		this.emitAudioFromObject.Add("devil_intro_pupils");
	}

	// Token: 0x06001AE5 RID: 6885 RVA: 0x000F5ABF File Offset: 0x000F3EBF
	private void RamMorphStartSFX()
	{
		AudioManager.Play("devil_ram_morph_start");
		this.emitAudioFromObject.Add("devil_ram_morph_start");
	}

	// Token: 0x06001AE6 RID: 6886 RVA: 0x000F5ADB File Offset: 0x000F3EDB
	private void RamMorphEndSFX()
	{
		AudioManager.Play("devil_ram_morph_end");
		this.emitAudioFromObject.Add("devil_ram_morph_end");
	}

	// Token: 0x06001AE7 RID: 6887 RVA: 0x000F5AF7 File Offset: 0x000F3EF7
	private void StartTridentHeadSFX()
	{
		AudioManager.Play("devil_trident_head");
		this.emitAudioFromObject.Add("devil_trident_head");
	}

	// Token: 0x06001AE8 RID: 6888 RVA: 0x000F5B13 File Offset: 0x000F3F13
	private void EndTridentHeadSFX()
	{
	}

	// Token: 0x06001AE9 RID: 6889 RVA: 0x000F5B15 File Offset: 0x000F3F15
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001AEA RID: 6890 RVA: 0x000F5B28 File Offset: 0x000F3F28
	public void ShowGoSign()
	{
		this.holeSign.SetActive(true);
	}

	// Token: 0x040023F3 RID: 9203
	public DevilLevelSittingDevil.State state;

	// Token: 0x040023F4 RID: 9204
	[SerializeField]
	private GameObject middleGround;

	// Token: 0x040023F5 RID: 9205
	[SerializeField]
	private DevilLevelGiantHead giantHead;

	// Token: 0x040023F6 RID: 9206
	[SerializeField]
	private DevilLevelDemon demonPrefab;

	// Token: 0x040023F7 RID: 9207
	[SerializeField]
	private Transform leftDemonPeek;

	// Token: 0x040023F8 RID: 9208
	[SerializeField]
	private Transform leftDemonJumpRoot;

	// Token: 0x040023F9 RID: 9209
	[SerializeField]
	private Transform leftDemonRunRoot;

	// Token: 0x040023FA RID: 9210
	[SerializeField]
	private Transform leftDemonPillar;

	// Token: 0x040023FB RID: 9211
	[SerializeField]
	private Transform leftDemonFront;

	// Token: 0x040023FC RID: 9212
	[SerializeField]
	private Transform rightDemonPeek;

	// Token: 0x040023FD RID: 9213
	[SerializeField]
	private Transform rightDemonJumpRoot;

	// Token: 0x040023FE RID: 9214
	[SerializeField]
	private Transform rightDemonRunRoot;

	// Token: 0x040023FF RID: 9215
	[SerializeField]
	private Transform rightDemonPillar;

	// Token: 0x04002400 RID: 9216
	[SerializeField]
	private Transform rightDemonFront;

	// Token: 0x04002401 RID: 9217
	[SerializeField]
	private DevilLevelDevilArm[] arms;

	// Token: 0x04002402 RID: 9218
	[SerializeField]
	private DevilLevelSpiderHead spiderHead;

	// Token: 0x04002403 RID: 9219
	[SerializeField]
	private DevilLevelDragonHead dragonHead;

	// Token: 0x04002404 RID: 9220
	[SerializeField]
	private Transform leftWall;

	// Token: 0x04002405 RID: 9221
	private float leftWallPositionX;

	// Token: 0x04002406 RID: 9222
	[SerializeField]
	private Transform rightWall;

	// Token: 0x04002407 RID: 9223
	private float rightWallPositionX;

	// Token: 0x04002408 RID: 9224
	[SerializeField]
	private DevilLevelPitchforkWheelProjectile wheelProjectilePrefab;

	// Token: 0x04002409 RID: 9225
	[SerializeField]
	private DevilLevelPitchforkOrbitingProjectile wheelOrbitingProjectilePrefab;

	// Token: 0x0400240A RID: 9226
	[SerializeField]
	private DevilLevelPitchforkJumpingProjectile jumpingProjectilePrefab;

	// Token: 0x0400240B RID: 9227
	[SerializeField]
	private DevilLevelPitchforkBouncingProjectile bouncingProjectilePrefab;

	// Token: 0x0400240C RID: 9228
	[SerializeField]
	private DevilLevelPitchforkSpinnerProjectile spinnerProjectilePrefab;

	// Token: 0x0400240D RID: 9229
	[SerializeField]
	private DevilLevelPitchforkOrbitingProjectile spinnerOrbitingProjectilePrefab;

	// Token: 0x0400240E RID: 9230
	[SerializeField]
	private DevilLevelPitchforkRingProjectile ringProjectilePrefab;

	// Token: 0x0400240F RID: 9231
	[SerializeField]
	private GameObject holeSign;

	// Token: 0x04002410 RID: 9232
	private Vector3 dragonPos;

	// Token: 0x04002411 RID: 9233
	private DamageReceiver damageReceiver;

	// Token: 0x04002412 RID: 9234
	private bool isSpiderAttackNext;

	// Token: 0x04002413 RID: 9235
	private bool endFire;

	// Token: 0x04002414 RID: 9236
	private bool endPH1;

	// Token: 0x04002415 RID: 9237
	private int spiderOffsetIndex;

	// Token: 0x04002416 RID: 9238
	private string[] spiderOffsets;

	// Token: 0x04002417 RID: 9239
	private int pitchforkPatternIndex;

	// Token: 0x04002418 RID: 9240
	private string[] pitchforkPattern;

	// Token: 0x04002419 RID: 9241
	private DevilLevelPitchforkProjectileSpawner pitchforkTwoFlameWheelSpawner;

	// Token: 0x0400241A RID: 9242
	private DevilLevelPitchforkProjectileSpawner pitchforkThreeFlameJumperSpawner;

	// Token: 0x0400241B RID: 9243
	private DevilLevelPitchforkProjectileSpawner pitchforkFourFlameBouncerSpawner;

	// Token: 0x0400241C RID: 9244
	private DevilLevelPitchforkProjectileSpawner pitchforkFiveFlameSpinnerSpawner;

	// Token: 0x0400241D RID: 9245
	private DevilLevelPitchforkProjectileSpawner pitchforkSixFlameRingSpawner;

	// Token: 0x0400241E RID: 9246
	public Action OnPhase1Death;

	// Token: 0x02000580 RID: 1408
	public enum State
	{
		// Token: 0x04002420 RID: 9248
		Intro,
		// Token: 0x04002421 RID: 9249
		Idle,
		// Token: 0x04002422 RID: 9250
		Clap,
		// Token: 0x04002423 RID: 9251
		Head,
		// Token: 0x04002424 RID: 9252
		Pitchfork,
		// Token: 0x04002425 RID: 9253
		EndPhase1
	}
}
