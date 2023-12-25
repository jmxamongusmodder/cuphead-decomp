using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B0D RID: 2829
public class AnimatedUISprite : MonoBehaviour
{
	// Token: 0x060044A7 RID: 17575 RVA: 0x00246165 File Offset: 0x00244565
	public void ResetAnimation()
	{
		this._currentSpriteIndex = 0;
	}

	// Token: 0x060044A8 RID: 17576 RVA: 0x0024616E File Offset: 0x0024456E
	private void OnEnable()
	{
		this.ResetAnimation();
	}

	// Token: 0x060044A9 RID: 17577 RVA: 0x00246176 File Offset: 0x00244576
	private void OnDisable()
	{
		this.ResetAnimation();
	}

	// Token: 0x060044AA RID: 17578 RVA: 0x00246180 File Offset: 0x00244580
	private void Update()
	{
		if (!this.Loop && this._currentSpriteIndex >= this.Sprites.Length - 1)
		{
			return;
		}
		if (this.Animating && Time.time >= this._lastRefreshTime + 1f / (float)this.FrameRate)
		{
			this._currentSpriteIndex++;
			if (this._currentSpriteIndex >= this.Sprites.Length)
			{
				this._currentSpriteIndex = 0;
			}
			this.UIImage.sprite = this.Sprites[this._currentSpriteIndex];
			this._lastRefreshTime = Time.time;
		}
	}

	// Token: 0x04004A60 RID: 19040
	public bool Animating;

	// Token: 0x04004A61 RID: 19041
	public bool Loop;

	// Token: 0x04004A62 RID: 19042
	public Image UIImage;

	// Token: 0x04004A63 RID: 19043
	public Sprite[] Sprites;

	// Token: 0x04004A64 RID: 19044
	public int FrameRate = 24;

	// Token: 0x04004A65 RID: 19045
	private int _currentSpriteIndex;

	// Token: 0x04004A66 RID: 19046
	private float _lastRefreshTime;
}
