using System;
using UnityEngine;

// Token: 0x0200048B RID: 1163
public class LevelHUD : AbstractMonoBehaviour
{
	// Token: 0x170002D0 RID: 720
	// (get) Token: 0x06001237 RID: 4663 RVA: 0x000A9092 File Offset: 0x000A7492
	// (set) Token: 0x06001238 RID: 4664 RVA: 0x000A9099 File Offset: 0x000A7499
	public static LevelHUD Current { get; private set; }

	// Token: 0x170002D1 RID: 721
	// (get) Token: 0x06001239 RID: 4665 RVA: 0x000A90A1 File Offset: 0x000A74A1
	public Canvas Canvas
	{
		get
		{
			return this.canvas;
		}
	}

	// Token: 0x0600123A RID: 4666 RVA: 0x000A90AC File Offset: 0x000A74AC
	protected override void Awake()
	{
		base.Awake();
		LevelGUI.DebugOnDisableGuiEvent += this.OnDisableGUI;
		PlayerManager.OnPlayerJoinedEvent += this.OnPlayerJoined;
		PlayerManager.OnPlayerLeaveEvent += this.OnPlayerLeave;
		LevelHUD.Current = this;
	}

	// Token: 0x0600123B RID: 4667 RVA: 0x000A90F8 File Offset: 0x000A74F8
	private void OnDestroy()
	{
		LevelGUI.DebugOnDisableGuiEvent -= this.OnDisableGUI;
		PlayerManager.OnPlayerJoinedEvent -= this.OnPlayerJoined;
		PlayerManager.OnPlayerLeaveEvent -= this.OnPlayerLeave;
		if (LevelHUD.Current == this)
		{
			LevelHUD.Current = null;
		}
	}

	// Token: 0x0600123C RID: 4668 RVA: 0x000A914E File Offset: 0x000A754E
	private void Start()
	{
		this.canvas.worldCamera = CupheadLevelCamera.Current.camera;
	}

	// Token: 0x0600123D RID: 4669 RVA: 0x000A9168 File Offset: 0x000A7568
	public void LevelInit()
	{
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		this.levelHudTemplate = UnityEngine.Object.Instantiate<LevelHUDPlayer>(this.cuphead);
		this.levelHudTemplate.gameObject.SetActive(false);
		if (PlayerManager.Multiplayer)
		{
			AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			this.mugman = UnityEngine.Object.Instantiate<LevelHUDPlayer>(this.levelHudTemplate);
			this.mugman.gameObject.SetActive(true);
			this.mugman.transform.SetParent(this.cuphead.transform.parent, false);
			this.mugman.Init(player2, false);
		}
		this.cuphead.Init(player, false);
	}

	// Token: 0x0600123E RID: 4670 RVA: 0x000A920C File Offset: 0x000A760C
	private void OnDisableGUI()
	{
		this.canvas.enabled = false;
	}

	// Token: 0x0600123F RID: 4671 RVA: 0x000A921C File Offset: 0x000A761C
	private void OnPlayerJoined(PlayerId player)
	{
		if (player == PlayerId.PlayerTwo)
		{
			AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			this.mugman = UnityEngine.Object.Instantiate<LevelHUDPlayer>(this.levelHudTemplate);
			this.mugman.gameObject.SetActive(true);
			this.mugman.transform.SetParent(this.cuphead.transform.parent, false);
			this.mugman.Init(player2, !Level.IsTowerOfPowerMain);
		}
	}

	// Token: 0x06001240 RID: 4672 RVA: 0x000A928E File Offset: 0x000A768E
	private void OnPlayerLeave(PlayerId player)
	{
		if (player == PlayerId.PlayerTwo)
		{
			UnityEngine.Object.Destroy(this.mugman.gameObject);
		}
	}

	// Token: 0x04001BAD RID: 7085
	[SerializeField]
	private Canvas canvas;

	// Token: 0x04001BAE RID: 7086
	[Space(10f)]
	[SerializeField]
	private LevelHUDPlayer cuphead;

	// Token: 0x04001BAF RID: 7087
	private LevelHUDPlayer levelHudTemplate;

	// Token: 0x04001BB0 RID: 7088
	private LevelHUDPlayer mugman;
}
