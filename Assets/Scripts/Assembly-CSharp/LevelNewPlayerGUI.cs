using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000482 RID: 1154
public class LevelNewPlayerGUI : AbstractMonoBehaviour
{
	// Token: 0x170002C8 RID: 712
	// (get) Token: 0x060011E0 RID: 4576 RVA: 0x000A7347 File Offset: 0x000A5747
	// (set) Token: 0x060011E1 RID: 4577 RVA: 0x000A734E File Offset: 0x000A574E
	public static LevelNewPlayerGUI Current { get; private set; }

	// Token: 0x060011E2 RID: 4578 RVA: 0x000A7358 File Offset: 0x000A5758
	protected override void Awake()
	{
		base.Awake();
		if (PlayerManager.player1IsMugman)
		{
			this.card.sprite = this.cupheadCard;
		}
		LevelNewPlayerGUI.Current = this;
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		this.canvasGroup.alpha = 0f;
		base.gameObject.SetActive(false);
	}

	// Token: 0x060011E3 RID: 4579 RVA: 0x000A73B4 File Offset: 0x000A57B4
	private void OnDestroy()
	{
		if (LevelNewPlayerGUI.Current == this)
		{
			LevelNewPlayerGUI.Current = null;
		}
	}

	// Token: 0x060011E4 RID: 4580 RVA: 0x000A73CC File Offset: 0x000A57CC
	public void Init()
	{
		base.gameObject.SetActive(true);
		if (OnlineManager.Instance.Interface.SupportsMultipleUsers && OnlineManager.Instance.Interface.GetUser(PlayerId.PlayerTwo) != null)
		{
			this.localizationHelper.ApplyTranslation(Localization.Find("PlayerTwoJoinedWithUser"), new LocalizationHelper.LocalizationSubtext[]
			{
				new LocalizationHelper.LocalizationSubtext("USERNAME", OnlineManager.Instance.Interface.GetUser(PlayerId.PlayerTwo).Name, true)
			});
		}
		base.StartCoroutine(this.tweenIn_cr());
		base.StartCoroutine(this.text_cr());
	}

	// Token: 0x060011E5 RID: 4581 RVA: 0x000A7470 File Offset: 0x000A5870
	protected IEnumerator tweenIn_cr()
	{
		base.animator.Play("In");
		float t = 0f;
		AudioManager.Play("player_joined");
		PauseManager.Pause();
		while (t < 0.2f)
		{
			float val = t / 0.2f;
			this.canvasGroup.alpha = Mathf.Lerp(0f, 1f, val);
			t += Time.deltaTime;
			yield return null;
		}
		this.canvasGroup.alpha = 1f;
		yield return new WaitForSeconds(2f);
		base.animator.Play("Out");
		base.StartCoroutine(this.tweenOut_cr());
		yield break;
	}

	// Token: 0x060011E6 RID: 4582 RVA: 0x000A748C File Offset: 0x000A588C
	protected IEnumerator tweenOut_cr()
	{
		float t = 0f;
		while (t < 0.2f)
		{
			float val = t / 0.2f;
			this.canvasGroup.alpha = Mathf.Lerp(1f, 0f, val);
			t += Time.deltaTime;
			yield return null;
		}
		this.canvasGroup.alpha = 0f;
		while (InterruptingPrompt.IsInterrupting())
		{
			yield return null;
		}
		PauseManager.Unpause();
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x060011E7 RID: 4583 RVA: 0x000A74A8 File Offset: 0x000A58A8
	private IEnumerator text_cr()
	{
		for (;;)
		{
			this.text.color = Color.white;
			yield return new WaitForSeconds(0.041666668f);
			this.text.color = ((!PlayerManager.player1IsMugman) ? this.mugmanColor : this.cupheadColor);
			yield return new WaitForSeconds(0.041666668f);
		}
		yield break;
	}

	// Token: 0x04001B67 RID: 7015
	[SerializeField]
	private Image background;

	// Token: 0x04001B68 RID: 7016
	[SerializeField]
	private Image card;

	// Token: 0x04001B69 RID: 7017
	[SerializeField]
	private Sprite cupheadCard;

	// Token: 0x04001B6A RID: 7018
	[SerializeField]
	private TextMeshProUGUI text;

	// Token: 0x04001B6B RID: 7019
	[SerializeField]
	private LocalizationHelper localizationHelper;

	// Token: 0x04001B6C RID: 7020
	[SerializeField]
	private Color cupheadColor;

	// Token: 0x04001B6D RID: 7021
	[SerializeField]
	private Color mugmanColor;

	// Token: 0x04001B6E RID: 7022
	private CanvasGroup canvasGroup;
}
