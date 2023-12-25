using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000653 RID: 1619
public class FlyingCowboyLevelMeat : LevelProperties.FlyingCowboy.Entity
{
	// Token: 0x060021A6 RID: 8614 RVA: 0x001386EC File Offset: 0x00136AEC
	private void Start()
	{
		Level.Current.OnBossDeathExplosionsEvent += this.onBossDeathExplosionsEventHandler;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.nextBulletSpawnPointA.position = this.sausageHolderA.position;
		this.nextBulletSpawnPointB.position = this.sausageHolderB.position;
	}

	// Token: 0x060021A7 RID: 8615 RVA: 0x00138769 File Offset: 0x00136B69
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060021A8 RID: 8616 RVA: 0x00138788 File Offset: 0x00136B88
	public override void LevelInit(LevelProperties.FlyingCowboy properties)
	{
		base.LevelInit(properties);
		this.runningSpitBulletParryPattern = new PatternString(properties.CurrentState.sausageRun.bulletParry, true);
		this.sausageTimeToMoveString = new PatternString(properties.CurrentState.sausageRun.timeTillSwitch, true, true);
		this.spitBulletParryString = new PatternString(properties.CurrentState.can.bulletParryString, true);
	}

	// Token: 0x060021A9 RID: 8617 RVA: 0x001387F1 File Offset: 0x00136BF1
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060021AA RID: 8618 RVA: 0x0013880C File Offset: 0x00136C0C
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.meatPhase == FlyingCowboyLevelMeat.MeatPhase.Can)
		{
			AudioManager.Play("sfx_dlc_cowgirl_p3_can_damage_metalimpact");
		}
		base.properties.DealDamage(info.damage);
		if (!this.isDead && base.properties.CurrentHealth <= 0f)
		{
			this.die();
		}
	}

	// Token: 0x060021AB RID: 8619 RVA: 0x00138865 File Offset: 0x00136C65
	public void SelectPhase(FlyingCowboyLevelMeat.MeatPhase meatPhase)
	{
		base.gameObject.SetActive(true);
		this.meatPhase = meatPhase;
		if (meatPhase != FlyingCowboyLevelMeat.MeatPhase.Can)
		{
			if (meatPhase == FlyingCowboyLevelMeat.MeatPhase.Sausage)
			{
				this.Sausage();
			}
		}
		else
		{
			this.Can();
		}
	}

	// Token: 0x060021AC RID: 8620 RVA: 0x001388A4 File Offset: 0x00136CA4
	public void Sausage()
	{
		Vector3 position = this.sausageSpawnPosition.position;
		position.y = 42f;
		base.transform.position = position;
		base.StartCoroutine(this.sausage_intro_cr());
	}

	// Token: 0x060021AD RID: 8621 RVA: 0x001388E4 File Offset: 0x00136CE4
	private IEnumerator sausage_intro_cr()
	{
		LevelProperties.FlyingCowboy.SausageRun p = base.properties.CurrentState.sausageRun;
		yield return CupheadTime.WaitForSeconds(this, p.mirrorTime);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Sg_Mirror_Cont", false, true);
		base.StartCoroutine(this.beans_cr());
		if (p.shootBullets)
		{
			base.StartCoroutine(this.sausageTurret_cr());
		}
		base.StartCoroutine(this.sausageSwitchHeight_cr());
		yield break;
	}

	// Token: 0x060021AE RID: 8622 RVA: 0x001388FF File Offset: 0x00136CFF
	private void animationEvent_RepositionSausage()
	{
		base.StartCoroutine(this.repositionSausage_cr());
	}

	// Token: 0x060021AF RID: 8623 RVA: 0x00138910 File Offset: 0x00136D10
	private IEnumerator repositionSausage_cr()
	{
		AudioManager.PlayLoop("sfx_dlc_cowgirl_p3_sausage_footstep_loop");
		float startX = base.transform.position.x;
		float elapsedTime = 0f;
		while (this.meatPhase == FlyingCowboyLevelMeat.MeatPhase.Sausage && elapsedTime < 4f)
		{
			yield return null;
			elapsedTime += CupheadTime.Delta;
			Vector3 position = base.transform.position;
			position.x = Mathf.Lerp(startX, 340f, elapsedTime / 4f);
			base.transform.position = position;
		}
		base.StartCoroutine(this.wobble_cr(base.transform, this.sausageWobbleRadius, this.sausageWobbleDuration, base.transform.position, FlyingCowboyLevelMeat.MeatPhase.Sausage, false, false));
		yield break;
	}

	// Token: 0x060021B0 RID: 8624 RVA: 0x0013892C File Offset: 0x00136D2C
	private IEnumerator sausageSwitchHeight_cr()
	{
		for (;;)
		{
			float time = this.sausageTimeToMoveString.PopFloat();
			yield return CupheadTime.WaitForSeconds(this, time);
			bool newFlyingStatus = !this.isFlying;
			base.animator.SetBool("IsFlying", newFlyingStatus);
			if (newFlyingStatus)
			{
				AudioManager.Stop("sfx_dlc_cowgirl_p3_sausage_footstep_loop");
			}
			string transitionAnimation = (!newFlyingStatus) ? "Sg_Fly_To_Run" : "Sg_Run_To_Fly";
			yield return base.animator.WaitForAnimationToEnd(this, transitionAnimation, false, true);
			if (!newFlyingStatus)
			{
				AudioManager.PlayLoop("sfx_dlc_cowgirl_p3_sausage_footstep_loop");
			}
			this.isFlying = newFlyingStatus;
		}
		yield break;
	}

	// Token: 0x060021B1 RID: 8625 RVA: 0x00138948 File Offset: 0x00136D48
	private IEnumerator beans_cr()
	{
		float startBeansHealthPercentage = base.properties.CurrentState.healthTrigger;
		float endBeansPercentage = base.properties.GetNextStateHealthTrigger();
		float startBeansHealth = startBeansHealthPercentage * base.properties.TotalHealth;
		float endBeansHealth = endBeansPercentage * base.properties.TotalHealth;
		float seventyFivePercentof = startBeansHealth + (endBeansHealth - startBeansHealth) * 0.75f;
		LevelProperties.FlyingCowboy.SausageRun p = base.properties.CurrentState.sausageRun;
		PatternString groupDelayPattern = new PatternString(p.groupBeansDelayString, true, true);
		PatternString positionPattern = new PatternString(p.beansPositionString, true, false);
		PatternString extendTimerPattern = new PatternString(p.beansExtendTimer, true);
		float positionX = 690f;
		while (this.meatPhase == FlyingCowboyLevelMeat.MeatPhase.Sausage)
		{
			string[] positionValues = positionPattern.GetString().Split(new char[]
			{
				':'
			});
			positionPattern.IncrementString();
			float positionY;
			Parser.FloatTryParse(positionValues[0], out positionY);
			bool pointingUp = positionValues[1] == "U";
			float currentPercentage = (base.properties.CurrentHealth - startBeansHealth) / (seventyFivePercentof - startBeansHealth);
			float speed = Mathf.Lerp(p.beansSpeed.min, p.beansSpeed.max, currentPercentage);
			FlyingCowboyLevelBeans beans = this.beansPrefab.Spawn<FlyingCowboyLevelBeans>();
			beans.Init(new Vector3(positionX, positionY), pointingUp, speed, extendTimerPattern.PopFloat());
			if (positionPattern.GetSubStringIndex() != 0)
			{
				float spawnDelay = Mathf.Lerp(p.beansSpawnDelay.max, p.beansSpawnDelay.min, currentPercentage);
				yield return CupheadTime.WaitForSeconds(this, spawnDelay);
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, groupDelayPattern.PopFloat());
			}
		}
		yield break;
	}

	// Token: 0x060021B2 RID: 8626 RVA: 0x00138964 File Offset: 0x00136D64
	private IEnumerator sausageTurret_cr()
	{
		LevelProperties.FlyingCowboy.SausageRun p = base.properties.CurrentState.sausageRun;
		AbstractPlayerController player = PlayerManager.GetNext();
		while (this.meatPhase == FlyingCowboyLevelMeat.MeatPhase.Sausage)
		{
			base.animator.SetTrigger("OnShoot");
			this.waitingToShoot = true;
			while (this.waitingToShoot)
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, p.bulletDelay);
		}
		yield break;
	}

	// Token: 0x060021B3 RID: 8627 RVA: 0x00138980 File Offset: 0x00136D80
	private void aniEvent_ShootTurret()
	{
		this.player = PlayerManager.GetNext();
		LevelProperties.FlyingCowboy.SausageRun sausageRun = base.properties.CurrentState.sausageRun;
		Vector3 vector = (!this.isFlying) ? this.runBottomSpitBulletSpawn.position : this.runTopSpitBulletSpawn.position;
		Vector3 position = (!this.isFlying) ? this.runBottomSpitBulletEffectSpawn.position : this.runTopSpitBulletEffectSpawn.position;
		this.player = PlayerManager.GetNext();
		Vector3 vector2 = this.player.transform.position - vector;
		float num;
		for (num = MathUtils.DirectionToAngle(vector2); num < 0f; num += 360f)
		{
		}
		float min;
		float max;
		bool clockwise;
		if (this.isFlying)
		{
			min = 180f - sausageRun.bulletTopMaxUpAngle;
			max = 180f + sausageRun.bulletTopMaxDownAngle;
			clockwise = sausageRun.bulletTopRotateClockwise;
		}
		else
		{
			min = 180f - sausageRun.bulletBottomMaxUpAngle;
			max = 180f + sausageRun.bulletBottomMaxDownAngle;
			clockwise = sausageRun.bulletBottomRotateClockwise;
		}
		num = Mathf.Clamp(num, min, max);
		vector2 = MathUtilities.AngleToDirection(num);
		this.sausageRunSpitBullet.Create(vector, sausageRun.bulletSpeed, sausageRun.bulletRotationSpeed, sausageRun.bulletRotationRadius, vector2, clockwise, this.runningSpitBulletParryPattern.PopLetter() == 'P');
		Effect effect = this.sausageRunSpitBulletEffect.Create(position);
		if (!this.isFlying)
		{
			effect.transform.rotation = Quaternion.Euler(0f, 0f, -30f);
		}
		this.waitingToShoot = false;
	}

	// Token: 0x060021B4 RID: 8628 RVA: 0x00138B29 File Offset: 0x00136F29
	public void Can()
	{
		base.StartCoroutine(this.toCan_cr());
	}

	// Token: 0x060021B5 RID: 8629 RVA: 0x00138B38 File Offset: 0x00136F38
	private IEnumerator toCan_cr()
	{
		AudioManager.Stop("sfx_dlc_cowgirl_p3_sausage_footstep_loop");
		base.animator.SetBool("ToCan", true);
		yield return base.animator.WaitForNormalizedTime(this, 1f, "SausageToCanEnd", 0, true, false, true);
		base.animator.Play("CanIntro", 0);
		base.animator.Update(0f);
		base.StartCoroutine(this.repositionCan_cr());
		LevelProperties.FlyingCowboy.Can p = base.properties.CurrentState.can;
		base.StartCoroutine(this.wobble_cr(this.canTransform, new Vector2(p.wobbleRadiusX, p.wobbleRadiusY), new Vector2(p.wobbleDurationX, p.wobbleDurationY), this.canTransform.localPosition, FlyingCowboyLevelMeat.MeatPhase.Can, true, true));
		yield break;
	}

	// Token: 0x060021B6 RID: 8630 RVA: 0x00138B54 File Offset: 0x00136F54
	private IEnumerator repositionCan_cr()
	{
		Vector3 startPosition = base.transform.position;
		Vector3 targetPosition = new Vector3(340f, 61f, startPosition.z);
		float elapsedTime = 0f;
		while (elapsedTime < 3f)
		{
			yield return null;
			elapsedTime += CupheadTime.Delta;
			base.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / 3f);
		}
		yield break;
	}

	// Token: 0x060021B7 RID: 8631 RVA: 0x00138B70 File Offset: 0x00136F70
	private void animationEvent_StartSausageLinks()
	{
		AudioManager.PlayLoop("sfx_dlc_cowgirl_p3_sausagemeattin_loop");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_sausagemeattin_loop");
		this.sausageTransforms.SetActive(true);
		base.StartCoroutine(this.sausageTrain_cr(true));
		base.StartCoroutine(this.sausageRotation_cr(this.sausageHolderA, 0));
		base.StartCoroutine(this.sausageTrain_cr(false));
		base.StartCoroutine(this.sausageRotation_cr(this.sausageHolderB, 1));
		if (base.properties.CurrentState.can.shootBullets)
		{
			base.StartCoroutine(this.shootCanBullets_cr());
		}
		base.StartCoroutine(this.beanCanTriggerZone_cr());
	}

	// Token: 0x060021B8 RID: 8632 RVA: 0x00138C1B File Offset: 0x0013701B
	private void animationEvent_TriggerCanBullets()
	{
		this.canBulletsTriggered = true;
	}

	// Token: 0x060021B9 RID: 8633 RVA: 0x00138C24 File Offset: 0x00137024
	private IEnumerator shootCanBullets_cr()
	{
		LevelProperties.FlyingCowboy.Can p = base.properties.CurrentState.can;
		int variant = 0;
		int fxVariant = UnityEngine.Random.Range(0, 3);
		PatternString bulletCountPattern = new PatternString(p.bulletCount, true, true);
		this.canBulletsTriggered = false;
		while (this.meatPhase == FlyingCowboyLevelMeat.MeatPhase.Can)
		{
			yield return CupheadTime.WaitForSeconds(this, p.shotDelay);
			base.animator.SetTrigger("OnShoot");
			while (!this.canBulletsTriggered)
			{
				yield return null;
			}
			this.canBulletsTriggered = false;
			Effect muzzleFX = this.canBulletMuzzleFX.Create(this.bulletRoot.position);
			muzzleFX.animator.SetInteger("Effect", fxVariant);
			fxVariant = MathUtilities.NextIndex(fxVariant, 3);
			this.SFX_CanSpitBurningFire();
			while (!this.canBulletsTriggered)
			{
				yield return null;
			}
			this.canBulletsTriggered = false;
			int count = bulletCountPattern.PopInt();
			float startAngle = -p.bulletSpreadAngle * 0.5f;
			float angleIncrement = p.bulletSpreadAngle / (float)(count - 1);
			for (int i = 0; i < count; i++)
			{
				float num = startAngle + angleIncrement * (float)i;
				float rotation = 180f - num;
				BasicProjectile basicProjectile = this.canBullet.Create(this.bulletRoot.position, rotation, p.bulletSpeed);
				bool flag = this.spitBulletParryString.PopLetter() == 'P';
				basicProjectile.SetParryable(flag);
				basicProjectile.animator.SetInteger("Variant", variant);
				basicProjectile.animator.Update(0f);
				basicProjectile.animator.Play(0, 0, UnityEngine.Random.Range(0f, 1f));
				basicProjectile.GetComponent<SpriteRenderer>().sortingOrder = i;
				basicProjectile.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(-num));
				if (!flag)
				{
					variant = ((variant != 0) ? 0 : 1);
				}
			}
		}
		this.sausageTransforms.SetActive(false);
		yield break;
	}

	// Token: 0x060021BA RID: 8634 RVA: 0x00138C40 File Offset: 0x00137040
	private IEnumerator sausageRotation_cr(Transform sausageHolder, int index)
	{
		LevelProperties.FlyingCowboy.Can p = base.properties.CurrentState.can;
		Transform holder = (index != 0) ? this.sausageHolderB : this.sausageHolderA;
		float topAngle = -p.maxSausageAngle;
		float bottomAngle = p.maxSausageAngle;
		bool goingUp = index == 0;
		float startAngle = (!goingUp) ? bottomAngle : 0f;
		float endAngle = (!goingUp) ? 0f : topAngle;
		int sortingOffset = index * 100;
		float elapsedTime = 0f;
		WaitForFixedUpdate wait = new WaitForFixedUpdate();
		while (this.meatPhase == FlyingCowboyLevelMeat.MeatPhase.Can)
		{
			while (elapsedTime < 2f)
			{
				if (this.meatPhase != FlyingCowboyLevelMeat.MeatPhase.Can)
				{
					break;
				}
				float t = (!goingUp) ? (1f - elapsedTime / 2f) : (elapsedTime / 2f);
				float angle = Mathf.Lerp(startAngle, endAngle, t);
				sausageHolder.transform.SetEulerAngles(null, null, new float?(angle));
				int sortingOrder;
				if (angle >= 15f)
				{
					sortingOrder = FlyingCowboyLevelMeat.LowSausageLinkSortingOrder + sortingOffset;
				}
				else if (angle <= -15f)
				{
					sortingOrder = FlyingCowboyLevelMeat.HighSausageLinkSortingOrder + sortingOffset;
				}
				else
				{
					sortingOrder = FlyingCowboyLevelMeat.MidSausageLinkSortingOrder + sortingOffset;
				}
				int childCount = holder.childCount;
				for (int i = 0; i < childCount; i++)
				{
					Transform child = holder.GetChild(i);
					SpriteRenderer component = child.GetComponent<SpriteRenderer>();
					if (component != null)
					{
						component.sortingOrder = sortingOrder + childCount - i;
					}
				}
				if (index == 0)
				{
					this.currentSausageLinkSortingOrderA = sortingOrder;
				}
				else if (index == 1)
				{
					this.currentSausageLinkSortingOrderB = sortingOrder;
				}
				elapsedTime += CupheadTime.FixedDelta;
				yield return wait;
			}
			if ((goingUp && startAngle == 0f) || (!goingUp && endAngle == 0f))
			{
				goingUp = !goingUp;
			}
			else if (!goingUp && startAngle == 0f)
			{
				startAngle = bottomAngle;
				endAngle = 0f;
			}
			else if (goingUp && startAngle == bottomAngle)
			{
				startAngle = 0f;
				endAngle = topAngle;
			}
			elapsedTime = 0f;
		}
		yield break;
	}

	// Token: 0x060021BB RID: 8635 RVA: 0x00138C6C File Offset: 0x0013706C
	private IEnumerator sausageTrain_cr(bool isTypeA)
	{
		LevelProperties.FlyingCowboy.Can p = base.properties.CurrentState.can;
		Transform sausageHolder = (!isTypeA) ? this.sausageHolderB : this.sausageHolderA;
		Transform nextSpawn = (!isTypeA) ? this.nextBulletSpawnPointB : this.nextBulletSpawnPointA;
		string[] sausageMainString = (!isTypeA) ? p.sausageStringB : p.sausageStringA;
		PatternString sausageAmountPattern = new PatternString(sausageMainString, true, true);
		PatternString gapPattern = new PatternString((!isTypeA) ? p.gapDistB : p.gapDistA, true, true);
		int sausageCounter = 0;
		int sausageMax = sausageAmountPattern.PopInt();
		FlyingCowboyLevelMeat.SausageType previousSausageType = FlyingCowboyLevelMeat.SausageType.H1;
		FlyingCowboyLevelSausageLink previousSausage = null;
		AudioManager.PlayLoop("sfx_dlc_cowgirl_p3_sausagemeattin_loop");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_sausagemeattin_loop");
		while (this.meatPhase == FlyingCowboyLevelMeat.MeatPhase.Can)
		{
			if (sausageCounter < sausageMax)
			{
				bool flag = false;
				FlyingCowboyLevelMeat.SausageType sausageType;
				if (previousSausageType == FlyingCowboyLevelMeat.SausageType.U1 || previousSausageType == FlyingCowboyLevelMeat.SausageType.U2 || previousSausageType == FlyingCowboyLevelMeat.SausageType.U3)
				{
					sausageType = FlyingCowboyLevelMeat.SausageTypeDown.RandomChoice<FlyingCowboyLevelMeat.SausageType>();
					flag = true;
				}
				else if (sausageMax - sausageCounter < 2)
				{
					sausageType = FlyingCowboyLevelMeat.SausageTypeEnd.RandomChoice<FlyingCowboyLevelMeat.SausageType>();
				}
				else
				{
					for (sausageType = previousSausageType; sausageType == previousSausageType; sausageType = FlyingCowboyLevelMeat.SausageTypeAny.RandomChoice<FlyingCowboyLevelMeat.SausageType>())
					{
					}
				}
				previousSausageType = sausageType;
				sausageCounter++;
				FlyingCowboyLevelSausageLink flyingCowboyLevelSausageLink = this.sausage.Create(nextSpawn.position, sausageHolder.transform.eulerAngles.z, -p.sausageTrainSpeed) as FlyingCowboyLevelSausageLink;
				flyingCowboyLevelSausageLink.transform.parent = sausageHolder;
				flyingCowboyLevelSausageLink.Initialize(sausageType, this.sausageLinkSqueezePoint, (!flag) ? null : previousSausage);
				if (flag)
				{
					flyingCowboyLevelSausageLink.animator.Play("SqueezeLoopDown");
				}
				previousSausage = flyingCowboyLevelSausageLink;
				nextSpawn.parent = flyingCowboyLevelSausageLink.transform;
				nextSpawn.localPosition = new Vector3(FlyingCowboyLevelMeat.SausageLinkWidth, 0f);
				SpriteRenderer component = flyingCowboyLevelSausageLink.GetComponent<SpriteRenderer>();
				component.sortingOrder = ((!isTypeA) ? this.currentSausageLinkSortingOrderB : this.currentSausageLinkSortingOrderA);
			}
			else
			{
				int num = gapPattern.PopInt() - 1;
				float x = FlyingCowboyLevelMeat.SausageGapWidths[num];
				nextSpawn.localPosition = new Vector3(x, 0f);
				BasicProjectile basicProjectile = this.sausageString.Create(nextSpawn.position, sausageHolder.transform.eulerAngles.z, -p.sausageTrainSpeed);
				basicProjectile.animator.Play(FlyingCowboyLevelMeat.SausageGapAnimationNames[num]);
				SpriteRenderer component2 = basicProjectile.GetComponent<SpriteRenderer>();
				component2.sortingOrder = ((!isTypeA) ? this.currentSausageLinkSortingOrderB : this.currentSausageLinkSortingOrderA);
				basicProjectile.transform.parent = sausageHolder;
				nextSpawn.parent = basicProjectile.transform;
				nextSpawn.localPosition = new Vector3(FlyingCowboyLevelMeat.SausageLinkWidth, 0f);
				sausageCounter = 0;
				sausageMax = sausageAmountPattern.PopInt();
			}
			while (nextSpawn.position.x > sausageHolder.position.x + 175f)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x060021BC RID: 8636 RVA: 0x00138C90 File Offset: 0x00137090
	private IEnumerator beanCanTriggerZone_cr()
	{
		LevelProperties.FlyingCowboy.Can p = base.properties.CurrentState.can;
		PatternString extendTimerPattern = new PatternString(p.beanCanExtendTimer, true);
		PatternString topSpawnPattern = new PatternString(p.beanCanPostionUpper, true, true);
		PatternString bottomSpawnPattern = new PatternString(p.beanCanPositionLower, true, true);
		float[] timers = new float[this.beanCanTriggerZones.Length];
		for (;;)
		{
			yield return null;
			for (int i = 0; i < this.beanCanTriggerZones.Length; i++)
			{
				bool flag = false;
				Vector3 vector = Vector3.zero;
				foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
				{
					TriggerZone triggerZone = this.beanCanTriggerZones[i];
					if (abstractPlayerController != null && triggerZone.Contains(abstractPlayerController.center))
					{
						timers[i] += CupheadTime.Delta;
						vector = abstractPlayerController.center;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					timers[i] = 0f;
				}
				else if (timers[i] > p.beanCanTriggerTime)
				{
					timers[i] -= p.beanCanTriggerTime;
					bool flag2 = this.beanCanTriggerZones[i].transform.position.y > 0f;
					string[] array = ((!flag2) ? bottomSpawnPattern.PopString() : topSpawnPattern.PopString()).Split(new char[]
					{
						':'
					});
					float y;
					Parser.FloatTryParse(array[0], out y);
					bool pointingUp = array[1] == "U";
					FlyingCowboyLevelBeans flyingCowboyLevelBeans = this.beansPrefab.Spawn<FlyingCowboyLevelBeans>();
					flyingCowboyLevelBeans.Init(new Vector3(690f, y), pointingUp, p.beanCanSpeed, extendTimerPattern.PopFloat());
				}
			}
		}
		yield break;
	}

	// Token: 0x060021BD RID: 8637 RVA: 0x00138CAC File Offset: 0x001370AC
	private void die()
	{
		this.isDead = true;
		this.StopAllCoroutines();
		if (Level.Current.mode == Level.Mode.Easy)
		{
			base.animator.Play("DeathEasy");
		}
		else
		{
			base.animator.Play("Death");
			base.StartCoroutine(this.spawnFloatingSausages_cr());
			AudioManager.Stop("sfx_dlc_cowgirl_p3_sausagemeattin_loop");
		}
		for (int i = 0; i < 2; i++)
		{
			Transform transform = (i != 0) ? this.sausageHolderB : this.sausageHolderA;
			int childCount = transform.childCount;
			for (int j = 0; j < childCount; j++)
			{
				Transform child = transform.GetChild(j);
				if (child.position.x > this.sausageLinkSqueezePoint.position.x)
				{
					UnityEngine.Object.Destroy(child.gameObject);
				}
			}
		}
	}

	// Token: 0x060021BE RID: 8638 RVA: 0x00138D94 File Offset: 0x00137194
	private IEnumerator spawnFloatingSausages_cr()
	{
		float delay = 1f;
		string[] animations = new string[]
		{
			"A",
			"B",
			"C"
		};
		float[] spawnFactors = new float[]
		{
			0.2f,
			0.6f,
			0f,
			0.8f,
			0.4f,
			1f
		};
		int spawnFactorIndex = UnityEngine.Random.Range(0, spawnFactors.Length);
		int animationIndex = UnityEngine.Random.Range(0, animations.Length);
		for (;;)
		{
			float factor = spawnFactors[spawnFactorIndex];
			Vector3 position = Vector3.Lerp(this.floatingSausageSpawnPointLeft.position, this.floatingSausageSpawnPointRight.position, factor);
			FlyingCowboyFloatingSausages s = this.floatingSausage.Create(position) as FlyingCowboyFloatingSausages;
			s.SetAnimation(animations[animationIndex]);
			spawnFactorIndex = MathUtilities.NextIndex(spawnFactorIndex, spawnFactors.Length);
			animationIndex = MathUtilities.NextIndex(animationIndex, animations.Length);
			yield return CupheadTime.WaitForSeconds(this, delay);
		}
		yield break;
	}

	// Token: 0x060021BF RID: 8639 RVA: 0x00138DB0 File Offset: 0x001371B0
	private void onBossDeathExplosionsEventHandler()
	{
		Level.Current.OnBossDeathExplosionsEvent -= this.onBossDeathExplosionsEventHandler;
		string[] list = new string[]
		{
			"A",
			"B",
			"C",
			"H"
		};
		string[] list2 = new string[]
		{
			"D",
			"E",
			"F",
			"G",
			"I"
		};
		string[] array = new string[]
		{
			"E",
			"F",
			"I"
		};
		for (int i = 0; i < 2; i++)
		{
			Transform transform = (i != 0) ? this.sausageHolderB : this.sausageHolderA;
			int childCount = transform.childCount;
			for (int j = 0; j < childCount; j++)
			{
				Transform child = transform.GetChild(j);
				if (child.name.Contains("String"))
				{
					Effect effect = this.sausageStringDeathEffect.Create(child.GetComponent<SpriteRenderer>().bounds.center);
					effect.transform.rotation = child.rotation;
					Animator animator = effect.animator;
					AnimatorStateInfo currentAnimatorStateInfo = child.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
					if (currentAnimatorStateInfo.IsName("String1"))
					{
						animator.Play(list.RandomChoice<string>());
					}
					else if (currentAnimatorStateInfo.IsName("String2"))
					{
						animator.Play(list2.RandomChoice<string>());
					}
					else
					{
						animator.Play(list2.RandomChoice<string>());
					}
				}
				else
				{
					SpriteRenderer component = child.GetComponent<SpriteRenderer>();
					this.sausageDeathEffect.Create(component.bounds.center);
				}
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}
	}

	// Token: 0x060021C0 RID: 8640 RVA: 0x00138F8A File Offset: 0x0013738A
	private void AnimationEvent_SFX_VocalSausageScreaming()
	{
		AudioManager.Play("sfx_dlc_cowgirl_vocal_p3sausagescreaming");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_vocal_p3sausagescreaming");
	}

	// Token: 0x060021C1 RID: 8641 RVA: 0x00138FA6 File Offset: 0x001373A6
	private void AnimationEvent_SFX_CanSlam()
	{
		AudioManager.Play("sfx_DLC_Cowgirl_P3_CanSlam_Transition");
		this.emitAudioFromObject.Add("sfx_DLC_Cowgirl_P3_CanSlam_Transition");
	}

	// Token: 0x060021C2 RID: 8642 RVA: 0x00138FC2 File Offset: 0x001373C2
	private void AnimationEvent_SFX_CanHoleBurst()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p3_can_holeburst_pop");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_can_holeburst_pop");
	}

	// Token: 0x060021C3 RID: 8643 RVA: 0x00138FDE File Offset: 0x001373DE
	private void SFX_CanSpitBurningFire()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p3_canspitburningfire");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_canspitburningfire");
	}

	// Token: 0x060021C4 RID: 8644 RVA: 0x00138FFA File Offset: 0x001373FA
	private void SFX_CanSpit()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p3_can_spit");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_can_spit");
	}

	// Token: 0x060021C5 RID: 8645 RVA: 0x00139016 File Offset: 0x00137416
	private void AnimationEvent_SFX_SausageBullRoar()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p3_sausagebullroar");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_sausagebullroar");
	}

	// Token: 0x060021C6 RID: 8646 RVA: 0x00139032 File Offset: 0x00137432
	private void AnimationEvent_SFX_SausageBullSpit()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p3_sausagebullspit");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_sausagebullspit");
	}

	// Token: 0x060021C7 RID: 8647 RVA: 0x0013904E File Offset: 0x0013744E
	private void AnimationEvent_SFX_SausageBullWingUp()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p3_sausagebullwingup");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_sausagebullwingup");
	}

	// Token: 0x060021C8 RID: 8648 RVA: 0x0013906A File Offset: 0x0013746A
	private void AnimationEvent_SFX_SausageBullWingDown()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p3_sausagebullwingdown");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_sausagebullwingdown");
	}

	// Token: 0x060021C9 RID: 8649 RVA: 0x00139086 File Offset: 0x00137486
	private void AnimationEvent_SFX_SausageBullRunToFly()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p3_sausagebull_runtofly");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_sausagebull_runtofly");
	}

	// Token: 0x060021CA RID: 8650 RVA: 0x001390A2 File Offset: 0x001374A2
	private void AnimationEvent_SFX_SausageBullPositionTransfer()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p3_sausage_position_transfer");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p3_sausage_position_transfer");
	}

	// Token: 0x060021CB RID: 8651 RVA: 0x001390C0 File Offset: 0x001374C0
	private IEnumerator wobble_cr(Transform transform, Vector2 wobbleRadius, Vector2 wobbleDuration, Vector3 initialPosition, FlyingCowboyLevelMeat.MeatPhase phase, bool useLocal, bool easeWobble)
	{
		float elapsedEaseTime = 0f;
		Vector3 shadowInitialPosition = this.shadowTransform.position;
		Vector2 wobbleTimeElapsed = wobbleDuration * 0.5f;
		while (this.meatPhase == phase)
		{
			if (easeWobble && elapsedEaseTime < 2f)
			{
				elapsedEaseTime += CupheadTime.Delta;
				float easeFactor = Mathf.Lerp(0f, 1f, elapsedEaseTime / 2f);
			}
			wobbleTimeElapsed.x += CupheadTime.Delta;
			wobbleTimeElapsed.y += CupheadTime.Delta;
			if (wobbleTimeElapsed.x >= 2f * wobbleDuration.x)
			{
				wobbleTimeElapsed.x -= 2f * wobbleDuration.x;
			}
			float tx;
			if (wobbleTimeElapsed.x > wobbleDuration.x)
			{
				tx = 1f - (wobbleTimeElapsed.x - wobbleDuration.x) / wobbleDuration.x;
			}
			else
			{
				tx = wobbleTimeElapsed.x / wobbleDuration.x;
			}
			if (wobbleTimeElapsed.y >= 2f * wobbleDuration.y)
			{
				wobbleTimeElapsed.y -= 2f * wobbleDuration.y;
			}
			float ty;
			if (wobbleTimeElapsed.y > wobbleDuration.y)
			{
				ty = 1f - (wobbleTimeElapsed.y - wobbleDuration.y) / wobbleDuration.y;
			}
			else
			{
				ty = wobbleTimeElapsed.y / wobbleDuration.y;
			}
			Vector3 positionChange = new Vector3(EaseUtils.EaseInOutSine(wobbleRadius.x, -wobbleRadius.x, tx), EaseUtils.EaseInOutSine(wobbleRadius.y, -wobbleRadius.y, ty));
			if (useLocal)
			{
				transform.localPosition = initialPosition + positionChange;
			}
			else
			{
				transform.position = initialPosition + positionChange;
			}
			if (this.meatPhase == FlyingCowboyLevelMeat.MeatPhase.Can && !Mathf.Approximately(wobbleRadius.y, 0f))
			{
				Vector3 b = positionChange;
				b.y *= 0.2f;
				Vector3 position = shadowInitialPosition + b;
				float num = 0f;
				if (position.y >= -220f)
				{
					num = MathUtilities.LerpMapping(position.y, -220f, -150f, 0f, 0.65000004f, true);
					position.y = -220f;
				}
				this.shadowTransform.position = position;
				float num2 = 0.1f * (positionChange.y / wobbleRadius.y);
				float value = 0.8f - num2 - num;
				this.shadowTransform.SetScale(new float?(value), new float?(value), null);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002A3E RID: 10814
	private static readonly float SausageLinkWidth = 120f;

	// Token: 0x04002A3F RID: 10815
	private static readonly FlyingCowboyLevelMeat.SausageType[] SausageTypeAny = new FlyingCowboyLevelMeat.SausageType[]
	{
		FlyingCowboyLevelMeat.SausageType.H1,
		FlyingCowboyLevelMeat.SausageType.H2,
		FlyingCowboyLevelMeat.SausageType.H3,
		FlyingCowboyLevelMeat.SausageType.H4,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.U1,
		FlyingCowboyLevelMeat.SausageType.U2,
		FlyingCowboyLevelMeat.SausageType.U3
	};

	// Token: 0x04002A40 RID: 10816
	private static readonly FlyingCowboyLevelMeat.SausageType[] SausageTypeEnd = new FlyingCowboyLevelMeat.SausageType[]
	{
		FlyingCowboyLevelMeat.SausageType.H1,
		FlyingCowboyLevelMeat.SausageType.H2,
		FlyingCowboyLevelMeat.SausageType.H3,
		FlyingCowboyLevelMeat.SausageType.H4,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5,
		FlyingCowboyLevelMeat.SausageType.L5
	};

	// Token: 0x04002A41 RID: 10817
	private static readonly FlyingCowboyLevelMeat.SausageType[] SausageTypeDown = new FlyingCowboyLevelMeat.SausageType[]
	{
		FlyingCowboyLevelMeat.SausageType.D1,
		FlyingCowboyLevelMeat.SausageType.D2,
		FlyingCowboyLevelMeat.SausageType.D3
	};

	// Token: 0x04002A42 RID: 10818
	private static readonly float[] SausageGapWidths = new float[]
	{
		146f,
		189f,
		286f
	};

	// Token: 0x04002A43 RID: 10819
	private static readonly string[] SausageGapAnimationNames = new string[]
	{
		"String1",
		"String2",
		"String3"
	};

	// Token: 0x04002A44 RID: 10820
	private static readonly int LowSausageLinkSortingOrder = 20;

	// Token: 0x04002A45 RID: 10821
	private static readonly int MidSausageLinkSortingOrder = 40;

	// Token: 0x04002A46 RID: 10822
	private static readonly int HighSausageLinkSortingOrder = 60;

	// Token: 0x04002A47 RID: 10823
	[Header("Sausage")]
	[SerializeField]
	private Transform sausageSpawnPosition;

	// Token: 0x04002A48 RID: 10824
	[SerializeField]
	private FlyingCowboyLevelBeans beansPrefab;

	// Token: 0x04002A49 RID: 10825
	[SerializeField]
	private FlyingCowboyLevelSpinningBullet sausageRunSpitBullet;

	// Token: 0x04002A4A RID: 10826
	[SerializeField]
	private Effect sausageRunSpitBulletEffect;

	// Token: 0x04002A4B RID: 10827
	[SerializeField]
	private Transform runTopSpitBulletSpawn;

	// Token: 0x04002A4C RID: 10828
	[SerializeField]
	private Transform runTopSpitBulletEffectSpawn;

	// Token: 0x04002A4D RID: 10829
	[SerializeField]
	private Transform runBottomSpitBulletSpawn;

	// Token: 0x04002A4E RID: 10830
	[SerializeField]
	private Transform runBottomSpitBulletEffectSpawn;

	// Token: 0x04002A4F RID: 10831
	[SerializeField]
	private Vector2 sausageWobbleRadius;

	// Token: 0x04002A50 RID: 10832
	[SerializeField]
	private Vector2 sausageWobbleDuration;

	// Token: 0x04002A51 RID: 10833
	[Header("Can")]
	[SerializeField]
	private Transform canTransform;

	// Token: 0x04002A52 RID: 10834
	[SerializeField]
	private GameObject sausageTransforms;

	// Token: 0x04002A53 RID: 10835
	[SerializeField]
	private BasicProjectile canBullet;

	// Token: 0x04002A54 RID: 10836
	[SerializeField]
	private Effect canBulletMuzzleFX;

	// Token: 0x04002A55 RID: 10837
	[SerializeField]
	private Transform bulletRoot;

	// Token: 0x04002A56 RID: 10838
	[SerializeField]
	private Transform shadowTransform;

	// Token: 0x04002A57 RID: 10839
	[SerializeField]
	private BasicProjectile sausage;

	// Token: 0x04002A58 RID: 10840
	[SerializeField]
	private Transform sausageLinkSqueezePoint;

	// Token: 0x04002A59 RID: 10841
	[SerializeField]
	private Transform sausageHolderA;

	// Token: 0x04002A5A RID: 10842
	[SerializeField]
	private Transform sausageHolderB;

	// Token: 0x04002A5B RID: 10843
	[SerializeField]
	private Transform nextBulletSpawnPointA;

	// Token: 0x04002A5C RID: 10844
	[SerializeField]
	private Transform nextBulletSpawnPointB;

	// Token: 0x04002A5D RID: 10845
	[SerializeField]
	private FlyingCowboyFloatingSausages floatingSausage;

	// Token: 0x04002A5E RID: 10846
	[SerializeField]
	private Transform floatingSausageSpawnPointLeft;

	// Token: 0x04002A5F RID: 10847
	[SerializeField]
	private Transform floatingSausageSpawnPointRight;

	// Token: 0x04002A60 RID: 10848
	[SerializeField]
	private BasicProjectile sausageString;

	// Token: 0x04002A61 RID: 10849
	[SerializeField]
	private TriggerZone[] beanCanTriggerZones;

	// Token: 0x04002A62 RID: 10850
	[SerializeField]
	private Effect sausageDeathEffect;

	// Token: 0x04002A63 RID: 10851
	[SerializeField]
	private Effect sausageStringDeathEffect;

	// Token: 0x04002A64 RID: 10852
	private AbstractPlayerController player;

	// Token: 0x04002A65 RID: 10853
	private FlyingCowboyLevelMeat.MeatPhase meatPhase;

	// Token: 0x04002A66 RID: 10854
	private DamageDealer damageDealer;

	// Token: 0x04002A67 RID: 10855
	private DamageReceiver damageReceiver;

	// Token: 0x04002A68 RID: 10856
	private PatternString runningSpitBulletParryPattern;

	// Token: 0x04002A69 RID: 10857
	private PatternString sausageTimeToMoveString;

	// Token: 0x04002A6A RID: 10858
	private PatternString spitBulletParryString;

	// Token: 0x04002A6B RID: 10859
	private bool isFlying;

	// Token: 0x04002A6C RID: 10860
	private bool waitingToShoot;

	// Token: 0x04002A6D RID: 10861
	private bool isDead;

	// Token: 0x04002A6E RID: 10862
	private bool canBulletsTriggered;

	// Token: 0x04002A6F RID: 10863
	private int currentSausageLinkSortingOrderA = FlyingCowboyLevelMeat.MidSausageLinkSortingOrder;

	// Token: 0x04002A70 RID: 10864
	private int currentSausageLinkSortingOrderB = FlyingCowboyLevelMeat.MidSausageLinkSortingOrder + 1;

	// Token: 0x02000654 RID: 1620
	public enum MeatPhase
	{
		// Token: 0x04002A72 RID: 10866
		Can,
		// Token: 0x04002A73 RID: 10867
		Sausage,
		// Token: 0x04002A74 RID: 10868
		Switching
	}

	// Token: 0x02000655 RID: 1621
	public enum SausageType
	{
		// Token: 0x04002A76 RID: 10870
		H1,
		// Token: 0x04002A77 RID: 10871
		H2,
		// Token: 0x04002A78 RID: 10872
		H3,
		// Token: 0x04002A79 RID: 10873
		H4,
		// Token: 0x04002A7A RID: 10874
		L5,
		// Token: 0x04002A7B RID: 10875
		U1,
		// Token: 0x04002A7C RID: 10876
		U2,
		// Token: 0x04002A7D RID: 10877
		U3,
		// Token: 0x04002A7E RID: 10878
		D1,
		// Token: 0x04002A7F RID: 10879
		D2,
		// Token: 0x04002A80 RID: 10880
		D3
	}
}
