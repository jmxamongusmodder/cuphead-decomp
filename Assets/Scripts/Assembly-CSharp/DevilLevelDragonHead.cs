using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000571 RID: 1393
public class DevilLevelDragonHead : AbstractCollidableObject
{
	// Token: 0x06001A68 RID: 6760 RVA: 0x000F1A60 File Offset: 0x000EFE60
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		foreach (CollisionChild collisionChild in this.children.GetComponentsInChildren<CollisionChild>())
		{
			collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
		}
		this.children.gameObject.SetActive(false);
	}

	// Token: 0x06001A69 RID: 6761 RVA: 0x000F1AC6 File Offset: 0x000EFEC6
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001A6A RID: 6762 RVA: 0x000F1ADE File Offset: 0x000EFEDE
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001A6B RID: 6763 RVA: 0x000F1B08 File Offset: 0x000EFF08
	public void Attack(DevilLevelSittingDevil parent, bool isLeft)
	{
		this.state = DevilLevelDragonHead.State.Moving;
		base.transform.SetScale(new float?((float)((!isLeft) ? -1 : 1)), null, null);
		base.StartCoroutine(this.move_cr(parent, isLeft));
	}

	// Token: 0x06001A6C RID: 6764 RVA: 0x000F1B5C File Offset: 0x000EFF5C
	private IEnumerator move_cr(DevilLevelSittingDevil parent, bool isLeft)
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		base.transform.position = ((!isLeft) ? this.rightRoot.position : this.leftRoot.position);
		Vector3 dir = (!isLeft) ? Vector3.left : Vector3.right;
		yield return parent.animator.WaitForAnimationToEnd(this, "Morph_Start" + ((!isLeft) ? "_Right" : "_Left"), false, true);
		parent.animator.SetTrigger("OnDragonAttack");
		this.children.gameObject.SetActive(true);
		while (this.state == DevilLevelDragonHead.State.Moving)
		{
			base.transform.position += dir * (this.speed * CupheadTime.FixedDelta) * parent.animator.speed;
			yield return wait;
		}
		yield return parent.animator.WaitForAnimationToEnd(this, "Morph_Attack", 1, false, true);
		this.state = DevilLevelDragonHead.State.Idle;
		this.children.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06001A6D RID: 6765 RVA: 0x000F1B85 File Offset: 0x000EFF85
	public void SetPosition(Vector3 pos)
	{
		this.children.gameObject.SetActive(false);
		base.transform.position = pos;
	}

	// Token: 0x04002390 RID: 9104
	[SerializeField]
	private float speed;

	// Token: 0x04002391 RID: 9105
	[SerializeField]
	private Transform leftRoot;

	// Token: 0x04002392 RID: 9106
	[SerializeField]
	private Transform rightRoot;

	// Token: 0x04002393 RID: 9107
	[SerializeField]
	private Transform children;

	// Token: 0x04002394 RID: 9108
	public DevilLevelDragonHead.State state;

	// Token: 0x04002395 RID: 9109
	private const float FRAME_RATE = 0.041666668f;

	// Token: 0x04002396 RID: 9110
	private DamageDealer damageDealer;

	// Token: 0x02000572 RID: 1394
	public enum State
	{
		// Token: 0x04002398 RID: 9112
		Idle,
		// Token: 0x04002399 RID: 9113
		Moving,
		// Token: 0x0400239A RID: 9114
		Stopped
	}
}
