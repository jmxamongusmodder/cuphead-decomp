using System;
using UnityEngine;

// Token: 0x02000583 RID: 1411
public class DevilLevelSplitDevil : LevelProperties.Devil.Entity
{
	// Token: 0x06001AF2 RID: 6898 RVA: 0x000F7D70 File Offset: 0x000F6170
	protected override void Awake()
	{
		base.Awake();
		base.animator.Play("Idle");
		this.state = DevilLevelSplitDevil.State.Idle;
	}

	// Token: 0x06001AF3 RID: 6899 RVA: 0x000F7D90 File Offset: 0x000F6190
	private void LateUpdate()
	{
		LevelPlayerController levelPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne) as LevelPlayerController;
		LevelPlayerController levelPlayerController2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo) as LevelPlayerController;
		bool value = levelPlayerController == null || levelPlayerController.transform.localScale.x > 0f;
		this.headsControler.SetBool("LookRight", value);
		base.animator.SetBool("LookRight", value);
		this.headsControler.SetBool("DevilLeft", this.DevilLeft);
	}

	// Token: 0x06001AF4 RID: 6900 RVA: 0x000F7E18 File Offset: 0x000F6218
	public void OnIdleLeftEnd()
	{
		bool @bool = base.animator.GetBool("Shoot");
		bool bool2 = base.animator.GetBool("LookRight");
		string stateName = "DevilLeftShootTransition_2_3_4";
		string stateName2 = "DevilLeftIdleBody_2_3_4";
		if (@bool)
		{
			if (bool2)
			{
				stateName = "DevilToAngel_Transition_2_3_4";
				stateName2 = "DevilToAngelIdleBody_2_3_4";
			}
			this.headsControler.enabled = true;
			this.headsControler.SetBool("Shoot", true);
			this.headsControler.Play(stateName, -1, 1f);
			base.animator.Play(stateName2, -1, 1f);
		}
	}

	// Token: 0x06001AF5 RID: 6901 RVA: 0x000F7EAC File Offset: 0x000F62AC
	public void OnIdleRightEnd()
	{
		bool @bool = base.animator.GetBool("Shoot");
		bool bool2 = base.animator.GetBool("LookRight");
		string stateName = "DevilRightShootTransition_2_3_4";
		string stateName2 = "DevilRightIdleBody_2_3_4";
		if (@bool)
		{
			if (!bool2)
			{
				stateName = "AngelToDevil_transition_2_3_4";
				stateName2 = "AngelToDevilIdleBody_2_3_4";
			}
			this.headsControler.enabled = true;
			this.headsControler.SetBool("Shoot", true);
			this.headsControler.Play(stateName, -1, 1f);
			base.animator.Play(stateName2, -1, 1f);
		}
	}

	// Token: 0x06001AF6 RID: 6902 RVA: 0x000F7F40 File Offset: 0x000F6340
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001AF7 RID: 6903 RVA: 0x000F7F53 File Offset: 0x000F6353
	public void StartTransform()
	{
		base.animator.SetTrigger("IsDead");
	}

	// Token: 0x06001AF8 RID: 6904 RVA: 0x000F7F65 File Offset: 0x000F6365
	public void OnDeadAnimationDone()
	{
		this.SplitDevilAnimationDone = true;
		base.gameObject.SetActive(false);
	}

	// Token: 0x0400242C RID: 9260
	public DevilLevelSplitDevil.State state;

	// Token: 0x0400242D RID: 9261
	[SerializeField]
	private Animator headsControler;

	// Token: 0x0400242E RID: 9262
	public bool DevilLeft = true;

	// Token: 0x0400242F RID: 9263
	public bool SplitDevilAnimationDone;

	// Token: 0x04002430 RID: 9264
	[SerializeField]
	private DevilLevelSplitDevilProjectile projectilePrefab;

	// Token: 0x04002431 RID: 9265
	private DevilLevelSplitDevilProjectile AngelprojectilePrefab;

	// Token: 0x04002432 RID: 9266
	[SerializeField]
	private Transform projectileRootLeft;

	// Token: 0x04002433 RID: 9267
	[SerializeField]
	private Transform projectileRootRight;

	// Token: 0x04002434 RID: 9268
	private int patternIndex;

	// Token: 0x04002435 RID: 9269
	private LevelProperties.Devil.Pattern pattern;

	// Token: 0x02000584 RID: 1412
	public enum State
	{
		// Token: 0x04002437 RID: 9271
		Idle,
		// Token: 0x04002438 RID: 9272
		Shoot,
		// Token: 0x04002439 RID: 9273
		summon
	}
}
