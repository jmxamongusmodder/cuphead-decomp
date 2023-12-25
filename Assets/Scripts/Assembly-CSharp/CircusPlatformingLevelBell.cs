using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200089E RID: 2206
public class CircusPlatformingLevelBell : PlatformingLevelPathMovementEnemy
{
	// Token: 0x06003356 RID: 13142 RVA: 0x001DDE3F File Offset: 0x001DC23F
	public override void OnParry(AbstractPlayerController player)
	{
		base.animator.Play("Ring");
		AudioManager.Play("circus_bell_ding");
		base.GetComponent<Collider2D>().enabled = false;
		base.StartCoroutine(this.timer_cr());
	}

	// Token: 0x06003357 RID: 13143 RVA: 0x001DDE74 File Offset: 0x001DC274
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
	}

	// Token: 0x06003358 RID: 13144 RVA: 0x001DDE78 File Offset: 0x001DC278
	private IEnumerator timer_cr()
	{
		float t = 0f;
		while (t < this.coolDown)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		Collider2D collider = base.GetComponent<Collider2D>();
		collider.enabled = true;
		yield return null;
		yield break;
	}

	// Token: 0x06003359 RID: 13145 RVA: 0x001DDE93 File Offset: 0x001DC293
	protected override void CalculateCollider()
	{
	}

	// Token: 0x04003BA0 RID: 15264
	public float coolDown = 0.4f;
}
