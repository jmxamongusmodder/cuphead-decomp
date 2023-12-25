using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000B2C RID: 2860
public class WaitForFrameTimePersistent : IEnumerator
{
	// Token: 0x06004559 RID: 17753 RVA: 0x00247EF9 File Offset: 0x002462F9
	public WaitForFrameTimePersistent(float frameTime, bool useUnalteredTime = false)
	{
		this.frameTime = frameTime;
		this.useUnalteredTime = useUnalteredTime;
	}

	// Token: 0x1700062D RID: 1581
	// (get) Token: 0x0600455A RID: 17754 RVA: 0x00247F0F File Offset: 0x0024630F
	// (set) Token: 0x0600455B RID: 17755 RVA: 0x00247F17 File Offset: 0x00246317
	public float accumulator { get; protected set; }

	// Token: 0x1700062E RID: 1582
	// (get) Token: 0x0600455C RID: 17756 RVA: 0x00247F20 File Offset: 0x00246320
	// (set) Token: 0x0600455D RID: 17757 RVA: 0x00247F28 File Offset: 0x00246328
	public float frameTime { get; protected set; }

	// Token: 0x1700062F RID: 1583
	// (get) Token: 0x0600455E RID: 17758 RVA: 0x00247F31 File Offset: 0x00246331
	public float totalDelta
	{
		get
		{
			return this.frameTime + this.accumulator;
		}
	}

	// Token: 0x17000630 RID: 1584
	// (get) Token: 0x0600455F RID: 17759 RVA: 0x00247F40 File Offset: 0x00246340
	public virtual object Current
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06004560 RID: 17760 RVA: 0x00247F44 File Offset: 0x00246344
	public bool MoveNext()
	{
		this.accumulator += this.deltaTime();
		bool flag = this.accumulator >= this.frameTime;
		if (flag)
		{
			this.accumulator -= Mathf.Floor(this.accumulator / this.frameTime) * this.frameTime;
		}
		return !flag;
	}

	// Token: 0x06004561 RID: 17761 RVA: 0x00247FA6 File Offset: 0x002463A6
	protected virtual float deltaTime()
	{
		return (!this.useUnalteredTime) ? CupheadTime.Delta : Time.deltaTime;
	}

	// Token: 0x06004562 RID: 17762 RVA: 0x00247FC7 File Offset: 0x002463C7
	public void Reset()
	{
		this.accumulator = 0f;
	}

	// Token: 0x04004AF5 RID: 19189
	private bool useUnalteredTime;
}
