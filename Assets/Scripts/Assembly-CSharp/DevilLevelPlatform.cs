using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200057C RID: 1404
public class DevilLevelPlatform : AbstractPausableComponent
{
	// Token: 0x06001AB7 RID: 6839 RVA: 0x000F4EB8 File Offset: 0x000F32B8
	protected override void Awake()
	{
		base.Awake();
		base.animator.Play("Platform_" + this.type.ToString());
		this.baseY = base.transform.position.y;
	}

	// Token: 0x06001AB8 RID: 6840 RVA: 0x000F4F0A File Offset: 0x000F330A
	public void Raise(float speed, float height, float delay)
	{
		this.state = DevilLevelPlatform.State.Raising;
		base.StartCoroutine(this.raise_cr(speed, height, delay));
	}

	// Token: 0x06001AB9 RID: 6841 RVA: 0x000F4F24 File Offset: 0x000F3324
	private IEnumerator raise_cr(float speed, float height, float delay)
	{
		float t = 0f;
		float moveTime = height / speed;
		while (t < moveTime)
		{
			base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, this.baseY, this.baseY + height, t / moveTime)), null);
			t += CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		base.transform.SetPosition(null, new float?(this.baseY + height), null);
		yield return CupheadTime.WaitForSeconds(this, delay);
		t = 0f;
		while (t < moveTime)
		{
			base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, this.baseY + height, this.baseY, t / moveTime)), null);
			t += CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		base.transform.SetPosition(null, new float?(this.baseY), null);
		this.state = DevilLevelPlatform.State.Idle;
		yield break;
	}

	// Token: 0x06001ABA RID: 6842 RVA: 0x000F4F54 File Offset: 0x000F3354
	public void Lower(float speed)
	{
		this.state = DevilLevelPlatform.State.Lowering;
		base.StartCoroutine(this.lower_cr(speed));
	}

	// Token: 0x06001ABB RID: 6843 RVA: 0x000F4F6C File Offset: 0x000F336C
	private IEnumerator lower_cr(float speed)
	{
		float t = 0f;
		float moveTime = 300f / speed;
		while (t < moveTime)
		{
			base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInSine, this.baseY, this.baseY - 300f, t / moveTime)), null);
			t += CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		this.Die();
		yield break;
	}

	// Token: 0x06001ABC RID: 6844 RVA: 0x000F4F90 File Offset: 0x000F3390
	public void Die()
	{
		foreach (AbstractPlayerController abstractPlayerController in base.GetComponentsInChildren<AbstractPlayerController>())
		{
			if (!(abstractPlayerController == null))
			{
				abstractPlayerController.transform.parent = null;
			}
		}
		base.gameObject.SetActive(false);
		this.state = DevilLevelPlatform.State.Dead;
	}

	// Token: 0x040023E4 RID: 9188
	private const float LOWER_DISTANCE = 300f;

	// Token: 0x040023E5 RID: 9189
	public DevilLevelPlatform.PlatformType type;

	// Token: 0x040023E6 RID: 9190
	public DevilLevelPlatform.State state;

	// Token: 0x040023E7 RID: 9191
	private float baseY;

	// Token: 0x0200057D RID: 1405
	public enum PlatformType
	{
		// Token: 0x040023E9 RID: 9193
		A,
		// Token: 0x040023EA RID: 9194
		B,
		// Token: 0x040023EB RID: 9195
		C,
		// Token: 0x040023EC RID: 9196
		D,
		// Token: 0x040023ED RID: 9197
		E
	}

	// Token: 0x0200057E RID: 1406
	public enum State
	{
		// Token: 0x040023EF RID: 9199
		Idle,
		// Token: 0x040023F0 RID: 9200
		Raising,
		// Token: 0x040023F1 RID: 9201
		Lowering,
		// Token: 0x040023F2 RID: 9202
		Dead
	}
}
