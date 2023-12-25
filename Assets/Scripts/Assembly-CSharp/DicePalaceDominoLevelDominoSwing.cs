using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005B7 RID: 1463
public class DicePalaceDominoLevelDominoSwing : AbstractCollidableObject
{
	// Token: 0x06001C6D RID: 7277 RVA: 0x001044D7 File Offset: 0x001028D7
	protected override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06001C6E RID: 7278 RVA: 0x001044E0 File Offset: 0x001028E0
	public void InitSwing(LevelProperties.DicePalaceDomino properties)
	{
		this.speed = properties.CurrentState.domino.swingSpeed;
		this.strength = properties.CurrentState.domino.swingDistance;
		this.origin = new Vector3(base.transform.position.x, properties.CurrentState.domino.swingPosY);
		base.transform.position = this.origin;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001C6F RID: 7279 RVA: 0x00104565 File Offset: 0x00102965
	protected virtual float hitPauseCoefficient()
	{
		return (!base.GetComponentInChildren<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x06001C70 RID: 7280 RVA: 0x00104588 File Offset: 0x00102988
	private IEnumerator move_cr()
	{
		yield return this.domino.GetComponent<Animator>().WaitForAnimationToEnd(this, "Intro", false, true);
		float angle = 0f;
		for (;;)
		{
			angle += this.speed * CupheadTime.Delta * this.hitPauseCoefficient();
			base.transform.position = this.origin + Vector3.up * (Mathf.Sin(angle) * this.strength);
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002568 RID: 9576
	[SerializeField]
	private DicePalaceDominoLevelDomino domino;

	// Token: 0x04002569 RID: 9577
	private float speed;

	// Token: 0x0400256A RID: 9578
	private float strength;

	// Token: 0x0400256B RID: 9579
	private Vector3 origin;
}
