using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

// Token: 0x02000016 RID: 22
public class AirplaneLevel : Level
{
	// Token: 0x06000021 RID: 33 RVA: 0x00050BE0 File Offset: 0x0004EFE0
	protected override void PartialInit()
	{
		this.properties = LevelProperties.Airplane.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000022 RID: 34 RVA: 0x00050C76 File Offset: 0x0004F076
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Airplane;
		}
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000023 RID: 35 RVA: 0x00050C7D File Offset: 0x0004F07D
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_airplane;
		}
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000024 RID: 36 RVA: 0x00050C84 File Offset: 0x0004F084
	public override Sprite BossPortrait
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Airplane.States.Main:
			case LevelProperties.Airplane.States.Rocket:
				return this._bossPortraitMain;
			case LevelProperties.Airplane.States.Terriers:
				return this._bossPortraitPhaseTwo;
			case LevelProperties.Airplane.States.Leader:
				return (!this.secretPhaseActivated) ? this._bossPortraitPhaseThree : this._bossPortraitPhaseSecret;
			}
			global::Debug.LogError("Couldn't find portrait for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
			return this._bossPortraitMain;
		}
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000025 RID: 37 RVA: 0x00050D1C File Offset: 0x0004F11C
	public override string BossQuote
	{
		get
		{
			switch (this.properties.CurrentState.stateName)
			{
			case LevelProperties.Airplane.States.Main:
			case LevelProperties.Airplane.States.Rocket:
				return this._bossQuoteMain;
			case LevelProperties.Airplane.States.Terriers:
				return this._bossQuotePhaseTwo;
			case LevelProperties.Airplane.States.Leader:
				return this._bossQuotePhaseThree;
			}
			global::Debug.LogError("Couldn't find quote for state " + this.properties.CurrentState.stateName + ". Using Main.", null);
			return this._bossQuoteMain;
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000026 RID: 38 RVA: 0x00050D9E File Offset: 0x0004F19E
	// (set) Token: 0x06000027 RID: 39 RVA: 0x00050DA6 File Offset: 0x0004F1A6
	public bool Rotating { get; private set; }

	// Token: 0x06000028 RID: 40 RVA: 0x00050DB0 File Offset: 0x0004F1B0
	protected override void Start()
	{
		base.CameraRotates = true;
		this.Rotating = false;
		base.Start();
		this.airplane.LevelInit(this.properties);
		this.bulldogPlane.LevelInit(this.properties);
		this.bulldogParachute.LevelInit(this.properties);
		this.bulldogCatAttack.LevelInit(this.properties);
		this.canteenAnimator.LevelInit(this.properties);
		this.secretLeader.gameObject.SetActive(false);
		this.airplane.SetXRange(-480f, 480f);
		this.leader.LevelInit(this.properties);
		this.leader.gameObject.SetActive(false);
		base.Invoke("StartGetShadowRenderers", 0.1f);
		PlayerManager.OnPlayerJoinedEvent += this.GetShadowableRenderers;
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00050E8F File Offset: 0x0004F28F
	protected override void OnDestroy()
	{
		PlayerManager.OnPlayerJoinedEvent -= this.GetShadowableRenderers;
		base.OnDestroy();
		this._bossPortraitMain = null;
		this._bossPortraitPhaseTwo = null;
		this._bossPortraitPhaseThree = null;
		this._bossPortraitPhaseSecret = null;
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00050ECC File Offset: 0x0004F2CC
	private void StartGetShadowRenderers()
	{
		this.GetShadowableRenderers(PlayerId.PlayerOne);
		this.GetShadowableRenderers(PlayerId.PlayerTwo);
		foreach (SpriteRenderer spriteRenderer in this.airplane.GetComponentsInChildren<SpriteRenderer>())
		{
			if (spriteRenderer.name != "PaladinShadow0" && spriteRenderer.name != "PaladinShadow1")
			{
				this.shadowableRenderers.Add(spriteRenderer);
			}
		}
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00050F44 File Offset: 0x0004F344
	private void GetShadowableRenderers(PlayerId playerId)
	{
		AbstractPlayerController player = PlayerManager.GetPlayer(playerId);
		if (player)
		{
			foreach (SpriteRenderer spriteRenderer in player.GetComponentsInChildren<SpriteRenderer>())
			{
				if (spriteRenderer.name != "PaladinShadow0" && spriteRenderer.name != "PaladinShadow1")
				{
					this.shadowableRenderers.Add(spriteRenderer);
				}
			}
		}
		this.UpdateShadow(this.currentShadowLevel);
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00050FC8 File Offset: 0x0004F3C8
	public void UpdateShadow(float shadowValue)
	{
		this.currentShadowLevel = shadowValue;
		this.shadowableRenderers.Remove(null);
		foreach (SpriteRenderer spriteRenderer in this.shadowableRenderers)
		{
			if (spriteRenderer != null)
			{
				spriteRenderer.color = new Color(shadowValue, shadowValue, shadowValue);
			}
		}
	}

	// Token: 0x0600002D RID: 45 RVA: 0x0005104C File Offset: 0x0004F44C
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
		if (this.properties.CurrentState.stateName == LevelProperties.Airplane.States.Rocket)
		{
			this.bulldogPlane.StartRocket();
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.Airplane.States.Terriers)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.terrier_cr());
			this.canteenAnimator.triggerCheer = true;
		}
		else if (this.properties.CurrentState.stateName == LevelProperties.Airplane.States.Leader && !this.secretPhaseActivated)
		{
			AudioManager.FadeSFXVolume("sfx_dlc_dogfight_p2_terrierjetpack_loop", 0f, 0.5f);
			this.canteenAnimator.triggerCheer = true;
			this.StartPhase3();
		}
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00051108 File Offset: 0x0004F508
	private void StartPhase3()
	{
		for (int i = 0; i < this.smokeFXPool.Count; i++)
		{
			this.smokeFXPool[i].dead = true;
			if (!this.smokeFXPool[i].inUse)
			{
				UnityEngine.Object.Destroy(this.smokeFXPool[i].gameObject);
			}
		}
		this.StopAllCoroutines();
		base.StartCoroutine(this.leader_cr());
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00051184 File Offset: 0x0004F584
	public Vector3 CurrentEnemyPos()
	{
		switch (this.properties.CurrentState.stateName)
		{
		case LevelProperties.Airplane.States.Main:
		case LevelProperties.Airplane.States.Generic:
		case LevelProperties.Airplane.States.Rocket:
			return this.bulldogPlane.transform.position;
		case LevelProperties.Airplane.States.Terriers:
		{
			int index = UnityEngine.Random.Range(0, this.terriers.Count);
			if (this.terriers[index] != null)
			{
				return this.terriers[index].transform.position;
			}
			return Vector3.zero;
		}
		case LevelProperties.Airplane.States.Leader:
			return this.leader.transform.position;
		default:
			return Vector3.zero;
		}
	}

	// Token: 0x06000030 RID: 48 RVA: 0x0005122E File Offset: 0x0004F62E
	public bool ScreenHorizontal()
	{
		return this.leader.camRotatedHorizontally;
	}

	// Token: 0x06000031 RID: 49 RVA: 0x0005123C File Offset: 0x0004F63C
	private IEnumerator terrier_cr()
	{
		this.bulldogPlane.OnStageChange();
		while (this.bulldogPlane.state != AirplaneLevelBulldogPlane.State.Main)
		{
			yield return null;
		}
		this.bulldogPlane.BulldogDeath();
		while (!this.bulldogPlane.startPhaseTwo)
		{
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		base.StartCoroutine(this.handle_terriers_cr());
		yield break;
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00051258 File Offset: 0x0004F658
	private IEnumerator handle_terriers_cr()
	{
		LevelProperties.Airplane.Terriers p = this.properties.CurrentState.terriers;
		PatternString shotOrder = new PatternString(p.shotOrder, true, true);
		PatternString delayString = new PatternString(p.shotDelayString, true, true);
		PatternString typeString = new PatternString(p.shotTypeString, true, true);
		this.decreaseAmount = 0f;
		Vector3 start = this.airplane.transform.position;
		Vector3 end = new Vector3(0f, this.properties.CurrentState.plane.moveDown);
		this.airplane.AutoMoveToPos(new Vector3(0f, this.properties.CurrentState.plane.moveDown), true, true);
		this.canteenAnimator.ForceLook(new Vector3(0f, this.properties.CurrentState.plane.moveDown), 5);
		yield return CupheadTime.WaitForSeconds(this, 2f);
		this.terriers = new List<AirplaneLevelTerrier>(4);
		float startHealthPercentage = this.properties.CurrentState.healthTrigger;
		float endHealthPercentage = this.properties.GetNextStateHealthTrigger();
		float endHealth = endHealthPercentage * this.properties.TotalHealth;
		float startHealth = startHealthPercentage * this.properties.TotalHealth;
		this.terrierHPTotal = startHealth - endHealth;
		float terrierHP = this.terrierHPTotal / 4f;
		bool isClockwise = Rand.Bool();
		for (int i = 0; i < 4; i++)
		{
			this.terriers.Add(UnityEngine.Object.Instantiate<AirplaneLevelTerrier>(this.terrierPrefab));
			this.terriers[i].Init(this.terrierPivotPoint, (float)(90 * i), this.properties.CurrentState.terriers, terrierHP, AirplaneLevel.TERRIER_PIVOT_OFFSET_X[i], AirplaneLevel.TERRIER_PIVOT_OFFSET_Y[i], isClockwise, i);
		}
		AudioManager.PlayLoop("sfx_dlc_dogfight_p2_terrierjetpack_loop");
		yield return null;
		AudioManager.FadeSFXVolume("sfx_dlc_dogfight_p2_terrierjetpack_loop", 0.3f, 3f);
		bool readyToMove = true;
		while (readyToMove)
		{
			readyToMove = true;
			foreach (AirplaneLevelTerrier airplaneLevelTerrier in this.terriers)
			{
				if (airplaneLevelTerrier.ReadyToMove)
				{
					readyToMove = false;
					break;
				}
			}
			yield return null;
		}
		foreach (AirplaneLevelTerrier airplaneLevelTerrier2 in this.terriers)
		{
			airplaneLevelTerrier2.StartMoving();
		}
		base.StartCoroutine(this.move_pivot_cr());
		Coroutine CheckTerriersCR = base.StartCoroutine(this.check_terriers_cr(terrierHP));
		float delay = 0f;
		bool isWow = false;
		while (!this.AllTerriersSmoking())
		{
			bool shotRound = false;
			bool isPink = typeString.PopLetter() == 'P';
			delay = delayString.PopFloat();
			yield return CupheadTime.WaitForSeconds(this, delay - this.decreaseAmount);
			while (!shotRound)
			{
				int shot = shotOrder.PopInt();
				if (this.terriers[shot] != null && !this.terriers[shot].IsDead)
				{
					float num;
					if (PlayerManager.BothPlayersActive())
					{
						num = Mathf.Min(Vector3.Distance(this.terriers[shot].GetPredictedAttackPos(), PlayerManager.GetPlayer(PlayerId.PlayerOne).transform.position), Vector3.Distance(this.terriers[shot].GetPredictedAttackPos(), PlayerManager.GetPlayer(PlayerId.PlayerTwo).transform.position));
					}
					else
					{
						num = Vector3.Distance(this.terriers[shot].GetPredictedAttackPos(), PlayerManager.GetNext().transform.position);
					}
					if (num > p.minAttackDistance)
					{
						this.terriers[shot].StartAttack(isPink, isWow);
						isWow = !isWow;
						shotRound = true;
					}
				}
				yield return null;
			}
		}
		if (this.AllTerriersSmoking())
		{
			this.secretPhaseActivated = true;
			base.StartCoroutine(this.handle_secret_intro_cr(isClockwise, terrierHP));
		}
		yield break;
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00051274 File Offset: 0x0004F674
	private IEnumerator check_terriers_cr(float terrierHP)
	{
		bool allDead = false;
		bool[] deadOnes = new bool[this.terriers.Count];
		for (int j = 0; j < deadOnes.Length; j++)
		{
			deadOnes[j] = false;
		}
		while (!allDead)
		{
			allDead = true;
			int count = 0;
			for (int i = 0; i < this.terriers.Count; i++)
			{
				if (this.terriers[i] == null || this.terriers[i].IsDead)
				{
					count++;
					if (!deadOnes[i])
					{
						this.properties.DealDamage(terrierHP);
						this.decreaseAmount += this.properties.CurrentState.terriers.shotMinus;
						deadOnes[i] = true;
					}
					yield return null;
				}
				else
				{
					allDead = false;
				}
				if (this.terriers[i].lastOne)
				{
					count = 0;
				}
			}
			if (count == 3)
			{
				for (int k = 0; k < this.terriers.Count; k++)
				{
					if (this.terriers[k] != null && !this.terriers[k].IsDead)
					{
						this.terriers[k].lastOne = true;
					}
				}
			}
			yield return null;
		}
		if (this.properties.CurrentState.stateName == LevelProperties.Airplane.States.Terriers)
		{
			this.properties.DealDamageToNextNamedState();
		}
		yield break;
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00051298 File Offset: 0x0004F698
	public bool AllTerriersSmoking()
	{
		if (this.secretPhaseActivated)
		{
			return true;
		}
		bool result = true;
		for (int i = 0; i < this.terriers.Count; i++)
		{
			if (this.terriers[i] == null || this.terriers[i].IsDead || !this.terriers[i].IsSmoking())
			{
				result = false;
			}
		}
		return result;
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00051318 File Offset: 0x0004F718
	public void CreateSmokeFX(Vector3 pos, Vector3 vel, bool isSmoking, int sortingLayerID, int sortingOrder)
	{
		AirplaneLevelTerrierSmokeFX airplaneLevelTerrierSmokeFX = null;
		for (int i = 0; i < this.smokeFXPool.Count; i++)
		{
			if (!this.smokeFXPool[i].inUse)
			{
				airplaneLevelTerrierSmokeFX = this.smokeFXPool[i];
				break;
			}
		}
		if (airplaneLevelTerrierSmokeFX == null)
		{
			airplaneLevelTerrierSmokeFX = (AirplaneLevelTerrierSmokeFX)this.smokePrefab.Create(pos);
			this.smokeFXPool.Add(airplaneLevelTerrierSmokeFX);
		}
		airplaneLevelTerrierSmokeFX.animator.SetInteger("Effect", UnityEngine.Random.Range(0, 3));
		airplaneLevelTerrierSmokeFX.animator.Play((!isSmoking) ? "A" : "AGray", 0, 0f);
		airplaneLevelTerrierSmokeFX.rend.sortingLayerID = sortingLayerID;
		airplaneLevelTerrierSmokeFX.rend.sortingOrder = sortingOrder;
		airplaneLevelTerrierSmokeFX.rend.enabled = true;
		airplaneLevelTerrierSmokeFX.inUse = true;
		airplaneLevelTerrierSmokeFX.vel = vel;
		airplaneLevelTerrierSmokeFX.myTransform = airplaneLevelTerrierSmokeFX.transform;
		airplaneLevelTerrierSmokeFX.myTransform.position = pos;
	}

	// Token: 0x06000036 RID: 54 RVA: 0x0005141E File Offset: 0x0004F81E
	private void HandleTimelineHP(float terrierHP)
	{
		base.timeline.DealDamage(terrierHP);
	}

	// Token: 0x06000037 RID: 55 RVA: 0x0005142C File Offset: 0x0004F82C
	private IEnumerator move_pivot_cr()
	{
		float t = 1.5f;
		float val = 1f;
		bool reversed = Rand.Bool();
		float start = this.terrierPivotPoint.position.x + 30f;
		float end = this.terrierPivotPoint.position.x - 30f;
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			while (t < 3f)
			{
				t += CupheadTime.FixedDelta;
				if (reversed)
				{
					this.terrierPivotPoint.transform.SetPosition(new float?(Mathf.Lerp(start, end, val - t / 3f)), null, null);
				}
				else
				{
					this.terrierPivotPoint.transform.SetPosition(new float?(Mathf.Lerp(start, end, t / 3f)), null, null);
				}
				yield return wait;
			}
			t = 0f;
			reversed = !reversed;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00051448 File Offset: 0x0004F848
	private IEnumerator leader_cr()
	{
		LevelProperties.Airplane.Plane p = this.properties.CurrentState.plane;
		if (this.terriers == null)
		{
			while (this.bulldogPlane.state != AirplaneLevelBulldogPlane.State.Main)
			{
				yield return null;
			}
			this.bulldogPlane.BulldogDeath();
		}
		if (!this.secretPhaseActivated)
		{
			yield return CupheadTime.WaitForSeconds(this, 0.5f);
		}
		this.leader.gameObject.SetActive(true);
		this.leader.StartLeader();
		if (this.secretPhaseActivated)
		{
			this.leader.animator.Play("Intro", 0, 0.54f);
			yield return null;
		}
		else
		{
			this.airplane.AutoMoveToPos(new Vector3(0f, p.moveDownPhThree), true, true);
			this.canteenAnimator.ForceLook(new Vector3(0f, p.moveDownPhThree), 3);
			this.airplane.SetXRange(-225f, 225f);
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		int target = Animator.StringToHash(this.leader.animator.GetLayerName(0) + ".Intro");
		while (this.leader.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == target)
		{
			if (this.leader.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.75f)
			{
				((AirplaneLevel)Level.Current).UpdateShadow(1f - Mathf.InverseLerp(0.75f, 1f, this.leader.animator.GetCurrentAnimatorStateInfo(0).normalizedTime) * 0.1f);
			}
			yield return null;
		}
		if (!this.secretPhaseActivated)
		{
			base.StartCoroutine(this.rotate_camera());
		}
		else
		{
			base.StartCoroutine(this.secret_phase_cr());
		}
		yield break;
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00051463 File Offset: 0x0004F863
	public void MoveBoundsIn()
	{
		base.StartCoroutine(this.move_bounds_for_phase_three_cr());
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00051474 File Offset: 0x0004F874
	private IEnumerator move_bounds_for_phase_three_cr()
	{
		float t = 0f;
		float time = 0.2f;
		float boundsStart = (float)this.bounds.left;
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
			{
				if (abstractPlayerController)
				{
					abstractPlayerController.transform.position = new Vector3(Mathf.Clamp(abstractPlayerController.transform.position.x, Mathf.Lerp(-boundsStart, -465f, t / time), Mathf.Lerp(boundsStart, 465f, t / time)), abstractPlayerController.transform.position.y);
					if (abstractPlayerController.stats.State == PlayerStatsManager.PlayerState.Super)
					{
						LevelPlayerWeaponManager component = abstractPlayerController.GetComponent<LevelPlayerWeaponManager>();
						if (component.activeSuper != null)
						{
							component.activeSuper.transform.position = new Vector3(Mathf.Clamp(component.transform.position.x, Mathf.Lerp(-boundsStart, -465f, t / time), Mathf.Lerp(boundsStart, 465f, t / time)), component.transform.position.y);
						}
					}
				}
			}
			yield return new WaitForFixedUpdate();
		}
		this.bounds.left = 465;
		this.bounds.right = 465;
		yield break;
	}

	// Token: 0x0600003B RID: 59 RVA: 0x0005148F File Offset: 0x0004F88F
	public void BlurBGCamera()
	{
		base.StartCoroutine(this.bg_camera_blur_cr());
	}

	// Token: 0x0600003C RID: 60 RVA: 0x000514A0 File Offset: 0x0004F8A0
	private IEnumerator bg_camera_blur_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		while (this.bgBlur.blurSize < 3f)
		{
			this.bgBlur.blurSize += CupheadTime.FixedDelta * 10f;
			yield return wait;
		}
		this.bgBlur.blurSize = 3f;
		yield break;
	}

	// Token: 0x0600003D RID: 61 RVA: 0x000514BC File Offset: 0x0004F8BC
	private IEnumerator rotate_camera()
	{
		LevelProperties.Airplane.Leader p = this.properties.CurrentState.leader;
		Vector3 airplanePhase3Pos = new Vector3(0f, this.properties.CurrentState.plane.moveDownPhThree);
		float startAngle = 0f;
		float endAngle = 360f;
		while (this.properties.CurrentState.stateName == LevelProperties.Airplane.States.Leader)
		{
			yield return CupheadTime.WaitForSeconds(this, p.attackDelay);
			if (this.leader.camRotatedHorizontally)
			{
				AudioManager.PlayLoop("sfx_dlc_dogfight_p3_dogcopter_close_loop");
				AudioManager.FadeSFXVolume("sfx_dlc_dogfight_p3_dogcopter_close_loop", 0.7f, 0.5f);
				this.leader.StartDropshot();
			}
			else
			{
				this.leader.StartLaser();
			}
			while (this.leader.IsAttacking)
			{
				yield return null;
			}
			if (Level.Current.mode != Level.Mode.Easy)
			{
				if (endAngle == 0f)
				{
					startAngle = 360f;
				}
				else
				{
					startAngle = endAngle;
				}
				endAngle = startAngle - 90f;
				this.leader.RotateCamera();
				if (this.leader.camRotatedHorizontally)
				{
					this.pitTop.gameObject.SetActive(false);
				}
				this.leaderAnimator.SetTrigger("Continue");
				this.Rotating = true;
				string animation = "Rotate_Start_" + ((!this.leader.camRotatedHorizontally) ? "ToVer" : "ToHor");
				yield return this.leaderAnimator.WaitForAnimationToEnd(this, animation, false, true);
				this.SFX_DOGFIGHT_PlayerPlane_PositionChangeFlyby();
				float rotateTime = this.rotateClip.length;
				string animName = (!this.leader.camRotatedHorizontally) ? "Rotate_Mid_ToVer" : "Rotate_Mid_ToHor";
				float start = this.airplane.transform.position.y;
				float end = (!this.leader.camRotatedHorizontally) ? airplanePhase3Pos.y : this.properties.CurrentState.plane.moveWhenRotate;
				this.SFX_DOGFIGHT_P3_Dogcopter_ScreenRotateEndClunk();
				while (this.leaderAnimator.GetCurrentAnimatorStateInfo(0).IsName(animName))
				{
					float easePos = (!this.leader.camRotatedHorizontally) ? Mathf.Clamp(EaseUtils.EaseInOutSine(-0.1f, 1f, this.leaderAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime), 0f, 1f) : EaseUtils.EaseOutSine(0f, 1f, this.leaderAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
					float rotationAmount = startAngle + Mathf.Lerp(0f, endAngle - startAngle, easePos);
					this.airplane.transform.SetPosition(null, new float?(Mathf.Lerp(start, end, this.leaderAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime)), null);
					CupheadLevelCamera.Current.SetRotation(rotationAmount);
					this.bgCamera.transform.SetEulerAngles(null, null, new float?(rotationAmount));
					if (startAngle == 180f || startAngle == 90f)
					{
						this.leaderAnimator.transform.SetEulerAngles(null, null, new float?(180f + rotationAmount));
					}
					else
					{
						this.leaderAnimator.transform.SetEulerAngles(null, null, new float?(rotationAmount));
					}
					yield return null;
				}
				if (!this.leader.camRotatedHorizontally)
				{
					CupheadLevelCamera.Current.Shake(20f, 0.5f, false);
					this.pitTop.gameObject.SetActive(true);
				}
				else
				{
					CupheadLevelCamera.Current.Shake(30f, 0.7f, false);
				}
				CupheadLevelCamera.Current.SetRotation(endAngle);
				this.bgCamera.transform.SetEulerAngles(null, null, new float?(endAngle));
				if (this.leader.camRotatedHorizontally)
				{
					this.leaderAnimator.transform.SetEulerAngles(null, null, new float?((startAngle != 180f) ? endAngle : (-endAngle)));
				}
				else
				{
					this.leaderAnimator.transform.SetEulerAngles(null, null, new float?(180f));
					AudioManager.FadeSFXVolume("sfx_dlc_dogfight_p3_dogcopter_close_loop", 0f, 0.5f);
				}
				yield return this.leaderAnimator.WaitForAnimationToEnd(this, (!this.leader.camRotatedHorizontally) ? "Rotate_End_ToVer" : "Rotate_End_ToHor", false, true);
				this.leaderAnimator.SetBool("CloseUp", this.leader.camRotatedHorizontally);
				this.leaderAnimator.Update(0f);
				this.Rotating = false;
			}
		}
		yield break;
	}

	// Token: 0x0600003E RID: 62 RVA: 0x000514D8 File Offset: 0x0004F8D8
	private void LateUpdate()
	{
		if (!this.Rotating)
		{
			return;
		}
		if (this.leaderAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
		{
			this.leaderAnimator.transform.SetEulerAngles(null, null, new float?(0f));
		}
		if (this.leaderAnimator.GetCurrentAnimatorStateInfo(0).IsName("Rotate_End_ToHor"))
		{
			this.leaderAnimator.Play("Push_Wait", 3, 0f);
			this.leaderAnimator.Update(0f);
		}
		if (this.leaderAnimator.GetCurrentAnimatorStateInfo(0).IsName("Rotate_Mid_ToVer"))
		{
			this.leaderAnimator.Play("None", 3, 0f);
			this.leaderAnimator.Update(0f);
		}
		if (this.properties.CurrentState.stateName == LevelProperties.Airplane.States.Terriers && !this.terriersIntroFinished)
		{
			float deltaTime = Time.deltaTime;
			for (int i = 0; i < this.smokeFXPool.Count; i++)
			{
				if (this.smokeFXPool[i] != null && this.smokeFXPool[i].inUse)
				{
					this.smokeFXPool[i].Step(deltaTime);
				}
			}
		}
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00051650 File Offset: 0x0004FA50
	private IEnumerator handle_secret_intro_cr(bool clockwise, float terrierHP)
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		this.canteenAnimator.triggerCheer = true;
		for (int i = 0; i < this.terriers.Count; i++)
		{
			this.terriers[i].StartSecret();
		}
		Vector3 start = this.airplane.transform.position;
		Vector3 end = new Vector3(325f * (float)((!clockwise) ? -1 : 1), this.airplane.transform.position.y);
		float time = Mathf.Max(1f, Vector3.Distance(start, end) / 200f);
		if (clockwise)
		{
			this.airplane.SetXRange(250f, 480f);
		}
		else
		{
			this.airplane.SetXRange(-480f, -250f);
		}
		this.canteenAnimator.ForceLook((!clockwise) ? (Vector3.right * 1000f) : (Vector3.left * 1000f), 9);
		this.airplane.AutoMoveToPos(end, true, true);
		yield return CupheadTime.WaitForSeconds(this, time / 2f);
		if (clockwise)
		{
			this.secretIntro.transform.localScale = new Vector3(-1f, 1f);
		}
		yield return base.StartCoroutine(this.wait_for_terrier_to_reach_chomp_position_cr());
		this.secretIntro.transform.position = new Vector3(-780f * (float)((!clockwise) ? -1 : 1), this.secretIntro.transform.position.y);
		this.secretIntro.gameObject.SetActive(true);
		this.secretIntro.Play("Chomp", 0, 0f);
		AudioManager.Play("sfx_dlc_dogfight_ps_dogcopterintro_chompstart");
		base.StartCoroutine(this.handle_secret_intro_move_in_cr(clockwise));
		int hash = Animator.StringToHash(this.secretIntro.GetLayerName(0) + ".Chomp");
		while (this.terriers.Count > 0)
		{
			while (this.secretIntro.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f < 0.55f && this.secretIntro.GetCurrentAnimatorStateInfo(0).fullPathHash == hash)
			{
				yield return wait;
			}
			UnityEngine.Object.Destroy(this.terriers[this.secretTargetTerrier].gameObject);
			this.terriers.RemoveAt(this.secretTargetTerrier);
			if (this.terriers.Count > 0)
			{
				yield return base.StartCoroutine(this.wait_for_terrier_to_reach_chomp_position_cr());
				this.secretIntro.Play("Chomp", 0, 0f);
				this.secretIntro.Update(0f);
			}
			if (this.terriers.Count == 1)
			{
				this.secretIntro.SetTrigger("Exit");
				AudioManager.FadeSFXVolume("sfx_dlc_dogfight_p2_terrierjetpack_loop", 0f, 0.5f);
			}
		}
		yield return this.secretIntro.WaitForAnimationToStart(this, "Exit", false);
		AudioManager.Play("sfx_dlc_dogfight_ps_dogcopterintro_chomplicklip");
		while (this.secretIntro.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5f)
		{
			yield return wait;
		}
		AudioManager.Play("sfx_dlc_dogfight_ps_dogcopterintro_leaderintro");
		start = this.airplane.transform.position;
		end = new Vector3(0f, this.properties.CurrentState.plane.moveDownPhThree);
		this.airplane.AutoMoveToPos(end, true, true);
		this.canteenAnimator.ForceLook(end, 3);
		this.airplane.SetXRange(-225f, 225f);
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield return this.secretIntro.WaitForAnimationToEnd(this, "Exit", false, false);
		this.secretIntro.gameObject.SetActive(false);
		yield return CupheadTime.WaitForSeconds(this, 0.3f);
		this.properties.DealDamage(terrierHP * (float)this.terriers.Count);
		this.StartPhase3();
		yield break;
	}

	// Token: 0x06000040 RID: 64 RVA: 0x0005167C File Offset: 0x0004FA7C
	private IEnumerator wait_for_terrier_to_reach_chomp_position_cr()
	{
		bool foundNext = false;
		while (!foundNext)
		{
			for (int i = 0; i < this.terriers.Count; i++)
			{
				if (this.terriers[i].RelativeAngle() > 2.8415928f && this.terriers[i].RelativeAngle() < 3.2415926f)
				{
					foundNext = true;
					this.secretTargetTerrier = i;
					this.terriers[this.secretTargetTerrier].PrepareForChomp();
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00051698 File Offset: 0x0004FA98
	private IEnumerator handle_secret_intro_move_in_cr(bool clockwise)
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		yield return wait;
		float t = 0f;
		while (t < 1f)
		{
			this.secretIntro.transform.position = new Vector3(EaseUtils.EaseOutSine(-780f * (float)((!clockwise) ? -1 : 1), -35f * (float)((!clockwise) ? -1 : 1), t), this.secretIntro.transform.position.y);
			if (t <= this.secretIntro.GetCurrentAnimatorStateInfo(0).normalizedTime)
			{
				t = this.secretIntro.GetCurrentAnimatorStateInfo(0).normalizedTime;
			}
			else
			{
				t = 1f;
			}
			yield return wait;
		}
		this.secretIntro.transform.position = new Vector3(-35f * (float)((!clockwise) ? -1 : 1), this.secretIntro.transform.position.y);
		yield break;
	}

	// Token: 0x06000042 RID: 66 RVA: 0x000516BC File Offset: 0x0004FABC
	public int GetNextHole()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.secretPhaseHoleOccupied.Length; i++)
		{
			if (!this.secretPhaseHoleOccupied[i])
			{
				list.Add(i);
			}
		}
		if (list.Count == 0)
		{
			return -1;
		}
		int num = list[UnityEngine.Random.Range(0, list.Count)];
		this.secretPhaseHoleOccupied[num] = true;
		return num;
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00051726 File Offset: 0x0004FB26
	public Vector3 GetHolePosition(int value, bool isLeader)
	{
		return (!isLeader) ? this.secretPhaseTerrierPositions[value].position : this.secretPhaseLeaderPositions[value].position;
	}

	// Token: 0x06000044 RID: 68 RVA: 0x0005174D File Offset: 0x0004FB4D
	public Vector3 GetLeaderDeathPosition(int value)
	{
		return this.leaderDeathPositions[value].position;
	}

	// Token: 0x06000045 RID: 69 RVA: 0x0005175C File Offset: 0x0004FB5C
	public void LeaveHole(int value)
	{
		this.secretPhaseHoleOccupied[value] = false;
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00051767 File Offset: 0x0004FB67
	public void OccupyHole(int value)
	{
		this.secretPhaseHoleOccupied[value] = true;
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00051774 File Offset: 0x0004FB74
	private IEnumerator secret_phase_cr()
	{
		this.leader.OpenPawHoles();
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		this.secretPhaseHoleOccupied = new bool[this.secretPhaseTerrierPositions.Length];
		this.secretLeader.gameObject.SetActive(true);
		this.secretLeader.LevelInit(this.properties);
		for (int i = 0; i < 4; i++)
		{
			this.secretTerriers[i].gameObject.SetActive(true);
			this.secretTerriers[i].LevelInit(this.properties);
		}
		PatternString dogAttackDelayString = new PatternString(this.properties.CurrentState.secretTerriers.dogAttackDelayString, true, true);
		PatternString dogAttackOrderString = new PatternString(this.properties.CurrentState.secretTerriers.dogAttackOrderString, true, true);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, dogAttackDelayString.PopFloat());
			int nextAttacker = 0;
			nextAttacker = dogAttackOrderString.PopInt();
			this.secretTerriers[nextAttacker].TryStartAttack();
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000048 RID: 72 RVA: 0x0005178F File Offset: 0x0004FB8F
	public void LeaderDeath()
	{
		this.secretLeader.gameObject.SetActive(true);
		this.secretLeader.DieMain();
	}

	// Token: 0x06000049 RID: 73 RVA: 0x000517AD File Offset: 0x0004FBAD
	private void SFX_DOGFIGHT_PlayerPlane_PositionChangeFlyby()
	{
		AudioManager.Play("sfx_DLC_Dogfight_PlayerPlane_PositionChangeFlyby");
	}

	// Token: 0x0600004A RID: 74 RVA: 0x000517B9 File Offset: 0x0004FBB9
	private void SFX_DOGFIGHT_P3_Dogcopter_ScreenRotateEndClunk()
	{
		AudioManager.Play("sfx_DLC_Dogfight_P3_DogCopter_ScreenRotateEndClunk");
	}

	// Token: 0x0600004B RID: 75 RVA: 0x000517C8 File Offset: 0x0004FBC8
	private void WORKAROUND_NullifyFields()
	{
		this.leader = null;
		this.airplane = null;
		this.canteenAnimator = null;
		this.bulldogPlane = null;
		this.bulldogParachute = null;
		this.bulldogCatAttack = null;
		this.secretIntro = null;
		this.leaderAnimator = null;
		this.secretLeader = null;
		this.secretTerriers = null;
		this.rotateClip = null;
		this.pitTop = null;
		this.terrierPivotPoint = null;
		this.secretPhaseTerrierPositions = null;
		this.secretPhaseLeaderPositions = null;
		this.leaderDeathPositions = null;
		this.secretPhaseHoleOccupied = null;
		this.bgBlur = null;
		this.bgCamera = null;
		this.terrierPrefab = null;
		this._bossPortraitMain = null;
		this._bossPortraitPhaseTwo = null;
		this._bossPortraitPhaseThree = null;
		this._bossPortraitPhaseSecret = null;
		this._bossQuoteMain = null;
		this._bossQuotePhaseTwo = null;
		this._bossQuotePhaseThree = null;
		this.terriers = null;
		this.smokeFXPool = null;
		this.smokePrefab = null;
		this.shadowableRenderers = null;
	}

	// Token: 0x040000AA RID: 170
	private LevelProperties.Airplane properties;

	// Token: 0x040000AB RID: 171
	private const float PHASE_TWO_DELAY = 1f;

	// Token: 0x040000AC RID: 172
	private const float PHASE_THREE_DELAY = 0.5f;

	// Token: 0x040000AD RID: 173
	private const float SECRET_INTRO_REAPPEAR_DELAY = 0.3f;

	// Token: 0x040000AE RID: 174
	private const float PHASE_THREE_BOUNDS = 465f;

	// Token: 0x040000AF RID: 175
	private const float PLANE_MAX_PHASE_ONE = 480f;

	// Token: 0x040000B0 RID: 176
	private const float PLANE_MAX_PHASE_THREE = 225f;

	// Token: 0x040000B1 RID: 177
	private const int TERRIERCOUNT = 4;

	// Token: 0x040000B2 RID: 178
	private static readonly float[] TERRIER_PIVOT_OFFSET_X = new float[]
	{
		20f,
		-20f,
		20f,
		-20f
	};

	// Token: 0x040000B3 RID: 179
	private static readonly float[] TERRIER_PIVOT_OFFSET_Y = new float[]
	{
		20f,
		-20f,
		-20f,
		20f
	};

	// Token: 0x040000B4 RID: 180
	private const float PIVOT_MOVE = 30f;

	// Token: 0x040000B5 RID: 181
	private const float MOVE_TIME = 3f;

	// Token: 0x040000B6 RID: 182
	private const float SECRET_INTRO_X_START = -780f;

	// Token: 0x040000B7 RID: 183
	private const float SECRET_INTRO_X_END = -35f;

	// Token: 0x040000B8 RID: 184
	private const float SECRET_INTRO_PLANE_POS = 325f;

	// Token: 0x040000B9 RID: 185
	[SerializeField]
	private AirplaneLevelPlayerPlane airplane;

	// Token: 0x040000BA RID: 186
	[SerializeField]
	private AirplaneLevelCanteenAnimator canteenAnimator;

	// Token: 0x040000BB RID: 187
	[SerializeField]
	private AirplaneLevelBulldogPlane bulldogPlane;

	// Token: 0x040000BC RID: 188
	[SerializeField]
	private AirplaneLevelBulldogParachute bulldogParachute;

	// Token: 0x040000BD RID: 189
	[SerializeField]
	private AirplaneLevelBulldogCatAttack bulldogCatAttack;

	// Token: 0x040000BE RID: 190
	[SerializeField]
	public AirplaneLevelLeader leader;

	// Token: 0x040000BF RID: 191
	[SerializeField]
	private Animator secretIntro;

	// Token: 0x040000C0 RID: 192
	[SerializeField]
	private Animator leaderAnimator;

	// Token: 0x040000C1 RID: 193
	[SerializeField]
	private AirplaneLevelSecretLeader secretLeader;

	// Token: 0x040000C2 RID: 194
	[SerializeField]
	private AirplaneLevelSecretTerrier[] secretTerriers;

	// Token: 0x040000C3 RID: 195
	[SerializeField]
	private AnimationClip rotateClip;

	// Token: 0x040000C4 RID: 196
	[SerializeField]
	private LevelPit pitTop;

	// Token: 0x040000C5 RID: 197
	[SerializeField]
	private Transform terrierPivotPoint;

	// Token: 0x040000C6 RID: 198
	[SerializeField]
	private Transform[] secretPhaseTerrierPositions;

	// Token: 0x040000C7 RID: 199
	[SerializeField]
	private Transform[] secretPhaseLeaderPositions;

	// Token: 0x040000C8 RID: 200
	[SerializeField]
	private Transform[] leaderDeathPositions;

	// Token: 0x040000C9 RID: 201
	private bool[] secretPhaseHoleOccupied;

	// Token: 0x040000CA RID: 202
	[SerializeField]
	private BlurOptimized bgBlur;

	// Token: 0x040000CB RID: 203
	[SerializeField]
	private UnityEngine.Camera bgCamera;

	// Token: 0x040000CC RID: 204
	[Header("Prefabs")]
	[SerializeField]
	private AirplaneLevelTerrier terrierPrefab;

	// Token: 0x040000CD RID: 205
	public bool terriersIntroFinished;

	// Token: 0x040000CE RID: 206
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitMain;

	// Token: 0x040000CF RID: 207
	[SerializeField]
	private Sprite _bossPortraitPhaseTwo;

	// Token: 0x040000D0 RID: 208
	[SerializeField]
	private Sprite _bossPortraitPhaseThree;

	// Token: 0x040000D1 RID: 209
	[SerializeField]
	private Sprite _bossPortraitPhaseSecret;

	// Token: 0x040000D2 RID: 210
	[SerializeField]
	private string _bossQuoteMain;

	// Token: 0x040000D3 RID: 211
	[SerializeField]
	private string _bossQuotePhaseTwo;

	// Token: 0x040000D4 RID: 212
	[SerializeField]
	private string _bossQuotePhaseThree;

	// Token: 0x040000D5 RID: 213
	private float terrierHPTotal;

	// Token: 0x040000D6 RID: 214
	private float decreaseAmount;

	// Token: 0x040000D7 RID: 215
	private bool allTerriersDead;

	// Token: 0x040000D8 RID: 216
	private bool secretPhaseActivated;

	// Token: 0x040000D9 RID: 217
	private int secretTargetTerrier;

	// Token: 0x040000DB RID: 219
	private List<AirplaneLevelTerrier> terriers;

	// Token: 0x040000DC RID: 220
	private List<AirplaneLevelTerrierSmokeFX> smokeFXPool = new List<AirplaneLevelTerrierSmokeFX>();

	// Token: 0x040000DD RID: 221
	[SerializeField]
	private AirplaneLevelTerrierSmokeFX smokePrefab;

	// Token: 0x040000DE RID: 222
	private HashSet<SpriteRenderer> shadowableRenderers = new HashSet<SpriteRenderer>();

	// Token: 0x040000DF RID: 223
	private float currentShadowLevel = 1f;
}
