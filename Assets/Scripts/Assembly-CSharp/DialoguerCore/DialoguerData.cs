using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace DialoguerCore
{
	// Token: 0x02000B6D RID: 2925
	public class DialoguerData
	{
		// Token: 0x06004694 RID: 18068 RVA: 0x0024E6CE File Offset: 0x0024CACE
		public DialoguerData(DialoguerGlobalVariables globalVariables, List<DialoguerDialogue> dialogues, List<DialoguerTheme> themes)
		{
			this.globalVariables = globalVariables;
			this.dialogues = dialogues;
			this.themes = themes;
		}

		// Token: 0x06004695 RID: 18069 RVA: 0x0024E6EC File Offset: 0x0024CAEC
		public void loadGlobalVariablesState(string globalVariablesXml)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(DialoguerGlobalVariables));
			XmlReader xmlReader = XmlReader.Create(new StringReader(globalVariablesXml));
			DialoguerGlobalVariables dialoguerGlobalVariables = (DialoguerGlobalVariables)xmlSerializer.Deserialize(xmlReader);
			for (int i = 0; i < dialoguerGlobalVariables.booleans.Count; i++)
			{
				if (i >= this.globalVariables.booleans.Count)
				{
					break;
				}
				this.globalVariables.booleans[i] = dialoguerGlobalVariables.booleans[i];
			}
			for (int j = 0; j < dialoguerGlobalVariables.floats.Count; j++)
			{
				if (j >= this.globalVariables.floats.Count)
				{
					break;
				}
				this.globalVariables.floats[j] = dialoguerGlobalVariables.floats[j];
			}
			for (int k = 0; k < dialoguerGlobalVariables.strings.Count; k++)
			{
				if (k >= this.globalVariables.strings.Count)
				{
					break;
				}
				this.globalVariables.strings[k] = dialoguerGlobalVariables.strings[k];
			}
		}

		// Token: 0x04004C70 RID: 19568
		public readonly DialoguerGlobalVariables globalVariables;

		// Token: 0x04004C71 RID: 19569
		public readonly List<DialoguerDialogue> dialogues;

		// Token: 0x04004C72 RID: 19570
		public readonly List<DialoguerTheme> themes;
	}
}
