using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000A0B RID: 2571
public class PlayerJoinEffect : AbstractMonoBehaviour
{
	// Token: 0x06003CC4 RID: 15556 RVA: 0x0021A108 File Offset: 0x00218508
	public static PlayerJoinEffect Create(PlayerId playerId, Vector2 pos, PlayerMode mode, bool isChalice)
	{
		PlayerJoinEffect playerJoinEffect = UnityEngine.Object.Instantiate<PlayerJoinEffect>(Level.Current.LevelResources.joinEffect);
		playerJoinEffect.name = playerJoinEffect.name.Replace("(Clone)", string.Empty);
		playerJoinEffect.Init(playerId, pos, mode, isChalice);
		return playerJoinEffect;
	}

	// Token: 0x14000089 RID: 137
	// (add) Token: 0x06003CC5 RID: 15557 RVA: 0x0021A150 File Offset: 0x00218550
	// (remove) Token: 0x06003CC6 RID: 15558 RVA: 0x0021A188 File Offset: 0x00218588
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event AbstractPlayerController.OnReviveHandler OnPreReviveEvent;

	// Token: 0x1400008A RID: 138
	// (add) Token: 0x06003CC7 RID: 15559 RVA: 0x0021A1C0 File Offset: 0x002185C0
	// (remove) Token: 0x06003CC8 RID: 15560 RVA: 0x0021A1F8 File Offset: 0x002185F8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event AbstractPlayerController.OnReviveHandler OnReviveEvent;

	// Token: 0x06003CC9 RID: 15561 RVA: 0x0021A22E File Offset: 0x0021862E
	protected override void Awake()
	{
		base.Awake();
		PlayerManager.OnPlayerLeaveEvent += this.OnPlayerLeave;
	}

	// Token: 0x06003CCA RID: 15562 RVA: 0x0021A248 File Offset: 0x00218648
	private void Init(PlayerId playerId, Vector2 pos, PlayerMode mode, bool isChalice)
	{
		this.playerId = playerId;
		this.playerMode = mode;
		base.animator.SetInteger("Mode", (int)this.playerMode);
		if (playerId == PlayerId.PlayerOne || playerId != PlayerId.PlayerTwo)
		{
			this.spriteRenderer = this.cuphead;
		}
		else
		{
			this.spriteRenderer = this.mugman;
		}
		if (isChalice)
		{
			this.spriteRenderer = this.chalice;
		}
		this.cuphead.gameObject.SetActive(false);
		this.mugman.gameObject.SetActive(false);
		this.chalice.gameObject.SetActive(false);
		this.spriteRenderer.gameObject.SetActive(true);
		base.transform.position = pos;
	}

	// Token: 0x06003CCB RID: 15563 RVA: 0x0021A318 File Offset: 0x00218718
	public void GameOverUnpause()
	{
		base.animator.enabled = true;
		AnimationHelper component = base.GetComponent<AnimationHelper>();
		component.IgnoreGlobal = true;
		this.ignoreGlobalTime = true;
	}

	// Token: 0x06003CCC RID: 15564 RVA: 0x0021A346 File Offset: 0x00218746
	private void OnReviveStealAnimComplete()
	{
		if (this.OnReviveEvent != null)
		{
			this.OnReviveEvent(base.transform.position);
		}
		this.OnReviveEvent = null;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003CCD RID: 15565 RVA: 0x0021A37B File Offset: 0x0021877B
	private void OnPlayerLeave(PlayerId id)
	{
		if (this.playerId == id)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06003CCE RID: 15566 RVA: 0x0021A394 File Offset: 0x00218794
	private void OnDestroy()
	{
		PlayerManager.OnPlayerLeaveEvent -= this.OnPlayerLeave;
	}

	// Token: 0x0400440E RID: 17422
	public const string NAME = "Player_Join";

	// Token: 0x0400440F RID: 17423
	[SerializeField]
	private SpriteRenderer cuphead;

	// Token: 0x04004410 RID: 17424
	[SerializeField]
	private SpriteRenderer mugman;

	// Token: 0x04004411 RID: 17425
	[SerializeField]
	private SpriteRenderer chalice;

	// Token: 0x04004412 RID: 17426
	private PlayerId playerId;

	// Token: 0x04004413 RID: 17427
	private SpriteRenderer spriteRenderer;

	// Token: 0x04004414 RID: 17428
	private PlayerMode playerMode;
}
