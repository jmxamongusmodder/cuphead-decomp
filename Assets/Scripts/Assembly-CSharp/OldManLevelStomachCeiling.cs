using System;
using UnityEngine;

// Token: 0x02000715 RID: 1813
public class OldManLevelStomachCeiling : MonoBehaviour
{
	// Token: 0x0600276B RID: 10091 RVA: 0x00172140 File Offset: 0x00170540
	private void Update()
	{
		this.currentPosition = (int)(Mathf.Clamp(this.leader.GetPosition(), 0f, 0.99f) * (float)this.sprites.Length);
		this.timeSinceChangeFrame += CupheadTime.Delta;
		if (this.lastPosition != this.currentPosition)
		{
			this.lastPosition = this.currentPosition;
			this.timeSinceChangeFrame = 0f;
		}
		this.offset = (int)(this.timeSinceChangeFrame / 0.16666667f) % 2 * (int)(-(int)Mathf.Sign(this.leader.GetPosition() - 0.5f));
		this.rend.sprite = this.sprites[this.currentPosition + this.offset];
	}

	// Token: 0x04003025 RID: 12325
	private const float MAX_FRAME_TIME = 0.16666667f;

	// Token: 0x04003026 RID: 12326
	[SerializeField]
	private SpriteRenderer rend;

	// Token: 0x04003027 RID: 12327
	[SerializeField]
	private Sprite[] sprites;

	// Token: 0x04003028 RID: 12328
	[SerializeField]
	private OldManLevelGnomeLeader leader;

	// Token: 0x04003029 RID: 12329
	private int currentPosition;

	// Token: 0x0400302A RID: 12330
	private int lastPosition;

	// Token: 0x0400302B RID: 12331
	private float timeSinceChangeFrame;

	// Token: 0x0400302C RID: 12332
	private int offset;
}
