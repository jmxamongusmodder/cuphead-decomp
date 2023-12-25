using System;
using UnityEngine;

// Token: 0x02000666 RID: 1638
public class FlyingGenieLevelConfettiParts : SpriteDeathParts
{
	// Token: 0x06002222 RID: 8738 RVA: 0x0013DF0C File Offset: 0x0013C30C
	public FlyingGenieLevelConfettiParts CreatePart(Vector3 position, Color purpleColor)
	{
		FlyingGenieLevelConfettiParts flyingGenieLevelConfettiParts = base.CreatePart(position) as FlyingGenieLevelConfettiParts;
		flyingGenieLevelConfettiParts.purpleVersion.color = purpleColor;
		return flyingGenieLevelConfettiParts;
	}

	// Token: 0x04002AC9 RID: 10953
	[SerializeField]
	private SpriteRenderer purpleVersion;
}
