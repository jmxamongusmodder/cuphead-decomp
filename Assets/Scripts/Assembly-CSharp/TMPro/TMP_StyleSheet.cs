using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000C7B RID: 3195
	[Serializable]
	public class TMP_StyleSheet : ScriptableObject
	{
		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x06005011 RID: 20497 RVA: 0x002955AC File Offset: 0x002939AC
		public static TMP_StyleSheet instance
		{
			get
			{
				if (TMP_StyleSheet.s_Instance == null)
				{
					TMP_StyleSheet.s_Instance = TMP_Settings.defaultStyleSheet;
					if (TMP_StyleSheet.s_Instance == null)
					{
						TMP_StyleSheet.s_Instance = (Resources.Load("Style Sheets/TMP Default Style Sheet") as TMP_StyleSheet);
					}
					if (TMP_StyleSheet.s_Instance == null)
					{
						return null;
					}
					TMP_StyleSheet.s_Instance.LoadStyleDictionaryInternal();
				}
				return TMP_StyleSheet.s_Instance;
			}
		}

		// Token: 0x06005012 RID: 20498 RVA: 0x00295618 File Offset: 0x00293A18
		public static TMP_StyleSheet LoadDefaultStyleSheet()
		{
			return TMP_StyleSheet.instance;
		}

		// Token: 0x06005013 RID: 20499 RVA: 0x0029561F File Offset: 0x00293A1F
		public static TMP_Style GetStyle(int hashCode)
		{
			return TMP_StyleSheet.instance.GetStyleInternal(hashCode);
		}

		// Token: 0x06005014 RID: 20500 RVA: 0x0029562C File Offset: 0x00293A2C
		private TMP_Style GetStyleInternal(int hashCode)
		{
			TMP_Style result;
			if (this.m_StyleDictionary.TryGetValue(hashCode, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06005015 RID: 20501 RVA: 0x00295650 File Offset: 0x00293A50
		public void UpdateStyleDictionaryKey(int old_key, int new_key)
		{
			if (this.m_StyleDictionary.ContainsKey(old_key))
			{
				TMP_Style value = this.m_StyleDictionary[old_key];
				this.m_StyleDictionary.Add(new_key, value);
				this.m_StyleDictionary.Remove(old_key);
			}
		}

		// Token: 0x06005016 RID: 20502 RVA: 0x00295695 File Offset: 0x00293A95
		public static void RefreshStyles()
		{
			TMP_StyleSheet.s_Instance.LoadStyleDictionaryInternal();
		}

		// Token: 0x06005017 RID: 20503 RVA: 0x002956A4 File Offset: 0x00293AA4
		private void LoadStyleDictionaryInternal()
		{
			this.m_StyleDictionary.Clear();
			for (int i = 0; i < this.m_StyleList.Count; i++)
			{
				this.m_StyleList[i].RefreshStyle();
				if (!this.m_StyleDictionary.ContainsKey(this.m_StyleList[i].hashCode))
				{
					this.m_StyleDictionary.Add(this.m_StyleList[i].hashCode, this.m_StyleList[i]);
				}
			}
		}

		// Token: 0x040052BA RID: 21178
		private static TMP_StyleSheet s_Instance;

		// Token: 0x040052BB RID: 21179
		[SerializeField]
		private List<TMP_Style> m_StyleList = new List<TMP_Style>(1);

		// Token: 0x040052BC RID: 21180
		private Dictionary<int, TMP_Style> m_StyleDictionary = new Dictionary<int, TMP_Style>();
	}
}
