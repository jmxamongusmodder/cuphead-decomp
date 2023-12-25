using System;
using System.IO;
using System.Xml.Serialization;
using DialoguerEditor;
using UnityEngine;

namespace DialoguerCore
{
	// Token: 0x02000B6B RID: 2923
	public class DialoguerDataManager
	{
		// Token: 0x06004686 RID: 18054 RVA: 0x0024E4B8 File Offset: 0x0024C8B8
		public static void Initialize()
		{
			DialogueEditorMasterObject data = (Resources.Load("dialoguer_data_object") as DialogueEditorMasterObjectWrapper).data;
			DialoguerDataManager._data = data.getDialoguerData();
		}

		// Token: 0x06004687 RID: 18055 RVA: 0x0024E4E8 File Offset: 0x0024C8E8
		public static string GetGlobalVariablesState()
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(DialoguerGlobalVariables));
			StringWriter stringWriter = new StringWriter();
			xmlSerializer.Serialize(stringWriter, DialoguerDataManager._data.globalVariables);
			return stringWriter.ToString();
		}

		// Token: 0x06004688 RID: 18056 RVA: 0x0024E522 File Offset: 0x0024C922
		public static void LoadGlobalVariablesState(string globalVariablesXml)
		{
			DialoguerDataManager._data.loadGlobalVariablesState(globalVariablesXml);
		}

		// Token: 0x06004689 RID: 18057 RVA: 0x0024E52F File Offset: 0x0024C92F
		public static float GetGlobalFloat(int floatId)
		{
			return DialoguerDataManager._data.globalVariables.floats[floatId];
		}

		// Token: 0x0600468A RID: 18058 RVA: 0x0024E546 File Offset: 0x0024C946
		public static void SetGlobalFloat(int floatId, float floatValue)
		{
			DialoguerDataManager._data.globalVariables.floats[floatId] = floatValue;
		}

		// Token: 0x0600468B RID: 18059 RVA: 0x0024E55E File Offset: 0x0024C95E
		public static bool GetGlobalBoolean(int booleanId)
		{
			return DialoguerDataManager._data.globalVariables.booleans[booleanId];
		}

		// Token: 0x0600468C RID: 18060 RVA: 0x0024E575 File Offset: 0x0024C975
		public static void SetGlobalBoolean(int booleanId, bool booleanValue)
		{
			DialoguerDataManager._data.globalVariables.booleans[booleanId] = booleanValue;
		}

		// Token: 0x0600468D RID: 18061 RVA: 0x0024E58D File Offset: 0x0024C98D
		public static string GetGlobalString(int stringId)
		{
			return DialoguerDataManager._data.globalVariables.strings[stringId];
		}

		// Token: 0x0600468E RID: 18062 RVA: 0x0024E5A4 File Offset: 0x0024C9A4
		public static void SetGlobalString(int stringId, string stringValue)
		{
			DialoguerDataManager._data.globalVariables.strings[stringId] = stringValue;
		}

		// Token: 0x0600468F RID: 18063 RVA: 0x0024E5BC File Offset: 0x0024C9BC
		public static DialoguerDialogue GetDialogueById(int dialogueId)
		{
			if (DialoguerDataManager._data.dialogues.Count <= dialogueId)
			{
				return null;
			}
			return DialoguerDataManager._data.dialogues[dialogueId];
		}

		// Token: 0x04004C6A RID: 19562
		private static DialoguerData _data;
	}
}
