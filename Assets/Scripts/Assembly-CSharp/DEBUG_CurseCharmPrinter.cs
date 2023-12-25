using System;
using UnityEngine;

// Token: 0x02000B39 RID: 2873
public class DEBUG_CurseCharmPrinter : MonoBehaviour
{
	// Token: 0x060045A0 RID: 17824 RVA: 0x0024BCF4 File Offset: 0x0024A0F4
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x060045A1 RID: 17825 RVA: 0x0024BD04 File Offset: 0x0024A104
	private void OnGUI()
	{
		if (Time.frameCount < 120)
		{
			return;
		}
		if (this.style == null)
		{
			this.style = new GUIStyle(GUI.skin.GetStyle("Box"));
			this.style.alignment = TextAnchor.UpperLeft;
		}
		if (PlayerData.Data == null)
		{
			return;
		}
		string text = string.Format("Curse: {0} / {1} / {2}", CharmCurse.CalculateLevel(PlayerId.PlayerOne), PlayerData.Data.CalculateCurseCharmAccumulatedValue(PlayerId.PlayerOne, CharmCurse.CountableLevels), CharmCurse.IsMaxLevel(PlayerId.PlayerOne));
		GUI.Box(new Rect(0f, 0f, 200f, 100f), text);
	}

	// Token: 0x04004BCB RID: 19403
	private GUIStyle style;
}
