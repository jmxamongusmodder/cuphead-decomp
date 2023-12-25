using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005AF RID: 1455
public class DicePalaceCigarLevelCigaretteGhost : AbstractProjectile
{
	// Token: 0x06001C28 RID: 7208 RVA: 0x0010261C File Offset: 0x00100A1C
	public void InitGhost(LevelProperties.DicePalaceCigar properties)
	{
		this.properties = properties;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001C29 RID: 7209 RVA: 0x00102634 File Offset: 0x00100A34
	private IEnumerator move_cr()
	{
		base.StartCoroutine(this.spawn_fx_cr());
		YieldInstruction wait = new WaitForFixedUpdate();
		while (base.transform.position.y < 560f)
		{
			base.transform.AddPosition(0f, this.properties.CurrentState.cigaretteGhost.verticalSpeed * CupheadTime.FixedDelta, 0f);
			yield return wait;
		}
		this.Die();
		yield break;
	}

	// Token: 0x06001C2A RID: 7210 RVA: 0x0010264F File Offset: 0x00100A4F
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001C2B RID: 7211 RVA: 0x0010266D File Offset: 0x00100A6D
	protected override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
	}

	// Token: 0x06001C2C RID: 7212 RVA: 0x0010267B File Offset: 0x00100A7B
	protected override void Die()
	{
		base.Die();
		this.StopAllCoroutines();
	}

	// Token: 0x06001C2D RID: 7213 RVA: 0x0010268C File Offset: 0x00100A8C
	private IEnumerator spawn_fx_cr()
	{
		bool isVal = Rand.Bool();
		for (;;)
		{
			float value = UnityEngine.Random.Range(0.4f, 0.6f);
			float value2 = UnityEngine.Random.Range(0.2f, 0.3f);
			float chosenVal = (!isVal) ? value2 : value;
			yield return CupheadTime.WaitForSeconds(this, chosenVal);
			float t = 0f;
			float time = chosenVal;
			while (t < time)
			{
				t += CupheadTime.Delta;
				this.fx.Create(this.root.transform.position);
				yield return CupheadTime.WaitForSeconds(this, 0.1f);
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0.25f, 0.45f));
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002537 RID: 9527
	[SerializeField]
	private Transform root;

	// Token: 0x04002538 RID: 9528
	[SerializeField]
	private Effect fx;

	// Token: 0x04002539 RID: 9529
	private Vector3 centerPoint;

	// Token: 0x0400253A RID: 9530
	private LevelProperties.DicePalaceCigar properties;
}
