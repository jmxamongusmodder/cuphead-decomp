using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000485 RID: 1157
[RequireComponent(typeof(CanvasGroup))]
public class LevelReviveCardGUI : AbstractMonoBehaviour
{
	// Token: 0x06001214 RID: 4628 RVA: 0x000A85F8 File Offset: 0x000A69F8
	protected override void Awake()
	{
		base.Awake();
		LevelReviveCardGUI.Current = this;
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		base.gameObject.SetActive(false);
		this.input = new CupheadInput.AnyPlayerInput(false);
		this.helpCanvasGroup.alpha = 0f;
		this.ignoreGlobalTime = true;
		this.timeLayer = CupheadTime.Layer.UI;
		this.state = LevelReviveCardGUI.State.Init;
	}

	// Token: 0x06001215 RID: 4629 RVA: 0x000A865A File Offset: 0x000A6A5A
	private void OnDestroy()
	{
		LevelReviveCardGUI.Current = null;
	}

	// Token: 0x06001216 RID: 4630 RVA: 0x000A8662 File Offset: 0x000A6A62
	private void Update()
	{
		if (this.state != LevelReviveCardGUI.State.Ready)
		{
			return;
		}
		if (PlayerManager.GetPlayerInput(this.deadPlayer).GetButtonDown(13))
		{
			this.RevivePlayer();
			this.state = LevelReviveCardGUI.State.Exiting;
		}
	}

	// Token: 0x06001217 RID: 4631 RVA: 0x000A8698 File Offset: 0x000A6A98
	private void RevivePlayer()
	{
		TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)this.deadPlayer].HP = 3;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)this.deadPlayer].BonusHP = 0;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)this.deadPlayer].SuperCharge = 0f;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)this.deadPlayer].tokenCount--;
		this.playerOneCard.SetTokenCount();
		this.playerTwoCard.SetTokenCount();
		SceneLoader.ContinueTowerOfPower();
	}

	// Token: 0x06001218 RID: 4632 RVA: 0x000A8714 File Offset: 0x000A6B14
	public void In()
	{
		base.gameObject.SetActive(true);
		this.playerOneCard.Init(PlayerId.PlayerOne);
		this.playerTwoCard.Init(PlayerId.PlayerTwo);
		if (TowerOfPowerLevelGameInfo.PLAYER_STATS[0].HP == 0)
		{
			this.deadPlayer = PlayerId.PlayerOne;
		}
		else
		{
			this.deadPlayer = PlayerId.PlayerTwo;
		}
		base.StartCoroutine(this.in_cr());
	}

	// Token: 0x06001219 RID: 4633 RVA: 0x000A8778 File Offset: 0x000A6B78
	private IEnumerator in_cr()
	{
		AudioManager.Play("level_menu_card_up");
		yield return base.TweenValue(0f, 1f, 0.05f, EaseUtils.EaseType.linear, new AbstractMonoBehaviour.TweenUpdateHandler(this.SetAlpha));
		yield return new WaitForSeconds(1f);
		AudioManager.Play("player_die_vinylscratch");
		AudioManager.HandleSnapshot(AudioManager.Snapshots.Death.ToString(), 4f);
		if (!Level.IsChessBoss)
		{
			AudioManager.ChangeBGMPitch(0.7f, 6f);
		}
		base.TweenValue(0f, 1f, 0.3f, EaseUtils.EaseType.easeOutCubic, new AbstractMonoBehaviour.TweenUpdateHandler(this.SetCardValue));
		yield return null;
		this.state = LevelReviveCardGUI.State.Ready;
		yield break;
	}

	// Token: 0x0600121A RID: 4634 RVA: 0x000A8793 File Offset: 0x000A6B93
	private void SetAlpha(float value)
	{
		this.canvasGroup.alpha = value;
	}

	// Token: 0x0600121B RID: 4635 RVA: 0x000A87A4 File Offset: 0x000A6BA4
	private void SetCardValue(float value)
	{
		this.playerOneCard.SetAlpha(value);
		this.playerTwoCard.SetAlpha(value);
		this.helpCanvasGroup.alpha = value;
		this.playerOneCard.transform.SetLocalEulerAngles(null, null, new float?(Mathf.Lerp(15f, 4f, value)));
		this.playerTwoCard.transform.SetLocalEulerAngles(null, null, new float?(Mathf.Lerp(-15f, -4f, value)));
	}

	// Token: 0x04001B85 RID: 7045
	public static LevelReviveCardGUI Current;

	// Token: 0x04001B86 RID: 7046
	[Space(10f)]
	[SerializeField]
	private TowerOfPowerContinueCardGUI playerOneCard;

	// Token: 0x04001B87 RID: 7047
	[Space(10f)]
	[SerializeField]
	private TowerOfPowerContinueCardGUI playerTwoCard;

	// Token: 0x04001B88 RID: 7048
	[Space(10f)]
	[SerializeField]
	private CanvasGroup helpCanvasGroup;

	// Token: 0x04001B89 RID: 7049
	private LevelReviveCardGUI.State state;

	// Token: 0x04001B8A RID: 7050
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x04001B8B RID: 7051
	private CanvasGroup canvasGroup;

	// Token: 0x04001B8C RID: 7052
	private PlayerId deadPlayer;

	// Token: 0x02000486 RID: 1158
	private enum State
	{
		// Token: 0x04001B8E RID: 7054
		Init,
		// Token: 0x04001B8F RID: 7055
		Ready,
		// Token: 0x04001B90 RID: 7056
		Exiting
	}
}
