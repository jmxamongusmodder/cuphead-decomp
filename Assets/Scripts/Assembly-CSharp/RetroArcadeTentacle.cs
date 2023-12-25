using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200075B RID: 1883
public class RetroArcadeTentacle : AbstractProjectile
{
	// Token: 0x0600290D RID: 10509 RVA: 0x0017EC20 File Offset: 0x0017D020
	public virtual AbstractProjectile Init(Vector3 pos, float targetPosY, bool onLeft, float verticalSpeed, float horizontalSpeed)
	{
		base.ResetLifetime();
		base.ResetDistance();
		this.target.transform.SetLocalPosition(new float?(this.targetRoot.localPosition.x + ((!onLeft) ? -15f : 15f)), new float?(this.targetRoot.localPosition.y + targetPosY), null);
		this.verticalSpeed = verticalSpeed;
		this.horizontalSpeed = horizontalSpeed;
		this.onLeft = onLeft;
		base.transform.position = pos;
		this.startPos = pos;
		base.StartCoroutine(this.move_cr());
		return this;
	}

	// Token: 0x0600290E RID: 10510 RVA: 0x0017ECD2 File Offset: 0x0017D0D2
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600290F RID: 10511 RVA: 0x0017ECF0 File Offset: 0x0017D0F0
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002910 RID: 10512 RVA: 0x0017ED10 File Offset: 0x0017D110
	private IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		Vector3 direction = (!this.onLeft) ? Vector3.left : Vector3.right;
		this.canMove = true;
		while (base.transform.position.y < 0f)
		{
			base.transform.position += Vector3.up * this.verticalSpeed * CupheadTime.FixedDelta;
			yield return wait;
		}
		while (this.canMove && !this.target.IsDead)
		{
			base.transform.position += direction * this.horizontalSpeed * CupheadTime.FixedDelta;
			yield return wait;
		}
		while (base.transform.position.y > this.startPos.y)
		{
			base.transform.position += Vector3.down * this.verticalSpeed * CupheadTime.FixedDelta;
			yield return wait;
		}
		this.Recycle<RetroArcadeTentacle>();
		yield break;
	}

	// Token: 0x06002911 RID: 10513 RVA: 0x0017ED2B File Offset: 0x0017D12B
	protected override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (hit.GetComponent<RetroArcadeTentacle>())
		{
			this.canMove = false;
		}
	}

	// Token: 0x040031F7 RID: 12791
	[SerializeField]
	private RetroArcadeTentacleTarget target;

	// Token: 0x040031F8 RID: 12792
	[SerializeField]
	private Transform targetRoot;

	// Token: 0x040031F9 RID: 12793
	private const float OFFSET = 15f;

	// Token: 0x040031FA RID: 12794
	private float verticalSpeed;

	// Token: 0x040031FB RID: 12795
	private float horizontalSpeed;

	// Token: 0x040031FC RID: 12796
	private bool onLeft;

	// Token: 0x040031FD RID: 12797
	private bool canMove;

	// Token: 0x040031FE RID: 12798
	private Vector3 startPos;
}
