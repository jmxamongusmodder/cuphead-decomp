using System;
using UnityEngine;

// Token: 0x020007CF RID: 1999
public class SaltbakerLevelMintHandSFXHandler : MonoBehaviour
{
	// Token: 0x06002D64 RID: 11620 RVA: 0x001AC9A4 File Offset: 0x001AADA4
	private void AniEvent_SFXLeafRustle()
	{
		this.main.SFXLeafRustle();
	}

	// Token: 0x06002D65 RID: 11621 RVA: 0x001AC9B1 File Offset: 0x001AADB1
	private void AniEvent_SFXLaunchThrow()
	{
		this.main.SFXLaunchThrow();
	}

	// Token: 0x040035EC RID: 13804
	[SerializeField]
	private SaltbakerLevelSaltbaker main;
}
