using System;

namespace TMPro
{
	// Token: 0x02000CA9 RID: 3241
	public struct TMP_WordInfo
	{
		// Token: 0x06005196 RID: 20886 RVA: 0x00299FC4 File Offset: 0x002983C4
		public string GetWord()
		{
			string text = string.Empty;
			TMP_CharacterInfo[] characterInfo = this.textComponent.textInfo.characterInfo;
			for (int i = this.firstCharacterIndex; i < this.lastCharacterIndex + 1; i++)
			{
				text += characterInfo[i].character;
			}
			return text;
		}

		// Token: 0x04005486 RID: 21638
		public TMP_Text textComponent;

		// Token: 0x04005487 RID: 21639
		public int firstCharacterIndex;

		// Token: 0x04005488 RID: 21640
		public int lastCharacterIndex;

		// Token: 0x04005489 RID: 21641
		public int characterCount;
	}
}
