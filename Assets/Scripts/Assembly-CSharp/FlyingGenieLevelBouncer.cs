using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000665 RID: 1637
public class FlyingGenieLevelBouncer : AbstractProjectile
{
	// Token: 0x06002216 RID: 8726 RVA: 0x0013D7A0 File Offset: 0x0013BBA0
	public FlyingGenieLevelBouncer Init(Vector3 pos, LevelProperties.FlyingGenie.Obelisk properties, float angle)
	{
		base.transform.position = pos;
		this.properties = properties;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(angle));
		this.sprite.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
		return this;
	}

	// Token: 0x06002217 RID: 8727 RVA: 0x0013D814 File Offset: 0x0013BC14
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002218 RID: 8728 RVA: 0x0013D829 File Offset: 0x0013BC29
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002219 RID: 8729 RVA: 0x0013D847 File Offset: 0x0013BC47
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600221A RID: 8730 RVA: 0x0013D868 File Offset: 0x0013BC68
	protected override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionWalls(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			if (Vector3.Dot(hit.transform.position, Vector3.right) > 0f)
			{
				Vector3 a = new Vector3((float)Level.Current.Left, base.transform.position.y, 0f);
				Vector3 position = base.transform.position;
				this.collisionPoint = a - position;
				base.StartCoroutine(this.change_dir_cr(this.collisionPoint));
			}
			else
			{
				Vector3 position2 = base.transform.position;
				Vector3 a2 = new Vector3((float)Level.Current.Right, base.transform.position.y, 0f);
				this.collisionPoint = a2 - position2;
				base.StartCoroutine(this.change_dir_cr(this.collisionPoint));
			}
		}
	}

	// Token: 0x0600221B RID: 8731 RVA: 0x0013D958 File Offset: 0x0013BD58
	protected override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			Vector3 position = base.transform.position;
			Vector3 a = new Vector3(base.transform.position.x, (float)Level.Current.Ground, 0f);
			this.collisionPoint = a - position;
			base.StartCoroutine(this.change_dir_cr(this.collisionPoint));
		}
	}

	// Token: 0x0600221C RID: 8732 RVA: 0x0013D9CC File Offset: 0x0013BDCC
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			Vector3 position = base.transform.position;
			Vector3 a = new Vector3(base.transform.position.x, (float)Level.Current.Ceiling, 0f);
			this.collisionPoint = a - position;
			base.StartCoroutine(this.change_dir_cr(this.collisionPoint));
		}
	}

	// Token: 0x0600221D RID: 8733 RVA: 0x0013DA40 File Offset: 0x0013BE40
	private IEnumerator move_cr()
	{
		this.velocity = base.transform.up;
		this.speed = this.properties.bouncerSpeed;
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			base.transform.position += this.velocity * this.speed * CupheadTime.Delta;
			yield return wait;
		}
		yield break;
	}

	// Token: 0x0600221E RID: 8734 RVA: 0x0013DA5C File Offset: 0x0013BE5C
	private IEnumerator change_dir_cr(Vector3 collisionPoint)
	{
		this.velocity = 1f * (-2f * Vector3.Dot(this.velocity, Vector3.Normalize(collisionPoint.normalized)) * Vector3.Normalize(collisionPoint.normalized) + this.velocity);
		yield return null;
		yield break;
	}

	// Token: 0x0600221F RID: 8735 RVA: 0x0013DA80 File Offset: 0x0013BE80
	private void ChangeSpeed()
	{
		if (Vector3.Dot(this.velocity, Vector3.right) > 0f)
		{
			float num = this.properties.bouncerSpeed - this.properties.obeliskMovementSpeed;
			this.speed -= num;
		}
		else
		{
			this.speed = this.properties.bouncerSpeed;
		}
	}

	// Token: 0x06002220 RID: 8736 RVA: 0x0013DAE3 File Offset: 0x0013BEE3
	protected override void Die()
	{
		base.Die();
	}

	// Token: 0x04002AC3 RID: 10947
	[SerializeField]
	private Transform sprite;

	// Token: 0x04002AC4 RID: 10948
	private LevelProperties.FlyingGenie.Obelisk properties;

	// Token: 0x04002AC5 RID: 10949
	private Vector3 velocity;

	// Token: 0x04002AC6 RID: 10950
	private Vector3 average;

	// Token: 0x04002AC7 RID: 10951
	private Vector3 collisionPoint;

	// Token: 0x04002AC8 RID: 10952
	private float speed;
}
