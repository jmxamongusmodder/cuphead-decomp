using System;
using UnityEngine;

// Token: 0x02000B8D RID: 2957
[Serializable]
public class NextGenRingPieces
{
	// Token: 0x0600480B RID: 18443 RVA: 0x0025D7DC File Offset: 0x0025BBDC
	public Texture[] getPieces()
	{
		if (this._pieces == null)
		{
			this._pieces = new Texture[6];
			this._pieces[0] = this.topRight;
			this._pieces[1] = this.middleRight;
			this._pieces[2] = this.bottomRight;
			this._pieces[3] = this.topLeft;
			this._pieces[4] = this.middleLeft;
			this._pieces[5] = this.bottomLeft;
		}
		return this._pieces;
	}

	// Token: 0x04004D42 RID: 19778
	public Texture topLeft;

	// Token: 0x04004D43 RID: 19779
	public Texture topRight;

	// Token: 0x04004D44 RID: 19780
	public Texture middleLeft;

	// Token: 0x04004D45 RID: 19781
	public Texture middleRight;

	// Token: 0x04004D46 RID: 19782
	public Texture bottomLeft;

	// Token: 0x04004D47 RID: 19783
	public Texture bottomRight;

	// Token: 0x04004D48 RID: 19784
	private Texture[] _pieces;
}
