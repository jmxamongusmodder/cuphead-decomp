using System;

// Token: 0x02000A90 RID: 2704
public abstract class AbstractPlanePlayerComponent : AbstractPlayerComponent
{
	// Token: 0x1700059E RID: 1438
	// (get) Token: 0x060040A4 RID: 16548 RVA: 0x00232B8C File Offset: 0x00230F8C
	// (set) Token: 0x060040A5 RID: 16549 RVA: 0x00232B94 File Offset: 0x00230F94
	public PlanePlayerController player { get; private set; }

	// Token: 0x060040A6 RID: 16550 RVA: 0x00232B9D File Offset: 0x00230F9D
	protected override void OnAwake()
	{
		base.OnAwake();
		this.player = base.GetComponent<PlanePlayerController>();
	}
}
