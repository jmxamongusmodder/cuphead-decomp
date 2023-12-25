using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A9A RID: 2714
public class PlanePlayerDeathPart : AbstractMonoBehaviour
{
	// Token: 0x0600410D RID: 16653 RVA: 0x002357E8 File Offset: 0x00233BE8
	public PlanePlayerDeathPart CreatePart(PlayerId player, Vector3 position)
	{
		PlanePlayerDeathPart planePlayerDeathPart = this.InstantiatePrefab<PlanePlayerDeathPart>();
		planePlayerDeathPart.transform.position = position;
		planePlayerDeathPart.animator.SetInteger("Player", (int)player);
		return planePlayerDeathPart;
	}

	// Token: 0x0600410E RID: 16654 RVA: 0x0023581A File Offset: 0x00233C1A
	protected override void Awake()
	{
		base.Awake();
		this.velocity = new Vector2(UnityEngine.Random.Range(-500f, 500f), UnityEngine.Random.Range(500f, 1000f));
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x0600410F RID: 16655 RVA: 0x00235858 File Offset: 0x00233C58
	private IEnumerator move_cr()
	{
		for (;;)
		{
			base.transform.position += (this.velocity + new Vector2(-300f, this.accumulatedGravity)) * base.LocalDeltaTime;
			this.accumulatedGravity += -6000f * base.LocalDeltaTime;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06004110 RID: 16656 RVA: 0x00235874 File Offset: 0x00233C74
	public void GameOverUnpause()
	{
		base.animator.enabled = true;
		AnimationHelper component = base.GetComponent<AnimationHelper>();
		component.IgnoreGlobal = true;
		this.ignoreGlobalTime = true;
		base.enabled = true;
	}

	// Token: 0x040047A5 RID: 18341
	private const float VELOCITY_X_MIN = -500f;

	// Token: 0x040047A6 RID: 18342
	private const float VELOCITY_X_MAX = 500f;

	// Token: 0x040047A7 RID: 18343
	private const float VELOCITY_Y_MIN = 500f;

	// Token: 0x040047A8 RID: 18344
	private const float VELOCITY_Y_MAX = 1000f;

	// Token: 0x040047A9 RID: 18345
	private const float GRAVITY = -6000f;

	// Token: 0x040047AA RID: 18346
	private Vector2 velocity;

	// Token: 0x040047AB RID: 18347
	private float accumulatedGravity;
}
