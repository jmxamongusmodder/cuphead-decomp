using System;
using UnityEngine;

// Token: 0x020009A7 RID: 2471
public class LevelSelectScene : AbstractMonoBehaviour
{
	// Token: 0x060039F8 RID: 14840 RVA: 0x0020F3F5 File Offset: 0x0020D7F5
	protected override void Awake()
	{
		base.Awake();
		Cuphead.Init(false);
		CupheadEventSystem.Init();
		this.UpdatePlayers();
	}

	// Token: 0x060039F9 RID: 14841 RVA: 0x0020F40E File Offset: 0x0020D80E
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	// Token: 0x060039FA RID: 14842 RVA: 0x0020F421 File Offset: 0x0020D821
	public void OnOnePlayerButtonPressed()
	{
		PlayerManager.Multiplayer = false;
		this.UpdatePlayers();
	}

	// Token: 0x060039FB RID: 14843 RVA: 0x0020F42F File Offset: 0x0020D82F
	public void OnTwoPlayersButtonPressed()
	{
		PlayerManager.Multiplayer = true;
		this.UpdatePlayers();
	}

	// Token: 0x060039FC RID: 14844 RVA: 0x0020F440 File Offset: 0x0020D840
	private void UpdatePlayers()
	{
		float alpha = 0.3f;
		this.onePlayerButton.alpha = alpha;
		this.twoPlayersButton.alpha = alpha;
		if (PlayerManager.Multiplayer)
		{
			this.twoPlayersButton.alpha = 1f;
		}
		else
		{
			this.onePlayerButton.alpha = 1f;
		}
	}

	// Token: 0x040041DF RID: 16863
	[SerializeField]
	private CanvasGroup onePlayerButton;

	// Token: 0x040041E0 RID: 16864
	[SerializeField]
	private CanvasGroup twoPlayersButton;
}
