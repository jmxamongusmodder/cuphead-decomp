using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000535 RID: 1333
public class ChessBishopLevelBell : AbstractProjectile
{
	// Token: 0x17000331 RID: 817
	// (get) Token: 0x0600181B RID: 6171 RVA: 0x000D9E03 File Offset: 0x000D8203
	protected override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600181C RID: 6172 RVA: 0x000D9E06 File Offset: 0x000D8206
	public virtual ChessBishopLevelBell Init(Vector3 pos, AbstractPlayerController player, LevelProperties.ChessBishop.Bishop properties)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = pos;
		this.properties = properties;
		this.player = player;
		base.StartCoroutine(this.move_cr());
		return this;
	}

	// Token: 0x0600181D RID: 6173 RVA: 0x000D9E3C File Offset: 0x000D823C
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0600181E RID: 6174 RVA: 0x000D9E5C File Offset: 0x000D825C
	private IEnumerator move_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.projectileDelayRange.RandomFloat());
		base.animator.SetTrigger("Attack");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", 0, false, true);
		Vector3 direction = (this.player.transform.position - base.transform.position).normalized;
		if (base.animator.GetInteger(AbstractProjectile.Variant) == 0)
		{
			base.animator.Play("A", 1);
			base.animator.Play("A", 2);
			base.animator.Play("IntroA", 3);
			foreach (Transform transform in this.smokeTransforms)
			{
				transform.rotation = Quaternion.Euler(0f, 0f, 45f);
			}
			base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(90f + MathUtils.DirectionToAngle(direction)));
		}
		else
		{
			base.animator.Play("B", 1);
		}
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			base.transform.position += direction * this.properties.projectileSpeed * CupheadTime.FixedDelta;
			yield return wait;
		}
		yield break;
	}

	// Token: 0x04002148 RID: 8520
	private const int AnimatorBaseLayer = 0;

	// Token: 0x04002149 RID: 8521
	private const int AnimatorSmokeTopLayer = 1;

	// Token: 0x0400214A RID: 8522
	private const int AnimatorSmokeMiddleLayer = 2;

	// Token: 0x0400214B RID: 8523
	private const int AnimatorSmokeBottomLayer = 3;

	// Token: 0x0400214C RID: 8524
	[SerializeField]
	private Transform[] smokeTransforms;

	// Token: 0x0400214D RID: 8525
	private LevelProperties.ChessBishop.Bishop properties;

	// Token: 0x0400214E RID: 8526
	private AbstractPlayerController player;
}
