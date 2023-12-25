using System;
using UnityEngine;

// Token: 0x0200037E RID: 894
[Serializable]
public class MinMax
{
	// Token: 0x06000A70 RID: 2672 RVA: 0x0007ED17 File Offset: 0x0007D117
	public MinMax(float min, float max)
	{
		this.min = min;
		this.max = max;
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x0007ED2D File Offset: 0x0007D12D
	public float RandomFloat()
	{
		return UnityEngine.Random.Range(this.min, this.max);
	}

	// Token: 0x06000A72 RID: 2674 RVA: 0x0007ED40 File Offset: 0x0007D140
	public int RandomInt()
	{
		int num = (int)this.min;
		int num2 = (int)this.max;
		return UnityEngine.Random.Range(num, num2);
	}

	// Token: 0x06000A73 RID: 2675 RVA: 0x0007ED64 File Offset: 0x0007D164
	public float GetFloatAt(float i)
	{
		return Mathf.Lerp(this.min, this.max, i);
	}

	// Token: 0x06000A74 RID: 2676 RVA: 0x0007ED78 File Offset: 0x0007D178
	public float GetIntAt(float i)
	{
		return (float)((int)Mathf.Lerp(this.min, this.max, i));
	}

	// Token: 0x06000A75 RID: 2677 RVA: 0x0007ED8E File Offset: 0x0007D18E
	public MinMax Clone()
	{
		return new MinMax(this.min, this.max);
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x0007EDA1 File Offset: 0x0007D1A1
	public static implicit operator float(MinMax m)
	{
		return m.RandomFloat();
	}

	// Token: 0x06000A77 RID: 2679 RVA: 0x0007EDA9 File Offset: 0x0007D1A9
	public static implicit operator int(MinMax m)
	{
		return m.RandomInt();
	}

	// Token: 0x04001471 RID: 5233
	public float min;

	// Token: 0x04001472 RID: 5234
	public float max;
}
