using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007EC RID: 2028
public class SnowCultLevelJackFrost : LevelProperties.SnowCult.Entity
{
	// Token: 0x06002E74 RID: 11892 RVA: 0x001B633C File Offset: 0x001B473C
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.properties.OnBossDeath += this.OnBossDeath;
		this.rend = base.GetComponent<SpriteRenderer>();
		this.blinkCount = UnityEngine.Random.Range(1, 5);
	}

	// Token: 0x06002E75 RID: 11893 RVA: 0x001B63AD File Offset: 0x001B47AD
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002E76 RID: 11894 RVA: 0x001B63C5 File Offset: 0x001B47C5
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06002E77 RID: 11895 RVA: 0x001B63D8 File Offset: 0x001B47D8
	public void Intro()
	{
		this.state = SnowCultLevelJackFrost.States.Intro;
	}

	// Token: 0x06002E78 RID: 11896 RVA: 0x001B63E4 File Offset: 0x001B47E4
	public void StartPhase3()
	{
		this.bucket.SetActive(true);
		base.gameObject.SetActive(true);
		this.scale = base.transform.localScale;
		this.positionX = base.transform.position.x;
		this.onRight = Rand.Bool();
		this.ChangeSide();
		this.rightSideUp = true;
		this.faceOrientation = new PatternString(base.properties.CurrentState.face.faceOrientationString, true, true);
		this.splitShotPink = new PatternString(base.properties.CurrentState.splitShot.pinkString, true, true);
		this.shotCoord = new PatternString(base.properties.CurrentState.splitShot.shotCoordString, true, true);
		this.splitShotPink.SetSubStringIndex(-1);
		this.shotCoord.SetSubStringIndex(-1);
		this.shardAngleOffsetString = new PatternString(base.properties.CurrentState.shardAttack.angleOffset, true);
		base.StartCoroutine(this.remove_platforms_cr());
	}

	// Token: 0x06002E79 RID: 11897 RVA: 0x001B64F8 File Offset: 0x001B48F8
	private IEnumerator remove_platforms_cr()
	{
		int count = this.presetPlatforms.Length;
		while (count > 0)
		{
			foreach (SnowCultLevelPlatform snowCultLevelPlatform in this.presetPlatforms)
			{
				if (snowCultLevelPlatform != null && snowCultLevelPlatform.transform.position.y < Camera.main.transform.position.y - 450f)
				{
					snowCultLevelPlatform.transform.DetachChildren();
					UnityEngine.Object.Destroy(snowCultLevelPlatform.gameObject);
					count--;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002E7A RID: 11898 RVA: 0x001B6513 File Offset: 0x001B4913
	private void EndIntro()
	{
		this.state = SnowCultLevelJackFrost.States.Idle;
		this.boxCollider.enabled = true;
	}

	// Token: 0x06002E7B RID: 11899 RVA: 0x001B6528 File Offset: 0x001B4928
	public void CreatePlatforms()
	{
		this.presetPlatforms = new SnowCultLevelPlatform[this.platformsPresetPositions.Length];
		this.isClockwise = Rand.Bool();
		LevelProperties.SnowCult.Platforms platforms = base.properties.CurrentState.platforms;
		this.circlePlatforms = new SnowCultLevelPlatform[platforms.platformNum];
		float num = 360f / (float)platforms.platformNum;
		for (int i = 0; i < platforms.platformNum; i++)
		{
			this.circlePlatforms[i] = UnityEngine.Object.Instantiate<GameObject>(this.platformPrefab).transform.GetChild(0).GetComponent<SnowCultLevelPlatform>();
			this.circlePlatforms[i].transform.parent.position = this.platformPivotPoint.transform.position;
			this.circlePlatforms[i].StartRotate(num * (float)i, new Vector3(this.platformPivotPoint.transform.position.x, this.platformPivotPoint.transform.position.y + platforms.pivotPointYOffset), platforms.loopSizeX, platforms.loopSizeY, platforms.platformSpeed, platforms.pivotPointYOffset, this.isClockwise);
			this.circlePlatforms[i].SetID(i);
		}
	}

	// Token: 0x06002E7C RID: 11900 RVA: 0x001B6660 File Offset: 0x001B4A60
	public void CreateAscendingPlatform(int i)
	{
		this.presetPlatforms[i] = UnityEngine.Object.Instantiate<GameObject>(this.platformPrefab).transform.GetChild(0).GetComponent<SnowCultLevelPlatform>();
		this.presetPlatforms[i].transform.parent.position = this.platformsPresetPositions[i].transform.position;
		this.presetPlatforms[i].SetID(i);
	}

	// Token: 0x06002E7D RID: 11901 RVA: 0x001B66C8 File Offset: 0x001B4AC8
	private void AniEvent_CheckBlink()
	{
		this.blinkCounter++;
		if (this.blinkCounter >= this.blinkCount)
		{
			base.animator.SetTrigger("Blink");
			this.blinkCount = UnityEngine.Random.Range(1, 5);
			this.blinkCounter = 0;
		}
	}

	// Token: 0x06002E7E RID: 11902 RVA: 0x001B6718 File Offset: 0x001B4B18
	public void StartSwitch()
	{
		if (this.firstAttack)
		{
			this.firstAttack = false;
		}
		else
		{
			base.StartCoroutine(this.switch_cr());
		}
	}

	// Token: 0x06002E7F RID: 11903 RVA: 0x001B6740 File Offset: 0x001B4B40
	private IEnumerator switch_cr()
	{
		this.state = SnowCultLevelJackFrost.States.Switch;
		LevelProperties.SnowCult.Face p = base.properties.CurrentState.face;
		bool flippedY = false;
		char c = this.faceOrientation.PopLetter();
		if ((c == 'U' && base.transform.parent.localScale.y == -1f) || (c == 'D' && base.transform.parent.localScale.y == 1f))
		{
			flippedY = true;
		}
		bool isFront = false;
		string triggerName;
		string stateName;
		if (UnityEngine.Random.Range(0f, 1f) < 0.25f)
		{
			triggerName = ((!flippedY) ? "FrontSwap" : "FrontSwapFlip");
			stateName = ((!flippedY) ? "SideSwapFront" : "SideSwapFrontFlip");
			isFront = true;
		}
		else
		{
			triggerName = ((!flippedY) ? "BackSwap" : "BackSwapFlip");
			stateName = ((!flippedY) ? "SideSwapBack" : "SideSwapBackFlip");
		}
		base.animator.SetTrigger(triggerName);
		yield return base.animator.WaitForAnimationToEnd(this, "Idle", false, true);
		if (isFront)
		{
			this.rend.sortingLayerName = "Foreground";
		}
		if (flippedY)
		{
			this.rightSideUp = !this.rightSideUp;
		}
		yield return base.animator.WaitForAnimationToEnd(this, stateName, false, true);
		yield return new WaitForEndOfFrame();
		this.rend.sortingLayerName = "Default";
		this.state = SnowCultLevelJackFrost.States.Idle;
		yield break;
	}

	// Token: 0x06002E80 RID: 11904 RVA: 0x001B675B File Offset: 0x001B4B5B
	private void FlipParentTransformX()
	{
		this.onRight = !this.onRight;
		this.ChangeSide();
	}

	// Token: 0x06002E81 RID: 11905 RVA: 0x001B6774 File Offset: 0x001B4B74
	private void FlipParentTransformXY()
	{
		base.transform.parent.SetScale(null, new float?((float)((!this.rightSideUp) ? -1 : 1)), null);
		this.FlipParentTransformX();
	}

	// Token: 0x06002E82 RID: 11906 RVA: 0x001B67C4 File Offset: 0x001B4BC4
	private void ChangeSide()
	{
		base.transform.parent.SetScale(new float?((!this.onRight) ? (-this.scale.x) : this.scale.x), null, null);
	}

	// Token: 0x06002E83 RID: 11907 RVA: 0x001B681F File Offset: 0x001B4C1F
	public void StartEyeAttack()
	{
		base.animator.SetTrigger("EyeAttack");
		this.state = SnowCultLevelJackFrost.States.Eye;
	}

	// Token: 0x06002E84 RID: 11908 RVA: 0x001B6838 File Offset: 0x001B4C38
	private void aniEvent_LaunchEye()
	{
		base.StartCoroutine(this.eye_attack_cr());
	}

	// Token: 0x06002E85 RID: 11909 RVA: 0x001B6848 File Offset: 0x001B4C48
	private IEnumerator eye_attack_cr()
	{
		LevelProperties.SnowCult.EyeAttack p = base.properties.CurrentState.eyeAttack;
		this.activeEyeProjectile = this.eyeProjectile.Spawn<SnowCultLevelEyeProjectile>();
		this.activeEyeProjectile.Init(this.eyeRoot.position, this.mouthRoot.position, this.onRight, this.rightSideUp, p);
		this.activeEyeProjectile.main = this;
		while (!this.activeEyeProjectile.readyToOpenMouth)
		{
			yield return null;
		}
		base.animator.SetTrigger("EyeAttackOpenMouth");
		while (!this.activeEyeProjectile.readyToCloseMouth)
		{
			yield return null;
		}
		base.animator.Play("EyeAttackEnd");
		base.animator.Update(0f);
		this.SFX_SNOWCULT_JackFrostEyeballReturn();
		yield return base.animator.WaitForAnimationToEnd(this, "EyeAttackEnd", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.attackDelay);
		this.state = SnowCultLevelJackFrost.States.Idle;
		yield break;
	}

	// Token: 0x06002E86 RID: 11910 RVA: 0x001B6863 File Offset: 0x001B4C63
	private void AniEvent_RemoveEye()
	{
		this.activeEyeProjectile.ReturnToSnowflake();
	}

	// Token: 0x06002E87 RID: 11911 RVA: 0x001B6870 File Offset: 0x001B4C70
	public void StartShardAttack()
	{
		base.StartCoroutine(this.shard_attack_cr());
	}

	// Token: 0x06002E88 RID: 11912 RVA: 0x001B6880 File Offset: 0x001B4C80
	private IEnumerator shard_attack_cr()
	{
		this.state = SnowCultLevelJackFrost.States.Shard;
		LevelProperties.SnowCult.ShardAttack p = base.properties.CurrentState.shardAttack;
		float degrees = 360f / (float)p.shardNumber;
		float loopSizeX = p.circleSizeX;
		float loopSizeY = p.circleSizeY;
		SnowCultLevelShard[] shards = new SnowCultLevelShard[p.shardNumber];
		string[] angleOffsetString = p.angleOffset.Split(new char[]
		{
			','
		});
		float angleOffset = this.shardAngleOffsetString.PopFloat();
		List<float> angleList = new List<float>();
		for (int k = 0; k < p.shardNumber; k++)
		{
			angleList.Add((degrees * (float)k + angleOffset) % 360f);
		}
		angleList.Sort((float a, float b) => ((a + 90f) % 360f).CompareTo((b + 90f) % 360f));
		this.iceCreamGhostRenderer.sortingOrder = -12;
		base.animator.SetTrigger("IceCreamAttack");
		yield return base.animator.WaitForAnimationToStart(this, "IceCream", false);
		this.SFX_SNOWCULT_JackFrostIcecream();
		YieldInstruction wait = new WaitForFixedUpdate();
		int count = 0;
		float sparkleDelay = UnityEngine.Random.Range(0.1f, 0.3f);
		while (count < p.shardNumber)
		{
			float normalizedAngle = Mathf.InverseLerp(0.11627907f, 0.7906977f, base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
			sparkleDelay -= CupheadTime.FixedDelta;
			if (sparkleDelay <= 0f)
			{
				float f = (normalizedAngle + 0.25f) % 1f * 3.1415927f * 2f;
				this.iceCreamSparkle.Create(this.platformPivotPoint.position + p.circleOffsetY * Vector3.up + Vector3.right * base.transform.parent.localScale.x * Mathf.Sin(f) * loopSizeX + Vector3.down * base.transform.parent.localScale.y * Mathf.Cos(f) * loopSizeY);
				sparkleDelay += UnityEngine.Random.Range(0.1f, 0.3f);
			}
			if ((float)count < normalizedAngle * (float)p.shardNumber)
			{
				float num = angleList[count];
				if (base.transform.parent.localScale.x < 0f)
				{
					num = 360f - num;
				}
				if (base.transform.parent.localScale.y < 0f)
				{
					num = (num + (90f - num) * 2f) % 360f;
				}
				SnowCultLevelShard snowCultLevelShard = this.shardPrefab.Spawn<SnowCultLevelShard>();
				snowCultLevelShard.Init(this.platformPivotPoint.position + p.circleOffsetY * Vector3.up + Vector3.forward * (float)count * 0.001f, num, loopSizeX, loopSizeY, p);
				shards[count] = snowCultLevelShard;
				count++;
			}
			yield return wait;
		}
		yield return CupheadTime.WaitForSeconds(this, p.warningLength);
		for (int l = 0; l < shards.Length; l++)
		{
			shards[l].Appear();
		}
		yield return CupheadTime.WaitForSeconds(this, p.shardHesitation);
		if (this.isClockwise)
		{
			for (int i = shards.Length - 1; i >= 0; i--)
			{
				yield return CupheadTime.WaitForSeconds(this, p.shardDelay);
				if (shards[i] != null)
				{
					shards[i].LaunchProjectile();
				}
			}
		}
		else
		{
			for (int j = 0; j < shards.Length; j++)
			{
				yield return CupheadTime.WaitForSeconds(this, p.shardDelay);
				if (shards[j] != null)
				{
					shards[j].LaunchProjectile();
				}
			}
		}
		yield return CupheadTime.WaitForSeconds(this, p.attackDelay);
		this.state = SnowCultLevelJackFrost.States.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06002E89 RID: 11913 RVA: 0x001B689B File Offset: 0x001B4C9B
	private void AniEvent_SetGhostLayerBehindSnowflakeMiddleLayer()
	{
		this.iceCreamGhostRenderer.sortingOrder = -17;
	}

	// Token: 0x06002E8A RID: 11914 RVA: 0x001B68AA File Offset: 0x001B4CAA
	public void StartMouthShot()
	{
		base.StartCoroutine(this.split_shot_cr());
	}

	// Token: 0x06002E8B RID: 11915 RVA: 0x001B68BC File Offset: 0x001B4CBC
	private IEnumerator split_shot_cr()
	{
		LevelProperties.SnowCult.SplitShot p = base.properties.CurrentState.splitShot;
		this.state = SnowCultLevelJackFrost.States.SplitShot;
		float posX = 0f;
		float posY = 0f;
		int timesToShoot = this.shotCoord.SubStringLength();
		base.animator.SetBool("SplitShot", true);
		yield return base.animator.WaitForAnimationToStart(this, "SplitShotStart", false);
		this.SFX_SNOWCULT_JackFrostSplitshotHandwavingStart();
		yield return base.animator.WaitForAnimationToStart(this, "SplitShotAnti", false);
		this.SFX_SNOWCULT_JackFrostSplitshotHandwavingLoop();
		for (int i = 0; i < timesToShoot; i++)
		{
			posY = this.shotCoord.PopFloat();
			posX = (float)((!this.onRight) ? 640 : -640);
			Vector3 pos = new Vector3(posX, base.transform.position.y + posY);
			Vector3 dir = pos - this.splitShotRoot.position;
			SnowCultLevelSplitShotBullet splitShot = (this.splitShotPink.PopLetter() != 'P') ? this.mouthPrefab.Spawn<SnowCultLevelSplitShotBullet>() : this.mouthPinkPrefab.Spawn<SnowCultLevelSplitShotBullet>();
			splitShot.Init(this.splitShotRoot.position, MathUtils.DirectionToAngle(dir), p.shotSpeed, p.shatterCount, p.spreadAngle, p);
			splitShot.transform.localScale = new Vector3(base.transform.parent.localScale.x, 1f);
			splitShot.main = this;
			yield return CupheadTime.WaitForSeconds(this, p.shotDelay - 0.45f);
			if (splitShot)
			{
				splitShot.Grow();
			}
			yield return CupheadTime.WaitForSeconds(this, 0.45f);
			if (splitShot)
			{
				base.animator.SetTrigger("SplitShotFire");
				this.SFX_SNOWCULT_JackFrostSplitshotBucketLaunch();
				while (!this.fireSplitShot)
				{
					yield return null;
				}
				this.fireSplitShot = false;
				if (splitShot)
				{
					splitShot.Fire();
				}
			}
		}
		base.animator.SetBool("SplitShot", false);
		this.SFX_SNOWCULT_JackFrostSplitshotHandwavingLoopStop();
		yield return CupheadTime.WaitForSeconds(this, p.attackDelay);
		this.state = SnowCultLevelJackFrost.States.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06002E8C RID: 11916 RVA: 0x001B68D7 File Offset: 0x001B4CD7
	private void AniEvent_FireSplitShot()
	{
		this.fireSplitShot = true;
	}

	// Token: 0x06002E8D RID: 11917 RVA: 0x001B68E0 File Offset: 0x001B4CE0
	private void OnBossDeath()
	{
		this.dead = true;
		base.transform.parent.SetScale(null, new float?(1f), null);
		base.animator.Play((!this.rightSideUp) ? "FlipDeath" : "Death");
	}

	// Token: 0x06002E8E RID: 11918 RVA: 0x001B6945 File Offset: 0x001B4D45
	private void EnableWizardDeathAnimation()
	{
		this.wizardDeath.SetActive(true);
	}

	// Token: 0x06002E8F RID: 11919 RVA: 0x001B6953 File Offset: 0x001B4D53
	private void AnimationEvent_SFX_SNOWCULT_JackFrostIntroThumblick()
	{
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_intro_thumblick");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_intro_thumblick");
	}

	// Token: 0x06002E90 RID: 11920 RVA: 0x001B696F File Offset: 0x001B4D6F
	private void AnimationEvent_SFX_SNOWCULT_JackFrostEyeballAttack()
	{
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_eyeball_attack");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_eyeball_attack");
	}

	// Token: 0x06002E91 RID: 11921 RVA: 0x001B698B File Offset: 0x001B4D8B
	private void SFX_SNOWCULT_JackFrostEyeballReturn()
	{
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_eyeball_return");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_eyeball_return");
	}

	// Token: 0x06002E92 RID: 11922 RVA: 0x001B69A7 File Offset: 0x001B4DA7
	private void SFX_SNOWCULT_JackFrostIcecream()
	{
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_icecreamattack");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_icecreamattack");
	}

	// Token: 0x06002E93 RID: 11923 RVA: 0x001B69C3 File Offset: 0x001B4DC3
	private void AnimationEvent_SFX_SNOWCULT_JackFrostSideSwap()
	{
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_sideswap");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_sideswap");
	}

	// Token: 0x06002E94 RID: 11924 RVA: 0x001B69DF File Offset: 0x001B4DDF
	private void SFX_SNOWCULT_JackFrostSplitshotHandwavingLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_snowcult_p3_snowflake_splitshot_handwaving_attack_loop");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_splitshot_handwaving_attack_loop");
	}

	// Token: 0x06002E95 RID: 11925 RVA: 0x001B69FB File Offset: 0x001B4DFB
	private void SFX_SNOWCULT_JackFrostSplitshotHandwavingLoopStop()
	{
		AudioManager.Stop("sfx_dlc_snowcult_p3_snowflake_splitshot_handwaving_attack_loop");
	}

	// Token: 0x06002E96 RID: 11926 RVA: 0x001B6A07 File Offset: 0x001B4E07
	private void SFX_SNOWCULT_JackFrostSplitshotHandwavingStart()
	{
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_splitshot_handwaving_attack_start");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_splitshot_handwaving_attack_start");
	}

	// Token: 0x06002E97 RID: 11927 RVA: 0x001B6A23 File Offset: 0x001B4E23
	private void SFX_SNOWCULT_JackFrostSplitshotBucketLaunch()
	{
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_splitshot_attack_bucket_launch");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_splitshot_attack_bucket_launch");
	}

	// Token: 0x06002E98 RID: 11928 RVA: 0x001B6A3F File Offset: 0x001B4E3F
	private void AnimationEvent_SFX_SNOWCULT_JackFrostDeath()
	{
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_death");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_death");
	}

	// Token: 0x04003709 RID: 14089
	private const int BLINK_LOOP_COUNT_MIN = 1;

	// Token: 0x0400370A RID: 14090
	private const int BLINK_LOOP_COUNT_MAX = 5;

	// Token: 0x0400370B RID: 14091
	[SerializeField]
	private BoxCollider2D boxCollider;

	// Token: 0x0400370C RID: 14092
	[SerializeField]
	private SnowCultLevelSplitShotBullet mouthPrefab;

	// Token: 0x0400370D RID: 14093
	[SerializeField]
	private SnowCultLevelSplitShotBullet mouthPinkPrefab;

	// Token: 0x0400370E RID: 14094
	[SerializeField]
	private SnowCultLevelShard shardPrefab;

	// Token: 0x0400370F RID: 14095
	[SerializeField]
	private Effect iceCreamSparkle;

	// Token: 0x04003710 RID: 14096
	[SerializeField]
	private SnowCultLevelEyeProjectile eyeProjectile;

	// Token: 0x04003711 RID: 14097
	private SnowCultLevelEyeProjectile activeEyeProjectile;

	// Token: 0x04003712 RID: 14098
	public Transform eyeProjectileGuide;

	// Token: 0x04003713 RID: 14099
	[SerializeField]
	private Transform eyeRoot;

	// Token: 0x04003714 RID: 14100
	[SerializeField]
	private Transform mouthRoot;

	// Token: 0x04003715 RID: 14101
	[SerializeField]
	private Transform splitShotRoot;

	// Token: 0x04003716 RID: 14102
	[SerializeField]
	private Transform platformPivotPoint;

	// Token: 0x04003717 RID: 14103
	[SerializeField]
	private GameObject platformPrefab;

	// Token: 0x04003718 RID: 14104
	[SerializeField]
	private Transform[] platformsPresetPositions;

	// Token: 0x04003719 RID: 14105
	[SerializeField]
	private GameObject wizardDeath;

	// Token: 0x0400371A RID: 14106
	[SerializeField]
	private GameObject bucket;

	// Token: 0x0400371B RID: 14107
	[SerializeField]
	private SpriteRenderer iceCreamGhostRenderer;

	// Token: 0x0400371C RID: 14108
	private bool onRight;

	// Token: 0x0400371D RID: 14109
	private bool rightSideUp;

	// Token: 0x0400371E RID: 14110
	private bool isClockwise;

	// Token: 0x0400371F RID: 14111
	private bool firstAttack = true;

	// Token: 0x04003720 RID: 14112
	private float positionX;

	// Token: 0x04003721 RID: 14113
	private Vector3 scale;

	// Token: 0x04003722 RID: 14114
	private SnowCultLevelPlatform[] presetPlatforms;

	// Token: 0x04003723 RID: 14115
	private SnowCultLevelPlatform[] circlePlatforms;

	// Token: 0x04003724 RID: 14116
	private AbstractPlayerController player;

	// Token: 0x04003725 RID: 14117
	private DamageDealer damageDealer;

	// Token: 0x04003726 RID: 14118
	private DamageReceiver damageReceiver;

	// Token: 0x04003727 RID: 14119
	public SnowCultLevelJackFrost.States state;

	// Token: 0x04003728 RID: 14120
	private PatternString faceOrientation;

	// Token: 0x04003729 RID: 14121
	private PatternString splitShotPink;

	// Token: 0x0400372A RID: 14122
	private PatternString shotCoord;

	// Token: 0x0400372B RID: 14123
	private PatternString shardAngleOffsetString;

	// Token: 0x0400372C RID: 14124
	private SpriteRenderer rend;

	// Token: 0x0400372D RID: 14125
	private bool fireSplitShot;

	// Token: 0x0400372E RID: 14126
	private int blinkCounter;

	// Token: 0x0400372F RID: 14127
	private int blinkCount;

	// Token: 0x04003730 RID: 14128
	public bool dead;

	// Token: 0x020007ED RID: 2029
	public enum States
	{
		// Token: 0x04003732 RID: 14130
		Intro,
		// Token: 0x04003733 RID: 14131
		Idle,
		// Token: 0x04003734 RID: 14132
		Switch,
		// Token: 0x04003735 RID: 14133
		Eye,
		// Token: 0x04003736 RID: 14134
		Beam,
		// Token: 0x04003737 RID: 14135
		Hazard,
		// Token: 0x04003738 RID: 14136
		Shard,
		// Token: 0x04003739 RID: 14137
		SplitShot,
		// Token: 0x0400373A RID: 14138
		Arc
	}
}
