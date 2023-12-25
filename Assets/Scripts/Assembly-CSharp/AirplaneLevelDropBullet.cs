using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004BA RID: 1210
public class AirplaneLevelDropBullet : AbstractProjectile
{
	// Token: 0x1700030F RID: 783
	// (get) Token: 0x06001406 RID: 5126 RVA: 0x000B23D3 File Offset: 0x000B07D3
	// (set) Token: 0x06001407 RID: 5127 RVA: 0x000B23DB File Offset: 0x000B07DB
	public bool isMoving { get; private set; }

	// Token: 0x06001408 RID: 5128 RVA: 0x000B23E4 File Offset: 0x000B07E4
	public virtual AirplaneLevelDropBullet Init(Vector3 targetPos, Vector3 startPos, float dropSpeed, float shootSpeed, bool onLeft, bool camHorizontal)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = startPos;
		this.YtoSwitch = targetPos.y;
		this.shootSpeed = shootSpeed;
		this.onLeft = onLeft;
		base.transform.SetScale(new float?((float)((!onLeft) ? -1 : 1)), null, null);
		this.rend.sortingOrder = ((!onLeft) ? 501 : 500);
		this.targetPos = targetPos;
		this.bounds = ((!camHorizontal) ? CupheadLevelCamera.Current.Bounds.xMax : CupheadLevelCamera.Current.Bounds.yMax);
		this.dropSpeed = dropSpeed;
		this.moveDir = ((!onLeft) ? Vector3.right : Vector3.left);
		this.goingDown = true;
		this.isMoving = true;
		this.boxColl.enabled = false;
		this.circColl.enabled = true;
		this.t = 0.7853982f;
		this.startPos = startPos + Vector3.down * (Mathf.Sin(this.t) * 600f);
		return this;
	}

	// Token: 0x06001409 RID: 5129 RVA: 0x000B2531 File Offset: 0x000B0931
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.rotate_cr());
	}

	// Token: 0x0600140A RID: 5130 RVA: 0x000B2548 File Offset: 0x000B0948
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.goingDown)
		{
			this.t += CupheadTime.FixedDelta * this.dropSpeed;
			if (this.t < 3.1415927f)
			{
				base.transform.position = new Vector3(EaseUtils.EaseOutSine(this.startPos.x, this.targetPos.x, Mathf.InverseLerp(0.7853982f, 3.1415927f, this.t)), this.startPos.y + Mathf.Sin(this.t) * 600f);
			}
			else
			{
				base.transform.position += Vector3.up * (Mathf.Sin(3.1415927f) - Mathf.Sin(3.1415927f - CupheadTime.FixedDelta * this.dropSpeed)) * 600f;
			}
			if (base.transform.position.y < this.YtoSwitch)
			{
				base.transform.position = new Vector3(base.transform.position.x, this.YtoSwitch);
				this.moveDir = ((!this.onLeft) ? Vector3.left : Vector3.right);
				this.dropSpeed = this.shootSpeed;
				this.goingDown = false;
				base.animator.SetTrigger("ToShoot");
				this.boxColl.enabled = true;
				this.circColl.enabled = false;
				this.shootFX.Create(base.transform.position, base.transform.localScale);
				this.t = 0f;
			}
		}
		else
		{
			this.t += CupheadTime.FixedDelta;
			if (this.t > 0.33333334f)
			{
				this.speedLines.enabled = false;
			}
			base.transform.position += this.moveDir * this.dropSpeed * CupheadTime.FixedDelta;
			if (base.transform.position.x < -this.bounds - 100f || base.transform.position.x > this.bounds + 100f)
			{
				this.isMoving = false;
				this.Recycle<AirplaneLevelDropBullet>();
			}
		}
	}

	// Token: 0x0600140B RID: 5131 RVA: 0x000B27C6 File Offset: 0x000B0BC6
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0600140C RID: 5132 RVA: 0x000B27E4 File Offset: 0x000B0BE4
	private IEnumerator rotate_cr()
	{
		WaitForFrameTimePersistent wait = new WaitForFrameTimePersistent(0.041666668f, false);
		float startRotation = 360f;
		float rotateSpeed = 1f;
		float rotateTime = 0f;
		while (this.goingDown)
		{
			base.transform.SetEulerAngles(null, null, new float?(startRotation * rotateTime));
			rotateTime += CupheadTime.FixedDelta * rotateSpeed;
			yield return wait;
		}
		base.transform.SetEulerAngles(null, null, new float?(startRotation));
		yield return null;
		yield break;
	}

	// Token: 0x04001D29 RID: 7465
	private const float SPAWN_OFFSET = 100f;

	// Token: 0x04001D2A RID: 7466
	private const float ARC_HEIGHT = 600f;

	// Token: 0x04001D2C RID: 7468
	private Vector3 moveDir;

	// Token: 0x04001D2D RID: 7469
	private Vector3 startPos;

	// Token: 0x04001D2E RID: 7470
	private Vector3 targetPos;

	// Token: 0x04001D2F RID: 7471
	private float shootSpeed;

	// Token: 0x04001D30 RID: 7472
	private float dropSpeed;

	// Token: 0x04001D31 RID: 7473
	private float YtoSwitch;

	// Token: 0x04001D32 RID: 7474
	private float bounds;

	// Token: 0x04001D33 RID: 7475
	private bool goingDown;

	// Token: 0x04001D34 RID: 7476
	private bool onLeft;

	// Token: 0x04001D35 RID: 7477
	[SerializeField]
	private CircleCollider2D circColl;

	// Token: 0x04001D36 RID: 7478
	[SerializeField]
	private BoxCollider2D boxColl;

	// Token: 0x04001D37 RID: 7479
	[SerializeField]
	private Effect shootFX;

	// Token: 0x04001D38 RID: 7480
	[SerializeField]
	private SpriteRenderer speedLines;

	// Token: 0x04001D39 RID: 7481
	[SerializeField]
	private SpriteRenderer rend;

	// Token: 0x04001D3A RID: 7482
	private float t;
}
