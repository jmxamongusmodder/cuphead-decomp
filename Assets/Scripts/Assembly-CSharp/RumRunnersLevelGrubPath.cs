using System;
using UnityEngine;

// Token: 0x0200078F RID: 1935
public class RumRunnersLevelGrubPath : MonoBehaviour
{
	// Token: 0x06002ADC RID: 10972 RVA: 0x0018FFE9 File Offset: 0x0018E3E9
	public Vector2 GetPoint(float t)
	{
		return Vector2.LerpUnclamped(Vector2.LerpUnclamped(this.start, this.controlPoint, t), Vector2.LerpUnclamped(this.controlPoint, base.transform.position, t), t);
	}

	// Token: 0x0400339A RID: 13210
	public Vector2 start;

	// Token: 0x0400339B RID: 13211
	public Vector2 controlPoint;

	// Token: 0x0400339C RID: 13212
	public float forceFGSet = 2f;
}
