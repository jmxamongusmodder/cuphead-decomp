using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007A6 RID: 1958
public class SallyStagePlayLevelBackgroundHandler : AbstractPausableComponent
{
	// Token: 0x17000404 RID: 1028
	// (get) Token: 0x06002BED RID: 11245 RVA: 0x0019BAC9 File Offset: 0x00199EC9
	// (set) Token: 0x06002BEE RID: 11246 RVA: 0x0019BAD0 File Offset: 0x00199ED0
	public static bool HUSBAND_GONE { get; private set; }

	// Token: 0x17000405 RID: 1029
	// (get) Token: 0x06002BEF RID: 11247 RVA: 0x0019BAD8 File Offset: 0x00199ED8
	// (set) Token: 0x06002BF0 RID: 11248 RVA: 0x0019BADF File Offset: 0x00199EDF
	public static bool CURTAIN_OPEN { get; private set; }

	// Token: 0x06002BF1 RID: 11249 RVA: 0x0019BAE7 File Offset: 0x00199EE7
	protected override void Awake()
	{
		base.Awake();
		this.curtain.gameObject.SetActive(true);
	}

	// Token: 0x06002BF2 RID: 11250 RVA: 0x0019BB00 File Offset: 0x00199F00
	private void Start()
	{
		Level.Current.OnLevelStartEvent += this.StartPriestLoop;
		Level.Current.OnLevelStartEvent += this.StartHusbandLoop;
		this.curtainStartPos = this.curtainSprite.position;
		this.curtainShadowStartPos = this.curtainShadow.position;
		this.chandelierStartPosX = this.chandelier.transform.position.x;
		SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE = false;
		foreach (SallyStagePlayLevelBackgroundHandler.Cupid cupid in this.cupids)
		{
			cupid.startPosition = cupid.cupidTransform.position;
		}
		this.applauseHandler.SlideApplause(true);
	}

	// Token: 0x06002BF3 RID: 11251 RVA: 0x0019BBBC File Offset: 0x00199FBC
	public void GetProperties(LevelProperties.SallyStagePlay properties, SallyStagePlayLevel parent)
	{
		this.properties = properties;
		this.parent = parent;
		foreach (SpriteRenderer flicker in this.flickeringLights)
		{
			base.StartCoroutine(this.flicker_cr(flicker));
		}
		this.phaseDependentCoroutines = new List<Coroutine>();
		this.phaseDependentCoroutines.Add(base.StartCoroutine(this.check_bools_cr()));
		for (int j = 0; j < this.cupids.Length; j++)
		{
			this.phaseDependentCoroutines.Add(base.StartCoroutine(this.cupid_check_falling(this.cupids[j])));
		}
		foreach (Transform swing in this.churchSwingies)
		{
			this.phaseDependentCoroutines.Add(base.StartCoroutine(this.swing_cr(swing)));
		}
		AbstractPlayerController next = PlayerManager.GetNext();
		LevelPlayerController levelPlayerController = (LevelPlayerController)next;
		levelPlayerController.motor.OnHitEvent += this.PlayYay;
		parent.OnPhase2 += this.OnPhase2;
		parent.OnPhase3 += this.OnPhase3;
		parent.OnPhase4 += this.OnPhase4;
	}

	// Token: 0x06002BF4 RID: 11252 RVA: 0x0019BCFD File Offset: 0x0019A0FD
	public void OpenCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds backgroundSelected)
	{
		if (backgroundSelected != SallyStagePlayLevelBackgroundHandler.Backgrounds.Finale)
		{
			this.applauseHandler.SlideApplause(false);
		}
		base.StartCoroutine(this.open_curtain_cr(backgroundSelected));
	}

	// Token: 0x06002BF5 RID: 11253 RVA: 0x0019BD20 File Offset: 0x0019A120
	public void CloseCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds backgroundSelected)
	{
		this.applauseHandler.SlideApplause(true);
		base.StartCoroutine(this.close_curtain_cr(backgroundSelected));
	}

	// Token: 0x06002BF6 RID: 11254 RVA: 0x0019BD3C File Offset: 0x0019A13C
	private IEnumerator open_curtain_cr(SallyStagePlayLevelBackgroundHandler.Backgrounds backgroundsSelected)
	{
		this.SelectBackground(backgroundsSelected);
		float t = 0f;
		float frameTime = 0f;
		float openTime = 1.58f;
		Vector3 shadowRoot = new Vector3(this.curtainUpRoot.position.x, this.curtainUpRoot.position.y - 100f);
		AudioManager.Play("sally_bg_stage_curtain_raise");
		this.emitAudioFromObject.Add("sally_bg_stage_curtain_raise");
		while (t < openTime)
		{
			frameTime += CupheadTime.Delta;
			t += CupheadTime.Delta;
			if (frameTime > 0.041666668f)
			{
				float t2 = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / openTime);
				this.curtainSprite.transform.position = Vector2.Lerp(this.curtainSprite.transform.position, this.curtainUpRoot.position, t2);
				this.curtainShadow.transform.position = Vector2.Lerp(this.curtainShadow.transform.position, shadowRoot, t2);
				frameTime -= 0.041666668f;
			}
			yield return null;
		}
		SallyStagePlayLevelBackgroundHandler.CURTAIN_OPEN = true;
		yield return null;
		yield break;
	}

	// Token: 0x06002BF7 RID: 11255 RVA: 0x0019BD60 File Offset: 0x0019A160
	private IEnumerator close_curtain_cr(SallyStagePlayLevelBackgroundHandler.Backgrounds backgroundSelected)
	{
		float t = 0f;
		float frameTime = 0f;
		float closeTime = 1.58f;
		AudioManager.Play("sally_bg_stage_curtain_lower");
		this.emitAudioFromObject.Add("sally_bg_stage_curtain_lower");
		switch (backgroundSelected)
		{
		case SallyStagePlayLevelBackgroundHandler.Backgrounds.House:
			AudioManager.Play("sally_bg_stage_reset_phase1");
			this.emitAudioFromObject.Add("sally_bg_stage_reset_phase1");
			break;
		case SallyStagePlayLevelBackgroundHandler.Backgrounds.Nunnery:
			AudioManager.Play("sally_bg_stage_reset_phase1");
			this.emitAudioFromObject.Add("sally_bg_stage_reset_phase1");
			break;
		case SallyStagePlayLevelBackgroundHandler.Backgrounds.Purgatory:
			AudioManager.Play("sally_bg_stage_reset_phase2");
			this.emitAudioFromObject.Add("sally_bg_stage_reset_phase2");
			break;
		case SallyStagePlayLevelBackgroundHandler.Backgrounds.Finale:
			AudioManager.Play("sally_bg_stage_reset_phase3");
			this.emitAudioFromObject.Add("sally_bg_stage_reset_phase3");
			break;
		}
		while (t < closeTime)
		{
			frameTime += CupheadTime.Delta;
			t += CupheadTime.Delta;
			if (frameTime > 0.041666668f)
			{
				float t2 = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / closeTime);
				this.curtainSprite.transform.position = Vector2.Lerp(this.curtainSprite.transform.position, this.curtainStartPos, t2);
				this.curtainShadow.transform.position = Vector2.Lerp(this.curtainShadow.transform.position, this.curtainShadowStartPos, t2);
				frameTime -= 0.041666668f;
			}
			yield return null;
		}
		SallyStagePlayLevelBackgroundHandler.CURTAIN_OPEN = false;
		yield return null;
		yield break;
	}

	// Token: 0x06002BF8 RID: 11256 RVA: 0x0019BD84 File Offset: 0x0019A184
	private void SelectBackground(SallyStagePlayLevelBackgroundHandler.Backgrounds backgroundSelected)
	{
		for (int i = 0; i < this.backgrounds.Length; i++)
		{
			if (i == (int)backgroundSelected)
			{
				this.backgrounds[i].SetActive(true);
			}
			else
			{
				this.backgrounds[i].SetActive(false);
			}
		}
	}

	// Token: 0x06002BF9 RID: 11257 RVA: 0x0019BDD4 File Offset: 0x0019A1D4
	private IEnumerator flicker_cr(SpriteRenderer flicker)
	{
		float flickerTime = 0.3f;
		for (;;)
		{
			int counter = 0;
			float waitTime = UnityEngine.Random.Range(this.fadeWaitMinSecond, this.fadeWaitMaxSecond);
			float t = 0f;
			yield return CupheadTime.WaitForSeconds(this, waitTime);
			while (counter < 2)
			{
				while (t < flickerTime)
				{
					flicker.color = new Color(1f, 1f, 1f, 1f - t / flickerTime);
					t += CupheadTime.Delta;
					yield return null;
				}
				t = 0f;
				flicker.color = new Color(1f, 1f, 1f, 0f);
				while (t < flickerTime)
				{
					flicker.color = new Color(1f, 1f, 1f, t / flickerTime);
					t += CupheadTime.Delta;
					yield return null;
				}
				flicker.color = new Color(1f, 1f, 1f, 1f);
				counter++;
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002BFA RID: 11258 RVA: 0x0019BDF8 File Offset: 0x0019A1F8
	private IEnumerator swing_cr(Transform swing)
	{
		float t = 0f;
		float speed = 0f;
		float maxSpeed = 8f;
		float minSpeed = 3f;
		bool movingRight = Rand.Bool();
		speed = minSpeed;
		for (;;)
		{
			t = ((!movingRight) ? (t - CupheadTime.Delta) : (t + CupheadTime.Delta));
			float phase = Mathf.Sin(t);
			swing.localRotation = Quaternion.Euler(new Vector3(0f, 0f, phase * speed));
			if (CupheadLevelCamera.Current.isShaking)
			{
				if (speed < maxSpeed)
				{
					speed += 0.15f;
				}
			}
			else if (speed > minSpeed)
			{
				speed -= 0.05f;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002BFB RID: 11259 RVA: 0x0019BE13 File Offset: 0x0019A213
	private void StartPriestLoop()
	{
		this.phaseDependentCoroutines.Add(base.StartCoroutine(this.priest_animations_cr()));
	}

	// Token: 0x06002BFC RID: 11260 RVA: 0x0019BE2C File Offset: 0x0019A22C
	private IEnumerator priest_animations_cr()
	{
		while (this.properties.CurrentState.stateName == LevelProperties.SallyStagePlay.States.Generic)
		{
			bool tuckDown = false;
			int counter = 0;
			int maxCounter = UnityEngine.Random.Range(2, 6);
			while (!tuckDown)
			{
				yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(2f, 5f));
				this.priest.SetTrigger("Continue");
				if (counter < maxCounter)
				{
					counter++;
				}
				else
				{
					tuckDown = true;
				}
				yield return null;
			}
			bool isLookingRight = true;
			maxCounter = UnityEngine.Random.Range(4, 8);
			counter = 0;
			this.priest.Play("Tuck_Down");
			while (tuckDown)
			{
				yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(2f, 5f));
				this.priest.SetBool("isLookingRight", isLookingRight);
				if (counter < maxCounter)
				{
					counter++;
				}
				else
				{
					tuckDown = false;
				}
				isLookingRight = !isLookingRight;
				yield return null;
			}
			this.priest.Play("Stand_Up");
			yield return null;
		}
		this.priest.Play("Look_Around");
		yield return null;
		yield break;
	}

	// Token: 0x06002BFD RID: 11261 RVA: 0x0019BE47 File Offset: 0x0019A247
	private void StartHusbandLoop()
	{
		this.husband.SetTrigger("Continue");
		this.phaseDependentCoroutines.Add(base.StartCoroutine(this.husband_move_cr()));
	}

	// Token: 0x06002BFE RID: 11262 RVA: 0x0019BE70 File Offset: 0x0019A270
	private IEnumerator husband_move_cr()
	{
		bool movingRight = true;
		float start = 0f;
		float t = 0f;
		float time = 2f;
		float end = 0f;
		float moveOffset = 400f;
		yield return this.husband.WaitForAnimationToStart(this, "Move", false);
		while (this.husbandMoving)
		{
			yield return null;
			t = 0f;
			start = this.husband.transform.position.x;
			if (movingRight)
			{
				end = 640f - moveOffset;
			}
			else
			{
				end = -640f + moveOffset;
			}
			while (t < time && this.husbandMoving)
			{
				while (this.husband.GetCurrentAnimatorStateInfo(0).IsName("OhNo") || this.husband.GetCurrentAnimatorStateInfo(0).IsName("Yay"))
				{
					yield return null;
				}
				float val = t / time;
				this.husband.transform.SetPosition(new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, end, val)), null, null);
				t += CupheadTime.Delta;
				yield return null;
			}
			if (this.husbandMoving)
			{
				this.husband.transform.SetPosition(new float?(end), null, null);
			}
			movingRight = !movingRight;
			yield return null;
		}
		if (SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE)
		{
			t = 0f;
			time = 0.3f;
			start = this.husband.transform.position.x;
			while (t < time)
			{
				t += CupheadTime.Delta;
				this.husband.transform.SetPosition(new float?(Mathf.Lerp(start, 0f, t / time)), null, null);
				yield return null;
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002BFF RID: 11263 RVA: 0x0019BE8C File Offset: 0x0019A28C
	private void PlayYay()
	{
		if (this.husbandMoving && !AudioManager.CheckIfPlaying("sally_bg_church_fiance_yay"))
		{
			AudioManager.Play("sally_bg_church_fiance_yay");
			this.emitAudioFromObject.Add("sally_bg_church_fiance_yay");
			this.husband.Play("Yay");
		}
	}

	// Token: 0x06002C00 RID: 11264 RVA: 0x0019BEE0 File Offset: 0x0019A2E0
	private IEnumerator cupid_check_falling(SallyStagePlayLevelBackgroundHandler.Cupid cupid)
	{
		LevelProperties.SallyStagePlay.General p = this.properties.CurrentState.general;
		for (;;)
		{
			AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
			AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			if (player2 != null && !player2.IsDead)
			{
				if (player.IsDead)
				{
					if (player2.transform.parent != cupid.cupidTransform && cupid.cupidTransform.position.y >= p.cupidDropMaxY)
					{
						if (cupid.cupidTransform.position.y < cupid.startPosition.y)
						{
							cupid.cupidTransform.position += Vector3.up * p.cupidMoveSpeed * CupheadTime.Delta;
						}
						yield return null;
					}
					else if (cupid.cupidTransform.position.y > p.cupidDropMaxY)
					{
						cupid.cupidTransform.position += Vector3.down * p.cupidMoveSpeed * CupheadTime.Delta;
					}
				}
				else if (player.transform.parent != cupid.cupidTransform && player2.transform.parent != cupid.cupidTransform && cupid.cupidTransform.position.y >= p.cupidDropMaxY)
				{
					if (cupid.cupidTransform.position.y < cupid.startPosition.y)
					{
						cupid.cupidTransform.position += Vector3.up * p.cupidMoveSpeed * CupheadTime.Delta;
					}
					yield return null;
				}
				else if (cupid.cupidTransform.position.y > p.cupidDropMaxY)
				{
					cupid.cupidTransform.position += Vector3.down * p.cupidMoveSpeed * CupheadTime.Delta;
				}
			}
			else if (player.transform.parent != cupid.cupidTransform && cupid.cupidTransform.position.y >= p.cupidDropMaxY)
			{
				if (cupid.cupidTransform.position.y < cupid.startPosition.y)
				{
					cupid.cupidTransform.position += Vector3.up * p.cupidMoveSpeed * CupheadTime.Delta;
				}
				yield return null;
			}
			else if (cupid.cupidTransform.position.y > p.cupidDropMaxY)
			{
				cupid.cupidTransform.position += Vector3.down * p.cupidMoveSpeed * CupheadTime.Delta;
			}
			if (cupid.cupidTransform.position.y <= p.cupidDropMaxY)
			{
				if (!cupid.playSound)
				{
					AudioManager.Play("sally_platform_cherub_full_travel");
					this.emitAudioFromObject.Add("sally_platform_cherub_full_travel");
					cupid.playSound = true;
				}
				cupid.acceptableLevel = true;
			}
			else
			{
				cupid.acceptableLevel = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002C01 RID: 11265 RVA: 0x0019BF04 File Offset: 0x0019A304
	private IEnumerator check_bools_cr()
	{
		bool chandelierWarning = false;
		while (!this.cupids[0].acceptableLevel || !this.cupids[1].acceptableLevel)
		{
			if ((this.cupids[0].acceptableLevel || this.cupids[1].acceptableLevel) && !chandelierWarning)
			{
				chandelierWarning = true;
				base.StartCoroutine(this.chandelier_cr(true));
			}
			yield return null;
		}
		base.StartCoroutine(this.chandelier_cr(false));
		SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE = true;
		float clamp = 20f;
		while (this.husband.transform.position.x > clamp || this.husband.transform.position.x < -clamp)
		{
			yield return null;
		}
		this.husbandMoving = false;
		this.husband.SetTrigger("Dead");
		this.properties.DealDamageToNextNamedState();
		yield return CupheadTime.WaitForSeconds(this, 0.75f);
		this.dropChandelier = true;
		yield return null;
		float t = 0f;
		float frameTime = 0f;
		float moveTime = 0.3f;
		Vector3 start = new Vector3(this.chandelierStartPosX, this.chandelier.transform.position.y);
		Vector3 end = new Vector3(this.chandelierStartPosX, this.sallyBackground.transform.position.y - 70f);
		while (t < moveTime)
		{
			frameTime += CupheadTime.Delta;
			t += CupheadTime.Delta;
			if (frameTime > 0.041666668f)
			{
				float t2 = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / moveTime);
				this.chandelier.transform.position = Vector2.Lerp(start, end, t2);
				frameTime -= 0.041666668f;
			}
			yield return null;
		}
		yield return null;
		CupheadLevelCamera.Current.Shake(10f, 0.4f, false);
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x06002C02 RID: 11266 RVA: 0x0019BF20 File Offset: 0x0019A320
	private IEnumerator chandelier_cr(bool isWarning)
	{
		float t = 0f;
		this.chandelier.GetComponent<Animator>().Play("Shake");
		while (!this.dropChandelier)
		{
			t += CupheadTime.Delta;
			if (t > 0.8f && isWarning)
			{
				break;
			}
			yield return null;
		}
		if (isWarning)
		{
			this.chandelier.GetComponent<Animator>().SetTrigger("OnSlump");
			AudioManager.Play("sally_chandelier_warning");
		}
		else
		{
			this.chandelier.GetComponent<Animator>().Play("Off");
			AudioManager.Play("sally_chandelier_impact");
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002C03 RID: 11267 RVA: 0x0019BF42 File Offset: 0x0019A342
	private void OnPhase2()
	{
		if (!SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE)
		{
			base.StartCoroutine(this.just_married_cr());
		}
		else
		{
			base.StartCoroutine(this.crying_cr());
		}
	}

	// Token: 0x06002C04 RID: 11268 RVA: 0x0019BF70 File Offset: 0x0019A370
	public void RollUpCupids()
	{
		foreach (SallyStagePlayLevelBackgroundHandler.Cupid cupid in this.cupids)
		{
			base.StartCoroutine(this.roll_up_cupids_cr(cupid));
		}
	}

	// Token: 0x06002C05 RID: 11269 RVA: 0x0019BFAC File Offset: 0x0019A3AC
	private IEnumerator roll_up_cupids_cr(SallyStagePlayLevelBackgroundHandler.Cupid cupid)
	{
		float t = 0f;
		float frameTime = 0f;
		float moveTime = 3.5f;
		Vector3 end = new Vector3(cupid.cupidTransform.position.x, 800f);
		cupid.cupidTransform.GetComponent<Collider2D>().enabled = false;
		while (t < moveTime)
		{
			frameTime += CupheadTime.Delta;
			t += CupheadTime.Delta;
			if (frameTime > 0.041666668f)
			{
				float t2 = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / moveTime);
				cupid.cupidTransform.position = Vector2.Lerp(cupid.cupidTransform.position, end, t2);
				frameTime -= 0.041666668f;
			}
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002C06 RID: 11270 RVA: 0x0019BFC8 File Offset: 0x0019A3C8
	private IEnumerator just_married_cr()
	{
		float t = 0f;
		float frameTime = 0f;
		float moveTime = 1.5f;
		Vector3 end = new Vector3(this.husband.transform.position.x, this.car.position.y);
		while (this.sally.state != SallyStagePlayLevelSally.State.Transition)
		{
			yield return null;
		}
		this.sally.animator.SetTrigger("OnPhase2");
		this.priest.SetTrigger("CarAppeared");
		this.husbandMoving = false;
		this.husband.SetTrigger("Married");
		yield return this.husband.WaitForAnimationToEnd(this, "Tada_Start", false, true);
		this.sallyBackground.Play("Wave");
		AudioManager.Play("sally_ph1_bg_car_enter");
		this.sallyBackground.transform.parent = this.car.transform;
		this.sallyBackground.transform.position = this.carRoot.transform.position;
		this.sallyBackground.transform.position = this.carRoot.transform.position;
		while (t < moveTime)
		{
			frameTime += CupheadTime.Delta;
			t += CupheadTime.Delta;
			if (frameTime > 0.041666668f)
			{
				float t2 = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / moveTime);
				this.car.transform.position = Vector2.Lerp(this.car.transform.position, end, t2);
				frameTime -= 0.041666668f;
			}
			yield return null;
		}
		AudioManager.PlayLoop("sally_ph1_bg_car_loop");
		this.husband.SetTrigger("Drive");
		this.husband.transform.parent = this.car.transform;
		AudioManager.Play("sally_ph1_bg_car_exit");
		AudioManager.Stop("sally_ph1_bg_car_loop");
		t = 0f;
		frameTime = 0f;
		moveTime = 2f;
		end.x = 1140f;
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		while (t < moveTime)
		{
			frameTime += CupheadTime.Delta;
			t += CupheadTime.Delta;
			if (frameTime > 0.041666668f)
			{
				float t3 = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0f, 1f, t / moveTime);
				this.car.transform.position = Vector2.Lerp(this.car.transform.position, end, t3);
				frameTime -= 0.041666668f;
			}
			yield return null;
		}
		this.CloseCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.House);
		yield return CupheadTime.WaitForSeconds(this, this.curtainWaitTime);
		this.HaltCoroutines();
		this.OpenCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.House);
		this.residence.StartPhase2(this.parent, this.properties);
		this.sally.StartPhase2();
		this.StartPeeking();
		yield return null;
		yield break;
	}

	// Token: 0x06002C07 RID: 11271 RVA: 0x0019BFE4 File Offset: 0x0019A3E4
	private IEnumerator cry_sound_cr()
	{
		yield return this.sallyBackground.WaitForAnimationToEnd(this, "Run", false, true);
		AudioManager.Play("sally_cry");
		this.emitAudioFromObject.Add("sally_cry");
		yield return null;
		yield break;
	}

	// Token: 0x06002C08 RID: 11272 RVA: 0x0019C000 File Offset: 0x0019A400
	private IEnumerator crying_cr()
	{
		while (this.sally.state != SallyStagePlayLevelSally.State.Transition)
		{
			yield return null;
		}
		this.sally.animator.SetTrigger("OnPhase2");
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		this.priest.Play("Tuck_Down_Disappear");
		base.StartCoroutine(this.cry_sound_cr());
		this.sallyBackground.Play("Run");
		yield return this.sallyBackground.WaitForAnimationToEnd(this, "Run_End", false, true);
		this.CloseCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.Nunnery);
		SallyStagePlayHusbandExplosion ex = this.husband.GetComponent<SallyStagePlayHusbandExplosion>();
		if (ex != null)
		{
			ex.StopExplosions();
		}
		yield return CupheadTime.WaitForSeconds(this, this.curtainWaitTime);
		this.HaltCoroutines();
		this.OpenCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.Nunnery);
		this.residence.StartPhase2(this.parent, this.properties);
		this.sally.StartPhase2();
		this.StartPeeking();
		yield return null;
		yield break;
	}

	// Token: 0x06002C09 RID: 11273 RVA: 0x0019C01C File Offset: 0x0019A41C
	private void HaltCoroutines()
	{
		foreach (Coroutine routine in this.phaseDependentCoroutines)
		{
			base.StopCoroutine(routine);
		}
		this.phaseDependentCoroutines.Clear();
	}

	// Token: 0x06002C0A RID: 11274 RVA: 0x0019C084 File Offset: 0x0019A484
	private void StartPeeking()
	{
		if (SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE)
		{
			this.phaseDependentCoroutines.Add(base.StartCoroutine(this.peek_cr(this.priestPhase2, this.priestRoots)));
		}
		else
		{
			this.phaseDependentCoroutines.Add(base.StartCoroutine(this.peek_cr(this.husbandPhase2, this.husbandRoots)));
		}
	}

	// Token: 0x06002C0B RID: 11275 RVA: 0x0019C0E8 File Offset: 0x0019A4E8
	private IEnumerator peek_cr(Animator animator, Transform[] roots)
	{
		float waitTime = UnityEngine.Random.Range(8f, 20f);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, waitTime);
			yield return null;
			int rootChosen = UnityEngine.Random.Range(0, roots.Length);
			animator.GetComponent<Transform>().position = roots[rootChosen].position;
			animator.GetComponent<Transform>().SetScale(new float?(roots[rootChosen].localScale.x), null, null);
			if (SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE && rootChosen == 1)
			{
				animator.SetBool("isDiag", true);
			}
			else
			{
				animator.SetBool("isDiag", Rand.Bool());
			}
			animator.SetTrigger("Peek");
			waitTime = UnityEngine.Random.Range(8f, 20f);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002C0C RID: 11276 RVA: 0x0019C114 File Offset: 0x0019A514
	private void OnPhase3()
	{
		this.HaltCoroutines();
		foreach (Transform swing in this.purgSwingies)
		{
			this.phaseDependentCoroutines.Add(base.StartCoroutine(this.swing_cr(swing)));
		}
		if (SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE)
		{
			this.priestPhase2.Play("Shake");
		}
		else
		{
			this.husbandPhase2.Play("Cry");
		}
		base.StartCoroutine(this.phase3_background_cr());
	}

	// Token: 0x06002C0D RID: 11277 RVA: 0x0019C19C File Offset: 0x0019A59C
	private IEnumerator phase3_background_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 2f);
		this.CloseCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.Purgatory);
		yield return CupheadTime.WaitForSeconds(this, this.curtainWaitTime);
		this.OpenCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.Purgatory);
		yield return null;
		yield break;
	}

	// Token: 0x06002C0E RID: 11278 RVA: 0x0019C1B8 File Offset: 0x0019A5B8
	private void OnPhase4()
	{
		this.HaltCoroutines();
		foreach (Transform swing in this.finaleSwingies)
		{
			this.phaseDependentCoroutines.Add(base.StartCoroutine(this.swing_cr(swing)));
		}
		base.StartCoroutine(this.phase4_background_cr());
		if (SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE)
		{
			this.husbandDeadObject.SetActive(true);
		}
		else
		{
			this.husbandAliveObject.SetActive(true);
		}
	}

	// Token: 0x06002C0F RID: 11279 RVA: 0x0019C238 File Offset: 0x0019A638
	private IEnumerator phase4_background_cr()
	{
		this.CloseCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.Finale);
		yield return CupheadTime.WaitForSeconds(this, this.curtainWaitTime);
		this.OpenCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.Finale);
		yield return null;
		yield break;
	}

	// Token: 0x0400349A RID: 13466
	private const float FRAME_TIME = 0.041666668f;

	// Token: 0x0400349B RID: 13467
	[Header("Main")]
	[SerializeField]
	private SallyStagePlayLevelSally sally;

	// Token: 0x0400349C RID: 13468
	[SerializeField]
	private Transform curtain;

	// Token: 0x0400349D RID: 13469
	[SerializeField]
	private Transform curtainSprite;

	// Token: 0x0400349E RID: 13470
	[SerializeField]
	private Transform curtainShadow;

	// Token: 0x0400349F RID: 13471
	[SerializeField]
	private Transform curtainUpRoot;

	// Token: 0x040034A0 RID: 13472
	[SerializeField]
	private SpriteRenderer[] flickeringLights;

	// Token: 0x040034A1 RID: 13473
	[SerializeField]
	private SallyStagePlayApplauseHandler applauseHandler;

	// Token: 0x040034A2 RID: 13474
	[Header("Church")]
	[SerializeField]
	private Transform[] churchSwingies;

	// Token: 0x040034A3 RID: 13475
	[SerializeField]
	private SallyStagePlayLevelBackgroundHandler.Cupid[] cupids;

	// Token: 0x040034A4 RID: 13476
	[SerializeField]
	private Animator priest;

	// Token: 0x040034A5 RID: 13477
	[SerializeField]
	private Animator husband;

	// Token: 0x040034A6 RID: 13478
	[SerializeField]
	private Animator sallyBackground;

	// Token: 0x040034A7 RID: 13479
	[SerializeField]
	private Transform car;

	// Token: 0x040034A8 RID: 13480
	[SerializeField]
	private Transform carRoot;

	// Token: 0x040034A9 RID: 13481
	[SerializeField]
	private Transform chandelier;

	// Token: 0x040034AA RID: 13482
	[SerializeField]
	private Transform sallyRoot;

	// Token: 0x040034AB RID: 13483
	[Header("Residence")]
	[SerializeField]
	private SallyStagePlayLevelHouse residence;

	// Token: 0x040034AC RID: 13484
	[SerializeField]
	private Animator husbandPhase2;

	// Token: 0x040034AD RID: 13485
	[SerializeField]
	private Transform[] husbandRoots;

	// Token: 0x040034AE RID: 13486
	[SerializeField]
	private Animator priestPhase2;

	// Token: 0x040034AF RID: 13487
	[SerializeField]
	private Transform[] priestRoots;

	// Token: 0x040034B0 RID: 13488
	[Header("Purgatory")]
	[SerializeField]
	private Transform[] purgSwingies;

	// Token: 0x040034B1 RID: 13489
	[Header("Finale")]
	[SerializeField]
	private Transform[] finaleSwingies;

	// Token: 0x040034B2 RID: 13490
	[SerializeField]
	private GameObject husbandAliveObject;

	// Token: 0x040034B3 RID: 13491
	[SerializeField]
	private GameObject husbandDeadObject;

	// Token: 0x040034B4 RID: 13492
	[Header("Backgrounds")]
	[SerializeField]
	private GameObject[] backgrounds;

	// Token: 0x040034B5 RID: 13493
	private float fadeWaitMinSecond = 8f;

	// Token: 0x040034B6 RID: 13494
	private float fadeWaitMaxSecond = 25f;

	// Token: 0x040034B7 RID: 13495
	private float curtainWaitTime = 2.5f;

	// Token: 0x040034B8 RID: 13496
	private float chandelierStartPosX;

	// Token: 0x040034B9 RID: 13497
	private bool husbandMoving = true;

	// Token: 0x040034BA RID: 13498
	private bool dropChandelier;

	// Token: 0x040034BB RID: 13499
	private Vector3 curtainStartPos;

	// Token: 0x040034BC RID: 13500
	private Vector3 curtainShadowStartPos;

	// Token: 0x040034BD RID: 13501
	private LevelProperties.SallyStagePlay properties;

	// Token: 0x040034BE RID: 13502
	private SallyStagePlayLevel parent;

	// Token: 0x040034BF RID: 13503
	private List<Coroutine> phaseDependentCoroutines;

	// Token: 0x020007A7 RID: 1959
	public enum Backgrounds
	{
		// Token: 0x040034C1 RID: 13505
		Church,
		// Token: 0x040034C2 RID: 13506
		House,
		// Token: 0x040034C3 RID: 13507
		Nunnery,
		// Token: 0x040034C4 RID: 13508
		Purgatory,
		// Token: 0x040034C5 RID: 13509
		Finale
	}

	// Token: 0x020007A8 RID: 1960
	[Serializable]
	public class Cupid
	{
		// Token: 0x040034C6 RID: 13510
		public Transform cupidTransform;

		// Token: 0x040034C7 RID: 13511
		public Vector3 startPosition;

		// Token: 0x040034C8 RID: 13512
		public bool acceptableLevel;

		// Token: 0x040034C9 RID: 13513
		public bool playSound;
	}
}
