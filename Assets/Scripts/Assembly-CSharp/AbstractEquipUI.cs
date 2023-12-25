using System;
using UnityEngine;

// Token: 0x02000983 RID: 2435
public abstract class AbstractEquipUI : AbstractPauseGUI
{
	// Token: 0x1700049E RID: 1182
	// (get) Token: 0x060038DB RID: 14555 RVA: 0x00205554 File Offset: 0x00203954
	// (set) Token: 0x060038DC RID: 14556 RVA: 0x0020555B File Offset: 0x0020395B
	public static AbstractEquipUI Current { get; private set; }

	// Token: 0x1700049F RID: 1183
	// (get) Token: 0x060038DD RID: 14557 RVA: 0x00205563 File Offset: 0x00203963
	// (set) Token: 0x060038DE RID: 14558 RVA: 0x0020556B File Offset: 0x0020396B
	public AbstractEquipUI.ActiveState CurrentState { get; private set; }

	// Token: 0x170004A0 RID: 1184
	// (get) Token: 0x060038DF RID: 14559 RVA: 0x00205574 File Offset: 0x00203974
	protected override AbstractPauseGUI.InputActionSet CheckedActionSet
	{
		get
		{
			return AbstractPauseGUI.InputActionSet.UIInput;
		}
	}

	// Token: 0x170004A1 RID: 1185
	// (get) Token: 0x060038E0 RID: 14560 RVA: 0x00205577 File Offset: 0x00203977
	protected override bool CanPause
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170004A2 RID: 1186
	// (get) Token: 0x060038E1 RID: 14561 RVA: 0x0020557A File Offset: 0x0020397A
	protected override bool CanUnpause
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060038E2 RID: 14562 RVA: 0x0020557D File Offset: 0x0020397D
	protected override void InAnimation(float i)
	{
	}

	// Token: 0x060038E3 RID: 14563 RVA: 0x0020557F File Offset: 0x0020397F
	protected override void OutAnimation(float i)
	{
	}

	// Token: 0x060038E4 RID: 14564 RVA: 0x00205584 File Offset: 0x00203984
	protected override void Awake()
	{
		base.Awake();
		AbstractEquipUI.Current = this;
		this.playerTwo = UnityEngine.Object.Instantiate<MapEquipUICard>(this.playerOne);
		this.playerTwo.transform.SetParent(this.playerOne.transform.parent, false);
		this.playerTwo.Init(PlayerId.PlayerTwo, this);
		this.playerTwo.name = "PlayerTwo";
		this.playerOne.transform.SetSiblingIndex(this.playerTwo.transform.GetSiblingIndex());
		this.playerOne.Init(PlayerId.PlayerOne, this);
	}

	// Token: 0x060038E5 RID: 14565 RVA: 0x0020561C File Offset: 0x00203A1C
	private void Start()
	{
		PlayerManager.OnPlayerJoinedEvent += this.OnPlayerJoined;
		PlayerManager.OnPlayerLeaveEvent += this.OnPlayerLeft;
		if (PlayerManager.Multiplayer)
		{
			Vector2 anchoredPosition = this.playerOne.container.anchoredPosition;
			this.playerOne.container.anchoredPosition = anchoredPosition;
			this.playerTwo.container.anchoredPosition = anchoredPosition;
		}
	}

	// Token: 0x060038E6 RID: 14566 RVA: 0x00205688 File Offset: 0x00203A88
	private void OnDestroy()
	{
		PlayerManager.OnPlayerJoinedEvent -= this.OnPlayerJoined;
		PlayerManager.OnPlayerLeaveEvent -= this.OnPlayerLeft;
		if (AbstractEquipUI.Current == this)
		{
			AbstractEquipUI.Current = null;
		}
	}

	// Token: 0x060038E7 RID: 14567 RVA: 0x002056C4 File Offset: 0x00203AC4
	private void OnPlayerJoined(PlayerId playerId)
	{
		Vector2 anchoredPosition = this.playerOne.container.anchoredPosition;
		anchoredPosition.y += 10f;
		this.playerOne.container.anchoredPosition = anchoredPosition;
		this.playerTwo.container.anchoredPosition = anchoredPosition;
	}

	// Token: 0x060038E8 RID: 14568 RVA: 0x00205718 File Offset: 0x00203B18
	private void OnPlayerLeft(PlayerId playerId)
	{
		Vector2 anchoredPosition = this.playerOne.container.anchoredPosition;
		anchoredPosition.y -= 10f;
		this.playerOne.container.anchoredPosition = anchoredPosition;
		this.playerTwo.container.anchoredPosition = anchoredPosition;
	}

	// Token: 0x060038E9 RID: 14569 RVA: 0x0020576C File Offset: 0x00203B6C
	protected override void OnPause()
	{
		if (PlatformHelper.GarbageCollectOnPause)
		{
			GC.Collect();
		}
		this.OnPauseAudio();
		base.FrameDelayedCallback(new Action(this.SetStateActive), 1);
		this.playerOne.CanRotate = false;
		this.playerTwo.CanRotate = false;
		AudioManager.Play("menu_cardup");
		if (PlayerManager.Multiplayer)
		{
			this.playerOne.SetActive(true);
			this.playerTwo.SetActive(true);
			this.playerOne.SetMultiplayerOut(true);
			this.playerTwo.SetMultiplayerOut(true);
			this.playerOne.SetMultiplayerIn(false);
			this.playerTwo.SetMultiplayerIn(false);
		}
		else
		{
			this.playerOne.SetActive(true);
			this.playerTwo.SetActive(false);
			this.playerOne.SetSinglePlayerOut(true);
			this.playerOne.SetSinglePlayerIn(false);
		}
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, false, false);
		PlayerData.Data.ResetHasNewPurchase(PlayerId.Any);
	}

	// Token: 0x060038EA RID: 14570 RVA: 0x00205862 File Offset: 0x00203C62
	private void SetStateActive()
	{
		this.CurrentState = AbstractEquipUI.ActiveState.Active;
	}

	// Token: 0x060038EB RID: 14571 RVA: 0x0020586B File Offset: 0x00203C6B
	protected override void OnPauseComplete()
	{
		base.OnPauseComplete();
		this.playerOne.CanRotate = true;
		this.playerTwo.CanRotate = true;
	}

	// Token: 0x060038EC RID: 14572 RVA: 0x0020588C File Offset: 0x00203C8C
	protected override void OnUnpause()
	{
		this.OnUnpauseAudio();
		base.OnUnpause();
		this.playerOne.CanRotate = false;
		this.playerTwo.CanRotate = false;
		if (PlayerManager.Multiplayer)
		{
			this.playerOne.SetMultiplayerOut(false);
			this.playerTwo.SetMultiplayerOut(false);
		}
		else
		{
			this.playerOne.SetSinglePlayerOut(false);
		}
	}

	// Token: 0x060038ED RID: 14573 RVA: 0x002058F0 File Offset: 0x00203CF0
	protected virtual void OnPauseAudio()
	{
	}

	// Token: 0x060038EE RID: 14574 RVA: 0x002058F2 File Offset: 0x00203CF2
	protected virtual void OnUnpauseAudio()
	{
	}

	// Token: 0x060038EF RID: 14575 RVA: 0x002058F4 File Offset: 0x00203CF4
	protected override void OnUnpauseComplete()
	{
		base.OnUnpauseComplete();
		this.CurrentState = AbstractEquipUI.ActiveState.Inactive;
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerTwo, true, true);
	}

	// Token: 0x060038F0 RID: 14576 RVA: 0x0020590C File Offset: 0x00203D0C
	public bool Close()
	{
		if (PlayerManager.Multiplayer && !this.playerOne.ReadyAndWaiting && !this.playerTwo.ReadyAndWaiting)
		{
			return false;
		}
		if (Map.Current != null)
		{
			Map.Current.OnCloseEquipMenu();
		}
		AudioManager.Play("menu_carddown");
		this.Unpause();
		return true;
	}

	// Token: 0x04004082 RID: 16514
	[SerializeField]
	private MapEquipUICard playerOne;

	// Token: 0x04004083 RID: 16515
	private MapEquipUICard playerTwo;

	// Token: 0x02000984 RID: 2436
	public enum ActiveState
	{
		// Token: 0x04004086 RID: 16518
		Inactive,
		// Token: 0x04004087 RID: 16519
		Active
	}
}
