using System;

// Token: 0x0200045D RID: 1117
public class InterruptingPrompt : AbstractMonoBehaviour
{
	// Token: 0x060010F0 RID: 4336 RVA: 0x000A0C2C File Offset: 0x0009F02C
	public static void SetCanInterrupt(bool canInterrupt)
	{
		if (ControllerDisconnectedPrompt.Instance != null)
		{
			ControllerDisconnectedPrompt.Instance.allowedToShow = canInterrupt;
		}
	}

	// Token: 0x060010F1 RID: 4337 RVA: 0x000A0C49 File Offset: 0x0009F049
	public static bool IsInterrupting()
	{
		return ControllerDisconnectedPrompt.Instance != null && ControllerDisconnectedPrompt.Instance.Visible;
	}

	// Token: 0x060010F2 RID: 4338 RVA: 0x000A0C68 File Offset: 0x0009F068
	public static bool CanInterrupt()
	{
		return ControllerDisconnectedPrompt.Instance != null && ControllerDisconnectedPrompt.Instance.allowedToShow;
	}

	// Token: 0x170002AA RID: 682
	// (get) Token: 0x060010F3 RID: 4339 RVA: 0x000A0C86 File Offset: 0x0009F086
	public bool Visible
	{
		get
		{
			return base.gameObject.activeSelf;
		}
	}

	// Token: 0x060010F4 RID: 4340 RVA: 0x000A0C93 File Offset: 0x0009F093
	protected override void Awake()
	{
		base.Awake();
		base.gameObject.SetActive(false);
	}

	// Token: 0x060010F5 RID: 4341 RVA: 0x000A0CA7 File Offset: 0x0009F0A7
	public void Show()
	{
		base.gameObject.SetActive(true);
		this.wasPausedBeforeInterrupt = (PauseManager.state == PauseManager.State.Paused);
		if (!this.wasPausedBeforeInterrupt)
		{
			PauseManager.Pause();
		}
	}

	// Token: 0x060010F6 RID: 4342 RVA: 0x000A0CD3 File Offset: 0x0009F0D3
	public void Dismiss()
	{
		base.gameObject.SetActive(false);
		if (!this.wasPausedBeforeInterrupt)
		{
			PauseManager.Unpause();
		}
	}

	// Token: 0x04001A5C RID: 6748
	private bool wasPausedBeforeInterrupt;
}
