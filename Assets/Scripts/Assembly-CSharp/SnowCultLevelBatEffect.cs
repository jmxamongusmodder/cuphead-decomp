using System;
using UnityEngine;

// Token: 0x020007E8 RID: 2024
public class SnowCultLevelBatEffect : Effect
{
	// Token: 0x06002E57 RID: 11863 RVA: 0x001B4F7B File Offset: 0x001B337B
	public void SetColor(string s)
	{
		this.colorString = s;
		base.animator.Play(this.baseAnimName + s);
		if (this.secondaryRenderer)
		{
			this.secondaryRenderer.flipX = Rand.Bool();
		}
	}

	// Token: 0x040036EA RID: 14058
	[SerializeField]
	private SpriteRenderer secondaryRenderer;

	// Token: 0x040036EB RID: 14059
	[SerializeField]
	private string baseAnimName;

	// Token: 0x040036EC RID: 14060
	protected string colorString;
}
