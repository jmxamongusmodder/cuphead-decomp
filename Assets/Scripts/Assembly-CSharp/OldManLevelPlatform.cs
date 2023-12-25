using System;
using UnityEngine;

// Token: 0x0200070A RID: 1802
[Serializable]
public class OldManLevelPlatform
{
	// Token: 0x04002F8A RID: 12170
	public Transform platform;

	// Token: 0x04002F8B RID: 12171
	public Transform sockBulletPos;

	// Token: 0x04002F8C RID: 12172
	public bool isMoving;

	// Token: 0x04002F8D RID: 12173
	public bool removed;

	// Token: 0x04002F8E RID: 12174
	public float effectiveVel;

	// Token: 0x04002F8F RID: 12175
	public OldManLevelGnomeClimber activeClimber;
}
