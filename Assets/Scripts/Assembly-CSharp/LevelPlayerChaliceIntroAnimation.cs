using System;
using UnityEngine;

// Token: 0x02000A18 RID: 2584
public class LevelPlayerChaliceIntroAnimation : Effect
{
	// Token: 0x06003D42 RID: 15682 RVA: 0x0021E410 File Offset: 0x0021C810
	public LevelPlayerChaliceIntroAnimation Create(Vector3 position, bool isMugman, bool isScared)
	{
		LevelPlayerChaliceIntroAnimation levelPlayerChaliceIntroAnimation = base.Create(position) as LevelPlayerChaliceIntroAnimation;
		levelPlayerChaliceIntroAnimation.SetSprites(isMugman);
		string stateName = "Intro_CH_MM" + ((!isScared) ? "_Hold" : "_Scared");
		levelPlayerChaliceIntroAnimation.animator.Play(stateName);
		return levelPlayerChaliceIntroAnimation;
	}

	// Token: 0x06003D43 RID: 15683 RVA: 0x0021E45E File Offset: 0x0021C85E
	public void EndHold()
	{
		base.animator.SetTrigger("Continue");
	}

	// Token: 0x06003D44 RID: 15684 RVA: 0x0021E470 File Offset: 0x0021C870
	private void SetSprites(bool isMugman)
	{
		if (Level.Current.CurrentLevel == Levels.Saltbaker)
		{
			this.cuphead.SetActive(false);
			this.mugman.SetActive(false);
		}
		else
		{
			this.cuphead.SetActive(!isMugman);
			this.mugman.SetActive(isMugman);
		}
	}

	// Token: 0x04004492 RID: 17554
	[SerializeField]
	private GameObject cuphead;

	// Token: 0x04004493 RID: 17555
	[SerializeField]
	private GameObject mugman;
}
