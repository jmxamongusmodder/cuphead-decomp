using System;

// Token: 0x020009D4 RID: 2516
public abstract class AbstractArcadePlayerComponent : AbstractPlayerComponent
{
	// Token: 0x170004E9 RID: 1257
	// (get) Token: 0x06003B41 RID: 15169 RVA: 0x00214070 File Offset: 0x00212470
	// (set) Token: 0x06003B42 RID: 15170 RVA: 0x00214078 File Offset: 0x00212478
	public ArcadePlayerController player { get; private set; }

	// Token: 0x06003B43 RID: 15171 RVA: 0x00214081 File Offset: 0x00212481
	protected override void OnAwake()
	{
		base.OnAwake();
		this.player = (base.basePlayer as ArcadePlayerController);
	}
}
