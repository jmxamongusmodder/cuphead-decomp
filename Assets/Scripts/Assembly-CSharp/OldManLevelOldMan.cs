using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000706 RID: 1798
public class OldManLevelOldMan : LevelProperties.OldMan.Entity
{
	// Token: 0x170003CC RID: 972
	// (get) Token: 0x060026A6 RID: 9894 RVA: 0x00169BE6 File Offset: 0x00167FE6
	// (set) Token: 0x060026A7 RID: 9895 RVA: 0x00169BEE File Offset: 0x00167FEE
	public OldManLevelOldMan.State state { get; private set; }

	// Token: 0x060026A8 RID: 9896 RVA: 0x00169BF7 File Offset: 0x00167FF7
	private void Start()
	{
		this.sprite = base.GetComponent<SpriteRenderer>();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x060026A9 RID: 9897 RVA: 0x00169C33 File Offset: 0x00168033
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060026AA RID: 9898 RVA: 0x00169C4B File Offset: 0x0016804B
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x060026AB RID: 9899 RVA: 0x00169C60 File Offset: 0x00168060
	public override void LevelInit(LevelProperties.OldMan properties)
	{
		base.LevelInit(properties);
		LevelProperties.OldMan.SpitAttack spitAttack = properties.CurrentState.spitAttack;
		this.spitStringMainIndex = UnityEngine.Random.Range(0, spitAttack.spitString.Length);
		this.gooseSpawnString = new PatternString(properties.CurrentState.gooseAttack.gooseSpawnString, ':', true, true);
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x060026AC RID: 9900 RVA: 0x00169CC0 File Offset: 0x001680C0
	private IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 3f);
		this.state = OldManLevelOldMan.State.Idle;
		yield break;
	}

	// Token: 0x060026AD RID: 9901 RVA: 0x00169CDB File Offset: 0x001680DB
	private void AniEvent_CameraRumble()
	{
		CupheadLevelCamera.Current.Shake(5f, 0.66f, false);
	}

	// Token: 0x060026AE RID: 9902 RVA: 0x00169CF2 File Offset: 0x001680F2
	private void AniEvent_CameraShake()
	{
		CupheadLevelCamera.Current.Shake(30f, 1.2f, false);
	}

	// Token: 0x060026AF RID: 9903 RVA: 0x00169D09 File Offset: 0x00168109
	private void ChangeColor(Color color)
	{
		this.sprite.color = color;
	}

	// Token: 0x060026B0 RID: 9904 RVA: 0x00169D17 File Offset: 0x00168117
	public void Spit()
	{
		base.StartCoroutine(this.spit_cr());
	}

	// Token: 0x060026B1 RID: 9905 RVA: 0x00169D28 File Offset: 0x00168128
	private IEnumerator spit_cr()
	{
		AudioManager.FadeSFXVolume("sfx_dlc_omm_mouthcauldron_stirring_loop_start", 0f, 0.0001f);
		this.state = OldManLevelOldMan.State.Spit;
		base.animator.SetBool(OldManLevelOldMan.IsSpitAttackParameterID, true);
		LevelProperties.OldMan.SpitAttack p = base.properties.CurrentState.spitAttack;
		string[] spitString = p.spitString[this.spitStringMainIndex].Split(new char[]
		{
			','
		});
		PatternString spitParryString = new PatternString(p.spitParryString, true);
		yield return base.animator.WaitForAnimationToEnd(this, "Spit_Intro_Continued", false, true);
		float height = p.spitApexHeight;
		float apexTime2 = p.spitApexTime * p.spitApexTime;
		float g = -2f * height / apexTime2;
		float viX = 2f * height / p.spitApexTime;
		float viY2 = viX * viX;
		float endPosX = this.spitEndArc.position.x;
		float endPosY = 0f;
		Vector3 startPosition = Vector3.zero;
		Vector3 endPosition = Vector3.zero;
		for (int i = 0; i < spitString.Length; i++)
		{
			if (this.endPhaseOne)
			{
				break;
			}
			Parser.FloatTryParse(spitString[i], out endPosY);
			startPosition = this.spitRoot.transform.position + Vector3.right * (float)UnityEngine.Random.Range(-15, 15);
			endPosition = new Vector3(endPosX, endPosY);
			float x = endPosition.x - startPosition.x;
			float y = endPosition.y - startPosition.y;
			float sqrtRooted = viY2 + 2f * g * x;
			float tEnd = (-viX + Mathf.Sqrt(sqrtRooted)) / g;
			float tEnd2 = (-viX - Mathf.Sqrt(sqrtRooted)) / g;
			float tEnd3 = Mathf.Max(tEnd, tEnd2);
			float velocityY = y / tEnd3;
			OldManLevelSpitProjectile projectile = (spitParryString.PopLetter() != 'P') ? (this.spitProjectile.Create() as OldManLevelSpitProjectile) : (this.spitProjectilePink.Create() as OldManLevelSpitProjectile);
			projectile.Move(startPosition, viX, velocityY, this.spitEndArc.position.x, g, p.spitApexTime, i % 4);
			if (!this.endPhaseOne)
			{
				yield return CupheadTime.WaitForSeconds(this, p.spitDelay);
			}
		}
		base.animator.SetBool(OldManLevelOldMan.IsSpitAttackParameterID, false);
		this.spitStringMainIndex = (this.spitStringMainIndex + 1) % p.spitString.Length;
		yield return base.animator.WaitForAnimationToEnd(this, "Spit_Outro", false, true);
		float t = 0f;
		while (t < p.attackCooldown && !this.endPhaseOne)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		this.state = OldManLevelOldMan.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x060026B2 RID: 9906 RVA: 0x00169D43 File Offset: 0x00168143
	public void Goose()
	{
		base.StartCoroutine(this.Goose_cr());
	}

	// Token: 0x060026B3 RID: 9907 RVA: 0x00169D54 File Offset: 0x00168154
	private IEnumerator Goose_cr()
	{
		LevelProperties.OldMan.GooseAttack p = base.properties.CurrentState.gooseAttack;
		this.state = OldManLevelOldMan.State.Goose;
		YieldInstruction wait = new WaitForFixedUpdate();
		base.animator.SetBool(OldManLevelOldMan.IsGooseAttackParameterID, true);
		int targetAnimHash = Animator.StringToHash(base.animator.GetLayerName(0) + ".Goose_Atk_Anti");
		int idleOneAnimHash = Animator.StringToHash(base.animator.GetLayerName(0) + ".Idle_Part_1");
		int idleTwoAnimHash = Animator.StringToHash(base.animator.GetLayerName(0) + ".Idle_Part_2");
		while (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash != targetAnimHash && !this.endPhaseOne)
		{
			yield return null;
		}
		if (this.endPhaseOne && (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == idleOneAnimHash || base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == idleTwoAnimHash))
		{
			base.animator.SetBool(OldManLevelOldMan.IsGooseAttackParameterID, false);
			yield break;
		}
		float t = 0f;
		while (t < p.goosePreAntic && !this.endPhaseOne)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetTrigger(OldManLevelOldMan.ContinueParameterID);
		t = 0f;
		while (t < p.gooseWarning && !this.endPhaseOne)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		bool spawningGeese = true;
		float geeseDurationTimer = 0f;
		float geeseDelayTimer = 0f;
		float geeseDelayMaxTime = this.gooseSpawnString.GetSubsubstringFloat(0);
		float xPos = 840f;
		float speed = 0f;
		while (spawningGeese && !this.endPhaseOne)
		{
			geeseDurationTimer += CupheadTime.FixedDelta;
			geeseDelayTimer += CupheadTime.FixedDelta;
			if (geeseDelayTimer >= geeseDelayMaxTime)
			{
				OldManLevelGoose oldManLevelGoose = this.goosePrefab.Create() as OldManLevelGoose;
				bool hasCollision = false;
				string sortingLayer = "Default";
				int sortingOrder = 100;
				float whiten = 0f;
				char subsubstringLetter = this.gooseSpawnString.GetSubsubstringLetter(1);
				switch (subsubstringLetter)
				{
				case 'B':
					oldManLevelGoose.transform.localScale = new Vector3(0.655f, 0.655f);
					speed = p.gooseBSpeed;
					sortingLayer = "Background";
					sortingOrder = 1000;
					whiten = 0.15f;
					break;
				case 'C':
					speed = p.gooseCSpeed;
					hasCollision = true;
					break;
				default:
					if (subsubstringLetter == 'M')
					{
						oldManLevelGoose.transform.localScale = new Vector3(0.848f, 0.848f);
						speed = p.gooseMSpeed;
						hasCollision = true;
						sortingOrder = -100;
						whiten = 0.05f;
					}
					break;
				case 'F':
					oldManLevelGoose.transform.localScale = new Vector3(1.4414f, 1.4414f);
					speed = p.gooseFSpeed;
					sortingLayer = "Foreground";
					whiten = 0.85f;
					break;
				}
				Vector3 v = new Vector3(xPos, this.gooseSpawnString.GetSubsubstringFloat(2));
				oldManLevelGoose.Init(v, speed, p, hasCollision, sortingLayer, sortingOrder, whiten);
				geeseDelayTimer = 0f;
				geeseDelayMaxTime = this.gooseSpawnString.GetSubsubstringFloat(0);
				this.gooseSpawnString.IncrementString();
			}
			if (geeseDurationTimer >= p.gooseDuration)
			{
				spawningGeese = false;
			}
			yield return wait;
		}
		base.animator.SetBool(OldManLevelOldMan.IsGooseAttackParameterID, false);
		yield return base.animator.WaitForAnimationToEnd(this, "Goose_Atk_End", false, true);
		t = 0f;
		while (t < p.gooseCooldown && !this.endPhaseOne)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		this.state = OldManLevelOldMan.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x060026B4 RID: 9908 RVA: 0x00169D6F File Offset: 0x0016816F
	public void Bear()
	{
		base.StartCoroutine(this.bear_cr());
	}

	// Token: 0x060026B5 RID: 9909 RVA: 0x00169D80 File Offset: 0x00168180
	private IEnumerator bear_cr()
	{
		LevelProperties.OldMan.CamelAttack p = base.properties.CurrentState.camelAttack;
		this.state = OldManLevelOldMan.State.Bear;
		base.animator.SetBool(OldManLevelOldMan.IsBearAttackParameterID, true);
		int targetAnimHash = Animator.StringToHash(base.animator.GetLayerName(0) + ".Bear_Atk_Start");
		int targetAnimHashAlt = Animator.StringToHash(base.animator.GetLayerName(0) + ".Bear_Atk_Start_F10");
		int idleOneAnimHash = Animator.StringToHash(base.animator.GetLayerName(0) + ".Idle_Part_1");
		int idleTwoAnimHash = Animator.StringToHash(base.animator.GetLayerName(0) + ".Idle_Part_2");
		while (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash != targetAnimHash && base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash != targetAnimHashAlt && !this.endPhaseOne)
		{
			yield return null;
		}
		if (this.endPhaseOne && (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == idleOneAnimHash || base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == idleTwoAnimHash))
		{
			base.animator.SetBool(OldManLevelOldMan.IsBearAttackParameterID, false);
			yield break;
		}
		yield return base.animator.WaitForAnimationToStart(this, "Bear_Atk_Anti", false);
		Animator bearAni = this.bearBeam.GetComponent<Animator>();
		this.bearBeam.gameObject.SetActive(true);
		this.bearBeam.transform.position = new Vector3(-1300f, 100f);
		this.bearBeam.thrown = false;
		float t = 0f;
		while (t < p.camelAttackWarning && !this.endPhaseOne)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetTrigger(OldManLevelOldMan.ContinueParameterID);
		t = 0f;
		yield return base.animator.WaitForNormalizedTime(this, 1.2916666f, "Bear_Atk_Cont", 0, false, false, true);
		yield return CupheadTime.WaitForSeconds(this, (!this.endPhaseOne) ? p.camelOffScreenTime : 0.5f);
		float endPoint = (!this.endPhaseOne) ? p.endingPoint : -990f;
		bool exiting = false;
		YieldInstruction wait = new WaitForFixedUpdate();
		bearAni.Play("Idle");
		while (this.bearBeam.transform.position.x < endPoint && !this.bearBeam.thrown)
		{
			if ((this.endPhaseOne || this.bearBeam.transform.position.x > p.boredomPoint) && !exiting)
			{
				exiting = true;
				base.StartCoroutine(this.exit_bear_cr());
			}
			this.bearBeam.transform.position += Vector3.right * p.camelAttackSpeed * CupheadTime.FixedDelta;
			yield return wait;
		}
		if (!exiting)
		{
			yield return base.StartCoroutine(this.exit_bear_cr());
		}
		t = 0f;
		while (t < p.camelAttackCooldown && !this.endPhaseOne)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		this.state = OldManLevelOldMan.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x060026B6 RID: 9910 RVA: 0x00169D9C File Offset: 0x0016819C
	private IEnumerator exit_bear_cr()
	{
		Animator bearAni = this.bearBeam.GetComponent<Animator>();
		base.animator.SetBool(OldManLevelOldMan.IsBearAttackParameterID, false);
		yield return base.animator.WaitForAnimationToStart(this, "Bear_Atk_End_1", false);
		bearAni.SetTrigger("OnExit");
		yield return bearAni.WaitForAnimationToEnd(this, "End", false, true);
		this.bearBeam.StartCoroutine(this.bearBeam.fall_cr());
		base.animator.SetTrigger(OldManLevelOldMan.ContinueParameterID);
		yield return base.animator.WaitForAnimationToEnd(this, "Bear_Atk_End", false, false);
		yield break;
	}

	// Token: 0x060026B7 RID: 9911 RVA: 0x00169DB7 File Offset: 0x001681B7
	public void EndPhase1()
	{
		base.animator.SetBool("Phase2", true);
		this.endPhaseOne = true;
	}

	// Token: 0x060026B8 RID: 9912 RVA: 0x00169DD1 File Offset: 0x001681D1
	private void AniEvent_EndPhase1BeardBoil()
	{
		this.rightWall.SetActive(false);
		base.animator.Play("None", 1);
	}

	// Token: 0x060026B9 RID: 9913 RVA: 0x00169DF0 File Offset: 0x001681F0
	private void AniEvent_ActivatePhase2Beard()
	{
		((OldManLevel)Level.Current).ActivatePhase2Beard();
		base.GetComponent<SpriteRenderer>().sortingOrder = -1;
	}

	// Token: 0x060026BA RID: 9914 RVA: 0x00169E0D File Offset: 0x0016820D
	public void OnPhase2()
	{
		this.sockPuppets.StartPhase2();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060026BB RID: 9915 RVA: 0x00169E25 File Offset: 0x00168225
	private void animationEvent_PlayGooseFX()
	{
		this.gooseFXAnimator.Play("FX");
	}

	// Token: 0x060026BC RID: 9916 RVA: 0x00169E37 File Offset: 0x00168237
	private void animationEvent_IdleBlinkStart()
	{
		if (this.shouldIdleBlink)
		{
			this.eyeRenderer.enabled = true;
		}
		this.shouldIdleBlink = !this.shouldIdleBlink;
	}

	// Token: 0x060026BD RID: 9917 RVA: 0x00169E5F File Offset: 0x0016825F
	private void animationEvent_IdleBlinkEnd()
	{
		this.eyeRenderer.enabled = false;
	}

	// Token: 0x060026BE RID: 9918 RVA: 0x00169E6D File Offset: 0x0016826D
	private void animationEvent_BeginCauldron()
	{
		this.cauldron.SetActive(true);
	}

	// Token: 0x060026BF RID: 9919 RVA: 0x00169E7B File Offset: 0x0016827B
	private void animationEvent_EndCauldron()
	{
		this.cauldron.SetActive(false);
	}

	// Token: 0x060026C0 RID: 9920 RVA: 0x00169E89 File Offset: 0x00168289
	private void animationEvent_BeginSpitEyes()
	{
		this.cauldronEyes.SetActive(true);
	}

	// Token: 0x060026C1 RID: 9921 RVA: 0x00169E97 File Offset: 0x00168297
	private void animationEvent_EndSpitEyes()
	{
		this.cauldronEyes.SetActive(false);
	}

	// Token: 0x060026C2 RID: 9922 RVA: 0x00169EA5 File Offset: 0x001682A5
	private void AnimationEvent_SFX_OMM_Intro()
	{
		AudioManager.Play("sfx_dlc_omm_intro");
		this.emitAudioFromObject.Add("sfx_dlc_omm_intro");
	}

	// Token: 0x060026C3 RID: 9923 RVA: 0x00169EC1 File Offset: 0x001682C1
	private void AnimationEvent_SFX_OMM_IntroPickaxe()
	{
		AudioManager.Play("sfx_dlc_omm_intropickaxe");
		this.emitAudioFromObject.Add("sfx_dlc_omm_intropickaxe");
	}

	// Token: 0x060026C4 RID: 9924 RVA: 0x00169EDD File Offset: 0x001682DD
	private void AnimationEvent_SFX_OMM_GooseStormIntro()
	{
		AudioManager.Play("sfx_dlc_omm_goosestorm");
		this.emitAudioFromObject.Add("sfx_dlc_omm_goosestorm");
	}

	// Token: 0x060026C5 RID: 9925 RVA: 0x00169EF9 File Offset: 0x001682F9
	private void AnimationEvent_SFX_OMM_GooseStormLoop()
	{
		base.StartCoroutine(this.SFX_OMM_GooseStormLoop_cr());
	}

	// Token: 0x060026C6 RID: 9926 RVA: 0x00169F08 File Offset: 0x00168308
	private IEnumerator SFX_OMM_GooseStormLoop_cr()
	{
		yield return new WaitForEndOfFrame();
		AudioManager.PlayLoop("sfx_dlc_omm_goosestorm_loop");
		this.emitAudioFromObject.Add("sfx_dlc_omm_goosestorm_loop");
		yield break;
	}

	// Token: 0x060026C7 RID: 9927 RVA: 0x00169F23 File Offset: 0x00168323
	private void AnimationEvent_SFX_OMM_GooseStormLoopEnd()
	{
		AudioManager.Stop("sfx_dlc_omm_goosestorm_loop");
		AudioManager.Play("sfx_dlc_omm_goosestorm_loop_end");
		this.emitAudioFromObject.Add("sfx_dlc_omm_goosestorm_loop_end");
	}

	// Token: 0x060026C8 RID: 9928 RVA: 0x00169F49 File Offset: 0x00168349
	private void AnimationEvent_SFX_OMM_MouthCauldron_MouthClose()
	{
		AudioManager.Play("sfx_dlc_omm_mouthcauldron_mouthclose");
		this.emitAudioFromObject.Add("sfx_dlc_omm_mouthcauldron_mouthclose");
		AudioManager.Stop("sfx_dlc_omm_mouthcauldron_stirring_loop");
	}

	// Token: 0x060026C9 RID: 9929 RVA: 0x00169F70 File Offset: 0x00168370
	private void AnimationEvent_SFX_OMM_MouthCauldron_MouthOpen()
	{
		AudioManager.Play("sfx_dlc_omm_mouthcauldron_mouthopen");
		this.emitAudioFromObject.Add("sfx_dlc_omm_mouthcauldron_mouthopen");
		AudioManager.FadeSFXVolume("sfx_dlc_omm_mouthcauldron_stirring_loop_start", 1f, 0.1f);
		AudioManager.Play("sfx_dlc_omm_mouthcauldron_stirring_loop_start");
		this.emitAudioFromObject.Add("sfx_dlc_omm_mouthcauldron_stirring_loop_start");
		AudioManager.PlayLoop("sfx_dlc_omm_mouthcauldron_stirring_loop");
		this.emitAudioFromObject.Add("sfx_dlc_omm_mouthcauldron_stirring_loop");
	}

	// Token: 0x060026CA RID: 9930 RVA: 0x00169FDF File Offset: 0x001683DF
	private void AnimationEvent_SFX_OMM_BearAttackOMMStartVocal()
	{
		AudioManager.Play("sfx_dlc_omm_bearattack_ommstartvocal");
		this.emitAudioFromObject.Add("sfx_dlc_omm_bearattack_ommstartvocal");
	}

	// Token: 0x060026CB RID: 9931 RVA: 0x00169FFB File Offset: 0x001683FB
	private void AnimationEvent_SFX_OMM_BearAttackStart()
	{
		AudioManager.Play("sfx_dlc_omm_bearattack_start");
		this.emitAudioFromObject.Add("sfx_dlc_omm_bearattack_start");
	}

	// Token: 0x060026CC RID: 9932 RVA: 0x0016A017 File Offset: 0x00168417
	private void AnimationEvent_SFX_OMM_P2_OMMVocalFrustrated()
	{
		AudioManager.Play("sfx_dlc_omm_p2_end_ommvocalfrustrated");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_end_ommvocalfrustrated");
	}

	// Token: 0x060026CD RID: 9933 RVA: 0x0016A033 File Offset: 0x00168433
	private void AnimationEvent_SFX_OMM_P2_TransitionBeardPull()
	{
		AudioManager.Play("sfx_dlc_omm_p2_transition_pullbeardoff");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_transition_pullbeardoff");
	}

	// Token: 0x04002F5D RID: 12125
	private static readonly int IsSpitAttackParameterID = Animator.StringToHash("IsSpitAttack");

	// Token: 0x04002F5E RID: 12126
	private static readonly int IsSpitAttackEyeLoopParameterID = Animator.StringToHash("IsSpitAttackEyeLoop");

	// Token: 0x04002F5F RID: 12127
	private static readonly int IsGooseAttackParameterID = Animator.StringToHash("IsGooseAttack");

	// Token: 0x04002F60 RID: 12128
	private static readonly int ContinueParameterID = Animator.StringToHash("Continue");

	// Token: 0x04002F61 RID: 12129
	private static readonly int IsBearAttackParameterID = Animator.StringToHash("IsBearAttack");

	// Token: 0x04002F62 RID: 12130
	private const float DUCK_MOVE_END = -165f;

	// Token: 0x04002F63 RID: 12131
	private const float BEAR_START_X = -1300f;

	// Token: 0x04002F64 RID: 12132
	private const float BEAR_Y = 100f;

	// Token: 0x04002F65 RID: 12133
	[SerializeField]
	private OldManLevelGoose goosePrefab;

	// Token: 0x04002F66 RID: 12134
	[SerializeField]
	private OldManLevelBear bearBeam;

	// Token: 0x04002F67 RID: 12135
	[SerializeField]
	private Transform spitRoot;

	// Token: 0x04002F68 RID: 12136
	[SerializeField]
	private Transform spitEndArc;

	// Token: 0x04002F69 RID: 12137
	[SerializeField]
	private OldManLevelSpitProjectile spitProjectile;

	// Token: 0x04002F6A RID: 12138
	[SerializeField]
	private OldManLevelSpitProjectile spitProjectilePink;

	// Token: 0x04002F6B RID: 12139
	[SerializeField]
	private OldManLevelPlatformManager platformManager;

	// Token: 0x04002F6C RID: 12140
	[SerializeField]
	private OldManLevelSockPuppetHandler sockPuppets;

	// Token: 0x04002F6D RID: 12141
	[SerializeField]
	private SpriteRenderer eyeRenderer;

	// Token: 0x04002F6E RID: 12142
	[SerializeField]
	private GameObject cauldron;

	// Token: 0x04002F6F RID: 12143
	[SerializeField]
	private GameObject cauldronEyes;

	// Token: 0x04002F70 RID: 12144
	[SerializeField]
	private Animator gooseFXAnimator;

	// Token: 0x04002F71 RID: 12145
	[SerializeField]
	private GameObject rightWall;

	// Token: 0x04002F73 RID: 12147
	private SpriteRenderer sprite;

	// Token: 0x04002F74 RID: 12148
	private DamageReceiver damageReceiver;

	// Token: 0x04002F75 RID: 12149
	private DamageDealer damageDealer;

	// Token: 0x04002F76 RID: 12150
	private int spitStringMainIndex;

	// Token: 0x04002F77 RID: 12151
	private bool shouldIdleBlink;

	// Token: 0x04002F78 RID: 12152
	private bool endPhaseOne;

	// Token: 0x04002F79 RID: 12153
	private PatternString gooseSpawnString;

	// Token: 0x02000707 RID: 1799
	public enum State
	{
		// Token: 0x04002F7B RID: 12155
		Intro,
		// Token: 0x04002F7C RID: 12156
		Idle,
		// Token: 0x04002F7D RID: 12157
		Spit,
		// Token: 0x04002F7E RID: 12158
		Goose,
		// Token: 0x04002F7F RID: 12159
		Bear
	}
}
