using System;
using UnityEngine;

// Token: 0x020005F4 RID: 1524
public class DragonLevelLightning : AbstractPausableComponent
{
	// Token: 0x06001E5E RID: 7774 RVA: 0x00118430 File Offset: 0x00116830
	public void PlayLightning()
	{
		int num = UnityEngine.Random.Range(1, 11);
		base.animator.SetInteger("LightningID", num);
		base.animator.SetTrigger("Continue");
		num = UnityEngine.Random.Range(0, this.layerOrder.Length);
		this.spriteRenderer.sortingOrder = this.layerOrder[num];
		AudioManager.Play("level_dragon_amb_thunder");
	}

	// Token: 0x0400273C RID: 10044
	private readonly int[] layerOrder = new int[]
	{
		91,
		93,
		95
	};

	// Token: 0x0400273D RID: 10045
	[SerializeField]
	private SpriteRenderer spriteRenderer;
}
