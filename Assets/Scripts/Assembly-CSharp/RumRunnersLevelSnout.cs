using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200079C RID: 1948
public class RumRunnersLevelSnout : AbstractCollidableObject
{
	// Token: 0x170003FD RID: 1021
	// (get) Token: 0x06002B4E RID: 11086 RVA: 0x001937D8 File Offset: 0x00191BD8
	// (set) Token: 0x06002B4F RID: 11087 RVA: 0x001937E0 File Offset: 0x00191BE0
	public bool isAttacking { get; private set; }

	// Token: 0x06002B50 RID: 11088 RVA: 0x001937EC File Offset: 0x00191BEC
	private void Start()
	{
		foreach (DamageReceiver damageReceiver in this.damageReceivers)
		{
			damageReceiver.OnDamageTaken += this.OnDamageTaken;
		}
		this.snoutScale = base.transform.localScale;
		base.transform.position = RumRunnersLevelSnout.OffscreenCoord;
	}

	// Token: 0x06002B51 RID: 11089 RVA: 0x00193850 File Offset: 0x00191C50
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.parent.DoDamage(info.damage);
	}

	// Token: 0x06002B52 RID: 11090 RVA: 0x00193863 File Offset: 0x00191C63
	public void Setup(LevelProperties.RumRunners properties)
	{
		this.properties = properties;
		this.copBallLaunchAnglePattern = new PatternString(properties.CurrentState.copBall.copBallLaunchAngleString, true, true);
	}

	// Token: 0x06002B53 RID: 11091 RVA: 0x0019388C File Offset: 0x00191C8C
	public void Attack(Vector3 position, Vector2 shadowPosition, bool onLeft, RumRunnersLevelSnout.AttackType attackType)
	{
		Vector3 position2 = position;
		position2.x = (float)((!onLeft) ? Level.Current.Right : Level.Current.Left);
		Vector3 one = Vector3.one;
		one.x *= (float)((!onLeft) ? -1 : 1);
		this.dirtEffect.Create(position2, one);
		base.transform.position = position;
		this.shadowTransform.localPosition = shadowPosition;
		base.StartCoroutine(this.attack_cr(onLeft, attackType));
	}

	// Token: 0x06002B54 RID: 11092 RVA: 0x00193920 File Offset: 0x00191D20
	private IEnumerator attack_cr(bool onLeft, RumRunnersLevelSnout.AttackType attackType)
	{
		LevelProperties.RumRunners.AnteaterSnout p = this.properties.CurrentState.anteaterSnout;
		this.onLeft = onLeft;
		this.endNormal = (this.endTongue = false);
		base.transform.SetScale(new float?((!onLeft) ? (-this.snoutScale.x) : this.snoutScale.x), new float?(this.snoutScale.y), null);
		this.parent.SetEyeSide(onLeft);
		base.animator.SetBool("Fake", attackType == RumRunnersLevelSnout.AttackType.Fake);
		base.animator.SetBool("Tongue", attackType == RumRunnersLevelSnout.AttackType.Tongue);
		base.animator.SetTrigger("Attack");
		this.isAttacking = true;
		if (attackType == RumRunnersLevelSnout.AttackType.Fake || attackType == RumRunnersLevelSnout.AttackType.Tongue)
		{
			float fullOutBoilDelay = p.snoutFullOutBoilDelay;
			if (fullOutBoilDelay > 0f)
			{
				yield return base.animator.WaitForAnimationToStart(this, "FullOutHold", false);
				yield return CupheadTime.WaitForSeconds(this, fullOutBoilDelay);
			}
			base.animator.SetTrigger("HoldComplete");
		}
		if (attackType == RumRunnersLevelSnout.AttackType.Tongue)
		{
			yield return base.animator.WaitForAnimationToStart(this, "TongueHold", false);
			yield return CupheadTime.WaitForSeconds(this, p.tongueHoldDuration);
			base.animator.SetBool("Tongue", false);
			while (!this.endTongue)
			{
				yield return null;
			}
		}
		else
		{
			while (!this.endNormal)
			{
				yield return null;
			}
		}
		this.isAttacking = false;
		if (attackType == RumRunnersLevelSnout.AttackType.Tongue)
		{
			yield return base.animator.WaitForAnimationToStart(this, "Off", false);
		}
		else
		{
			yield return base.animator.WaitForAnimationToEnd(this, "QuickEnd", false, true);
		}
		base.transform.position = RumRunnersLevelSnout.OffscreenCoord;
		yield break;
	}

	// Token: 0x06002B55 RID: 11093 RVA: 0x00193949 File Offset: 0x00191D49
	private void animationEvent_EndNormalAttack()
	{
		this.endNormal = true;
	}

	// Token: 0x06002B56 RID: 11094 RVA: 0x00193952 File Offset: 0x00191D52
	private void animationEvent_TriggerTongueEyes()
	{
		this.parent.TriggerEyesTurnaround();
	}

	// Token: 0x06002B57 RID: 11095 RVA: 0x00193960 File Offset: 0x00191D60
	private void animationEvent_EndFakeTongueAttack()
	{
		Effect effect = this.fakeTongueSpittleEffect.Create(this.tonguePokeFXTransform.position);
		if (!this.onLeft)
		{
			Vector3 localScale = effect.transform.localScale;
			localScale.x *= -1f;
			effect.transform.localScale = localScale;
		}
	}

	// Token: 0x06002B58 RID: 11096 RVA: 0x001939BA File Offset: 0x00191DBA
	private void animationEvent_EndTongueAttack()
	{
		this.endTongue = true;
	}

	// Token: 0x06002B59 RID: 11097 RVA: 0x001939C4 File Offset: 0x00191DC4
	private void animationEvent_FlipIfNecessary()
	{
		float num = this.copBallLaunchAnglePattern.PopFloat();
		base.animator.SetBool("ThrowDown", num < 0f);
	}

	// Token: 0x06002B5A RID: 11098 RVA: 0x001939F8 File Offset: 0x00191DF8
	private void animationEvent_SpawnCopBall()
	{
		LevelProperties.RumRunners.CopBall copBall = this.properties.CurrentState.copBall;
		do
		{
			this.copBallList.RemoveAll((RumRunnersLevelCopBall b) => b == null || b.leaveScreen);
			if (this.copBallList.Count >= copBall.copBallMaxCount)
			{
				this.copBallList[0].leaveScreen = true;
			}
		}
		while (this.copBallList.Count >= copBall.copBallMaxCount);
		float @float = this.copBallLaunchAnglePattern.GetFloat();
		RumRunnersLevelCopBall rumRunnersLevelCopBall = this.copBallPrefab.Spawn<RumRunnersLevelCopBall>();
		float angle = (!this.onLeft) ? (180f - @float) : @float;
		rumRunnersLevelCopBall.Init(this.copballLaunchOrigin.position, MathUtils.AngleToDirection(angle), copBall.copBallSpeed, copBall.copBallHP, copBall, this.copballLaunchOrigin);
		this.copBallList.Add(rumRunnersLevelCopBall);
	}

	// Token: 0x06002B5B RID: 11099 RVA: 0x00193AEC File Offset: 0x00191EEC
	private void animationEvent_FireCopBall()
	{
		if (this.copBallList[this.copBallList.Count - 1] != null)
		{
			this.copBallList[this.copBallList.Count - 1].Launch();
		}
	}

	// Token: 0x06002B5C RID: 11100 RVA: 0x00193B3C File Offset: 0x00191F3C
	public void Death()
	{
		this.StopAllCoroutines();
		foreach (RumRunnersLevelCopBall rumRunnersLevelCopBall in this.copBallList)
		{
			if (rumRunnersLevelCopBall != null)
			{
				rumRunnersLevelCopBall.Death(false);
			}
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x06002B5D RID: 11101 RVA: 0x00193BB8 File Offset: 0x00191FB8
	private void AnimationEvent_SFX_RUMRUN_P3_AntEater_Attack_Enter()
	{
		if (base.animator.GetBool("Tongue"))
		{
			AudioManager.Play("sfx_dlc_rumrun_p3_anteater_attack_snout_tongue_fullouthold");
			this.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_attack_snout_tongue_fullouthold");
		}
		else
		{
			AudioManager.Play("sfx_dlc_rumrun_p3_anteater_attack_short_enter");
			this.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_attack_short_enter");
		}
	}

	// Token: 0x06002B5E RID: 11102 RVA: 0x00193C13 File Offset: 0x00192013
	private void AnimationEvent_SFX_RUMRUN_P3_AntEater_Attack_Tongue()
	{
		if (base.animator.GetBool("Tongue"))
		{
			AudioManager.Play("sfx_dlc_rumrun_p3_anteater_attack_snout_tongue_attack");
		}
	}

	// Token: 0x06002B5F RID: 11103 RVA: 0x00193C39 File Offset: 0x00192039
	private void AnimationEvent_SFX_RUMRUN_P3_AntEater_Attack_ShortExit()
	{
		AudioManager.Play("sfx_dlc_rumrun_p3_anteater_attack_short_exit");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_attack_short_exit");
	}

	// Token: 0x06002B60 RID: 11104 RVA: 0x00193C55 File Offset: 0x00192055
	private void AnimationEvent_SFX_RUMRUN_P3_AntEater_Attack_SpitBallCop()
	{
		AudioManager.Play("sfx_dlc_rumrun_p3_anteater_attack_snout_tongue_spitballcop");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_anteater_attack_snout_tongue_spitballcop");
	}

	// Token: 0x06002B61 RID: 11105 RVA: 0x00193C71 File Offset: 0x00192071
	private void AnimationEvent_SFX_RUMRUN_P3_BallCop_SpitVocalShouts()
	{
		AudioManager.Play("sfx_dlc_rumrun_p3_ballcop_spitvocalshouts");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_p3_ballcop_spitvocalshouts");
	}

	// Token: 0x04003403 RID: 13315
	private static readonly Vector3 OffscreenCoord = new Vector3(0f, 1500f);

	// Token: 0x04003404 RID: 13316
	[SerializeField]
	private Transform copballLaunchOrigin;

	// Token: 0x04003405 RID: 13317
	[SerializeField]
	private RumRunnersLevelCopBall copBallPrefab;

	// Token: 0x04003406 RID: 13318
	[SerializeField]
	private Effect dirtEffect;

	// Token: 0x04003407 RID: 13319
	[SerializeField]
	private RumRunnersLevelAnteater parent;

	// Token: 0x04003408 RID: 13320
	[SerializeField]
	private Transform shadowTransform;

	// Token: 0x04003409 RID: 13321
	[SerializeField]
	private DamageReceiver[] damageReceivers;

	// Token: 0x0400340A RID: 13322
	[SerializeField]
	private Transform tonguePokeFXTransform;

	// Token: 0x0400340B RID: 13323
	[SerializeField]
	private Effect fakeTongueSpittleEffect;

	// Token: 0x0400340D RID: 13325
	private LevelProperties.RumRunners properties;

	// Token: 0x0400340E RID: 13326
	private Vector2 snoutScale;

	// Token: 0x0400340F RID: 13327
	private List<RumRunnersLevelCopBall> copBallList = new List<RumRunnersLevelCopBall>();

	// Token: 0x04003410 RID: 13328
	private bool onLeft;

	// Token: 0x04003411 RID: 13329
	private bool endNormal;

	// Token: 0x04003412 RID: 13330
	private bool endTongue;

	// Token: 0x04003413 RID: 13331
	private PatternString copBallLaunchAnglePattern;

	// Token: 0x0200079D RID: 1949
	public enum AttackType
	{
		// Token: 0x04003416 RID: 13334
		Quick,
		// Token: 0x04003417 RID: 13335
		Fake,
		// Token: 0x04003418 RID: 13336
		Tongue
	}
}
