using System;

// Token: 0x020003DB RID: 987
public class CupheadShopCamera : AbstractCupheadGameCamera
{
	// Token: 0x1700023C RID: 572
	// (get) Token: 0x06000D3E RID: 3390 RVA: 0x0008C811 File Offset: 0x0008AC11
	// (set) Token: 0x06000D3F RID: 3391 RVA: 0x0008C818 File Offset: 0x0008AC18
	public static CupheadShopCamera Current { get; private set; }

	// Token: 0x1700023D RID: 573
	// (get) Token: 0x06000D40 RID: 3392 RVA: 0x0008C820 File Offset: 0x0008AC20
	public override float OrthographicSize
	{
		get
		{
			return 360f;
		}
	}

	// Token: 0x06000D41 RID: 3393 RVA: 0x0008C827 File Offset: 0x0008AC27
	protected override void Awake()
	{
		base.Awake();
		CupheadShopCamera.Current = this;
	}

	// Token: 0x06000D42 RID: 3394 RVA: 0x0008C835 File Offset: 0x0008AC35
	private void OnDestroy()
	{
		if (CupheadShopCamera.Current == this)
		{
			CupheadShopCamera.Current = null;
		}
	}
}
