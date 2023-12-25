using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008B8 RID: 2232
public class FunhousePlatformingLevelJackinBoxProjectile : BasicProjectile
{
	// Token: 0x06003417 RID: 13335 RVA: 0x001E3E5F File Offset: 0x001E225F
	protected override void Start()
	{
		base.Start();
		this.move = false;
		base.StartCoroutine(this.animation_cr());
	}

	// Token: 0x06003418 RID: 13336 RVA: 0x001E3E7C File Offset: 0x001E227C
	public FunhousePlatformingLevelJackinBoxProjectile Create(Vector3 pos, float speed, float delay, AbstractPlayerController player, int direction)
	{
		FunhousePlatformingLevelJackinBoxProjectile funhousePlatformingLevelJackinBoxProjectile = base.Create(pos, 0f, speed) as FunhousePlatformingLevelJackinBoxProjectile;
		funhousePlatformingLevelJackinBoxProjectile.delay = delay;
		funhousePlatformingLevelJackinBoxProjectile.player = player;
		funhousePlatformingLevelJackinBoxProjectile.StartAnimation(direction);
		return funhousePlatformingLevelJackinBoxProjectile;
	}

	// Token: 0x06003419 RID: 13337 RVA: 0x001E3EBC File Offset: 0x001E22BC
	private void StartAnimation(int direction)
	{
		switch (direction)
		{
		case 1:
			base.animator.Play("Top_Start");
			break;
		case 2:
			base.animator.Play("Left_Start");
			break;
		case 3:
			base.animator.Play("Bottom_Start");
			break;
		case 4:
			base.animator.Play("Right_Start");
			break;
		}
	}

	// Token: 0x0600341A RID: 13338 RVA: 0x001E3F3C File Offset: 0x001E233C
	private IEnumerator animation_cr()
	{
		yield return base.animator.WaitForAnimationToStart(this, "Projectile", false);
		yield return CupheadTime.WaitForSeconds(this, this.delay);
		base.animator.SetTrigger("Move");
		yield return base.animator.WaitForAnimationToEnd(this, "Projectile_Move_Start", false, true);
		Vector3 dir = this.player.transform.position - base.transform.position;
		float start = base.transform.rotation.z;
		float end = MathUtils.DirectionToAngle(dir);
		float t = 0f;
		float time = 0.1f;
		while (t < time)
		{
			t += CupheadTime.Delta;
			base.transform.SetEulerAngles(null, null, new float?(Mathf.Lerp(start, end, t / time)));
			yield return null;
		}
		this.move = true;
		yield return null;
		yield break;
	}

	// Token: 0x04003C5D RID: 15453
	private AbstractPlayerController player;

	// Token: 0x04003C5E RID: 15454
	private float delay;
}
