using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200057B RID: 1403
public class DevilLevelPitchforkProjectileSpawner
{
	// Token: 0x06001AB4 RID: 6836 RVA: 0x000F4DFA File Offset: 0x000F31FA
	public DevilLevelPitchforkProjectileSpawner(int numProjectiles, string angleOffsets)
	{
		this.numProjectiles = numProjectiles;
		this.angleOffsets = angleOffsets.Split(new char[]
		{
			','
		});
		this.angleOffsetIndex = UnityEngine.Random.Range(0, angleOffsets.Length);
	}

	// Token: 0x06001AB5 RID: 6837 RVA: 0x000F4E34 File Offset: 0x000F3234
	public List<float> getSpawnAngles()
	{
		List<float> list = new List<float>();
		this.angleOffsetIndex = (this.angleOffsetIndex + 1) % this.angleOffsets.Length;
		float num = 0f;
		Parser.FloatTryParse(this.angleOffsets[this.angleOffsetIndex], out num);
		for (int i = 0; i < this.numProjectiles; i++)
		{
			list.Add((float)i * 360f / (float)this.numProjectiles + num + 90f);
		}
		return list;
	}

	// Token: 0x040023E1 RID: 9185
	private int numProjectiles;

	// Token: 0x040023E2 RID: 9186
	private string[] angleOffsets;

	// Token: 0x040023E3 RID: 9187
	private int angleOffsetIndex;
}
