using System;

// Token: 0x020009C9 RID: 2505
public class SoundEmitter
{
	// Token: 0x06003AE4 RID: 15076 RVA: 0x0021256D File Offset: 0x0021096D
	public SoundEmitter(AbstractPausableComponent parent)
	{
		this.parent = parent;
	}

	// Token: 0x06003AE5 RID: 15077 RVA: 0x0021257C File Offset: 0x0021097C
	public void Add(string key)
	{
		this.parent.EmitSound(key);
	}

	// Token: 0x040042A3 RID: 17059
	private AbstractPausableComponent parent;
}
