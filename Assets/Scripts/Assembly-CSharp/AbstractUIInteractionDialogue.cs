using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200044E RID: 1102
public abstract class AbstractUIInteractionDialogue : AbstractMonoBehaviour
{
	// Token: 0x17000299 RID: 665
	// (get) Token: 0x0600108E RID: 4238 RVA: 0x0009F5E4 File Offset: 0x0009D9E4
	protected virtual float OpenTime
	{
		get
		{
			return 0.3f;
		}
	}

	// Token: 0x1700029A RID: 666
	// (get) Token: 0x0600108F RID: 4239 RVA: 0x0009F5EB File Offset: 0x0009D9EB
	protected virtual float CloseTime
	{
		get
		{
			return 0.3f;
		}
	}

	// Token: 0x1700029B RID: 667
	// (get) Token: 0x06001090 RID: 4240 RVA: 0x0009F5F2 File Offset: 0x0009D9F2
	protected virtual float OpenScale
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x1700029C RID: 668
	// (get) Token: 0x06001091 RID: 4241 RVA: 0x0009F5F9 File Offset: 0x0009D9F9
	// (set) Token: 0x06001092 RID: 4242 RVA: 0x0009F606 File Offset: 0x0009DA06
	protected string Text
	{
		get
		{
			return this.tmpText.text;
		}
		set
		{
			this.tmpText.text = value;
		}
	}

	// Token: 0x1700029D RID: 669
	// (get) Token: 0x06001093 RID: 4243 RVA: 0x0009F614 File Offset: 0x0009DA14
	protected virtual float PreferredWidth
	{
		get
		{
			return this.tmpText.preferredWidth + this.glyph.preferredWidth;
		}
	}

	// Token: 0x06001094 RID: 4244 RVA: 0x0009F62D File Offset: 0x0009DA2D
	private void Start()
	{
		base.transform.localScale = Vector3.zero;
	}

	// Token: 0x06001095 RID: 4245 RVA: 0x0009F640 File Offset: 0x0009DA40
	protected virtual void Init(AbstractUIInteractionDialogue.Properties properties, PlayerInput player, Vector2 offset)
	{
		float num = 40f;
		this.target = player.transform;
		this.dialogueOffset = offset;
		int id;
		if (Parser.IntTryParse(properties.text, out id))
		{
			this.Text = Localization.Translate(id).text.ToUpper();
			this.tmpText.font = Localization.Instance.fonts[(int)Localization.language][28].fontAsset;
		}
		else
		{
			this.Text = properties.text.ToUpper();
		}
		this.glyph.rewiredPlayerId = (int)player.playerId;
		this.glyph.button = properties.button;
		this.glyph.Init();
		this.back.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.PreferredWidth + 10f);
		this.back.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num + 11f);
		base.TweenValue(0f, 1f, this.OpenTime, EaseUtils.EaseType.linear, new AbstractMonoBehaviour.TweenUpdateHandler(this.OpenTween));
	}

	// Token: 0x06001096 RID: 4246 RVA: 0x0009F750 File Offset: 0x0009DB50
	public void Close()
	{
		this.closeScale = base.transform.localScale.x;
		this.StopAllCoroutines();
		base.TweenValue(0f, 1f, this.CloseTime, EaseUtils.EaseType.linear, new AbstractMonoBehaviour.TweenUpdateHandler(this.CloseTween));
	}

	// Token: 0x06001097 RID: 4247 RVA: 0x0009F7A4 File Offset: 0x0009DBA4
	protected virtual void OpenTween(float value)
	{
		float num = 40f;
		this.back.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.PreferredWidth + 10f);
		this.back.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num + 11f);
		base.transform.localScale = Vector3.one * EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0f, this.OpenScale, value);
	}

	// Token: 0x06001098 RID: 4248 RVA: 0x0009F80C File Offset: 0x0009DC0C
	protected virtual void CloseTween(float value)
	{
		base.transform.localScale = Vector3.one * EaseUtils.Ease(EaseUtils.EaseType.easeInBack, this.closeScale, 0f, value);
		if (base.transform.localScale.x < 0.001f)
		{
			this.StopAllCoroutines();
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x040019C1 RID: 6593
	protected const float PADDINGH = 10f;

	// Token: 0x040019C2 RID: 6594
	protected const float PADDINGV = 11f;

	// Token: 0x040019C3 RID: 6595
	[SerializeField]
	protected Text uiText;

	// Token: 0x040019C4 RID: 6596
	[SerializeField]
	protected TextMeshProUGUI tmpText;

	// Token: 0x040019C5 RID: 6597
	[SerializeField]
	protected CupheadGlyph glyph;

	// Token: 0x040019C6 RID: 6598
	[SerializeField]
	protected RectTransform back;

	// Token: 0x040019C7 RID: 6599
	protected Transform target;

	// Token: 0x040019C8 RID: 6600
	protected Vector2 dialogueOffset;

	// Token: 0x040019C9 RID: 6601
	private float closeScale;

	// Token: 0x0200044F RID: 1103
	public enum AnimationType
	{
		// Token: 0x040019CB RID: 6603
		Full,
		// Token: 0x040019CC RID: 6604
		Individual
	}

	// Token: 0x02000450 RID: 1104
	[Serializable]
	public class Properties
	{
		// Token: 0x06001099 RID: 4249 RVA: 0x0009F870 File Offset: 0x0009DC70
		public Properties()
		{
			this.text = string.Empty;
			this.subtext = string.Empty;
			this.button = CupheadButton.Accept;
			this.animationType = AbstractUIInteractionDialogue.AnimationType.Full;
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x0009F8C8 File Offset: 0x0009DCC8
		public Properties(string text)
		{
			this.text = text;
			this.subtext = string.Empty;
			this.button = CupheadButton.Accept;
			this.animationType = AbstractUIInteractionDialogue.AnimationType.Full;
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x0009F91C File Offset: 0x0009DD1C
		public Properties(string text, CupheadButton button)
		{
			this.text = text;
			this.subtext = string.Empty;
			this.button = button;
			this.animationType = AbstractUIInteractionDialogue.AnimationType.Full;
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x0009F970 File Offset: 0x0009DD70
		public Properties(string text, CupheadButton button, AbstractUIInteractionDialogue.AnimationType animationType)
		{
			this.text = text;
			this.subtext = string.Empty;
			this.button = button;
			this.animationType = animationType;
		}

		// Token: 0x040019CD RID: 6605
		public const AbstractUIInteractionDialogue.AnimationType DEFAULT_ANIM_TYPE = AbstractUIInteractionDialogue.AnimationType.Full;

		// Token: 0x040019CE RID: 6606
		public const CupheadButton DEFAULT_BUTTON = CupheadButton.Accept;

		// Token: 0x040019CF RID: 6607
		public string text = string.Empty;

		// Token: 0x040019D0 RID: 6608
		public string subtext = string.Empty;

		// Token: 0x040019D1 RID: 6609
		public AbstractUIInteractionDialogue.AnimationType animationType;

		// Token: 0x040019D2 RID: 6610
		public CupheadButton button = CupheadButton.Accept;
	}
}
