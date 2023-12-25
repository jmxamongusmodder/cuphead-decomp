using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TMPro
{
	// Token: 0x02000C8A RID: 3210
	public class TMP_Text : MaskableGraphic
	{
		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x06005066 RID: 20582 RVA: 0x0027C3C7 File Offset: 0x0027A7C7
		// (set) Token: 0x06005067 RID: 20583 RVA: 0x0027C3D0 File Offset: 0x0027A7D0
		public string text
		{
			get
			{
				return this.m_text;
			}
			set
			{
				if (this.m_text == value)
				{
					return;
				}
				this.m_text = value;
				this.m_inputSource = TMP_Text.TextInputSources.Text;
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06005068 RID: 20584 RVA: 0x0027C41E File Offset: 0x0027A81E
		// (set) Token: 0x06005069 RID: 20585 RVA: 0x0027C428 File Offset: 0x0027A828
		public TMP_FontAsset font
		{
			get
			{
				return this.m_fontAsset;
			}
			set
			{
				if (this.m_fontAsset == value)
				{
					return;
				}
				this.m_fontAsset = value;
				this.LoadFontAsset();
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x0600506A RID: 20586 RVA: 0x0027C475 File Offset: 0x0027A875
		// (set) Token: 0x0600506B RID: 20587 RVA: 0x0027C47D File Offset: 0x0027A87D
		public virtual Material fontSharedMaterial
		{
			get
			{
				return this.m_sharedMaterial;
			}
			set
			{
				if (this.m_sharedMaterial == value)
				{
					return;
				}
				this.SetSharedMaterial(value);
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetMaterialDirty();
			}
		}

		// Token: 0x17000863 RID: 2147
		// (get) Token: 0x0600506C RID: 20588 RVA: 0x0027C4B2 File Offset: 0x0027A8B2
		// (set) Token: 0x0600506D RID: 20589 RVA: 0x0027C4BA File Offset: 0x0027A8BA
		public virtual Material[] fontSharedMaterials
		{
			get
			{
				return this.GetSharedMaterials();
			}
			set
			{
				this.SetSharedMaterials(value);
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetMaterialDirty();
			}
		}

		// Token: 0x17000864 RID: 2148
		// (get) Token: 0x0600506E RID: 20590 RVA: 0x0027C4DD File Offset: 0x0027A8DD
		// (set) Token: 0x0600506F RID: 20591 RVA: 0x0027C4EC File Offset: 0x0027A8EC
		public Material fontMaterial
		{
			get
			{
				return this.GetMaterial(this.m_sharedMaterial);
			}
			set
			{
				if (this.m_sharedMaterial != null && this.m_sharedMaterial.GetInstanceID() == value.GetInstanceID())
				{
					return;
				}
				this.m_sharedMaterial = value;
				this.m_padding = this.GetPaddingForMaterial();
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetMaterialDirty();
			}
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06005070 RID: 20592 RVA: 0x0027C54E File Offset: 0x0027A94E
		// (set) Token: 0x06005071 RID: 20593 RVA: 0x0027C55C File Offset: 0x0027A95C
		public virtual Material[] fontMaterials
		{
			get
			{
				return this.GetMaterials(this.m_fontSharedMaterials);
			}
			set
			{
				this.SetSharedMaterials(value);
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetMaterialDirty();
			}
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06005072 RID: 20594 RVA: 0x0027C57F File Offset: 0x0027A97F
		// (set) Token: 0x06005073 RID: 20595 RVA: 0x0027C587 File Offset: 0x0027A987
		public new Color color
		{
			get
			{
				return this.m_fontColor;
			}
			set
			{
				if (this.m_fontColor == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_fontColor = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06005074 RID: 20596 RVA: 0x0027C5AF File Offset: 0x0027A9AF
		// (set) Token: 0x06005075 RID: 20597 RVA: 0x0027C5BC File Offset: 0x0027A9BC
		public float alpha
		{
			get
			{
				return this.m_fontColor.a;
			}
			set
			{
				if (this.m_fontColor.a == value)
				{
					return;
				}
				this.m_fontColor.a = value;
				this.m_havePropertiesChanged = true;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06005076 RID: 20598 RVA: 0x0027C5E9 File Offset: 0x0027A9E9
		// (set) Token: 0x06005077 RID: 20599 RVA: 0x0027C5F1 File Offset: 0x0027A9F1
		public bool enableVertexGradient
		{
			get
			{
				return this.m_enableVertexGradient;
			}
			set
			{
				if (this.m_enableVertexGradient == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_enableVertexGradient = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06005078 RID: 20600 RVA: 0x0027C614 File Offset: 0x0027AA14
		// (set) Token: 0x06005079 RID: 20601 RVA: 0x0027C61C File Offset: 0x0027AA1C
		public VertexGradient colorGradient
		{
			get
			{
				return this.m_fontColorGradient;
			}
			set
			{
				this.m_havePropertiesChanged = true;
				this.m_fontColorGradient = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x0600507A RID: 20602 RVA: 0x0027C632 File Offset: 0x0027AA32
		// (set) Token: 0x0600507B RID: 20603 RVA: 0x0027C63A File Offset: 0x0027AA3A
		public TMP_SpriteAsset spriteAsset
		{
			get
			{
				return this.m_spriteAsset;
			}
			set
			{
				this.m_spriteAsset = value;
			}
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x0600507C RID: 20604 RVA: 0x0027C643 File Offset: 0x0027AA43
		// (set) Token: 0x0600507D RID: 20605 RVA: 0x0027C64B File Offset: 0x0027AA4B
		public bool tintAllSprites
		{
			get
			{
				return this.m_tintAllSprites;
			}
			set
			{
				if (this.m_tintAllSprites == value)
				{
					return;
				}
				this.m_tintAllSprites = value;
				this.m_havePropertiesChanged = true;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x0600507E RID: 20606 RVA: 0x0027C66E File Offset: 0x0027AA6E
		// (set) Token: 0x0600507F RID: 20607 RVA: 0x0027C676 File Offset: 0x0027AA76
		public bool overrideColorTags
		{
			get
			{
				return this.m_overrideHtmlColors;
			}
			set
			{
				if (this.m_overrideHtmlColors == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_overrideHtmlColors = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06005080 RID: 20608 RVA: 0x0027C699 File Offset: 0x0027AA99
		// (set) Token: 0x06005081 RID: 20609 RVA: 0x0027C6D4 File Offset: 0x0027AAD4
		public Color32 faceColor
		{
			get
			{
				if (this.m_sharedMaterial == null)
				{
					return this.m_faceColor;
				}
				this.m_faceColor = this.m_sharedMaterial.GetColor(ShaderUtilities.ID_FaceColor);
				return this.m_faceColor;
			}
			set
			{
				if (this.m_faceColor.Compare(value))
				{
					return;
				}
				this.SetFaceColor(value);
				this.m_havePropertiesChanged = true;
				this.m_faceColor = value;
				this.SetVerticesDirty();
				this.SetMaterialDirty();
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06005082 RID: 20610 RVA: 0x0027C709 File Offset: 0x0027AB09
		// (set) Token: 0x06005083 RID: 20611 RVA: 0x0027C744 File Offset: 0x0027AB44
		public Color32 outlineColor
		{
			get
			{
				if (this.m_sharedMaterial == null)
				{
					return this.m_outlineColor;
				}
				this.m_outlineColor = this.m_sharedMaterial.GetColor(ShaderUtilities.ID_OutlineColor);
				return this.m_outlineColor;
			}
			set
			{
				if (this.m_outlineColor.Compare(value))
				{
					return;
				}
				this.SetOutlineColor(value);
				this.m_havePropertiesChanged = true;
				this.m_outlineColor = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x06005084 RID: 20612 RVA: 0x0027C773 File Offset: 0x0027AB73
		// (set) Token: 0x06005085 RID: 20613 RVA: 0x0027C7A9 File Offset: 0x0027ABA9
		public float outlineWidth
		{
			get
			{
				if (this.m_sharedMaterial == null)
				{
					return this.m_outlineWidth;
				}
				this.m_outlineWidth = this.m_sharedMaterial.GetFloat(ShaderUtilities.ID_OutlineWidth);
				return this.m_outlineWidth;
			}
			set
			{
				if (this.m_outlineWidth == value)
				{
					return;
				}
				this.SetOutlineThickness(value);
				this.m_havePropertiesChanged = true;
				this.m_outlineWidth = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x06005086 RID: 20614 RVA: 0x0027C7D3 File Offset: 0x0027ABD3
		// (set) Token: 0x06005087 RID: 20615 RVA: 0x0027C7DC File Offset: 0x0027ABDC
		public float fontSize
		{
			get
			{
				return this.m_fontSize;
			}
			set
			{
				if (this.m_fontSize == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_fontSize = value;
				if (!this.m_enableAutoSizing)
				{
					this.m_fontSizeBase = this.m_fontSize;
				}
			}
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06005088 RID: 20616 RVA: 0x0027C82E File Offset: 0x0027AC2E
		public float fontScale
		{
			get
			{
				return this.m_fontScale;
			}
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06005089 RID: 20617 RVA: 0x0027C836 File Offset: 0x0027AC36
		// (set) Token: 0x0600508A RID: 20618 RVA: 0x0027C83E File Offset: 0x0027AC3E
		public int fontWeight
		{
			get
			{
				return this.m_fontWeight;
			}
			set
			{
				if (this.m_fontWeight == value)
				{
					return;
				}
				this.m_fontWeight = value;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x0600508B RID: 20619 RVA: 0x0027C868 File Offset: 0x0027AC68
		public float pixelsPerUnit
		{
			get
			{
				Canvas canvas = base.canvas;
				if (!canvas)
				{
					return 1f;
				}
				if (!this.font)
				{
					return canvas.scaleFactor;
				}
				if (this.m_currentFontAsset == null || this.m_currentFontAsset.fontInfo.PointSize <= 0f || this.m_fontSize <= 0f)
				{
					return 1f;
				}
				return this.m_fontSize / this.m_currentFontAsset.fontInfo.PointSize;
			}
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x0600508C RID: 20620 RVA: 0x0027C8FC File Offset: 0x0027ACFC
		// (set) Token: 0x0600508D RID: 20621 RVA: 0x0027C904 File Offset: 0x0027AD04
		public bool enableAutoSizing
		{
			get
			{
				return this.m_enableAutoSizing;
			}
			set
			{
				if (this.m_enableAutoSizing == value)
				{
					return;
				}
				this.m_enableAutoSizing = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x0600508E RID: 20622 RVA: 0x0027C926 File Offset: 0x0027AD26
		// (set) Token: 0x0600508F RID: 20623 RVA: 0x0027C92E File Offset: 0x0027AD2E
		public float fontSizeMin
		{
			get
			{
				return this.m_fontSizeMin;
			}
			set
			{
				if (this.m_fontSizeMin == value)
				{
					return;
				}
				this.m_fontSizeMin = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06005090 RID: 20624 RVA: 0x0027C950 File Offset: 0x0027AD50
		// (set) Token: 0x06005091 RID: 20625 RVA: 0x0027C958 File Offset: 0x0027AD58
		public float fontSizeMax
		{
			get
			{
				return this.m_fontSizeMax;
			}
			set
			{
				if (this.m_fontSizeMax == value)
				{
					return;
				}
				this.m_fontSizeMax = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06005092 RID: 20626 RVA: 0x0027C97A File Offset: 0x0027AD7A
		// (set) Token: 0x06005093 RID: 20627 RVA: 0x0027C982 File Offset: 0x0027AD82
		public FontStyles fontStyle
		{
			get
			{
				return this.m_fontStyle;
			}
			set
			{
				if (this.m_fontStyle == value)
				{
					return;
				}
				this.m_fontStyle = value;
				this.m_havePropertiesChanged = true;
				this.checkPaddingRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06005094 RID: 20628 RVA: 0x0027C9B2 File Offset: 0x0027ADB2
		public bool isUsingBold
		{
			get
			{
				return this.m_isUsingBold;
			}
		}

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06005095 RID: 20629 RVA: 0x0027C9BA File Offset: 0x0027ADBA
		// (set) Token: 0x06005096 RID: 20630 RVA: 0x0027C9C2 File Offset: 0x0027ADC2
		public TextAlignmentOptions alignment
		{
			get
			{
				return this.m_textAlignment;
			}
			set
			{
				if (this.m_textAlignment == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_textAlignment = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06005097 RID: 20631 RVA: 0x0027C9E5 File Offset: 0x0027ADE5
		// (set) Token: 0x06005098 RID: 20632 RVA: 0x0027C9ED File Offset: 0x0027ADED
		public float characterSpacing
		{
			get
			{
				return this.m_characterSpacing;
			}
			set
			{
				if (this.m_characterSpacing == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_characterSpacing = value;
			}
		}

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06005099 RID: 20633 RVA: 0x0027CA1D File Offset: 0x0027AE1D
		// (set) Token: 0x0600509A RID: 20634 RVA: 0x0027CA25 File Offset: 0x0027AE25
		public float lineSpacing
		{
			get
			{
				return this.m_lineSpacing;
			}
			set
			{
				if (this.m_lineSpacing == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_lineSpacing = value;
			}
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x0600509B RID: 20635 RVA: 0x0027CA55 File Offset: 0x0027AE55
		// (set) Token: 0x0600509C RID: 20636 RVA: 0x0027CA5D File Offset: 0x0027AE5D
		public float paragraphSpacing
		{
			get
			{
				return this.m_paragraphSpacing;
			}
			set
			{
				if (this.m_paragraphSpacing == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_paragraphSpacing = value;
			}
		}

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x0600509D RID: 20637 RVA: 0x0027CA8D File Offset: 0x0027AE8D
		// (set) Token: 0x0600509E RID: 20638 RVA: 0x0027CA95 File Offset: 0x0027AE95
		public float characterWidthAdjustment
		{
			get
			{
				return this.m_charWidthMaxAdj;
			}
			set
			{
				if (this.m_charWidthMaxAdj == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_charWidthMaxAdj = value;
			}
		}

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x0600509F RID: 20639 RVA: 0x0027CAC5 File Offset: 0x0027AEC5
		// (set) Token: 0x060050A0 RID: 20640 RVA: 0x0027CACD File Offset: 0x0027AECD
		public bool enableWordWrapping
		{
			get
			{
				return this.m_enableWordWrapping;
			}
			set
			{
				if (this.m_enableWordWrapping == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.m_isCalculateSizeRequired = true;
				this.m_enableWordWrapping = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x060050A1 RID: 20641 RVA: 0x0027CB04 File Offset: 0x0027AF04
		// (set) Token: 0x060050A2 RID: 20642 RVA: 0x0027CB0C File Offset: 0x0027AF0C
		public float wordWrappingRatios
		{
			get
			{
				return this.m_wordWrappingRatios;
			}
			set
			{
				if (this.m_wordWrappingRatios == value)
				{
					return;
				}
				this.m_wordWrappingRatios = value;
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x060050A3 RID: 20643 RVA: 0x0027CB3C File Offset: 0x0027AF3C
		// (set) Token: 0x060050A4 RID: 20644 RVA: 0x0027CB44 File Offset: 0x0027AF44
		public TextOverflowModes OverflowMode
		{
			get
			{
				return this.m_overflowMode;
			}
			set
			{
				if (this.m_overflowMode == value)
				{
					return;
				}
				this.m_overflowMode = value;
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x060050A5 RID: 20645 RVA: 0x0027CB74 File Offset: 0x0027AF74
		// (set) Token: 0x060050A6 RID: 20646 RVA: 0x0027CB7C File Offset: 0x0027AF7C
		public bool enableKerning
		{
			get
			{
				return this.m_enableKerning;
			}
			set
			{
				if (this.m_enableKerning == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_enableKerning = value;
			}
		}

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x060050A7 RID: 20647 RVA: 0x0027CBAC File Offset: 0x0027AFAC
		// (set) Token: 0x060050A8 RID: 20648 RVA: 0x0027CBB4 File Offset: 0x0027AFB4
		public bool extraPadding
		{
			get
			{
				return this.m_enableExtraPadding;
			}
			set
			{
				if (this.m_enableExtraPadding == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_enableExtraPadding = value;
				this.UpdateMeshPadding();
				this.SetVerticesDirty();
			}
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x060050A9 RID: 20649 RVA: 0x0027CBDD File Offset: 0x0027AFDD
		// (set) Token: 0x060050AA RID: 20650 RVA: 0x0027CBE5 File Offset: 0x0027AFE5
		public bool richText
		{
			get
			{
				return this.m_isRichText;
			}
			set
			{
				if (this.m_isRichText == value)
				{
					return;
				}
				this.m_isRichText = value;
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_isInputParsingRequired = true;
			}
		}

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x060050AB RID: 20651 RVA: 0x0027CC1C File Offset: 0x0027B01C
		// (set) Token: 0x060050AC RID: 20652 RVA: 0x0027CC24 File Offset: 0x0027B024
		public bool parseCtrlCharacters
		{
			get
			{
				return this.m_parseCtrlCharacters;
			}
			set
			{
				if (this.m_parseCtrlCharacters == value)
				{
					return;
				}
				this.m_parseCtrlCharacters = value;
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_isInputParsingRequired = true;
			}
		}

		// Token: 0x17000885 RID: 2181
		// (get) Token: 0x060050AD RID: 20653 RVA: 0x0027CC5B File Offset: 0x0027B05B
		// (set) Token: 0x060050AE RID: 20654 RVA: 0x0027CC63 File Offset: 0x0027B063
		public bool isOverlay
		{
			get
			{
				return this.m_isOverlay;
			}
			set
			{
				if (this.m_isOverlay == value)
				{
					return;
				}
				this.m_isOverlay = value;
				this.SetShaderDepth();
				this.m_havePropertiesChanged = true;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x060050AF RID: 20655 RVA: 0x0027CC8C File Offset: 0x0027B08C
		// (set) Token: 0x060050B0 RID: 20656 RVA: 0x0027CC94 File Offset: 0x0027B094
		public bool isOrthographic
		{
			get
			{
				return this.m_isOrthographic;
			}
			set
			{
				if (this.m_isOrthographic == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isOrthographic = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x060050B1 RID: 20657 RVA: 0x0027CCB7 File Offset: 0x0027B0B7
		// (set) Token: 0x060050B2 RID: 20658 RVA: 0x0027CCBF File Offset: 0x0027B0BF
		public bool enableCulling
		{
			get
			{
				return this.m_isCullingEnabled;
			}
			set
			{
				if (this.m_isCullingEnabled == value)
				{
					return;
				}
				this.m_isCullingEnabled = value;
				this.SetCulling();
				this.m_havePropertiesChanged = true;
			}
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x060050B3 RID: 20659 RVA: 0x0027CCE2 File Offset: 0x0027B0E2
		// (set) Token: 0x060050B4 RID: 20660 RVA: 0x0027CCEA File Offset: 0x0027B0EA
		public bool ignoreVisibility
		{
			get
			{
				return this.m_ignoreCulling;
			}
			set
			{
				if (this.m_ignoreCulling == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_ignoreCulling = value;
			}
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x060050B5 RID: 20661 RVA: 0x0027CD07 File Offset: 0x0027B107
		// (set) Token: 0x060050B6 RID: 20662 RVA: 0x0027CD0F File Offset: 0x0027B10F
		public TextureMappingOptions horizontalMapping
		{
			get
			{
				return this.m_horizontalMapping;
			}
			set
			{
				if (this.m_horizontalMapping == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_horizontalMapping = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x060050B7 RID: 20663 RVA: 0x0027CD32 File Offset: 0x0027B132
		// (set) Token: 0x060050B8 RID: 20664 RVA: 0x0027CD3A File Offset: 0x0027B13A
		public TextureMappingOptions verticalMapping
		{
			get
			{
				return this.m_verticalMapping;
			}
			set
			{
				if (this.m_verticalMapping == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_verticalMapping = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x060050B9 RID: 20665 RVA: 0x0027CD5D File Offset: 0x0027B15D
		// (set) Token: 0x060050BA RID: 20666 RVA: 0x0027CD65 File Offset: 0x0027B165
		public TextRenderFlags renderMode
		{
			get
			{
				return this.m_renderMode;
			}
			set
			{
				if (this.m_renderMode == value)
				{
					return;
				}
				this.m_renderMode = value;
				this.m_havePropertiesChanged = true;
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x060050BB RID: 20667 RVA: 0x0027CD82 File Offset: 0x0027B182
		// (set) Token: 0x060050BC RID: 20668 RVA: 0x0027CD8A File Offset: 0x0027B18A
		public int maxVisibleCharacters
		{
			get
			{
				return this.m_maxVisibleCharacters;
			}
			set
			{
				if (this.m_maxVisibleCharacters == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_maxVisibleCharacters = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x060050BD RID: 20669 RVA: 0x0027CDAD File Offset: 0x0027B1AD
		// (set) Token: 0x060050BE RID: 20670 RVA: 0x0027CDB5 File Offset: 0x0027B1B5
		public int maxVisibleWords
		{
			get
			{
				return this.m_maxVisibleWords;
			}
			set
			{
				if (this.m_maxVisibleWords == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_maxVisibleWords = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x060050BF RID: 20671 RVA: 0x0027CDD8 File Offset: 0x0027B1D8
		// (set) Token: 0x060050C0 RID: 20672 RVA: 0x0027CDE0 File Offset: 0x0027B1E0
		public int maxVisibleLines
		{
			get
			{
				return this.m_maxVisibleLines;
			}
			set
			{
				if (this.m_maxVisibleLines == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.m_maxVisibleLines = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x060050C1 RID: 20673 RVA: 0x0027CE0A File Offset: 0x0027B20A
		// (set) Token: 0x060050C2 RID: 20674 RVA: 0x0027CE12 File Offset: 0x0027B212
		public int pageToDisplay
		{
			get
			{
				return this.m_pageToDisplay;
			}
			set
			{
				if (this.m_pageToDisplay == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_pageToDisplay = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x060050C3 RID: 20675 RVA: 0x0027CE35 File Offset: 0x0027B235
		// (set) Token: 0x060050C4 RID: 20676 RVA: 0x0027CE3D File Offset: 0x0027B23D
		public virtual Vector4 margin
		{
			get
			{
				return this.m_margin;
			}
			set
			{
				if (this.m_margin == value)
				{
					return;
				}
				this.m_margin = value;
				this.ComputeMarginSize();
				this.m_havePropertiesChanged = true;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x060050C5 RID: 20677 RVA: 0x0027CE6B File Offset: 0x0027B26B
		public TMP_TextInfo textInfo
		{
			get
			{
				return this.m_textInfo;
			}
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x060050C6 RID: 20678 RVA: 0x0027CE73 File Offset: 0x0027B273
		// (set) Token: 0x060050C7 RID: 20679 RVA: 0x0027CE7B File Offset: 0x0027B27B
		public bool havePropertiesChanged
		{
			get
			{
				return this.m_havePropertiesChanged;
			}
			set
			{
				if (this.m_havePropertiesChanged == value)
				{
					return;
				}
				this.m_havePropertiesChanged = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x060050C8 RID: 20680 RVA: 0x0027CE9D File Offset: 0x0027B29D
		// (set) Token: 0x060050C9 RID: 20681 RVA: 0x0027CEA5 File Offset: 0x0027B2A5
		public bool isUsingLegacyAnimationComponent
		{
			get
			{
				return this.m_isUsingLegacyAnimationComponent;
			}
			set
			{
				this.m_isUsingLegacyAnimationComponent = value;
			}
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x060050CA RID: 20682 RVA: 0x0027CEAE File Offset: 0x0027B2AE
		public new Transform transform
		{
			get
			{
				if (this.m_transform == null)
				{
					this.m_transform = base.GetComponent<Transform>();
				}
				return this.m_transform;
			}
		}

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x060050CB RID: 20683 RVA: 0x0027CED3 File Offset: 0x0027B2D3
		public new RectTransform rectTransform
		{
			get
			{
				if (this.m_rectTransform == null)
				{
					this.m_rectTransform = base.GetComponent<RectTransform>();
				}
				return this.m_rectTransform;
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x060050CC RID: 20684 RVA: 0x0027CEF8 File Offset: 0x0027B2F8
		// (set) Token: 0x060050CD RID: 20685 RVA: 0x0027CF00 File Offset: 0x0027B300
		public virtual bool autoSizeTextContainer { get; set; }

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x060050CE RID: 20686 RVA: 0x0027CF09 File Offset: 0x0027B309
		public virtual Mesh mesh
		{
			get
			{
				return this.m_mesh;
			}
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x060050CF RID: 20687 RVA: 0x0027CF11 File Offset: 0x0027B311
		// (set) Token: 0x060050D0 RID: 20688 RVA: 0x0027CF19 File Offset: 0x0027B319
		public virtual Bounds bounds { get; set; }

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x060050D1 RID: 20689 RVA: 0x0027CF22 File Offset: 0x0027B322
		public float flexibleHeight
		{
			get
			{
				return this.m_flexibleHeight;
			}
		}

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x060050D2 RID: 20690 RVA: 0x0027CF2A File Offset: 0x0027B32A
		public float flexibleWidth
		{
			get
			{
				return this.m_flexibleWidth;
			}
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x060050D3 RID: 20691 RVA: 0x0027CF32 File Offset: 0x0027B332
		public float minHeight
		{
			get
			{
				return this.m_minHeight;
			}
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x060050D4 RID: 20692 RVA: 0x0027CF3A File Offset: 0x0027B33A
		public float minWidth
		{
			get
			{
				return this.m_minWidth;
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x060050D5 RID: 20693 RVA: 0x0027CF42 File Offset: 0x0027B342
		public virtual float preferredWidth
		{
			get
			{
				return (this.m_preferredWidth != 9999f) ? this.m_preferredWidth : this.GetPreferredWidth();
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x060050D6 RID: 20694 RVA: 0x0027CF65 File Offset: 0x0027B365
		public virtual float preferredHeight
		{
			get
			{
				return (this.m_preferredHeight != 9999f) ? this.m_preferredHeight : this.GetPreferredHeight();
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x060050D7 RID: 20695 RVA: 0x0027CF88 File Offset: 0x0027B388
		public int layoutPriority
		{
			get
			{
				return this.m_layoutPriority;
			}
		}

		// Token: 0x060050D8 RID: 20696 RVA: 0x0027CF90 File Offset: 0x0027B390
		protected virtual void LoadFontAsset()
		{
		}

		// Token: 0x060050D9 RID: 20697 RVA: 0x0027CF92 File Offset: 0x0027B392
		protected virtual void SetSharedMaterial(Material mat)
		{
		}

		// Token: 0x060050DA RID: 20698 RVA: 0x0027CF94 File Offset: 0x0027B394
		protected virtual Material GetMaterial(Material mat)
		{
			return null;
		}

		// Token: 0x060050DB RID: 20699 RVA: 0x0027CF97 File Offset: 0x0027B397
		protected virtual void SetFontBaseMaterial(Material mat)
		{
		}

		// Token: 0x060050DC RID: 20700 RVA: 0x0027CF99 File Offset: 0x0027B399
		protected virtual Material[] GetSharedMaterials()
		{
			return null;
		}

		// Token: 0x060050DD RID: 20701 RVA: 0x0027CF9C File Offset: 0x0027B39C
		protected virtual void SetSharedMaterials(Material[] materials)
		{
		}

		// Token: 0x060050DE RID: 20702 RVA: 0x0027CF9E File Offset: 0x0027B39E
		protected virtual Material[] GetMaterials(Material[] mats)
		{
			return null;
		}

		// Token: 0x060050DF RID: 20703 RVA: 0x0027CFA4 File Offset: 0x0027B3A4
		protected virtual Material CreateMaterialInstance(Material source)
		{
			Material material = new Material(source);
			material.shaderKeywords = source.shaderKeywords;
			Material material2 = material;
			material2.name += " (Instance)";
			return material;
		}

		// Token: 0x060050E0 RID: 20704 RVA: 0x0027CFDB File Offset: 0x0027B3DB
		protected virtual void SetFaceColor(Color32 color)
		{
		}

		// Token: 0x060050E1 RID: 20705 RVA: 0x0027CFDD File Offset: 0x0027B3DD
		protected virtual void SetOutlineColor(Color32 color)
		{
		}

		// Token: 0x060050E2 RID: 20706 RVA: 0x0027CFDF File Offset: 0x0027B3DF
		protected virtual void SetOutlineThickness(float thickness)
		{
		}

		// Token: 0x060050E3 RID: 20707 RVA: 0x0027CFE1 File Offset: 0x0027B3E1
		protected virtual void SetShaderDepth()
		{
		}

		// Token: 0x060050E4 RID: 20708 RVA: 0x0027CFE3 File Offset: 0x0027B3E3
		protected virtual void SetCulling()
		{
		}

		// Token: 0x060050E5 RID: 20709 RVA: 0x0027CFE5 File Offset: 0x0027B3E5
		protected virtual float GetPaddingForMaterial()
		{
			return 0f;
		}

		// Token: 0x060050E6 RID: 20710 RVA: 0x0027CFEC File Offset: 0x0027B3EC
		protected virtual float GetPaddingForMaterial(Material mat)
		{
			return 0f;
		}

		// Token: 0x060050E7 RID: 20711 RVA: 0x0027CFF3 File Offset: 0x0027B3F3
		protected virtual Vector3[] GetTextContainerLocalCorners()
		{
			return null;
		}

		// Token: 0x060050E8 RID: 20712 RVA: 0x0027CFF6 File Offset: 0x0027B3F6
		public virtual void ForceMeshUpdate()
		{
		}

		// Token: 0x060050E9 RID: 20713 RVA: 0x0027CFF8 File Offset: 0x0027B3F8
		public virtual void UpdateGeometry(Mesh mesh, int index)
		{
		}

		// Token: 0x060050EA RID: 20714 RVA: 0x0027CFFA File Offset: 0x0027B3FA
		public virtual void UpdateVertexData(TMP_VertexDataUpdateFlags flags)
		{
		}

		// Token: 0x060050EB RID: 20715 RVA: 0x0027CFFC File Offset: 0x0027B3FC
		public virtual void UpdateVertexData()
		{
		}

		// Token: 0x060050EC RID: 20716 RVA: 0x0027CFFE File Offset: 0x0027B3FE
		public virtual void SetVertices(Vector3[] vertices)
		{
		}

		// Token: 0x060050ED RID: 20717 RVA: 0x0027D000 File Offset: 0x0027B400
		public virtual void UpdateMeshPadding()
		{
		}

		// Token: 0x060050EE RID: 20718 RVA: 0x0027D002 File Offset: 0x0027B402
		public void SetText(string text)
		{
			this.StringToCharArray(text, ref this.m_char_buffer);
			this.m_inputSource = TMP_Text.TextInputSources.SetCharArray;
			this.m_isInputParsingRequired = true;
			this.m_havePropertiesChanged = true;
			this.m_isCalculateSizeRequired = true;
			this.SetVerticesDirty();
			this.SetLayoutDirty();
		}

		// Token: 0x060050EF RID: 20719 RVA: 0x0027D039 File Offset: 0x0027B439
		public void SetText(string text, float arg0)
		{
			this.SetText(text, arg0, 255f, 255f);
		}

		// Token: 0x060050F0 RID: 20720 RVA: 0x0027D04D File Offset: 0x0027B44D
		public void SetText(string text, float arg0, float arg1)
		{
			this.SetText(text, arg0, arg1, 255f);
		}

		// Token: 0x060050F1 RID: 20721 RVA: 0x0027D060 File Offset: 0x0027B460
		public void SetText(string text, float arg0, float arg1, float arg2)
		{
			if (text == this.old_text && arg0 == this.old_arg0 && arg1 == this.old_arg1 && arg2 == this.old_arg2)
			{
				return;
			}
			this.old_text = text;
			this.old_arg1 = 255f;
			this.old_arg2 = 255f;
			int precision = 0;
			int num = 0;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (c == '{')
				{
					if (text[i + 2] == ':')
					{
						precision = (int)(text[i + 3] - '0');
					}
					int num2 = (int)(text[i + 1] - '0');
					if (num2 != 0)
					{
						if (num2 != 1)
						{
							if (num2 == 2)
							{
								this.old_arg2 = arg2;
								this.AddFloatToCharArray(arg2, ref num, precision);
							}
						}
						else
						{
							this.old_arg1 = arg1;
							this.AddFloatToCharArray(arg1, ref num, precision);
						}
					}
					else
					{
						this.old_arg0 = arg0;
						this.AddFloatToCharArray(arg0, ref num, precision);
					}
					if (text[i + 2] == ':')
					{
						i += 4;
					}
					else
					{
						i += 2;
					}
				}
				else
				{
					this.m_input_CharArray[num] = c;
					num++;
				}
			}
			this.m_input_CharArray[num] = '\0';
			this.m_charArray_Length = num;
			this.m_inputSource = TMP_Text.TextInputSources.SetText;
			this.m_isInputParsingRequired = true;
			this.m_havePropertiesChanged = true;
			this.m_isCalculateSizeRequired = true;
			this.SetVerticesDirty();
			this.SetLayoutDirty();
		}

		// Token: 0x060050F2 RID: 20722 RVA: 0x0027D1DE File Offset: 0x0027B5DE
		public void SetText(StringBuilder text)
		{
			this.StringBuilderToIntArray(text, ref this.m_char_buffer);
			this.m_inputSource = TMP_Text.TextInputSources.SetCharArray;
			this.m_isInputParsingRequired = true;
			this.m_havePropertiesChanged = true;
			this.m_isCalculateSizeRequired = true;
			this.SetVerticesDirty();
			this.SetLayoutDirty();
		}

		// Token: 0x060050F3 RID: 20723 RVA: 0x0027D218 File Offset: 0x0027B618
		public void SetCharArray(char[] charArray)
		{
			if (charArray == null || charArray.Length == 0)
			{
				return;
			}
			if (this.m_char_buffer.Length <= charArray.Length)
			{
				int num = Mathf.NextPowerOfTwo(charArray.Length + 1);
				this.m_char_buffer = new int[num];
			}
			int num2 = 0;
			int i = 0;
			while (i < charArray.Length)
			{
				if (charArray[i] != '\\' || i >= charArray.Length - 1)
				{
					goto IL_BC;
				}
				int num3 = (int)charArray[i + 1];
				if (num3 != 110)
				{
					if (num3 != 114)
					{
						if (num3 != 116)
						{
							goto IL_BC;
						}
						this.m_char_buffer[num2] = 9;
						i++;
						num2++;
					}
					else
					{
						this.m_char_buffer[num2] = 13;
						i++;
						num2++;
					}
				}
				else
				{
					this.m_char_buffer[num2] = 10;
					i++;
					num2++;
				}
				IL_CB:
				i++;
				continue;
				IL_BC:
				this.m_char_buffer[num2] = (int)charArray[i];
				num2++;
				goto IL_CB;
			}
			this.m_char_buffer[num2] = 0;
			this.m_inputSource = TMP_Text.TextInputSources.SetCharArray;
			this.m_havePropertiesChanged = true;
			this.m_isInputParsingRequired = true;
		}

		// Token: 0x060050F4 RID: 20724 RVA: 0x0027D31C File Offset: 0x0027B71C
		protected void SetTextArrayToCharArray(char[] charArray, ref int[] charBuffer)
		{
			if (charArray == null || this.m_charArray_Length == 0)
			{
				return;
			}
			if (charBuffer.Length <= this.m_charArray_Length)
			{
				int num = (this.m_charArray_Length <= 1024) ? Mathf.NextPowerOfTwo(this.m_charArray_Length + 1) : (this.m_charArray_Length + 256);
				charBuffer = new int[num];
			}
			int num2 = 0;
			for (int i = 0; i < this.m_charArray_Length; i++)
			{
				if (char.IsHighSurrogate(charArray[i]) && char.IsLowSurrogate(charArray[i + 1]))
				{
					charBuffer[num2] = char.ConvertToUtf32(charArray[i], charArray[i + 1]);
					i++;
					num2++;
				}
				else
				{
					charBuffer[num2] = (int)charArray[i];
					num2++;
				}
			}
			charBuffer[num2] = 0;
		}

		// Token: 0x060050F5 RID: 20725 RVA: 0x0027D3E4 File Offset: 0x0027B7E4
		protected void StringToCharArray(string text, ref int[] chars)
		{
			if (text == null)
			{
				chars[0] = 0;
				return;
			}
			if (chars == null || chars.Length <= text.Length)
			{
				int num = (text.Length <= 1024) ? Mathf.NextPowerOfTwo(text.Length + 1) : (text.Length + 256);
				chars = new int[num];
			}
			int num2 = 0;
			int i = 0;
			while (i < text.Length)
			{
				if (!this.m_parseCtrlCharacters || text[i] != '\\' || text.Length <= i + 1)
				{
					goto IL_19B;
				}
				int num3 = (int)text[i + 1];
				switch (num3)
				{
				case 114:
					chars[num2] = 13;
					i++;
					num2++;
					break;
				default:
					if (num3 != 85)
					{
						if (num3 != 92)
						{
							if (num3 != 110)
							{
								goto IL_19B;
							}
							chars[num2] = 10;
							i++;
							num2++;
						}
						else
						{
							if (text.Length <= i + 2)
							{
								goto IL_19B;
							}
							chars[num2] = (int)text[i + 1];
							chars[num2 + 1] = (int)text[i + 2];
							i += 2;
							num2 += 2;
						}
					}
					else
					{
						if (text.Length <= i + 9)
						{
							goto IL_19B;
						}
						chars[num2] = this.GetUTF32(i + 2);
						i += 9;
						num2++;
					}
					break;
				case 116:
					chars[num2] = 9;
					i++;
					num2++;
					break;
				case 117:
					if (text.Length <= i + 5)
					{
						goto IL_19B;
					}
					chars[num2] = (int)((ushort)this.GetUTF16(i + 2));
					i += 5;
					num2++;
					break;
				}
				IL_1F4:
				i++;
				continue;
				IL_19B:
				if (char.IsHighSurrogate(text[i]) && char.IsLowSurrogate(text[i + 1]))
				{
					chars[num2] = char.ConvertToUtf32(text[i], text[i + 1]);
					i++;
					num2++;
					goto IL_1F4;
				}
				chars[num2] = (int)text[i];
				num2++;
				goto IL_1F4;
			}
			chars[num2] = 0;
		}

		// Token: 0x060050F6 RID: 20726 RVA: 0x0027D5FC File Offset: 0x0027B9FC
		protected void StringBuilderToIntArray(StringBuilder text, ref int[] chars)
		{
			if (text == null)
			{
				chars[0] = 0;
				return;
			}
			if (chars == null || chars.Length <= text.Length)
			{
				int num = (text.Length <= 1024) ? Mathf.NextPowerOfTwo(text.Length + 1) : (text.Length + 256);
				chars = new int[num];
			}
			int num2 = 0;
			int i = 0;
			while (i < text.Length)
			{
				if (!this.m_parseCtrlCharacters || text[i] != '\\' || text.Length <= i + 1)
				{
					goto IL_19B;
				}
				int num3 = (int)text[i + 1];
				switch (num3)
				{
				case 114:
					chars[num2] = 13;
					i++;
					num2++;
					break;
				default:
					if (num3 != 85)
					{
						if (num3 != 92)
						{
							if (num3 != 110)
							{
								goto IL_19B;
							}
							chars[num2] = 10;
							i++;
							num2++;
						}
						else
						{
							if (text.Length <= i + 2)
							{
								goto IL_19B;
							}
							chars[num2] = (int)text[i + 1];
							chars[num2 + 1] = (int)text[i + 2];
							i += 2;
							num2 += 2;
						}
					}
					else
					{
						if (text.Length <= i + 9)
						{
							goto IL_19B;
						}
						chars[num2] = this.GetUTF32(i + 2);
						i += 9;
						num2++;
					}
					break;
				case 116:
					chars[num2] = 9;
					i++;
					num2++;
					break;
				case 117:
					if (text.Length <= i + 5)
					{
						goto IL_19B;
					}
					chars[num2] = (int)((ushort)this.GetUTF16(i + 2));
					i += 5;
					num2++;
					break;
				}
				IL_1F4:
				i++;
				continue;
				IL_19B:
				if (char.IsHighSurrogate(text[i]) && char.IsLowSurrogate(text[i + 1]))
				{
					chars[num2] = char.ConvertToUtf32(text[i], text[i + 1]);
					i++;
					num2++;
					goto IL_1F4;
				}
				chars[num2] = (int)text[i];
				num2++;
				goto IL_1F4;
			}
			chars[num2] = 0;
		}

		// Token: 0x060050F7 RID: 20727 RVA: 0x0027D814 File Offset: 0x0027BC14
		protected void AddFloatToCharArray(float number, ref int index, int precision)
		{
			if (number < 0f)
			{
				this.m_input_CharArray[index++] = '-';
				number = -number;
			}
			number += this.k_Power[Mathf.Min(9, precision)];
			int num = (int)number;
			this.AddIntToCharArray(num, ref index, precision);
			if (precision > 0)
			{
				this.m_input_CharArray[index++] = '.';
				number -= (float)num;
				for (int i = 0; i < precision; i++)
				{
					number *= 10f;
					int num2 = (int)number;
					this.m_input_CharArray[index++] = (char)(num2 + 48);
					number -= (float)num2;
				}
			}
		}

		// Token: 0x060050F8 RID: 20728 RVA: 0x0027D8BC File Offset: 0x0027BCBC
		protected void AddIntToCharArray(int number, ref int index, int precision)
		{
			if (number < 0)
			{
				this.m_input_CharArray[index++] = '-';
				number = -number;
			}
			int num = index;
			do
			{
				this.m_input_CharArray[num++] = (char)(number % 10 + 48);
				number /= 10;
			}
			while (number > 0);
			int num2 = num;
			while (index + 1 < num)
			{
				num--;
				char c = this.m_input_CharArray[index];
				this.m_input_CharArray[index] = this.m_input_CharArray[num];
				this.m_input_CharArray[num] = c;
				index++;
			}
			index = num2;
		}

		// Token: 0x060050F9 RID: 20729 RVA: 0x0027D94C File Offset: 0x0027BD4C
		protected virtual int SetArraySizes(int[] chars)
		{
			return 0;
		}

		// Token: 0x060050FA RID: 20730 RVA: 0x0027D950 File Offset: 0x0027BD50
		protected void ParseInputText()
		{
			this.m_isInputParsingRequired = false;
			TMP_Text.TextInputSources inputSource = this.m_inputSource;
			if (inputSource != TMP_Text.TextInputSources.Text)
			{
				if (inputSource != TMP_Text.TextInputSources.SetText)
				{
					if (inputSource != TMP_Text.TextInputSources.SetCharArray)
					{
					}
				}
				else
				{
					this.SetTextArrayToCharArray(this.m_input_CharArray, ref this.m_char_buffer);
				}
			}
			else
			{
				this.StringToCharArray(this.m_text, ref this.m_char_buffer);
			}
			this.SetArraySizes(this.m_char_buffer);
		}

		// Token: 0x060050FB RID: 20731 RVA: 0x0027D9C4 File Offset: 0x0027BDC4
		protected virtual void GenerateTextMesh()
		{
		}

		// Token: 0x060050FC RID: 20732 RVA: 0x0027D9C8 File Offset: 0x0027BDC8
		public Vector2 GetPreferredValues()
		{
			if (this.m_isInputParsingRequired || this.m_isTextTruncated)
			{
				this.ParseInputText();
			}
			float preferredWidth = this.GetPreferredWidth();
			float preferredHeight = this.GetPreferredHeight();
			return new Vector2(preferredWidth, preferredHeight);
		}

		// Token: 0x060050FD RID: 20733 RVA: 0x0027DA08 File Offset: 0x0027BE08
		public Vector2 GetPreferredValues(float width, float height)
		{
			if (this.m_isInputParsingRequired || this.m_isTextTruncated)
			{
				this.ParseInputText();
			}
			Vector2 margin = new Vector2(width, height);
			float preferredWidth = this.GetPreferredWidth(margin);
			float preferredHeight = this.GetPreferredHeight(margin);
			return new Vector2(preferredWidth, preferredHeight);
		}

		// Token: 0x060050FE RID: 20734 RVA: 0x0027DA54 File Offset: 0x0027BE54
		public Vector2 GetPreferredValues(string text)
		{
			this.StringToCharArray(text, ref this.m_char_buffer);
			this.SetArraySizes(this.m_char_buffer);
			Vector2 margin = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
			float preferredWidth = this.GetPreferredWidth(margin);
			float preferredHeight = this.GetPreferredHeight(margin);
			return new Vector2(preferredWidth, preferredHeight);
		}

		// Token: 0x060050FF RID: 20735 RVA: 0x0027DAA4 File Offset: 0x0027BEA4
		public Vector2 GetPreferredValues(string text, float width, float height)
		{
			this.StringToCharArray(text, ref this.m_char_buffer);
			this.SetArraySizes(this.m_char_buffer);
			Vector2 margin = new Vector2(width, height);
			float preferredWidth = this.GetPreferredWidth(margin);
			float preferredHeight = this.GetPreferredHeight(margin);
			return new Vector2(preferredWidth, preferredHeight);
		}

		// Token: 0x06005100 RID: 20736 RVA: 0x0027DAEC File Offset: 0x0027BEEC
		protected float GetPreferredWidth()
		{
			float defaultFontSize = (!this.m_enableAutoSizing) ? this.m_fontSize : this.m_fontSizeMax;
			Vector2 marginSize = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
			if (this.m_isInputParsingRequired || this.m_isTextTruncated)
			{
				this.ParseInputText();
			}
			return this.CalculatePreferredValues(defaultFontSize, marginSize).x;
		}

		// Token: 0x06005101 RID: 20737 RVA: 0x0027DB58 File Offset: 0x0027BF58
		protected float GetPreferredWidth(Vector2 margin)
		{
			float defaultFontSize = (!this.m_enableAutoSizing) ? this.m_fontSize : this.m_fontSizeMax;
			return this.CalculatePreferredValues(defaultFontSize, margin).x;
		}

		// Token: 0x06005102 RID: 20738 RVA: 0x0027DB94 File Offset: 0x0027BF94
		protected float GetPreferredHeight()
		{
			float defaultFontSize = (!this.m_enableAutoSizing) ? this.m_fontSize : this.m_fontSizeMax;
			Vector2 marginSize = new Vector2((this.m_marginWidth == 0f) ? float.PositiveInfinity : this.m_marginWidth, float.PositiveInfinity);
			if (this.m_isInputParsingRequired || this.m_isTextTruncated)
			{
				this.ParseInputText();
			}
			return this.CalculatePreferredValues(defaultFontSize, marginSize).y;
		}

		// Token: 0x06005103 RID: 20739 RVA: 0x0027DC18 File Offset: 0x0027C018
		protected float GetPreferredHeight(Vector2 margin)
		{
			float defaultFontSize = (!this.m_enableAutoSizing) ? this.m_fontSize : this.m_fontSizeMax;
			return this.CalculatePreferredValues(defaultFontSize, margin).y;
		}

		// Token: 0x06005104 RID: 20740 RVA: 0x0027DC54 File Offset: 0x0027C054
		protected virtual Vector2 CalculatePreferredValues(float defaultFontSize, Vector2 marginSize)
		{
			if (this.m_fontAsset == null || this.m_fontAsset.characterDictionary == null)
			{
				return Vector2.zero;
			}
			if (this.m_char_buffer == null || this.m_char_buffer.Length == 0 || this.m_char_buffer[0] == 0)
			{
				return Vector2.zero;
			}
			this.m_currentFontAsset = this.m_fontAsset;
			this.m_currentMaterial = this.m_sharedMaterial;
			this.m_currentMaterialIndex = 0;
			this.m_materialReferenceStack.SetDefault(new MaterialReference(0, this.m_currentFontAsset, null, this.m_currentMaterial, this.m_padding));
			int totalCharacterCount = this.m_totalCharacterCount;
			if (this.m_internalCharacterInfo == null || totalCharacterCount > this.m_internalCharacterInfo.Length)
			{
				this.m_internalCharacterInfo = new TMP_CharacterInfo[(totalCharacterCount <= 1024) ? Mathf.NextPowerOfTwo(totalCharacterCount) : (totalCharacterCount + 256)];
			}
			this.m_fontScale = defaultFontSize / this.m_currentFontAsset.fontInfo.PointSize * ((!this.m_isOrthographic) ? 0.1f : 1f);
			this.m_fontScaleMultiplier = 1f;
			float num = defaultFontSize / this.m_fontAsset.fontInfo.PointSize * this.m_fontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
			float num2 = this.m_fontScale;
			this.m_currentFontSize = defaultFontSize;
			this.m_sizeStack.SetDefault(this.m_currentFontSize);
			this.m_style = this.m_fontStyle;
			this.m_baselineOffset = 0f;
			this.m_styleStack.Clear();
			this.m_lineOffset = 0f;
			this.m_lineHeight = 0f;
			float num3 = this.m_currentFontAsset.fontInfo.LineHeight - (this.m_currentFontAsset.fontInfo.Ascender - this.m_currentFontAsset.fontInfo.Descender);
			this.m_cSpacing = 0f;
			this.m_monoSpacing = 0f;
			this.m_xAdvance = 0f;
			float a = 0f;
			this.tag_LineIndent = 0f;
			this.tag_Indent = 0f;
			this.m_indentStack.SetDefault(0f);
			this.tag_NoParsing = false;
			this.m_characterCount = 0;
			this.m_firstCharacterOfLine = 0;
			this.m_maxLineAscender = float.NegativeInfinity;
			this.m_maxLineDescender = float.PositiveInfinity;
			this.m_lineNumber = 0;
			float x = marginSize.x;
			this.m_marginLeft = 0f;
			this.m_marginRight = 0f;
			this.m_width = -1f;
			float num4 = 0f;
			float num5 = 0f;
			this.m_maxAscender = 0f;
			this.m_maxDescender = 0f;
			bool flag = true;
			bool flag2 = false;
			WordWrapState wordWrapState = default(WordWrapState);
			this.SaveWordWrappingState(ref wordWrapState, 0, 0);
			WordWrapState wordWrapState2 = default(WordWrapState);
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			while (this.m_char_buffer[num8] != 0)
			{
				int num9 = this.m_char_buffer[num8];
				this.m_textElementType = TMP_TextElementType.Character;
				this.m_currentMaterialIndex = this.m_textInfo.characterInfo[this.m_characterCount].materialReferenceIndex;
				this.m_currentFontAsset = this.m_materialReferences[this.m_currentMaterialIndex].fontAsset;
				int currentMaterialIndex = this.m_currentMaterialIndex;
				if (!this.m_isRichText || num9 != 60)
				{
					goto IL_38C;
				}
				this.m_isParsingText = true;
				if (!this.ValidateHtmlTag(this.m_char_buffer, num8 + 1, out num7))
				{
					goto IL_38C;
				}
				num8 = num7;
				if (this.m_textElementType != TMP_TextElementType.Character)
				{
					goto IL_38C;
				}
				IL_FE9:
				num8++;
				continue;
				IL_38C:
				this.m_isParsingText = false;
				float num10 = 1f;
				if (this.m_textElementType == TMP_TextElementType.Character)
				{
					if ((this.m_style & FontStyles.UpperCase) == FontStyles.UpperCase)
					{
						if (char.IsLower((char)num9))
						{
							num9 = (int)char.ToUpper((char)num9);
						}
					}
					else if ((this.m_style & FontStyles.LowerCase) == FontStyles.LowerCase)
					{
						if (char.IsUpper((char)num9))
						{
							num9 = (int)char.ToLower((char)num9);
						}
					}
					else if (((this.m_fontStyle & FontStyles.SmallCaps) == FontStyles.SmallCaps || (this.m_style & FontStyles.SmallCaps) == FontStyles.SmallCaps) && char.IsLower((char)num9))
					{
						num10 = 0.8f;
						num9 = (int)char.ToUpper((char)num9);
					}
				}
				if (this.m_textElementType == TMP_TextElementType.Sprite)
				{
					TMP_Sprite tmp_Sprite = this.m_currentSpriteAsset.spriteInfoList[this.m_spriteIndex];
					if (tmp_Sprite == null)
					{
						goto IL_FE9;
					}
					num9 = 57344 + this.m_spriteIndex;
					this.m_cached_TextElement = tmp_Sprite;
					num2 = this.m_fontAsset.fontInfo.Ascender / tmp_Sprite.height * tmp_Sprite.scale * num;
					this.m_internalCharacterInfo[this.m_characterCount].elementType = TMP_TextElementType.Sprite;
					this.m_currentMaterialIndex = currentMaterialIndex;
				}
				else if (this.m_textElementType == TMP_TextElementType.Character)
				{
					this.m_cached_TextElement = this.m_textInfo.characterInfo[this.m_characterCount].textElement;
					this.m_currentFontAsset = this.m_textInfo.characterInfo[this.m_characterCount].fontAsset;
					this.m_currentMaterialIndex = this.m_textInfo.characterInfo[this.m_characterCount].materialReferenceIndex;
					this.m_fontScale = this.m_currentFontSize * num10 / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
					num2 = this.m_fontScale * this.m_fontScaleMultiplier;
					this.m_internalCharacterInfo[this.m_characterCount].elementType = TMP_TextElementType.Character;
				}
				this.m_internalCharacterInfo[this.m_characterCount].character = (char)num9;
				if (this.m_enableKerning && this.m_characterCount >= 1)
				{
					int character = (int)this.m_internalCharacterInfo[this.m_characterCount - 1].character;
					KerningPairKey kerningPairKey = new KerningPairKey(character, num9);
					KerningPair kerningPair;
					this.m_currentFontAsset.kerningDictionary.TryGetValue(kerningPairKey.key, out kerningPair);
					if (kerningPair != null)
					{
						this.m_xAdvance += kerningPair.XadvanceOffset * num2;
					}
				}
				float num11 = 0f;
				if (this.m_monoSpacing != 0f)
				{
					num11 = this.m_monoSpacing / 2f - (this.m_cached_TextElement.width / 2f + this.m_cached_TextElement.xOffset) * num2;
					this.m_xAdvance += num11;
				}
				float num12;
				if ((this.m_style & FontStyles.Bold) == FontStyles.Bold || (this.m_fontStyle & FontStyles.Bold) == FontStyles.Bold)
				{
					num12 = 1f + this.m_currentFontAsset.boldSpacing * 0.01f;
				}
				else
				{
					num12 = 1f;
				}
				this.m_internalCharacterInfo[this.m_characterCount].baseLine = 0f - this.m_lineOffset + this.m_baselineOffset;
				float num13 = this.m_currentFontAsset.fontInfo.Ascender * ((this.m_textElementType != TMP_TextElementType.Character) ? num : num2) + this.m_baselineOffset;
				this.m_internalCharacterInfo[this.m_characterCount].ascender = num13 - this.m_lineOffset;
				this.m_maxLineAscender = ((num13 <= this.m_maxLineAscender) ? this.m_maxLineAscender : num13);
				float num14 = this.m_currentFontAsset.fontInfo.Descender * ((this.m_textElementType != TMP_TextElementType.Character) ? num : num2) + this.m_baselineOffset;
				float num15 = this.m_internalCharacterInfo[this.m_characterCount].descender = num14 - this.m_lineOffset;
				this.m_maxLineDescender = ((num14 >= this.m_maxLineDescender) ? this.m_maxLineDescender : num14);
				if ((this.m_style & FontStyles.Subscript) == FontStyles.Subscript || (this.m_style & FontStyles.Superscript) == FontStyles.Superscript)
				{
					float num16 = (num13 - this.m_baselineOffset) / this.m_currentFontAsset.fontInfo.SubSize;
					num13 = this.m_maxLineAscender;
					this.m_maxLineAscender = ((num16 <= this.m_maxLineAscender) ? this.m_maxLineAscender : num16);
					float num17 = (num14 - this.m_baselineOffset) / this.m_currentFontAsset.fontInfo.SubSize;
					num14 = this.m_maxLineDescender;
					this.m_maxLineDescender = ((num17 >= this.m_maxLineDescender) ? this.m_maxLineDescender : num17);
				}
				if (this.m_lineNumber == 0)
				{
					this.m_maxAscender = ((this.m_maxAscender <= num13) ? num13 : this.m_maxAscender);
				}
				if (num9 == 9 || !char.IsWhiteSpace((char)num9) || this.m_textElementType == TMP_TextElementType.Sprite)
				{
					float num18 = (this.m_width == -1f) ? (x + 0.0001f - this.m_marginLeft - this.m_marginRight) : Mathf.Min(x + 0.0001f - this.m_marginLeft - this.m_marginRight, this.m_width);
					if (this.m_xAdvance + this.m_cached_TextElement.xAdvance * num2 > num18 && this.enableWordWrapping && this.m_characterCount != this.m_firstCharacterOfLine)
					{
						if (num6 == wordWrapState2.previous_WordBreak || flag)
						{
							if (!this.m_isCharacterWrappingEnabled)
							{
								this.m_isCharacterWrappingEnabled = true;
							}
							else
							{
								flag2 = true;
							}
						}
						num8 = this.RestoreWordWrappingState(ref wordWrapState2);
						num6 = num8;
						if (this.m_lineNumber > 0 && !TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender) && this.m_lineHeight == 0f)
						{
							float num19 = this.m_maxLineAscender - this.m_startOfLineAscender;
							this.AdjustLineOffset(this.m_firstCharacterOfLine, this.m_characterCount, num19);
							this.m_lineOffset += num19;
							wordWrapState2.lineOffset = this.m_lineOffset;
							wordWrapState2.previousLineAscender = this.m_maxLineAscender;
						}
						float num20 = this.m_maxLineAscender - this.m_lineOffset;
						float num21 = this.m_maxLineDescender - this.m_lineOffset;
						this.m_maxDescender = ((this.m_maxDescender >= num21) ? num21 : this.m_maxDescender);
						this.m_firstCharacterOfLine = this.m_characterCount;
						num4 += this.m_xAdvance;
						if (this.m_enableWordWrapping)
						{
							num5 = this.m_maxAscender - this.m_maxDescender;
						}
						else
						{
							num5 = Mathf.Max(num5, num20 - num21);
						}
						this.SaveWordWrappingState(ref wordWrapState, num8, this.m_characterCount - 1);
						this.m_lineNumber++;
						if (this.m_lineHeight == 0f)
						{
							float num22 = this.m_internalCharacterInfo[this.m_characterCount].ascender - this.m_internalCharacterInfo[this.m_characterCount].baseLine;
							float num23 = 0f - this.m_maxLineDescender + num22 + (num3 + this.m_lineSpacing + this.m_lineSpacingDelta) * num;
							this.m_lineOffset += num23;
							this.m_startOfLineAscender = num22;
						}
						else
						{
							this.m_lineOffset += this.m_lineHeight + this.m_lineSpacing * num;
						}
						this.m_maxLineAscender = float.NegativeInfinity;
						this.m_maxLineDescender = float.PositiveInfinity;
						this.m_xAdvance = this.tag_Indent;
						goto IL_FE9;
					}
				}
				if (this.m_lineNumber > 0 && !TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender) && this.m_lineHeight == 0f && !this.m_isNewPage)
				{
					float num24 = this.m_maxLineAscender - this.m_startOfLineAscender;
					this.AdjustLineOffset(this.m_firstCharacterOfLine, this.m_characterCount, num24);
					num15 -= num24;
					this.m_lineOffset += num24;
					this.m_startOfLineAscender += num24;
					wordWrapState2.lineOffset = this.m_lineOffset;
					wordWrapState2.previousLineAscender = this.m_startOfLineAscender;
				}
				if (num9 == 9)
				{
					this.m_xAdvance += this.m_currentFontAsset.fontInfo.TabWidth * num2;
				}
				else if (this.m_monoSpacing != 0f)
				{
					this.m_xAdvance += this.m_monoSpacing - num11 + (this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2 + this.m_cSpacing;
				}
				else
				{
					this.m_xAdvance += (this.m_cached_TextElement.xAdvance * num12 + this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2 + this.m_cSpacing;
				}
				if (num9 == 13)
				{
					a = Mathf.Max(a, num4 + this.m_xAdvance);
					num4 = 0f;
					this.m_xAdvance = this.tag_Indent;
				}
				if (num9 == 10 || this.m_characterCount == totalCharacterCount - 1)
				{
					if (this.m_lineNumber > 0 && !TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender) && this.m_lineHeight == 0f)
					{
						float num25 = this.m_maxLineAscender - this.m_startOfLineAscender;
						this.AdjustLineOffset(this.m_firstCharacterOfLine, this.m_characterCount, num25);
						num15 -= num25;
						this.m_lineOffset += num25;
					}
					float num26 = this.m_maxLineDescender - this.m_lineOffset;
					this.m_maxDescender = ((this.m_maxDescender >= num26) ? num26 : this.m_maxDescender);
					this.m_firstCharacterOfLine = this.m_characterCount + 1;
					if (num9 == 10 && this.m_characterCount != totalCharacterCount - 1)
					{
						a = Mathf.Max(a, num4 + this.m_xAdvance);
						num4 = 0f;
					}
					else
					{
						num4 = Mathf.Max(a, num4 + this.m_xAdvance);
					}
					num5 = this.m_maxAscender - this.m_maxDescender;
					if (num9 == 10)
					{
						this.SaveWordWrappingState(ref wordWrapState, num8, this.m_characterCount);
						this.SaveWordWrappingState(ref wordWrapState2, num8, this.m_characterCount);
						this.m_lineNumber++;
						if (this.m_lineHeight == 0f)
						{
							float num23 = 0f - this.m_maxLineDescender + num13 + (num3 + this.m_lineSpacing + this.m_paragraphSpacing + this.m_lineSpacingDelta) * num;
							this.m_lineOffset += num23;
						}
						else
						{
							this.m_lineOffset += this.m_lineHeight + (this.m_lineSpacing + this.m_paragraphSpacing) * num;
						}
						this.m_maxLineAscender = float.NegativeInfinity;
						this.m_maxLineDescender = float.PositiveInfinity;
						this.m_startOfLineAscender = num13;
						this.m_xAdvance = this.tag_LineIndent + this.tag_Indent;
					}
				}
				if (this.m_enableWordWrapping || this.m_overflowMode == TextOverflowModes.Truncate || this.m_overflowMode == TextOverflowModes.Ellipsis)
				{
					if ((num9 == 9 || num9 == 32) && !this.m_isNonBreakingSpace)
					{
						this.SaveWordWrappingState(ref wordWrapState2, num8, this.m_characterCount);
						this.m_isCharacterWrappingEnabled = false;
						flag = false;
					}
					else if (num9 > 11904 && num9 < 40959)
					{
						if (!this.m_currentFontAsset.lineBreakingInfo.leadingCharacters.ContainsKey(num9) && this.m_characterCount < totalCharacterCount - 1 && !this.m_currentFontAsset.lineBreakingInfo.followingCharacters.ContainsKey((int)this.m_internalCharacterInfo[this.m_characterCount + 1].character))
						{
							this.SaveWordWrappingState(ref wordWrapState2, num8, this.m_characterCount);
							this.m_isCharacterWrappingEnabled = false;
							flag = false;
						}
					}
					else if (flag || this.m_isCharacterWrappingEnabled || flag2)
					{
						this.SaveWordWrappingState(ref wordWrapState2, num8, this.m_characterCount);
					}
				}
				this.m_characterCount++;
				goto IL_FE9;
			}
			this.m_isCharacterWrappingEnabled = false;
			num4 += ((this.m_margin.x <= 0f) ? 0f : this.m_margin.x);
			num4 += ((this.m_margin.z <= 0f) ? 0f : this.m_margin.z);
			num5 += ((this.m_margin.y <= 0f) ? 0f : this.m_margin.y);
			num5 += ((this.m_margin.w <= 0f) ? 0f : this.m_margin.w);
			return new Vector2(num4, num5);
		}

		// Token: 0x06005105 RID: 20741 RVA: 0x0027ED2A File Offset: 0x0027D12A
		protected virtual void AdjustLineOffset(int startIndex, int endIndex, float offset)
		{
		}

		// Token: 0x06005106 RID: 20742 RVA: 0x0027ED2C File Offset: 0x0027D12C
		protected void ResizeLineExtents(int size)
		{
			size = ((size <= 1024) ? Mathf.NextPowerOfTwo(size + 1) : (size + 256));
			TMP_LineInfo[] array = new TMP_LineInfo[size];
			for (int i = 0; i < size; i++)
			{
				if (i < this.m_textInfo.lineInfo.Length)
				{
					array[i] = this.m_textInfo.lineInfo[i];
				}
				else
				{
					array[i].lineExtents.min = TMP_Text.k_InfinityVectorPositive;
					array[i].lineExtents.max = TMP_Text.k_InfinityVectorNegative;
					array[i].ascender = TMP_Text.k_InfinityVectorNegative.x;
					array[i].descender = TMP_Text.k_InfinityVectorPositive.x;
				}
			}
			this.m_textInfo.lineInfo = array;
		}

		// Token: 0x06005107 RID: 20743 RVA: 0x0027EE11 File Offset: 0x0027D211
		public virtual TMP_TextInfo GetTextInfo(string text)
		{
			return null;
		}

		// Token: 0x06005108 RID: 20744 RVA: 0x0027EE14 File Offset: 0x0027D214
		protected virtual void ComputeMarginSize()
		{
		}

		// Token: 0x06005109 RID: 20745 RVA: 0x0027EE18 File Offset: 0x0027D218
		protected int GetArraySizes(int[] chars)
		{
			int num = 0;
			this.m_totalCharacterCount = 0;
			this.m_isUsingBold = false;
			this.m_isParsingText = false;
			int num2 = 0;
			while (chars[num2] != 0)
			{
				int num3 = chars[num2];
				if (this.m_isRichText && num3 == 60 && this.ValidateHtmlTag(chars, num2 + 1, out num))
				{
					num2 = num;
					if ((this.m_style & FontStyles.Bold) == FontStyles.Bold)
					{
						this.m_isUsingBold = true;
					}
				}
				else
				{
					if (!char.IsWhiteSpace((char)num3))
					{
					}
					this.m_totalCharacterCount++;
				}
				num2++;
			}
			return this.m_totalCharacterCount;
		}

		// Token: 0x0600510A RID: 20746 RVA: 0x0027EEB4 File Offset: 0x0027D2B4
		protected void SaveWordWrappingState(ref WordWrapState state, int index, int count)
		{
			state.currentFontAsset = this.m_currentFontAsset;
			state.currentSpriteAsset = this.m_currentSpriteAsset;
			state.currentMaterial = this.m_currentMaterial;
			state.currentMaterialIndex = this.m_currentMaterialIndex;
			state.previous_WordBreak = index;
			state.total_CharacterCount = count;
			state.visible_CharacterCount = this.m_visibleCharacterCount;
			state.visible_SpriteCount = this.m_visibleSpriteCount;
			state.visible_LinkCount = this.m_textInfo.linkCount;
			state.firstCharacterIndex = this.m_firstCharacterOfLine;
			state.firstVisibleCharacterIndex = this.m_firstVisibleCharacterOfLine;
			state.lastVisibleCharIndex = this.m_lastVisibleCharacterOfLine;
			state.fontStyle = this.m_style;
			state.fontScale = this.m_fontScale;
			state.fontScaleMultiplier = this.m_fontScaleMultiplier;
			state.currentFontSize = this.m_currentFontSize;
			state.xAdvance = this.m_xAdvance;
			state.maxAscender = this.m_maxAscender;
			state.maxDescender = this.m_maxDescender;
			state.maxLineAscender = this.m_maxLineAscender;
			state.maxLineDescender = this.m_maxLineDescender;
			state.previousLineAscender = this.m_startOfLineAscender;
			state.preferredWidth = this.m_preferredWidth;
			state.preferredHeight = this.m_preferredHeight;
			state.meshExtents = this.m_meshExtents;
			state.lineNumber = this.m_lineNumber;
			state.lineOffset = this.m_lineOffset;
			state.baselineOffset = this.m_baselineOffset;
			state.vertexColor = this.m_htmlColor;
			state.tagNoParsing = this.tag_NoParsing;
			state.colorStack = this.m_colorStack;
			state.sizeStack = this.m_sizeStack;
			state.fontWeightStack = this.m_fontWeightStack;
			state.styleStack = this.m_styleStack;
			state.actionStack = this.m_actionStack;
			state.materialReferenceStack = this.m_materialReferenceStack;
			if (this.m_lineNumber < this.m_textInfo.lineInfo.Length)
			{
				state.lineInfo = this.m_textInfo.lineInfo[this.m_lineNumber];
			}
		}

		// Token: 0x0600510B RID: 20747 RVA: 0x0027F0A8 File Offset: 0x0027D4A8
		protected int RestoreWordWrappingState(ref WordWrapState state)
		{
			int previous_WordBreak = state.previous_WordBreak;
			this.m_currentFontAsset = state.currentFontAsset;
			this.m_currentSpriteAsset = state.currentSpriteAsset;
			this.m_currentMaterial = state.currentMaterial;
			this.m_currentMaterialIndex = state.currentMaterialIndex;
			this.m_characterCount = state.total_CharacterCount + 1;
			this.m_visibleCharacterCount = state.visible_CharacterCount;
			this.m_visibleSpriteCount = state.visible_SpriteCount;
			this.m_textInfo.linkCount = state.visible_LinkCount;
			this.m_firstCharacterOfLine = state.firstCharacterIndex;
			this.m_firstVisibleCharacterOfLine = state.firstVisibleCharacterIndex;
			this.m_lastVisibleCharacterOfLine = state.lastVisibleCharIndex;
			this.m_style = state.fontStyle;
			this.m_fontScale = state.fontScale;
			this.m_fontScaleMultiplier = state.fontScaleMultiplier;
			this.m_currentFontSize = state.currentFontSize;
			this.m_xAdvance = state.xAdvance;
			this.m_maxAscender = state.maxAscender;
			this.m_maxDescender = state.maxDescender;
			this.m_maxLineAscender = state.maxLineAscender;
			this.m_maxLineDescender = state.maxLineDescender;
			this.m_startOfLineAscender = state.previousLineAscender;
			this.m_preferredWidth = state.preferredWidth;
			this.m_preferredHeight = state.preferredHeight;
			this.m_meshExtents = state.meshExtents;
			this.m_lineNumber = state.lineNumber;
			this.m_lineOffset = state.lineOffset;
			this.m_baselineOffset = state.baselineOffset;
			this.m_htmlColor = state.vertexColor;
			this.tag_NoParsing = state.tagNoParsing;
			this.m_colorStack = state.colorStack;
			this.m_sizeStack = state.sizeStack;
			this.m_fontWeightStack = state.fontWeightStack;
			this.m_styleStack = state.styleStack;
			this.m_actionStack = state.actionStack;
			this.m_materialReferenceStack = state.materialReferenceStack;
			if (this.m_lineNumber < this.m_textInfo.lineInfo.Length)
			{
				this.m_textInfo.lineInfo[this.m_lineNumber] = state.lineInfo;
			}
			return previous_WordBreak;
		}

		// Token: 0x0600510C RID: 20748 RVA: 0x0027F2A4 File Offset: 0x0027D6A4
		protected virtual void SaveGlyphVertexInfo(float padding, float style_padding, Color32 vertexColor)
		{
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.position = this.m_textInfo.characterInfo[this.m_characterCount].bottomLeft;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.position = this.m_textInfo.characterInfo[this.m_characterCount].topLeft;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.position = this.m_textInfo.characterInfo[this.m_characterCount].topRight;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.position = this.m_textInfo.characterInfo[this.m_characterCount].bottomRight;
			vertexColor.a = ((this.m_fontColor32.a >= vertexColor.a) ? vertexColor.a : this.m_fontColor32.a);
			if (!this.m_enableVertexGradient)
			{
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = vertexColor;
			}
			else if (!this.m_overrideHtmlColors && !this.m_htmlColor.CompareRGB(this.m_fontColor32))
			{
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = vertexColor;
			}
			else
			{
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = this.m_fontColorGradient.bottomLeft * vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = this.m_fontColorGradient.topLeft * vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = this.m_fontColorGradient.topRight * vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = this.m_fontColorGradient.bottomRight * vertexColor;
			}
			if (!this.m_isSDFShader)
			{
				style_padding = 0f;
			}
			FaceInfo fontInfo = this.m_currentFontAsset.fontInfo;
			Vector2 uv;
			uv.x = (this.m_cached_TextElement.x - padding - style_padding) / fontInfo.AtlasWidth;
			uv.y = 1f - (this.m_cached_TextElement.y + padding + style_padding + this.m_cached_TextElement.height) / fontInfo.AtlasHeight;
			Vector2 uv2;
			uv2.x = uv.x;
			uv2.y = 1f - (this.m_cached_TextElement.y - padding - style_padding) / fontInfo.AtlasHeight;
			Vector2 uv3;
			uv3.x = (this.m_cached_TextElement.x + padding + style_padding + this.m_cached_TextElement.width) / fontInfo.AtlasWidth;
			uv3.y = uv2.y;
			Vector2 uv4;
			uv4.x = uv3.x;
			uv4.y = uv.y;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.uv = uv;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.uv = uv2;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.uv = uv3;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.uv = uv4;
		}

		// Token: 0x0600510D RID: 20749 RVA: 0x0027F770 File Offset: 0x0027DB70
		protected virtual void SaveSpriteVertexInfo(Color32 vertexColor)
		{
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.position = this.m_textInfo.characterInfo[this.m_characterCount].bottomLeft;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.position = this.m_textInfo.characterInfo[this.m_characterCount].topLeft;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.position = this.m_textInfo.characterInfo[this.m_characterCount].topRight;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.position = this.m_textInfo.characterInfo[this.m_characterCount].bottomRight;
			if (this.m_tintAllSprites)
			{
				this.m_tintSprite = true;
			}
			Color32 color = (!this.m_tintSprite) ? this.m_spriteColor : this.m_spriteColor.Multiply(vertexColor);
			color.a = ((color.a >= this.m_fontColor32.a) ? this.m_fontColor32.a : (color.a = ((color.a >= vertexColor.a) ? vertexColor.a : color.a)));
			if (!this.m_enableVertexGradient)
			{
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = color;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = color;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = color;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = color;
			}
			else if (!this.m_overrideHtmlColors && !this.m_htmlColor.CompareRGB(this.m_fontColor32))
			{
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = color;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = color;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = color;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = color;
			}
			else
			{
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = ((!this.m_tintSprite) ? color : color.Multiply(this.m_fontColorGradient.bottomLeft));
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = ((!this.m_tintSprite) ? color : color.Multiply(this.m_fontColorGradient.topLeft));
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = ((!this.m_tintSprite) ? color : color.Multiply(this.m_fontColorGradient.topRight));
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = ((!this.m_tintSprite) ? color : color.Multiply(this.m_fontColorGradient.bottomRight));
			}
			Vector2 uv = new Vector2(this.m_cached_TextElement.x / (float)this.m_currentSpriteAsset.spriteSheet.width, this.m_cached_TextElement.y / (float)this.m_currentSpriteAsset.spriteSheet.height);
			Vector2 uv2 = new Vector2(uv.x, (this.m_cached_TextElement.y + this.m_cached_TextElement.height) / (float)this.m_currentSpriteAsset.spriteSheet.height);
			Vector2 uv3 = new Vector2((this.m_cached_TextElement.x + this.m_cached_TextElement.width) / (float)this.m_currentSpriteAsset.spriteSheet.width, uv2.y);
			Vector2 uv4 = new Vector2(uv3.x, uv.y);
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.uv = uv;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.uv = uv2;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.uv = uv3;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.uv = uv4;
		}

		// Token: 0x0600510E RID: 20750 RVA: 0x0027FCA0 File Offset: 0x0027E0A0
		protected virtual void FillCharacterVertexBuffers(int i, int index_X4)
		{
			int materialReferenceIndex = this.m_textInfo.characterInfo[i].materialReferenceIndex;
			index_X4 = this.m_textInfo.meshInfo[materialReferenceIndex].vertexCount;
			TMP_CharacterInfo[] characterInfo = this.m_textInfo.characterInfo;
			this.m_textInfo.characterInfo[i].vertexIndex = (short)index_X4;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[index_X4] = characterInfo[i].vertex_BL.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[1 + index_X4] = characterInfo[i].vertex_TL.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[2 + index_X4] = characterInfo[i].vertex_TR.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[3 + index_X4] = characterInfo[i].vertex_BR.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[index_X4] = characterInfo[i].vertex_BL.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[1 + index_X4] = characterInfo[i].vertex_TL.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[2 + index_X4] = characterInfo[i].vertex_TR.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[3 + index_X4] = characterInfo[i].vertex_BR.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[index_X4] = characterInfo[i].vertex_BL.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[1 + index_X4] = characterInfo[i].vertex_TL.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[2 + index_X4] = characterInfo[i].vertex_TR.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[3 + index_X4] = characterInfo[i].vertex_BR.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[index_X4] = characterInfo[i].vertex_BL.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[1 + index_X4] = characterInfo[i].vertex_TL.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[2 + index_X4] = characterInfo[i].vertex_TR.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[3 + index_X4] = characterInfo[i].vertex_BR.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertexCount = index_X4 + 4;
		}

		// Token: 0x0600510F RID: 20751 RVA: 0x00280054 File Offset: 0x0027E454
		protected virtual void FillSpriteVertexBuffers(int i, int index_X4)
		{
			int materialReferenceIndex = this.m_textInfo.characterInfo[i].materialReferenceIndex;
			index_X4 = this.m_textInfo.meshInfo[materialReferenceIndex].vertexCount;
			TMP_CharacterInfo[] characterInfo = this.m_textInfo.characterInfo;
			this.m_textInfo.characterInfo[i].vertexIndex = (short)index_X4;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[index_X4] = characterInfo[i].vertex_BL.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[1 + index_X4] = characterInfo[i].vertex_TL.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[2 + index_X4] = characterInfo[i].vertex_TR.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[3 + index_X4] = characterInfo[i].vertex_BR.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[index_X4] = characterInfo[i].vertex_BL.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[1 + index_X4] = characterInfo[i].vertex_TL.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[2 + index_X4] = characterInfo[i].vertex_TR.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[3 + index_X4] = characterInfo[i].vertex_BR.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[index_X4] = characterInfo[i].vertex_BL.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[1 + index_X4] = characterInfo[i].vertex_TL.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[2 + index_X4] = characterInfo[i].vertex_TR.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[3 + index_X4] = characterInfo[i].vertex_BR.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[index_X4] = characterInfo[i].vertex_BL.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[1 + index_X4] = characterInfo[i].vertex_TL.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[2 + index_X4] = characterInfo[i].vertex_TR.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[3 + index_X4] = characterInfo[i].vertex_BR.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertexCount = index_X4 + 4;
		}

		// Token: 0x06005110 RID: 20752 RVA: 0x00280408 File Offset: 0x0027E808
		protected virtual void DrawUnderlineMesh(Vector3 start, Vector3 end, ref int index, float startScale, float endScale, float maxScale, Color32 underlineColor)
		{
			if (this.m_cached_Underline_GlyphInfo == null)
			{
				if (!TMP_Settings.warningsDisabled)
				{
				}
				return;
			}
			int num = index + 12;
			if (num > this.m_textInfo.meshInfo[0].vertices.Length)
			{
				this.m_textInfo.meshInfo[0].ResizeMeshInfo(num / 4);
			}
			start.y = Mathf.Min(start.y, end.y);
			end.y = Mathf.Min(start.y, end.y);
			float num2 = this.m_cached_Underline_GlyphInfo.width / 2f * maxScale;
			if (end.x - start.x < this.m_cached_Underline_GlyphInfo.width * maxScale)
			{
				num2 = (end.x - start.x) / 2f;
			}
			float num3 = this.m_padding * startScale / maxScale;
			float num4 = this.m_padding * endScale / maxScale;
			float height = this.m_cached_Underline_GlyphInfo.height;
			Vector3[] vertices = this.m_textInfo.meshInfo[0].vertices;
			vertices[index] = start + new Vector3(0f, 0f - (height + this.m_padding) * maxScale, 0f);
			vertices[index + 1] = start + new Vector3(0f, this.m_padding * maxScale, 0f);
			vertices[index + 2] = vertices[index + 1] + new Vector3(num2, 0f, 0f);
			vertices[index + 3] = vertices[index] + new Vector3(num2, 0f, 0f);
			vertices[index + 4] = vertices[index + 3];
			vertices[index + 5] = vertices[index + 2];
			vertices[index + 6] = end + new Vector3(-num2, this.m_padding * maxScale, 0f);
			vertices[index + 7] = end + new Vector3(-num2, -(height + this.m_padding) * maxScale, 0f);
			vertices[index + 8] = vertices[index + 7];
			vertices[index + 9] = vertices[index + 6];
			vertices[index + 10] = end + new Vector3(0f, this.m_padding * maxScale, 0f);
			vertices[index + 11] = end + new Vector3(0f, -(height + this.m_padding) * maxScale, 0f);
			Vector2[] uvs = this.m_textInfo.meshInfo[0].uvs0;
			Vector2 vector = new Vector2((this.m_cached_Underline_GlyphInfo.x - num3) / this.m_fontAsset.fontInfo.AtlasWidth, 1f - (this.m_cached_Underline_GlyphInfo.y + this.m_padding + this.m_cached_Underline_GlyphInfo.height) / this.m_fontAsset.fontInfo.AtlasHeight);
			Vector2 vector2 = new Vector2(vector.x, 1f - (this.m_cached_Underline_GlyphInfo.y - this.m_padding) / this.m_fontAsset.fontInfo.AtlasHeight);
			Vector2 vector3 = new Vector2((this.m_cached_Underline_GlyphInfo.x - num3 + this.m_cached_Underline_GlyphInfo.width / 2f) / this.m_fontAsset.fontInfo.AtlasWidth, vector2.y);
			Vector2 vector4 = new Vector2(vector3.x, vector.y);
			Vector2 vector5 = new Vector2((this.m_cached_Underline_GlyphInfo.x + num4 + this.m_cached_Underline_GlyphInfo.width / 2f) / this.m_fontAsset.fontInfo.AtlasWidth, vector2.y);
			Vector2 vector6 = new Vector2(vector5.x, vector.y);
			Vector2 vector7 = new Vector2((this.m_cached_Underline_GlyphInfo.x + num4 + this.m_cached_Underline_GlyphInfo.width) / this.m_fontAsset.fontInfo.AtlasWidth, vector2.y);
			Vector2 vector8 = new Vector2(vector7.x, vector.y);
			uvs[index] = vector;
			uvs[1 + index] = vector2;
			uvs[2 + index] = vector3;
			uvs[3 + index] = vector4;
			uvs[4 + index] = new Vector2(vector3.x - vector3.x * 0.001f, vector.y);
			uvs[5 + index] = new Vector2(vector3.x - vector3.x * 0.001f, vector2.y);
			uvs[6 + index] = new Vector2(vector3.x + vector3.x * 0.001f, vector2.y);
			uvs[7 + index] = new Vector2(vector3.x + vector3.x * 0.001f, vector.y);
			uvs[8 + index] = vector6;
			uvs[9 + index] = vector5;
			uvs[10 + index] = vector7;
			uvs[11 + index] = vector8;
			float x = (vertices[index + 2].x - start.x) / (end.x - start.x);
			float num5 = (maxScale * this.m_rectTransform.lossyScale.y != 0f) ? this.m_rectTransform.lossyScale.y : 1f;
			float scale = num5;
			Vector2[] uvs2 = this.m_textInfo.meshInfo[0].uvs2;
			uvs2[index] = this.PackUV(0f, 0f, num5);
			uvs2[1 + index] = this.PackUV(0f, 1f, num5);
			uvs2[2 + index] = this.PackUV(x, 1f, num5);
			uvs2[3 + index] = this.PackUV(x, 0f, num5);
			float x2 = (vertices[index + 4].x - start.x) / (end.x - start.x);
			x = (vertices[index + 6].x - start.x) / (end.x - start.x);
			uvs2[4 + index] = this.PackUV(x2, 0f, scale);
			uvs2[5 + index] = this.PackUV(x2, 1f, scale);
			uvs2[6 + index] = this.PackUV(x, 1f, scale);
			uvs2[7 + index] = this.PackUV(x, 0f, scale);
			x2 = (vertices[index + 8].x - start.x) / (end.x - start.x);
			x = (vertices[index + 6].x - start.x) / (end.x - start.x);
			uvs2[8 + index] = this.PackUV(x2, 0f, num5);
			uvs2[9 + index] = this.PackUV(x2, 1f, num5);
			uvs2[10 + index] = this.PackUV(1f, 1f, num5);
			uvs2[11 + index] = this.PackUV(1f, 0f, num5);
			Color32[] colors = this.m_textInfo.meshInfo[0].colors32;
			colors[index] = underlineColor;
			colors[1 + index] = underlineColor;
			colors[2 + index] = underlineColor;
			colors[3 + index] = underlineColor;
			colors[4 + index] = underlineColor;
			colors[5 + index] = underlineColor;
			colors[6 + index] = underlineColor;
			colors[7 + index] = underlineColor;
			colors[8 + index] = underlineColor;
			colors[9 + index] = underlineColor;
			colors[10 + index] = underlineColor;
			colors[11 + index] = underlineColor;
			index += 12;
		}

		// Token: 0x06005111 RID: 20753 RVA: 0x00280DD0 File Offset: 0x0027F1D0
		protected void GetSpecialCharacters(TMP_FontAsset fontAsset)
		{
			if (fontAsset.characterDictionary.TryGetValue(95, out this.m_cached_Underline_GlyphInfo) || !TMP_Settings.warningsDisabled)
			{
			}
			if (fontAsset.characterDictionary.TryGetValue(8230, out this.m_cached_Ellipsis_GlyphInfo) || !TMP_Settings.warningsDisabled)
			{
			}
		}

		// Token: 0x06005112 RID: 20754 RVA: 0x00280E24 File Offset: 0x0027F224
		protected int GetMaterialReferenceForFontWeight()
		{
			this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentFontAsset.fontWeights[0].italicTypeface.material, this.m_currentFontAsset.fontWeights[0].italicTypeface, this.m_materialReferences, this.m_materialReferenceIndexLookup);
			return 0;
		}

		// Token: 0x06005113 RID: 20755 RVA: 0x00280E74 File Offset: 0x0027F274
		protected TMP_FontAsset GetAlternativeFontAsset()
		{
			bool flag = (this.m_style & FontStyles.Italic) == FontStyles.Italic || (this.m_fontStyle & FontStyles.Italic) == FontStyles.Italic;
			int num = this.m_fontWeightInternal / 100;
			TMP_FontAsset tmp_FontAsset;
			if (flag)
			{
				tmp_FontAsset = this.m_currentFontAsset.fontWeights[num].italicTypeface;
			}
			else
			{
				tmp_FontAsset = this.m_currentFontAsset.fontWeights[num].regularTypeface;
			}
			if (tmp_FontAsset == null)
			{
				return this.m_currentFontAsset;
			}
			this.m_currentFontAsset = tmp_FontAsset;
			return this.m_currentFontAsset;
		}

		// Token: 0x06005114 RID: 20756 RVA: 0x00280EFC File Offset: 0x0027F2FC
		protected Vector2 PackUV(float x, float y, float scale)
		{
			Vector2 result;
			result.x = Mathf.Floor(x * 511f);
			result.y = Mathf.Floor(y * 511f);
			result.x = result.x * 4096f + result.y;
			result.y = scale;
			return result;
		}

		// Token: 0x06005115 RID: 20757 RVA: 0x00280F54 File Offset: 0x0027F354
		protected float PackUV(float x, float y)
		{
			double num = Math.Floor((double)(x * 511f));
			double num2 = Math.Floor((double)(y * 511f));
			return (float)(num * 4096.0 + num2);
		}

		// Token: 0x06005116 RID: 20758 RVA: 0x00280F8C File Offset: 0x0027F38C
		protected int HexToInt(char hex)
		{
			switch (hex)
			{
			case '0':
				return 0;
			case '1':
				return 1;
			case '2':
				return 2;
			case '3':
				return 3;
			case '4':
				return 4;
			case '5':
				return 5;
			case '6':
				return 6;
			case '7':
				return 7;
			case '8':
				return 8;
			case '9':
				return 9;
			default:
				switch (hex)
				{
				case 'a':
					return 10;
				case 'b':
					return 11;
				case 'c':
					return 12;
				case 'd':
					return 13;
				case 'e':
					return 14;
				case 'f':
					return 15;
				default:
					return 15;
				}
				break;
			case 'A':
				return 10;
			case 'B':
				return 11;
			case 'C':
				return 12;
			case 'D':
				return 13;
			case 'E':
				return 14;
			case 'F':
				return 15;
			}
		}

		// Token: 0x06005117 RID: 20759 RVA: 0x00281060 File Offset: 0x0027F460
		protected int GetUTF16(int i)
		{
			int num = this.HexToInt(this.m_text[i]) * 4096;
			num += this.HexToInt(this.m_text[i + 1]) * 256;
			num += this.HexToInt(this.m_text[i + 2]) * 16;
			return num + this.HexToInt(this.m_text[i + 3]);
		}

		// Token: 0x06005118 RID: 20760 RVA: 0x002810D8 File Offset: 0x0027F4D8
		protected int GetUTF32(int i)
		{
			int num = 0;
			num += this.HexToInt(this.m_text[i]) * 268435456;
			num += this.HexToInt(this.m_text[i + 1]) * 16777216;
			num += this.HexToInt(this.m_text[i + 2]) * 1048576;
			num += this.HexToInt(this.m_text[i + 3]) * 65536;
			num += this.HexToInt(this.m_text[i + 4]) * 4096;
			num += this.HexToInt(this.m_text[i + 5]) * 256;
			num += this.HexToInt(this.m_text[i + 6]) * 16;
			return num + this.HexToInt(this.m_text[i + 7]);
		}

		// Token: 0x06005119 RID: 20761 RVA: 0x002811C8 File Offset: 0x0027F5C8
		protected Color32 HexCharsToColor(char[] hexChars, int tagCount)
		{
			if (tagCount == 7)
			{
				byte r = (byte)(this.HexToInt(hexChars[1]) * 16 + this.HexToInt(hexChars[2]));
				byte g = (byte)(this.HexToInt(hexChars[3]) * 16 + this.HexToInt(hexChars[4]));
				byte b = (byte)(this.HexToInt(hexChars[5]) * 16 + this.HexToInt(hexChars[6]));
				return new Color32(r, g, b, byte.MaxValue);
			}
			if (tagCount == 9)
			{
				byte r2 = (byte)(this.HexToInt(hexChars[1]) * 16 + this.HexToInt(hexChars[2]));
				byte g2 = (byte)(this.HexToInt(hexChars[3]) * 16 + this.HexToInt(hexChars[4]));
				byte b2 = (byte)(this.HexToInt(hexChars[5]) * 16 + this.HexToInt(hexChars[6]));
				byte a = (byte)(this.HexToInt(hexChars[7]) * 16 + this.HexToInt(hexChars[8]));
				return new Color32(r2, g2, b2, a);
			}
			if (tagCount == 13)
			{
				byte r3 = (byte)(this.HexToInt(hexChars[7]) * 16 + this.HexToInt(hexChars[8]));
				byte g3 = (byte)(this.HexToInt(hexChars[9]) * 16 + this.HexToInt(hexChars[10]));
				byte b3 = (byte)(this.HexToInt(hexChars[11]) * 16 + this.HexToInt(hexChars[12]));
				return new Color32(r3, g3, b3, byte.MaxValue);
			}
			if (tagCount == 15)
			{
				byte r4 = (byte)(this.HexToInt(hexChars[7]) * 16 + this.HexToInt(hexChars[8]));
				byte g4 = (byte)(this.HexToInt(hexChars[9]) * 16 + this.HexToInt(hexChars[10]));
				byte b4 = (byte)(this.HexToInt(hexChars[11]) * 16 + this.HexToInt(hexChars[12]));
				byte a2 = (byte)(this.HexToInt(hexChars[13]) * 16 + this.HexToInt(hexChars[14]));
				return new Color32(r4, g4, b4, a2);
			}
			return new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}

		// Token: 0x0600511A RID: 20762 RVA: 0x002813AC File Offset: 0x0027F7AC
		protected Color32 HexCharsToColor(char[] hexChars, int startIndex, int length)
		{
			if (length == 7)
			{
				byte r = (byte)(this.HexToInt(hexChars[startIndex + 1]) * 16 + this.HexToInt(hexChars[startIndex + 2]));
				byte g = (byte)(this.HexToInt(hexChars[startIndex + 3]) * 16 + this.HexToInt(hexChars[startIndex + 4]));
				byte b = (byte)(this.HexToInt(hexChars[startIndex + 5]) * 16 + this.HexToInt(hexChars[startIndex + 6]));
				return new Color32(r, g, b, byte.MaxValue);
			}
			if (length == 9)
			{
				byte r2 = (byte)(this.HexToInt(hexChars[startIndex + 1]) * 16 + this.HexToInt(hexChars[startIndex + 2]));
				byte g2 = (byte)(this.HexToInt(hexChars[startIndex + 3]) * 16 + this.HexToInt(hexChars[startIndex + 4]));
				byte b2 = (byte)(this.HexToInt(hexChars[startIndex + 5]) * 16 + this.HexToInt(hexChars[startIndex + 6]));
				byte a = (byte)(this.HexToInt(hexChars[startIndex + 7]) * 16 + this.HexToInt(hexChars[startIndex + 8]));
				return new Color32(r2, g2, b2, a);
			}
			return new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}

		// Token: 0x0600511B RID: 20763 RVA: 0x002814C4 File Offset: 0x0027F8C4
		protected float ConvertToFloat(char[] chars, int startIndex, int length, int decimalPointIndex)
		{
			if (startIndex == 0)
			{
				return -9999f;
			}
			int num = startIndex + length - 1;
			float num2 = 0f;
			float num3 = 1f;
			decimalPointIndex = ((decimalPointIndex <= 0) ? (num + 1) : decimalPointIndex);
			if (chars[startIndex] == '-')
			{
				startIndex++;
				num3 = -1f;
			}
			if (chars[startIndex] == '+' || chars[startIndex] == '%')
			{
				startIndex++;
			}
			for (int i = startIndex; i < num + 1; i++)
			{
				if (!char.IsDigit(chars[i]) && chars[i] != '.')
				{
					return -9999f;
				}
				int num4 = decimalPointIndex - i;
				switch (num4 + 3)
				{
				case 0:
					num2 += (float)(chars[i] - '0') * 0.001f;
					break;
				case 1:
					num2 += (float)(chars[i] - '0') * 0.01f;
					break;
				case 2:
					num2 += (float)(chars[i] - '0') * 0.1f;
					break;
				case 4:
					num2 += (float)(chars[i] - '0');
					break;
				case 5:
					num2 += (float)((chars[i] - '0') * '\n');
					break;
				case 6:
					num2 += (float)((chars[i] - '0') * 'd');
					break;
				case 7:
					num2 += (float)((chars[i] - '0') * 'Ϩ');
					break;
				}
			}
			return num2 * num3;
		}

		// Token: 0x0600511C RID: 20764 RVA: 0x00281620 File Offset: 0x0027FA20
		protected bool ValidateHtmlTag(int[] chars, int startIndex, out int endIndex)
		{
			int num = 0;
			byte b = 0;
			TagUnits tagUnits = TagUnits.Pixels;
			TagType tagType = TagType.None;
			int num2 = 0;
			this.m_xmlAttribute[num2].nameHashCode = 0;
			this.m_xmlAttribute[num2].valueType = TagType.None;
			this.m_xmlAttribute[num2].valueHashCode = 0;
			this.m_xmlAttribute[num2].valueStartIndex = 0;
			this.m_xmlAttribute[num2].valueLength = 0;
			this.m_xmlAttribute[num2].valueDecimalIndex = 0;
			endIndex = startIndex;
			bool flag = false;
			bool flag2 = false;
			int num3 = startIndex;
			while (num3 < chars.Length && chars[num3] != 0 && num < this.m_htmlTag.Length && chars[num3] != 60)
			{
				if (chars[num3] == 62)
				{
					flag2 = true;
					endIndex = num3;
					this.m_htmlTag[num] = '\0';
					break;
				}
				this.m_htmlTag[num] = (char)chars[num3];
				num++;
				if (b == 1)
				{
					if (this.m_xmlAttribute[num2].valueStartIndex == 0)
					{
						if (chars[num3] == 43 || chars[num3] == 45 || char.IsDigit((char)chars[num3]))
						{
							tagType = TagType.NumericalValue;
							this.m_xmlAttribute[num2].valueType = TagType.NumericalValue;
							this.m_xmlAttribute[num2].valueStartIndex = num - 1;
							XML_TagAttribute[] xmlAttribute = this.m_xmlAttribute;
							int num4 = num2;
							xmlAttribute[num4].valueLength = xmlAttribute[num4].valueLength + 1;
						}
						else if (chars[num3] == 35)
						{
							tagType = TagType.ColorValue;
							this.m_xmlAttribute[num2].valueType = TagType.ColorValue;
							this.m_xmlAttribute[num2].valueStartIndex = num - 1;
							XML_TagAttribute[] xmlAttribute2 = this.m_xmlAttribute;
							int num5 = num2;
							xmlAttribute2[num5].valueLength = xmlAttribute2[num5].valueLength + 1;
						}
						else if (chars[num3] != 34)
						{
							tagType = TagType.StringValue;
							this.m_xmlAttribute[num2].valueType = TagType.StringValue;
							this.m_xmlAttribute[num2].valueStartIndex = num - 1;
							this.m_xmlAttribute[num2].valueHashCode = ((this.m_xmlAttribute[num2].valueHashCode << 5) + this.m_xmlAttribute[num2].valueHashCode ^ chars[num3]);
							XML_TagAttribute[] xmlAttribute3 = this.m_xmlAttribute;
							int num6 = num2;
							xmlAttribute3[num6].valueLength = xmlAttribute3[num6].valueLength + 1;
						}
					}
					else if (tagType == TagType.NumericalValue)
					{
						if (chars[num3] == 46)
						{
							this.m_xmlAttribute[num2].valueDecimalIndex = num - 1;
						}
						if (chars[num3] == 112 || chars[num3] == 101 || chars[num3] == 37 || chars[num3] == 32)
						{
							b = 2;
							tagType = TagType.None;
							num2++;
							this.m_xmlAttribute[num2].nameHashCode = 0;
							this.m_xmlAttribute[num2].valueType = TagType.None;
							this.m_xmlAttribute[num2].valueHashCode = 0;
							this.m_xmlAttribute[num2].valueStartIndex = 0;
							this.m_xmlAttribute[num2].valueLength = 0;
							this.m_xmlAttribute[num2].valueDecimalIndex = 0;
							if (chars[num3] == 101)
							{
								tagUnits = TagUnits.FontUnits;
							}
							else if (chars[num3] == 37)
							{
								tagUnits = TagUnits.Percentage;
							}
						}
						else if (b != 2)
						{
							XML_TagAttribute[] xmlAttribute4 = this.m_xmlAttribute;
							int num7 = num2;
							xmlAttribute4[num7].valueLength = xmlAttribute4[num7].valueLength + 1;
						}
					}
					else if (tagType == TagType.ColorValue)
					{
						if (chars[num3] != 32)
						{
							XML_TagAttribute[] xmlAttribute5 = this.m_xmlAttribute;
							int num8 = num2;
							xmlAttribute5[num8].valueLength = xmlAttribute5[num8].valueLength + 1;
						}
						else
						{
							b = 2;
							tagType = TagType.None;
							num2++;
							this.m_xmlAttribute[num2].nameHashCode = 0;
							this.m_xmlAttribute[num2].valueType = TagType.None;
							this.m_xmlAttribute[num2].valueHashCode = 0;
							this.m_xmlAttribute[num2].valueStartIndex = 0;
							this.m_xmlAttribute[num2].valueLength = 0;
							this.m_xmlAttribute[num2].valueDecimalIndex = 0;
						}
					}
					else if (tagType == TagType.StringValue)
					{
						if (chars[num3] != 34)
						{
							this.m_xmlAttribute[num2].valueHashCode = ((this.m_xmlAttribute[num2].valueHashCode << 5) + this.m_xmlAttribute[num2].valueHashCode ^ chars[num3]);
							XML_TagAttribute[] xmlAttribute6 = this.m_xmlAttribute;
							int num9 = num2;
							xmlAttribute6[num9].valueLength = xmlAttribute6[num9].valueLength + 1;
						}
						else
						{
							b = 2;
							tagType = TagType.None;
							num2++;
							this.m_xmlAttribute[num2].nameHashCode = 0;
							this.m_xmlAttribute[num2].valueType = TagType.None;
							this.m_xmlAttribute[num2].valueHashCode = 0;
							this.m_xmlAttribute[num2].valueStartIndex = 0;
							this.m_xmlAttribute[num2].valueLength = 0;
							this.m_xmlAttribute[num2].valueDecimalIndex = 0;
						}
					}
				}
				if (chars[num3] == 61)
				{
					b = 1;
				}
				if (b == 0 && chars[num3] == 32)
				{
					if (flag)
					{
						return false;
					}
					flag = true;
					b = 2;
					tagType = TagType.None;
					num2++;
					this.m_xmlAttribute[num2].nameHashCode = 0;
					this.m_xmlAttribute[num2].valueType = TagType.None;
					this.m_xmlAttribute[num2].valueHashCode = 0;
					this.m_xmlAttribute[num2].valueStartIndex = 0;
					this.m_xmlAttribute[num2].valueLength = 0;
					this.m_xmlAttribute[num2].valueDecimalIndex = 0;
				}
				if (b == 0)
				{
					this.m_xmlAttribute[num2].nameHashCode = (this.m_xmlAttribute[num2].nameHashCode << 3) - this.m_xmlAttribute[num2].nameHashCode + chars[num3];
				}
				if (b == 2 && chars[num3] == 32)
				{
					b = 0;
				}
				num3++;
			}
			if (!flag2)
			{
				return false;
			}
			if (this.tag_NoParsing && this.m_xmlAttribute[0].nameHashCode != 53822163)
			{
				return false;
			}
			if (this.m_xmlAttribute[0].nameHashCode == 53822163)
			{
				this.tag_NoParsing = false;
				return true;
			}
			if (this.m_htmlTag[0] == '#' && num == 7)
			{
				this.m_htmlColor = this.HexCharsToColor(this.m_htmlTag, num);
				this.m_colorStack.Add(this.m_htmlColor);
				return true;
			}
			if (this.m_htmlTag[0] == '#' && num == 9)
			{
				this.m_htmlColor = this.HexCharsToColor(this.m_htmlTag, num);
				this.m_colorStack.Add(this.m_htmlColor);
				return true;
			}
			int nameHashCode = this.m_xmlAttribute[0].nameHashCode;
			switch (nameHashCode)
			{
			case 115:
				this.m_style |= FontStyles.Strikethrough;
				return true;
			default:
				if (nameHashCode == 426)
				{
					return true;
				}
				if (nameHashCode == 427)
				{
					if ((this.m_fontStyle & FontStyles.Bold) != FontStyles.Bold)
					{
						this.m_style &= (FontStyles)(-2);
						this.m_fontWeightInternal = this.m_fontWeightStack.Remove();
					}
					return true;
				}
				switch (nameHashCode)
				{
				case 444:
					if ((this.m_fontStyle & FontStyles.Strikethrough) != FontStyles.Strikethrough)
					{
						this.m_style &= (FontStyles)(-65);
					}
					return true;
				default:
					if (nameHashCode != 13526026)
					{
						if (nameHashCode == 730022849)
						{
							this.m_style |= FontStyles.LowerCase;
							return true;
						}
						if (nameHashCode == 766244328)
						{
							this.m_style |= FontStyles.SmallCaps;
							return true;
						}
						if (nameHashCode != 781906058)
						{
							if (nameHashCode != 1100728678)
							{
								if (nameHashCode != 1109349752)
								{
									if (nameHashCode != 1109386397)
									{
										if (nameHashCode == -1885698441)
										{
											this.m_fontWeightInternal = this.m_fontWeightStack.Remove();
											return true;
										}
										if (nameHashCode == -1668324918)
										{
											this.m_style &= (FontStyles)(-9);
											return true;
										}
										if (nameHashCode != -1632103439)
										{
											if (nameHashCode != -1616441709)
											{
												if (nameHashCode != -884817987)
												{
													if (nameHashCode == -445573839)
													{
														this.m_lineHeight = 0f;
														return true;
													}
													if (nameHashCode == -445537194)
													{
														this.tag_LineIndent = 0f;
														return true;
													}
													if (nameHashCode != -330774850)
													{
														if (nameHashCode == 98)
														{
															this.m_style |= FontStyles.Bold;
															this.m_fontWeightInternal = 700;
															this.m_fontWeightStack.Add(700);
															return true;
														}
														if (nameHashCode == 105)
														{
															this.m_style |= FontStyles.Italic;
															return true;
														}
														if (nameHashCode == 434)
														{
															this.m_style &= (FontStyles)(-3);
															return true;
														}
														if (nameHashCode != 6380)
														{
															if (nameHashCode == 6552)
															{
																this.m_fontScaleMultiplier = ((this.m_currentFontAsset.fontInfo.SubSize <= 0f) ? 1f : this.m_currentFontAsset.fontInfo.SubSize);
																this.m_baselineOffset = this.m_currentFontAsset.fontInfo.SubscriptOffset * this.m_fontScale * this.m_fontScaleMultiplier;
																this.m_style |= FontStyles.Subscript;
																return true;
															}
															if (nameHashCode == 6566)
															{
																this.m_fontScaleMultiplier = ((this.m_currentFontAsset.fontInfo.SubSize <= 0f) ? 1f : this.m_currentFontAsset.fontInfo.SubSize);
																this.m_baselineOffset = this.m_currentFontAsset.fontInfo.SuperscriptOffset * this.m_fontScale * this.m_fontScaleMultiplier;
																this.m_style |= FontStyles.Superscript;
																return true;
															}
															if (nameHashCode == 22501)
															{
																this.m_isIgnoringAlignment = false;
																return true;
															}
															if (nameHashCode == 22673)
															{
																if ((this.m_style & FontStyles.Subscript) == FontStyles.Subscript)
																{
																	if ((this.m_style & FontStyles.Superscript) == FontStyles.Superscript)
																	{
																		this.m_fontScaleMultiplier = ((this.m_currentFontAsset.fontInfo.SubSize <= 0f) ? 1f : this.m_currentFontAsset.fontInfo.SubSize);
																		this.m_baselineOffset = this.m_currentFontAsset.fontInfo.SuperscriptOffset * this.m_fontScale * this.m_fontScaleMultiplier;
																	}
																	else
																	{
																		this.m_baselineOffset = 0f;
																		this.m_fontScaleMultiplier = 1f;
																	}
																	this.m_style &= (FontStyles)(-257);
																}
																return true;
															}
															if (nameHashCode == 22687)
															{
																if ((this.m_style & FontStyles.Superscript) == FontStyles.Superscript)
																{
																	if ((this.m_style & FontStyles.Subscript) == FontStyles.Subscript)
																	{
																		this.m_fontScaleMultiplier = ((this.m_currentFontAsset.fontInfo.SubSize <= 0f) ? 1f : this.m_currentFontAsset.fontInfo.SubSize);
																		this.m_baselineOffset = this.m_currentFontAsset.fontInfo.SubscriptOffset * this.m_fontScale * this.m_fontScaleMultiplier;
																	}
																	else
																	{
																		this.m_baselineOffset = 0f;
																		this.m_fontScaleMultiplier = 1f;
																	}
																	this.m_style &= (FontStyles)(-129);
																}
																return true;
															}
															if (nameHashCode != 41311)
															{
																if (nameHashCode == 43066)
																{
																	if (this.m_isParsingText)
																	{
																		int num10 = this.m_textInfo.linkInfo.Length;
																		if (this.m_textInfo.linkCount + 1 > num10)
																		{
																			TMP_TextInfo.Resize<TMP_LinkInfo>(ref this.m_textInfo.linkInfo, num10 + 1);
																		}
																		int linkCount = this.m_textInfo.linkCount;
																		this.m_textInfo.linkInfo[linkCount].textComponent = this;
																		this.m_textInfo.linkInfo[linkCount].hashCode = this.m_xmlAttribute[0].valueHashCode;
																		this.m_textInfo.linkInfo[linkCount].linkTextfirstCharacterIndex = this.m_characterCount;
																		this.m_textInfo.linkInfo[linkCount].linkIdFirstCharacterIndex = startIndex + this.m_xmlAttribute[0].valueStartIndex;
																		this.m_textInfo.linkInfo[linkCount].linkIdLength = this.m_xmlAttribute[0].valueLength;
																		this.m_textInfo.linkInfo[linkCount].SetLinkID(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																	}
																	return true;
																}
																if (nameHashCode == 43969)
																{
																	this.m_isNonBreakingSpace = true;
																	return true;
																}
																if (nameHashCode == 43991)
																{
																	if (this.m_overflowMode == TextOverflowModes.Page)
																	{
																		this.m_xAdvance = this.tag_LineIndent + this.tag_Indent;
																		this.m_lineOffset = 0f;
																		this.m_pageNumber++;
																		this.m_isNewPage = true;
																	}
																	return true;
																}
																if (nameHashCode != 45545)
																{
																	if (nameHashCode == 154158)
																	{
																		MaterialReference materialReference = this.m_materialReferenceStack.Remove();
																		this.m_currentFontAsset = materialReference.fontAsset;
																		this.m_currentMaterial = materialReference.material;
																		this.m_currentMaterialIndex = materialReference.index;
																		this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																		return true;
																	}
																	if (nameHashCode == 155913)
																	{
																		if (this.m_isParsingText)
																		{
																			this.m_textInfo.linkInfo[this.m_textInfo.linkCount].linkTextLength = this.m_characterCount - this.m_textInfo.linkInfo[this.m_textInfo.linkCount].linkTextfirstCharacterIndex;
																			this.m_textInfo.linkCount++;
																		}
																		return true;
																	}
																	if (nameHashCode == 156816)
																	{
																		this.m_isNonBreakingSpace = false;
																		return true;
																	}
																	if (nameHashCode == 158392)
																	{
																		this.m_currentFontSize = this.m_sizeStack.Remove();
																		this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																		return true;
																	}
																	if (nameHashCode != 275917)
																	{
																		if (nameHashCode != 276254)
																		{
																			if (nameHashCode == 280416)
																			{
																				return false;
																			}
																			if (nameHashCode != 281955)
																			{
																				if (nameHashCode != 320078)
																				{
																					if (nameHashCode != 322689)
																					{
																						if (nameHashCode != 327550)
																						{
																							if (nameHashCode == 1065846)
																							{
																								this.m_lineJustification = this.m_textAlignment;
																								return true;
																							}
																							if (nameHashCode == 1071884)
																							{
																								this.m_htmlColor = this.m_colorStack.Remove();
																								return true;
																							}
																							if (nameHashCode != 1112618)
																							{
																								if (nameHashCode == 1117479)
																								{
																									this.m_width = -1f;
																									return true;
																								}
																								if (nameHashCode == 1750458)
																								{
																									return false;
																								}
																								if (nameHashCode == 1913798)
																								{
																									int valueHashCode = this.m_xmlAttribute[0].valueHashCode;
																									if (this.m_isParsingText)
																									{
																										this.m_actionStack.Add(valueHashCode);
																									}
																									return true;
																								}
																								if (nameHashCode != 1983971)
																								{
																									if (nameHashCode != 2068980)
																									{
																										if (nameHashCode != 2109854)
																										{
																											if (nameHashCode != 2152041)
																											{
																												if (nameHashCode == 2246877)
																												{
																													int valueHashCode2 = this.m_xmlAttribute[0].valueHashCode;
																													TMP_SpriteAsset tmp_SpriteAsset;
																													if (this.m_xmlAttribute[0].valueType == TagType.None || this.m_xmlAttribute[0].valueType == TagType.NumericalValue)
																													{
																														if (this.m_defaultSpriteAsset == null)
																														{
																															if (TMP_Settings.defaultSpriteAsset != null)
																															{
																																this.m_defaultSpriteAsset = TMP_Settings.defaultSpriteAsset;
																															}
																															else
																															{
																																this.m_defaultSpriteAsset = Resources.Load<TMP_SpriteAsset>("Sprite Assets/Default Sprite Asset");
																															}
																														}
																														this.m_currentSpriteAsset = this.m_defaultSpriteAsset;
																														if (this.m_currentSpriteAsset == null)
																														{
																															return false;
																														}
																													}
																													else if (MaterialReferenceManager.TryGetSpriteAsset(valueHashCode2, out tmp_SpriteAsset))
																													{
																														this.m_currentSpriteAsset = tmp_SpriteAsset;
																													}
																													else
																													{
																														if (tmp_SpriteAsset == null)
																														{
																															tmp_SpriteAsset = Resources.Load<TMP_SpriteAsset>("Sprites/" + new string(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength));
																														}
																														if (tmp_SpriteAsset == null)
																														{
																															return false;
																														}
																														MaterialReferenceManager.AddSpriteAsset(valueHashCode2, tmp_SpriteAsset);
																														this.m_currentSpriteAsset = tmp_SpriteAsset;
																													}
																													if (this.m_xmlAttribute[0].valueType == TagType.NumericalValue)
																													{
																														int num11 = (int)this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
																														if (num11 == -9999)
																														{
																															return false;
																														}
																														if (num11 > this.m_currentSpriteAsset.spriteInfoList.Count - 1)
																														{
																															return false;
																														}
																														this.m_spriteIndex = num11;
																													}
																													else if (this.m_xmlAttribute[1].nameHashCode == 43347)
																													{
																														int spriteIndex = this.m_currentSpriteAsset.GetSpriteIndex(this.m_xmlAttribute[1].valueHashCode);
																														if (spriteIndex == -1)
																														{
																															return false;
																														}
																														this.m_spriteIndex = spriteIndex;
																													}
																													else
																													{
																														if (this.m_xmlAttribute[1].nameHashCode != 295562)
																														{
																															return false;
																														}
																														int num12 = (int)this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[1].valueStartIndex, this.m_xmlAttribute[1].valueLength, this.m_xmlAttribute[1].valueDecimalIndex);
																														if (num12 == -9999)
																														{
																															return false;
																														}
																														if (num12 > this.m_currentSpriteAsset.spriteInfoList.Count - 1)
																														{
																															return false;
																														}
																														this.m_spriteIndex = num12;
																													}
																													this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentSpriteAsset.material, this.m_currentSpriteAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
																													this.m_spriteColor = TMP_Text.s_colorWhite;
																													this.m_tintSprite = false;
																													if (this.m_xmlAttribute[1].nameHashCode == 45819)
																													{
																														this.m_tintSprite = (this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[1].valueStartIndex, this.m_xmlAttribute[1].valueLength, this.m_xmlAttribute[1].valueDecimalIndex) != 0f);
																													}
																													else if (this.m_xmlAttribute[2].nameHashCode == 45819)
																													{
																														this.m_tintSprite = (this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[2].valueStartIndex, this.m_xmlAttribute[2].valueLength, this.m_xmlAttribute[2].valueDecimalIndex) != 0f);
																													}
																													if (this.m_xmlAttribute[1].nameHashCode == 281955)
																													{
																														this.m_spriteColor = this.HexCharsToColor(this.m_htmlTag, this.m_xmlAttribute[1].valueStartIndex, this.m_xmlAttribute[1].valueLength);
																													}
																													else if (this.m_xmlAttribute[2].nameHashCode == 281955)
																													{
																														this.m_spriteColor = this.HexCharsToColor(this.m_htmlTag, this.m_xmlAttribute[2].valueStartIndex, this.m_xmlAttribute[2].valueLength);
																													}
																													this.m_xmlAttribute[1].nameHashCode = 0;
																													this.m_xmlAttribute[2].nameHashCode = 0;
																													this.m_textElementType = TMP_TextElementType.Sprite;
																													return true;
																												}
																												if (nameHashCode == 7443301)
																												{
																													if (this.m_isParsingText)
																													{
																													}
																													this.m_actionStack.Remove();
																													return true;
																												}
																												if (nameHashCode == 7513474)
																												{
																													this.m_cSpacing = 0f;
																													return true;
																												}
																												if (nameHashCode == 7598483)
																												{
																													this.tag_Indent = this.m_indentStack.Remove();
																													return true;
																												}
																												if (nameHashCode == 7639357)
																												{
																													this.m_marginLeft = 0f;
																													this.m_marginRight = 0f;
																													return true;
																												}
																												if (nameHashCode == 7681544)
																												{
																													this.m_monoSpacing = 0f;
																													return true;
																												}
																												if (nameHashCode == 15115642)
																												{
																													this.tag_NoParsing = true;
																													return true;
																												}
																												if (nameHashCode != 16034505)
																												{
																													if (nameHashCode != 52232547)
																													{
																														if (nameHashCode != 54741026)
																														{
																															return false;
																														}
																														this.m_baselineOffset = 0f;
																														return true;
																													}
																												}
																												else
																												{
																													float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
																													if (num13 == -9999f || num13 == 0f)
																													{
																														return false;
																													}
																													if (tagUnits == TagUnits.Pixels)
																													{
																														this.m_baselineOffset = num13;
																														return true;
																													}
																													if (tagUnits != TagUnits.FontUnits)
																													{
																														return tagUnits != TagUnits.Percentage && false;
																													}
																													this.m_baselineOffset = num13 * this.m_fontScale * this.m_fontAsset.fontInfo.Ascender;
																													return true;
																												}
																											}
																											else
																											{
																												float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
																												if (num13 == -9999f || num13 == 0f)
																												{
																													return false;
																												}
																												if (tagUnits != TagUnits.Pixels)
																												{
																													if (tagUnits != TagUnits.FontUnits)
																													{
																														if (tagUnits == TagUnits.Percentage)
																														{
																															return false;
																														}
																													}
																													else
																													{
																														this.m_monoSpacing = num13;
																														this.m_monoSpacing *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																													}
																												}
																												else
																												{
																													this.m_monoSpacing = num13;
																												}
																												return true;
																											}
																										}
																										else
																										{
																											float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
																											if (num13 == -9999f || num13 == 0f)
																											{
																												return false;
																											}
																											this.m_marginLeft = num13;
																											if (tagUnits != TagUnits.Pixels)
																											{
																												if (tagUnits != TagUnits.FontUnits)
																												{
																													if (tagUnits == TagUnits.Percentage)
																													{
																														this.m_marginLeft = (this.m_marginWidth - ((this.m_width == -1f) ? 0f : this.m_width)) * this.m_marginLeft / 100f;
																													}
																												}
																												else
																												{
																													this.m_marginLeft *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																												}
																											}
																											this.m_marginLeft = ((this.m_marginLeft < 0f) ? 0f : this.m_marginLeft);
																											this.m_marginRight = this.m_marginLeft;
																											return true;
																										}
																									}
																									else
																									{
																										float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
																										if (num13 == -9999f || num13 == 0f)
																										{
																											return false;
																										}
																										if (tagUnits != TagUnits.Pixels)
																										{
																											if (tagUnits != TagUnits.FontUnits)
																											{
																												if (tagUnits == TagUnits.Percentage)
																												{
																													this.tag_Indent = this.m_marginWidth * num13 / 100f;
																												}
																											}
																											else
																											{
																												this.tag_Indent = num13;
																												this.tag_Indent *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																											}
																										}
																										else
																										{
																											this.tag_Indent = num13;
																										}
																										this.m_indentStack.Add(this.tag_Indent);
																										this.m_xAdvance = this.tag_Indent;
																										return true;
																									}
																								}
																								else
																								{
																									float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
																									if (num13 == -9999f || num13 == 0f)
																									{
																										return false;
																									}
																									if (tagUnits != TagUnits.Pixels)
																									{
																										if (tagUnits != TagUnits.FontUnits)
																										{
																											if (tagUnits == TagUnits.Percentage)
																											{
																												return false;
																											}
																										}
																										else
																										{
																											this.m_cSpacing = num13;
																											this.m_cSpacing *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																										}
																									}
																									else
																									{
																										this.m_cSpacing = num13;
																									}
																									return true;
																								}
																							}
																							else
																							{
																								TMP_Style style = TMP_StyleSheet.GetStyle(this.m_xmlAttribute[0].valueHashCode);
																								if (style == null)
																								{
																									int hashCode = this.m_styleStack.Remove();
																									style = TMP_StyleSheet.GetStyle(hashCode);
																								}
																								if (style == null)
																								{
																									return false;
																								}
																								for (int i = 0; i < style.styleClosingTagArray.Length; i++)
																								{
																									if (style.styleClosingTagArray[i] == 60)
																									{
																										this.ValidateHtmlTag(style.styleClosingTagArray, i + 1, out i);
																									}
																								}
																								return true;
																							}
																						}
																						else
																						{
																							float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
																							if (num13 == -9999f || num13 == 0f)
																							{
																								return false;
																							}
																							if (tagUnits != TagUnits.Pixels)
																							{
																								if (tagUnits == TagUnits.FontUnits)
																								{
																									return false;
																								}
																								if (tagUnits == TagUnits.Percentage)
																								{
																									this.m_width = this.m_marginWidth * num13 / 100f;
																								}
																							}
																							else
																							{
																								this.m_width = num13;
																							}
																							return true;
																						}
																					}
																					else
																					{
																						TMP_Style style = TMP_StyleSheet.GetStyle(this.m_xmlAttribute[0].valueHashCode);
																						if (style == null)
																						{
																							return false;
																						}
																						this.m_styleStack.Add(style.hashCode);
																						for (int j = 0; j < style.styleOpeningTagArray.Length; j++)
																						{
																							if (style.styleOpeningTagArray[j] == 60)
																							{
																								this.ValidateHtmlTag(style.styleOpeningTagArray, j + 1, out j);
																							}
																						}
																						return true;
																					}
																				}
																				else
																				{
																					float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
																					if (num13 == -9999f || num13 == 0f)
																					{
																						return false;
																					}
																					if (tagUnits == TagUnits.Pixels)
																					{
																						this.m_xAdvance += num13;
																						return true;
																					}
																					if (tagUnits != TagUnits.FontUnits)
																					{
																						return tagUnits != TagUnits.Percentage && false;
																					}
																					this.m_xAdvance += num13 * this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																					return true;
																				}
																			}
																			else
																			{
																				if (this.m_htmlTag[6] == '#' && num == 13)
																				{
																					this.m_htmlColor = this.HexCharsToColor(this.m_htmlTag, num);
																					this.m_colorStack.Add(this.m_htmlColor);
																					return true;
																				}
																				if (this.m_htmlTag[6] == '#' && num == 15)
																				{
																					this.m_htmlColor = this.HexCharsToColor(this.m_htmlTag, num);
																					this.m_colorStack.Add(this.m_htmlColor);
																					return true;
																				}
																				int valueHashCode3 = this.m_xmlAttribute[0].valueHashCode;
																				if (valueHashCode3 == -36881330)
																				{
																					this.m_htmlColor = new Color32(160, 32, 240, byte.MaxValue);
																					this.m_colorStack.Add(this.m_htmlColor);
																					return true;
																				}
																				if (valueHashCode3 == 125395)
																				{
																					this.m_htmlColor = Color.red;
																					this.m_colorStack.Add(this.m_htmlColor);
																					return true;
																				}
																				if (valueHashCode3 == 3573310)
																				{
																					this.m_htmlColor = Color.blue;
																					this.m_colorStack.Add(this.m_htmlColor);
																					return true;
																				}
																				if (valueHashCode3 == 26556144)
																				{
																					this.m_htmlColor = new Color32(byte.MaxValue, 128, 0, byte.MaxValue);
																					this.m_colorStack.Add(this.m_htmlColor);
																					return true;
																				}
																				if (valueHashCode3 == 117905991)
																				{
																					this.m_htmlColor = Color.black;
																					this.m_colorStack.Add(this.m_htmlColor);
																					return true;
																				}
																				if (valueHashCode3 == 121463835)
																				{
																					this.m_htmlColor = Color.green;
																					this.m_colorStack.Add(this.m_htmlColor);
																					return true;
																				}
																				if (valueHashCode3 == 140357351)
																				{
																					this.m_htmlColor = Color.white;
																					this.m_colorStack.Add(this.m_htmlColor);
																					return true;
																				}
																				if (valueHashCode3 != 554054276)
																				{
																					return false;
																				}
																				this.m_htmlColor = Color.yellow;
																				this.m_colorStack.Add(this.m_htmlColor);
																				return true;
																			}
																		}
																		else
																		{
																			if (this.m_xmlAttribute[0].valueLength != 3)
																			{
																				return false;
																			}
																			this.m_htmlColor.a = (byte)(this.HexToInt(this.m_htmlTag[7]) * 16 + this.HexToInt(this.m_htmlTag[8]));
																			return true;
																		}
																	}
																	else
																	{
																		int valueHashCode4 = this.m_xmlAttribute[0].valueHashCode;
																		if (valueHashCode4 == -523808257)
																		{
																			this.m_lineJustification = TextAlignmentOptions.Justified;
																			return true;
																		}
																		if (valueHashCode4 == -458210101)
																		{
																			this.m_lineJustification = TextAlignmentOptions.Center;
																			return true;
																		}
																		if (valueHashCode4 == 3774683)
																		{
																			this.m_lineJustification = TextAlignmentOptions.Left;
																			return true;
																		}
																		if (valueHashCode4 != 136703040)
																		{
																			return false;
																		}
																		this.m_lineJustification = TextAlignmentOptions.Right;
																		return true;
																	}
																}
																else
																{
																	float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
																	if (num13 == -9999f || num13 == 0f)
																	{
																		return false;
																	}
																	if (tagUnits != TagUnits.Pixels)
																	{
																		if (tagUnits == TagUnits.FontUnits)
																		{
																			this.m_currentFontSize = this.m_fontSize * num13;
																			this.m_sizeStack.Add(this.m_currentFontSize);
																			this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																			return true;
																		}
																		if (tagUnits != TagUnits.Percentage)
																		{
																			return false;
																		}
																		this.m_currentFontSize = this.m_fontSize * num13 / 100f;
																		this.m_sizeStack.Add(this.m_currentFontSize);
																		this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																		return true;
																	}
																	else
																	{
																		if (this.m_htmlTag[5] == '+')
																		{
																			this.m_currentFontSize = this.m_fontSize + num13;
																			this.m_sizeStack.Add(this.m_currentFontSize);
																			this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																			return true;
																		}
																		if (this.m_htmlTag[5] == '-')
																		{
																			this.m_currentFontSize = this.m_fontSize + num13;
																			this.m_sizeStack.Add(this.m_currentFontSize);
																			this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																			return true;
																		}
																		this.m_currentFontSize = num13;
																		this.m_sizeStack.Add(this.m_currentFontSize);
																		this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																		return true;
																	}
																}
															}
															else
															{
																int valueHashCode5 = this.m_xmlAttribute[0].valueHashCode;
																int nameHashCode2 = this.m_xmlAttribute[1].nameHashCode;
																int valueHashCode6 = this.m_xmlAttribute[1].valueHashCode;
																if (valueHashCode5 == 764638571 || valueHashCode5 == 523367755)
																{
																	this.m_currentFontAsset = this.m_materialReferences[0].fontAsset;
																	this.m_currentMaterial = this.m_materialReferences[0].material;
																	this.m_currentMaterialIndex = 0;
																	this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																	this.m_materialReferenceStack.Add(this.m_materialReferences[0]);
																	return true;
																}
																TMP_FontAsset tmp_FontAsset;
																if (!MaterialReferenceManager.TryGetFontAsset(valueHashCode5, out tmp_FontAsset))
																{
																	tmp_FontAsset = Resources.Load<TMP_FontAsset>("Fonts & Materials/" + new string(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength));
																	if (tmp_FontAsset == null)
																	{
																		return false;
																	}
																	MaterialReferenceManager.AddFontAsset(tmp_FontAsset);
																}
																if (nameHashCode2 == 0 && valueHashCode6 == 0)
																{
																	this.m_currentMaterial = tmp_FontAsset.material;
																	this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, tmp_FontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
																	this.m_materialReferenceStack.Add(this.m_materialReferences[this.m_currentMaterialIndex]);
																}
																else
																{
																	if (nameHashCode2 != 103415287)
																	{
																		return false;
																	}
																	Material material;
																	if (MaterialReferenceManager.TryGetMaterial(valueHashCode6, out material))
																	{
																		this.m_currentMaterial = material;
																		this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, tmp_FontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
																		this.m_materialReferenceStack.Add(this.m_materialReferences[this.m_currentMaterialIndex]);
																	}
																	else
																	{
																		material = Resources.Load<Material>("Fonts & Materials/" + new string(this.m_htmlTag, this.m_xmlAttribute[1].valueStartIndex, this.m_xmlAttribute[1].valueLength));
																		if (material == null)
																		{
																			return false;
																		}
																		MaterialReferenceManager.AddFontMaterial(valueHashCode6, material);
																		this.m_currentMaterial = material;
																		this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, tmp_FontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
																		this.m_materialReferenceStack.Add(this.m_materialReferences[this.m_currentMaterialIndex]);
																	}
																}
																this.m_currentFontAsset = tmp_FontAsset;
																this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																return true;
															}
														}
														else
														{
															float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
															if (num13 == -9999f)
															{
																return false;
															}
															if (tagUnits == TagUnits.Pixels)
															{
																this.m_xAdvance = num13;
																return true;
															}
															if (tagUnits == TagUnits.FontUnits)
															{
																this.m_xAdvance = num13 * this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																return true;
															}
															if (tagUnits != TagUnits.Percentage)
															{
																return false;
															}
															this.m_xAdvance = this.m_marginWidth * num13 / 100f;
															return true;
														}
													}
													else
													{
														float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
														if (num13 == -9999f || num13 == 0f)
														{
															return false;
														}
														if ((this.m_fontStyle & FontStyles.Bold) == FontStyles.Bold)
														{
															return true;
														}
														this.m_style &= (FontStyles)(-2);
														int num14 = (int)num13;
														if (num14 != 100)
														{
															if (num14 != 200)
															{
																if (num14 != 300)
																{
																	if (num14 != 400)
																	{
																		if (num14 != 500)
																		{
																			if (num14 != 600)
																			{
																				if (num14 != 700)
																				{
																					if (num14 != 800)
																					{
																						if (num14 == 900)
																						{
																							this.m_fontWeightInternal = 900;
																						}
																					}
																					else
																					{
																						this.m_fontWeightInternal = 800;
																					}
																				}
																				else
																				{
																					this.m_fontWeightInternal = 700;
																					this.m_style |= FontStyles.Bold;
																				}
																			}
																			else
																			{
																				this.m_fontWeightInternal = 600;
																			}
																		}
																		else
																		{
																			this.m_fontWeightInternal = 500;
																		}
																	}
																	else
																	{
																		this.m_fontWeightInternal = 400;
																	}
																}
																else
																{
																	this.m_fontWeightInternal = 300;
																}
															}
															else
															{
																this.m_fontWeightInternal = 200;
															}
														}
														else
														{
															this.m_fontWeightInternal = 100;
														}
														this.m_fontWeightStack.Add(this.m_fontWeightInternal);
														return true;
													}
												}
												else
												{
													float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
													if (num13 == -9999f || num13 == 0f)
													{
														return false;
													}
													this.m_marginRight = num13;
													if (tagUnits != TagUnits.Pixels)
													{
														if (tagUnits != TagUnits.FontUnits)
														{
															if (tagUnits == TagUnits.Percentage)
															{
																this.m_marginRight = (this.m_marginWidth - ((this.m_width == -1f) ? 0f : this.m_width)) * this.m_marginRight / 100f;
															}
														}
														else
														{
															this.m_marginRight *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
														}
													}
													this.m_marginRight = ((this.m_marginRight < 0f) ? 0f : this.m_marginRight);
													return true;
												}
											}
											this.m_style &= (FontStyles)(-17);
											return true;
										}
										this.m_style &= (FontStyles)(-33);
										return true;
									}
									else
									{
										float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
										if (num13 == -9999f || num13 == 0f)
										{
											return false;
										}
										if (tagUnits != TagUnits.Pixels)
										{
											if (tagUnits != TagUnits.FontUnits)
											{
												if (tagUnits == TagUnits.Percentage)
												{
													this.tag_LineIndent = this.m_marginWidth * num13 / 100f;
												}
											}
											else
											{
												this.tag_LineIndent = num13;
												this.tag_LineIndent *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
											}
										}
										else
										{
											this.tag_LineIndent = num13;
										}
										this.m_xAdvance += this.tag_LineIndent;
										return true;
									}
								}
								else
								{
									float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
									if (num13 == -9999f || num13 == 0f)
									{
										return false;
									}
									this.m_lineHeight = num13;
									if (tagUnits != TagUnits.Pixels)
									{
										if (tagUnits != TagUnits.FontUnits)
										{
											if (tagUnits == TagUnits.Percentage)
											{
												this.m_lineHeight = this.m_fontAsset.fontInfo.LineHeight * this.m_lineHeight / 100f * this.m_fontScale;
											}
										}
										else
										{
											this.m_lineHeight *= this.m_fontAsset.fontInfo.LineHeight * this.m_fontScale;
										}
									}
									return true;
								}
							}
							else
							{
								float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
								if (num13 == -9999f || num13 == 0f)
								{
									return false;
								}
								this.m_marginLeft = num13;
								if (tagUnits != TagUnits.Pixels)
								{
									if (tagUnits != TagUnits.FontUnits)
									{
										if (tagUnits == TagUnits.Percentage)
										{
											this.m_marginLeft = (this.m_marginWidth - ((this.m_width == -1f) ? 0f : this.m_width)) * this.m_marginLeft / 100f;
										}
									}
									else
									{
										this.m_marginLeft *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
									}
								}
								this.m_marginLeft = ((this.m_marginLeft < 0f) ? 0f : this.m_marginLeft);
								return true;
							}
						}
					}
					this.m_style |= FontStyles.UpperCase;
					return true;
				case 446:
					if ((this.m_fontStyle & FontStyles.Underline) != FontStyles.Underline)
					{
						this.m_style &= (FontStyles)(-5);
					}
					return true;
				}
				break;
			case 117:
				this.m_style |= FontStyles.Underline;
				return true;
			}
		}

		// Token: 0x04005320 RID: 21280
		[SerializeField]
		protected string m_text;

		// Token: 0x04005321 RID: 21281
		[SerializeField]
		protected TMP_FontAsset m_fontAsset;

		// Token: 0x04005322 RID: 21282
		protected TMP_FontAsset m_currentFontAsset;

		// Token: 0x04005323 RID: 21283
		protected bool m_isSDFShader;

		// Token: 0x04005324 RID: 21284
		[SerializeField]
		protected Material m_sharedMaterial;

		// Token: 0x04005325 RID: 21285
		protected Material m_currentMaterial;

		// Token: 0x04005326 RID: 21286
		protected MaterialReference[] m_materialReferences = new MaterialReference[32];

		// Token: 0x04005327 RID: 21287
		protected Dictionary<int, int> m_materialReferenceIndexLookup = new Dictionary<int, int>();

		// Token: 0x04005328 RID: 21288
		protected TMP_XmlTagStack<MaterialReference> m_materialReferenceStack = new TMP_XmlTagStack<MaterialReference>(new MaterialReference[16]);

		// Token: 0x04005329 RID: 21289
		protected int m_currentMaterialIndex;

		// Token: 0x0400532A RID: 21290
		protected int m_sharedMaterialHashCode;

		// Token: 0x0400532B RID: 21291
		[SerializeField]
		protected Material[] m_fontSharedMaterials;

		// Token: 0x0400532C RID: 21292
		[SerializeField]
		protected Material m_fontMaterial;

		// Token: 0x0400532D RID: 21293
		[SerializeField]
		protected Material[] m_fontMaterials;

		// Token: 0x0400532E RID: 21294
		protected bool m_isMaterialDirty;

		// Token: 0x0400532F RID: 21295
		[FormerlySerializedAs("m_fontColor")]
		[SerializeField]
		protected Color32 m_fontColor32 = Color.white;

		// Token: 0x04005330 RID: 21296
		[SerializeField]
		protected Color m_fontColor = Color.white;

		// Token: 0x04005331 RID: 21297
		protected static Color32 s_colorWhite = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x04005332 RID: 21298
		[SerializeField]
		protected bool m_enableVertexGradient;

		// Token: 0x04005333 RID: 21299
		[SerializeField]
		protected VertexGradient m_fontColorGradient = new VertexGradient(Color.white);

		// Token: 0x04005334 RID: 21300
		protected TMP_SpriteAsset m_spriteAsset;

		// Token: 0x04005335 RID: 21301
		[SerializeField]
		protected bool m_tintAllSprites;

		// Token: 0x04005336 RID: 21302
		protected bool m_tintSprite;

		// Token: 0x04005337 RID: 21303
		protected Color32 m_spriteColor;

		// Token: 0x04005338 RID: 21304
		[SerializeField]
		protected bool m_overrideHtmlColors;

		// Token: 0x04005339 RID: 21305
		[SerializeField]
		protected Color32 m_faceColor = Color.white;

		// Token: 0x0400533A RID: 21306
		[SerializeField]
		protected Color32 m_outlineColor = Color.black;

		// Token: 0x0400533B RID: 21307
		protected float m_outlineWidth;

		// Token: 0x0400533C RID: 21308
		[SerializeField]
		protected float m_fontSize = 36f;

		// Token: 0x0400533D RID: 21309
		protected float m_currentFontSize;

		// Token: 0x0400533E RID: 21310
		[SerializeField]
		protected float m_fontSizeBase = 36f;

		// Token: 0x0400533F RID: 21311
		protected TMP_XmlTagStack<float> m_sizeStack = new TMP_XmlTagStack<float>(new float[16]);

		// Token: 0x04005340 RID: 21312
		[SerializeField]
		protected int m_fontWeight = 400;

		// Token: 0x04005341 RID: 21313
		protected int m_fontWeightInternal;

		// Token: 0x04005342 RID: 21314
		protected TMP_XmlTagStack<int> m_fontWeightStack = new TMP_XmlTagStack<int>(new int[16]);

		// Token: 0x04005343 RID: 21315
		[SerializeField]
		protected bool m_enableAutoSizing;

		// Token: 0x04005344 RID: 21316
		protected float m_maxFontSize;

		// Token: 0x04005345 RID: 21317
		protected float m_minFontSize;

		// Token: 0x04005346 RID: 21318
		[SerializeField]
		protected float m_fontSizeMin;

		// Token: 0x04005347 RID: 21319
		[SerializeField]
		protected float m_fontSizeMax;

		// Token: 0x04005348 RID: 21320
		[SerializeField]
		protected FontStyles m_fontStyle;

		// Token: 0x04005349 RID: 21321
		protected FontStyles m_style;

		// Token: 0x0400534A RID: 21322
		protected bool m_isUsingBold;

		// Token: 0x0400534B RID: 21323
		[SerializeField]
		[FormerlySerializedAs("m_lineJustification")]
		protected TextAlignmentOptions m_textAlignment;

		// Token: 0x0400534C RID: 21324
		protected TextAlignmentOptions m_lineJustification;

		// Token: 0x0400534D RID: 21325
		protected Vector3[] m_textContainerLocalCorners = new Vector3[4];

		// Token: 0x0400534E RID: 21326
		[SerializeField]
		protected float m_characterSpacing;

		// Token: 0x0400534F RID: 21327
		protected float m_cSpacing;

		// Token: 0x04005350 RID: 21328
		protected float m_monoSpacing;

		// Token: 0x04005351 RID: 21329
		[SerializeField]
		protected float m_lineSpacing;

		// Token: 0x04005352 RID: 21330
		protected float m_lineSpacingDelta;

		// Token: 0x04005353 RID: 21331
		protected float m_lineHeight;

		// Token: 0x04005354 RID: 21332
		[SerializeField]
		protected float m_lineSpacingMax;

		// Token: 0x04005355 RID: 21333
		[SerializeField]
		protected float m_paragraphSpacing;

		// Token: 0x04005356 RID: 21334
		[SerializeField]
		protected float m_charWidthMaxAdj;

		// Token: 0x04005357 RID: 21335
		protected float m_charWidthAdjDelta;

		// Token: 0x04005358 RID: 21336
		[SerializeField]
		protected bool m_enableWordWrapping;

		// Token: 0x04005359 RID: 21337
		protected bool m_isCharacterWrappingEnabled;

		// Token: 0x0400535A RID: 21338
		protected bool m_isNonBreakingSpace;

		// Token: 0x0400535B RID: 21339
		protected bool m_isIgnoringAlignment;

		// Token: 0x0400535C RID: 21340
		[SerializeField]
		protected float m_wordWrappingRatios = 0.4f;

		// Token: 0x0400535D RID: 21341
		[SerializeField]
		protected TextOverflowModes m_overflowMode;

		// Token: 0x0400535E RID: 21342
		protected bool m_isTextTruncated;

		// Token: 0x0400535F RID: 21343
		[SerializeField]
		protected bool m_enableKerning;

		// Token: 0x04005360 RID: 21344
		[SerializeField]
		protected bool m_enableExtraPadding;

		// Token: 0x04005361 RID: 21345
		[SerializeField]
		protected bool checkPaddingRequired;

		// Token: 0x04005362 RID: 21346
		[SerializeField]
		protected bool m_isRichText = true;

		// Token: 0x04005363 RID: 21347
		protected bool m_parseCtrlCharacters = true;

		// Token: 0x04005364 RID: 21348
		protected bool m_isOverlay;

		// Token: 0x04005365 RID: 21349
		[SerializeField]
		protected bool m_isOrthographic;

		// Token: 0x04005366 RID: 21350
		[SerializeField]
		protected bool m_isCullingEnabled;

		// Token: 0x04005367 RID: 21351
		[SerializeField]
		protected bool m_ignoreCulling = true;

		// Token: 0x04005368 RID: 21352
		[SerializeField]
		protected TextureMappingOptions m_horizontalMapping;

		// Token: 0x04005369 RID: 21353
		[SerializeField]
		protected TextureMappingOptions m_verticalMapping;

		// Token: 0x0400536A RID: 21354
		protected TextRenderFlags m_renderMode = TextRenderFlags.Render;

		// Token: 0x0400536B RID: 21355
		protected int m_maxVisibleCharacters = 99999;

		// Token: 0x0400536C RID: 21356
		protected int m_maxVisibleWords = 99999;

		// Token: 0x0400536D RID: 21357
		protected int m_maxVisibleLines = 99999;

		// Token: 0x0400536E RID: 21358
		[SerializeField]
		protected int m_pageToDisplay = 1;

		// Token: 0x0400536F RID: 21359
		protected bool m_isNewPage;

		// Token: 0x04005370 RID: 21360
		[SerializeField]
		protected Vector4 m_margin = new Vector4(0f, 0f, 0f, 0f);

		// Token: 0x04005371 RID: 21361
		protected float m_marginLeft;

		// Token: 0x04005372 RID: 21362
		protected float m_marginRight;

		// Token: 0x04005373 RID: 21363
		protected float m_marginWidth;

		// Token: 0x04005374 RID: 21364
		protected float m_marginHeight;

		// Token: 0x04005375 RID: 21365
		protected float m_width = -1f;

		// Token: 0x04005376 RID: 21366
		[SerializeField]
		protected TMP_TextInfo m_textInfo;

		// Token: 0x04005377 RID: 21367
		[SerializeField]
		protected bool m_havePropertiesChanged;

		// Token: 0x04005378 RID: 21368
		[SerializeField]
		protected bool m_isUsingLegacyAnimationComponent;

		// Token: 0x04005379 RID: 21369
		protected Transform m_transform;

		// Token: 0x0400537A RID: 21370
		protected RectTransform m_rectTransform;

		// Token: 0x0400537C RID: 21372
		protected Mesh m_mesh;

		// Token: 0x0400537E RID: 21374
		protected float m_flexibleHeight = -1f;

		// Token: 0x0400537F RID: 21375
		protected float m_flexibleWidth = -1f;

		// Token: 0x04005380 RID: 21376
		protected float m_minHeight;

		// Token: 0x04005381 RID: 21377
		protected float m_minWidth;

		// Token: 0x04005382 RID: 21378
		protected float m_preferredWidth = 9999f;

		// Token: 0x04005383 RID: 21379
		protected float m_renderedWidth;

		// Token: 0x04005384 RID: 21380
		protected float m_preferredHeight = 9999f;

		// Token: 0x04005385 RID: 21381
		protected float m_renderedHeight;

		// Token: 0x04005386 RID: 21382
		protected int m_layoutPriority;

		// Token: 0x04005387 RID: 21383
		protected bool m_isCalculateSizeRequired;

		// Token: 0x04005388 RID: 21384
		protected bool m_isLayoutDirty;

		// Token: 0x04005389 RID: 21385
		protected bool m_verticesAlreadyDirty;

		// Token: 0x0400538A RID: 21386
		protected bool m_layoutAlreadyDirty;

		// Token: 0x0400538B RID: 21387
		[SerializeField]
		protected bool m_isInputParsingRequired;

		// Token: 0x0400538C RID: 21388
		[SerializeField]
		protected bool m_isRightToLeft;

		// Token: 0x0400538D RID: 21389
		[SerializeField]
		protected TMP_Text.TextInputSources m_inputSource;

		// Token: 0x0400538E RID: 21390
		protected string old_text;

		// Token: 0x0400538F RID: 21391
		protected float old_arg0;

		// Token: 0x04005390 RID: 21392
		protected float old_arg1;

		// Token: 0x04005391 RID: 21393
		protected float old_arg2;

		// Token: 0x04005392 RID: 21394
		protected float m_fontScale;

		// Token: 0x04005393 RID: 21395
		protected float m_fontScaleMultiplier;

		// Token: 0x04005394 RID: 21396
		protected char[] m_htmlTag = new char[64];

		// Token: 0x04005395 RID: 21397
		protected XML_TagAttribute[] m_xmlAttribute = new XML_TagAttribute[8];

		// Token: 0x04005396 RID: 21398
		protected float tag_LineIndent;

		// Token: 0x04005397 RID: 21399
		protected float tag_Indent;

		// Token: 0x04005398 RID: 21400
		protected TMP_XmlTagStack<float> m_indentStack = new TMP_XmlTagStack<float>(new float[16]);

		// Token: 0x04005399 RID: 21401
		protected bool tag_NoParsing;

		// Token: 0x0400539A RID: 21402
		protected bool m_isParsingText;

		// Token: 0x0400539B RID: 21403
		protected int[] m_char_buffer;

		// Token: 0x0400539C RID: 21404
		private TMP_CharacterInfo[] m_internalCharacterInfo;

		// Token: 0x0400539D RID: 21405
		protected char[] m_input_CharArray = new char[256];

		// Token: 0x0400539E RID: 21406
		private int m_charArray_Length;

		// Token: 0x0400539F RID: 21407
		protected int m_totalCharacterCount;

		// Token: 0x040053A0 RID: 21408
		protected int m_characterCount;

		// Token: 0x040053A1 RID: 21409
		protected int m_visibleCharacterCount;

		// Token: 0x040053A2 RID: 21410
		protected int m_visibleSpriteCount;

		// Token: 0x040053A3 RID: 21411
		protected int m_firstCharacterOfLine;

		// Token: 0x040053A4 RID: 21412
		protected int m_firstVisibleCharacterOfLine;

		// Token: 0x040053A5 RID: 21413
		protected int m_lastCharacterOfLine;

		// Token: 0x040053A6 RID: 21414
		protected int m_lastVisibleCharacterOfLine;

		// Token: 0x040053A7 RID: 21415
		protected int m_lineNumber;

		// Token: 0x040053A8 RID: 21416
		protected int m_pageNumber;

		// Token: 0x040053A9 RID: 21417
		protected float m_maxAscender;

		// Token: 0x040053AA RID: 21418
		protected float m_maxDescender;

		// Token: 0x040053AB RID: 21419
		protected float m_maxLineAscender;

		// Token: 0x040053AC RID: 21420
		protected float m_maxLineDescender;

		// Token: 0x040053AD RID: 21421
		protected float m_startOfLineAscender;

		// Token: 0x040053AE RID: 21422
		protected float m_lineOffset;

		// Token: 0x040053AF RID: 21423
		protected Extents m_meshExtents;

		// Token: 0x040053B0 RID: 21424
		protected Color32 m_htmlColor = new Color(255f, 255f, 255f, 128f);

		// Token: 0x040053B1 RID: 21425
		protected TMP_XmlTagStack<Color32> m_colorStack = new TMP_XmlTagStack<Color32>(new Color32[16]);

		// Token: 0x040053B2 RID: 21426
		protected float m_tabSpacing;

		// Token: 0x040053B3 RID: 21427
		protected float m_spacing;

		// Token: 0x040053B4 RID: 21428
		protected bool IsRectTransformDriven;

		// Token: 0x040053B5 RID: 21429
		protected TMP_XmlTagStack<int> m_styleStack = new TMP_XmlTagStack<int>(new int[16]);

		// Token: 0x040053B6 RID: 21430
		protected TMP_XmlTagStack<int> m_actionStack = new TMP_XmlTagStack<int>(new int[16]);

		// Token: 0x040053B7 RID: 21431
		protected float m_padding;

		// Token: 0x040053B8 RID: 21432
		protected float m_baselineOffset;

		// Token: 0x040053B9 RID: 21433
		protected float m_xAdvance;

		// Token: 0x040053BA RID: 21434
		protected TMP_TextElementType m_textElementType;

		// Token: 0x040053BB RID: 21435
		protected TMP_TextElement m_cached_TextElement;

		// Token: 0x040053BC RID: 21436
		protected TMP_Glyph m_cached_Underline_GlyphInfo;

		// Token: 0x040053BD RID: 21437
		protected TMP_Glyph m_cached_Ellipsis_GlyphInfo;

		// Token: 0x040053BE RID: 21438
		protected TMP_SpriteAsset m_defaultSpriteAsset;

		// Token: 0x040053BF RID: 21439
		protected TMP_SpriteAsset m_currentSpriteAsset;

		// Token: 0x040053C0 RID: 21440
		protected int m_spriteCount;

		// Token: 0x040053C1 RID: 21441
		protected int m_spriteIndex;

		// Token: 0x040053C2 RID: 21442
		protected InlineGraphicManager m_inlineGraphics;

		// Token: 0x040053C3 RID: 21443
		private readonly float[] k_Power = new float[]
		{
			0.5f,
			0.05f,
			0.005f,
			0.0005f,
			5E-05f,
			5E-06f,
			5E-07f,
			5E-08f,
			5E-09f,
			5E-10f
		};

		// Token: 0x040053C4 RID: 21444
		protected static Vector2 k_InfinityVectorPositive = new Vector2(1000000f, 1000000f);

		// Token: 0x040053C5 RID: 21445
		protected static Vector2 k_InfinityVectorNegative = new Vector2(-1000000f, -1000000f);

		// Token: 0x02000C8B RID: 3211
		protected enum TextInputSources
		{
			// Token: 0x040053C7 RID: 21447
			Text,
			// Token: 0x040053C8 RID: 21448
			SetText,
			// Token: 0x040053C9 RID: 21449
			SetCharArray
		}
	}
}
