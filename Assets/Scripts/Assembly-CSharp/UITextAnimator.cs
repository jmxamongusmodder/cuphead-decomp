using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000389 RID: 905
public class UITextAnimator : AbstractMonoBehaviour
{
	// Token: 0x06000ABA RID: 2746 RVA: 0x000800EC File Offset: 0x0007E4EC
	protected override void Awake()
	{
		base.Awake();
		this.text = base.GetComponent<Text>();
		if (this.useTMP = (this.text == null))
		{
			this.tmp_text = base.GetComponent<TMP_Text>();
			this.textString = this.tmp_text.text;
		}
		else
		{
			this.textString = this.text.text;
		}
	}

	// Token: 0x06000ABB RID: 2747 RVA: 0x00080158 File Offset: 0x0007E558
	private void Start()
	{
		base.StartCoroutine(this.anim_cr());
	}

	// Token: 0x06000ABC RID: 2748 RVA: 0x00080167 File Offset: 0x0007E567
	public void SetString(string s)
	{
		this.textString = s;
	}

	// Token: 0x06000ABD RID: 2749 RVA: 0x00080170 File Offset: 0x0007E570
	private IEnumerator anim_cr()
	{
		if (this.useTMP)
		{
			for (;;)
			{
				this.tmp_text.text = string.Empty;
				for (int i = 0; i < this.textString.Length; i++)
				{
					TMP_Text tmp_Text = this.tmp_text;
					string text = tmp_Text.text;
					tmp_Text.text = string.Concat(new object[]
					{
						text,
						"<size=",
						this.tmp_text.fontSize + (float)UnityEngine.Random.Range(-1, 1),
						">",
						this.textString[i].ToString(),
						"</size>"
					});
				}
				yield return new WaitForSeconds(this.frameDelay);
			}
		}
		else
		{
			for (;;)
			{
				this.text.text = string.Empty;
				for (int j = 0; j < this.textString.Length; j++)
				{
					Text text2 = this.text;
					string text = text2.text;
					text2.text = string.Concat(new object[]
					{
						text,
						"<size=",
						this.text.fontSize + UnityEngine.Random.Range(-1, 1),
						">",
						this.textString[j].ToString(),
						"</size>"
					});
				}
				yield return new WaitForSeconds(this.frameDelay);
			}
		}
		yield break;
	}

	// Token: 0x0400148B RID: 5259
	private const int DIFFERENCE = 1;

	// Token: 0x0400148C RID: 5260
	[SerializeField]
	private float frameDelay = 0.07f;

	// Token: 0x0400148D RID: 5261
	private Text text;

	// Token: 0x0400148E RID: 5262
	private TMP_Text tmp_text;

	// Token: 0x0400148F RID: 5263
	private string textString;

	// Token: 0x04001490 RID: 5264
	private bool useTMP;
}
