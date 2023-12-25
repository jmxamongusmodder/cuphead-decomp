using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200065B RID: 1627
public class FlyingCowboyLevelRicochetDebrisExplode : Effect
{
	// Token: 0x060021E8 RID: 8680 RVA: 0x0013BFEC File Offset: 0x0013A3EC
	private void Start()
	{
		Vector3 position = base.transform.position;
		position.z = UnityEngine.Random.Range(0f, 1f);
		base.transform.position = position;
	}

	// Token: 0x060021E9 RID: 8681 RVA: 0x0013C028 File Offset: 0x0013A428
	private IEnumerator movement_cr()
	{
		SpriteRenderer renderer = base.GetComponent<SpriteRenderer>();
		float elapsedTime = 0f;
		while (elapsedTime < 0.5f)
		{
			yield return null;
			elapsedTime += CupheadTime.Delta;
			Vector3 position = base.transform.position;
			position.x -= 900f * CupheadTime.Delta;
			base.transform.position = position;
			Color color = renderer.color;
			color.a = Mathf.Lerp(1f, 0f, elapsedTime / 0.5f);
			renderer.color = color;
		}
		this.OnEffectComplete();
		yield break;
	}

	// Token: 0x060021EA RID: 8682 RVA: 0x0013C043 File Offset: 0x0013A443
	private void animationEvent_StartMovement()
	{
		base.StartCoroutine(this.movement_cr());
	}
}
