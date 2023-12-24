using System;

namespace TMPro
{
	[Serializable]
	public class KerningPair
	{
		public KerningPair(int left, int right, float offset)
		{
		}

		public int AscII_Left;
		public int AscII_Right;
		public float XadvanceOffset;
	}
}
