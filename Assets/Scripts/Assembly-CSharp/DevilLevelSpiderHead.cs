using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000581 RID: 1409
public class DevilLevelSpiderHead : AbstractCollidableObject
{
	// Token: 0x06001AEC RID: 6892 RVA: 0x000F792D File Offset: 0x000F5D2D
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		base.gameObject.SetActive(false);
	}

	// Token: 0x06001AED RID: 6893 RVA: 0x000F794C File Offset: 0x000F5D4C
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001AEE RID: 6894 RVA: 0x000F7964 File Offset: 0x000F5D64
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001AEF RID: 6895 RVA: 0x000F7990 File Offset: 0x000F5D90
	public void Attack(float xPos, float downSpeed, float upSpeed)
	{
		base.gameObject.SetActive(true);
		base.animator.SetBool("IsFalling", true);
		this.state = DevilLevelSpiderHead.State.Attacking;
		base.transform.SetPosition(new float?(xPos), null, null);
		base.StartCoroutine(this.attack_cr(downSpeed, upSpeed));
	}

	// Token: 0x06001AF0 RID: 6896 RVA: 0x000F79F4 File Offset: 0x000F5DF4
	private IEnumerator attack_cr(float downSpeed, float upSpeed)
	{
		float moveTime = Mathf.Abs(this.moveDistanceY) / downSpeed;
		float startY = base.transform.position.y;
		float t = 0f;
		base.GetComponent<Collider2D>().enabled = true;
		AudioManager.Play("devil_spider_fall");
		this.emitAudioFromObject.Add("devil_spider_fall");
		while (t < moveTime)
		{
			base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, startY, startY + this.moveDistanceY, t / moveTime)), null);
			yield return new WaitForFixedUpdate();
			t += CupheadTime.FixedDelta;
		}
		AudioManager.Play("devil_spider_head_hit_floor");
		this.emitAudioFromObject.Add("devil_spider_head_hit_floor");
		base.animator.SetBool("IsFalling", false);
		base.transform.SetPosition(null, new float?(startY + this.moveDistanceY), null);
		t = 0f;
		moveTime = Mathf.Abs(this.moveDistanceY) / upSpeed;
		yield return base.animator.WaitForAnimationToEnd(this, "Fall_Splat", false, true);
		while (t < moveTime)
		{
			base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInSine, startY + this.moveDistanceY, startY, t / moveTime)), null);
			yield return new WaitForFixedUpdate();
			t += CupheadTime.FixedDelta;
		}
		base.transform.SetPosition(null, new float?(startY), null);
		this.state = DevilLevelSpiderHead.State.Idle;
		base.GetComponent<Collider2D>().enabled = false;
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x04002426 RID: 9254
	public DevilLevelSpiderHead.State state;

	// Token: 0x04002427 RID: 9255
	private DamageDealer damageDealer;

	// Token: 0x04002428 RID: 9256
	[SerializeField]
	private float moveDistanceY;

	// Token: 0x02000582 RID: 1410
	public enum State
	{
		// Token: 0x0400242A RID: 9258
		Idle,
		// Token: 0x0400242B RID: 9259
		Attacking
	}
}
