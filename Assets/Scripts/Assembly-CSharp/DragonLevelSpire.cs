using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000600 RID: 1536
public class DragonLevelSpire : AbstractPausableComponent
{
	// Token: 0x06001E8A RID: 7818 RVA: 0x001197BE File Offset: 0x00117BBE
	private void Start()
	{
		this.helper = base.GetComponent<AnimationHelper>();
		this.helper.Speed = 0f;
		this.fadeTime = 3f;
		this.replacementSprite.enabled = false;
	}

	// Token: 0x06001E8B RID: 7819 RVA: 0x001197F3 File Offset: 0x00117BF3
	private void Update()
	{
		this.helper.Speed = DragonLevel.SPEED;
	}

	// Token: 0x06001E8C RID: 7820 RVA: 0x00119805 File Offset: 0x00117C05
	public void StartChange()
	{
		base.StartCoroutine(this.change_cr());
	}

	// Token: 0x06001E8D RID: 7821 RVA: 0x00119814 File Offset: 0x00117C14
	private IEnumerator change_cr()
	{
		float t = 0f;
		while (t < this.fadeTime)
		{
			this.replacementSprite.enabled = true;
			this.replacementSprite.color = new Color(1f, 1f, 1f, t / this.fadeTime);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.replacementSprite.color = new Color(1f, 1f, 1f, 1f);
		yield return null;
		yield break;
	}

	// Token: 0x04002763 RID: 10083
	private AnimationHelper helper;

	// Token: 0x04002764 RID: 10084
	[SerializeField]
	private SpriteRenderer replacementSprite;

	// Token: 0x04002765 RID: 10085
	private float fadeTime;
}
