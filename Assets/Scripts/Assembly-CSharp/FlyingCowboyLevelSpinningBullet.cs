using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200065F RID: 1631
public class FlyingCowboyLevelSpinningBullet : AbstractProjectile
{
	// Token: 0x060021F5 RID: 8693 RVA: 0x0013C558 File Offset: 0x0013A958
	public FlyingCowboyLevelSpinningBullet Create(Vector2 pos, float speed, float rotationSpeed, float rotationRadius, Vector3 direction, bool clockwise, bool parryable)
	{
		FlyingCowboyLevelSpinningBullet flyingCowboyLevelSpinningBullet = this.Create() as FlyingCowboyLevelSpinningBullet;
		flyingCowboyLevelSpinningBullet.child.localPosition = new Vector3(rotationRadius, 0f);
		flyingCowboyLevelSpinningBullet.StartCoroutine(flyingCowboyLevelSpinningBullet.bullet_cr(pos, speed, rotationSpeed, direction, clockwise));
		flyingCowboyLevelSpinningBullet.StartCoroutine(flyingCowboyLevelSpinningBullet.scale_cr());
		flyingCowboyLevelSpinningBullet.SetParryable(parryable);
		return flyingCowboyLevelSpinningBullet;
	}

	// Token: 0x060021F6 RID: 8694 RVA: 0x0013C5B2 File Offset: 0x0013A9B2
	protected override void Update()
	{
		base.Update();
		this.child.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
	}

	// Token: 0x060021F7 RID: 8695 RVA: 0x0013C5E3 File Offset: 0x0013A9E3
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060021F8 RID: 8696 RVA: 0x0013C5F9 File Offset: 0x0013A9F9
	protected override void Die()
	{
		this.child.SetLocalEulerAngles(new float?(0f), new float?(0f), new float?((float)UnityEngine.Random.Range(0, 360)));
		base.Die();
	}

	// Token: 0x060021F9 RID: 8697 RVA: 0x0013C634 File Offset: 0x0013AA34
	private IEnumerator scale_cr()
	{
		Vector3 initialScale = base.transform.localScale;
		base.transform.localScale = initialScale * 0.75f;
		float elapsedTime = 0f;
		while (elapsedTime < 0.3f)
		{
			yield return null;
			elapsedTime += CupheadTime.Delta;
			Vector3 scale = Vector3.Lerp(initialScale * 0.75f, initialScale, elapsedTime / 0.3f);
			base.transform.localScale = scale;
		}
		yield break;
	}

	// Token: 0x060021FA RID: 8698 RVA: 0x0013C650 File Offset: 0x0013AA50
	private IEnumerator bullet_cr(Vector2 pos, float speed, float rotationSpeed, Vector3 direction, bool clockwise)
	{
		if (!clockwise)
		{
			base.animator.SetFloat("Speed", -1f);
		}
		base.transform.position = pos - this.child.localPosition;
		for (;;)
		{
			base.transform.position += direction * speed * CupheadTime.Delta;
			base.transform.AddEulerAngles(0f, 0f, (float)((!clockwise) ? 1 : -1) * rotationSpeed * CupheadTime.Delta);
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002AAC RID: 10924
	[SerializeField]
	private Transform child;
}
