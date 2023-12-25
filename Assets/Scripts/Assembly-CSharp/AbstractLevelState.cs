using System;
using UnityEngine;

// Token: 0x0200047B RID: 1147
public abstract class AbstractLevelState<PATTERN, STATE_NAMES>
{
	// Token: 0x060011AC RID: 4524 RVA: 0x00008AC4 File Offset: 0x00006EC4
	public AbstractLevelState(float healthTrigger, PATTERN[][] patterns, STATE_NAMES stateName)
	{
		this.healthTrigger = Mathf.Clamp(healthTrigger, 0f, 1f);
		this.patterns = patterns[UnityEngine.Random.Range(0, patterns.Length)];
		this.patternIndex = UnityEngine.Random.Range(0, this.patterns.Length);
		this.stateName = stateName;
	}

	// Token: 0x170002C0 RID: 704
	// (get) Token: 0x060011AD RID: 4525 RVA: 0x00008B19 File Offset: 0x00006F19
	public PATTERN NextPattern
	{
		get
		{
			this.patternIndex++;
			if (this.patternIndex >= this.patterns.Length)
			{
				this.patternIndex = 0;
			}
			return this.patterns[this.patternIndex];
		}
	}

	// Token: 0x170002C1 RID: 705
	// (get) Token: 0x060011AE RID: 4526 RVA: 0x00008B54 File Offset: 0x00006F54
	public PATTERN PeekNextPattern
	{
		get
		{
			int num = this.patternIndex + 1;
			if (num >= this.patterns.Length)
			{
				num = 0;
			}
			return this.patterns[num];
		}
	}

	// Token: 0x170002C2 RID: 706
	// (get) Token: 0x060011AF RID: 4527 RVA: 0x00008B86 File Offset: 0x00006F86
	public PATTERN CurrentPattern
	{
		get
		{
			return this.patterns[this.patternIndex];
		}
	}

	// Token: 0x04001B30 RID: 6960
	public readonly float healthTrigger;

	// Token: 0x04001B31 RID: 6961
	public readonly PATTERN[] patterns;

	// Token: 0x04001B32 RID: 6962
	public readonly STATE_NAMES stateName;

	// Token: 0x04001B33 RID: 6963
	private int patternIndex;
}
