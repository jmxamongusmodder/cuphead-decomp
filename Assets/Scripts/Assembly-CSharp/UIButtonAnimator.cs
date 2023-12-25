using System;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;

// Token: 0x02000386 RID: 902
public class UIButtonAnimator : AbstractMonoBehaviour
{
	// Token: 0x17000202 RID: 514
	// (get) Token: 0x06000AAF RID: 2735 RVA: 0x0007FAE6 File Offset: 0x0007DEE6
	// (set) Token: 0x06000AB0 RID: 2736 RVA: 0x0007FAF3 File Offset: 0x0007DEF3
	protected string Text
	{
		get
		{
			return this.tmpText.text;
		}
		set
		{
			this.tmpText.SetText(value);
		}
	}

	// Token: 0x06000AB1 RID: 2737 RVA: 0x0007FB01 File Offset: 0x0007DF01
	private void Start()
	{
		this.tmpText = base.GetComponent<TextMeshProUGUI>();
		base.StartCoroutine(this.animate_cr());
	}

	// Token: 0x06000AB2 RID: 2738 RVA: 0x0007FB1C File Offset: 0x0007DF1C
	private IEnumerator animate_cr()
	{
		yield return null;
		yield return null;
		string first = this.Text;
		string second = this.Text;
		MatchCollection keys = Regex.Matches(this.Text, "{([^}]*)}", RegexOptions.Multiline | RegexOptions.ExplicitCapture);
		CupheadButton[] buttons = new CupheadButton[keys.Count];
		for (int i = 0; i < keys.Count; i++)
		{
			buttons[i] = (CupheadButton)Enum.Parse(typeof(CupheadButton), keys[i].Value.Substring(1, keys[i].Value.Length - 2));
		}
		for (int j = 0; j < CupheadInput.pairs.Length; j++)
		{
			for (int k = 0; k < buttons.Length; k++)
			{
				CupheadInput.InputSymbols inputSymbols = CupheadInput.InputSymbolForButton(buttons[k]);
				if (inputSymbols == CupheadInput.pairs[j].symbol)
				{
					first = first.Replace("{" + buttons[k].ToString() + "}", CupheadInput.pairs[j].first);
				}
			}
		}
		for (int l = 0; l < CupheadInput.pairs.Length; l++)
		{
			for (int m = 0; m < buttons.Length; m++)
			{
				CupheadInput.InputSymbols inputSymbols2 = CupheadInput.InputSymbolForButton(buttons[m]);
				if (inputSymbols2 == CupheadInput.pairs[l].symbol)
				{
					second = second.Replace("{" + buttons[m].ToString() + "}", CupheadInput.pairs[l].second);
				}
			}
		}
		this.Text = first;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, 0.4f);
			this.Text = second;
			yield return CupheadTime.WaitForSeconds(this, 0.4f);
			this.Text = first;
		}
		yield break;
	}

	// Token: 0x04001480 RID: 5248
	private const float FRAME_DELAY = 0.4f;

	// Token: 0x04001481 RID: 5249
	private TextMeshProUGUI tmpText;
}
