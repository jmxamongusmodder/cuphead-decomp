using System;
using UnityEngine;

// Token: 0x02000673 RID: 1651
public class FlyingGenieLevelMummyDeathEffect : Effect
{
	// Token: 0x060022BD RID: 8893 RVA: 0x001463E8 File Offset: 0x001447E8
	public FlyingGenieLevelMummyDeathEffect Create(Vector3 pos, Color purpleColor)
	{
		FlyingGenieLevelMummyDeathEffect flyingGenieLevelMummyDeathEffect = base.Create(pos) as FlyingGenieLevelMummyDeathEffect;
		flyingGenieLevelMummyDeathEffect.transform.position = pos;
		flyingGenieLevelMummyDeathEffect.purpleColor = purpleColor;
		return flyingGenieLevelMummyDeathEffect;
	}

	// Token: 0x060022BE RID: 8894 RVA: 0x00146418 File Offset: 0x00144818
	private void CreateConfetti()
	{
		foreach (FlyingGenieLevelConfettiParts flyingGenieLevelConfettiParts in this.parts)
		{
			flyingGenieLevelConfettiParts.CreatePart(base.transform.position, this.purpleColor);
		}
	}

	// Token: 0x04002B5B RID: 11099
	[SerializeField]
	private FlyingGenieLevelConfettiParts[] parts;

	// Token: 0x04002B5C RID: 11100
	private Color purpleColor;
}
