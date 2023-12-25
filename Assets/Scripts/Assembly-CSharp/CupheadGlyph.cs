using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000458 RID: 1112
public class CupheadGlyph : MonoBehaviour
{
	// Token: 0x170002A5 RID: 677
	// (get) Token: 0x060010CF RID: 4303 RVA: 0x000A0E2C File Offset: 0x0009F22C
	public float preferredWidth
	{
		get
		{
			return Mathf.Max(this.glyphText.preferredWidth + this.paddingText, this.glyphChar.rectTransform.sizeDelta.y);
		}
	}

	// Token: 0x060010D0 RID: 4304 RVA: 0x000A0E68 File Offset: 0x0009F268
	private void Awake()
	{
		this.initialFontSize = this.glyphChar.fontSize;
		this.initialCharColor = this.glyphChar.color;
		this.initialScale = base.transform.localScale;
		this.initialCharWrapMode = this.glyphChar.verticalOverflow;
		if (this.platformGlyphType == CupheadGlyph.PlatformGlyphType.TutorialInstruction || this.platformGlyphType == CupheadGlyph.PlatformGlyphType.TutorialInstructionDescend || this.platformGlyphType == CupheadGlyph.PlatformGlyphType.Shop || this.platformGlyphType == CupheadGlyph.PlatformGlyphType.ShmupTutorial)
		{
			this.initialCharMaterial = this.glyphChar.material;
		}
	}

	// Token: 0x060010D1 RID: 4305 RVA: 0x000A0EFA File Offset: 0x0009F2FA
	private void Start()
	{
		this.Init();
		PlayerManager.OnControlsChanged += this.OnControlsChanged;
		Localization.OnLanguageChangedEvent += this.OnLanguageChanged;
	}

	// Token: 0x060010D2 RID: 4306 RVA: 0x000A0F24 File Offset: 0x0009F324
	private void OnLanguageChanged()
	{
		this.Init();
	}

	// Token: 0x060010D3 RID: 4307 RVA: 0x000A0F2C File Offset: 0x0009F32C
	private void OnControlsChanged()
	{
		this.Init();
	}

	// Token: 0x060010D4 RID: 4308 RVA: 0x000A0F34 File Offset: 0x0009F334
	public void Init()
	{
		Localization.Translation translation = CupheadInput.InputDisplayForButton(this.button, this.rewiredPlayerId);
		this.AlignDashInstructions(translation);
		string text = translation.text;
		bool flag = text.Length > 1;
		this.glyphSymbolText.gameObject.SetActive(flag);
		this.glyphText.gameObject.SetActive(flag);
		this.glyphChar.gameObject.SetActive(!flag);
		this.glyphSymbolChar.gameObject.SetActive(!flag);
		this.glyphText.text = text;
		this.glyphChar.text = text;
		this.glyphText.font = ((translation.fonts == null) ? Localization.Instance.fonts[(int)Localization.language][29].font : translation.fonts.font);
		for (int i = 0; i < this.rectTransformTexts.Length; i++)
		{
			if (flag)
			{
				float preferredWidth = this.preferredWidth;
				if (this.maxSize > 0f && preferredWidth > this.maxSize)
				{
					preferredWidth = this.maxSize;
				}
				this.rectTransformTexts[i].sizeDelta = new Vector2(preferredWidth, this.rectTransformTexts[i].sizeDelta.y);
			}
			else
			{
				RectTransform component = this.glyphChar.GetComponent<RectTransform>();
				if (component != null)
				{
					byte[] bytes = Encoding.ASCII.GetBytes(text);
					if (bytes.Length > 0)
					{
						int num = (int)(bytes[0] - 65);
						if (this.letterOffset == CupheadGlyph.LetterOffset.Normal)
						{
							if (num >= 0 && num < CupheadGlyph.letterSpecificOffset.Length)
							{
								component.anchoredPosition = CupheadGlyph.letterSpecificOffset[num];
							}
							else
							{
								num = this.PS4CharToIndex((char)bytes[0]);
								if (num >= 0)
								{
									component.anchoredPosition = this.ps4NormalOffset[num];
								}
							}
						}
						else if (num >= 0 && num < CupheadGlyph.letterSpecificSmallOffset.Length)
						{
							component.anchoredPosition = CupheadGlyph.letterSpecificSmallOffset[num];
						}
						else
						{
							num = this.PS4CharToIndex((char)bytes[0]);
							if (num >= 0)
							{
								component.anchoredPosition = CupheadGlyph.ps4SmallOffset[num];
							}
						}
					}
				}
				this.rectTransformTexts[i].sizeDelta = new Vector2(Mathf.Max(this.preferredWidth, this.rectTransformTexts[i].sizeDelta.y), this.rectTransformTexts[i].sizeDelta.y);
			}
		}
		LayoutElement component2 = base.GetComponent<LayoutElement>();
		if (component2 != null)
		{
			component2.preferredWidth = ((!flag) ? (this.preferredWidth - this.paddingText) : this.preferredWidth);
		}
		if (flag && this.maxSize > 0f)
		{
			this.glyphText.resizeTextMaxSize = this.glyphText.fontSize * 4;
			this.glyphText.resizeTextForBestFit = true;
			RectTransform component3 = this.glyphText.GetComponent<RectTransform>();
			component3.sizeDelta *= 4f;
			component3.localScale = Vector3.one * 0.25f;
		}
	}

	// Token: 0x060010D5 RID: 4309 RVA: 0x000A1294 File Offset: 0x0009F694
	private void OnDestroy()
	{
		PlayerManager.OnControlsChanged -= this.OnControlsChanged;
		Localization.OnLanguageChangedEvent -= this.OnLanguageChanged;
	}

	// Token: 0x060010D6 RID: 4310 RVA: 0x000A12B8 File Offset: 0x0009F6B8
	private int PS4CharToIndex(char c)
	{
		return -1;
	}

	// Token: 0x060010D7 RID: 4311 RVA: 0x000A12BB File Offset: 0x0009F6BB
	private int SwitchCharToIndex(char c)
	{
		if (c == CupheadGlyph.NintendoSwitchUp)
		{
			return 0;
		}
		if (c == CupheadGlyph.NintendoSwitchDown)
		{
			return 1;
		}
		if (c == CupheadGlyph.NintendoSwitchLeft)
		{
			return 2;
		}
		if (c == CupheadGlyph.NintendoSwitchRight)
		{
			return 3;
		}
		return -1;
	}

	// Token: 0x060010D8 RID: 4312 RVA: 0x000A12F4 File Offset: 0x0009F6F4
	private void SetSwitchGlyph(bool isSwitchGlyph, RectTransform rectTransform)
	{
		if (isSwitchGlyph)
		{
			this.glyphSymbolChar.gameObject.SetActive(false);
			this.glyphChar.fontSize = CupheadGlyph.NintendoSwitchFontSize;
			this.glyphChar.color = CupheadGlyph.NintendoSwitchColor;
			this.glyphChar.verticalOverflow = VerticalWrapMode.Overflow;
			if (this.platformGlyphType == CupheadGlyph.PlatformGlyphType.TutorialInstruction || this.platformGlyphType == CupheadGlyph.PlatformGlyphType.TutorialInstructionDescend)
			{
				this.glyphChar.material = null;
				this.glyphChar.color = CupheadGlyph.NintendoSwitchTutorialInstructionColor;
			}
			else if (this.platformGlyphType == CupheadGlyph.PlatformGlyphType.Shop || this.platformGlyphType == CupheadGlyph.PlatformGlyphType.ShmupTutorial)
			{
				this.glyphChar.material = null;
			}
			if (this.platformGlyphType == CupheadGlyph.PlatformGlyphType.SwitchWeapon)
			{
				base.transform.localScale = Vector3.one;
			}
			if (this.platformGlyphType == CupheadGlyph.PlatformGlyphType.Equip)
			{
				this.glyphChar.GetComponent<Shadow>().enabled = true;
				this.glyphChar.GetComponent<Outline>().enabled = true;
			}
			Vector2 anchoredPosition = CupheadGlyph.SwitchOffsetMapping[(int)this.platformGlyphType];
			rectTransform.anchoredPosition = anchoredPosition;
		}
		else
		{
			this.glyphSymbolChar.gameObject.SetActive(true);
			this.glyphChar.fontSize = this.initialFontSize;
			this.glyphChar.color = this.initialCharColor;
			this.glyphChar.verticalOverflow = this.initialCharWrapMode;
			if (this.platformGlyphType == CupheadGlyph.PlatformGlyphType.TutorialInstruction || this.platformGlyphType == CupheadGlyph.PlatformGlyphType.TutorialInstructionDescend || this.platformGlyphType == CupheadGlyph.PlatformGlyphType.Shop || this.platformGlyphType == CupheadGlyph.PlatformGlyphType.ShmupTutorial)
			{
				this.glyphChar.material = this.initialCharMaterial;
			}
			if (this.platformGlyphType == CupheadGlyph.PlatformGlyphType.SwitchWeapon)
			{
				base.transform.localScale = this.initialScale;
			}
			if (this.platformGlyphType == CupheadGlyph.PlatformGlyphType.Equip)
			{
				this.glyphChar.GetComponent<Shadow>().enabled = false;
				this.glyphChar.GetComponent<Outline>().enabled = false;
			}
		}
	}

	// Token: 0x060010D9 RID: 4313 RVA: 0x000A14D8 File Offset: 0x0009F8D8
	public void AlignDashInstructions(Localization.Translation translation)
	{
		if (this.glyphLayouts != null)
		{
			bool enabled = !translation.text.Equals("Y");
			for (int i = 0; i < this.glyphLayouts.Length; i++)
			{
				this.glyphLayouts[i].enabled = enabled;
			}
		}
	}

	// Token: 0x04001A17 RID: 6679
	public static readonly char NintendoSwitchUp = '{';

	// Token: 0x04001A18 RID: 6680
	public static readonly char NintendoSwitchDown = '}';

	// Token: 0x04001A19 RID: 6681
	public static readonly char NintendoSwitchLeft = '<';

	// Token: 0x04001A1A RID: 6682
	public static readonly char NintendoSwitchRight = '>';

	// Token: 0x04001A1B RID: 6683
	public static readonly char PlayStation4Cross = '†';

	// Token: 0x04001A1C RID: 6684
	public static readonly char PlayStation4Circle = '‡';

	// Token: 0x04001A1D RID: 6685
	public static readonly char PlayStation4Square = '°';

	// Token: 0x04001A1E RID: 6686
	public static readonly char PlayStation4Triangle = '~';

	// Token: 0x04001A1F RID: 6687
	private static readonly int NintendoSwitchFontSize = 24;

	// Token: 0x04001A20 RID: 6688
	private static readonly Color NintendoSwitchColor = Color.white;

	// Token: 0x04001A21 RID: 6689
	private static readonly Color NintendoSwitchTutorialInstructionColor = new Color(0.25490198f, 0.25490198f, 0.25490198f, 1f);

	// Token: 0x04001A22 RID: 6690
	private static readonly Vector2[] letterSpecificOffset = new Vector2[]
	{
		new Vector2(1.6f, -0.4f),
		new Vector2(1.29f, -0.85f),
		new Vector2(1.3f, -1f),
		new Vector2(1.81f, -1.18f),
		new Vector2(0.9f, -1f),
		new Vector2(0.9f, -1f),
		new Vector2(1.2f, -1f),
		new Vector2(1f, -1f),
		new Vector2(0.8f, -1.2f),
		new Vector2(1.3f, -1.2f),
		new Vector2(0.5f, -1.2f),
		new Vector2(1.1f, -1f),
		new Vector2(0.8f, -1f),
		new Vector2(1.2f, -1.2f),
		new Vector2(1.1f, -1f),
		new Vector2(1.3f, -1.2f),
		new Vector2(1.1f, 0f),
		new Vector2(1.5f, -1.2f),
		new Vector2(1.5f, -1.2f),
		new Vector2(1.3f, -1.8f),
		new Vector2(0.9f, -1.4f),
		new Vector2(1.35f, -1.6f),
		new Vector2(0.6f, -2f),
		new Vector2(0.8f, -1.3f),
		new Vector2(0.95f, -1.8f),
		new Vector2(1.6f, -1f)
	};

	// Token: 0x04001A23 RID: 6691
	private static readonly Vector2[] letterSpecificSmallOffset = new Vector2[]
	{
		new Vector2(1.2f, 0f),
		new Vector2(0.5f, -0.2f),
		new Vector2(0.9f, -0.6f),
		new Vector2(1.1f, -0.3f),
		new Vector2(0.32f, -0.27f),
		new Vector2(0.32f, -0.85f),
		new Vector2(0.93f, -0.64f),
		new Vector2(0.64f, -0.56f),
		new Vector2(0.69f, -0.56f),
		new Vector2(0.53f, -0.38f),
		new Vector2(1.01f, -0.38f),
		new Vector2(0.77f, -0.19f),
		new Vector2(0.93f, -0.49f),
		new Vector2(0.79f, -0.67f),
		new Vector2(0.92f, -0.47f),
		new Vector2(1.34f, -0.44f),
		new Vector2(0.97f, 0.63f),
		new Vector2(1.01f, -0.3f),
		new Vector2(0.81f, -0.8f),
		new Vector2(0.48f, -1.02f),
		new Vector2(0.23f, -0.81f),
		new Vector2(0.44f, -0.81f),
		new Vector2(0.94f, -1.36f),
		new Vector2(1.19f, -0.73f),
		new Vector2(1.19f, -0.62f),
		new Vector2(0.89f, -0.62f)
	};

	// Token: 0x04001A24 RID: 6692
	private static readonly Vector2[] ps4SmallOffset = new Vector2[]
	{
		new Vector2(0.69f, -0.48f),
		new Vector2(0.97f, -0.38f),
		new Vector2(0.91f, -0.34f),
		new Vector2(1.11f, 0.41f)
	};

	// Token: 0x04001A25 RID: 6693
	protected Vector2[] ps4NormalOffset = new Vector2[]
	{
		new Vector2(1.74f, -1f),
		new Vector2(2.27f, -0.97f),
		new Vector2(2.78f, -1.13f),
		new Vector2(1.55f, 0.55f)
	};

	// Token: 0x04001A26 RID: 6694
	private static readonly Dictionary<int, Vector2> SwitchOffsetMapping = new Dictionary<int, Vector2>
	{
		{
			0,
			new Vector2(-0.97f, -1f)
		},
		{
			1,
			new Vector2(0f, 9.06f)
		},
		{
			2,
			new Vector2(3.24f, 9.06f)
		},
		{
			3,
			new Vector2(-8.3f, -0.8f)
		},
		{
			4,
			new Vector2(0f, 9.06f)
		},
		{
			5,
			new Vector2(-1.61f, -1f)
		},
		{
			6,
			new Vector2(-0.97f, 9.06f)
		},
		{
			7,
			new Vector2(0f, 0f)
		},
		{
			8,
			new Vector2(0f, 6f)
		}
	};

	// Token: 0x04001A27 RID: 6695
	private static readonly Dictionary<int, Vector2> PlayStation4OffsetMapping = new Dictionary<int, Vector2>
	{
		{
			0,
			new Vector2(1.1f, -1f)
		},
		{
			1,
			new Vector2(0.8f, -0.4f)
		},
		{
			2,
			new Vector2(0.8f, -0.4f)
		},
		{
			3,
			new Vector2(1f, -1f)
		},
		{
			4,
			new Vector2(0.1f, -0.1f)
		},
		{
			5,
			new Vector2(0.3f, -0.35f)
		},
		{
			6,
			new Vector2(0.5f, -0.4f)
		},
		{
			7,
			new Vector2(0.5f, 0f)
		},
		{
			8,
			new Vector2(1.2f, -1.09f)
		}
	};

	// Token: 0x04001A28 RID: 6696
	protected const float PADDINGH = 25f;

	// Token: 0x04001A29 RID: 6697
	public int rewiredPlayerId;

	// Token: 0x04001A2A RID: 6698
	public CupheadButton button;

	// Token: 0x04001A2B RID: 6699
	[SerializeField]
	private Image glyphSymbolText;

	// Token: 0x04001A2C RID: 6700
	[SerializeField]
	private Text glyphText;

	// Token: 0x04001A2D RID: 6701
	[SerializeField]
	private Image glyphSymbolChar;

	// Token: 0x04001A2E RID: 6702
	[SerializeField]
	private Text glyphChar;

	// Token: 0x04001A2F RID: 6703
	[SerializeField]
	private RectTransform[] rectTransformTexts;

	// Token: 0x04001A30 RID: 6704
	[SerializeField]
	protected Vector2 startSize = new Vector2(37f, 37f);

	// Token: 0x04001A31 RID: 6705
	[SerializeField]
	protected float paddingText = 10.7f;

	// Token: 0x04001A32 RID: 6706
	[SerializeField]
	private float maxSize;

	// Token: 0x04001A33 RID: 6707
	[SerializeField]
	private CupheadGlyph.LetterOffset letterOffset;

	// Token: 0x04001A34 RID: 6708
	[SerializeField]
	private CupheadGlyph.PlatformGlyphType platformGlyphType;

	// Token: 0x04001A35 RID: 6709
	[SerializeField]
	private CustomLanguageLayout[] glyphLayouts;

	// Token: 0x04001A36 RID: 6710
	private int initialFontSize;

	// Token: 0x04001A37 RID: 6711
	private Color initialCharColor;

	// Token: 0x04001A38 RID: 6712
	private Vector3 initialScale;

	// Token: 0x04001A39 RID: 6713
	private VerticalWrapMode initialCharWrapMode;

	// Token: 0x04001A3A RID: 6714
	private Material initialCharMaterial;

	// Token: 0x02000459 RID: 1113
	public enum LetterOffset
	{
		// Token: 0x04001A3C RID: 6716
		Normal,
		// Token: 0x04001A3D RID: 6717
		Small
	}

	// Token: 0x0200045A RID: 1114
	public enum PlatformGlyphType
	{
		// Token: 0x04001A3F RID: 6719
		Normal,
		// Token: 0x04001A40 RID: 6720
		TutorialInstruction,
		// Token: 0x04001A41 RID: 6721
		TutorialInstructionDescend,
		// Token: 0x04001A42 RID: 6722
		LevelUIInteractionDialogue,
		// Token: 0x04001A43 RID: 6723
		Shop,
		// Token: 0x04001A44 RID: 6724
		SwitchWeapon,
		// Token: 0x04001A45 RID: 6725
		ShmupTutorial,
		// Token: 0x04001A46 RID: 6726
		Equip,
		// Token: 0x04001A47 RID: 6727
		OffsetPrompt
	}
}
