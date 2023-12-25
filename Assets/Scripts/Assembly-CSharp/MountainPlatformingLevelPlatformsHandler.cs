using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008EB RID: 2283
public class MountainPlatformingLevelPlatformsHandler : AbstractPausableComponent
{
	// Token: 0x0600358A RID: 13706 RVA: 0x001F30F8 File Offset: 0x001F14F8
	private void Start()
	{
		this.platforms = new Transform[this.platformHolder.GetComponentsInChildren<Transform>().Length];
		this.platforms = this.platformHolder.GetComponentsInChildren<Transform>();
		this.platformsStartPos = new Vector3[this.platformHolder.GetComponentsInChildren<Transform>().Length];
		for (int i = 0; i < this.platforms.Length; i++)
		{
			this.platformsStartPos[i] = this.platforms[i].position;
		}
		for (int j = 0; j < this.platforms.Length; j++)
		{
			this.OffScreen(this.platforms[j]);
		}
		this.parrySwitch.OnActivate += this.MovePlatforms;
	}

	// Token: 0x0600358B RID: 13707 RVA: 0x001F31BB File Offset: 0x001F15BB
	private void MovePlatforms()
	{
		if (!this.hasSwitched)
		{
			base.StartCoroutine(this.moving_cr());
			this.hasSwitched = true;
		}
	}

	// Token: 0x0600358C RID: 13708 RVA: 0x001F31DC File Offset: 0x001F15DC
	private IEnumerator moving_cr()
	{
		for (int i = 0; i < this.platforms.Length; i++)
		{
			base.StartCoroutine(this.move_platform_cr(this.platforms[i], this.platformsStartPos[i]));
			yield return CupheadTime.WaitForSeconds(this, this.platformAppearDelay);
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600358D RID: 13709 RVA: 0x001F31F7 File Offset: 0x001F15F7
	private void OffScreen(Transform platform)
	{
		platform.transform.position += Vector3.down * this.lowerAmount;
	}

	// Token: 0x0600358E RID: 13710 RVA: 0x001F3220 File Offset: 0x001F1620
	private IEnumerator move_platform_cr(Transform platform, Vector3 startPos)
	{
		float t = 0f;
		float time = this.platformMoveTime;
		Vector2 start = platform.transform.position;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			platform.transform.position = Vector2.Lerp(start, startPos, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		platform.transform.position = startPos;
		yield return null;
		yield break;
	}

	// Token: 0x04003DA1 RID: 15777
	[SerializeField]
	private Transform platformHolder;

	// Token: 0x04003DA2 RID: 15778
	[SerializeField]
	private ParrySwitch parrySwitch;

	// Token: 0x04003DA3 RID: 15779
	[SerializeField]
	private float platformMoveTime;

	// Token: 0x04003DA4 RID: 15780
	[SerializeField]
	private float platformAppearDelay;

	// Token: 0x04003DA5 RID: 15781
	private bool hasSwitched;

	// Token: 0x04003DA6 RID: 15782
	private Transform[] platforms;

	// Token: 0x04003DA7 RID: 15783
	private Vector3[] platformsStartPos;

	// Token: 0x04003DA8 RID: 15784
	private float lowerAmount = 1000f;
}
