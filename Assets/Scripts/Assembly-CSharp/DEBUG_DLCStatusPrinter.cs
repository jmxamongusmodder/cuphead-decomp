using System;
using UnityEngine;

// Token: 0x02000B3A RID: 2874
public class DEBUG_DLCStatusPrinter : MonoBehaviour
{
	// Token: 0x060045A3 RID: 17827 RVA: 0x0024BDB7 File Offset: 0x0024A1B7
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x060045A4 RID: 17828 RVA: 0x0024BDC4 File Offset: 0x0024A1C4
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
		GUI.Box(new Rect(0f, 0f, 200f, 100f), "DLC Enabled: " + DLCManager.DLCEnabled());
	}

	// Token: 0x04004BCC RID: 19404
	private GUIStyle style;
}
