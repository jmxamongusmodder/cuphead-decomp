using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000B3D RID: 2877
public class DEBUG_LocalAchievementsPrinter : MonoBehaviour
{
	// Token: 0x060045AD RID: 17837 RVA: 0x0024C177 File Offset: 0x0024A577
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x060045AE RID: 17838 RVA: 0x0024C184 File Offset: 0x0024A584
	private void OnGUI()
	{
		if (Time.frameCount < 120)
		{
			return;
		}
		IList<LocalAchievementsManager.Achievement> unlockedAchievements = LocalAchievementsManager.GetUnlockedAchievements();
		foreach (string text in DEBUG_LocalAchievementsPrinter.AllAchievements)
		{
			bool flag = unlockedAchievements.Contains((LocalAchievementsManager.Achievement)Enum.Parse(typeof(LocalAchievementsManager.Achievement), text));
			this.builder.AppendFormat("{0}....{1}\n", (!flag) ? "L" : "U", text);
		}
		GUIStyle guistyle = new GUIStyle(GUI.skin.GetStyle("Box"));
		guistyle.alignment = TextAnchor.UpperLeft;
		GUI.Box(new Rect(0f, 0f, 200f, 500f), this.builder.ToString(), guistyle);
		this.builder.Length = 0;
	}

	// Token: 0x04004BDA RID: 19418
	private static readonly string[] AllAchievements = Enum.GetNames(typeof(LocalAchievementsManager.Achievement));

	// Token: 0x04004BDB RID: 19419
	private StringBuilder builder = new StringBuilder();
}
