using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200078C RID: 1932
public class RumRunnersLevelFullscreenDirtFX : Effect
{
	// Token: 0x06002AB2 RID: 10930 RVA: 0x0018EE70 File Offset: 0x0018D270
	public override void Initialize(Vector3 position, Vector3 scale, bool randomR)
	{
		base.Initialize(position, scale, randomR);
		int num = base.animator.GetInteger("Effect");
		while (num == RumRunnersLevelFullscreenDirtFX.PreviousEffectA || num == RumRunnersLevelFullscreenDirtFX.PreviousEffectB)
		{
			num = UnityEngine.Random.Range(0, base.animator.GetInteger("Count"));
			base.animator.SetInteger("Effect", num);
		}
		RumRunnersLevelFullscreenDirtFX.PreviousEffectB = RumRunnersLevelFullscreenDirtFX.PreviousEffectA;
		RumRunnersLevelFullscreenDirtFX.PreviousEffectA = num;
		base.animator.Update(0f);
		float y = base.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
		if (num == 0 || num == 1)
		{
			base.StartCoroutine(this.fall_cr(this.loopDirtSpeed, y));
		}
		else
		{
			float currentClipLength = base.animator.GetCurrentClipLength(0);
			if (currentClipLength == 0f)
			{
				this.OnEffectComplete();
			}
			base.animator.Update(0f);
			float speed = (887.79285f + y) / currentClipLength;
			base.StartCoroutine(this.fall_cr(speed, y));
		}
	}

	// Token: 0x06002AB3 RID: 10931 RVA: 0x0018EF90 File Offset: 0x0018D390
	private IEnumerator fall_cr(float speed, float spriteHeight)
	{
		while (base.transform.position.y > -360f - spriteHeight - 100f)
		{
			yield return null;
			Vector3 position = base.transform.position;
			position.y -= speed * CupheadTime.Delta;
			base.transform.position = position;
		}
		this.OnEffectComplete();
		yield break;
	}

	// Token: 0x04003370 RID: 13168
	private static int PreviousEffectA = -1;

	// Token: 0x04003371 RID: 13169
	private static int PreviousEffectB = -1;

	// Token: 0x04003372 RID: 13170
	[SerializeField]
	private float loopDirtSpeed;
}
