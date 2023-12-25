using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

// Token: 0x02000449 RID: 1097
public class GlowText : MonoBehaviour
{
	// Token: 0x0600105A RID: 4186 RVA: 0x0009F2E0 File Offset: 0x0009D6E0
	public void InitTMPText(params MaskableGraphic[] tmp_texts)
	{
		if (tmp_texts.Length > this.tmpTextsToGlow.Length)
		{
			return;
		}
		for (int i = 0; i < tmp_texts.Length; i++)
		{
			if (tmp_texts[i] is Text)
			{
				Text text = tmp_texts[i] as Text;
				this.tmpTextsToGlow[i].enabled = true;
				this.tmpTextsToGlow[i].text = text.text;
				this.tmpTextsToGlow[i].fontSize = (float)text.fontSize;
			}
			else if (tmp_texts[i] is TextMeshProUGUI)
			{
				TextMeshProUGUI textMeshProUGUI = tmp_texts[i] as TextMeshProUGUI;
				tmp_texts[i] = (tmp_texts[i] as Text);
				this.tmpTextsToGlow[i].enabled = true;
				this.tmpTextsToGlow[i].text = textMeshProUGUI.text;
				this.tmpTextsToGlow[i].fontSize = textMeshProUGUI.fontSize;
				this.tmpTextsToGlow[i].font = textMeshProUGUI.font;
				this.tmpTextsToGlow[i].outlineWidth = textMeshProUGUI.outlineWidth;
			}
		}
	}

	// Token: 0x0600105B RID: 4187 RVA: 0x0009F3E0 File Offset: 0x0009D7E0
	public void DisableTMPText()
	{
		for (int i = 0; i < this.tmpTextsToGlow.Length; i++)
		{
			this.tmpTextsToGlow[i].enabled = false;
		}
	}

	// Token: 0x0600105C RID: 4188 RVA: 0x0009F414 File Offset: 0x0009D814
	public void InitImages(params Image[] images)
	{
		if (images.Length > this.imagesToGlow.Length)
		{
			return;
		}
		for (int i = 0; i < images.Length; i++)
		{
			this.imagesToGlow[i].enabled = true;
			this.imagesToGlow[i].sprite = images[i].sprite;
			this.imagesToGlow[i].color = images[i].color;
		}
	}

	// Token: 0x0600105D RID: 4189 RVA: 0x0009F480 File Offset: 0x0009D880
	public void DisableImages()
	{
		for (int i = 0; i < this.imagesToGlow.Length; i++)
		{
			this.imagesToGlow[i].enabled = false;
		}
	}

	// Token: 0x0600105E RID: 4190 RVA: 0x0009F4B4 File Offset: 0x0009D8B4
	public void BeginGlow()
	{
		this.rawImageGlow.enabled = true;
		base.StartCoroutine(this.Glow_cr());
	}

	// Token: 0x0600105F RID: 4191 RVA: 0x0009F4CF File Offset: 0x0009D8CF
	public void StopGlow()
	{
		this.rawImageGlow.enabled = false;
	}

	// Token: 0x06001060 RID: 4192 RVA: 0x0009F4E0 File Offset: 0x0009D8E0
	private IEnumerator Glow_cr()
	{
		RenderTexture rt = RenderTexture.active;
		RenderTexture.active = this.renderTextureGlow;
		GL.Clear(true, true, Color.clear);
		RenderTexture.active = rt;
		this.cameraGlow.GetComponent<PostProcessingBehaviour>().enabled = true;
		yield return null;
		this.cameraGlow.GetComponent<PostProcessingBehaviour>().enabled = false;
		yield break;
	}

	// Token: 0x040019B1 RID: 6577
	[SerializeField]
	private RenderTexture renderTextureGlow;

	// Token: 0x040019B2 RID: 6578
	[SerializeField]
	private GameObject cameraGlow;

	// Token: 0x040019B3 RID: 6579
	[SerializeField]
	private RawImage rawImageGlow;

	// Token: 0x040019B4 RID: 6580
	[SerializeField]
	private TextMeshProUGUI[] tmpTextsToGlow;

	// Token: 0x040019B5 RID: 6581
	[SerializeField]
	private Image[] imagesToGlow;
}
