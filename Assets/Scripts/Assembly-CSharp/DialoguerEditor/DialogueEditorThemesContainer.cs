using System;
using System.Collections.Generic;

namespace DialoguerEditor
{
	// Token: 0x02000B4C RID: 2892
	[Serializable]
	public class DialogueEditorThemesContainer
	{
		// Token: 0x060045E6 RID: 17894 RVA: 0x0024D728 File Offset: 0x0024BB28
		public DialogueEditorThemesContainer()
		{
			this.themes = new List<DialogueEditorThemeObject>();
		}

		// Token: 0x060045E7 RID: 17895 RVA: 0x0024D73C File Offset: 0x0024BB3C
		public void addTheme()
		{
			int count = this.themes.Count;
			this.themes.Add(new DialogueEditorThemeObject());
			this.themes[count].id = count;
		}

		// Token: 0x060045E8 RID: 17896 RVA: 0x0024D778 File Offset: 0x0024BB78
		public void removeTheme()
		{
			this.themes.RemoveAt(this.themes.Count - 1);
			if (this.selection >= this.themes.Count)
			{
				this.selection = this.themes.Count - 1;
			}
		}

		// Token: 0x04004C27 RID: 19495
		public List<DialogueEditorThemeObject> themes;

		// Token: 0x04004C28 RID: 19496
		public int selection;
	}
}
