using System;
using System.Collections;
using System.Diagnostics;
using Rewired;
using UnityEngine;

// Token: 0x02000981 RID: 2433
public class AbstractMapSceneStartUI : AbstractMonoBehaviour
{
	// Token: 0x1700049C RID: 1180
	// (get) Token: 0x060038C8 RID: 14536 RVA: 0x00205155 File Offset: 0x00203555
	// (set) Token: 0x060038C9 RID: 14537 RVA: 0x0020515D File Offset: 0x0020355D
	public AbstractMapSceneStartUI.State CurrentState { get; protected set; }

	// Token: 0x1400006E RID: 110
	// (add) Token: 0x060038CA RID: 14538 RVA: 0x00205168 File Offset: 0x00203568
	// (remove) Token: 0x060038CB RID: 14539 RVA: 0x002051A0 File Offset: 0x002035A0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnLoadLevelEvent;

	// Token: 0x1400006F RID: 111
	// (add) Token: 0x060038CC RID: 14540 RVA: 0x002051D8 File Offset: 0x002035D8
	// (remove) Token: 0x060038CD RID: 14541 RVA: 0x00205210 File Offset: 0x00203610
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnBackEvent;

	// Token: 0x1700049D RID: 1181
	// (get) Token: 0x060038CE RID: 14542 RVA: 0x00205248 File Offset: 0x00203648
	protected bool Able
	{
		get
		{
			return this.CurrentState == AbstractMapSceneStartUI.State.Active && AbstractEquipUI.Current.CurrentState == AbstractEquipUI.ActiveState.Inactive && Map.Current.CurrentState == Map.State.Ready && !InterruptingPrompt.IsInterrupting() && (!(Map.Current != null) || Map.Current.CurrentState != Map.State.Graveyard);
		}
	}

	// Token: 0x060038CF RID: 14543 RVA: 0x002052B5 File Offset: 0x002036B5
	protected override void Awake()
	{
		base.Awake();
		this.timeLayer = CupheadTime.Layer.UI;
		this.ignoreGlobalTime = true;
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		this.SetAlpha(0f);
	}

	// Token: 0x060038D0 RID: 14544 RVA: 0x002052E2 File Offset: 0x002036E2
	protected virtual void Start()
	{
		PlayerManager.OnPlayerJoinedEvent += this.OnPlayerJoined;
		PlayerManager.OnPlayerJoinedEvent += this.OnPlayerLeft;
	}

	// Token: 0x060038D1 RID: 14545 RVA: 0x00205306 File Offset: 0x00203706
	private void OnDestroy()
	{
		PlayerManager.OnPlayerJoinedEvent -= this.OnPlayerJoined;
		PlayerManager.OnPlayerJoinedEvent -= this.OnPlayerLeft;
	}

	// Token: 0x060038D2 RID: 14546 RVA: 0x0020532A File Offset: 0x0020372A
	protected bool GetButtonDown(CupheadButton button)
	{
		return this.Able && !InterruptingPrompt.IsInterrupting() && (this.player != null && this.player.GetButtonDown((int)button));
	}

	// Token: 0x060038D3 RID: 14547 RVA: 0x00205362 File Offset: 0x00203762
	private void OnPlayerJoined(PlayerId playerId)
	{
	}

	// Token: 0x060038D4 RID: 14548 RVA: 0x00205364 File Offset: 0x00203764
	private void OnPlayerLeft(PlayerId playerId)
	{
	}

	// Token: 0x060038D5 RID: 14549 RVA: 0x00205366 File Offset: 0x00203766
	protected void LoadLevel()
	{
		this.CurrentState = AbstractMapSceneStartUI.State.Loading;
		if (this.OnLoadLevelEvent != null)
		{
			this.OnLoadLevelEvent();
		}
		this.OnLoadLevelEvent = null;
	}

	// Token: 0x060038D6 RID: 14550 RVA: 0x0020538C File Offset: 0x0020378C
	public void In(MapPlayerController playerController)
	{
		this.player = playerController.input.actions;
		base.StartCoroutine(this.fade_cr(1f, AbstractMapSceneStartUI.State.Active));
	}

	// Token: 0x060038D7 RID: 14551 RVA: 0x002053B2 File Offset: 0x002037B2
	public void Out()
	{
		base.StartCoroutine(this.fade_cr(0f, AbstractMapSceneStartUI.State.Inactive));
		if (this.OnBackEvent != null)
		{
			this.OnBackEvent();
		}
		this.OnBackEvent = null;
	}

	// Token: 0x060038D8 RID: 14552 RVA: 0x002053E4 File Offset: 0x002037E4
	protected void SetAlpha(float alpha)
	{
		this.canvasGroup.alpha = alpha;
	}

	// Token: 0x060038D9 RID: 14553 RVA: 0x002053F4 File Offset: 0x002037F4
	private IEnumerator fade_cr(float end, AbstractMapSceneStartUI.State endState)
	{
		float t = 0f;
		float start = this.canvasGroup.alpha;
		this.CurrentState = AbstractMapSceneStartUI.State.Animating;
		while (t < 0.2f)
		{
			float val = t / 0.2f;
			this.SetAlpha(Mathf.Lerp(start, end, val));
			t += Time.deltaTime;
			yield return null;
		}
		this.SetAlpha(end);
		this.CurrentState = endState;
		yield break;
	}

	// Token: 0x04004076 RID: 16502
	[HideInInspector]
	public string level;

	// Token: 0x04004078 RID: 16504
	private CanvasGroup canvasGroup;

	// Token: 0x04004079 RID: 16505
	private Player player;

	// Token: 0x02000982 RID: 2434
	public enum State
	{
		// Token: 0x0400407D RID: 16509
		Inactive,
		// Token: 0x0400407E RID: 16510
		Animating,
		// Token: 0x0400407F RID: 16511
		Active,
		// Token: 0x04004080 RID: 16512
		Loading
	}
}
