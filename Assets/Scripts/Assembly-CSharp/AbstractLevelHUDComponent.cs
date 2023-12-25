using System;

// Token: 0x0200048A RID: 1162
public class AbstractLevelHUDComponent : AbstractMonoBehaviour
{
	// Token: 0x170002CE RID: 718
	// (get) Token: 0x06001230 RID: 4656 RVA: 0x000A9025 File Offset: 0x000A7425
	// (set) Token: 0x06001231 RID: 4657 RVA: 0x000A902D File Offset: 0x000A742D
	private protected LevelHUDPlayer _hud { protected get; private set; }

	// Token: 0x170002CF RID: 719
	// (get) Token: 0x06001232 RID: 4658 RVA: 0x000A9036 File Offset: 0x000A7436
	protected AbstractPlayerController _player
	{
		get
		{
			return this._hud.player;
		}
	}

	// Token: 0x06001233 RID: 4659 RVA: 0x000A9043 File Offset: 0x000A7443
	protected override void Awake()
	{
		base.Awake();
		this.ignoreGlobalTime = true;
		this.timeLayer = CupheadTime.Layer.UI;
	}

	// Token: 0x06001234 RID: 4660 RVA: 0x000A9059 File Offset: 0x000A7459
	private void Start()
	{
		if (this._parentToHudCanvas)
		{
			base.transform.SetParent(LevelHUD.Current.Canvas.transform, false);
		}
	}

	// Token: 0x06001235 RID: 4661 RVA: 0x000A9081 File Offset: 0x000A7481
	public virtual void Init(LevelHUDPlayer hud)
	{
		this._hud = hud;
	}

	// Token: 0x04001BAB RID: 7083
	protected bool _parentToHudCanvas;
}
