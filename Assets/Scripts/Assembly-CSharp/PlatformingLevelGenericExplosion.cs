using System;
using UnityEngine;

// Token: 0x02000860 RID: 2144
public class PlatformingLevelGenericExplosion : Effect
{
	// Token: 0x060031D2 RID: 12754 RVA: 0x001D1650 File Offset: 0x001CFA50
	public override Effect Create(Vector3 position, Vector3 scale)
	{
		float num = UnityEngine.Random.Range(0f, 1f);
		if (num < this.lightningOnlyChance + this.starOnlyChance + this.starsPlusLightningChance)
		{
			if (num < this.lightningOnlyChance || num > this.lightningOnlyChance + this.starOnlyChance)
			{
				this.lightningPrefab.Create(position, scale);
			}
			if (num > this.lightningOnlyChance)
			{
				this.starsPrefab.Create(position, scale);
			}
		}
		return base.Create(position, scale);
	}

	// Token: 0x060031D3 RID: 12755 RVA: 0x001D16D6 File Offset: 0x001CFAD6
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.lightningPrefab = null;
		this.starsPrefab = null;
	}

	// Token: 0x04003A2D RID: 14893
	[SerializeField]
	private Effect lightningPrefab;

	// Token: 0x04003A2E RID: 14894
	[SerializeField]
	private Effect starsPrefab;

	// Token: 0x04003A2F RID: 14895
	[SerializeField]
	private float lightningOnlyChance;

	// Token: 0x04003A30 RID: 14896
	[SerializeField]
	private float starOnlyChance;

	// Token: 0x04003A31 RID: 14897
	[SerializeField]
	private float starsPlusLightningChance;
}
