using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004B5 RID: 1205
public class AirplaneLevelBulldogPlane : LevelProperties.Airplane.Entity
{
	// Token: 0x060013CA RID: 5066 RVA: 0x000AF164 File Offset: 0x000AD564
	private void Start()
	{
		this.state = AirplaneLevelBulldogPlane.State.Intro;
		this.startPosY = 256f;
		this.baseX = base.transform.position.x;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = this.bullDogPlane.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.bulldogParachute.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.bulldogCatAttack.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.bulldogParachute.gameObject.SetActive(false);
		this.bulldogCatAttack.gameObject.SetActive(false);
		base.StartCoroutine(this.idle_timer_cr());
		base.StartCoroutine(this.rotate_cr());
	}

	// Token: 0x060013CB RID: 5067 RVA: 0x000AF243 File Offset: 0x000AD643
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060013CC RID: 5068 RVA: 0x000AF259 File Offset: 0x000AD659
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060013CD RID: 5069 RVA: 0x000AF274 File Offset: 0x000AD674
	private void FixedUpdate()
	{
		if (this.state == AirplaneLevelBulldogPlane.State.Intro)
		{
			return;
		}
		this.moveTime += Time.fixedDeltaTime;
		Vector2 vector = base.transform.position;
		vector.y = Mathf.Sin(this.moveTime / 0.3f) * 4f;
		if (this.bounceXTimer > 0f)
		{
			this.bounceXTimer -= CupheadTime.FixedDelta * 3f * ((this.bounceXTimer <= 0.5f) ? 0.25f : 1f);
			this.bounceX = ((this.bounceXTimer <= 0.5f) ? (this.bounceX = EaseUtils.EaseInOutSine(30f, 0f, 1f - this.bounceXTimer * 2f)) : (Mathf.Sin(this.bounceXTimer * 3.1415927f) * 30f));
			this.bounceX *= this.bounceXDir;
		}
		else
		{
			this.bounceXTimer = 0f;
			this.bounceX = 0f;
		}
		if (this.bounceYTimer > 0f)
		{
			this.bounceYTimer -= CupheadTime.FixedDelta * ((!this.exitBounce) ? 1.7f : 1.6f);
			this.bounceY = ((this.bounceYTimer <= 0.5f) ? (this.bounceY = EaseUtils.EaseInOutSine((!this.exitBounce) ? 60f : 40f, 0f, 1f - this.bounceYTimer * 2f)) : (Mathf.Sin(this.bounceYTimer * 3.1415927f) * ((!this.exitBounce) ? 60f : 40f)));
		}
		else
		{
			this.bounceYTimer = 0f;
			this.bounceY = 0f;
		}
		base.transform.SetPosition(new float?(this.baseX + Mathf.Sin(this.wobbleTimer * 3f) * this.wobbleX + this.bounceX), new float?(this.startPosY + vector.y - this.bounceY + Mathf.Sin(this.wobbleTimer * 2f) * this.wobbleY), null);
		this.wobbleTimer += CupheadTime.FixedDelta * this.wobbleSpeed;
		if (!this.isDead)
		{
			this.smokePuffLTimer -= CupheadTime.FixedDelta;
			this.smokePuffRTimer -= CupheadTime.FixedDelta;
			if (this.smokePuffLTimer <= 0f)
			{
				this.smokePuff[this.smokePuffLCounter % 3].Play((this.smokePuffLCounter % 4).ToString(), 0, 0f);
				this.smokePuff[this.smokePuffLCounter % 3].transform.localPosition = Vector3.left * 300f + Vector3.up * 50f;
				this.smokePuffLTimer += 0.25f;
				this.smokePuffLCounter++;
			}
			if (this.smokePuffRTimer <= 0f)
			{
				this.smokePuff[this.smokePuffRCounter % 3 + 3].Play((this.smokePuffRCounter % 4).ToString(), 0, 0f);
				this.smokePuff[this.smokePuffRCounter % 3 + 3].transform.localPosition = Vector3.right * 300f + Vector3.up * 50f;
				this.smokePuffRTimer += 0.27f;
				this.smokePuffRCounter++;
			}
		}
		for (int i = 0; i < this.smokePuff.Length; i++)
		{
			this.smokePuff[i].transform.localPosition += new Vector3((float)((i >= 3) ? -3 : 3), 2f - 4f * this.smokePuff[i].GetCurrentAnimatorStateInfo(0).normalizedTime) * CupheadTime.FixedDelta * 100f;
		}
	}

	// Token: 0x060013CE RID: 5070 RVA: 0x000AF700 File Offset: 0x000ADB00
	private IEnumerator rotate_cr()
	{
		float t = 0f;
		float time = 4f;
		float maxAngle = 1f;
		base.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -maxAngle));
		for (;;)
		{
			while (t < time)
			{
				t += CupheadTime.Delta;
				base.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Mathf.Lerp(-maxAngle, maxAngle, EaseUtils.EaseInOutSine(0f, 1f, t / time))));
				yield return null;
			}
			t = 0f;
			maxAngle = -maxAngle;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060013CF RID: 5071 RVA: 0x000AF71B File Offset: 0x000ADB1B
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.dontDamage)
		{
			return;
		}
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x060013D0 RID: 5072 RVA: 0x000AF73A File Offset: 0x000ADB3A
	public override void LevelInit(LevelProperties.Airplane properties)
	{
		base.LevelInit(properties);
		this.sideString = new PatternString(properties.CurrentState.parachute.sideString, true);
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x060013D1 RID: 5073 RVA: 0x000AF76C File Offset: 0x000ADB6C
	private IEnumerator intro_cr()
	{
		this.leaderIntroBG.SetTrigger("Continue");
		YieldInstruction wait = new WaitForFixedUpdate();
		yield return this.bullDogPlane.WaitForAnimationToStart(this, "Intro", false);
		int target = Animator.StringToHash(this.bullDogPlane.GetLayerName(0) + ".Intro");
		while (this.bullDogPlane.GetCurrentAnimatorStateInfo(0).fullPathHash == target)
		{
			float s = this.bullDogPlane.GetCurrentAnimatorStateInfo(0).normalizedTime;
			if (s > 0.7f && s < 0.95f)
			{
				((AirplaneLevel)Level.Current).UpdateShadow(1f - Mathf.Sin(Mathf.InverseLerp(0.7f, 0.95f, s) * 3.1415927f) * 0.2f);
			}
			else
			{
				((AirplaneLevel)Level.Current).UpdateShadow(1f);
			}
			yield return wait;
		}
		((AirplaneLevel)Level.Current).UpdateShadow(1f);
		yield return CupheadTime.WaitForSeconds(this, 0.35f);
		this.SFX_DOGFIGHT_BulldogPlane_Loop();
		this.SFX_DOGFIGHT_Intro_BulldogPlaneDecend();
		base.StartCoroutine(this.turret_cr());
		base.StartCoroutine(this.mainattack_cr());
		float t = 0f;
		float time = 0.8f;
		float endTime = 0.4f;
		float start = base.transform.position.y;
		base.StartCoroutine(this.scale_in_cr());
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0f, 1f, t / time);
			base.transform.SetPosition(null, new float?(Mathf.Lerp(start, 156f, val)), null);
			yield return wait;
		}
		t = 0f;
		start = base.transform.position.y;
		while (t < endTime)
		{
			t += CupheadTime.FixedDelta;
			float val2 = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0f, 1f, t / endTime);
			base.transform.SetPosition(null, new float?(Mathf.Lerp(start, 256f, val2)), null);
			yield return wait;
		}
		base.transform.SetPosition(null, new float?(256f), null);
		if (this.state == AirplaneLevelBulldogPlane.State.Intro)
		{
			this.state = AirplaneLevelBulldogPlane.State.Main;
		}
		base.StartCoroutine(this.move_cr());
		yield return null;
		yield break;
	}

	// Token: 0x060013D2 RID: 5074 RVA: 0x000AF788 File Offset: 0x000ADB88
	private IEnumerator scale_in_cr()
	{
		base.transform.localScale = new Vector3(0.8f, 0.8f);
		YieldInstruction wait = new WaitForFixedUpdate();
		float t = 0f;
		float frameTime = 0f;
		while (t < 1.2f)
		{
			while (frameTime < 0.041666668f)
			{
				frameTime += CupheadTime.FixedDelta;
				yield return wait;
			}
			t += frameTime;
			frameTime -= 0.041666668f;
			base.transform.localScale = Vector3.Lerp(new Vector3(0.8f, 0.8f), new Vector3(1f, 1f), EaseUtils.EaseOutSine(0f, 1f, Mathf.InverseLerp(0f, 1.2f, t)));
		}
		base.transform.localScale = new Vector3(1f, 1f);
		yield break;
	}

	// Token: 0x060013D3 RID: 5075 RVA: 0x000AF7A3 File Offset: 0x000ADBA3
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x060013D4 RID: 5076 RVA: 0x000AF7B4 File Offset: 0x000ADBB4
	private IEnumerator mainattack_cr()
	{
		LevelProperties.Airplane.Main p = base.properties.CurrentState.main;
		PatternString attackType = new PatternString(p.attackType, true);
		for (;;)
		{
			if (this.firstAttack)
			{
				yield return CupheadTime.WaitForSeconds(this, 0.6f);
				yield return CupheadTime.WaitForSeconds(this, p.firstAttackDelay);
				this.firstAttack = false;
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, p.attackDelayRange.RandomFloat());
			}
			char c = attackType.PopLetter();
			if (c != 'P')
			{
				if (c == 'T')
				{
					yield return base.StartCoroutine(this.catattack_cr());
				}
			}
			else
			{
				yield return base.StartCoroutine(this.parachute_cr());
			}
			this.state = AirplaneLevelBulldogPlane.State.Main;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060013D5 RID: 5077 RVA: 0x000AF7D0 File Offset: 0x000ADBD0
	private IEnumerator idle_timer_cr()
	{
		bool pickSide = Rand.Bool();
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(2f, 3f));
			if (this.state == AirplaneLevelBulldogPlane.State.Main)
			{
				pickSide = ((!Rand.Bool()) ? (base.transform.position.x > this.canteenPlane.transform.position.x) : Rand.Bool());
				string side = (!pickSide) ? "Right" : "Left";
				this.bullDogPlane.SetTrigger("OnIdle" + side);
				this.bullDogPlane.SetInteger("IdleLoopCount", (pickSide != base.transform.position.x > this.canteenPlane.transform.position.x) ? 2 : 0);
				yield return this.bullDogPlane.WaitForAnimationToStart(this, "Idle", false);
			}
			while (this.state != AirplaneLevelBulldogPlane.State.Main)
			{
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060013D6 RID: 5078 RVA: 0x000AF7EC File Offset: 0x000ADBEC
	private IEnumerator move_cr()
	{
		this.movingRight = Rand.Bool();
		float t = 0f;
		float time = base.properties.CurrentState.main.moveTime;
		float start = 0f;
		float end = 0f;
		float speedModifier = 1f;
		for (;;)
		{
			t = 0f;
			start = base.transform.position.x;
			end = ((!this.movingRight) ? -245f : 245f);
			while (t < time)
			{
				t += CupheadTime.FixedDelta * speedModifier;
				speedModifier = Mathf.Clamp(speedModifier + ((this.state != AirplaneLevelBulldogPlane.State.Main) ? -0.01f : 0.01f), 0f, 1f);
				float val = t / time;
				this.baseX = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, end, val);
				yield return new WaitForFixedUpdate();
			}
			base.transform.SetPosition(new float?(end), null, null);
			this.movingRight = !this.movingRight;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060013D7 RID: 5079 RVA: 0x000AF808 File Offset: 0x000ADC08
	private IEnumerator turret_cr()
	{
		LevelProperties.Airplane.Turrets p = base.properties.CurrentState.turrets;
		PatternString positionString = new PatternString(p.positionString, true, true);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, p.attackDelayRange.RandomFloat());
			this.turretSpawnPoints[positionString.PopInt()].StartAttack(p.velocityX, p.velocityY, p.gravity);
		}
		yield break;
	}

	// Token: 0x060013D8 RID: 5080 RVA: 0x000AF823 File Offset: 0x000ADC23
	public void StartRocket()
	{
		base.StartCoroutine(this.rocket_cr());
	}

	// Token: 0x060013D9 RID: 5081 RVA: 0x000AF834 File Offset: 0x000ADC34
	private IEnumerator rocket_cr()
	{
		LevelProperties.Airplane.Rocket p = base.properties.CurrentState.rocket;
		PatternString delayString = new PatternString(p.attackDelayString, true, true);
		PatternString dirString = new PatternString(p.attackOrderString, true, true);
		this.hydrantAttackBG.Play("Fly");
		yield return this.hydrantAttackBG.WaitForAnimationToEnd(this, "Fly", false, true);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, (float)delayString.PopInt());
			Vector3 position = (dirString.PopLetter() != 'R') ? this.rocketSpawnLeft.position : this.rocketSpawnRight.position;
			this.rocketPrefab.Create(PlayerManager.GetNext(), position, p.homingSpeed, p.homingRotation, p.homingHP, p.homingTime);
		}
		yield break;
	}

	// Token: 0x060013DA RID: 5082 RVA: 0x000AF850 File Offset: 0x000ADC50
	private IEnumerator parachute_cr()
	{
		bool onLeft = this.sideString.PopLetter() == 'L';
		this.bullDogPlane.SetInteger("IdleLoopCount", 3);
		this.bullDogPlane.SetBool("InParachuteATK", true);
		yield return this.bullDogPlane.WaitForAnimationToStart(this, "Parachute_Start", false);
		this.state = AirplaneLevelBulldogPlane.State.Parachute;
		yield return CupheadTime.WaitForSeconds(this, 0.6666667f);
		this.exitBounce = true;
		this.bounceYTimer = 1f;
		this.bullDogPlane.GetComponent<Collider2D>().enabled = false;
		yield return this.bullDogPlane.WaitForAnimationToEnd(this, "Parachute_Start", false, false);
		this.SFX_DOGFIGHT_Bulldog_ParachuteDown();
		float posX = (!onLeft) ? 575f : -575f;
		Vector3 pos = new Vector3(posX, 0f);
		float scale = (float)((!onLeft) ? -1 : 1);
		yield return CupheadTime.WaitForSeconds(this, 0.35f);
		this.bulldogParachute.gameObject.SetActive(true);
		this.bulldogParachute.StartDescent(pos, scale);
		while (this.bulldogParachute.isMoving)
		{
			yield return null;
		}
		this.bulldogParachute.gameObject.SetActive(false);
		this.bullDogPlane.SetBool("InParachuteATK", false);
		yield return CupheadTime.WaitForSeconds(this, 0.125f);
		this.exitBounce = false;
		this.bounceYTimer = 1f;
		this.SFX_DOGFIGHT_BulldogPlane_ParachuteDownStop();
		yield return this.bullDogPlane.WaitForAnimationToEnd(this, "Parachute_End", false, false);
		this.bullDogPlane.GetComponent<Collider2D>().enabled = true;
		yield break;
	}

	// Token: 0x060013DB RID: 5083 RVA: 0x000AF86C File Offset: 0x000ADC6C
	private IEnumerator catattack_cr()
	{
		LevelProperties.Airplane.Triple p = base.properties.CurrentState.triple;
		bool onLeft = this.sideString.PopLetter() == 'L';
		this.bullDogPlane.SetInteger("IdleLoopCount", 3);
		this.bullDogPlane.SetBool("InParachuteATK", true);
		this.bullDogPlane.SetBool("OnLeft", onLeft);
		yield return this.bullDogPlane.WaitForAnimationToStart(this, "Parachute_Start", false);
		this.state = AirplaneLevelBulldogPlane.State.CatAttack;
		yield return CupheadTime.WaitForSeconds(this, 0.6666667f);
		this.exitBounce = true;
		this.bounceYTimer = 1f;
		this.bullDogPlane.GetComponent<Collider2D>().enabled = false;
		yield return this.bullDogPlane.WaitForAnimationToEnd(this, "Parachute_Start", false, false);
		yield return CupheadTime.WaitForSeconds(this, 0.35f);
		float posX = (!onLeft) ? 600f : -600f;
		Vector3 startPos = new Vector3(posX, p.yHeight);
		this.bulldogCatAttack.gameObject.SetActive(true);
		yield return null;
		this.bulldogCatAttack.StartCat(startPos);
		while (this.bulldogCatAttack.isAttacking)
		{
			yield return null;
		}
		this.bulldogCatAttack.gameObject.SetActive(false);
		this.bullDogPlane.SetBool("InParachuteATK", false);
		yield return CupheadTime.WaitForSeconds(this, 0.125f);
		this.exitBounce = false;
		this.bounceYTimer = 1f;
		this.SFX_DOGFIGHT_BulldogPlane_ParachuteDownStop();
		yield return this.bullDogPlane.WaitForAnimationToEnd(this, "Parachute_End", false, false);
		this.bullDogPlane.GetComponent<Collider2D>().enabled = true;
		yield break;
	}

	// Token: 0x060013DC RID: 5084 RVA: 0x000AF888 File Offset: 0x000ADC88
	public void OnStageChange()
	{
		this.endPhaseOne = true;
		this.dontDamage = true;
		if (this.bulldogCatAttack.isAttacking)
		{
			this.bulldogCatAttack.EarlyExit();
		}
		if (this.bulldogParachute.isMoving)
		{
			this.bulldogParachute.EarlyExit();
		}
	}

	// Token: 0x060013DD RID: 5085 RVA: 0x000AF8D9 File Offset: 0x000ADCD9
	public void BulldogDeath()
	{
		this.isDead = true;
		this.StopAllCoroutines();
		base.StartCoroutine(this.death_cr());
	}

	// Token: 0x060013DE RID: 5086 RVA: 0x000AF8F8 File Offset: 0x000ADCF8
	private IEnumerator death_cr()
	{
		this.bullDogPlane.SetBool("Dead", true);
		this.bullDogPlane.SetBool("InTripleATK", false);
		if (!this.bulldogParachute.gameObject.activeInHierarchy)
		{
			this.bullDogPlane.SetBool("InParachuteATK", false);
		}
		yield return this.bullDogPlane.WaitForAnimationToStart(this, "Death", false);
		this.startPhaseTwo = true;
		this.SFX_DOGFIGHT_BulldogPlane_StopLoop();
		this.bullDogPlane.GetComponent<Collider2D>().enabled = false;
		this.bullDogPlane.SetLayerWeight(1, 0f);
		this.bullDogPlane.SetLayerWeight(2, 0f);
		this.bullDogPlane.SetLayerWeight(3, 0f);
		yield return this.bullDogPlane.WaitForAnimationToEnd(this, "Death", false, true);
		UnityEngine.Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x060013DF RID: 5087 RVA: 0x000AF913 File Offset: 0x000ADD13
	private void SFX_DOGFIGHT_Intro_BulldogPlaneDecend()
	{
		AudioManager.Play("sfx_dlc_dogfight_bulldogplane_introdecend");
	}

	// Token: 0x060013E0 RID: 5088 RVA: 0x000AF91F File Offset: 0x000ADD1F
	private void SFX_DOGFIGHT_BulldogPlane_Loop()
	{
		AudioManager.PlayLoop("sfx_dlc_dogfight_bulldogplane_loop");
		AudioManager.FadeSFXVolumeLinear("sfx_dlc_dogfight_bulldogplane_loop", 0.25f, 3f);
	}

	// Token: 0x060013E1 RID: 5089 RVA: 0x000AF93F File Offset: 0x000ADD3F
	private void SFX_DOGFIGHT_BulldogPlane_StopLoop()
	{
		AudioManager.Stop("sfx_dlc_dogfight_bulldogplane_loop");
	}

	// Token: 0x060013E2 RID: 5090 RVA: 0x000AF94B File Offset: 0x000ADD4B
	private void SFX_DOGFIGHT_Bulldog_ParachuteDown()
	{
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_parachutedown");
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_p1_bulldog_parachutedown");
		AudioManager.Play("sfx_DLC_Dogfight_P1_Bulldog_ParachuteFlump");
	}

	// Token: 0x060013E3 RID: 5091 RVA: 0x000AF971 File Offset: 0x000ADD71
	private void SFX_DOGFIGHT_BulldogPlane_ParachuteDownStop()
	{
		AudioManager.Stop("sfx_dlc_dogfight_p1_bulldog_parachutedown");
	}

	// Token: 0x060013E4 RID: 5092 RVA: 0x000AF980 File Offset: 0x000ADD80
	private void WORKAROUND_NullifyFields()
	{
		this.leaderIntroBG = null;
		this.hydrantAttackBG = null;
		this.turretSpawnPoints = null;
		this.rocketSpawnLeft = null;
		this.rocketSpawnRight = null;
		this.rocketPrefab = null;
		this.bullDogPlane = null;
		this.bulldogParachute = null;
		this.bulldogCatAttack = null;
		this.canteenPlane = null;
		this.damageDealer = null;
		this.sideString = null;
		this.smokePuff = null;
	}

	// Token: 0x04001CD9 RID: 7385
	private const float MOVE_POS_X = 245f;

	// Token: 0x04001CDA RID: 7386
	private const float PARACHUTE_POS_X = 575f;

	// Token: 0x04001CDB RID: 7387
	private const float PARACHUTE_APPEAR_DELAY = 0.35f;

	// Token: 0x04001CDC RID: 7388
	private const float PARACHUTE_EXIT_BOUNCE_HEIGHT = 40f;

	// Token: 0x04001CDD RID: 7389
	private const float PARACHUTE_EXIT_BOUNCE_SPEED = 1.6f;

	// Token: 0x04001CDE RID: 7390
	private const float PARACHUTE_EXIT_BOUNCE_FRAME_DELAY = 16f;

	// Token: 0x04001CDF RID: 7391
	private const float PARACHUTE_RETURN_BOUNCE_HEIGHT = 60f;

	// Token: 0x04001CE0 RID: 7392
	private const float PARACHUTE_RETURN_BOUNCE_SPEED = 1.7f;

	// Token: 0x04001CE1 RID: 7393
	private const float PARACHUTE_RETURN_BOUNCE_FRAME_DELAY = 3f;

	// Token: 0x04001CE2 RID: 7394
	private const float BUMP_RETURN_BOUNCE_HEIGHT = 30f;

	// Token: 0x04001CE3 RID: 7395
	private const float BUMP_RETURN_BOUNCE_SPEED = 3f;

	// Token: 0x04001CE4 RID: 7396
	private const float CAT_ATTACK_POS_X = 600f;

	// Token: 0x04001CE5 RID: 7397
	private const float CAT_ATTACK_APPEAR_DELAY = 0.35f;

	// Token: 0x04001CE6 RID: 7398
	private const float MOVE_DOWN_POS = 256f;

	// Token: 0x04001CE7 RID: 7399
	private const float MOVE_TIME = 0.3f;

	// Token: 0x04001CE8 RID: 7400
	private const float MOVE_LENGTH = 4f;

	// Token: 0x04001CE9 RID: 7401
	public AirplaneLevelBulldogPlane.State state;

	// Token: 0x04001CEA RID: 7402
	[SerializeField]
	private Animator leaderIntroBG;

	// Token: 0x04001CEB RID: 7403
	[SerializeField]
	private Animator hydrantAttackBG;

	// Token: 0x04001CEC RID: 7404
	[Header("Roots")]
	[SerializeField]
	private AirplaneLevelTurretDog[] turretSpawnPoints;

	// Token: 0x04001CED RID: 7405
	[SerializeField]
	private Transform rocketSpawnLeft;

	// Token: 0x04001CEE RID: 7406
	[SerializeField]
	private Transform rocketSpawnRight;

	// Token: 0x04001CEF RID: 7407
	[Header("Prefabs")]
	[SerializeField]
	private AirplaneLevelRocket rocketPrefab;

	// Token: 0x04001CF0 RID: 7408
	[Header("Bulldog")]
	[SerializeField]
	private Animator bullDogPlane;

	// Token: 0x04001CF1 RID: 7409
	[SerializeField]
	private AirplaneLevelBulldogParachute bulldogParachute;

	// Token: 0x04001CF2 RID: 7410
	[SerializeField]
	private AirplaneLevelBulldogCatAttack bulldogCatAttack;

	// Token: 0x04001CF3 RID: 7411
	[SerializeField]
	private GameObject canteenPlane;

	// Token: 0x04001CF4 RID: 7412
	private DamageDealer damageDealer;

	// Token: 0x04001CF5 RID: 7413
	private DamageReceiver damageReceiver;

	// Token: 0x04001CF6 RID: 7414
	private float moveTime;

	// Token: 0x04001CF7 RID: 7415
	private float startPosY;

	// Token: 0x04001CF8 RID: 7416
	private float bounceY;

	// Token: 0x04001CF9 RID: 7417
	private float bounceYTimer;

	// Token: 0x04001CFA RID: 7418
	private float bounceX;

	// Token: 0x04001CFB RID: 7419
	private float bounceXTimer;

	// Token: 0x04001CFC RID: 7420
	private float bounceXDir = 1f;

	// Token: 0x04001CFD RID: 7421
	private bool exitBounce;

	// Token: 0x04001CFE RID: 7422
	private float baseX;

	// Token: 0x04001CFF RID: 7423
	private float wobbleTimer;

	// Token: 0x04001D00 RID: 7424
	[SerializeField]
	private float wobbleX = 10f;

	// Token: 0x04001D01 RID: 7425
	[SerializeField]
	private float wobbleY = 10f;

	// Token: 0x04001D02 RID: 7426
	[SerializeField]
	private float wobbleSpeed = 1f;

	// Token: 0x04001D03 RID: 7427
	private bool movingRight;

	// Token: 0x04001D04 RID: 7428
	private bool dontDamage;

	// Token: 0x04001D05 RID: 7429
	public bool endPhaseOne;

	// Token: 0x04001D06 RID: 7430
	private bool firstAttack = true;

	// Token: 0x04001D07 RID: 7431
	private PatternString sideString;

	// Token: 0x04001D08 RID: 7432
	[SerializeField]
	private Animator[] smokePuff;

	// Token: 0x04001D09 RID: 7433
	private int smokePuffLCounter;

	// Token: 0x04001D0A RID: 7434
	private int smokePuffRCounter = 2;

	// Token: 0x04001D0B RID: 7435
	private float smokePuffLTimer;

	// Token: 0x04001D0C RID: 7436
	private float smokePuffRTimer;

	// Token: 0x04001D0D RID: 7437
	public bool isDead;

	// Token: 0x04001D0E RID: 7438
	public bool startPhaseTwo;

	// Token: 0x020004B6 RID: 1206
	public enum State
	{
		// Token: 0x04001D10 RID: 7440
		Intro,
		// Token: 0x04001D11 RID: 7441
		Main,
		// Token: 0x04001D12 RID: 7442
		Parachute,
		// Token: 0x04001D13 RID: 7443
		TripleAttack,
		// Token: 0x04001D14 RID: 7444
		CatAttack
	}
}
