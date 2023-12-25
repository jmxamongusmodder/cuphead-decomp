using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000554 RID: 1364
public class ChessRookLevelRook : LevelProperties.ChessRook.Entity
{
	// Token: 0x06001964 RID: 6500 RVA: 0x000E62D4 File Offset: 0x000E46D4
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06001965 RID: 6501 RVA: 0x000E62E7 File Offset: 0x000E46E7
	public override void LevelInit(LevelProperties.ChessRook properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x06001966 RID: 6502 RVA: 0x000E62F0 File Offset: 0x000E46F0
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmosSelected();
		Gizmos.color = Color.cyan;
		foreach (Transform transform in this.straightShotSpawnPoints)
		{
			Gizmos.DrawLine(transform.transform.position, transform.transform.position - Vector3.right * 1000f);
		}
	}

	// Token: 0x06001967 RID: 6503 RVA: 0x000E635B File Offset: 0x000E475B
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001968 RID: 6504 RVA: 0x000E637C File Offset: 0x000E477C
	protected override void OnCollisionEnemyProjectile(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemyProjectile(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			ChessRookLevelPinkCannonBall component = hit.GetComponent<ChessRookLevelPinkCannonBall>();
			if (component && component.finishedOriginalArc)
			{
				this.damaged();
				component.Explosion();
			}
		}
	}

	// Token: 0x06001969 RID: 6505 RVA: 0x000E63C0 File Offset: 0x000E47C0
	private void damaged()
	{
		if (this.dead)
		{
			return;
		}
		AudioManager.Play("sfx_dlc_kog_rook_hurt");
		this.emitAudioFromObject.Add("sfx_dlc_kog_rook_hurt");
		this.hitFlash.Flash(0.7f);
		LevelProperties.ChessRook.States stateName = base.properties.CurrentState.stateName;
		base.properties.DealDamage((!PlayerManager.BothPlayersActive()) ? 10f : ChessKingLevelKing.multiplayerDamageNerf);
		if (base.properties.CurrentHealth <= 0f && !this.dead)
		{
			this.die();
		}
		else if (stateName == LevelProperties.ChessRook.States.PhaseThree || stateName == LevelProperties.ChessRook.States.PhaseFour)
		{
			if (this.transitionCoroutine != null)
			{
				base.StopCoroutine(this.transitionCoroutine);
				base.animator.ResetTrigger("Transition");
				this.transitionCoroutine = null;
				base.animator.Play("LateIntro", ChessRookLevelRook.SparkLayerStateIndex);
			}
			this.hitSparkEffect.Create(base.transform.position);
			base.animator.Play("Hit2", 0, 0f);
		}
		else
		{
			base.animator.Play("Hit1", 0, 0f);
			base.animator.Play("HitSmoke", 3, 0f);
		}
	}

	// Token: 0x0600196A RID: 6506 RVA: 0x000E6514 File Offset: 0x000E4914
	public void OnPhaseChange()
	{
		this.StopAllCoroutines();
		this.StartAttacks();
		base.animator.ResetTrigger("SparkAttack");
		if (base.properties.CurrentState.stateName == LevelProperties.ChessRook.States.PhaseThree)
		{
			this.transitionCoroutine = base.StartCoroutine(this.transition_cr());
		}
	}

	// Token: 0x0600196B RID: 6507 RVA: 0x000E6565 File Offset: 0x000E4965
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600196C RID: 6508 RVA: 0x000E6580 File Offset: 0x000E4980
	private void animationEvent_IntroFinished()
	{
		this.StartAttacks();
		base.animator.Play("Intro", ChessRookLevelRook.SparkLayerStateIndex);
		AudioManager.PlayLoop("sfx_dlc_kog_rook_grindingwheel_lowspeed");
		this.emitAudioFromObject.Add("sfx_dlc_kog_rook_grindingwheel_lowspeed");
		AudioManager.PlayLoop("sfx_dlc_kog_rook_grindingwheel_lowspeed_axeonwheel");
		AudioManager.FadeSFXVolume("sfx_dlc_kog_rook_grindingwheel_lowspeed_axeonwheel", 0.0001f, 0.0001f);
		this.emitAudioFromObject.Add("sfx_dlc_kog_rook_grindingwheel_lowspeed_axeonwheel");
		AudioManager.PlayLoop("sfx_dlc_kog_rook_sparks_loop");
	}

	// Token: 0x0600196D RID: 6509 RVA: 0x000E65FC File Offset: 0x000E49FC
	private IEnumerator transition_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "Hit1", false, true);
		base.animator.SetTrigger("Transition");
		yield return base.animator.WaitForAnimationToEnd(this, "Transition", false, true);
		AudioManager.Stop("sfx_dlc_kog_rook_grindingwheel_lowspeed");
		AudioManager.PlayLoop("sfx_dlc_kog_rook_grindingwheel_highspeed");
		this.emitAudioFromObject.Add("sfx_dlc_kog_rook_grindingwheel_highspeed");
		this.transitionCoroutine = null;
		yield break;
	}

	// Token: 0x0600196E RID: 6510 RVA: 0x000E6617 File Offset: 0x000E4A17
	private void animationEvent_EndEarlyPhaseSparks()
	{
		base.animator.Play("EarlyOutro", ChessRookLevelRook.SparkLayerStateIndex);
	}

	// Token: 0x0600196F RID: 6511 RVA: 0x000E662E File Offset: 0x000E4A2E
	private void animationEvent_StartLatePhaseSparks()
	{
		base.animator.Play("LateIntro", ChessRookLevelRook.SparkLayerStateIndex);
	}

	// Token: 0x06001970 RID: 6512 RVA: 0x000E6648 File Offset: 0x000E4A48
	private void StartAttacks()
	{
		base.StartCoroutine(this.pink_cannonballs_cr());
		base.StartCoroutine(this.regular_cannonballs_cr());
		if (base.properties.CurrentState.straightShooters.straightShotOn)
		{
			base.StartCoroutine(this.straight_shot_cr());
		}
	}

	// Token: 0x06001971 RID: 6513 RVA: 0x000E6698 File Offset: 0x000E4A98
	private IEnumerator pink_cannonballs_cr()
	{
		LevelProperties.ChessRook.PinkCannonBall p = base.properties.CurrentState.pinkCannonBall;
		PatternString delayPattern = new PatternString(p.pinkShotDelayString, true, true);
		PatternString apexHeightPattern = new PatternString(p.pinkShotApexHeightString, true, true);
		PatternString targetPattern = new PatternString(p.pinkShotTargetString, true, true);
		for (;;)
		{
			float delay = delayPattern.PopFloat();
			float apexHeight = apexHeightPattern.PopFloat();
			float targetDistance = targetPattern.PopFloat();
			yield return CupheadTime.WaitForSeconds(this, delay - 0.16666667f);
			this.spawnEffect.Play("Spawn" + ((!Rand.Bool()) ? "B" : "A") + "Head" + ((!this.headTypeB) ? "A" : "B"), 0, 0f);
			this.spawnEffect.Update(0f);
			yield return CupheadTime.WaitForSeconds(this, 0.16666667f);
			ChessRookLevelPinkCannonBall cannonBall = this.cannonballPink.Spawn<ChessRookLevelPinkCannonBall>();
			cannonBall.Create(this.cannonballSpawnRoot.position + base.transform.forward * 1E-05f * (float)this.headZOffset, apexHeight, targetDistance, p);
			cannonBall.animator.Play((!this.headTypeB) ? "A" : "B");
			this.headTypeB = !this.headTypeB;
			this.headZOffset = (this.headZOffset + 1) % 10;
		}
		yield break;
	}

	// Token: 0x06001972 RID: 6514 RVA: 0x000E66B4 File Offset: 0x000E4AB4
	private IEnumerator regular_cannonballs_cr()
	{
		LevelProperties.ChessRook.RegularCannonBall p = base.properties.CurrentState.regularCannonBall;
		PatternString delayPattern = new PatternString(p.cannonDelayString, true, true);
		PatternString apexHeightPattern = new PatternString(p.cannonApexHeightString, true, true);
		PatternString targetPattern = new PatternString(p.cannonTargetString, true, true);
		for (;;)
		{
			float delay = delayPattern.PopFloat();
			float apexHeight = apexHeightPattern.PopFloat();
			float targetDistance = targetPattern.PopFloat();
			yield return CupheadTime.WaitForSeconds(this, delay - 0.16666667f);
			this.spawnEffect.Play("Spawn" + ((!Rand.Bool()) ? "B" : "A") + "Skull", 0, 0f);
			this.spawnEffect.Update(0f);
			yield return CupheadTime.WaitForSeconds(this, 0.16666667f);
			ChessRookLevelRegularCannonball cannonBall = this.cannonballRegular.Spawn<ChessRookLevelRegularCannonball>();
			cannonBall.Create(this.cannonballSpawnRoot.position + base.transform.forward * 1E-05f * (float)this.headZOffset, apexHeight, targetDistance, p);
			this.headZOffset = (this.headZOffset + 1) % 10;
		}
		yield break;
	}

	// Token: 0x06001973 RID: 6515 RVA: 0x000E66D0 File Offset: 0x000E4AD0
	private IEnumerator straight_shot_cr()
	{
		LevelProperties.ChessRook.StraightShooters p = base.properties.CurrentState.straightShooters;
		PatternString sequencePattern = new PatternString(p.straightShotSeqString, true, true);
		PatternString delayPattern = new PatternString(p.straightShotDelayString, true, true);
		float EarlyPhaseTransitionOffset = 0.14583333f;
		Rangef EarlyPhaseShootOffsetRange = new Rangef(0.16666667f, 0.33333334f);
		for (;;)
		{
			float delay = delayPattern.PopFloat();
			bool isEarlyPhase = base.properties.CurrentState.stateName == LevelProperties.ChessRook.States.Main || base.properties.CurrentState.stateName == LevelProperties.ChessRook.States.PhaseTwo;
			float shootDelay = 0f;
			if (isEarlyPhase)
			{
				shootDelay = UnityEngine.Random.Range(EarlyPhaseShootOffsetRange.minimum, EarlyPhaseShootOffsetRange.maximum);
				delay -= EarlyPhaseTransitionOffset;
				delay -= shootDelay;
			}
			yield return CupheadTime.WaitForSeconds(this, delay);
			if (isEarlyPhase)
			{
				base.animator.SetTrigger("SparkAttack");
				yield return base.animator.WaitForAnimationToEnd(this, "Idle1.Main", false, true);
				base.animator.Play("EarlyActiveA", ChessRookLevelRook.SparkLayerStateIndex);
				yield return CupheadTime.WaitForSeconds(this, shootDelay);
			}
			char sequence = sequencePattern.PopLetter();
			int spawnPosIndex = 0;
			if (sequence == 'T')
			{
				spawnPosIndex = 0;
			}
			else if (sequence == 'M')
			{
				spawnPosIndex = 1;
			}
			else if (sequence == 'B')
			{
				spawnPosIndex = 2;
			}
			Vector3 position = this.straightShotSpawnPoints[spawnPosIndex].position;
			this.straightShot.Create(position, 180f, p.straightShotBulletSpeed);
			this.smokeEffect.Create(position);
			this.straightShotSparkEffect.Create(position);
			AudioManager.Play("sfx_dlc_kog_rook_sparks_singles");
		}
		yield break;
	}

	// Token: 0x06001974 RID: 6516 RVA: 0x000E66EC File Offset: 0x000E4AEC
	private void die()
	{
		this.dead = true;
		this.StopAllCoroutines();
		AudioManager.Play("sfx_dlc_kog_rook_death");
		AudioManager.Stop("sfx_dlc_kog_rook_sparks_loop");
		AudioManager.Stop("sfx_dlc_kog_rook_grindingwheel_lowspeed");
		AudioManager.Stop("sfx_dlc_kog_rook_grindingwheel_highspeed");
		AudioManager.Stop("sfx_dlc_kog_rook_grindingwheel_lowspeed_axeonwheel");
		base.animator.Play("Death", ChessRookLevelRook.BaseLayerStateIndex);
		base.animator.Play("Off", ChessRookLevelRook.SparkLayerStateIndex);
		base.animator.Play("Off", ChessRookLevelRook.WheelLayerStateIndex);
		this.wheelRenderer.sortingOrder = 1000;
		this.wheelRenderer.sortingLayerName = "Foreground";
	}

	// Token: 0x06001975 RID: 6517 RVA: 0x000E6797 File Offset: 0x000E4B97
	private void SFX_GrindAxe()
	{
		base.StartCoroutine(this.sfx_grind_axe_cr());
	}

	// Token: 0x06001976 RID: 6518 RVA: 0x000E67A8 File Offset: 0x000E4BA8
	private IEnumerator sfx_grind_axe_cr()
	{
		AudioManager.FadeSFXVolume("sfx_dlc_kog_rook_grindingwheel_lowspeed_axeonwheel", 0.7f, 0.1f);
		yield return CupheadTime.WaitForSeconds(this, 0.3f);
		AudioManager.FadeSFXVolume("sfx_dlc_kog_rook_grindingwheel_lowspeed_axeonwheel", 0.0001f, 0.1f);
		yield break;
	}

	// Token: 0x0400227F RID: 8831
	private static readonly int BaseLayerStateIndex;

	// Token: 0x04002280 RID: 8832
	private static readonly int SparkLayerStateIndex = 1;

	// Token: 0x04002281 RID: 8833
	private static readonly int WheelLayerStateIndex = 2;

	// Token: 0x04002282 RID: 8834
	[SerializeField]
	private SpriteRenderer wheelRenderer;

	// Token: 0x04002283 RID: 8835
	[SerializeField]
	private Transform cannonballSpawnRoot;

	// Token: 0x04002284 RID: 8836
	[SerializeField]
	private ChessRookLevelPinkCannonBall cannonballPink;

	// Token: 0x04002285 RID: 8837
	[SerializeField]
	private ChessRookLevelRegularCannonball cannonballRegular;

	// Token: 0x04002286 RID: 8838
	[SerializeField]
	private BasicProjectile straightShot;

	// Token: 0x04002287 RID: 8839
	[SerializeField]
	private Transform[] straightShotSpawnPoints;

	// Token: 0x04002288 RID: 8840
	[SerializeField]
	private Effect hitSparkEffect;

	// Token: 0x04002289 RID: 8841
	[SerializeField]
	private Effect straightShotSparkEffect;

	// Token: 0x0400228A RID: 8842
	[SerializeField]
	private Effect smokeEffect;

	// Token: 0x0400228B RID: 8843
	[SerializeField]
	private Animator spawnEffect;

	// Token: 0x0400228C RID: 8844
	[SerializeField]
	private HitFlash hitFlash;

	// Token: 0x0400228D RID: 8845
	private DamageDealer damageDealer;

	// Token: 0x0400228E RID: 8846
	private Coroutine transitionCoroutine;

	// Token: 0x0400228F RID: 8847
	private bool dead;

	// Token: 0x04002290 RID: 8848
	private bool headTypeB;

	// Token: 0x04002291 RID: 8849
	private int headZOffset;
}
