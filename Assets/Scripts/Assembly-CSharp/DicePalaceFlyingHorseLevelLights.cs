using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005C3 RID: 1475
public class DicePalaceFlyingHorseLevelLights : AbstractPausableComponent
{
	// Token: 0x06001CC3 RID: 7363 RVA: 0x00107803 File Offset: 0x00105C03
	private void Start()
	{
		base.FrameDelayedCallback(new Action(this.GetSprites), 1);
	}

	// Token: 0x06001CC4 RID: 7364 RVA: 0x00107819 File Offset: 0x00105C19
	private void GetSprites()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001CC5 RID: 7365 RVA: 0x00107828 File Offset: 0x00105C28
	private IEnumerator move_cr()
	{
		for (;;)
		{
			if (base.transform.position.x > -640f - this.size)
			{
				base.transform.position += Vector3.left * this.speed * CupheadTime.Delta;
			}
			else
			{
				base.transform.position = new Vector3(640f + this.size, base.transform.position.y, 0f);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x040025B1 RID: 9649
	[SerializeField]
	private float size = 500f;

	// Token: 0x040025B2 RID: 9650
	[SerializeField]
	private float speed = 30f;
}
