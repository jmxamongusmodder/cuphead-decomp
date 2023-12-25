using System;
using UnityEngine;

// Token: 0x020006FD RID: 1789
public class OldManLevelBeardController : MonoBehaviour
{
	// Token: 0x0600264E RID: 9806 RVA: 0x00165E60 File Offset: 0x00164260
	public void CueRuffle(int which)
	{
		if (this.ruffles[which] != null && this.ruffles[which].GetCurrentAnimatorStateInfo(0).IsName("None"))
		{
			this.ruffleCued[which] = true;
		}
	}

	// Token: 0x0600264F RID: 9807 RVA: 0x00165EA9 File Offset: 0x001642A9
	private void FixedUpdate()
	{
		this.frameTimer += CupheadTime.FixedDelta;
		if (this.frameTimer > 0.041666668f)
		{
			this.frameTimer -= 0.041666668f;
			this.Step();
		}
	}

	// Token: 0x06002650 RID: 9808 RVA: 0x00165EE5 File Offset: 0x001642E5
	private void Step()
	{
		this.frameNum = (this.frameNum + 1) % 6;
		this.rend.sprite = this.sprites[this.frameNum / 2];
	}

	// Token: 0x06002651 RID: 9809 RVA: 0x00165F14 File Offset: 0x00164314
	private void LateUpdate()
	{
		for (int i = 0; i < this.ruffleCued.Length; i++)
		{
			if (this.ruffleCued[i])
			{
				int num = this.ruffleStartFrames[i] - this.frameNum;
				if (num == 0 || num == 1)
				{
					float num2 = this.frameTimer * 24f;
					float normalizedTime = ((float)num + num2) * 0.071428575f;
					this.ruffles[i].Play("Ruffle", 0, normalizedTime);
					this.ruffles[i].Update(0f);
					this.ruffleCued[i] = false;
				}
			}
		}
	}

	// Token: 0x04002ED4 RID: 11988
	private const float RUFFLE_FRAME_TIME_NORMALIZED = 0.071428575f;

	// Token: 0x04002ED5 RID: 11989
	[SerializeField]
	private SpriteRenderer rend;

	// Token: 0x04002ED6 RID: 11990
	[SerializeField]
	private Sprite[] sprites;

	// Token: 0x04002ED7 RID: 11991
	[SerializeField]
	private Animator[] ruffles;

	// Token: 0x04002ED8 RID: 11992
	[SerializeField]
	private int[] ruffleStartFrames;

	// Token: 0x04002ED9 RID: 11993
	private bool[] ruffleCued = new bool[10];

	// Token: 0x04002EDA RID: 11994
	private float frameTimer;

	// Token: 0x04002EDB RID: 11995
	private int frameNum;
}
