using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000489 RID: 1161
[RequireComponent(typeof(CanvasGroup))]
public class TowerOfPowerContinueCardGUI : AbstractMonoBehaviour
{
	// Token: 0x06001225 RID: 4645 RVA: 0x000A8C70 File Offset: 0x000A7070
	protected override void Awake()
	{
		base.Awake();
		foreach (UIImageAnimationLoop uiimageAnimationLoop in this.CardMugmanAnimation)
		{
			uiimageAnimationLoop.gameObject.SetActive(false);
		}
		foreach (UIImageAnimationLoop uiimageAnimationLoop2 in this.CardCupheadAnimation)
		{
			uiimageAnimationLoop2.gameObject.SetActive(false);
		}
	}

	// Token: 0x06001226 RID: 4646 RVA: 0x000A8D28 File Offset: 0x000A7128
	public void Init(PlayerId playerId)
	{
		this.canvas.alpha = 0f;
		this.SetPlayer(playerId);
		this.player = TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId];
		this.SetTitlePlayersName();
		this.SetTokenCount();
		this.Continue.gameObject.SetActive(this.player.HP == 0);
		this.CountDown_text.gameObject.SetActive(this.player.HP == 0);
		this.SetAnimation();
		if (this.player.HP == 0)
		{
			base.StartCoroutine(this.update_countdown_cr());
		}
	}

	// Token: 0x06001227 RID: 4647 RVA: 0x000A8DC4 File Offset: 0x000A71C4
	private void SetAnimation()
	{
		if (this.yourPlayerIsMugman)
		{
			this.CardMugmanAnimation[(this.player.HP != 0) ? 0 : 1].gameObject.SetActive(true);
		}
		else
		{
			this.CardCupheadAnimation[(this.player.HP != 0) ? 0 : 1].gameObject.SetActive(true);
		}
	}

	// Token: 0x06001228 RID: 4648 RVA: 0x000A8E3B File Offset: 0x000A723B
	private void SetPlayer(PlayerId playerId)
	{
		if (playerId == PlayerId.PlayerOne)
		{
			this.yourPlayerIsMugman = PlayerManager.player1IsMugman;
		}
		else
		{
			this.yourPlayerIsMugman = !PlayerManager.player1IsMugman;
		}
	}

	// Token: 0x06001229 RID: 4649 RVA: 0x000A8E64 File Offset: 0x000A7264
	private void SetTitlePlayersName()
	{
		if (this.yourPlayerIsMugman)
		{
		}
	}

	// Token: 0x0600122A RID: 4650 RVA: 0x000A8E90 File Offset: 0x000A7290
	public void SetTokenCount()
	{
		int tokenCount = this.player.tokenCount;
		this.TokenLeft_text.text = "Token: " + tokenCount;
	}

	// Token: 0x0600122B RID: 4651 RVA: 0x000A8EC4 File Offset: 0x000A72C4
	public IEnumerator update_countdown_cr()
	{
		while (this.countDown > 0)
		{
			yield return CupheadTime.WaitForSeconds(this, 1f);
			this.countDown--;
			this.UpdateCountDownText();
		}
		yield return null;
		SceneLoader.ContinueTowerOfPower();
		yield break;
	}

	// Token: 0x0600122C RID: 4652 RVA: 0x000A8EDF File Offset: 0x000A72DF
	private void UpdateCountDownText()
	{
		this.CountDown_text.text = this.countDown.ToString();
	}

	// Token: 0x0600122D RID: 4653 RVA: 0x000A8EFD File Offset: 0x000A72FD
	private void OnDestroy()
	{
		this.Continue = null;
		this.CountDown_text = null;
		this.CardCupheadAnimation.Clear();
		this.CardMugmanAnimation.Clear();
	}

	// Token: 0x0600122E RID: 4654 RVA: 0x000A8F23 File Offset: 0x000A7323
	public void SetAlpha(float value)
	{
		this.canvas.alpha = value;
	}

	// Token: 0x04001B9E RID: 7070
	[SerializeField]
	private SpriteRenderer PlayerName;

	// Token: 0x04001B9F RID: 7071
	[SerializeField]
	private Sprite CupheadNameData;

	// Token: 0x04001BA0 RID: 7072
	[SerializeField]
	private Sprite CupmanNameData;

	// Token: 0x04001BA1 RID: 7073
	[Space(10f)]
	[SerializeField]
	private Text TokenLeft_text;

	// Token: 0x04001BA2 RID: 7074
	[SerializeField]
	private Text Continue;

	// Token: 0x04001BA3 RID: 7075
	[SerializeField]
	private Text CountDown_text;

	// Token: 0x04001BA4 RID: 7076
	[SerializeField]
	private List<UIImageAnimationLoop> CardCupheadAnimation = new List<UIImageAnimationLoop>();

	// Token: 0x04001BA5 RID: 7077
	[SerializeField]
	private List<UIImageAnimationLoop> CardMugmanAnimation = new List<UIImageAnimationLoop>();

	// Token: 0x04001BA6 RID: 7078
	[SerializeField]
	private CanvasGroup canvas;

	// Token: 0x04001BA7 RID: 7079
	private bool yourPlayerIsMugman;

	// Token: 0x04001BA8 RID: 7080
	private int countDown = 10;

	// Token: 0x04001BA9 RID: 7081
	private PlayersStatsBossesHub player;
}
