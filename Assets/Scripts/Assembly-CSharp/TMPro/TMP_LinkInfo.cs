using System;

namespace TMPro
{
	// Token: 0x02000CA8 RID: 3240
	public struct TMP_LinkInfo
	{
		// Token: 0x06005193 RID: 20883 RVA: 0x00299EEC File Offset: 0x002982EC
		internal void SetLinkID(char[] text, int startIndex, int length)
		{
			if (this.linkID == null || this.linkID.Length < length)
			{
				this.linkID = new char[length];
			}
			for (int i = 0; i < length; i++)
			{
				this.linkID[i] = text[startIndex + i];
			}
		}

		// Token: 0x06005194 RID: 20884 RVA: 0x00299F40 File Offset: 0x00298340
		public string GetLinkText()
		{
			string text = string.Empty;
			TMP_TextInfo textInfo = this.textComponent.textInfo;
			for (int i = this.linkTextfirstCharacterIndex; i < this.linkTextfirstCharacterIndex + this.linkTextLength; i++)
			{
				text += textInfo.characterInfo[i].character;
			}
			return text;
		}

		// Token: 0x06005195 RID: 20885 RVA: 0x00299FA0 File Offset: 0x002983A0
		public string GetLinkID()
		{
			if (this.textComponent == null)
			{
				return string.Empty;
			}
			return new string(this.linkID);
		}

		// Token: 0x0400547F RID: 21631
		public TMP_Text textComponent;

		// Token: 0x04005480 RID: 21632
		public int hashCode;

		// Token: 0x04005481 RID: 21633
		public int linkIdFirstCharacterIndex;

		// Token: 0x04005482 RID: 21634
		public int linkIdLength;

		// Token: 0x04005483 RID: 21635
		public int linkTextfirstCharacterIndex;

		// Token: 0x04005484 RID: 21636
		public int linkTextLength;

		// Token: 0x04005485 RID: 21637
		internal char[] linkID;
	}
}
