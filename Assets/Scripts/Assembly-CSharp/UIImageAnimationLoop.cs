using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000387 RID: 903
public class UIImageAnimationLoop : AbstractMonoBehaviour
{
	// Token: 0x06000AB4 RID: 2740 RVA: 0x0007FE73 File Offset: 0x0007E273
	protected override void Awake()
	{
		base.Awake();
		this.image = base.GetComponent<Image>();
		this.ignoreGlobalTime = this.IgnoreGlobalTime;
	}

	// Token: 0x06000AB5 RID: 2741 RVA: 0x0007FE93 File Offset: 0x0007E293
	private void Start()
	{
		base.StartCoroutine(this.anim_cr());
	}

	// Token: 0x06000AB6 RID: 2742 RVA: 0x0007FEA4 File Offset: 0x0007E2A4
	private void Shuffle()
	{
		List<Sprite> list = new List<Sprite>(this.sprites);
		System.Random random = new System.Random();
		int i = list.Count;
		while (i > 1)
		{
			i--;
			int index = random.Next(i + 1);
			Sprite value = list[index];
			list[index] = list[i];
			list[i] = value;
		}
		this.sprites = list.ToArray();
	}

	// Token: 0x06000AB7 RID: 2743 RVA: 0x0007FF10 File Offset: 0x0007E310
	private IEnumerator anim_cr()
	{
		if (this.mode == UIImageAnimationLoop.Mode.Shuffle)
		{
			this.Shuffle();
		}
		YieldInstruction waitInstruction = new WaitForSeconds(this.frameDelay);
		int i = 0;
		for (;;)
		{
			this.image.sprite = this.sprites[i];
			i++;
			if (i >= this.sprites.Length)
			{
				i = 0;
			}
			if (!this.IgnoreGlobalTime)
			{
				float t = 0f;
				while (t < this.frameDelay)
				{
					t += CupheadTime.Delta[CupheadTime.Layer.Default];
					yield return null;
				}
			}
			else
			{
				yield return waitInstruction;
			}
			if (this.mode == UIImageAnimationLoop.Mode.Random)
			{
				this.Shuffle();
			}
		}
		yield break;
	}

	// Token: 0x06000AB8 RID: 2744 RVA: 0x0007FF2B File Offset: 0x0007E32B
	private void OnDestroy()
	{
		this.sprites = null;
	}

	// Token: 0x04001482 RID: 5250
	[SerializeField]
	private UIImageAnimationLoop.Mode mode;

	// Token: 0x04001483 RID: 5251
	[SerializeField]
	private float frameDelay = 0.07f;

	// Token: 0x04001484 RID: 5252
	[SerializeField]
	private Sprite[] sprites;

	// Token: 0x04001485 RID: 5253
	[SerializeField]
	private bool IgnoreGlobalTime;

	// Token: 0x04001486 RID: 5254
	private Image image;

	// Token: 0x02000388 RID: 904
	public enum Mode
	{
		// Token: 0x04001488 RID: 5256
		Linear,
		// Token: 0x04001489 RID: 5257
		Shuffle,
		// Token: 0x0400148A RID: 5258
		Random
	}
}
