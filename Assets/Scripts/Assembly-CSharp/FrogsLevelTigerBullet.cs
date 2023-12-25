using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006B7 RID: 1719
public class FrogsLevelTigerBullet : AbstractFrogsLevelSlotBullet
{
	// Token: 0x06002474 RID: 9332 RVA: 0x00155A03 File Offset: 0x00153E03
	protected override void Start()
	{
		base.Start();
		this.bullet.OnPlayerCollision += base.DealDamage;
		base.StartCoroutine(this.bullet_cr());
	}

	// Token: 0x06002475 RID: 9333 RVA: 0x00155A30 File Offset: 0x00153E30
	private IEnumerator bullet_cr()
	{
		float t = 0f;
		Transform trans = this.bullet.transform;
		float start = trans.localPosition.y;
		float end = start + 500f;
		for (;;)
		{
			t = 0f;
			AudioManager.Play("level_frogs_ball_platform_ball_launch");
			while (t < 0.5f)
			{
				float val = t / 0.5f;
				float y = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, start, end, val);
				trans.SetLocalPosition(null, new float?(y), null);
				t += CupheadTime.Delta;
				yield return null;
			}
			t = 0f;
			while (t < 0.5f)
			{
				float val2 = t / 0.5f;
				float y2 = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, end, start, val2);
				trans.SetLocalPosition(null, new float?(y2), null);
				t += CupheadTime.Delta;
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x04002D23 RID: 11555
	private const float BULLET_TIME = 0.5f;

	// Token: 0x04002D24 RID: 11556
	private const float BULLET_HEIGHT = 500f;

	// Token: 0x04002D25 RID: 11557
	public CollisionChild bullet;
}
