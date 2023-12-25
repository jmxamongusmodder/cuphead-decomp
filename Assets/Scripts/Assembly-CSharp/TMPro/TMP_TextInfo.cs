using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000C8D RID: 3213
	[Serializable]
	public class TMP_TextInfo
	{
		// Token: 0x0600511F RID: 20767 RVA: 0x002961AC File Offset: 0x002945AC
		public TMP_TextInfo()
		{
			this.characterInfo = new TMP_CharacterInfo[8];
			this.wordInfo = new TMP_WordInfo[16];
			this.linkInfo = new TMP_LinkInfo[0];
			this.lineInfo = new TMP_LineInfo[2];
			this.pageInfo = new TMP_PageInfo[16];
			this.meshInfo = new TMP_MeshInfo[1];
		}

		// Token: 0x06005120 RID: 20768 RVA: 0x0029620C File Offset: 0x0029460C
		public TMP_TextInfo(TMP_Text textComponent)
		{
			this.textComponent = textComponent;
			this.characterInfo = new TMP_CharacterInfo[8];
			this.wordInfo = new TMP_WordInfo[4];
			this.linkInfo = new TMP_LinkInfo[0];
			this.lineInfo = new TMP_LineInfo[2];
			this.pageInfo = new TMP_PageInfo[16];
			this.meshInfo = new TMP_MeshInfo[1];
			this.meshInfo[0].mesh = textComponent.mesh;
			this.materialCount = 1;
		}

		// Token: 0x06005121 RID: 20769 RVA: 0x00296290 File Offset: 0x00294690
		public void Clear()
		{
			this.characterCount = 0;
			this.spaceCount = 0;
			this.wordCount = 0;
			this.linkCount = 0;
			this.lineCount = 0;
			this.pageCount = 0;
			this.spriteCount = 0;
			for (int i = 0; i < this.meshInfo.Length; i++)
			{
				this.meshInfo[i].vertexCount = 0;
			}
		}

		// Token: 0x06005122 RID: 20770 RVA: 0x002962FC File Offset: 0x002946FC
		public void ClearMeshInfo(bool updateMesh)
		{
			for (int i = 0; i < this.meshInfo.Length; i++)
			{
				this.meshInfo[i].Clear(updateMesh);
			}
		}

		// Token: 0x06005123 RID: 20771 RVA: 0x00296334 File Offset: 0x00294734
		public void ClearAllMeshInfo()
		{
			for (int i = 0; i < this.meshInfo.Length; i++)
			{
				this.meshInfo[i].Clear(true);
			}
		}

		// Token: 0x06005124 RID: 20772 RVA: 0x0029636C File Offset: 0x0029476C
		public void ClearUnusedVertices(MaterialReference[] materials)
		{
			for (int i = 0; i < this.meshInfo.Length; i++)
			{
				int startIndex = 0;
				this.meshInfo[i].ClearUnusedVertices(startIndex);
			}
		}

		// Token: 0x06005125 RID: 20773 RVA: 0x002963A8 File Offset: 0x002947A8
		public void ClearLineInfo()
		{
			if (this.lineInfo == null)
			{
				this.lineInfo = new TMP_LineInfo[2];
			}
			for (int i = 0; i < this.lineInfo.Length; i++)
			{
				this.lineInfo[i].characterCount = 0;
				this.lineInfo[i].spaceCount = 0;
				this.lineInfo[i].width = 0f;
				this.lineInfo[i].ascender = TMP_TextInfo.k_InfinityVectorNegative.x;
				this.lineInfo[i].descender = TMP_TextInfo.k_InfinityVectorPositive.x;
				this.lineInfo[i].lineExtents.min = TMP_TextInfo.k_InfinityVectorPositive;
				this.lineInfo[i].lineExtents.max = TMP_TextInfo.k_InfinityVectorNegative;
				this.lineInfo[i].maxAdvance = 0f;
			}
		}

		// Token: 0x06005126 RID: 20774 RVA: 0x002964A4 File Offset: 0x002948A4
		public static void Resize<T>(ref T[] array, int size)
		{
			int newSize = (size <= 1024) ? Mathf.NextPowerOfTwo(size) : (size + 256);
			Array.Resize<T>(ref array, newSize);
		}

		// Token: 0x06005127 RID: 20775 RVA: 0x002964D6 File Offset: 0x002948D6
		public static void Resize<T>(ref T[] array, int size, bool isBlockAllocated)
		{
			if (size <= array.Length)
			{
				return;
			}
			if (isBlockAllocated)
			{
				size = ((size <= 1024) ? Mathf.NextPowerOfTwo(size) : (size + 256));
			}
			Array.Resize<T>(ref array, size);
		}

		// Token: 0x040053D2 RID: 21458
		private static Vector2 k_InfinityVectorPositive = new Vector2(1000000f, 1000000f);

		// Token: 0x040053D3 RID: 21459
		private static Vector2 k_InfinityVectorNegative = new Vector2(-1000000f, -1000000f);

		// Token: 0x040053D4 RID: 21460
		public TMP_Text textComponent;

		// Token: 0x040053D5 RID: 21461
		public int characterCount;

		// Token: 0x040053D6 RID: 21462
		public int spriteCount;

		// Token: 0x040053D7 RID: 21463
		public int spaceCount;

		// Token: 0x040053D8 RID: 21464
		public int wordCount;

		// Token: 0x040053D9 RID: 21465
		public int linkCount;

		// Token: 0x040053DA RID: 21466
		public int lineCount;

		// Token: 0x040053DB RID: 21467
		public int pageCount;

		// Token: 0x040053DC RID: 21468
		public int materialCount;

		// Token: 0x040053DD RID: 21469
		public TMP_CharacterInfo[] characterInfo;

		// Token: 0x040053DE RID: 21470
		public TMP_WordInfo[] wordInfo;

		// Token: 0x040053DF RID: 21471
		public TMP_LinkInfo[] linkInfo;

		// Token: 0x040053E0 RID: 21472
		public TMP_LineInfo[] lineInfo;

		// Token: 0x040053E1 RID: 21473
		public TMP_PageInfo[] pageInfo;

		// Token: 0x040053E2 RID: 21474
		public TMP_MeshInfo[] meshInfo;
	}
}
