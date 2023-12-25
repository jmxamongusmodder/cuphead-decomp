using System;
using UnityEngine;

// Token: 0x020008AB RID: 2219
public class CircusPlatformingLevelPretzelHead : MonoBehaviour
{
	// Token: 0x060033BB RID: 13243 RVA: 0x001E0908 File Offset: 0x001DED08
	private void JumpSFX()
	{
		this.circusPlatformingLevelPretzel.JumpSFX();
	}

	// Token: 0x04003C09 RID: 15369
	[SerializeField]
	private CircusPlatformingLevelPretzel circusPlatformingLevelPretzel;
}
