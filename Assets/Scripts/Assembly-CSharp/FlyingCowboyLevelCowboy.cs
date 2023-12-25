using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200064F RID: 1615
public class FlyingCowboyLevelCowboy : LevelProperties.FlyingCowboy.Entity
{
	// Token: 0x1700038C RID: 908
	// (get) Token: 0x0600213D RID: 8509 RVA: 0x001334EC File Offset: 0x001318EC
	// (set) Token: 0x0600213E RID: 8510 RVA: 0x001334F4 File Offset: 0x001318F4
	public FlyingCowboyLevelCowboy.State state { get; private set; }

	// Token: 0x1700038D RID: 909
	// (get) Token: 0x0600213F RID: 8511 RVA: 0x001334FD File Offset: 0x001318FD
	// (set) Token: 0x06002140 RID: 8512 RVA: 0x00133505 File Offset: 0x00131905
	public bool IsDead { get; private set; }

	// Token: 0x1700038E RID: 910
	// (get) Token: 0x06002141 RID: 8513 RVA: 0x0013350E File Offset: 0x0013190E
	// (set) Token: 0x06002142 RID: 8514 RVA: 0x00133516 File Offset: 0x00131916
	public bool onBottom { get; private set; }

	// Token: 0x06002143 RID: 8515 RVA: 0x0013351F File Offset: 0x0013191F
	private void OnEnable()
	{
		SceneLoader.OnFadeOutStartEvent += this.onFadeOutStartEvent;
		PlayerManager.OnPlayerJoinedEvent += this.onPlayerJoinedEvent;
	}

	// Token: 0x06002144 RID: 8516 RVA: 0x00133543 File Offset: 0x00131943
	private void OnDisable()
	{
		SceneLoader.OnFadeOutStartEvent -= this.onFadeOutStartEvent;
		PlayerManager.OnPlayerJoinedEvent -= this.onPlayerJoinedEvent;
	}

	// Token: 0x06002145 RID: 8517 RVA: 0x00133568 File Offset: 0x00131968
	private void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.allDebris = new List<FlyingCowboyLevelDebris>();
		this.topPositions = new Vector3[6];
		this.sidePositions = new Vector3[6];
		this.bottomPositions = new Vector3[6];
		this.topCurvePositions = new Vector3[4];
		this.bottomCurvePositions = new Vector3[4];
		LevelProperties.FlyingCowboy.State currentState = base.properties.CurrentState;
		this.debrisCurveString = new PatternString(currentState.debris.debrisCurveShotString, true, true);
		this.debrisParryString = new PatternString(currentState.debris.debrisParryString, true);
		this.ricochetParryString = new PatternString(currentState.ricochet.splitParryString, true);
		this.backshotHighSpawnPosition = new PatternString(currentState.backshotEnemy.highSpawnPosition, true, true);
		this.backshotLowSpawnPosition = new PatternString(currentState.backshotEnemy.lowSpawnPosition, true, true);
		this.backshotSpawnDelay = new PatternString(currentState.backshotEnemy.spawnDelay, true, true);
		this.backshotBulletParryable = new PatternString(currentState.backshotEnemy.bulletParryString, true);
		this.backshotAnticipationStartDistancePattern = new PatternString(currentState.backshotEnemy.anticipationStartDistance, true, true);
		this.SetupDebrisSpawnPoints();
		base.StartCoroutine(this.wobble_cr());
		this.introBird = this.birdPrefab.Spawn(this.birdEndPosition.position);
		this.introBird.InitializeIntro(this.birdEndPosition.position);
		this.SFX_COWGIRL_COWGIRL_WheelConstantLoop();
	}

	// Token: 0x06002146 RID: 8518 RVA: 0x00133700 File Offset: 0x00131B00
	private void Update()
	{
		if (this.forcePlayer1 != null)
		{
			this.forcePlayer1.UpdateStrength(CupheadTime.Delta);
		}
		if (this.forcePlayer2 != null)
		{
			this.forcePlayer2.UpdateStrength(CupheadTime.Delta);
		}
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002147 RID: 8519 RVA: 0x00133764 File Offset: 0x00131B64
	private void SetupDebrisSpawnPoints()
	{
		this.sidePositions = new Vector3[6];
		float num = (this.vacuumSpawnTop.position.y - this.vacuumSpawnBottom.position.y) / 5f;
		for (int i = 0; i < 6; i++)
		{
			float x = this.vacuumSpawnBottom.position.x;
			float y = (i != 5) ? (this.vacuumSpawnBottom.position.y + num * (float)i) : this.vacuumSpawnTop.position.y;
			this.sidePositions[i] = new Vector3(x, y);
		}
		float num2 = ((float)this.debrisSpawnHorizontalSpacing - this.vacuumSpawnTop.position.x) / 6f;
		for (int j = 0; j < 6; j++)
		{
			float x2 = this.vacuumSpawnTop.position.x + num2 + num2 * (float)j;
			float y2 = this.vacuumSpawnTop.position.y;
			this.topPositions[j] = new Vector3(x2, y2);
		}
		for (int k = 0; k < 4; k++)
		{
			float x3 = this.vacuumSpawnTop.position.x + num2 + num2 * (float)(6 + k);
			float y3 = this.vacuumSpawnTop.position.y;
			this.topCurvePositions[k] = new Vector3(x3, y3);
		}
		for (int l = 0; l < 6; l++)
		{
			float x4 = this.vacuumSpawnBottom.position.x + num2 + num2 * (float)l;
			float y4 = this.vacuumSpawnBottom.position.y;
			this.bottomPositions[l] = new Vector3(x4, y4);
		}
		for (int m = 0; m < 4; m++)
		{
			float x5 = this.vacuumSpawnTop.position.x + num2 + num2 * (float)(6 + m);
			float y5 = this.vacuumSpawnTop.position.y;
			this.topCurvePositions[m] = new Vector3(x5, y5);
		}
		for (int n = 0; n < 4; n++)
		{
			float x6 = this.vacuumSpawnBottom.position.x + num2 + num2 * (float)(6 + n);
			float y6 = this.vacuumSpawnBottom.position.y;
			this.bottomCurvePositions[n] = new Vector3(x6, y6);
		}
	}

	// Token: 0x06002148 RID: 8520 RVA: 0x00133A58 File Offset: 0x00131E58
	public override void LevelInit(LevelProperties.FlyingCowboy properties)
	{
		base.LevelInit(properties);
		Level.Current.OnIntroEvent += this.onIntroEventHandler;
		this.snakeOilShotsPerAttackString = new PatternString(properties.CurrentState.snakeAttack.shotsPerAttack, true, true);
		this.snakeOffsetString = new PatternString(properties.CurrentState.snakeAttack.snakeOffsetString, true, true);
		this.snakeWidthString = new PatternString(properties.CurrentState.snakeAttack.snakeWidthString, true, true);
		this.debrisTopMainIndex = UnityEngine.Random.Range(0, properties.CurrentState.debris.debrisTopSpawn.Length);
		this.debrisBottomMainIndex = UnityEngine.Random.Range(0, properties.CurrentState.debris.debrisBottomSpawn.Length);
		this.debrisSideMainIndex = UnityEngine.Random.Range(0, properties.CurrentState.debris.debrisSideSpawn.Length);
		this.initialSaloonPosition = base.transform.position;
		this.state = FlyingCowboyLevelCowboy.State.Idle;
	}

	// Token: 0x06002149 RID: 8521 RVA: 0x00133B4B File Offset: 0x00131F4B
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x0600214A RID: 8522 RVA: 0x00133B5E File Offset: 0x00131F5E
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600214B RID: 8523 RVA: 0x00133B7C File Offset: 0x00131F7C
	private void onIntroEventHandler()
	{
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x0600214C RID: 8524 RVA: 0x00133B8C File Offset: 0x00131F8C
	private IEnumerator intro_cr()
	{
		base.StartCoroutine(this.introBird_cr(0f));
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		base.animator.Play("Intro", 0);
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", 0, false, true);
		base.StartCoroutine(this.main_cr());
		yield break;
	}

	// Token: 0x0600214D RID: 8525 RVA: 0x00133BA8 File Offset: 0x00131FA8
	private IEnumerator introBird_cr(float time)
	{
		if (this.introBirdTriggered)
		{
			yield break;
		}
		this.introBirdTriggered = true;
		yield return CupheadTime.WaitForSeconds(this, time);
		this.introBird.MoveIntro(this.birdStartPosition.position, base.properties.CurrentState.bird);
		this.introBird = null;
		yield break;
	}

	// Token: 0x0600214E RID: 8526 RVA: 0x00133BCC File Offset: 0x00131FCC
	private IEnumerator main_cr()
	{
		LevelProperties.FlyingCowboy.Cart p = base.properties.CurrentState.cart;
		PatternString pattern = new PatternString(p.cartAttackString, true, true);
		base.StartCoroutine(this.spawnBirdEnemies_cr());
		while (pattern.GetString() != "S")
		{
			pattern.PopString();
			yield return null;
		}
		for (;;)
		{
			while (this.state != FlyingCowboyLevelCowboy.State.Idle || this.phase2Trigger)
			{
				yield return null;
			}
			yield return null;
			string @string = pattern.GetString();
			if (@string != null)
			{
				if (!(@string == "M"))
				{
					if (!(@string == "S"))
					{
						if (@string == "B")
						{
							base.StartCoroutine(this.beamAttack_cr());
							base.StartCoroutine(this.spawnBackshotEnemy_cr());
						}
					}
					else
					{
						base.StartCoroutine(this.snakeAttack_cr());
						base.StartCoroutine(this.spawnBackshotEnemy_cr());
					}
				}
				else
				{
					base.StartCoroutine(this.wait_cr());
				}
			}
			pattern.PopString();
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600214F RID: 8527 RVA: 0x00133BE8 File Offset: 0x00131FE8
	private IEnumerator breakableRecoveryPhase1_cr(float duration)
	{
		float t = 0f;
		while (t < duration && !this.phase2Trigger)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002150 RID: 8528 RVA: 0x00133C0C File Offset: 0x0013200C
	private IEnumerator wobble_cr()
	{
		for (;;)
		{
			this.wobbleTimeElapsed.x = this.wobbleTimeElapsed.x + CupheadTime.Delta;
			this.wobbleTimeElapsed.y = this.wobbleTimeElapsed.y + CupheadTime.Delta;
			if (this.wobbleTimeElapsed.x >= 2f * this.wobbleDuration.x)
			{
				this.wobbleTimeElapsed.x = this.wobbleTimeElapsed.x - 2f * this.wobbleDuration.x;
			}
			float tx;
			if (this.wobbleTimeElapsed.x > this.wobbleDuration.x)
			{
				tx = 1f - (this.wobbleTimeElapsed.x - this.wobbleDuration.x) / this.wobbleDuration.x;
			}
			else
			{
				tx = this.wobbleTimeElapsed.x / this.wobbleDuration.x;
			}
			if (this.wobbleTimeElapsed.y >= 2f * this.wobbleDuration.y)
			{
				this.wobbleTimeElapsed.y = this.wobbleTimeElapsed.y - 2f * this.wobbleDuration.y;
			}
			float ty;
			if (this.wobbleTimeElapsed.y > this.wobbleDuration.y)
			{
				ty = 1f - (this.wobbleTimeElapsed.y - this.wobbleDuration.y) / this.wobbleDuration.y;
			}
			else
			{
				ty = this.wobbleTimeElapsed.y / this.wobbleDuration.y;
			}
			Vector3 position = this.initialSaloonPosition;
			position.x += EaseUtils.EaseInOutSine(this.wobbleRadius.x, -this.wobbleRadius.x, tx);
			position.y += EaseUtils.EaseInOutSine(this.wobbleRadius.y, -this.wobbleRadius.y, ty);
			base.transform.position = position;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002151 RID: 8529 RVA: 0x00133C28 File Offset: 0x00132028
	private IEnumerator wait_cr()
	{
		this.state = FlyingCowboyLevelCowboy.State.Wait;
		LevelProperties.FlyingCowboy.Cart p = base.properties.CurrentState.cart;
		base.animator.SetBool("OnHide", true);
		string animationBaseName = (!this.onBottom) ? "HideToLow" : "HideToHigh";
		yield return base.animator.WaitForNormalizedTime(this, 1f, animationBaseName + "Start", 0, false, false, true);
		base.animator.Play((!this.onBottom) ? "ToOpen" : "ToClosed", FlyingCowboyLevelCowboy.DoorsAnimatorLayer);
		yield return CupheadTime.WaitForSeconds(this, p.cartPopinTime);
		this.onBottom = !this.onBottom;
		base.animator.SetBool("IsLow", this.onBottom);
		yield return base.animator.WaitForAnimationToStart(this, animationBaseName + "End", false);
		base.animator.SetBool("OnHide", false);
		yield return base.animator.WaitForAnimationToEnd(this, animationBaseName + "End", false, true);
		this.state = FlyingCowboyLevelCowboy.State.Idle;
		yield break;
	}

	// Token: 0x06002152 RID: 8530 RVA: 0x00133C44 File Offset: 0x00132044
	private IEnumerator snakeAttack_cr()
	{
		LevelProperties.FlyingCowboy.SnakeAttack p = base.properties.CurrentState.snakeAttack;
		this.state = FlyingCowboyLevelCowboy.State.SnakeAttack;
		string animationPrefix = "SnakeOil" + ((!this.onBottom) ? "_High" : "_Low") + ".";
		base.animator.SetTrigger("OnSnakeOil");
		base.animator.SetBool("SnakeInitialDelay", false);
		int shotsPerAttack = this.snakeOilShotsPerAttackString.PopInt();
		for (int shotCount = 0; shotCount < shotsPerAttack; shotCount++)
		{
			if (this.phase2Trigger && shotCount > 0)
			{
				break;
			}
			if (shotCount > 0)
			{
				yield return base.animator.WaitForAnimationToEnd(this, animationPrefix + "SnakeOilShoot", false, true);
				yield return CupheadTime.WaitForSeconds(this, p.attackDelay);
				base.animator.SetTrigger("OnSnakeShoot");
			}
			yield return base.animator.WaitForAnimationToStart(this, animationPrefix + "SnakeOilShoot", false);
		}
		base.animator.SetTrigger("OnSnakeEnd");
		yield return base.animator.WaitForAnimationToEnd(this, animationPrefix + "SnakeOilExit", false, true);
		yield return base.StartCoroutine(this.breakableRecoveryPhase1_cr(p.attackRecovery));
		this.state = FlyingCowboyLevelCowboy.State.Idle;
		yield break;
	}

	// Token: 0x06002153 RID: 8531 RVA: 0x00133C60 File Offset: 0x00132060
	private void animationEvent_SnakeShoot()
	{
		LevelProperties.FlyingCowboy.SnakeAttack snakeAttack = base.properties.CurrentState.snakeAttack;
		float num = this.snakeOffsetString.PopFloat();
		float num2 = this.snakeWidthString.PopFloat();
		AbstractPlayerController next = PlayerManager.GetNext();
		float snakeSpawnX = 640f - snakeAttack.breakLinePosition;
		float num3 = next.transform.position.y + num;
		for (int i = 0; i < 2; i++)
		{
			float num4 = (i != 0) ? (-num2) : num2;
			float num5 = num3 + num4;
			float finalYPosition = (num5 <= 0f) ? ((num5 <= -360f) ? -340f : num5) : ((num5 >= 360f) ? 340f : num5);
			Vector3 position = (!this.onBottom) ? this.snakeTopRoot[i].position : this.snakeBottomRoot[i].position;
			this.snakeOilMuzzleFXPrefab.Create(position);
			this.oilBlobPrefab.Create(position, finalYPosition, snakeSpawnX, snakeAttack, i == 0);
		}
	}

	// Token: 0x06002154 RID: 8532 RVA: 0x00133D88 File Offset: 0x00132188
	private IEnumerator beamAttack_cr()
	{
		LevelProperties.FlyingCowboy.BeamAttack p = base.properties.CurrentState.beamAttack;
		this.state = FlyingCowboyLevelCowboy.State.BeamAttack;
		this.cactus.SetActive(true);
		string prefix = (!this.onBottom) ? "Cactus_High." : "Cactus_Low.";
		base.animator.SetTrigger("OnCactus");
		yield return base.animator.WaitForAnimationToEnd(this, prefix + "Intro", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.beamWarningTime);
		base.animator.SetTrigger("EndLasso");
		this.SFX_COWGIRL_COWGIRL_LassoSpinLoopStop();
		yield return base.animator.WaitForAnimationToStart(this, prefix + "Hold", false);
		yield return CupheadTime.WaitForSeconds(this, p.beamDuration);
		base.animator.SetTrigger("EndCactusHold");
		yield return base.animator.WaitForAnimationToEnd(this, prefix + "End", false, true);
		this.cactus.SetActive(false);
		yield return base.StartCoroutine(this.breakableRecoveryPhase1_cr(p.attackRecovery));
		this.state = FlyingCowboyLevelCowboy.State.Idle;
		yield break;
	}

	// Token: 0x06002155 RID: 8533 RVA: 0x00133DA4 File Offset: 0x001321A4
	private IEnumerator spawnBackshotEnemy_cr()
	{
		LevelProperties.FlyingCowboy.BackshotEnemy p = base.properties.CurrentState.backshotEnemy;
		yield return CupheadTime.WaitForSeconds(this, this.backshotSpawnDelay.PopFloat());
		float positionY = (!this.onBottom) ? this.backshotLowSpawnPosition.PopFloat() : this.backshotHighSpawnPosition.PopFloat();
		Vector3 position = new Vector3(740f, positionY);
		this.backshotPrefab.Create(position, 180f, p.enemySpeed, p.bulletSpeed, p.enemyHealth, this.backshotAnticipationStartDistancePattern.PopFloat(), this.backshotBulletParryable.PopLetter() == 'P');
		yield break;
	}

	// Token: 0x06002156 RID: 8534 RVA: 0x00133DC0 File Offset: 0x001321C0
	private IEnumerator spawnBirdEnemies_cr()
	{
		LevelProperties.FlyingCowboy.Bird p = base.properties.CurrentState.bird;
		PatternString bulletLandingPositionPattern = new PatternString(p.bulletLandingPosition, true, true);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, p.spawnDelayRange.RandomFloat());
			bool canSpawn = false;
			float safetyTimer = 0f;
			while (!canSpawn)
			{
				bool found = false;
				foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
				{
					if (abstractPlayerController != null && this.birdSafetyZone.Contains(abstractPlayerController.center))
					{
						found = true;
						safetyTimer += CupheadTime.Delta;
						break;
					}
				}
				if (found && safetyTimer < p.safetyZoneMaxDuration)
				{
					yield return null;
				}
				else
				{
					canSpawn = true;
				}
			}
			FlyingCowboyLevelBird bird = this.birdPrefab.Spawn(this.birdStartPosition.position);
			bird.Initialize(this.birdStartPosition.position, this.birdEndPosition.position, bulletLandingPositionPattern.PopFloat(), p, this);
			while (bird != null)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06002157 RID: 8535 RVA: 0x00133DDC File Offset: 0x001321DC
	private void SpawnUFOs()
	{
		LevelProperties.FlyingCowboy.UFOEnemy uFOEnemy = base.properties.CurrentState.uFOEnemy;
		Vector3 pos = new Vector3(740f, uFOEnemy.topUFOVerticalPosition);
		this.ufo = this.ufoPrefab.Spawn<FlyingCowboyLevelUFO>();
		this.ufo.Init(pos, base.properties.CurrentState.uFOEnemy, uFOEnemy.UFOHealth);
	}

	// Token: 0x06002158 RID: 8536 RVA: 0x00133E40 File Offset: 0x00132240
	public void OnPhase2(LevelProperties.FlyingCowboy.Pattern postTransitionPattern)
	{
		this.phase2Trigger = true;
		base.animator.SetBool("OnPhase2", true);
		base.StartCoroutine(this.phase2TransStart_cr(postTransitionPattern));
	}

	// Token: 0x06002159 RID: 8537 RVA: 0x00133E68 File Offset: 0x00132268
	private IEnumerator phase2TransStart_cr(LevelProperties.FlyingCowboy.Pattern postTransitionPattern)
	{
		int hash = Animator.StringToHash("HideToLowStart");
		int hash2 = Animator.StringToHash("HideToHighStart");
		for (;;)
		{
			int hash3 = base.animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
			if (hash3 == hash || hash3 == hash2)
			{
				break;
			}
			yield return null;
		}
		float previousT = float.MaxValue;
		WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
		for (;;)
		{
			yield return waitForEndOfFrame;
			float t = MathUtilities.DecimalPart(base.animator.GetCurrentAnimatorStateInfo(FlyingCowboyLevelCowboy.SaloonAnimatorLayer).normalizedTime);
			if (previousT < 0.041666668f && t > 0.041666668f)
			{
				break;
			}
			if (previousT < 0.5416667f && t > 0.5416667f)
			{
				goto Block_5;
			}
			previousT = t;
		}
		this.lanternARenderer.enabled = false;
		this.lanternBRenderer.enabled = true;
		goto IL_1A8;
		Block_5:
		this.lanternARenderer.enabled = true;
		this.lanternBRenderer.enabled = false;
		IL_1A8:
		base.animator.Play("Ph1_To_Ph2", 0);
		this.StopAllCoroutines();
		if (this.ufo != null)
		{
			this.ufo.Dead();
		}
		this.state = FlyingCowboyLevelCowboy.State.PhaseTrans;
		base.StartCoroutine(this.phase2_trans_cr(postTransitionPattern));
		yield break;
	}

	// Token: 0x0600215A RID: 8538 RVA: 0x00133E8C File Offset: 0x0013228C
	private IEnumerator phase2_trans_cr(LevelProperties.FlyingCowboy.Pattern postTransitionPattern)
	{
		yield return null;
		yield return base.animator.WaitForNormalizedTime(this, 0.8666667f, "Ph1_To_Ph2", 0, false, false, true);
		this.SFX_COWGIRL_COWGIRL_WheelConstantLoopStop();
		this.Vacuum(false, postTransitionPattern);
		yield return base.animator.WaitForNormalizedTime(this, 1f, "Ph1_To_Ph2", 0, false, false, true);
		base.animator.Play("Vacuum", 0);
		base.animator.Play("TransitionSmoke", FlyingCowboyLevelCowboy.TransitionSmokeLayer);
		yield return base.animator.WaitForAnimationToEnd(this, "TransitionSmoke", FlyingCowboyLevelCowboy.TransitionSmokeLayer, false, true);
		this.endTransitionTrigger = true;
		while (this.transitionVacuumAttackCoroutine != null)
		{
			yield return null;
		}
		if (postTransitionPattern != LevelProperties.FlyingCowboy.Pattern.Vacuum || this.phase3Trigger)
		{
			this.endVacuumPullPlayer();
		}
		this.endTransitionTrigger = false;
		yield return null;
		yield return null;
		this.state = FlyingCowboyLevelCowboy.State.Idle;
		yield break;
	}

	// Token: 0x0600215B RID: 8539 RVA: 0x00133EB0 File Offset: 0x001322B0
	private IEnumerator moveDown_cr()
	{
		this.phase2BasePosition = this.initialSaloonPosition;
		this.phase2BasePosition.y = -183f;
		Vector3 initialPosition = base.transform.position;
		Vector3 targetPosition = this.phase2BasePosition;
		float elapsedTime = 0f;
		while (elapsedTime < 2f)
		{
			yield return null;
			elapsedTime += CupheadTime.Delta;
			base.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / 2f);
		}
		yield break;
	}

	// Token: 0x0600215C RID: 8540 RVA: 0x00133ECC File Offset: 0x001322CC
	public void Vacuum(bool initial, LevelProperties.FlyingCowboy.Pattern postTransitionPattern = LevelProperties.FlyingCowboy.Pattern.Default)
	{
		base.animator.SetBool("OnRicochet", false);
		if (postTransitionPattern != LevelProperties.FlyingCowboy.Pattern.Default)
		{
			this.transitionVacuumAttackCoroutine = base.StartCoroutine(this.vacuum_cr(initial, postTransitionPattern));
		}
		else
		{
			base.StartCoroutine(this.vacuum_cr(initial, postTransitionPattern));
		}
	}

	// Token: 0x0600215D RID: 8541 RVA: 0x00133F18 File Offset: 0x00132318
	private IEnumerator vacuumCurveShots_cr(bool transition)
	{
		LevelProperties.FlyingCowboy.Debris p = base.properties.CurrentState.debris;
		string[] debrisCurveShotString;
		if (transition)
		{
			PatternString patternString = new PatternString(p.transitionCurveShotString, true, true);
			debrisCurveShotString = patternString.PopString().Split(new char[]
			{
				','
			});
		}
		else
		{
			debrisCurveShotString = this.debrisCurveString.PopString().Split(new char[]
			{
				','
			});
		}
		Vector3[] positions = this.topPositions;
		int spawnIndex = 0;
		float angle = 0f;
		Vector3 root = this.vacuumDebrisAimTransform.position;
		for (int i = 0; i < debrisCurveShotString.Length; i++)
		{
			string[] spawn = debrisCurveShotString[i].Split(new char[]
			{
				':'
			});
			foreach (string text in spawn)
			{
				if (text == "B")
				{
					positions = this.bottomCurvePositions;
				}
				else if (text == "T")
				{
					positions = this.topCurvePositions;
				}
				else
				{
					Parser.IntTryParse(text, out spawnIndex);
				}
			}
			float apexHeight = Mathf.Abs(this.vacuumDebrisAimTransform.position.x - positions[spawnIndex].x) + 300f;
			float timeToApex = p.debrisCurveApexTime;
			float height = -apexHeight;
			float apexTime2 = timeToApex * timeToApex;
			float g = -2f * height / apexTime2;
			float viX = 2f * height / timeToApex;
			float viY2 = viX * viX;
			float x = root.x - positions[spawnIndex].x;
			float y = root.y - positions[spawnIndex].y;
			float sqrtRooted = viY2 + 2f * g * x;
			float tEnd = (-viX + Mathf.Sqrt(sqrtRooted)) / g;
			float tEnd2 = (-viX - Mathf.Sqrt(sqrtRooted)) / g;
			float tEnd3 = Mathf.Max(tEnd, tEnd2);
			float velocityY = y / tEnd3;
			FlyingCowboyLevelDebris debris = this.largeVacuumDebrisPrefabs.GetRandom<FlyingCowboyLevelDebris>().Create(positions[spawnIndex], angle * 57.29578f, p.debrisOneSpeedStartEnd.min) as FlyingCowboyLevelDebris;
			debris.GetComponent<SpriteRenderer>().sortingOrder = i;
			bool parryable = this.debrisParryString.PopLetter() == 'P';
			debris.SetParryable(parryable);
			Vector3 velocity = new Vector3(viX, velocityY);
			debris.ToCurve(velocity, g);
			debris.SetupVacuum(this.vacuumDebrisAimTransform, this.vacuumDebrisDisappearTransform);
			this.allDebris.Add(debris);
			yield return CupheadTime.WaitForSeconds(this, p.debrisDelay);
			if (this.phase3Trigger || this.endTransitionTrigger)
			{
				break;
			}
		}
		yield break;
	}

	// Token: 0x0600215E RID: 8542 RVA: 0x00133F3C File Offset: 0x0013233C
	private IEnumerator vacuum_cr(bool initial, LevelProperties.FlyingCowboy.Pattern postTransitionPattern)
	{
		bool transition = postTransitionPattern != LevelProperties.FlyingCowboy.Pattern.Default;
		if (!initial)
		{
			this.SFX_COWGIRL_COWGIRL_P2_VacuumSuckLoop();
		}
		if (!transition)
		{
			this.state = FlyingCowboyLevelCowboy.State.Vacuum;
		}
		LevelProperties.FlyingCowboy.Debris p = base.properties.CurrentState.debris;
		if (!initial && !transition)
		{
			this.startVacuumPullPlayer(false);
		}
		if (!initial && !transition)
		{
			yield return CupheadTime.WaitForSeconds(this, p.warningDelayRange.RandomFloat());
		}
		PatternString debrisTypePattern = new PatternString(p.debrisTypeString, true);
		base.StartCoroutine(this.vacuumCurveShots_cr(transition));
		string[] debrisTop;
		string[] debrisBottom;
		string[] debrisSide;
		if (transition)
		{
			int num = UnityEngine.Random.Range(0, base.properties.CurrentState.debris.transitionTopSpawn.Length);
			int num2 = UnityEngine.Random.Range(0, base.properties.CurrentState.debris.transitionBottomSpawn.Length);
			int num3 = UnityEngine.Random.Range(0, base.properties.CurrentState.debris.transitionSideSpawn.Length);
			debrisTop = p.transitionTopSpawn[num].Split(new char[]
			{
				','
			});
			debrisBottom = p.transitionBottomSpawn[num2].Split(new char[]
			{
				','
			});
			debrisSide = p.transitionSideSpawn[num3].Split(new char[]
			{
				','
			});
		}
		else
		{
			debrisTop = p.debrisTopSpawn[this.debrisTopMainIndex].Split(new char[]
			{
				','
			});
			debrisBottom = p.debrisBottomSpawn[this.debrisBottomMainIndex].Split(new char[]
			{
				','
			});
			debrisSide = p.debrisSideSpawn[this.debrisSideMainIndex].Split(new char[]
			{
				','
			});
		}
		int debrisTopCount = 0;
		int debrisBottomCount = 0;
		int debrisSideCount = 0;
		int maxLength = Mathf.Max(new int[]
		{
			debrisTop.Length,
			debrisBottom.Length,
			debrisSide.Length
		});
		if (transition && postTransitionPattern == LevelProperties.FlyingCowboy.Pattern.Ricochet)
		{
			this.vacuumSizeCoroutine = base.StartCoroutine(this.growVacuum_cr());
		}
		for (int i = 0; i < maxLength; i++)
		{
			if (i < debrisTop.Length)
			{
				int posIndex;
				Parser.IntTryParse(debrisTop[debrisTopCount], out posIndex);
				this.createLinearDebris(this.topPositions[posIndex], debrisTypePattern.PopInt(), i);
				debrisTopCount++;
			}
			if (i < debrisBottom.Length)
			{
				int posIndex;
				Parser.IntTryParse(debrisBottom[debrisBottomCount], out posIndex);
				this.createLinearDebris(this.bottomPositions[posIndex], debrisTypePattern.PopInt(), i);
				debrisBottomCount++;
			}
			if (i < debrisSide.Length)
			{
				int posIndex;
				Parser.IntTryParse(debrisSide[debrisSideCount], out posIndex);
				this.createLinearDebris(this.sidePositions[posIndex], debrisTypePattern.PopInt(), i);
				debrisSideCount++;
			}
			yield return CupheadTime.WaitForSeconds(this, p.debrisDelay);
			if (this.phase3Trigger || this.endTransitionTrigger)
			{
				break;
			}
		}
		if (transition && postTransitionPattern == LevelProperties.FlyingCowboy.Pattern.Vacuum && !this.phase3Trigger)
		{
			this.allDebris.Clear();
		}
		else
		{
			if (!transition)
			{
				this.vacuumSizeCoroutine = base.StartCoroutine(this.growVacuum_cr());
			}
			bool allDebrisGone = false;
			while (!allDebrisGone)
			{
				allDebrisGone = true;
				for (int j = 0; j < this.allDebris.Count; j++)
				{
					if (this.allDebris[j] != null && !this.allDebris[j].dead)
					{
						allDebrisGone = false;
					}
				}
				yield return null;
			}
			this.allDebris.Clear();
			while (this.vacuumSizeCoroutine != null)
			{
				yield return null;
			}
		}
		if (!transition || (transition && (postTransitionPattern == LevelProperties.FlyingCowboy.Pattern.Ricochet || this.phase3Trigger)))
		{
			this.endVacuumPullPlayer();
			this.SFX_COWGIRL_COWGIRL_P2_VacuumSuckLoopStop();
		}
		if (transition)
		{
			this.transitionVacuumAttackCoroutine = null;
			if (this.phase3Trigger)
			{
				this.SFX_COWGIRL_COWGIRL_P2_VacuumSuckLoopStop();
				base.animator.SetBool("OnPhase3", true);
			}
		}
		else
		{
			this.SFX_COWGIRL_COWGIRL_P2_VacuumSuckLoopStop();
			this.debrisTopMainIndex = (this.debrisTopMainIndex + 1) % p.debrisTopSpawn.Length;
			this.debrisBottomMainIndex = (this.debrisBottomMainIndex + 1) % p.debrisBottomSpawn.Length;
			this.debrisSideMainIndex = (this.debrisSideMainIndex + 1) % p.debrisSideSpawn.Length;
			bool manualDeathHandling = true;
			if (this.phase3Trigger)
			{
				manualDeathHandling = false;
				base.animator.SetBool("OnPhase3", true);
			}
			else
			{
				base.animator.SetBool("OnRicochet", true);
				yield return CupheadTime.WaitForSeconds(this, p.hesitate);
			}
			this.state = FlyingCowboyLevelCowboy.State.Idle;
			if (this.phase3Trigger && manualDeathHandling)
			{
				this.Ricochet();
			}
		}
		yield break;
	}

	// Token: 0x0600215F RID: 8543 RVA: 0x00133F68 File Offset: 0x00132368
	private void createLinearDebris(Vector3 rootPosition, int type, int sortingIndex)
	{
		LevelProperties.FlyingCowboy.Debris debris = base.properties.CurrentState.debris;
		MinMax minMax;
		FlyingCowboyLevelDebris random;
		if (type == 1)
		{
			minMax = debris.debrisOneSpeedStartEnd;
			random = this.largeVacuumDebrisPrefabs.GetRandom<FlyingCowboyLevelDebris>();
		}
		else if (type == 2)
		{
			minMax = debris.debrisTwoSpeedStartEnd;
			random = this.mediumVacuumDebrisPrefabs.GetRandom<FlyingCowboyLevelDebris>();
		}
		else
		{
			minMax = debris.debrisThreeSpeedStartEnd;
			random = this.smallVacuumDebrisPrefabs.GetRandom<FlyingCowboyLevelDebris>();
		}
		Vector3 v = this.vacuumDebrisAimTransform.position - rootPosition;
		FlyingCowboyLevelDebris flyingCowboyLevelDebris = random.Create(rootPosition, MathUtils.DirectionToAngle(v), minMax.min) as FlyingCowboyLevelDebris;
		flyingCowboyLevelDebris.GetComponent<SpriteRenderer>().sortingOrder = 50 * type + sortingIndex;
		bool parryable = this.debrisParryString.PopLetter() == 'P';
		flyingCowboyLevelDebris.SetParryable(parryable);
		flyingCowboyLevelDebris.SetupLinearSpeed(minMax, debris.debrisSpeedUpDistance, this.vacuumDebrisAimTransform);
		flyingCowboyLevelDebris.SetupVacuum(this.vacuumDebrisAimTransform, this.vacuumDebrisDisappearTransform);
		this.allDebris.Add(flyingCowboyLevelDebris);
	}

	// Token: 0x06002160 RID: 8544 RVA: 0x00134070 File Offset: 0x00132470
	private void startVacuumPullPlayer(bool immediateFullStrength)
	{
		this.endVacuumPullPlayer();
		LevelProperties.FlyingCowboy.Debris debris = base.properties.CurrentState.debris;
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			PlanePlayerController planePlayerController = (PlanePlayerController)abstractPlayerController;
			if (!(planePlayerController == null))
			{
				FlyingCowboyLevelCowboy.VacuumForce force = new FlyingCowboyLevelCowboy.VacuumForce(planePlayerController, this.vacuumDebrisDisappearTransform, debris.vacuumWindStrength * 0.5f, (!immediateFullStrength) ? debris.vacuumTimeToFullStrength : 0f);
				planePlayerController.motor.AddForce(force);
				if (planePlayerController.id == PlayerId.PlayerOne)
				{
					this.forcePlayer1 = force;
				}
				else if (planePlayerController.id == PlayerId.PlayerTwo)
				{
					this.forcePlayer2 = force;
				}
			}
		}
	}

	// Token: 0x06002161 RID: 8545 RVA: 0x00134158 File Offset: 0x00132558
	private void endVacuumPullPlayer()
	{
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			PlanePlayerController planePlayerController = (PlanePlayerController)abstractPlayerController;
			if (!(planePlayerController == null) && !(planePlayerController.motor == null))
			{
				planePlayerController.motor.RemoveForce(this.forcePlayer1);
				planePlayerController.motor.RemoveForce(this.forcePlayer2);
			}
		}
		this.forcePlayer1 = null;
		this.forcePlayer2 = null;
	}

	// Token: 0x06002162 RID: 8546 RVA: 0x00134204 File Offset: 0x00132604
	private IEnumerator growVacuum_cr()
	{
		int hash = Animator.StringToHash("Vacuum");
		float previousTime = float.MaxValue;
		for (;;)
		{
			float normalizedTime = MathUtilities.DecimalPart(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
			if (previousTime < 0.41666666f && normalizedTime >= 0.41666666f)
			{
				break;
			}
			previousTime = normalizedTime;
			yield return null;
		}
		this.setVacuumTransition();
		previousTime = float.MaxValue;
		for (;;)
		{
			float normalizedTime2 = MathUtilities.DecimalPart(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
			if (previousTime < 0.29166666f && normalizedTime2 >= 0.29166666f)
			{
				break;
			}
			previousTime = normalizedTime2;
			yield return null;
		}
		this.setVacuumBig();
		this.vacuumSizeCoroutine = null;
		yield break;
	}

	// Token: 0x06002163 RID: 8547 RVA: 0x0013421F File Offset: 0x0013261F
	public void Ricochet()
	{
		base.animator.SetBool("OnRicochet", true);
		base.StartCoroutine(this.ricochet_cr());
	}

	// Token: 0x06002164 RID: 8548 RVA: 0x00134240 File Offset: 0x00132640
	private IEnumerator ricochet_cr()
	{
		this.nextSafeShoot = 1;
		this.SFX_COWGIRL_COWGIRL_P2_StirrupWheelsLoopStart();
		this.state = FlyingCowboyLevelCowboy.State.Ricochet;
		LevelProperties.FlyingCowboy.Ricochet p = base.properties.CurrentState.ricochet;
		this.setVacuumBig();
		yield return base.animator.WaitForAnimationToStart(this, "Ricochet", false);
		this.vacuumSizeCoroutine = base.StartCoroutine(this.shrinkVacuum_cr());
		base.transform.position = this.phase2BasePosition;
		float elapsedTime = 0f;
		PatternString delayPattern = new PatternString(p.rainDelayString, true);
		PatternString bulletTypePattern = new PatternString(p.rainTypeString, true);
		PatternString xPositionPattern = new PatternString(p.rainSpawnString, true);
		PatternString speedPattern = new PatternString(p.rainSpeedString, true);
		while (elapsedTime < p.rainDuration)
		{
			if (this.phase3Trigger && elapsedTime >= 2f)
			{
				break;
			}
			float delayTime = delayPattern.PopFloat();
			yield return CupheadTime.WaitForSeconds(this, delayTime);
			FlyingCowboyLevelRicochetDebris.BulletType bulletType = FlyingCowboyLevelRicochetDebris.BulletType.Nothing;
			if (bulletTypePattern.PopLetter() == 'R')
			{
				bulletType = FlyingCowboyLevelRicochetDebris.BulletType.Ricochet;
			}
			float xPosition = xPositionPattern.PopFloat();
			float speed = speedPattern.PopFloat();
			this.ricochetPrefab.Create(new Vector3(-xPosition, 430f), speed, p.splitBulletSpeed, bulletType, bulletType != FlyingCowboyLevelRicochetDebris.BulletType.Nothing && this.ricochetParryString.PopLetter() == 'P');
			elapsedTime += delayTime;
		}
		base.animator.SetBool("OnRicochet", false);
		this.SFX_COWGIRL_COWGIRL_P2_StirrupWheelsLoopStop();
		if (this.phase3Trigger)
		{
			if (this.vacuumSizeCoroutine != null)
			{
				base.StopCoroutine(this.vacuumSizeCoroutine);
				this.vacuumSizeCoroutine = null;
				this.setVacuumRegular();
			}
			base.animator.SetBool("OnPhase3", true);
		}
		else
		{
			yield return CupheadTime.WaitForSeconds(this, p.rainRecoveryTime);
		}
		if (this.phase3Trigger)
		{
			base.animator.SetBool("OnPhase3", true);
		}
		this.state = FlyingCowboyLevelCowboy.State.Idle;
		yield break;
	}

	// Token: 0x06002165 RID: 8549 RVA: 0x0013425C File Offset: 0x0013265C
	private void animationEvent_SafeShoot(int eventType)
	{
		if (eventType == 0)
		{
			if (this.nextSafeShoot == 0)
			{
				AbstractProjectile abstractProjectile = this.ricochetUpPrefab.Create(this.ricochetUpSpawnPoint.position);
				abstractProjectile.animator.Play("A");
				abstractProjectile.animator.Update(0f);
				this.nextSafeShoot = 1;
			}
			else if (this.nextSafeShoot == 2)
			{
				AbstractProjectile abstractProjectile2 = this.ricochetUpPrefab.Create(this.ricochetUpSpawnPoint.position);
				abstractProjectile2.animator.Play("C");
				abstractProjectile2.animator.Update(0f);
				this.nextSafeShoot = 1;
			}
		}
		else if (eventType == 1 && this.nextSafeShoot == 1)
		{
			AbstractProjectile abstractProjectile3 = this.ricochetUpPrefab.Create(this.ricochetUpSpawnPoint.position);
			abstractProjectile3.animator.Play("B");
			abstractProjectile3.animator.Update(0f);
			this.nextSafeShoot = ((!Rand.Bool()) ? 2 : 0);
			this.shootCoins();
		}
	}

	// Token: 0x06002166 RID: 8550 RVA: 0x00134384 File Offset: 0x00132784
	private void shootCoins()
	{
		LevelProperties.FlyingCowboy.Ricochet ricochet = base.properties.CurrentState.ricochet;
		int num = ricochet.coinCountRange.RandomInt();
		for (int i = 0; i < num; i++)
		{
			Vector2 vector = new Vector2(ricochet.coinSpeedXRange.RandomFloat(), KinematicUtilities.CalculateInitialSpeedToReachApex(ricochet.coinHeightRange.RandomFloat(), ricochet.coinGravity));
			float rotation = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			BasicProjectile basicProjectile = this.coinProjectile.Create(this.ricochetUpSpawnPoint.transform.position, rotation, vector.magnitude);
			basicProjectile.Gravity = ricochet.coinGravity;
			basicProjectile.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
		}
	}

	// Token: 0x06002167 RID: 8551 RVA: 0x00134448 File Offset: 0x00132848
	private IEnumerator shrinkVacuum_cr()
	{
		yield return base.animator.WaitForNormalizedTime(this, 2f, "Ricochet", 0, false, false, true);
		this.setVacuumTransition();
		yield return base.animator.WaitForNormalizedTime(this, 2.9375f, "Ricochet", 0, false, false, true);
		this.setVacuumRegular();
		this.vacuumSizeCoroutine = null;
		yield break;
	}

	// Token: 0x06002168 RID: 8552 RVA: 0x00134464 File Offset: 0x00132864
	private void setVacuumRegular()
	{
		Renderer renderer = this.regularVacuumRenderer;
		bool flag = true;
		this.regularHoseRenderer.enabled = flag;
		renderer.enabled = flag;
		Renderer renderer2 = this.transitionVacuumRenderer;
		flag = false;
		this.bigHoseRenderer.enabled = flag;
		flag = flag;
		this.bigVacuumRenderer.enabled = flag;
		flag = flag;
		this.transitionHoseRenderer.enabled = flag;
		renderer2.enabled = flag;
	}

	// Token: 0x06002169 RID: 8553 RVA: 0x001344C4 File Offset: 0x001328C4
	private void setVacuumTransition()
	{
		Renderer renderer = this.transitionVacuumRenderer;
		bool flag = true;
		this.transitionHoseRenderer.enabled = flag;
		renderer.enabled = flag;
		Renderer renderer2 = this.regularVacuumRenderer;
		flag = false;
		this.bigHoseRenderer.enabled = flag;
		flag = flag;
		this.bigVacuumRenderer.enabled = flag;
		flag = flag;
		this.regularHoseRenderer.enabled = flag;
		renderer2.enabled = flag;
	}

	// Token: 0x0600216A RID: 8554 RVA: 0x00134524 File Offset: 0x00132924
	private void setVacuumBig()
	{
		Renderer renderer = this.bigVacuumRenderer;
		bool flag = true;
		this.bigHoseRenderer.enabled = flag;
		renderer.enabled = flag;
		Renderer renderer2 = this.regularVacuumRenderer;
		flag = false;
		this.transitionHoseRenderer.enabled = flag;
		flag = flag;
		this.transitionVacuumRenderer.enabled = flag;
		flag = flag;
		this.regularHoseRenderer.enabled = flag;
		renderer2.enabled = flag;
	}

	// Token: 0x0600216B RID: 8555 RVA: 0x00134581 File Offset: 0x00132981
	public void Death()
	{
		base.StartCoroutine(this.phase3_cr());
	}

	// Token: 0x0600216C RID: 8556 RVA: 0x00134590 File Offset: 0x00132990
	private IEnumerator phase3_cr()
	{
		this.phase3Trigger = true;
		yield return base.animator.WaitForAnimationToEnd(this, "Ph2_To_Ph3", false, true);
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0600216D RID: 8557 RVA: 0x001345AB File Offset: 0x001329AB
	private void aniEvent_SpawnMeat()
	{
		this.IsDead = true;
	}

	// Token: 0x0600216E RID: 8558 RVA: 0x001345B4 File Offset: 0x001329B4
	private void animationEvent_PosterFlyAway()
	{
		if (!this.posterFlyAwayTriggered)
		{
			base.animator.Play("FlyAway", FlyingCowboyLevelCowboy.PosterAnimatorLayer);
			this.posterRenderer.sortingLayerName = "Effects";
		}
		this.posterFlyAwayTriggered = true;
	}

	// Token: 0x0600216F RID: 8559 RVA: 0x001345F0 File Offset: 0x001329F0
	private void animationEvent_DisablePhase1Saloon()
	{
		foreach (SpriteRenderer spriteRenderer in this.saloonTransitionDisableRenderers)
		{
			spriteRenderer.enabled = false;
		}
	}

	// Token: 0x06002170 RID: 8560 RVA: 0x00134623 File Offset: 0x00132A23
	private void animationEvent_DisableSaloonCollider()
	{
		this.saloonCollidersParent.SetActive(false);
	}

	// Token: 0x06002171 RID: 8561 RVA: 0x00134631 File Offset: 0x00132A31
	private void animationEvent_EnablePlayerVacuumForce()
	{
		this.startVacuumPullPlayer(false);
	}

	// Token: 0x06002172 RID: 8562 RVA: 0x0013463A File Offset: 0x00132A3A
	private void animationEvent_DisableFrontSaloonWheel()
	{
		this.frontWheelRenderer.enabled = false;
	}

	// Token: 0x06002173 RID: 8563 RVA: 0x00134648 File Offset: 0x00132A48
	private void animationEvent_DisableBackSaloonWheel()
	{
		this.backWheelRenderer.enabled = false;
	}

	// Token: 0x06002174 RID: 8564 RVA: 0x00134658 File Offset: 0x00132A58
	private void animationEvent_TurnOffPhase1Animators()
	{
		base.animator.Play("Off", FlyingCowboyLevelCowboy.SaloonAnimatorLayer);
		base.animator.Play("Off", FlyingCowboyLevelCowboy.PosterAnimatorLayer);
		base.animator.Play("Off", FlyingCowboyLevelCowboy.DoorsAnimatorLayer);
		base.animator.Play("Off", FlyingCowboyLevelCowboy.WheelSmokeAnimatorLayer);
	}

	// Token: 0x06002175 RID: 8565 RVA: 0x001346B9 File Offset: 0x00132AB9
	private void animationEvent_MoveCowgirlDown()
	{
		base.StartCoroutine(this.moveDown_cr());
	}

	// Token: 0x06002176 RID: 8566 RVA: 0x001346C8 File Offset: 0x00132AC8
	private void animationEvent_SwapPhase2Puffs()
	{
		this.phase2PuffARenderer.enabled = this.phase2PuffBRenderer.enabled;
		this.phase2PuffBRenderer.enabled = !this.phase2PuffARenderer.enabled;
	}

	// Token: 0x06002177 RID: 8567 RVA: 0x001346F9 File Offset: 0x00132AF9
	private void onFadeOutStartEvent(float time)
	{
		base.StartCoroutine(this.introBird_cr(0.25f));
	}

	// Token: 0x06002178 RID: 8568 RVA: 0x0013470D File Offset: 0x00132B0D
	private void onPlayerJoinedEvent(PlayerId playerId)
	{
		if (this.forcePlayer1 != null || this.forcePlayer2 != null)
		{
			this.startVacuumPullPlayer(true);
		}
	}

	// Token: 0x06002179 RID: 8569 RVA: 0x0013472C File Offset: 0x00132B2C
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.green;
		float num = (this.vacuumSpawnTop.position.y - this.vacuumSpawnBottom.position.y) / 5f;
		for (int i = 0; i < 6; i++)
		{
			float x = this.vacuumSpawnBottom.position.x;
			float y = (i != 5) ? (this.vacuumSpawnBottom.position.y + num * (float)i) : this.vacuumSpawnTop.position.y;
			Gizmos.DrawWireSphere(new Vector3(x, y), 10f);
		}
		Gizmos.color = Color.yellow;
		float num2 = ((float)this.debrisSpawnHorizontalSpacing - this.vacuumSpawnTop.position.x) / 6f;
		for (int j = 0; j < 6; j++)
		{
			float x2 = this.vacuumSpawnTop.position.x + num2 + num2 * (float)j;
			float y2 = this.vacuumSpawnTop.position.y;
			Gizmos.DrawWireSphere(new Vector3(x2, y2), 10f);
		}
		Gizmos.color = Color.yellow;
		num2 = ((float)this.debrisSpawnHorizontalSpacing - this.vacuumSpawnBottom.position.x) / 6f;
		for (int k = 0; k < 6; k++)
		{
			float x3 = this.vacuumSpawnBottom.position.x + num2 + num2 * (float)k;
			float y3 = this.vacuumSpawnBottom.position.y;
			Gizmos.DrawWireSphere(new Vector3(x3, y3), 10f);
		}
		Gizmos.color = Color.red;
		for (int l = 0; l < 4; l++)
		{
			float x4 = this.vacuumSpawnTop.position.x + num2 + num2 * (float)(6 + l);
			float y4 = this.vacuumSpawnTop.position.y;
			Gizmos.DrawWireSphere(new Vector3(x4, y4), 10f);
		}
		Gizmos.color = Color.red;
		for (int m = 0; m < 4; m++)
		{
			float x5 = this.vacuumSpawnBottom.position.x + num2 + num2 * (float)(6 + m);
			float y5 = this.vacuumSpawnBottom.position.y;
			Gizmos.DrawWireSphere(new Vector3(x5, y5), 10f);
		}
	}

	// Token: 0x0600217A RID: 8570 RVA: 0x001349DD File Offset: 0x00132DDD
	private void AnimationEvent_SFX_COWGIRL_Vocal_Laugh()
	{
		AudioManager.Play("sfx_dlc_cowgirl_vocal_laugh");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_vocal_laugh");
	}

	// Token: 0x0600217B RID: 8571 RVA: 0x001349F9 File Offset: 0x00132DF9
	private void AnimationEvent_SFX_COWGIRL_Vocal_MooHa()
	{
		AudioManager.Play("sfx_dlc_cowgirl_vocal_mooha");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_vocal_mooha");
	}

	// Token: 0x0600217C RID: 8572 RVA: 0x00134A15 File Offset: 0x00132E15
	private void AnimationEvent_SFX_COWGIRL_Vocal_Surprised()
	{
		AudioManager.Play("sfx_dlc_cowgirl_vocal_surprised");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_vocal_surprised");
	}

	// Token: 0x0600217D RID: 8573 RVA: 0x00134A31 File Offset: 0x00132E31
	private void AnimationEvent_SFX_COWGIRL_Vocal_YeeHaw()
	{
		AudioManager.Play("sfx_dlc_cowgirl_vocal_yeehaw");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_vocal_yeehaw");
	}

	// Token: 0x0600217E RID: 8574 RVA: 0x00134A4D File Offset: 0x00132E4D
	private void AnimationEvent_SFX_COWGIRL_COWGIRL_JugGunRaise()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_raise");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_death_stompoffscreen");
	}

	// Token: 0x0600217F RID: 8575 RVA: 0x00134A69 File Offset: 0x00132E69
	private void AnimationEvent_SFX_COWGIRL_COWGIRL_JugGunHolster()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_holster");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_holster");
	}

	// Token: 0x06002180 RID: 8576 RVA: 0x00134A85 File Offset: 0x00132E85
	private void AnimationEvent_SFX_COWGIRL_COWGIRL_JugGunBlow()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_blow");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_blow");
	}

	// Token: 0x06002181 RID: 8577 RVA: 0x00134AA1 File Offset: 0x00132EA1
	private void AnimationEvent_SFX_COWGIRL_COWGIRL_JugGunBlowAndHolster()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_blowandholster");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_blowandholster");
	}

	// Token: 0x06002182 RID: 8578 RVA: 0x00134ABD File Offset: 0x00132EBD
	private void AnimationEvent_SFX_COWGIRL_COWGIRL_JugGunBlast()
	{
		AudioManager.Stop("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_spin_loop");
		AudioManager.Play("sfx_dlc_cowgirl_p1_snakeoilattack_juggunblast");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_snakeoilattack_juggunblast");
	}

	// Token: 0x06002183 RID: 8579 RVA: 0x00134AE3 File Offset: 0x00132EE3
	private void SFX_COWGIRL_COWGIRL_JugGunSpinLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_spin_loop");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_snakeoilattack_juggun_spin_loop");
	}

	// Token: 0x06002184 RID: 8580 RVA: 0x00134AFF File Offset: 0x00132EFF
	private void AnimationEvent_SFX_COWGIRL_COWGIRL_P1toP2VacuumSuckup()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p1_death_saloon_vacuumsuckup");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_death_saloon_vacuumsuckup");
	}

	// Token: 0x06002185 RID: 8581 RVA: 0x00134B1B File Offset: 0x00132F1B
	private void SFX_COWGIRL_COWGIRL_LassoSpinLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_cowgirl_p1_lasso_spin_loop");
		AudioManager.FadeSFXVolume("sfx_dlc_cowgirl_p1_lasso_spin_loop", 0.7f, 0.01f);
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_lasso_spin_loop");
	}

	// Token: 0x06002186 RID: 8582 RVA: 0x00134B4B File Offset: 0x00132F4B
	private void SFX_COWGIRL_COWGIRL_LassoSpinLoopStop()
	{
		AudioManager.FadeSFXVolume("sfx_dlc_cowgirl_p1_lasso_spin_loop", 0f, 0.2f);
	}

	// Token: 0x06002187 RID: 8583 RVA: 0x00134B61 File Offset: 0x00132F61
	private void AnimationEvent_SFX_COWGIRL_COWGIRL_LassoThrowCatchRelease()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p1_lasso_throw_catch_release");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_lasso_throw_catch_release");
	}

	// Token: 0x06002188 RID: 8584 RVA: 0x00134B7D File Offset: 0x00132F7D
	private void SFX_COWGIRL_COWGIRL_WheelConstantLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_cowgirl_p1_saloon_wheelsconstant_loop");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_saloon_wheelsconstant_loop");
	}

	// Token: 0x06002189 RID: 8585 RVA: 0x00134B99 File Offset: 0x00132F99
	private void SFX_COWGIRL_COWGIRL_WheelConstantLoopStop()
	{
		AudioManager.Stop("sfx_dlc_cowgirl_p1_saloon_wheelsconstant_loop");
	}

	// Token: 0x0600218A RID: 8586 RVA: 0x00134BA5 File Offset: 0x00132FA5
	private void AnimationEvent_SFX_COWGIRL_COWGIRL_PositionLowtoHigh()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p1_saloon_positionchange_lowtohigh");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_saloon_positionchange_lowtohigh");
	}

	// Token: 0x0600218B RID: 8587 RVA: 0x00134BC1 File Offset: 0x00132FC1
	private void AnimationEvent_SFX_COWGIRL_COWGIRL_PositionHightoLow()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p1_saloon_positionchange_hightolow");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_saloon_positionchange_hightolow");
	}

	// Token: 0x0600218C RID: 8588 RVA: 0x00134BDD File Offset: 0x00132FDD
	private void AnimationEvent_SFX_COWGIRL_COWGIRL_P2_Stirrups()
	{
		AudioManager.Play("sfx_DLC_Cowgirl_Stirrups");
		this.emitAudioFromObject.Add("sfx_DLC_Cowgirl_Stirrups");
	}

	// Token: 0x0600218D RID: 8589 RVA: 0x00134BF9 File Offset: 0x00132FF9
	private void AnimationEvent_SFX_COWGIRL_COWGIRL_P2_VacuumBlowback()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p2_vacuum_blowback");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p2_vacuum_blowback");
	}

	// Token: 0x0600218E RID: 8590 RVA: 0x00134C15 File Offset: 0x00133015
	private void AnimationEvent_SFX_COWGIRL_COWGIRL_P2_VacuumCrouchPosition()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p2_vacuum_blowback_crouchposition");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p2_vacuum_blowback_crouchposition");
	}

	// Token: 0x0600218F RID: 8591 RVA: 0x00134C31 File Offset: 0x00133031
	private void SFX_COWGIRL_COWGIRL_P2_VacuumSuckLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_cowgirl_p2_vacuum_constantsuck_loop");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p2_vacuum_constantsuck_loop");
		AudioManager.FadeSFXVolume("sfx_dlc_cowgirl_p2_vacuum_constantsuck_loop", 0f, 1f, 1f);
	}

	// Token: 0x06002190 RID: 8592 RVA: 0x00134C66 File Offset: 0x00133066
	private void SFX_COWGIRL_COWGIRL_P2_VacuumSuckLoopStop()
	{
		AudioManager.FadeSFXVolume("sfx_dlc_cowgirl_p2_vacuum_constantsuck_loop", 0f, 1f);
	}

	// Token: 0x06002191 RID: 8593 RVA: 0x00134C7C File Offset: 0x0013307C
	private void AnimationEvent_SFX_COWGIRL_COWGIRL_P2_VacuumSuckIn()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p2_vacuum_suckin");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p2_vacuum_suckin");
	}

	// Token: 0x06002192 RID: 8594 RVA: 0x00134C98 File Offset: 0x00133098
	private void AnimationEvent_SFX_COWGIRL_COWGIRL_P2_VacuumeExplosionDeath()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p2_death_vacuumexplosion_transition");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p2_death_vacuumexplosion_transition");
	}

	// Token: 0x06002193 RID: 8595 RVA: 0x00134CB4 File Offset: 0x001330B4
	private void SFX_COWGIRL_COWGIRL_P2_StirrupWheelsLoopStart()
	{
		AudioManager.PlayLoop("sfx_dlc_cowgirl_stirrupswheels_loop");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_stirrupswheels_loop");
	}

	// Token: 0x06002194 RID: 8596 RVA: 0x00134CD0 File Offset: 0x001330D0
	private void SFX_COWGIRL_COWGIRL_P2_StirrupWheelsLoopStop()
	{
		AudioManager.Stop("sfx_dlc_cowgirl_stirrupswheels_loop");
	}

	// Token: 0x040029D2 RID: 10706
	private const int SNAKE_BULLET_COUNT = 2;

	// Token: 0x040029D3 RID: 10707
	private const int DEBRIS_SPAWN_COUNT = 6;

	// Token: 0x040029D4 RID: 10708
	private const int DEBRIS_CURVE_SPAWN_COUNT = 4;

	// Token: 0x040029D5 RID: 10709
	private static readonly int SaloonAnimatorLayer = 1;

	// Token: 0x040029D6 RID: 10710
	private static readonly int PosterAnimatorLayer = 2;

	// Token: 0x040029D7 RID: 10711
	private static readonly int DoorsAnimatorLayer = 3;

	// Token: 0x040029D8 RID: 10712
	private static readonly int WheelSmokeAnimatorLayer = 4;

	// Token: 0x040029D9 RID: 10713
	private static readonly int TransitionSmokeLayer = 5;

	// Token: 0x040029DA RID: 10714
	[SerializeField]
	private SpriteRenderer posterRenderer;

	// Token: 0x040029DB RID: 10715
	[SerializeField]
	private FlyingCowboyLevelUFO ufoPrefab;

	// Token: 0x040029DC RID: 10716
	[SerializeField]
	private FlyingCowboyLevelBird birdPrefab;

	// Token: 0x040029DD RID: 10717
	[SerializeField]
	private Transform birdStartPosition;

	// Token: 0x040029DE RID: 10718
	[SerializeField]
	private Transform birdEndPosition;

	// Token: 0x040029DF RID: 10719
	[SerializeField]
	private TriggerZone birdSafetyZone;

	// Token: 0x040029E0 RID: 10720
	[SerializeField]
	private FlyingCowboyLevelBackshot backshotPrefab;

	// Token: 0x040029E1 RID: 10721
	[SerializeField]
	private Transform[] snakeTopRoot;

	// Token: 0x040029E2 RID: 10722
	[SerializeField]
	private Transform[] snakeBottomRoot;

	// Token: 0x040029E3 RID: 10723
	[SerializeField]
	private FlyingCowboyLevelOilBlob oilBlobPrefab;

	// Token: 0x040029E4 RID: 10724
	[SerializeField]
	private Effect snakeOilMuzzleFXPrefab;

	// Token: 0x040029E5 RID: 10725
	[SerializeField]
	private GameObject cactus;

	// Token: 0x040029E6 RID: 10726
	[SerializeField]
	private Vector2 wobbleRadius;

	// Token: 0x040029E7 RID: 10727
	[SerializeField]
	private Vector2 wobbleDuration;

	// Token: 0x040029E8 RID: 10728
	[SerializeField]
	private GameObject saloonCollidersParent;

	// Token: 0x040029E9 RID: 10729
	[SerializeField]
	private SpriteRenderer lanternARenderer;

	// Token: 0x040029EA RID: 10730
	[SerializeField]
	private SpriteRenderer lanternBRenderer;

	// Token: 0x040029EB RID: 10731
	[SerializeField]
	private SpriteRenderer[] saloonTransitionDisableRenderers;

	// Token: 0x040029EC RID: 10732
	[SerializeField]
	private SpriteRenderer frontWheelRenderer;

	// Token: 0x040029ED RID: 10733
	[SerializeField]
	private SpriteRenderer backWheelRenderer;

	// Token: 0x040029EE RID: 10734
	[SerializeField]
	private FlyingCowboyLevelDebris[] smallVacuumDebrisPrefabs;

	// Token: 0x040029EF RID: 10735
	[SerializeField]
	private FlyingCowboyLevelDebris[] mediumVacuumDebrisPrefabs;

	// Token: 0x040029F0 RID: 10736
	[SerializeField]
	private FlyingCowboyLevelDebris[] largeVacuumDebrisPrefabs;

	// Token: 0x040029F1 RID: 10737
	[SerializeField]
	private FlyingCowboyLevelRicochetDebris ricochetPrefab;

	// Token: 0x040029F2 RID: 10738
	[SerializeField]
	private AbstractProjectile ricochetUpPrefab;

	// Token: 0x040029F3 RID: 10739
	[SerializeField]
	private Transform ricochetUpSpawnPoint;

	// Token: 0x040029F4 RID: 10740
	[SerializeField]
	private BasicProjectile coinProjectile;

	// Token: 0x040029F5 RID: 10741
	[SerializeField]
	private Transform pistolShootRoot;

	// Token: 0x040029F6 RID: 10742
	[SerializeField]
	private int debrisSpawnHorizontalSpacing = 140;

	// Token: 0x040029F7 RID: 10743
	[SerializeField]
	private Transform vacuumDebrisAimTransform;

	// Token: 0x040029F8 RID: 10744
	[SerializeField]
	private Transform vacuumDebrisDisappearTransform;

	// Token: 0x040029F9 RID: 10745
	[SerializeField]
	private Transform vacuumSpawnTop;

	// Token: 0x040029FA RID: 10746
	[SerializeField]
	private Transform vacuumSpawnBottom;

	// Token: 0x040029FB RID: 10747
	[SerializeField]
	private SpriteRenderer bigVacuumRenderer;

	// Token: 0x040029FC RID: 10748
	[SerializeField]
	private SpriteRenderer transitionVacuumRenderer;

	// Token: 0x040029FD RID: 10749
	[SerializeField]
	private SpriteRenderer regularVacuumRenderer;

	// Token: 0x040029FE RID: 10750
	[SerializeField]
	private SpriteRenderer bigHoseRenderer;

	// Token: 0x040029FF RID: 10751
	[SerializeField]
	private SpriteRenderer transitionHoseRenderer;

	// Token: 0x04002A00 RID: 10752
	[SerializeField]
	private SpriteRenderer regularHoseRenderer;

	// Token: 0x04002A01 RID: 10753
	[SerializeField]
	private SpriteRenderer phase2PuffARenderer;

	// Token: 0x04002A02 RID: 10754
	[SerializeField]
	private SpriteRenderer phase2PuffBRenderer;

	// Token: 0x04002A05 RID: 10757
	private bool introBirdTriggered;

	// Token: 0x04002A06 RID: 10758
	private FlyingCowboyLevelBird introBird;

	// Token: 0x04002A07 RID: 10759
	private bool phase2Trigger;

	// Token: 0x04002A08 RID: 10760
	private bool endTransitionTrigger;

	// Token: 0x04002A09 RID: 10761
	private bool phase3Trigger;

	// Token: 0x04002A0A RID: 10762
	private Vector3 initialSaloonPosition;

	// Token: 0x04002A0B RID: 10763
	private Vector2 wobbleTimeElapsed;

	// Token: 0x04002A0C RID: 10764
	private Vector3[] topPositions;

	// Token: 0x04002A0D RID: 10765
	private Vector3[] bottomPositions;

	// Token: 0x04002A0E RID: 10766
	private Vector3[] sidePositions;

	// Token: 0x04002A0F RID: 10767
	private Vector3[] topCurvePositions;

	// Token: 0x04002A10 RID: 10768
	private Vector3[] bottomCurvePositions;

	// Token: 0x04002A11 RID: 10769
	private Vector3 phase2BasePosition;

	// Token: 0x04002A12 RID: 10770
	private DamageDealer damageDealer;

	// Token: 0x04002A13 RID: 10771
	private DamageReceiver damageReceiver;

	// Token: 0x04002A14 RID: 10772
	private List<FlyingCowboyLevelDebris> allDebris;

	// Token: 0x04002A15 RID: 10773
	private FlyingCowboyLevelUFO ufo;

	// Token: 0x04002A16 RID: 10774
	private PatternString snakeOilShotsPerAttackString;

	// Token: 0x04002A17 RID: 10775
	private PatternString snakeOffsetString;

	// Token: 0x04002A18 RID: 10776
	private PatternString snakeWidthString;

	// Token: 0x04002A19 RID: 10777
	private PatternString backshotHighSpawnPosition;

	// Token: 0x04002A1A RID: 10778
	private PatternString backshotLowSpawnPosition;

	// Token: 0x04002A1B RID: 10779
	private PatternString backshotSpawnDelay;

	// Token: 0x04002A1C RID: 10780
	private PatternString backshotBulletParryable;

	// Token: 0x04002A1D RID: 10781
	private PatternString backshotAnticipationStartDistancePattern;

	// Token: 0x04002A1E RID: 10782
	private int debrisTopMainIndex;

	// Token: 0x04002A1F RID: 10783
	private int debrisBottomMainIndex;

	// Token: 0x04002A20 RID: 10784
	private int debrisSideMainIndex;

	// Token: 0x04002A21 RID: 10785
	private FlyingCowboyLevelCowboy.VacuumForce forcePlayer1;

	// Token: 0x04002A22 RID: 10786
	private FlyingCowboyLevelCowboy.VacuumForce forcePlayer2;

	// Token: 0x04002A23 RID: 10787
	private PatternString debrisCurveString;

	// Token: 0x04002A24 RID: 10788
	private PatternString debrisParryString;

	// Token: 0x04002A25 RID: 10789
	private PatternString ricochetParryString;

	// Token: 0x04002A26 RID: 10790
	private int nextSafeShoot;

	// Token: 0x04002A27 RID: 10791
	private bool posterFlyAwayTriggered;

	// Token: 0x04002A28 RID: 10792
	private Coroutine transitionVacuumAttackCoroutine;

	// Token: 0x04002A29 RID: 10793
	private Coroutine vacuumSizeCoroutine;

	// Token: 0x02000650 RID: 1616
	public enum State
	{
		// Token: 0x04002A2B RID: 10795
		Idle,
		// Token: 0x04002A2C RID: 10796
		Wait,
		// Token: 0x04002A2D RID: 10797
		SnakeAttack,
		// Token: 0x04002A2E RID: 10798
		BeamAttack,
		// Token: 0x04002A2F RID: 10799
		Vacuum,
		// Token: 0x04002A30 RID: 10800
		Ricochet,
		// Token: 0x04002A31 RID: 10801
		PhaseTrans
	}

	// Token: 0x02000651 RID: 1617
	public class VacuumForce : PlanePlayerMotor.Force
	{
		// Token: 0x06002196 RID: 8598 RVA: 0x00134D24 File Offset: 0x00133124
		public VacuumForce(PlanePlayerController player, Transform aimPointTransform, float strength, float timeToFullStrength) : base(Vector2.zero, true)
		{
			this.player = player;
			this.aimPointTransform = aimPointTransform;
			this.strength = strength;
			this.currentStrength = 0f;
			this.timeToFullStrength = timeToFullStrength;
			this.elapsedTime = 0f;
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06002197 RID: 8599 RVA: 0x00134D70 File Offset: 0x00133170
		public override Vector2 force
		{
			get
			{
				if (this.player == null)
				{
					return Vector2.zero;
				}
				return (this.player.center - this.aimPointTransform.position).normalized * this.currentStrength;
			}
		}

		// Token: 0x06002198 RID: 8600 RVA: 0x00134DC7 File Offset: 0x001331C7
		public void UpdateStrength(float deltaTime)
		{
			this.elapsedTime += deltaTime;
			this.currentStrength = Mathf.Lerp(0f, this.strength, this.elapsedTime / this.timeToFullStrength);
		}

		// Token: 0x04002A32 RID: 10802
		private PlanePlayerController player;

		// Token: 0x04002A33 RID: 10803
		private Transform aimPointTransform;

		// Token: 0x04002A34 RID: 10804
		private float strength;

		// Token: 0x04002A35 RID: 10805
		private float currentStrength;

		// Token: 0x04002A36 RID: 10806
		private float timeToFullStrength;

		// Token: 0x04002A37 RID: 10807
		private float elapsedTime;
	}
}
