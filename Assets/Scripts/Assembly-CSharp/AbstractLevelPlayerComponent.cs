using System;

// Token: 0x02000A0F RID: 2575
public abstract class AbstractLevelPlayerComponent : AbstractPlayerComponent
{
	// Token: 0x17000526 RID: 1318
	// (get) Token: 0x06003CDD RID: 15581 RVA: 0x0021AD43 File Offset: 0x00219143
	// (set) Token: 0x06003CDE RID: 15582 RVA: 0x0021AD4B File Offset: 0x0021914B
	public LevelPlayerController player { get; private set; }

	// Token: 0x06003CDF RID: 15583 RVA: 0x0021AD54 File Offset: 0x00219154
	protected override void OnAwake()
	{
		base.OnAwake();
		this.player = (base.basePlayer as LevelPlayerController);
	}
}
