using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200056F RID: 1391
public class DevilLevelDevilArm : AbstractCollidableObject
{
	// Token: 0x06001A5E RID: 6750 RVA: 0x000F1560 File Offset: 0x000EF960
	protected override void Awake()
	{
		base.Awake();
		this.startX = base.transform.position.x;
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06001A5F RID: 6751 RVA: 0x000F1597 File Offset: 0x000EF997
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001A60 RID: 6752 RVA: 0x000F15AF File Offset: 0x000EF9AF
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001A61 RID: 6753 RVA: 0x000F15D8 File Offset: 0x000EF9D8
	public void Attack(float speed)
	{
		this.state = DevilLevelDevilArm.State.Attacking;
		base.animator.SetTrigger("ArmsIn");
		base.StartCoroutine(this.attack_cr(speed));
	}

	// Token: 0x06001A62 RID: 6754 RVA: 0x000F1600 File Offset: 0x000EFA00
	private IEnumerator attack_cr(float speed)
	{
		this.speed = speed;
		bool isClapping = false;
		float t = 0f;
		base.GetComponent<Collider2D>().enabled = true;
		while (t < speed)
		{
			base.transform.SetPosition(new float?(Mathf.Lerp(this.startX, this.endPos.position.x, t / speed)), null, null);
			yield return new WaitForFixedUpdate();
			t += CupheadTime.FixedDelta;
			if (t / speed > 0.85f && !isClapping)
			{
				base.animator.SetTrigger("OnAttack");
				isClapping = true;
			}
		}
		base.transform.SetPosition(new float?(this.endPos.position.x), null, null);
		CupheadLevelCamera.Current.Shake(20f, 0.7f, false);
		yield return new WaitForFixedUpdate();
		yield break;
	}

	// Token: 0x06001A63 RID: 6755 RVA: 0x000F1622 File Offset: 0x000EFA22
	public void MoveAway()
	{
		base.StartCoroutine(this.move_away_cr());
	}

	// Token: 0x06001A64 RID: 6756 RVA: 0x000F1634 File Offset: 0x000EFA34
	private IEnumerator move_away_cr()
	{
		float currentPos = base.transform.position.x;
		float t = 0f;
		float moveTime = this.speed;
		for (t = 0f; t < moveTime; t += CupheadTime.FixedDelta)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / moveTime);
			base.transform.SetPosition(new float?(Mathf.Lerp(currentPos, this.startX, val)), null, null);
			yield return new WaitForFixedUpdate();
		}
		base.transform.SetPosition(new float?(this.startX), null, null);
		this.RamSlapSFXActive = false;
		base.GetComponent<Collider2D>().enabled = false;
		this.state = DevilLevelDevilArm.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06001A65 RID: 6757 RVA: 0x000F164F File Offset: 0x000EFA4F
	private void HandclapSFX()
	{
		if (this.isRight)
		{
			AudioManager.Play("devil_hand_clap");
		}
	}

	// Token: 0x06001A66 RID: 6758 RVA: 0x000F1666 File Offset: 0x000EFA66
	private void RamSlapSFX()
	{
		if (!this.RamSlapSFXActive)
		{
			AudioManager.Play("devil_ram_slap");
			this.RamSlapSFXActive = true;
		}
	}

	// Token: 0x04002386 RID: 9094
	public DevilLevelDevilArm.State state;

	// Token: 0x04002387 RID: 9095
	private bool RamSlapSFXActive;

	// Token: 0x04002388 RID: 9096
	private DamageDealer damageDealer;

	// Token: 0x04002389 RID: 9097
	[SerializeField]
	private Transform endPos;

	// Token: 0x0400238A RID: 9098
	private float speed;

	// Token: 0x0400238B RID: 9099
	private float startX;

	// Token: 0x0400238C RID: 9100
	public bool isRight;

	// Token: 0x02000570 RID: 1392
	public enum State
	{
		// Token: 0x0400238E RID: 9102
		Idle,
		// Token: 0x0400238F RID: 9103
		Attacking
	}
}
