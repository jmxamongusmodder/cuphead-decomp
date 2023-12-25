using System;
using UnityEngine;

// Token: 0x02000B33 RID: 2867
public class WinScreenCharacterAnimation : AbstractMonoBehaviour
{
	// Token: 0x06004579 RID: 17785 RVA: 0x00249C78 File Offset: 0x00248078
	private void Start()
	{
		this.blinkLayer.enabled = false;
		if (this.blinkLayer2 != null)
		{
			this.blinkLayer2.enabled = false;
		}
		this.results.SetBool("pickedA", Rand.Bool());
		this.singleBlinkAmount = UnityEngine.Random.Range(0, 4) + 4;
		this.doubleBlinkAmount = UnityEngine.Random.Range(0, 10) + 16;
	}

	// Token: 0x0600457A RID: 17786 RVA: 0x00249CE4 File Offset: 0x002480E4
	private void EndCycle()
	{
		this.blinkLayer.enabled = false;
		if (this.blinkLayer2 != null)
		{
			this.blinkLayer2.enabled = false;
		}
		if (this.singleCount < this.singleBlinkAmount)
		{
			this.singleCount++;
		}
		else
		{
			if (this.blinkLayer2 != null)
			{
				if (this.is2Player)
				{
					this.blinkLayer2.enabled = true;
				}
				else
				{
					this.blinkLayer.enabled = true;
				}
				this.is2Player = !this.is2Player;
			}
			else
			{
				this.blinkLayer.enabled = true;
			}
			this.singleCount = 0;
			this.singleBlinkAmount = UnityEngine.Random.Range(0, 4) + 4;
		}
		if (this.blinkLayer2 != null)
		{
			if (this.doubleCount < this.doubleBlinkAmount)
			{
				this.doubleCount++;
			}
			else
			{
				this.blinkLayer2.enabled = true;
				this.blinkLayer.enabled = true;
				this.doubleCount = 0;
				this.doubleBlinkAmount = UnityEngine.Random.Range(0, 10) + 16;
			}
		}
	}

	// Token: 0x04004B92 RID: 19346
	[SerializeField]
	private Animator results;

	// Token: 0x04004B93 RID: 19347
	[SerializeField]
	private SpriteRenderer blinkLayer;

	// Token: 0x04004B94 RID: 19348
	[SerializeField]
	private SpriteRenderer blinkLayer2;

	// Token: 0x04004B95 RID: 19349
	public bool is2Player;

	// Token: 0x04004B96 RID: 19350
	private int singleBlinkAmount;

	// Token: 0x04004B97 RID: 19351
	private int doubleBlinkAmount;

	// Token: 0x04004B98 RID: 19352
	private int singleCount;

	// Token: 0x04004B99 RID: 19353
	private int doubleCount;

	// Token: 0x04004B9A RID: 19354
	private bool p1Turn;
}
