using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200045B RID: 1115
public class DLCGUI : AbstractMonoBehaviour
{
	// Token: 0x170002A6 RID: 678
	// (get) Token: 0x060010DC RID: 4316 RVA: 0x000A1D82 File Offset: 0x000A0182
	// (set) Token: 0x060010DD RID: 4317 RVA: 0x000A1D8A File Offset: 0x000A018A
	public bool dlcMenuOpen { get; private set; }

	// Token: 0x170002A7 RID: 679
	// (get) Token: 0x060010DE RID: 4318 RVA: 0x000A1D93 File Offset: 0x000A0193
	// (set) Token: 0x060010DF RID: 4319 RVA: 0x000A1D9B File Offset: 0x000A019B
	public bool inputEnabled { get; private set; }

	// Token: 0x170002A8 RID: 680
	// (get) Token: 0x060010E0 RID: 4320 RVA: 0x000A1DA4 File Offset: 0x000A01A4
	// (set) Token: 0x060010E1 RID: 4321 RVA: 0x000A1DAC File Offset: 0x000A01AC
	public bool justClosed { get; private set; }

	// Token: 0x060010E2 RID: 4322 RVA: 0x000A1DB5 File Offset: 0x000A01B5
	protected override void Awake()
	{
		base.Awake();
		this.dlcMenuOpen = false;
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		this.canvasGroup.alpha = 0f;
	}

	// Token: 0x060010E3 RID: 4323 RVA: 0x000A1DE0 File Offset: 0x000A01E0
	public void Init(bool checkIfDead)
	{
		this.input = new CupheadInput.AnyPlayerInput(checkIfDead);
	}

	// Token: 0x060010E4 RID: 4324 RVA: 0x000A1DF0 File Offset: 0x000A01F0
	private void Update()
	{
		this.justClosed = false;
		this.timeSinceStart += Time.deltaTime;
		this.timeSinceConfirmPressed += Time.deltaTime;
		if (this.timeSinceStart < 0.25f)
		{
			return;
		}
		if (!this.inputEnabled)
		{
			return;
		}
		if (this.GetButtonDown(CupheadButton.Cancel))
		{
			base.StartCoroutine(this.hide_cr());
			return;
		}
		if (!this.dlcEnabled && this.timeSinceConfirmPressed >= 0.5f && DLCManager.CanRedirectToStore() && this.GetButtonDown(CupheadButton.Accept))
		{
			this.timeSinceConfirmPressed = 0f;
			DLCManager.LaunchStore();
			return;
		}
	}

	// Token: 0x060010E5 RID: 4325 RVA: 0x000A1EA4 File Offset: 0x000A02A4
	public void ShowDLCMenu()
	{
		this.dlcEnabled = DLCManager.DLCEnabled();
		this.timeSinceStart = 0f;
		this.timeSinceConfirmPressed = 0f;
		this.dlcMenuOpen = true;
		this.canvasGroup.alpha = 1f;
		base.StartCoroutine(this.show_cr());
	}

	// Token: 0x060010E6 RID: 4326 RVA: 0x000A1EF6 File Offset: 0x000A02F6
	private void hideDLCMenu()
	{
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
		this.inputEnabled = false;
		this.dlcMenuOpen = false;
		this.justClosed = true;
	}

	// Token: 0x060010E7 RID: 4327 RVA: 0x000A1F35 File Offset: 0x000A0335
	private void interactable()
	{
		this.canvasGroup.interactable = true;
		this.canvasGroup.blocksRaycasts = true;
		this.inputEnabled = true;
	}

	// Token: 0x060010E8 RID: 4328 RVA: 0x000A1F58 File Offset: 0x000A0358
	private IEnumerator show_cr()
	{
		Transform scaler;
		Text text;
		if (this.dlcEnabled)
		{
			scaler = this.installedScaler;
			this.notInstalled.SetActive(false);
			this.installed.SetActive(true);
			text = this.installedText;
		}
		else
		{
			scaler = this.notInstalledScaler;
			this.notInstalled.SetActive(true);
			this.installed.SetActive(false);
			text = this.notInstalledText;
		}
		this.fader.color = new Color(0f, 0f, 0f, DLCGUI.FaderAlpha);
		Image[] fadeImages = scaler.GetComponentsInChildren<Image>();
		foreach (Image image in fadeImages)
		{
			Color color2 = image.color;
			color2.a = 1f;
			image.color = color2;
		}
		float elapsedTime = 0f;
		while (elapsedTime < 0.4f)
		{
			elapsedTime += CupheadTime.Delta;
			Vector3 scale = scaler.localScale;
			scale.x = (scale.y = EaseUtils.EaseOutCubic(2f, 1f, elapsedTime / 0.4f));
			scaler.localScale = scale;
			Color color = text.color;
			color.a = Mathf.Lerp(0f, 1f, elapsedTime / 0.4f);
			text.color = color;
			yield return null;
		}
		this.interactable();
		yield break;
	}

	// Token: 0x060010E9 RID: 4329 RVA: 0x000A1F74 File Offset: 0x000A0374
	private IEnumerator hide_cr()
	{
		this.canvasGroup.interactable = false;
		this.canvasGroup.blocksRaycasts = false;
		this.inputEnabled = false;
		Transform scaler;
		Text text;
		if (this.dlcEnabled)
		{
			scaler = this.installedScaler;
			this.notInstalled.SetActive(false);
			this.installed.SetActive(true);
			text = this.installedText;
		}
		else
		{
			scaler = this.notInstalledScaler;
			this.notInstalled.SetActive(true);
			this.installed.SetActive(false);
			text = this.notInstalledText;
		}
		Image[] fadeImages = scaler.GetComponentsInChildren<Image>();
		float elapsedTime = 0f;
		while (elapsedTime < 0.2f)
		{
			elapsedTime += CupheadTime.Delta;
			Vector3 scale = scaler.localScale;
			scale.x = (scale.y = EaseUtils.EaseInCubic(1f, 2f, elapsedTime / 0.2f));
			scaler.localScale = scale;
			Color color = text.color;
			color.a = Mathf.Lerp(1f, 0f, elapsedTime / 0.2f);
			text.color = color;
			foreach (Image image in fadeImages)
			{
				color = image.color;
				color.a = Mathf.Lerp(1f, 0f, elapsedTime / 0.2f);
				image.color = color;
			}
			color = this.fader.color;
			color.a = Mathf.Lerp(DLCGUI.FaderAlpha, 0f, elapsedTime / 0.2f);
			this.fader.color = color;
			yield return null;
		}
		this.hideDLCMenu();
		yield break;
	}

	// Token: 0x060010EA RID: 4330 RVA: 0x000A1F8F File Offset: 0x000A038F
	protected bool GetButtonDown(CupheadButton button)
	{
		if (this.input.GetButtonDown(button))
		{
			AudioManager.Play("level_menu_select");
			return true;
		}
		return false;
	}

	// Token: 0x04001A48 RID: 6728
	private static readonly float FaderAlpha = 0.5f;

	// Token: 0x04001A49 RID: 6729
	[SerializeField]
	private GameObject notInstalled;

	// Token: 0x04001A4A RID: 6730
	[SerializeField]
	private GameObject installed;

	// Token: 0x04001A4B RID: 6731
	[SerializeField]
	private Transform notInstalledScaler;

	// Token: 0x04001A4C RID: 6732
	[SerializeField]
	private Transform installedScaler;

	// Token: 0x04001A4D RID: 6733
	[SerializeField]
	private Image fader;

	// Token: 0x04001A4E RID: 6734
	[SerializeField]
	private Text notInstalledText;

	// Token: 0x04001A4F RID: 6735
	[SerializeField]
	private Text installedText;

	// Token: 0x04001A50 RID: 6736
	private CanvasGroup canvasGroup;

	// Token: 0x04001A51 RID: 6737
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x04001A52 RID: 6738
	private float timeSinceStart;

	// Token: 0x04001A53 RID: 6739
	private float timeSinceConfirmPressed;

	// Token: 0x04001A54 RID: 6740
	private bool dlcEnabled;
}
