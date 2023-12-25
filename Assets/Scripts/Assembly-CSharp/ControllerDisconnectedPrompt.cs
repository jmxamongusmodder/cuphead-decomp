using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000457 RID: 1111
public class ControllerDisconnectedPrompt : InterruptingPrompt
{
	// Token: 0x060010CA RID: 4298 RVA: 0x000A0CF9 File Offset: 0x0009F0F9
	protected override void Awake()
	{
		base.Awake();
		ControllerDisconnectedPrompt.Instance = this;
	}

	// Token: 0x060010CB RID: 4299 RVA: 0x000A0D07 File Offset: 0x0009F107
	public void Show(PlayerId player)
	{
		this.currentPlayer = player;
		this.localizationHelper.currentID = Localization.Find((player != PlayerId.PlayerOne) ? "XboxPlayer2" : "XboxPlayer1").id;
		PlayerManager.OnDisconnectPromptDisplayed(player);
		base.Show();
	}

	// Token: 0x060010CC RID: 4300 RVA: 0x000A0D46 File Offset: 0x0009F146
	private void Update()
	{
		if (base.Visible && !PlayerManager.IsControllerDisconnected(this.currentPlayer, true))
		{
			base.FrameDelayedCallback(new Action(base.Dismiss), 2);
		}
	}

	// Token: 0x060010CD RID: 4301 RVA: 0x000A0D78 File Offset: 0x0009F178
	private void OnDestroy()
	{
		ControllerDisconnectedPrompt.Instance = null;
	}

	// Token: 0x04001A12 RID: 6674
	public static ControllerDisconnectedPrompt Instance;

	// Token: 0x04001A13 RID: 6675
	public PlayerId currentPlayer;

	// Token: 0x04001A14 RID: 6676
	public bool allowedToShow;

	// Token: 0x04001A15 RID: 6677
	[SerializeField]
	private Text playerText;

	// Token: 0x04001A16 RID: 6678
	[SerializeField]
	private LocalizationHelper localizationHelper;
}
