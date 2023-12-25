using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000909 RID: 2313
public class PlatformingLevelPlatformSag : LevelPlatform
{
	// Token: 0x06003646 RID: 13894 RVA: 0x000C2C54 File Offset: 0x000C1054
	private void Start()
	{
		this.localPosY = base.transform.localPosition.y;
	}

	// Token: 0x06003647 RID: 13895 RVA: 0x000C2C7A File Offset: 0x000C107A
	public override void AddChild(Transform player)
	{
		base.AddChild(player);
		if (base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.fall_cr());
		}
	}

	// Token: 0x06003648 RID: 13896 RVA: 0x000C2CA0 File Offset: 0x000C10A0
	public override void OnPlayerExit(Transform player)
	{
		base.OnPlayerExit(player);
		if (base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.go_up_cr());
		}
	}

	// Token: 0x06003649 RID: 13897 RVA: 0x000C2CC8 File Offset: 0x000C10C8
	private IEnumerator goTo_cr(float start, float end, float time, EaseUtils.EaseType ease)
	{
		float t = 0f;
		base.transform.SetLocalPosition(null, new float?(start), null);
		while (t < time)
		{
			float val = t / time;
			base.transform.SetLocalPosition(null, new float?(EaseUtils.Ease(ease, start, end, val)), null);
			t += Time.deltaTime;
			yield return base.StartCoroutine(base.WaitForPause_CR());
		}
		base.transform.SetLocalPosition(null, new float?(end), null);
		yield break;
	}

	// Token: 0x0600364A RID: 13898 RVA: 0x000C2D00 File Offset: 0x000C1100
	private IEnumerator fall_cr()
	{
		yield return base.StartCoroutine(this.goTo_cr(base.transform.localPosition.y, this.localPosY - this.sagAmount, 0.4f, EaseUtils.EaseType.easeOutBounce));
		yield break;
	}

	// Token: 0x0600364B RID: 13899 RVA: 0x000C2D1C File Offset: 0x000C111C
	private IEnumerator go_up_cr()
	{
		yield return base.StartCoroutine(this.goTo_cr(base.transform.localPosition.y, this.localPosY, 0.6f, EaseUtils.EaseType.easeOutBounce));
		yield break;
	}

	// Token: 0x04003E3C RID: 15932
	[SerializeField]
	private float sagAmount = 30f;

	// Token: 0x04003E3D RID: 15933
	private const EaseUtils.EaseType FALL_BOUNCE_EASE = EaseUtils.EaseType.easeOutBounce;

	// Token: 0x04003E3E RID: 15934
	public const float FALL_TIME = 0.4f;

	// Token: 0x04003E3F RID: 15935
	public const float RISE_TIME = 0.6f;

	// Token: 0x04003E40 RID: 15936
	private float localPosY;
}
