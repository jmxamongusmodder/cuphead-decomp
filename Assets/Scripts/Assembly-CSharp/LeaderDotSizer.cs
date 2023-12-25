using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200045F RID: 1119
public class LeaderDotSizer : MonoBehaviour
{
	// Token: 0x060010FB RID: 4347 RVA: 0x000A271F File Offset: 0x000A0B1F
	private void Start()
	{
		this.SetLeaderDots();
	}

	// Token: 0x060010FC RID: 4348 RVA: 0x000A2728 File Offset: 0x000A0B28
	private void SetLeaderDots()
	{
		this.leaderDotText.text = ". . . . . . . . . . . . . . . . . . . . . . .";
		float num = this.leaderDotText.rectTransform.sizeDelta.x - this.descriptionText.preferredWidth;
		if (num < 0f)
		{
			this.leaderDotText.text = string.Empty;
			return;
		}
		int num2 = 100000;
		while (this.leaderDotText.text.Length > 2 && this.leaderDotText.preferredWidth > num && num2 > 0)
		{
			num2--;
			this.leaderDotText.text = this.leaderDotText.text.Substring(0, this.leaderDotText.text.Length - 2);
		}
	}

	// Token: 0x04001A62 RID: 6754
	private const string Dots = ". . . . . . . . . . . . . . . . . . . . . . .";

	// Token: 0x04001A63 RID: 6755
	private const float DotsPadding = 5f;

	// Token: 0x04001A64 RID: 6756
	[SerializeField]
	private TextMeshProUGUI descriptionText;

	// Token: 0x04001A65 RID: 6757
	[SerializeField]
	private Text leaderDotText;
}
