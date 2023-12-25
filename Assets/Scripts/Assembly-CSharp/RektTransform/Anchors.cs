using System;

namespace RektTransform
{
	// Token: 0x0200036B RID: 875
	public static class Anchors
	{
		// Token: 0x04001449 RID: 5193
		public static readonly MinMax TopLeft = new MinMax(0f, 1f, 0f, 1f);

		// Token: 0x0400144A RID: 5194
		public static readonly MinMax TopCenter = new MinMax(0.5f, 1f, 0.5f, 1f);

		// Token: 0x0400144B RID: 5195
		public static readonly MinMax TopRight = new MinMax(1f, 1f, 1f, 1f);

		// Token: 0x0400144C RID: 5196
		public static readonly MinMax TopStretch = new MinMax(0f, 1f, 1f, 1f);

		// Token: 0x0400144D RID: 5197
		public static readonly MinMax MiddleLeft = new MinMax(0f, 0.5f, 0f, 0.5f);

		// Token: 0x0400144E RID: 5198
		public static readonly MinMax TrueCenter = new MinMax(0.5f, 0.5f, 0.5f, 0.5f);

		// Token: 0x0400144F RID: 5199
		public static readonly MinMax MiddleCenter = new MinMax(0.5f, 0.5f, 0.5f, 0.5f);

		// Token: 0x04001450 RID: 5200
		public static readonly MinMax MiddleRight = new MinMax(1f, 0.5f, 1f, 0.5f);

		// Token: 0x04001451 RID: 5201
		public static readonly MinMax MiddleStretch = new MinMax(0f, 0.5f, 1f, 0.5f);

		// Token: 0x04001452 RID: 5202
		public static readonly MinMax BottomLeft = new MinMax(0f, 0f, 0f, 0f);

		// Token: 0x04001453 RID: 5203
		public static readonly MinMax BottomCenter = new MinMax(0.5f, 0f, 0.5f, 0f);

		// Token: 0x04001454 RID: 5204
		public static readonly MinMax BottomRight = new MinMax(1f, 0f, 1f, 0f);

		// Token: 0x04001455 RID: 5205
		public static readonly MinMax BottomStretch = new MinMax(0f, 0f, 1f, 0f);

		// Token: 0x04001456 RID: 5206
		public static readonly MinMax StretchLeft = new MinMax(0f, 0f, 0f, 1f);

		// Token: 0x04001457 RID: 5207
		public static readonly MinMax StretchCenter = new MinMax(0.5f, 0f, 0.5f, 1f);

		// Token: 0x04001458 RID: 5208
		public static readonly MinMax StretchRight = new MinMax(1f, 0f, 1f, 1f);

		// Token: 0x04001459 RID: 5209
		public static readonly MinMax TrueStretch = new MinMax(0f, 0f, 1f, 1f);

		// Token: 0x0400145A RID: 5210
		public static readonly MinMax StretchStretch = new MinMax(0f, 0f, 1f, 1f);
	}
}
