using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200042A RID: 1066
public class SpeechBubble : AbstractPausableComponent
{
	// Token: 0x17000269 RID: 617
	// (get) Token: 0x06000F77 RID: 3959 RVA: 0x0009A5F9 File Offset: 0x000989F9
	// (set) Token: 0x06000F78 RID: 3960 RVA: 0x0009A601 File Offset: 0x00098A01
	public SpeechBubble.Mode mode { get; private set; }

	// Token: 0x1700026A RID: 618
	// (get) Token: 0x06000F79 RID: 3961 RVA: 0x0009A60A File Offset: 0x00098A0A
	// (set) Token: 0x06000F7A RID: 3962 RVA: 0x0009A612 File Offset: 0x00098A12
	public SpeechBubble.DisplayState displayState { get; private set; }

	// Token: 0x06000F7B RID: 3963 RVA: 0x0009A61C File Offset: 0x00098A1C
	protected override void Awake()
	{
		if (SpeechBubble.Instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			SpeechBubble.Instance = this;
		}
		this.arrowAnchoredPosition = this.arrowBox.anchoredPosition;
		this.panPosition = base.transform.position;
		base.Awake();
		Dialoguer.Initialize();
	}

	// Token: 0x06000F7C RID: 3964 RVA: 0x0009A684 File Offset: 0x00098A84
	private void Start()
	{
		if (this.expandOnTheRight)
		{
			base.rectTransform.anchorMin = Vector2.zero;
			base.rectTransform.anchorMax = Vector2.zero;
			base.rectTransform.pivot = Vector2.zero;
		}
		else
		{
			base.rectTransform.anchorMin = new Vector2(1f, 0f);
			base.rectTransform.anchorMax = new Vector2(1f, 0f);
			base.rectTransform.pivot = Vector2.one;
		}
		this.canvasGroup.alpha = 0f;
		this.basePosition = base.rectTransform.position;
		this.input = new CupheadInput.AnyPlayerInput(false);
		this.AddDialoguerEvents();
	}

	// Token: 0x06000F7D RID: 3965 RVA: 0x0009A750 File Offset: 0x00098B50
	private int ProcessChoice(int playerSelection)
	{
		int num = 0;
		int i = 0;
		while (i <= playerSelection)
		{
			if (!this.OptionHidden(num))
			{
				i++;
			}
			num++;
		}
		return num - 1;
	}

	// Token: 0x06000F7E RID: 3966 RVA: 0x0009A784 File Offset: 0x00098B84
	private void Update()
	{
		if (MapEventNotification.Current == null || !MapEventNotification.Current.showing)
		{
			if (this.waiting)
			{
				return;
			}
			if (this.waitForFade)
			{
				return;
			}
			if (this.input.GetButtonUp(CupheadButton.Accept))
			{
				this.waitForRealease = false;
			}
			if (!this.waitForRealease && this.input.GetButtonDown(CupheadButton.Accept))
			{
				if (this.currentChoiceIndex >= 0)
				{
					if (this.displayState == SpeechBubble.DisplayState.WaitForSelection)
					{
						AudioManager.Play("level_menu_select");
					}
					Dialoguer.ContinueDialogue(this.ProcessChoice(this.currentChoiceIndex));
				}
				else
				{
					Dialoguer.ContinueDialogue();
				}
			}
			if (this.displayState == SpeechBubble.DisplayState.WaitForSelection && this.input.GetButtonDown(CupheadButton.Cancel))
			{
				Dialoguer.EndDialogue();
			}
		}
	}

	// Token: 0x06000F7F RID: 3967 RVA: 0x0009A85E File Offset: 0x00098C5E
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.RemoveDialoguerEvents();
		SpeechBubble.Instance = null;
	}

	// Token: 0x06000F80 RID: 3968 RVA: 0x0009A872 File Offset: 0x00098C72
	private void OnLanguageChanged()
	{
		this.delayedShow = true;
	}

	// Token: 0x06000F81 RID: 3969 RVA: 0x0009A87B File Offset: 0x00098C7B
	private void OnEnable()
	{
		if (this.delayedShow)
		{
			this.delayedShow = false;
			this.Show(this.data.text);
		}
	}

	// Token: 0x06000F82 RID: 3970 RVA: 0x0009A8A0 File Offset: 0x00098CA0
	private string ProcessTextPreShow(string text)
	{
		string normalizedText = this.GetNormalizedText(Localization.Translate(text).SanitizedText());
		TMP_FontAsset fontAsset = Localization.Instance.fonts[(int)Localization.language][27].fontAsset;
		TMP_FontAsset fontAsset2 = Localization.Instance.fonts[0][27].fontAsset;
		return (!(fontAsset == fontAsset2)) ? normalizedText : this.AdjustSpacingInFont(StringVariantGenerator.Instance.Generate(normalizedText));
	}

	// Token: 0x06000F83 RID: 3971 RVA: 0x0009A928 File Offset: 0x00098D28
	public void Show(string text)
	{
		if (this.showCoroutine != null)
		{
			base.StopCoroutine(this.showCoroutine);
		}
		string text2 = this.ProcessTextPreShow(text);
		int num = 8;
		if (Localization.language == Localization.Languages.Japanese || Localization.language == Localization.Languages.SimplifiedChinese)
		{
			num = 5;
		}
		else if (Localization.language == Localization.Languages.Korean)
		{
			num = 6;
		}
		this.showCoroutine = base.StartCoroutine(this.show_cr(SpeechBubble.Mode.Text, string.Concat(new object[]
		{
			text2,
			"<space=",
			num,
			"em> "
		}), null));
	}

	// Token: 0x06000F84 RID: 3972 RVA: 0x0009A9C0 File Offset: 0x00098DC0
	public void Show(string text, List<string> listItems)
	{
		if (this.showCoroutine != null)
		{
			base.StopCoroutine(this.showCoroutine);
		}
		string text2 = this.ProcessTextPreShow(text);
		this.showCoroutine = base.StartCoroutine(this.show_cr(SpeechBubble.Mode.ListChoice, text2, listItems));
	}

	// Token: 0x06000F85 RID: 3973 RVA: 0x0009AA01 File Offset: 0x00098E01
	public void Dismiss()
	{
		base.StartCoroutine(this.dismiss_cr(this.preventQuit));
	}

	// Token: 0x06000F86 RID: 3974 RVA: 0x0009AA18 File Offset: 0x00098E18
	protected virtual string GetNormalizedText(string text)
	{
		string text2 = this.mainText.text;
		TMP_FontAsset font = this.mainText.font;
		text = text.Replace("{DEATHS}", "<size=15> </size><font=\"CupheadVogue-BoldSDF\"><b><size=36>" + PlayerData.Data.DeathCount(PlayerId.Any).ToStringInvariant() + "</size></b></font><size=15> </size>");
		text = text.Replace("{BOSSREF}", this.setBossRefText);
		string text3 = text;
		if (Localization.language != Localization.Languages.Japanese)
		{
			text3 = string.Empty;
			text = text.Replace("\n", " ");
			text = text.Replace(" ", "<space=11.19853> ");
			text = text.Replace("{BR}", "\n");
			this.mainText.text = text;
			this.mainText.font = Localization.Instance.fonts[(int)Localization.language][27].fontAsset;
			this.mainText.CalculateLayoutInputHorizontal();
			string text4 = string.Empty;
			int num = 10000;
			int num2 = 0;
			while (this.mainText.text.Length > 0 && num > 0)
			{
				num--;
				while (this.mainText.text.Length > 0 && this.mainText.preferredWidth > this.maxWidth && num > 0)
				{
					num--;
					string text5 = this.mainText.text.Substring(this.mainText.text.Length - 1, 1);
					if (text5.Equals(" "))
					{
						int num3 = this.mainText.text.LastIndexOf("<");
						text4 = this.mainText.text.Substring(num3, this.mainText.text.Length - num3) + text4;
						this.mainText.text = this.mainText.text.Substring(0, num3);
					}
					else
					{
						text4 = text5 + text4;
						this.mainText.text = this.mainText.text.Substring(0, this.mainText.text.Length - 1);
					}
					this.mainText.CalculateLayoutInputHorizontal();
				}
				int num4 = this.mainText.text.LastIndexOf(" ");
				if (num4 == -1 || string.IsNullOrEmpty(text4))
				{
					if (!string.IsNullOrEmpty(text4) && text4.Substring(0, 1).Equals("<"))
					{
						text3 = text3 + this.mainText.text + "\n";
					}
					else
					{
						text3 += this.mainText.text;
					}
				}
				else
				{
					text4 = this.mainText.text.Substring(num4 + 1) + text4;
					text3 = text3 + this.mainText.text.Substring(0, num4) + "\n";
				}
				this.mainText.text = text4;
				this.mainText.CalculateLayoutInputHorizontal();
				text4 = string.Empty;
				num2++;
			}
			if (num == 0)
			{
				global::Debug.LogError("THE WHILES ARE DEAD, BAD CODE !!!", null);
			}
			if (this.maxLines != -1 && num2 > this.maxLines)
			{
				text3 = text3.Replace("\n", " ");
				this.mainText.enableAutoSizing = true;
				this.textLayoutElement.enabled = true;
				this.layout.padding.left = 20;
				this.layout.padding.right = 20;
				this.layout.padding.bottom = 20;
				this.layout.padding.top = 20;
			}
			else
			{
				this.mainText.enableAutoSizing = false;
				this.textLayoutElement.enabled = false;
			}
		}
		else
		{
			this.mainText.enableAutoSizing = false;
			this.textLayoutElement.enabled = false;
		}
		this.mainText.text = text2;
		this.mainText.font = font;
		return text3;
	}

	// Token: 0x06000F87 RID: 3975 RVA: 0x0009AE30 File Offset: 0x00099230
	private string AdjustSpacingInFont(string text)
	{
		string text2 = string.Empty;
		text2 = text.Replace("<space=11.19853>\n", "\n");
		text2 = text2.Replace("<space=11.19853>]", "<space=0.01244>]");
		return text2.Replace("<space=11.19853>}", "<space=-0.00622>}");
	}

	// Token: 0x06000F88 RID: 3976 RVA: 0x0009AE78 File Offset: 0x00099278
	private IEnumerator show_cr(SpeechBubble.Mode mode, string text, List<string> listItems)
	{
		this.waitForFade = true;
		if (this.displayState != SpeechBubble.DisplayState.Hidden)
		{
			yield return base.StartCoroutine(this.dismiss_cr(false));
		}
		if (this.expandOnTheRight)
		{
			this.box.GetComponent<RectTransform>().pivot = Vector2.zero;
		}
		else
		{
			this.box.GetComponent<RectTransform>().pivot = new Vector2(1f, 0f);
		}
		this.layout.padding.left = 30;
		this.layout.padding.right = 30;
		this.layout.padding.bottom = 30;
		this.layout.padding.top = 30;
		this.layout.spacing = 0f;
		this.mainText.text = text;
		this.mainText.font = Localization.Instance.fonts[(int)Localization.language][27].fontAsset;
		this.choiceText.font = this.mainText.font;
		foreach (RectTransform rectTransform in this.bullets)
		{
			rectTransform.gameObject.SetActive(false);
		}
		this.currentChoiceIndex = -1;
		if (mode == SpeechBubble.Mode.ListChoice)
		{
			string choiceColumn = string.Empty;
			for (int i = 0; i < listItems.Count; i++)
			{
				if (i < listItems.Count - 1)
				{
					choiceColumn = choiceColumn + Localization.Translate(listItems[i]).SanitizedText() + "\n";
				}
				else
				{
					choiceColumn += Localization.Translate(listItems[i]).SanitizedText();
				}
			}
			if (Localization.language != Localization.Languages.Korean)
			{
				this.choiceText.text = StringVariantGenerator.Instance.Generate(choiceColumn);
			}
			else
			{
				this.choiceText.text = choiceColumn;
			}
			this.currentChoiceIndex = 0;
			this.layout.spacing = 30f;
			yield return null;
		}
		else
		{
			this.choiceText.text = null;
		}
		if (this.tailOnTheLeft)
		{
			this.tail.rectTransform.anchorMin = Vector2.zero;
			this.tail.rectTransform.anchorMax = Vector2.zero;
			this.tail.rectTransform.anchoredPosition = new Vector2(73f, this.tail.rectTransform.anchoredPosition.y);
		}
		else
		{
			this.tail.rectTransform.anchorMin = new Vector2(1f, 0f);
			this.tail.rectTransform.anchorMax = new Vector2(1f, 0f);
			this.tail.rectTransform.anchoredPosition = new Vector2(-73f, this.tail.rectTransform.anchoredPosition.y);
		}
		this.arrow.color = new Color(1f, 1f, 1f, 0f);
		float maxOffset = 0.05f;
		if (CupheadLevelCamera.Current != null)
		{
			maxOffset *= 100f;
		}
		base.rectTransform.position = this.basePosition + new Vector2(UnityEngine.Random.Range(-maxOffset, maxOffset), UnityEngine.Random.Range(-maxOffset, maxOffset)) * base.rectTransform.localScale.x;
		this.tail.sprite = this.tailVariants.RandomChoice<Sprite>();
		this.tail.enabled = !this.hideTail;
		this.arrow.sprite = this.arrowVariants.RandomChoice<Sprite>();
		base.animator.Play("Idle", 0, UnityEngine.Random.Range(0f, 1f));
		base.animator.Play("Idle", 1, UnityEngine.Random.Range(0f, 1f));
		this.displayState = SpeechBubble.DisplayState.FadeIn;
		yield return base.StartCoroutine(this.fade_cr(this.canvasGroup.alpha, 1f));
		yield return CupheadTime.WaitForSeconds(this, 0.125f);
		this.displayState = SpeechBubble.DisplayState.Showing;
		this.showCoroutine = null;
		Color colorHidden = new Color(1f, 1f, 1f, 0f);
		Color colorShown = new Color(1f, 1f, 1f, 1f);
		if (this.expandOnTheRight)
		{
			this.arrowBox.anchoredPosition = new Vector2(this.arrowAnchoredPosition.x + this.box.sizeDelta.x, this.arrowBox.anchoredPosition.y);
		}
		if (mode == SpeechBubble.Mode.Text)
		{
			this.arrow.color = ((!this.waiting) ? colorShown : colorHidden);
			this.cursor.color = colorHidden;
		}
		else
		{
			this.cursor.color = ((!this.waiting) ? colorShown : colorHidden);
			this.displayState = SpeechBubble.DisplayState.WaitForSelection;
			this.waitForFade = false;
			while (this.displayState == SpeechBubble.DisplayState.WaitForSelection)
			{
				if (this.waiting)
				{
					yield return null;
				}
				else
				{
					if (PauseManager.state != PauseManager.State.Paused)
					{
						if (this.input.GetButtonDown(CupheadButton.MenuDown) && this.currentChoiceIndex < listItems.Count - 1)
						{
							this.currentChoiceIndex++;
							base.animator.SetTrigger("MoveDown");
							AudioManager.Play("level_menu_move");
						}
						if (this.input.GetButtonDown(CupheadButton.MenuUp) && this.currentChoiceIndex > 0)
						{
							this.currentChoiceIndex--;
							base.animator.SetTrigger("MoveUp");
							AudioManager.Play("level_menu_move");
						}
					}
					this.cursorRoot.anchoredPosition = this.getCursorPos(this.currentChoiceIndex, listItems.Count);
					this.cursor.color = colorShown;
					yield return null;
				}
			}
		}
		this.waitForFade = false;
		this.cursor.color = colorHidden;
		yield break;
	}

	// Token: 0x06000F89 RID: 3977 RVA: 0x0009AEA8 File Offset: 0x000992A8
	private IEnumerator dismiss_cr(bool watchPreventQuit)
	{
		if (this.displayState == SpeechBubble.DisplayState.Hidden)
		{
			yield break;
		}
		while (this.displayState == SpeechBubble.DisplayState.FadeIn)
		{
			yield return null;
		}
		if (watchPreventQuit)
		{
			while (this.preventQuit)
			{
				yield return null;
			}
		}
		this.displayState = SpeechBubble.DisplayState.FadeOut;
		yield return base.StartCoroutine(this.fade_cr(this.canvasGroup.alpha, 0f));
		this.displayState = SpeechBubble.DisplayState.Hidden;
		yield break;
	}

	// Token: 0x06000F8A RID: 3978 RVA: 0x0009AECC File Offset: 0x000992CC
	private IEnumerator fade_cr(float startOpacity, float endOpacity)
	{
		if (endOpacity == 0f)
		{
			this.canvasGroup.alpha = endOpacity;
			yield break;
		}
		yield return null;
		float t = 0f;
		while (t < 0.07f)
		{
			yield return null;
			t += CupheadTime.Delta;
			this.canvasGroup.alpha = Mathf.Lerp(startOpacity, endOpacity, t / 0.07f);
		}
		this.canvasGroup.alpha = endOpacity;
		yield break;
	}

	// Token: 0x06000F8B RID: 3979 RVA: 0x0009AEF8 File Offset: 0x000992F8
	private Vector2 getCursorPos(int choiceIndex, int choiceCount)
	{
		float num = this.choiceText.bounds.extents.y / (float)choiceCount * 2f;
		return new Vector2(this.choiceText.margin.x - 10f, 0f) + Vector2.up * (((float)choiceCount - 1f) / 2f) * num + Vector2.down * ((float)choiceIndex * num);
	}

	// Token: 0x06000F8C RID: 3980 RVA: 0x0009AF83 File Offset: 0x00099383
	private void setOpacity(float opacity)
	{
	}

	// Token: 0x06000F8D RID: 3981 RVA: 0x0009AF88 File Offset: 0x00099388
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onStarted += this.OnDialogueStartedHandler;
		Dialoguer.events.onEnded += this.OnDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded += this.OnDialogueInstantlyEndedHandler;
		Dialoguer.events.onTextPhase += this.OnDialogueTextPhaseHandler;
		Dialoguer.events.onWindowClose += this.OnDialogueWindowCloseHandler;
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
	}

	// Token: 0x06000F8E RID: 3982 RVA: 0x0009B01C File Offset: 0x0009941C
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onStarted -= this.OnDialogueStartedHandler;
		Dialoguer.events.onEnded -= this.OnDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded -= this.OnDialogueInstantlyEndedHandler;
		Dialoguer.events.onTextPhase -= this.OnDialogueTextPhaseHandler;
		Dialoguer.events.onWindowClose -= this.OnDialogueWindowCloseHandler;
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x06000F8F RID: 3983 RVA: 0x0009B0B0 File Offset: 0x000994B0
	private void OnDialogueStartedHandler()
	{
		Localization.OnLanguageChangedEvent += this.OnLanguageChanged;
		if (Map.Current != null)
		{
			Map.Current.CurrentState = Map.State.Event;
		}
		if (CupheadMapCamera.Current != null)
		{
			CupheadMapCamera.Current.MoveToPosition(this.panPosition, 0.75f, 1f);
		}
		if (MapUIVignetteDialogue.Current != null)
		{
			MapUIVignetteDialogue.Current.FadeIn();
		}
	}

	// Token: 0x06000F90 RID: 3984 RVA: 0x0009B12E File Offset: 0x0009952E
	private void OnDialogueEndedHandler()
	{
		Localization.OnLanguageChangedEvent -= this.OnLanguageChanged;
		this.Dismiss();
	}

	// Token: 0x06000F91 RID: 3985 RVA: 0x0009B147 File Offset: 0x00099547
	private void OnDialogueInstantlyEndedHandler()
	{
		Localization.OnLanguageChangedEvent -= this.OnLanguageChanged;
		this.Dismiss();
	}

	// Token: 0x06000F92 RID: 3986 RVA: 0x0009B160 File Offset: 0x00099560
	private void OnDialogueTextPhaseHandler(DialoguerTextData data)
	{
		this.data = data;
		if (data.choices == null)
		{
			this.Show(data.text);
		}
		else if (data.choices.Length > 0)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < data.choices.Length; i++)
			{
				if (!this.OptionHidden(i))
				{
					list.Add(data.choices[i]);
				}
			}
			this.Show(data.text, list);
		}
	}

	// Token: 0x06000F93 RID: 3987 RVA: 0x0009B1EA File Offset: 0x000995EA
	public void ClearHideOptionBitmask()
	{
		this.hideOptionBitmask = 0;
	}

	// Token: 0x06000F94 RID: 3988 RVA: 0x0009B1F3 File Offset: 0x000995F3
	public void HideOptionByIndex(int i)
	{
		this.hideOptionBitmask |= 1 << i;
	}

	// Token: 0x06000F95 RID: 3989 RVA: 0x0009B208 File Offset: 0x00099608
	private bool OptionHidden(int i)
	{
		return (this.hideOptionBitmask & 1 << i) != 0;
	}

	// Token: 0x06000F96 RID: 3990 RVA: 0x0009B220 File Offset: 0x00099620
	private void OnDialogueWindowCloseHandler()
	{
		this.Dismiss();
		this.ClearHideOptionBitmask();
		if (MapUIVignetteDialogue.Current != null)
		{
			MapUIVignetteDialogue.Current.FadeOut();
		}
		if (Map.Current != null && Map.Current.CurrentState != Map.State.Graveyard)
		{
			Map.Current.CurrentState = Map.State.Ready;
		}
	}

	// Token: 0x06000F97 RID: 3991 RVA: 0x0009B27E File Offset: 0x0009967E
	private void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (message == "Wait")
		{
			base.StartCoroutine(this.wait_cr(Parser.FloatParse(metadata)));
		}
	}

	// Token: 0x06000F98 RID: 3992 RVA: 0x0009B2A4 File Offset: 0x000996A4
	private IEnumerator wait_cr(float waitDuration)
	{
		this.waiting = true;
		this.arrow.color = new Color(1f, 1f, 1f, 0f);
		while (waitDuration > 0f)
		{
			yield return null;
			waitDuration -= CupheadTime.Delta;
		}
		this.waiting = false;
		this.arrow.color = new Color(1f, 1f, 1f, 1f);
		yield break;
	}

	// Token: 0x04001892 RID: 6290
	private const int REGULAR_ARROW_PADDING = 8;

	// Token: 0x04001893 RID: 6291
	private const int KOREAN_ARROW_PADDING = 6;

	// Token: 0x04001894 RID: 6292
	private const int JAP_CHI_ARROW_PADDING = 5;

	// Token: 0x04001895 RID: 6293
	private const float DEFAULT_TIME = 2f;

	// Token: 0x04001896 RID: 6294
	private const float FADE_TIME = 0.07f;

	// Token: 0x04001897 RID: 6295
	private const float END_TIME = 0.25f;

	// Token: 0x04001898 RID: 6296
	private const float ARROW_WAIT_TIME = 0.125f;

	// Token: 0x04001899 RID: 6297
	private const float MAX_RANDOM_OFFSET = 0.05f;

	// Token: 0x0400189A RID: 6298
	private const int MAX_CHOICES_PER_COLUMN = 4;

	// Token: 0x0400189B RID: 6299
	private const int COLUMN_PADDING = 55;

	// Token: 0x0400189C RID: 6300
	private const int COLUMN_SPACING = 45;

	// Token: 0x0400189D RID: 6301
	private const int DEFAULT_PADDING = 30;

	// Token: 0x0400189E RID: 6302
	private const int SMALL_PADDING = 20;

	// Token: 0x0400189F RID: 6303
	private const float TAIL_POSITION_X = -73f;

	// Token: 0x040018A0 RID: 6304
	private const float CURSOR_OFFSET_H = 10f;

	// Token: 0x040018A3 RID: 6307
	public static SpeechBubble Instance;

	// Token: 0x040018A4 RID: 6308
	[SerializeField]
	private TextMeshProUGUI mainText;

	// Token: 0x040018A5 RID: 6309
	[SerializeField]
	private TextMeshProUGUI choiceText;

	// Token: 0x040018A6 RID: 6310
	[SerializeField]
	private VerticalLayoutGroup layout;

	// Token: 0x040018A7 RID: 6311
	[SerializeField]
	private Image tail;

	// Token: 0x040018A8 RID: 6312
	[SerializeField]
	private List<Sprite> tailVariants;

	// Token: 0x040018A9 RID: 6313
	[SerializeField]
	private RectTransform arrowBox;

	// Token: 0x040018AA RID: 6314
	[SerializeField]
	private Image arrow;

	// Token: 0x040018AB RID: 6315
	[SerializeField]
	private Image cursor;

	// Token: 0x040018AC RID: 6316
	[SerializeField]
	private RectTransform cursorRoot;

	// Token: 0x040018AD RID: 6317
	[SerializeField]
	private RectTransform box;

	// Token: 0x040018AE RID: 6318
	[SerializeField]
	private List<Sprite> arrowVariants;

	// Token: 0x040018AF RID: 6319
	[SerializeField]
	private CanvasGroup canvasGroup;

	// Token: 0x040018B0 RID: 6320
	[SerializeField]
	private List<RectTransform> bullets;

	// Token: 0x040018B1 RID: 6321
	private float maxWidth = 558f;

	// Token: 0x040018B2 RID: 6322
	private Vector2 arrowAnchoredPosition;

	// Token: 0x040018B3 RID: 6323
	public Vector2 basePosition;

	// Token: 0x040018B4 RID: 6324
	public Vector2 panPosition;

	// Token: 0x040018B5 RID: 6325
	private int currentChoiceIndex;

	// Token: 0x040018B6 RID: 6326
	private int hideOptionBitmask;

	// Token: 0x040018B7 RID: 6327
	public string setBossRefText = string.Empty;

	// Token: 0x040018B8 RID: 6328
	public int maxLines = -1;

	// Token: 0x040018B9 RID: 6329
	public bool tailOnTheLeft;

	// Token: 0x040018BA RID: 6330
	public bool expandOnTheRight;

	// Token: 0x040018BB RID: 6331
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x040018BC RID: 6332
	public bool waitForRealease;

	// Token: 0x040018BD RID: 6333
	public bool waitForFade;

	// Token: 0x040018BE RID: 6334
	public bool hideTail;

	// Token: 0x040018BF RID: 6335
	private Coroutine showCoroutine;

	// Token: 0x040018C0 RID: 6336
	[SerializeField]
	private LayoutElement textLayoutElement;

	// Token: 0x040018C1 RID: 6337
	private bool waiting;

	// Token: 0x040018C2 RID: 6338
	private bool delayedShow;

	// Token: 0x040018C3 RID: 6339
	private DialoguerTextData data;

	// Token: 0x040018C4 RID: 6340
	[HideInInspector]
	public bool preventQuit;

	// Token: 0x0200042B RID: 1067
	public enum Mode
	{
		// Token: 0x040018C6 RID: 6342
		Text,
		// Token: 0x040018C7 RID: 6343
		ListChoice
	}

	// Token: 0x0200042C RID: 1068
	public enum DisplayState
	{
		// Token: 0x040018C9 RID: 6345
		Hidden,
		// Token: 0x040018CA RID: 6346
		FadeIn,
		// Token: 0x040018CB RID: 6347
		Showing,
		// Token: 0x040018CC RID: 6348
		WaitForSelection,
		// Token: 0x040018CD RID: 6349
		FadeOut
	}
}
