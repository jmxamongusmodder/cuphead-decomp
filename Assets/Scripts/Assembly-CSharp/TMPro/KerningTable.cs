using System;
using System.Collections.Generic;
using System.Linq;

namespace TMPro
{
	// Token: 0x02000CA0 RID: 3232
	[Serializable]
	public class KerningTable
	{
		// Token: 0x06005180 RID: 20864 RVA: 0x00299691 File Offset: 0x00297A91
		public KerningTable()
		{
			this.kerningPairs = new List<KerningPair>();
		}

		// Token: 0x06005181 RID: 20865 RVA: 0x002996A4 File Offset: 0x00297AA4
		public void AddKerningPair()
		{
			if (this.kerningPairs.Count == 0)
			{
				this.kerningPairs.Add(new KerningPair(0, 0, 0f));
			}
			else
			{
				int ascII_Left = this.kerningPairs.Last<KerningPair>().AscII_Left;
				int ascII_Right = this.kerningPairs.Last<KerningPair>().AscII_Right;
				float xadvanceOffset = this.kerningPairs.Last<KerningPair>().XadvanceOffset;
				this.kerningPairs.Add(new KerningPair(ascII_Left, ascII_Right, xadvanceOffset));
			}
		}

		// Token: 0x06005182 RID: 20866 RVA: 0x00299724 File Offset: 0x00297B24
		public int AddKerningPair(int left, int right, float offset)
		{
			int num = this.kerningPairs.FindIndex((KerningPair item) => item.AscII_Left == left && item.AscII_Right == right);
			if (num == -1)
			{
				this.kerningPairs.Add(new KerningPair(left, right, offset));
				return 0;
			}
			return -1;
		}

		// Token: 0x06005183 RID: 20867 RVA: 0x00299784 File Offset: 0x00297B84
		public void RemoveKerningPair(int left, int right)
		{
			int num = this.kerningPairs.FindIndex((KerningPair item) => item.AscII_Left == left && item.AscII_Right == right);
			if (num != -1)
			{
				this.kerningPairs.RemoveAt(num);
			}
		}

		// Token: 0x06005184 RID: 20868 RVA: 0x002997D0 File Offset: 0x00297BD0
		public void RemoveKerningPair(int index)
		{
			this.kerningPairs.RemoveAt(index);
		}

		// Token: 0x06005185 RID: 20869 RVA: 0x002997E0 File Offset: 0x00297BE0
		public void SortKerningPairs()
		{
			if (this.kerningPairs.Count > 0)
			{
				this.kerningPairs = (from s in this.kerningPairs
				orderby s.AscII_Left, s.AscII_Right
				select s).ToList<KerningPair>();
			}
		}

		// Token: 0x04005439 RID: 21561
		public List<KerningPair> kerningPairs;
	}
}
