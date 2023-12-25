using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000776 RID: 1910
public class RobotLevelRobotChest : RobotLevelRobotBodyPart
{
	// Token: 0x060029B7 RID: 10679 RVA: 0x00184FE0 File Offset: 0x001833E0
	public override void InitBodyPart(RobotLevelRobot parent, LevelProperties.Robot properties, int primaryHP = 0, int secondaryHP = 1, float attackDelayMinus = 0f)
	{
		this.primaryAttackDelay = properties.CurrentState.orb.orbInitialSpawnDelay.RandomFloat();
		this.secondaryAttackDelay = properties.CurrentState.arms.attackDelayRange.RandomFloat();
		primaryHP = properties.CurrentState.orb.chestHP;
		secondaryHP = properties.CurrentState.heart.heartHP;
		attackDelayMinus = properties.CurrentState.orb.orbSpawnDelayMinus;
		this.armsTypeIndex = UnityEngine.Random.Range(0, properties.CurrentState.arms.attackString.Split(new char[]
		{
			','
		}).Length);
		this.twistyArmsPositionIndex = UnityEngine.Random.Range(0, properties.CurrentState.twistyArms.twistyPositionString.Split(new char[]
		{
			','
		}).Length);
		base.InitBodyPart(parent, properties, primaryHP, secondaryHP, attackDelayMinus);
		base.animator.Play("Porthole_Idle", 0, 0.75f);
		base.animator.Play("Off_Idle", 1, 0.75f);
		base.StartCoroutine(this.panicArmsLoop_cr());
		base.StartCoroutine(this.close_porthole_cr());
		this.StartPrimary();
		this.damageEffectRenderer = this.damageEffect.GetComponent<SpriteRenderer>();
	}

	// Token: 0x060029B8 RID: 10680 RVA: 0x0018511F File Offset: 0x0018351F
	protected override void OnPrimaryAttack()
	{
		if (this.current == RobotLevelRobotBodyPart.state.primary)
		{
			base.animator.SetTrigger("OnPortOpen");
			base.OnPrimaryAttack();
		}
	}

	// Token: 0x060029B9 RID: 10681 RVA: 0x00185142 File Offset: 0x00183542
	private void SpawnOrb()
	{
		if (this.current != RobotLevelRobotBodyPart.state.primary)
		{
			return;
		}
		base.StartCoroutine(this.spawn_orb_cr());
	}

	// Token: 0x060029BA RID: 10682 RVA: 0x00185160 File Offset: 0x00183560
	private IEnumerator spawn_orb_cr()
	{
		RobotLevelOrb orb = this.primary.GetComponent<RobotLevelOrb>().Create(this.portholeRoot.transform.position, this.portholeOffsetRoot.position);
		this.primaryAttackDelay = this.properties.CurrentState.orb.orbSpawnDelay;
		orb.InitOrb(this.properties);
		this.isAttacking = false;
		base.animator.SetTrigger("OnFirstClose");
		yield return null;
		yield break;
	}

	// Token: 0x060029BB RID: 10683 RVA: 0x0018517C File Offset: 0x0018357C
	private IEnumerator close_porthole_cr()
	{
		base.animator.SetTrigger("OnFirstClose");
		yield return null;
		yield break;
	}

	// Token: 0x060029BC RID: 10684 RVA: 0x00185198 File Offset: 0x00183598
	protected override void OnPrimaryDeath()
	{
		if (this.current != RobotLevelRobotBodyPart.state.secondary && this.currentHealth[0] <= 0f)
		{
			AudioManager.Play("robot_upper_chest_port_destroyed");
			this.emitAudioFromObject.Add("robot_upper_chest_port_destroyed");
			this.torsoTop.enabled = false;
			this.StartSecondary();
			base.GetComponent<Collider2D>().enabled = false;
			this.DeathEffect();
			this.panicArmsLoop = true;
			this.parent.animator.Play("Transition_Arms");
			foreach (SpriteRenderer spriteRenderer in base.transform.GetComponentsInChildren<SpriteRenderer>())
			{
				spriteRenderer.enabled = false;
			}
			foreach (GameObject gameObject in this.damagedPortholes)
			{
				gameObject.SetActive(true);
				gameObject.GetComponent<SpriteRenderer>().enabled = true;
			}
		}
		base.OnPrimaryDeath();
	}

	// Token: 0x060029BD RID: 10685 RVA: 0x00185288 File Offset: 0x00183688
	private void EnablePorthole()
	{
		if (this.current == RobotLevelRobotBodyPart.state.primary)
		{
			foreach (SpriteRenderer spriteRenderer in base.transform.GetComponentsInChildren<SpriteRenderer>())
			{
				spriteRenderer.enabled = true;
			}
		}
	}

	// Token: 0x060029BE RID: 10686 RVA: 0x001852CC File Offset: 0x001836CC
	private void DisablePorthole()
	{
		if (this.current == RobotLevelRobotBodyPart.state.primary)
		{
			foreach (SpriteRenderer spriteRenderer in base.transform.GetComponentsInChildren<SpriteRenderer>())
			{
				spriteRenderer.enabled = false;
			}
		}
	}

	// Token: 0x060029BF RID: 10687 RVA: 0x0018530F File Offset: 0x0018370F
	protected override void StartSecondary()
	{
		this.secondary = UnityEngine.Object.Instantiate<GameObject>(this.secondary);
		this.secondaryAnimator = this.secondary.GetComponent<Animator>();
		base.StartCoroutine(this.secondaryStart_cr());
	}

	// Token: 0x060029C0 RID: 10688 RVA: 0x00185340 File Offset: 0x00183740
	private IEnumerator secondaryStart_cr()
	{
		yield return this.parent.animator.WaitForAnimationToEnd(this.parent, "Extend", 5, true, true);
		base.StartSecondary();
		yield break;
	}

	// Token: 0x060029C1 RID: 10689 RVA: 0x0018535C File Offset: 0x0018375C
	protected override void OnSecondaryAttack()
	{
		if (!this.armsActive)
		{
			if ((float)UnityEngine.Random.Range(0, 100) <= 25f && !AudioManager.CheckIfPlaying("robot_vocals_laugh"))
			{
				AudioManager.Play("robot_vocals_laugh");
				this.emitAudioFromObject.Add("robot_vocals_laugh");
			}
			this.parent.animator.Play("Fast", 5, this.parent.animator.GetCurrentAnimatorStateInfo(5).normalizedTime % 1f);
			this.secondaryAttackDelay = 0f;
			this.armsActive = true;
			char c = this.properties.CurrentState.arms.attackString.Split(new char[]
			{
				','
			})[this.armsTypeIndex][0];
			if (c != 'M')
			{
				if (c == 'T')
				{
					base.StartCoroutine(this.twistyArmsEnter_cr());
				}
			}
			else
			{
				base.StartCoroutine(this.magnetArmsIntro_cr());
			}
		}
		base.OnSecondaryAttack();
	}

	// Token: 0x060029C2 RID: 10690 RVA: 0x00185470 File Offset: 0x00183870
	private IEnumerator twistyArmsEnter_cr()
	{
		this.secondary.GetComponent<RobotLevelSecondaryArms>().InitHelper(this.properties);
		this.secondaryAnimator.Play("Twisty_Arms_Enter", 0, 0f);
		AudioManager.Play("robot_arms_extend_appear");
		yield return null;
		this.spriteBounds = this.secondary.GetComponent<SpriteRenderer>().bounds.size;
		this.secondary.transform.position = new Vector3(-1248f, (float)(Level.Current.Ground + Parser.IntParse(this.properties.CurrentState.twistyArms.twistyPositionString.Split(new char[]
		{
			','
		})[this.twistyArmsPositionIndex])), 0f);
		this.secondary.transform.rotation = Quaternion.identity;
		while (this.secondary.transform.position.x < -1248f + this.properties.CurrentState.twistyArms.warningArmsMoveAmount)
		{
			this.secondary.transform.position += Vector3.right * this.properties.CurrentState.twistyArms.twistyMoveSpeed * CupheadTime.Delta;
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.twistyArms.warningDuration);
		this.secondary.GetComponent<BoxCollider2D>().enabled = true;
		AudioManager.Play("robot_arms_extend_across");
		this.emitAudioFromObject.Add("robot_arms_extend_across");
		while (this.secondary.transform.position.x < -1248f + this.spriteBounds.x / 8f * 6f)
		{
			this.secondary.transform.position += Vector3.right * this.properties.CurrentState.twistyArms.twistyMoveSpeed * CupheadTime.Delta;
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.twistyArms.twistyArmsStayDuration);
		AudioManager.Play("robot_arms_extend_back");
		this.emitAudioFromObject.Add("robot_arms_extend_back");
		base.StartCoroutine(this.twistyArmsExit_cr(false));
		yield break;
	}

	// Token: 0x060029C3 RID: 10691 RVA: 0x0018548C File Offset: 0x0018388C
	private IEnumerator twistyArmsExit_cr(bool isPermaDeath = false)
	{
		float speedMultiplier = 1f;
		if (isPermaDeath)
		{
			speedMultiplier = 2f;
		}
		float nt = base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;
		this.secondaryAnimator.Play("Twisty_Arms_Exit", -1, nt);
		while (this.secondary.transform.position.x > -1248f)
		{
			this.secondary.transform.position += Vector3.left * this.properties.CurrentState.twistyArms.twistyMoveSpeed * speedMultiplier * CupheadTime.Delta;
			yield return null;
		}
		this.secondaryAnimator.Play("Twisty_Arms_Enter");
		this.twistyArmsPositionIndex++;
		if (this.twistyArmsPositionIndex >= this.properties.CurrentState.twistyArms.twistyPositionString.Split(new char[]
		{
			','
		}).Length)
		{
			this.twistyArmsPositionIndex = 0;
		}
		this.secondaryAttackDelay = this.properties.CurrentState.arms.attackDelayRange.RandomFloat();
		yield return null;
		this.secondary.GetComponent<BoxCollider2D>().enabled = false;
		this.armsActive = false;
		this.armsTypeIndex++;
		if (this.armsTypeIndex >= this.properties.CurrentState.arms.attackString.Split(new char[]
		{
			','
		}).Length)
		{
			this.armsTypeIndex = 0;
		}
		this.parent.animator.Play("Slow", 5, this.parent.animator.GetCurrentAnimatorStateInfo(5).normalizedTime % 1f);
		yield break;
	}

	// Token: 0x060029C4 RID: 10692 RVA: 0x001854B0 File Offset: 0x001838B0
	private IEnumerator magnetArmsIntro_cr()
	{
		this.secondaryAnimator.Play("Magnet_Arms", -1, 0f);
		yield return null;
		this.spriteBounds = this.secondary.GetComponent<SpriteRenderer>().bounds.size;
		this.secondary.transform.position = this.magnetStartRoot.transform.position;
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.magnetArms.magnetStartDelay);
		AudioManager.Play("robot_magnet_arms_start");
		while (AudioManager.CheckIfPlaying("robot_magnet_arms_start"))
		{
			yield return null;
		}
		AudioManager.PlayLoop("robot_magnet_arms_loop");
		float t = 0f;
		float time = 1.8f;
		float deltaRotation = 0f;
		while (t < time)
		{
			t += CupheadTime.Delta;
			deltaRotation = Mathf.Lerp(60f, 0f, t / time);
			this.secondary.transform.position = Vector2.Lerp(this.magnetStartRoot.transform.position, this.magnetEndRoot.transform.position, t / time);
			this.secondary.transform.SetEulerAngles(null, null, new float?(deltaRotation));
			yield return null;
		}
		time = 0f;
		while (time < this.properties.CurrentState.magnetArms.magnetStayDelay)
		{
			time += CupheadTime.Delta;
			foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
			{
				PlanePlayerController player = (PlanePlayerController)abstractPlayerController;
				if (!(player == null))
				{
					Vector2 playerForce = (PlayerManager.GetNext().center - this.secondary.transform.GetChild(1).transform.position).normalized * this.properties.CurrentState.magnetArms.magnetForce * 0.5f;
					this.force = new PlanePlayerMotor.Force(playerForce, true);
					player.motor.AddForce(this.force);
					yield return null;
					time += CupheadTime.Delta;
					if (player.motor != null)
					{
						player.motor.RemoveForce(this.force);
					}
				}
			}
			if (this.current == RobotLevelRobotBodyPart.state.none)
			{
				time = this.properties.CurrentState.magnetArms.magnetStayDelay;
			}
			yield return null;
		}
		AudioManager.Stop("robot_magnet_arms_loop");
		AudioManager.Play("robot_magnet_arms_end");
		base.StartCoroutine(this.magnetArmsExit_cr(false));
		yield break;
	}

	// Token: 0x060029C5 RID: 10693 RVA: 0x001854CC File Offset: 0x001838CC
	private IEnumerator magnetArmsExit_cr(bool isPermaDeath = false)
	{
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			PlanePlayerController planePlayerController = (PlanePlayerController)abstractPlayerController;
			if (!(planePlayerController == null))
			{
				planePlayerController.motor.RemoveForce(this.force);
			}
		}
		float delay = (1f - this.secondaryAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime % 1f) * this.secondaryAnimator.GetCurrentAnimatorStateInfo(1).length;
		this.secondaryAnimator.Play("Magnet_Arms", -1, 0f);
		yield return CupheadTime.WaitForSeconds(this, delay);
		float t = 0f;
		float time = 1.8f;
		float deltaRotation = 0f;
		Vector3 root = (!isPermaDeath) ? this.magnetEndRoot.transform.position : this.secondary.transform.position;
		while (t < time)
		{
			t += CupheadTime.Delta;
			deltaRotation = Mathf.Lerp(0f, 60f, t / time);
			this.secondary.transform.position = Vector2.Lerp(root, this.magnetStartRoot.transform.position, t / time);
			this.secondary.transform.SetEulerAngles(null, null, new float?(deltaRotation));
			yield return null;
		}
		this.secondaryAttackDelay = this.properties.CurrentState.arms.attackDelayRange.RandomFloat();
		yield return null;
		this.armsActive = false;
		this.armsTypeIndex++;
		if (this.armsTypeIndex >= this.properties.CurrentState.arms.attackString.Split(new char[]
		{
			','
		}).Length)
		{
			this.armsTypeIndex = 0;
		}
		this.parent.animator.Play("Slow", 5, this.parent.animator.GetCurrentAnimatorStateInfo(5).normalizedTime % 1f);
		yield break;
	}

	// Token: 0x060029C6 RID: 10694 RVA: 0x001854F0 File Offset: 0x001838F0
	private IEnumerator panicArmsLoop_cr()
	{
		for (;;)
		{
			float normalizedTime = this.parent.animator.GetCurrentAnimatorStateInfo(7).normalizedTime;
			normalizedTime %= 1f;
			int currentFrame = (int)(normalizedTime * 24f);
			if (this.panicArmsLoop)
			{
				this.frontArm.transform.position = this.panicArmsPath[currentFrame].position;
				int num = currentFrame + 10;
				if (num >= 24)
				{
					num -= 24;
				}
				this.backArm.transform.position = this.panicArmsPath[num].position;
			}
			currentFrame++;
			if (currentFrame >= 24)
			{
				currentFrame = 0;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060029C7 RID: 10695 RVA: 0x0018550B File Offset: 0x0018390B
	protected override void OnSecondaryDeath()
	{
		base.StartCoroutine(this.heart_cr());
		base.OnSecondaryDeath();
	}

	// Token: 0x060029C8 RID: 10696 RVA: 0x00185520 File Offset: 0x00183920
	private void HeartIntroSFX()
	{
		AudioManager.Play("robot_heart_spring_out");
		this.emitAudioFromObject.Add("robot_heart_spring_out");
	}

	// Token: 0x060029C9 RID: 10697 RVA: 0x0018553C File Offset: 0x0018393C
	private IEnumerator heart_cr()
	{
		base.GetComponent<SpriteRenderer>().enabled = true;
		float waitDuration = this.parent.animator.GetCurrentAnimatorStateInfo(2).length;
		yield return CupheadTime.WaitForSeconds(this, waitDuration);
		base.animator.SetTrigger("OnHeartActive");
		base.GetComponent<Collider2D>().enabled = true;
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", 1, true, true);
		yield break;
	}

	// Token: 0x060029CA RID: 10698 RVA: 0x00185558 File Offset: 0x00183958
	protected override void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.current == RobotLevelRobotBodyPart.state.primary)
		{
			base.OnDamageTaken(info);
			if (this.damageEffectRoutine != null)
			{
				base.StopCoroutine(this.damageEffectRoutine);
			}
			this.damageEffectRoutine = this.damageEffect_cr();
			base.StartCoroutine(this.damageEffectRoutine);
		}
		else
		{
			float num = this.currentHealth[1];
			this.currentHealth[1] -= info.damage;
			if (this.currentHealth[1] / (float)this.properties.CurrentState.heart.heartHP * 100f <= (float)this.properties.CurrentState.heart.heartDamageChangePercentage)
			{
				if (!this.playedCrackedSound)
				{
					AudioManager.Play("robot_heart_spring_cracked");
					this.emitAudioFromObject.Add("robot_heart_spring_cracked");
					this.playedCrackedSound = true;
				}
				base.animator.Play("Damaged Loop", 1, base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f);
			}
			if (this.currentHealth[1] <= 0f && !this.destroyed)
			{
				AudioManager.Stop("robot_magnet_arms_loop");
				AudioManager.Play("robot_heart_spring_destroyed");
				this.emitAudioFromObject.Add("robot_heart_spring_destroyed");
				this.properties.DealDamageToNextNamedState();
				this.destroyed = true;
			}
			if (num > 0f)
			{
				Level.Current.timeline.DealDamage(Mathf.Clamp(num - this.currentHealth[1], 0f, num));
			}
		}
	}

	// Token: 0x060029CB RID: 10699 RVA: 0x001856E4 File Offset: 0x00183AE4
	protected override void ExitCurrentAttacks()
	{
		this.StopAllCoroutines();
		bool isPermaDeath = false;
		if (Level.Current.mode == Level.Mode.Easy)
		{
			isPermaDeath = true;
			this.secondary.GetComponent<RobotLevelSecondaryArms>().BossAlive = false;
		}
		if (this.armsActive)
		{
			char c = this.properties.CurrentState.arms.attackString.Split(new char[]
			{
				','
			})[this.armsTypeIndex][0];
			if (c != 'T')
			{
				if (c == 'M')
				{
					base.StartCoroutine(this.magnetArmsExit_cr(isPermaDeath));
				}
			}
			else
			{
				base.StartCoroutine(this.twistyArmsExit_cr(isPermaDeath));
			}
		}
		base.ExitCurrentAttacks();
	}

	// Token: 0x060029CC RID: 10700 RVA: 0x0018579F File Offset: 0x00183B9F
	public void InitAnims()
	{
		base.animator.SetTrigger("OnRobotIntro");
	}

	// Token: 0x060029CD RID: 10701 RVA: 0x001857B4 File Offset: 0x00183BB4
	protected override void Die()
	{
		if (this.damageEffectRoutine != null)
		{
			base.StopCoroutine(this.damageEffectRoutine);
		}
		this.damageEffect.SetActive(false);
		foreach (SpriteRenderer spriteRenderer in base.transform.GetComponentsInChildren<SpriteRenderer>())
		{
			spriteRenderer.enabled = false;
		}
		base.Die();
	}

	// Token: 0x060029CE RID: 10702 RVA: 0x00185818 File Offset: 0x00183C18
	private IEnumerator damageEffect_cr()
	{
		for (int i = 0; i < 3; i++)
		{
			this.damageEffectRenderer.enabled = true;
			this.damageEffect.SetActive(true);
			yield return CupheadTime.WaitForSeconds(this, 0.041666668f);
			this.damageEffect.SetActive(false);
			yield return CupheadTime.WaitForSeconds(this, 0.041666668f);
		}
		yield break;
	}

	// Token: 0x040032A3 RID: 12963
	private const int PANIC_ARMS_ANIM_FRAME_COUNT = 24;

	// Token: 0x040032A4 RID: 12964
	private int frameCount;

	// Token: 0x040032A5 RID: 12965
	private bool playedCrackedSound;

	// Token: 0x040032A6 RID: 12966
	private bool armsActive;

	// Token: 0x040032A7 RID: 12967
	private bool panicArmsLoop;

	// Token: 0x040032A8 RID: 12968
	private int armsTypeIndex;

	// Token: 0x040032A9 RID: 12969
	private int twistyArmsPositionIndex;

	// Token: 0x040032AA RID: 12970
	private Vector3 spriteBounds;

	// Token: 0x040032AB RID: 12971
	private Animator secondaryAnimator;

	// Token: 0x040032AC RID: 12972
	private PlanePlayerMotor.Force force;

	// Token: 0x040032AD RID: 12973
	private bool destroyed;

	// Token: 0x040032AE RID: 12974
	[SerializeField]
	private SpriteRenderer torsoTop;

	// Token: 0x040032AF RID: 12975
	[SerializeField]
	private GameObject[] damagedPortholes;

	// Token: 0x040032B0 RID: 12976
	[SerializeField]
	private GameObject frontArm;

	// Token: 0x040032B1 RID: 12977
	[SerializeField]
	private GameObject backArm;

	// Token: 0x040032B2 RID: 12978
	[SerializeField]
	private Transform portholeRoot;

	// Token: 0x040032B3 RID: 12979
	[SerializeField]
	private Transform portholeOffsetRoot;

	// Token: 0x040032B4 RID: 12980
	[SerializeField]
	private Transform[] panicArmsPath;

	// Token: 0x040032B5 RID: 12981
	[SerializeField]
	private Transform magnetStartRoot;

	// Token: 0x040032B6 RID: 12982
	[SerializeField]
	private Transform magnetEndRoot;

	// Token: 0x040032B7 RID: 12983
	[SerializeField]
	private GameObject damageEffect;

	// Token: 0x040032B8 RID: 12984
	private IEnumerator damageEffectRoutine;

	// Token: 0x040032B9 RID: 12985
	private SpriteRenderer damageEffectRenderer;

	// Token: 0x02000777 RID: 1911
	public enum AnimationLayers
	{
		// Token: 0x040032BB RID: 12987
		Main,
		// Token: 0x040032BC RID: 12988
		Pilot,
		// Token: 0x040032BD RID: 12989
		Head,
		// Token: 0x040032BE RID: 12990
		Hose,
		// Token: 0x040032BF RID: 12991
		Waist,
		// Token: 0x040032C0 RID: 12992
		BackArm,
		// Token: 0x040032C1 RID: 12993
		FrontArm,
		// Token: 0x040032C2 RID: 12994
		Torso
	}
}
