using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000702 RID: 1794
public class OldManLevelGnomeLeader : LevelProperties.OldMan.Entity
{
	// Token: 0x0600267A RID: 9850 RVA: 0x00167C58 File Offset: 0x00166058
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.transform.SetPosition(null, new float?(this.baseHeight + Mathf.Sin(this.GetPosition() * 3.1415927f) * this.heightRange), null);
	}

	// Token: 0x0600267B RID: 9851 RVA: 0x00167CDA File Offset: 0x001660DA
	private void Update()
	{
		this.NontargetablePlatformCount();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600267C RID: 9852 RVA: 0x00167CF9 File Offset: 0x001660F9
	public override void LevelInit(LevelProperties.OldMan properties)
	{
		base.LevelInit(properties);
		this.parryThermometer.gameObject.SetActive(false);
		this.parryString = new PatternString(properties.CurrentState.gnomeLeader.shotParryString, true);
	}

	// Token: 0x0600267D RID: 9853 RVA: 0x00167D2F File Offset: 0x0016612F
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
		if (base.properties.CurrentHealth <= 0f)
		{
			this.StartDeath();
		}
	}

	// Token: 0x0600267E RID: 9854 RVA: 0x00167D5D File Offset: 0x0016615D
	private void StartDeath()
	{
		this.isAlive = false;
		this.coll.enabled = false;
		this.StopAllCoroutines();
		base.animator.SetTrigger("Dead");
		this.SFX_Death();
		AudioManager.Stop("sfx_dlc_omm_p3_ulcer_movement_loop");
	}

	// Token: 0x0600267F RID: 9855 RVA: 0x00167D98 File Offset: 0x00166198
	public void StartGnomeLeader()
	{
		LevelProperties.OldMan.GnomeLeader gnomeLeader = base.properties.CurrentState.gnomeLeader;
		this.isAlive = true;
		this.pit.SetActive(true);
		this.stomachPlatforms = new OldManLevelStomachPlatform[this.platformPositions.Length];
		for (int i = 0; i < this.stomachPlatforms.Length; i++)
		{
			this.stomachPlatforms[i] = UnityEngine.Object.Instantiate<OldManLevelStomachPlatform>(this.stomachPlatformPrefab);
			this.stomachPlatforms[i].transform.position = this.platformPositions[i].position;
			if (i < 3)
			{
				this.stomachPlatforms[i].FlipX();
			}
			this.stomachPlatforms[i].sparkAnimator = this.platformPositions[i].GetComponent<Animator>();
			this.stomachPlatforms[i].main = this;
		}
		base.StartCoroutine(this.moving_cr());
	}

	// Token: 0x06002680 RID: 9856 RVA: 0x00167E70 File Offset: 0x00166270
	public float GetPosition()
	{
		return Mathf.InverseLerp((float)Level.Current.Right - this.screenEdgeOffset, (float)Level.Current.Left + this.screenEdgeOffset, base.transform.position.x);
	}

	// Token: 0x06002681 RID: 9857 RVA: 0x00167EBC File Offset: 0x001662BC
	private IEnumerator moving_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		this.movingRight = MathUtils.RandomBool();
		AudioManager.Play("sfx_dlc_omm_p3_ulcer_introlaugh");
		base.animator.Play((!this.movingRight) ? "IntroRight" : "IntroLeft");
		yield return wait;
		if (!this.movingRight)
		{
			while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.99f)
			{
				yield return null;
			}
			base.animator.Play("Idle");
			base.animator.Update(0f);
			base.transform.localScale = new Vector3(1f, 1f);
		}
		else
		{
			yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		}
		this.SFX_MoveLoop();
		base.StartCoroutine(this.gnome_leader_cr());
		this.timeForScreenCross = base.properties.CurrentState.gnomeLeader.bossMoveTime;
		this.locationEnd = 0f;
		AnimationHelper animHelper = base.animator.GetComponent<AnimationHelper>();
		for (;;)
		{
			this.locationTime = 0f;
			this.locationStart = base.transform.position.x;
			if (this.movingRight)
			{
				this.locationEnd = (float)Level.Current.Right - this.screenEdgeOffset;
			}
			else
			{
				this.locationEnd = (float)Level.Current.Left + this.screenEdgeOffset;
			}
			while (this.locationTime < this.timeForScreenCross)
			{
				base.transform.SetPosition(new float?(this.GetXPositionAtTimeValue(this.locationTime)), new float?(this.baseHeight + Mathf.Sin(this.GetPosition() * 3.1415927f) * this.heightRange), null);
				base.transform.SetEulerAngles(null, null, new float?(Mathf.Lerp((float)((!this.movingRight) ? -7 : 7), (float)((!this.movingRight) ? 7 : -7), this.locationTime / this.timeForScreenCross)));
				this.locationTime += CupheadTime.FixedDelta;
				if (this.turnTrigger && this.locationTime / this.timeForScreenCross > 0.8f)
				{
					base.animator.SetTrigger("OnTurn");
					this.turning = true;
					this.turnTrigger = false;
				}
				if (PauseManager.state != PauseManager.State.Paused)
				{
					float num = Mathf.Sin(Mathf.InverseLerp(this.locationStart, this.locationEnd, base.transform.position.x) * 3.1415927f);
					animHelper.Speed = 1f + num * (this.topAnimSpeed / 24f - 1f);
					this.SFXLoopVolume = 0.0001f + num * 0.3f;
					if (!this.spitting)
					{
						AudioManager.FadeSFXVolume("sfx_dlc_omm_p3_ulcer_movement_loop", this.SFXLoopVolume, 1E-05f);
					}
				}
				yield return wait;
			}
			this.turnTrigger = true;
			base.transform.SetPosition(new float?(this.locationEnd), null, null);
			this.movingRight = !this.movingRight;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002682 RID: 9858 RVA: 0x00167ED8 File Offset: 0x001662D8
	private float GetXPositionAtTimeValue(float time)
	{
		if (time > this.timeForScreenCross)
		{
			float value = time % this.timeForScreenCross / this.timeForScreenCross;
			return EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, this.locationEnd, this.locationStart, value);
		}
		float value2 = time / this.timeForScreenCross;
		return EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, this.locationStart, this.locationEnd, value2);
	}

	// Token: 0x06002683 RID: 9859 RVA: 0x00167F34 File Offset: 0x00166334
	private void AniEvent_Turn()
	{
		base.transform.SetScale(new float?(-base.transform.localScale.x), null, null);
		this.turning = false;
	}

	// Token: 0x06002684 RID: 9860 RVA: 0x00167F80 File Offset: 0x00166380
	private int NontargetablePlatformCount()
	{
		int num = 0;
		for (int i = 0; i < this.stomachPlatforms.Length; i++)
		{
			if (!this.stomachPlatforms[i].isActivated || this.stomachPlatforms[i].isTargeted)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06002685 RID: 9861 RVA: 0x00167FD4 File Offset: 0x001663D4
	private IEnumerator gnome_leader_cr()
	{
		LevelProperties.OldMan.GnomeLeader p = base.properties.CurrentState.gnomeLeader;
		PatternString platformCountToRemove = new PatternString(p.platformParryString, true, true);
		PatternString shotDelayString = new PatternString(p.shotDelayString, true, true);
		this.sequenceMainIndex = UnityEngine.Random.Range(0, p.shotPlatformString.Length);
		int numOfPlatformsToDestroy = 0;
		int platformToTarget = 0;
		bool projectileSpawnsParryable = false;
		while (this.isAlive)
		{
			this.restartSequence = false;
			projectileSpawnsParryable = false;
			this.currentTongue = -1;
			this.sequenceMainIndex = (this.sequenceMainIndex + 1) % p.shotPlatformString.Length;
			PatternString shotPlatformString = new PatternString(p.shotPlatformString[this.sequenceMainIndex], true);
			shotPlatformString.SetSubStringIndex(-1);
			this.sequenceIndex = 0;
			for (int i = 0; i < 5; i++)
			{
				this.sequence[i] = shotPlatformString.PopInt();
			}
			numOfPlatformsToDestroy = platformCountToRemove.PopInt();
			while (!this.restartSequence)
			{
				yield return CupheadTime.WaitForSeconds(this, shotDelayString.PopFloat());
				if (this.NontargetablePlatformCount() < 5 && !this.restartSequence)
				{
					while (this.turning)
					{
						yield return null;
					}
					base.animator.SetTrigger("Spit");
					int count = 0;
					do
					{
						platformToTarget = this.sequence[this.sequenceIndex];
						this.sequenceIndex = (this.sequenceIndex + 1) % 5;
						count++;
					}
					while ((!this.stomachPlatforms[platformToTarget].isActivated || this.stomachPlatforms[platformToTarget].isTargeted) && count < 5);
					if (this.currentTongue == -1 && this.NontargetablePlatformCount() >= numOfPlatformsToDestroy)
					{
						projectileSpawnsParryable = true;
						this.currentTongue = platformToTarget;
						base.StartCoroutine(this.wait_to_parry_cr());
					}
					else
					{
						projectileSpawnsParryable = false;
					}
					this.SFX_PreSpit();
					while (!this.readyToSpit)
					{
						yield return null;
					}
					yield return base.StartCoroutine(this.shoot_cr(this.stomachPlatforms[platformToTarget], projectileSpawnsParryable));
				}
			}
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
		}
		yield break;
	}

	// Token: 0x06002686 RID: 9862 RVA: 0x00167FF0 File Offset: 0x001663F0
	private IEnumerator wait_to_parry_cr()
	{
		while (!this.parryThermometer.isActivated)
		{
			yield return null;
		}
		foreach (OldManLevelStomachPlatform oldManLevelStomachPlatform in this.stomachPlatforms)
		{
			oldManLevelStomachPlatform.ActivatePlatform();
		}
		this.restartSequence = true;
		yield break;
	}

	// Token: 0x06002687 RID: 9863 RVA: 0x0016800B File Offset: 0x0016640B
	public void SpawnParryable(Vector3 spawnPosition)
	{
		this.parryThermometer.transform.position = spawnPosition;
		this.parryThermometer.gameObject.SetActive(true);
	}

	// Token: 0x06002688 RID: 9864 RVA: 0x00168030 File Offset: 0x00166430
	private IEnumerator shoot_cr(OldManLevelStomachPlatform selectedPlatform, bool projectileSpawnsParryable)
	{
		float predictedPos = this.GetXPositionAtTimeValue(this.locationTime + 0.5416667f);
		this.isBehind = false;
		bool willBeMovingRight = this.movingRight;
		if (this.locationTime + 0.5416667f > this.timeForScreenCross)
		{
			willBeMovingRight = !willBeMovingRight;
		}
		if (willBeMovingRight)
		{
			this.isBehind = (predictedPos + 250f > selectedPlatform.transform.position.x);
		}
		else
		{
			this.isBehind = (predictedPos - 250f < selectedPlatform.transform.position.x);
		}
		if (this.turning)
		{
			this.isBehind = !this.isBehind;
		}
		string animationName;
		if (Mathf.Abs(predictedPos - selectedPlatform.transform.position.x) < 350f)
		{
			base.animator.SetTrigger((!this.isBehind) ? "SpitForwardClose" : "SpitBehindClose");
			animationName = ((!this.isBehind) ? "Spit_Forward_Close" : "Spit_Behind_Close");
			this.spitVFXAnimator.transform.localPosition = this.spitRoots[0].transform.localPosition;
		}
		else
		{
			base.animator.SetTrigger((!this.isBehind) ? "SpitForward" : "SpitBehind");
			animationName = ((!this.isBehind) ? "Spit_Forward" : "Spit_Behind");
			this.spitVFXAnimator.transform.localPosition = this.spitRoots[1].transform.localPosition;
		}
		yield return base.animator.WaitForAnimationToStart(this, animationName, false);
		this.spitting = true;
		AudioManager.FadeSFXVolume("sfx_dlc_omm_p3_ulcer_movement_loop", Mathf.Min(0.25f, this.SFXLoopVolume), 0.25f);
		this.SFX_StartSpit();
		this.readyToSpit = false;
		while (!this.spitFrame)
		{
			yield return null;
		}
		LevelProperties.OldMan.GnomeLeader p = base.properties.CurrentState.gnomeLeader;
		this.spitVFXAnimator.transform.localPosition = new Vector3(Mathf.Abs(this.spitVFXAnimator.transform.localPosition.x) * (float)((!this.isBehind) ? -1 : 1), this.spitVFXAnimator.transform.localPosition.y);
		this.spitVFXAnimator.transform.localScale = new Vector3(Mathf.Sign(this.spitVFXAnimator.transform.localPosition.x), 1f);
		Vector3 startPos = this.spitVFXAnimator.transform.position;
		float x = selectedPlatform.transform.position.x - startPos.x;
		float y = selectedPlatform.transform.position.y - startPos.y;
		float timeToApex = p.shotApexTime;
		float height = p.shotApexHeight;
		float apexTime2 = timeToApex * timeToApex;
		float g = -2f * height / apexTime2;
		float viY = 2f * height / timeToApex;
		float viX2 = viY * viY;
		float sqrtRooted = viX2 + 2f * g * y;
		float tEnd = (-viY + Mathf.Sqrt(sqrtRooted)) / g;
		float tEnd2 = (-viY - Mathf.Sqrt(sqrtRooted)) / g;
		float tEnd3 = Mathf.Max(tEnd, tEnd2);
		float velocityX = x / tEnd3;
		Vector3 speed = new Vector3(velocityX, viY);
		selectedPlatform.Anticipation();
		this.spitVFXAnimator.Play("SpitSmoke");
		this.spitVFXAnimator.Update(0f);
		OldManLevelGnomeProjectile projectile = this.projectilePrefab.Spawn<OldManLevelGnomeProjectile>();
		projectile.Init(startPos, speed, g, projectileSpawnsParryable, this.parryString.PopLetter() == 'P' && !projectileSpawnsParryable, selectedPlatform);
		this.SFX_SpawnProjectile();
		this.spitFrame = false;
		AudioManager.FadeSFXVolume("sfx_dlc_omm_p3_ulcer_movement_loop", this.SFXLoopVolume, 1f);
		this.spitting = false;
		yield break;
	}

	// Token: 0x06002689 RID: 9865 RVA: 0x00168059 File Offset: 0x00166459
	private void AniEvent_ReadyToSpit()
	{
		this.readyToSpit = true;
	}

	// Token: 0x0600268A RID: 9866 RVA: 0x00168062 File Offset: 0x00166462
	private void AniEvent_Shoot()
	{
		this.spitFrame = true;
	}

	// Token: 0x0600268B RID: 9867 RVA: 0x0016806C File Offset: 0x0016646C
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (this.platformPositions != null)
		{
			foreach (Transform transform in this.platformPositions)
			{
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(transform.position, 50f);
			}
		}
	}

	// Token: 0x0600268C RID: 9868 RVA: 0x001680C3 File Offset: 0x001664C3
	private void SFX_MoveLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_omm_p3_ulcer_movement_loop");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_ulcer_movement_loop");
	}

	// Token: 0x0600268D RID: 9869 RVA: 0x001680DF File Offset: 0x001664DF
	private void SFX_PreSpit()
	{
		AudioManager.Play("sfx_dlc_omm_p3_ulcer_bonespitpre");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_ulcer_bonespitpre");
	}

	// Token: 0x0600268E RID: 9870 RVA: 0x001680FB File Offset: 0x001664FB
	private void SFX_StartSpit()
	{
		AudioManager.Play("sfx_dlc_omm_p3_ulcer_spitbonevocal");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_ulcer_spitbonevocal");
	}

	// Token: 0x0600268F RID: 9871 RVA: 0x00168117 File Offset: 0x00166517
	private void SFX_SpawnProjectile()
	{
		AudioManager.Play("sfx_dlc_omm_p3_ulcerspitbone");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_ulcerspitbone");
	}

	// Token: 0x06002690 RID: 9872 RVA: 0x00168133 File Offset: 0x00166533
	private void SFX_Death()
	{
		AudioManager.Play("sfx_dlc_omm_p3_ulcer_deathvocal");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_ulcer_deathvocal");
		AudioManager.FadeSFXVolume("sfx_dlc_omm_p3_stomachacid_amb_loop", 0f, 2f);
	}

	// Token: 0x04002F1E RID: 12062
	private const float DROP_Y = 5f;

	// Token: 0x04002F1F RID: 12063
	private const int BULLET_COUNT = 4;

	// Token: 0x04002F20 RID: 12064
	private const float SPIT_DELAY = 0.5416667f;

	// Token: 0x04002F21 RID: 12065
	private const float SPIT_DISTANCE_OFFSET = 250f;

	// Token: 0x04002F22 RID: 12066
	private const float CLOSE_SPIT_ANIM_RANGE = 350f;

	// Token: 0x04002F23 RID: 12067
	[SerializeField]
	private OldManLevelParryThermometer parryThermometer;

	// Token: 0x04002F24 RID: 12068
	public OldManLevelSplashHandler splashHandler;

	// Token: 0x04002F25 RID: 12069
	[SerializeField]
	private GameObject pit;

	// Token: 0x04002F26 RID: 12070
	[SerializeField]
	private Transform[] spitRoots;

	// Token: 0x04002F27 RID: 12071
	[SerializeField]
	private Animator spitVFXAnimator;

	// Token: 0x04002F28 RID: 12072
	[SerializeField]
	private OldManLevelGnomeProjectile projectilePrefab;

	// Token: 0x04002F29 RID: 12073
	[SerializeField]
	private OldManLevelStomachPlatform stomachPlatformPrefab;

	// Token: 0x04002F2A RID: 12074
	private OldManLevelStomachPlatform[] stomachPlatforms;

	// Token: 0x04002F2B RID: 12075
	private int currentTongue = -1;

	// Token: 0x04002F2C RID: 12076
	[SerializeField]
	public Transform[] platformPositions;

	// Token: 0x04002F2D RID: 12077
	private DamageDealer damageDealer;

	// Token: 0x04002F2E RID: 12078
	private DamageReceiver damageReceiver;

	// Token: 0x04002F2F RID: 12079
	private AbstractPlayerController player;

	// Token: 0x04002F30 RID: 12080
	private bool isAlive;

	// Token: 0x04002F31 RID: 12081
	private bool movingRight;

	// Token: 0x04002F32 RID: 12082
	private bool readyToSpit;

	// Token: 0x04002F33 RID: 12083
	private bool spitFrame;

	// Token: 0x04002F34 RID: 12084
	private bool spitting;

	// Token: 0x04002F35 RID: 12085
	private bool restartSequence;

	// Token: 0x04002F36 RID: 12086
	private int[] sequence = new int[5];

	// Token: 0x04002F37 RID: 12087
	private int sequenceIndex;

	// Token: 0x04002F38 RID: 12088
	private int sequenceMainIndex;

	// Token: 0x04002F39 RID: 12089
	private float locationTime;

	// Token: 0x04002F3A RID: 12090
	private float locationStart;

	// Token: 0x04002F3B RID: 12091
	private float locationEnd;

	// Token: 0x04002F3C RID: 12092
	private float timeForScreenCross;

	// Token: 0x04002F3D RID: 12093
	private bool turnTrigger = true;

	// Token: 0x04002F3E RID: 12094
	private bool turning;

	// Token: 0x04002F3F RID: 12095
	private PatternString parryString;

	// Token: 0x04002F40 RID: 12096
	private bool isBehind;

	// Token: 0x04002F41 RID: 12097
	private float screenEdgeOffset = 200f;

	// Token: 0x04002F42 RID: 12098
	[SerializeField]
	private float baseHeight = 188f;

	// Token: 0x04002F43 RID: 12099
	[SerializeField]
	private float topAnimSpeed = 30f;

	// Token: 0x04002F44 RID: 12100
	[SerializeField]
	private float heightRange;

	// Token: 0x04002F45 RID: 12101
	[SerializeField]
	private Collider2D coll;

	// Token: 0x04002F46 RID: 12102
	private float SFXLoopVolume;
}
