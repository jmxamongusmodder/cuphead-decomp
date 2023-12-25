using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007B5 RID: 1973
public class SallyStagePlayLevelSally : LevelProperties.SallyStagePlay.Entity
{
	// Token: 0x1700040A RID: 1034
	// (get) Token: 0x06002C6F RID: 11375 RVA: 0x001A1CCE File Offset: 0x001A00CE
	// (set) Token: 0x06002C70 RID: 11376 RVA: 0x001A1CD6 File Offset: 0x001A00D6
	public SallyStagePlayLevelSally.State state { get; private set; }

	// Token: 0x06002C71 RID: 11377 RVA: 0x001A1CE0 File Offset: 0x001A00E0
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.isInvincible)
		{
			return;
		}
		base.properties.DealDamage(info.damage);
		if (!SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE && this.husband.gameObject.activeInHierarchy)
		{
			if (!AudioManager.CheckIfPlaying("sally_bg_church_fiance_ohno"))
			{
				AudioManager.Play("sally_bg_church_fiance_ohno");
				this.emitAudioFromObject.Add("sally_bg_church_fiance_ohno");
			}
			this.husband.GetComponent<Animator>().Play("OhNo");
		}
	}

	// Token: 0x06002C72 RID: 11378 RVA: 0x001A1D68 File Offset: 0x001A0168
	public override void LevelInit(LevelProperties.SallyStagePlay properties)
	{
		base.LevelInit(properties);
		this.bounds = base.GetComponent<BoxCollider2D>().bounds.size;
		this.jumpTypeIndex = UnityEngine.Random.Range(0, properties.CurrentState.jump.JumpAttackString.Split(new char[]
		{
			','
		}).Length);
		this.jumpCountIndex = UnityEngine.Random.Range(0, properties.CurrentState.jump.JumpAttackCountString.Split(new char[]
		{
			','
		}).Length);
		this.jumpRollAttackTypeIndex = UnityEngine.Random.Range(0, properties.CurrentState.jumpRoll.JumpAttackTypeString.Split(new char[]
		{
			','
		}).Length);
		this.heartTypeIndex = UnityEngine.Random.Range(0, properties.CurrentState.kiss.heartType.Split(new char[]
		{
			','
		}).Length);
		this.teleportOffsetIndex = UnityEngine.Random.Range(0, properties.CurrentState.teleport.appearOffsetString.Split(new char[]
		{
			','
		}).Length);
		base.transform.position = this.ground;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06002C73 RID: 11379 RVA: 0x001A1E9E File Offset: 0x001A029E
	public void GetParent(SallyStagePlayLevel parent)
	{
		parent.OnPhase2 += this.OnPhase2;
	}

	// Token: 0x06002C74 RID: 11380 RVA: 0x001A1EB4 File Offset: 0x001A02B4
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
		this.collisionChild.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.ground = new Vector3(base.transform.position.x, (float)Level.Current.Ground + 300f, 0f);
	}

	// Token: 0x06002C75 RID: 11381 RVA: 0x001A1F60 File Offset: 0x001A0360
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		this.ground.x = base.transform.position.x;
	}

	// Token: 0x06002C76 RID: 11382 RVA: 0x001A1FA1 File Offset: 0x001A03A1
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002C77 RID: 11383 RVA: 0x001A1FC0 File Offset: 0x001A03C0
	private IEnumerator intro_cr()
	{
		this.state = SallyStagePlayLevelSally.State.Intro;
		yield return CupheadTime.WaitForSeconds(this, 2f);
		base.animator.SetTrigger("Continue");
		AudioManager.Play("sally_sally_intro_phase1");
		this.emitAudioFromObject.Add("sally_sally_intro_phase1");
		this.state = SallyStagePlayLevelSally.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06002C78 RID: 11384 RVA: 0x001A1FDC File Offset: 0x001A03DC
	public void OnJumpAttack()
	{
		this.state = SallyStagePlayLevelSally.State.Attack;
		this.jumpType = (SallyStagePlayLevelSally.JumpType)Parser.IntParse(base.properties.CurrentState.jump.JumpAttackString.Split(new char[]
		{
			','
		})[this.jumpTypeIndex]);
		this.target = PlayerManager.GetNext();
		this.currentJumpAttackCount = 0;
		base.StartCoroutine(this.jump_cr());
		this.jumpCountIndex = (this.jumpCountIndex + 1) % base.properties.CurrentState.jump.JumpAttackCountString.Split(new char[]
		{
			','
		}).Length;
	}

	// Token: 0x06002C79 RID: 11385 RVA: 0x001A207C File Offset: 0x001A047C
	private IEnumerator jump_cr()
	{
		if (this.currentJumpAttackCount >= Parser.IntParse(base.properties.CurrentState.jump.JumpAttackCountString.Split(new char[]
		{
			','
		})[this.jumpCountIndex]))
		{
			float rand = base.properties.CurrentState.jump.JumpHesitate.RandomFloat();
			if (rand > 0f)
			{
				yield return CupheadTime.WaitForSeconds(this, rand);
			}
			this.state = SallyStagePlayLevelSally.State.Idle;
			yield return null;
		}
		else
		{
			this.jumpTypeIndex = (this.jumpTypeIndex + 1) % base.properties.CurrentState.jump.JumpAttackString.Split(new char[]
			{
				','
			}).Length;
			this.jumpType = (SallyStagePlayLevelSally.JumpType)Parser.IntParse(base.properties.CurrentState.jump.JumpAttackString.Split(new char[]
			{
				','
			})[this.jumpTypeIndex]);
			if (this.currentJumpAttackCount > 0 && base.properties.CurrentState.jump.JumpDelay > 0f)
			{
				yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.jump.JumpDelay);
			}
			base.animator.SetInteger("JumpType", (int)this.jumpType);
			base.animator.SetTrigger("Jump");
			yield return base.animator.WaitForAnimationToEnd(this, "Jump_TakeOff", true, true);
			Vector3 start = base.transform.position;
			Vector3 end = start;
			SallyStagePlayLevelSally.JumpType jumpType = this.jumpType;
			if (jumpType != SallyStagePlayLevelSally.JumpType.DiveKick)
			{
				if (jumpType == SallyStagePlayLevelSally.JumpType.DoubleJump)
				{
					end += Vector3.up * base.properties.CurrentState.jumpRoll.JumpHeight.RandomFloat();
				}
			}
			else
			{
				end += Vector3.up * base.properties.CurrentState.diveKick.DiveAttackHeight.RandomFloat();
			}
			base.StartCoroutine(this.shadow_cr(true));
			float timePassed = 0f;
			while (timePassed / 0.1665f < 1f)
			{
				if (CupheadTime.Delta != 0f)
				{
					base.transform.position = start + (end - start) * (timePassed / 0.1665f);
					timePassed += CupheadTime.Delta;
				}
				yield return null;
			}
			base.animator.SetTrigger("OnAttack");
			this.currentJumpAttackCount++;
			this.StartJumpAttack(this.jumpType);
		}
		yield break;
	}

	// Token: 0x06002C7A RID: 11386 RVA: 0x001A2097 File Offset: 0x001A0497
	private void StartJumpAttack(SallyStagePlayLevelSally.JumpType type)
	{
		if (type != SallyStagePlayLevelSally.JumpType.DiveKick)
		{
			if (type == SallyStagePlayLevelSally.JumpType.DoubleJump)
			{
				base.StartCoroutine(this.jumpRoll_cr());
			}
		}
		else
		{
			base.StartCoroutine(this.diveKick_cr());
		}
	}

	// Token: 0x06002C7B RID: 11387 RVA: 0x001A20D8 File Offset: 0x001A04D8
	private IEnumerator landing_cr(bool useTrigger = true)
	{
		base.StartCoroutine(this.shadow_cr(false));
		if (this.target == null || this.target.IsDead)
		{
			this.target = PlayerManager.GetNext();
		}
		if (this.target.center.x > base.transform.position.x)
		{
			if (base.transform.right.x > 0f)
			{
				if (useTrigger)
				{
					yield return new WaitForEndOfFrame();
					base.animator.SetTrigger("OnTurnLanding");
					yield return base.animator.WaitForAnimationToEnd(this, true);
					base.transform.right *= -1f;
					yield return base.animator.WaitForAnimationToEnd(this, "Land_and_Turn", false, true);
				}
				else
				{
					yield return new WaitForEndOfFrame();
					base.animator.Play("Land_and_Turn");
					yield return base.animator.WaitForAnimationToEnd(this, true);
					base.transform.right *= -1f;
					yield return base.animator.WaitForAnimationToEnd(this, "Land_and_Turn", false, true);
				}
			}
			else if (useTrigger)
			{
				base.animator.SetTrigger("OnLanding");
				yield return base.animator.WaitForAnimationToEnd(this, "Land", true, true);
			}
			else
			{
				base.animator.Play("Land");
				yield return base.animator.WaitForAnimationToEnd(this, "Land", true, true);
			}
		}
		else if (base.transform.right.x < 0f)
		{
			if (useTrigger)
			{
				yield return new WaitForEndOfFrame();
				base.animator.SetTrigger("OnTurnLanding");
				yield return base.animator.WaitForAnimationToEnd(this, true);
				base.transform.right *= -1f;
				yield return base.animator.WaitForAnimationToEnd(this, "Land_and_Turn", false, true);
			}
			else
			{
				yield return new WaitForEndOfFrame();
				base.animator.Play("Land_and_Turn");
				yield return base.animator.WaitForAnimationToEnd(this, true);
				base.transform.right *= -1f;
				yield return base.animator.WaitForAnimationToEnd(this, "Land_and_Turn", false, true);
			}
		}
		else if (useTrigger)
		{
			base.animator.SetTrigger("OnLanding");
			yield return base.animator.WaitForAnimationToEnd(this, "Land", true, true);
		}
		else
		{
			base.animator.Play("Land");
			yield return base.animator.WaitForAnimationToEnd(this, "Land", true, true);
		}
		if (!this.getOutOfJump)
		{
			base.StartCoroutine(this.jump_cr());
		}
		else
		{
			this.state = SallyStagePlayLevelSally.State.Idle;
		}
		yield break;
	}

	// Token: 0x06002C7C RID: 11388 RVA: 0x001A20FA File Offset: 0x001A04FA
	private void LandSFX()
	{
		AudioManager.Play("sally_sally_land");
		this.emitAudioFromObject.Add("sally_sally_land");
	}

	// Token: 0x06002C7D RID: 11389 RVA: 0x001A2118 File Offset: 0x001A0518
	private IEnumerator shadow_cr(bool fadeOut = true)
	{
		GameObject shadow = UnityEngine.Object.Instantiate<GameObject>(this.shadowPrefab, new Vector3(base.transform.position.x, (float)Level.Current.Ground, 0f), Quaternion.identity);
		if (fadeOut)
		{
			shadow.GetComponent<Animator>().Play("FadeOut");
		}
		else
		{
			shadow.GetComponent<Animator>().Play("FadeIn");
		}
		yield return shadow.GetComponent<Animator>().WaitForAnimationToEnd(this, true);
		UnityEngine.Object.Destroy(shadow);
		yield break;
	}

	// Token: 0x06002C7E RID: 11390 RVA: 0x001A213C File Offset: 0x001A053C
	private IEnumerator diveKick_cr()
	{
		base.animator.Play("DiveKick_Transition");
		Vector2 direction = -base.transform.right;
		float angle = (float)base.properties.CurrentState.diveKick.DiveAngleRange.RandomInt() / 100f;
		if (angle == 0f)
		{
			angle = 0.001f;
		}
		direction.x = direction.x * Mathf.Cos(angle) - direction.y * Mathf.Sin(angle);
		direction.y = direction.x * Mathf.Sin(angle) + direction.y * Mathf.Cos(angle);
		direction.y = -Mathf.Abs(direction.y);
		bool attacking = true;
		AudioManager.Play("sally_divekick_loop");
		this.emitAudioFromObject.Add("sally_divekick_loop");
		Vector3 deltaPosition = Vector3.zero;
		while (attacking)
		{
			if (CupheadTime.Delta != 0f)
			{
				deltaPosition = Vector3.zero;
			}
			if (Mathf.Sign(direction.x) > 0f)
			{
				if (base.transform.position.x + this.bounds.x / 2f < (float)Level.Current.Right)
				{
					deltaPosition.x = direction.x * base.properties.CurrentState.diveKick.DiveSpeed * CupheadTime.Delta;
				}
			}
			else if (base.transform.position.x - this.bounds.x / 2f > (float)Level.Current.Left)
			{
				deltaPosition.x = direction.x * base.properties.CurrentState.diveKick.DiveSpeed * CupheadTime.Delta;
			}
			if (base.transform.position.y > this.ground.y)
			{
				deltaPosition.y = Mathf.Sign(direction.y) * base.properties.CurrentState.diveKick.DiveSpeed * CupheadTime.Delta;
			}
			else
			{
				deltaPosition.y = 0f;
			}
			if (deltaPosition.y == 0f)
			{
				if (CupheadTime.Delta != 0f)
				{
					attacking = false;
				}
			}
			else
			{
				base.transform.position += deltaPosition;
			}
			yield return null;
		}
		base.StartCoroutine(this.landing_cr(false));
		yield break;
	}

	// Token: 0x06002C7F RID: 11391 RVA: 0x001A2158 File Offset: 0x001A0558
	private IEnumerator jumpRoll_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "JumpRoll_Transition", true, true);
		if (!this.getOutOfJump)
		{
			base.StartCoroutine(this.rollAttack_cr());
			AudioManager.PlayLoop("sally_double_jump_roll_loop");
			this.emitAudioFromObject.Add("sally_double_jump_roll_loop");
		}
		Vector3 start = base.transform.position;
		Vector3 end = start + Vector3.up * base.properties.CurrentState.jumpRoll.RollJumpVerticalMovement;
		end += -base.transform.right * base.properties.CurrentState.jumpRoll.RollJumpHorizontalMovement.RandomFloat();
		if (end.x - this.bounds.x / 2f < (float)Level.Current.Left)
		{
			end.x = (float)Level.Current.Left + this.bounds.x / 2f;
		}
		else if (end.x + this.bounds.x / 2f > (float)Level.Current.Right)
		{
			end.x = (float)Level.Current.Right - this.bounds.x / 2f;
		}
		float pct = 0f;
		while (pct < base.properties.CurrentState.jumpRoll.JumpRollDuration)
		{
			base.transform.position = start + (end - start) * pct;
			pct += CupheadTime.Delta;
			yield return null;
		}
		yield return base.animator.WaitForAnimationToEnd(this, "JumpRoll_Roll", true, true);
		AudioManager.Stop("sally_double_jump_roll_loop");
		base.StartCoroutine(this.fall_cr());
		this.jumpRollAttackTypeIndex++;
		if (this.jumpRollAttackTypeIndex >= base.properties.CurrentState.jumpRoll.JumpAttackTypeString.Split(new char[]
		{
			','
		}).Length)
		{
			this.jumpRollAttackTypeIndex = 0;
		}
		yield break;
	}

	// Token: 0x06002C80 RID: 11392 RVA: 0x001A2174 File Offset: 0x001A0574
	private IEnumerator fall_cr()
	{
		float speed = base.properties.CurrentState.teleport.fallingSpeed.RandomFloat();
		int iteration = 1;
		float offset = 150f;
		bool useTrigger = false;
		if (this.isTeleporting)
		{
			offset = 180f;
			useTrigger = true;
		}
		else
		{
			offset = this.ground.y;
			useTrigger = false;
		}
		while (base.transform.position.y > offset)
		{
			base.transform.position += Vector3.down * speed * CupheadTime.Delta;
			if (CupheadTime.Delta != 0f)
			{
				speed += base.properties.CurrentState.teleport.acceleration * (float)iteration;
				iteration++;
			}
			yield return null;
		}
		base.transform.position = new Vector3(base.transform.position.x, offset, 0f);
		if (this.isTeleporting)
		{
			base.animator.SetTrigger("OnSawEnd");
		}
		base.StartCoroutine(this.landing_cr(useTrigger));
		yield return null;
		yield break;
	}

	// Token: 0x06002C81 RID: 11393 RVA: 0x001A2190 File Offset: 0x001A0590
	private IEnumerator rollAttack_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.jumpRoll.RollShotDelayRange.RandomFloat());
		char c = base.properties.CurrentState.jumpRoll.JumpAttackTypeString.Split(new char[]
		{
			','
		})[this.jumpRollAttackTypeIndex][0];
		if (c != 'S')
		{
			if (c == 'B')
			{
				this.SpawnProjectile();
			}
		}
		else
		{
			this.SpawnShuriken();
		}
		yield break;
	}

	// Token: 0x06002C82 RID: 11394 RVA: 0x001A21AC File Offset: 0x001A05AC
	private void SpawnShuriken()
	{
		for (int i = -1; i < 1; i++)
		{
			AbstractProjectile abstractProjectile = this.shurikenPrefab.Create(base.transform.position + Vector3.up * 0.5f);
			abstractProjectile.GetComponent<SallyStagePlayLevelShurikenBomb>().InitShuriken(base.properties, i, this.target);
		}
	}

	// Token: 0x06002C83 RID: 11395 RVA: 0x001A2214 File Offset: 0x001A0614
	private void SpawnProjectile()
	{
		Vector3 v = this.target.transform.position - this.centerPoint.transform.position;
		SallyStagePlayLevelProjectile sallyStagePlayLevelProjectile = UnityEngine.Object.Instantiate<SallyStagePlayLevelProjectile>(this.projectilePrefab);
		sallyStagePlayLevelProjectile.Init(this.centerPoint.transform.position, MathUtils.DirectionToAngle(v), base.properties.CurrentState.projectile);
	}

	// Token: 0x06002C84 RID: 11396 RVA: 0x001A2289 File Offset: 0x001A0689
	public void OnUmbrellaAttack()
	{
		this.state = SallyStagePlayLevelSally.State.Attack;
		base.StartCoroutine(this.startUmbrella_cr());
	}

	// Token: 0x06002C85 RID: 11397 RVA: 0x001A22A0 File Offset: 0x001A06A0
	private IEnumerator startUmbrella_cr()
	{
		base.animator.SetBool("UmbrellaAttack", true);
		yield return base.animator.WaitForAnimationToEnd(this, "Umbrella_Spin_Start", false, true);
		AudioManager.Play("sally_umbrella_spin");
		this.emitAudioFromObject.Add("sally_umbrella_spin");
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.umbrella.initialAttackDelay);
		for (int i = 0; i < base.properties.CurrentState.umbrella.objectCount; i++)
		{
			if (i != 0)
			{
				yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.umbrella.objectDelay);
			}
			AudioManager.Play("sally_umbrella_spin_shoot");
			this.emitAudioFromObject.Add("sally_umbrella_spin_shoot");
			AbstractProjectile proj = this.umbrellaProjectilePrefab.Create(this.spawnPoints[0].position);
			proj.GetComponent<SallyStagePlayLevelUmbrellaProjectile>().InitProjectile(base.properties, (int)(-(int)base.transform.right.x));
			proj = this.umbrellaProjectilePrefab.Create(this.spawnPoints[1].position);
			proj.GetComponent<SallyStagePlayLevelUmbrellaProjectile>().InitProjectile(base.properties, (int)base.transform.right.x);
			if (this.getOutOfJump)
			{
				break;
			}
		}
		base.animator.SetBool("UmbrellaAttack", false);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.umbrella.hesitate);
		AudioManager.Play("sally_umbrella_spin_end");
		this.emitAudioFromObject.Add("sally_umbrella_spin_end");
		this.state = SallyStagePlayLevelSally.State.Idle;
		yield break;
	}

	// Token: 0x06002C86 RID: 11398 RVA: 0x001A22BB File Offset: 0x001A06BB
	private void UmbrellaIntroSFX()
	{
		AudioManager.Play("sally_sally_umbrella_intro");
		this.emitAudioFromObject.Add("sally_sally_umbrella_intro");
	}

	// Token: 0x06002C87 RID: 11399 RVA: 0x001A22D8 File Offset: 0x001A06D8
	public void OnKissAttack()
	{
		this.state = SallyStagePlayLevelSally.State.Attack;
		base.animator.SetTrigger("OnKissAttack");
		this.target = PlayerManager.GetNext();
		if (this.target.center.x > this.centerPoint.position.x)
		{
			if (base.transform.eulerAngles.y == 0f)
			{
				base.transform.right *= -1f;
			}
		}
		else if (base.transform.eulerAngles.y == 180f)
		{
			base.transform.right *= -1f;
		}
	}

	// Token: 0x06002C88 RID: 11400 RVA: 0x001A23A8 File Offset: 0x001A07A8
	private void SpawnHeart()
	{
		AbstractProjectile abstractProjectile = this.heartPrefab.Create(this.spawnPoints[0].position);
		bool isParryable = base.properties.CurrentState.kiss.heartType.Split(new char[]
		{
			','
		})[this.heartTypeIndex][0] != 'R';
		int direction = ((int)base.transform.eulerAngles.y != 180) ? 1 : -1;
		abstractProjectile.GetComponent<SallyStagePlayLevelHeart>().InitHeart(base.properties, direction, isParryable);
		abstractProjectile.GetComponent<Transform>().SetScale(new float?((float)((base.transform.right.x <= 0f) ? -1 : 1)), null, null);
		this.heartTypeIndex++;
		if (this.heartTypeIndex >= base.properties.CurrentState.kiss.heartType.Split(new char[]
		{
			','
		}).Length)
		{
			this.heartTypeIndex = 0;
		}
		base.StartCoroutine(this.endKiss_cr());
	}

	// Token: 0x06002C89 RID: 11401 RVA: 0x001A24EB File Offset: 0x001A08EB
	private void KissSFX()
	{
		AudioManager.Play("sally_sally_kiss");
		this.emitAudioFromObject.Add("sally_sally_kiss");
	}

	// Token: 0x06002C8A RID: 11402 RVA: 0x001A2508 File Offset: 0x001A0908
	private IEnumerator endKiss_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.kiss.hesitate);
		this.state = SallyStagePlayLevelSally.State.Idle;
		yield break;
	}

	// Token: 0x06002C8B RID: 11403 RVA: 0x001A2523 File Offset: 0x001A0923
	public void OnTeleportAttack()
	{
		this.state = SallyStagePlayLevelSally.State.Attack;
		this.isTeleporting = true;
		base.animator.SetTrigger("OnTeleport");
	}

	// Token: 0x06002C8C RID: 11404 RVA: 0x001A2544 File Offset: 0x001A0944
	private void Teleport()
	{
		base.transform.SetPosition(null, new float?((float)Level.Current.Ceiling + this.teleportOffset), null);
		base.StartCoroutine(this.delay_cr());
	}

	// Token: 0x06002C8D RID: 11405 RVA: 0x001A2594 File Offset: 0x001A0994
	private IEnumerator delay_cr()
	{
		Vector3 pos;
		pos.y = (float)Level.Current.Ceiling + this.teleportOffset;
		pos.z = 0f;
		base.animator.SetTrigger("OnTeleport");
		yield return base.animator.WaitForAnimationToStart(this, "Teleport_Loop", false);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.teleport.offScreenDelay);
		this.target = PlayerManager.GetNext();
		pos.x = this.target.center.x + (float)Parser.IntParse(base.properties.CurrentState.teleport.appearOffsetString.Split(new char[]
		{
			','
		})[this.teleportOffsetIndex]);
		if (Parser.IntParse(base.properties.CurrentState.teleport.appearOffsetString.Split(new char[]
		{
			','
		})[this.teleportOffsetIndex]) <= 0)
		{
			if (pos.x - 75f < (float)Level.Current.Left)
			{
				pos.x = (float)(Level.Current.Left + 75);
			}
			base.transform.right *= -1f;
		}
		else
		{
			if (pos.x + 75f > (float)Level.Current.Right)
			{
				pos.x = (float)(Level.Current.Right - 75);
			}
			base.transform.right *= 1f;
		}
		base.transform.position = pos;
		base.StartCoroutine(this.fall_cr());
		yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		this.teleportOffsetIndex++;
		if (this.teleportOffsetIndex >= base.properties.CurrentState.teleport.appearOffsetString.Split(new char[]
		{
			','
		}).Length)
		{
			this.teleportOffsetIndex = 0;
		}
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.teleport.hesitate);
		base.transform.position = this.ground;
		this.isTeleporting = false;
		yield return null;
		yield break;
	}

	// Token: 0x06002C8E RID: 11406 RVA: 0x001A25AF File Offset: 0x001A09AF
	private void MovePosition()
	{
		base.StartCoroutine(this.move_position_cr());
	}

	// Token: 0x06002C8F RID: 11407 RVA: 0x001A25C0 File Offset: 0x001A09C0
	private IEnumerator move_position_cr()
	{
		Vector3 pos = base.transform.position;
		float speed = 700f;
		while (base.transform.position.y > this.ground.y)
		{
			pos.y -= speed * CupheadTime.Delta;
			base.transform.position = pos;
			yield return null;
		}
		base.transform.position = this.ground;
		yield return null;
		yield break;
	}

	// Token: 0x06002C90 RID: 11408 RVA: 0x001A25DB File Offset: 0x001A09DB
	private void TeleportOutSFX()
	{
		AudioManager.Play("sally_sally_teleport_out");
		this.emitAudioFromObject.Add("sally_sally_teleport_out");
	}

	// Token: 0x06002C91 RID: 11409 RVA: 0x001A25F7 File Offset: 0x001A09F7
	private void TeleportEndSFX()
	{
		AudioManager.Play("sally_sally_teleport_end");
		this.emitAudioFromObject.Add("sally_sally_teleport_end");
	}

	// Token: 0x06002C92 RID: 11410 RVA: 0x001A2613 File Offset: 0x001A0A13
	public void OnPhase3(bool killedHusband)
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.phase2_death_cr(killedHusband));
	}

	// Token: 0x06002C93 RID: 11411 RVA: 0x001A262C File Offset: 0x001A0A2C
	private IEnumerator phase2_death_cr(bool killedHusband)
	{
		float speed = 300f;
		base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		base.animator.SetTrigger("OnDeath");
		AudioManager.Play("sally_vox_death_cry");
		this.emitAudioFromObject.Add("sally_vox_death_cry");
		yield return base.animator.WaitForAnimationToEnd(this, "Death_Ph2_Start", false, true);
		while (base.transform.position.y < 660f)
		{
			base.transform.position += Vector3.up * speed * CupheadTime.Delta;
			yield return null;
		}
		base.GetComponent<LevelBossDeathExploder>().StopExplosions();
		this.angel.StartPhase3(killedHusband);
		UnityEngine.Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x06002C94 RID: 11412 RVA: 0x001A264E File Offset: 0x001A0A4E
	public void PrePhase2()
	{
		this.getOutOfJump = true;
		this.isInvincible = true;
	}

	// Token: 0x06002C95 RID: 11413 RVA: 0x001A265E File Offset: 0x001A0A5E
	public void OnPhase2()
	{
		this.state = SallyStagePlayLevelSally.State.Transition;
		this.jumpTypeIndex = 0;
		this.jumpRollAttackTypeIndex = 0;
	}

	// Token: 0x06002C96 RID: 11414 RVA: 0x001A2675 File Offset: 0x001A0A75
	public void StartPhase2()
	{
		this.getOutOfJump = true;
		base.animator.SetTrigger("OnIntro");
		base.StartCoroutine(this.phase_2_cr());
	}

	// Token: 0x06002C97 RID: 11415 RVA: 0x001A269B File Offset: 0x001A0A9B
	private void Intro2SFX()
	{
		AudioManager.Play("sally_sally_intro_phase2");
		this.emitAudioFromObject.Add("sally_sally_intro_phase2");
	}

	// Token: 0x06002C98 RID: 11416 RVA: 0x001A26B8 File Offset: 0x001A0AB8
	private IEnumerator phase_2_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "Teleport_GONE", false, true);
		base.StartCoroutine(this.slide_cr());
		yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		this.isInvincible = false;
		this.getOutOfJump = false;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.state = SallyStagePlayLevelSally.State.Idle;
		this.house.StartAttacks();
		yield return null;
		yield break;
	}

	// Token: 0x06002C99 RID: 11417 RVA: 0x001A26D4 File Offset: 0x001A0AD4
	private IEnumerator slide_cr()
	{
		float startPos = 0f;
		float endPos = 0f;
		float appearPos = 300f;
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player2 == null || player.IsDead || player2.IsDead)
		{
			if (this.target == null || this.target.IsDead)
			{
				this.target = PlayerManager.GetNext();
			}
			if (this.target.transform.position.x > 0f)
			{
				if (base.transform.right.x > 0f)
				{
					base.transform.right *= -1f;
				}
				startPos = -840f;
				endPos = -640f + appearPos;
			}
			else
			{
				if (base.transform.right.x < 0f)
				{
					base.transform.right *= -1f;
				}
				startPos = 840f;
				endPos = 640f - appearPos;
			}
		}
		else
		{
			float num = -640f - player.transform.position.x;
			float num2 = 640f - player.transform.position.x;
			float num3 = -640f - player2.transform.position.x;
			float num4 = 640f - player2.transform.position.x;
			if (player.transform.position.x < 0f)
			{
				if (player2.transform.position.x < 0f)
				{
					if (base.transform.right.x < 0f)
					{
						base.transform.right *= -1f;
					}
					startPos = 840f;
					endPos = 640f - appearPos;
				}
				else if (num < num4)
				{
					if (base.transform.right.x < 0f)
					{
						base.transform.right *= -1f;
					}
					startPos = 840f;
					endPos = 640f - appearPos;
				}
				else
				{
					if (base.transform.right.x > 0f)
					{
						base.transform.right *= -1f;
					}
					startPos = -840f;
					endPos = -640f + appearPos;
				}
			}
			else if (player2.transform.position.x > 0f)
			{
				if (base.transform.right.x > 0f)
				{
					base.transform.right *= -1f;
				}
				startPos = -840f;
				endPos = -640f + appearPos;
			}
			else if (num2 < num3)
			{
				if (base.transform.right.x < 0f)
				{
					base.transform.right *= -1f;
				}
				startPos = 840f;
				endPos = 640f - appearPos;
			}
			else
			{
				if (base.transform.right.x > 0f)
				{
					base.transform.right *= -1f;
				}
				startPos = -840f;
				endPos = -640f + appearPos;
			}
		}
		base.transform.position = new Vector3(startPos, base.transform.position.y, base.transform.position.z);
		float t = 0f;
		float time = 0.75f;
		YieldInstruction wait = new WaitForFixedUpdate();
		float frameTime = 0f;
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			frameTime += CupheadTime.FixedDelta;
			if (frameTime > 0.041666668f)
			{
				frameTime -= 0.041666668f;
				float t2 = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
				base.transform.SetPosition(new float?(Mathf.Lerp(startPos, endPos, t2)), null, null);
			}
			yield return wait;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002C9A RID: 11418 RVA: 0x001A26EF File Offset: 0x001A0AEF
	protected override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
		this.shurikenPrefab = null;
		this.projectilePrefab = null;
		this.umbrellaProjectilePrefab = null;
		this.heartPrefab = null;
		this.shadowPrefab = null;
	}

	// Token: 0x06002C9B RID: 11419 RVA: 0x001A2720 File Offset: 0x001A0B20
	private void SoundSallyVoxAttackMmmYoh()
	{
		AudioManager.Play("sally_vox_attack_mmm_yoh");
		this.emitAudioFromObject.Add("sally_vox_attack_mmm_yoh");
	}

	// Token: 0x06002C9C RID: 11420 RVA: 0x001A273C File Offset: 0x001A0B3C
	private void SoundSallyVoxAttackQuick()
	{
		AudioManager.Play("sally_vox_attack_quick");
		this.emitAudioFromObject.Add("sally_vox_attack_quick");
		AudioManager.Stop("sally_vox_maniacal");
	}

	// Token: 0x06002C9D RID: 11421 RVA: 0x001A2762 File Offset: 0x001A0B62
	private void SoundSallyVoxAttackDeathCry()
	{
		AudioManager.Play("sally_vox_death_cry");
		this.emitAudioFromObject.Add("sally_vox_death_cry");
	}

	// Token: 0x06002C9E RID: 11422 RVA: 0x001A277E File Offset: 0x001A0B7E
	private void SoundSallyVoxFrustrated()
	{
		AudioManager.Play("sally_vox_frustrated");
		this.emitAudioFromObject.Add("sally_vox_frustrated");
	}

	// Token: 0x06002C9F RID: 11423 RVA: 0x001A279A File Offset: 0x001A0B9A
	private void SoundSallyVoxLaughBig()
	{
		AudioManager.Play("sally_vox_laugh_big");
		this.emitAudioFromObject.Add("sally_vox_laugh_big");
	}

	// Token: 0x06002CA0 RID: 11424 RVA: 0x001A27B6 File Offset: 0x001A0BB6
	private void SoundSallyVoxLaughSmall()
	{
		AudioManager.Play("sally_vox_laugh_small");
		this.emitAudioFromObject.Add("sally_vox_laugh_small");
	}

	// Token: 0x06002CA1 RID: 11425 RVA: 0x001A27D2 File Offset: 0x001A0BD2
	private void SoundSallyVoxLaughManiacal()
	{
		AudioManager.Play("sally_vox_maniacal");
		this.emitAudioFromObject.Add("sally_vox_maniacal");
	}

	// Token: 0x06002CA2 RID: 11426 RVA: 0x001A27EE File Offset: 0x001A0BEE
	private void SoundSallyVoxDeathOperatic()
	{
		AudioManager.Play("sally_vox_operatic_death");
		this.emitAudioFromObject.Add("sally_vox_operatic_death");
	}

	// Token: 0x06002CA3 RID: 11427 RVA: 0x001A280A File Offset: 0x001A0C0A
	private void SoundSallyVoxPainGrowl()
	{
		AudioManager.Play("sally_vox_pain_growl");
		this.emitAudioFromObject.Add("sally_vox_pain_growl");
	}

	// Token: 0x04003500 RID: 13568
	[Header("Projectiles")]
	private const float FRAME_TIME = 0.041666668f;

	// Token: 0x04003501 RID: 13569
	[SerializeField]
	public CollisionChild collisionChild;

	// Token: 0x04003502 RID: 13570
	[SerializeField]
	private Transform husband;

	// Token: 0x04003503 RID: 13571
	[SerializeField]
	private SallyStagePlayLevelAngel angel;

	// Token: 0x04003504 RID: 13572
	[SerializeField]
	private SallyStagePlayLevelShurikenBomb shurikenPrefab;

	// Token: 0x04003505 RID: 13573
	[SerializeField]
	private SallyStagePlayLevelProjectile projectilePrefab;

	// Token: 0x04003506 RID: 13574
	[SerializeField]
	private SallyStagePlayLevelUmbrellaProjectile umbrellaProjectilePrefab;

	// Token: 0x04003507 RID: 13575
	[SerializeField]
	private SallyStagePlayLevelHeart heartPrefab;

	// Token: 0x04003508 RID: 13576
	[SerializeField]
	private SallyStagePlayLevelHouse house;

	// Token: 0x0400350A RID: 13578
	private const float SALLY_INIT_JUMP_TIME = 0.1665f;

	// Token: 0x0400350B RID: 13579
	private SallyStagePlayLevelSally.JumpType jumpType;

	// Token: 0x0400350C RID: 13580
	private int jumpTypeIndex;

	// Token: 0x0400350D RID: 13581
	private int jumpCountIndex;

	// Token: 0x0400350E RID: 13582
	private int currentJumpAttackCount;

	// Token: 0x0400350F RID: 13583
	private int jumpRollAttackTypeIndex;

	// Token: 0x04003510 RID: 13584
	private int heartTypeIndex;

	// Token: 0x04003511 RID: 13585
	private int teleportOffsetIndex;

	// Token: 0x04003512 RID: 13586
	private float teleportOffset = 500f;

	// Token: 0x04003513 RID: 13587
	private bool getOutOfJump;

	// Token: 0x04003514 RID: 13588
	private bool isTeleporting;

	// Token: 0x04003515 RID: 13589
	private bool isInvincible;

	// Token: 0x04003516 RID: 13590
	private Vector2 bounds;

	// Token: 0x04003517 RID: 13591
	private Vector3 ground;

	// Token: 0x04003518 RID: 13592
	private AbstractPlayerController target;

	// Token: 0x04003519 RID: 13593
	private DamageDealer damageDealer;

	// Token: 0x0400351A RID: 13594
	private DamageReceiver damageReceiver;

	// Token: 0x0400351B RID: 13595
	[Space(10f)]
	[SerializeField]
	private GameObject shadowPrefab;

	// Token: 0x0400351C RID: 13596
	[SerializeField]
	private Transform centerPoint;

	// Token: 0x0400351D RID: 13597
	[SerializeField]
	private Transform[] spawnPoints;

	// Token: 0x020007B6 RID: 1974
	public enum State
	{
		// Token: 0x0400351F RID: 13599
		Intro,
		// Token: 0x04003520 RID: 13600
		Idle,
		// Token: 0x04003521 RID: 13601
		Attack,
		// Token: 0x04003522 RID: 13602
		Transition
	}

	// Token: 0x020007B7 RID: 1975
	private enum JumpType
	{
		// Token: 0x04003524 RID: 13604
		DiveKick = 1,
		// Token: 0x04003525 RID: 13605
		DoubleJump
	}
}
