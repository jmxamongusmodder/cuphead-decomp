using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006A7 RID: 1703
public abstract class AbstractFrogsLevelSlotBullet : AbstractPausableComponent
{
	// Token: 0x170003A8 RID: 936
	// (get) Token: 0x06002414 RID: 9236 RVA: 0x00152EDF File Offset: 0x001512DF
	protected virtual float Y
	{
		get
		{
			return (float)(Level.Current.Ground + 50);
		}
	}

	// Token: 0x170003A9 RID: 937
	// (get) Token: 0x06002415 RID: 9237 RVA: 0x00152EEF File Offset: 0x001512EF
	protected virtual float Y_Time
	{
		get
		{
			return 0.45f;
		}
	}

	// Token: 0x170003AA RID: 938
	// (get) Token: 0x06002416 RID: 9238 RVA: 0x00152EF6 File Offset: 0x001512F6
	protected virtual EaseUtils.EaseType Y_Ease
	{
		get
		{
			return EaseUtils.EaseType.easeOutBounce;
		}
	}

	// Token: 0x06002417 RID: 9239 RVA: 0x00152EFC File Offset: 0x001512FC
	public AbstractFrogsLevelSlotBullet Create(Vector2 pos, float speed)
	{
		AbstractFrogsLevelSlotBullet abstractFrogsLevelSlotBullet = this.InstantiatePrefab<AbstractFrogsLevelSlotBullet>();
		abstractFrogsLevelSlotBullet.transform.SetPosition(new float?(pos.x), new float?(pos.y), null);
		abstractFrogsLevelSlotBullet.speed = speed;
		return abstractFrogsLevelSlotBullet;
	}

	// Token: 0x06002418 RID: 9240 RVA: 0x00152F44 File Offset: 0x00151344
	protected virtual void Start()
	{
		this.damageDealer = new DamageDealer(1f, 0.3f, DamageDealer.DamageSource.Enemy, true, false, false);
		this.damageDealer.SetDirection(DamageDealer.Direction.Neutral, base.transform);
		GameObject gameObject = new GameObject("Damage Shit!");
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.ResetLocalTransforms();
		BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
		boxCollider2D.size = new Vector2(240f, 40f);
		boxCollider2D.isTrigger = true;
		CollisionChild collisionChild = gameObject.AddComponent<CollisionChild>();
		collisionChild.OnPlayerCollision += this.DealDamage;
		base.StartCoroutine(this.x_cr());
		base.StartCoroutine(this.y_cr());
	}

	// Token: 0x06002419 RID: 9241 RVA: 0x00152FF9 File Offset: 0x001513F9
	private void Update()
	{
		this.damageDealer.Update();
	}

	// Token: 0x0600241A RID: 9242 RVA: 0x00153006 File Offset: 0x00151406
	protected void DealDamage(GameObject hit, CollisionPhase phase)
	{
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x0600241B RID: 9243 RVA: 0x00153015 File Offset: 0x00151415
	protected virtual void End()
	{
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600241C RID: 9244 RVA: 0x00153028 File Offset: 0x00151428
	private IEnumerator x_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			if (base.transform.position.x < -1280f)
			{
				this.End();
			}
			base.transform.AddPosition(-this.speed * CupheadTime.FixedDelta, 0f, 0f);
			yield return wait;
		}
		yield break;
	}

	// Token: 0x0600241D RID: 9245 RVA: 0x00153044 File Offset: 0x00151444
	private IEnumerator y_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float start = base.transform.position.y;
		float t = 0f;
		while (t < this.Y_Time)
		{
			float val = t / this.Y_Time;
			float y = EaseUtils.Ease(this.Y_Ease, start, this.Y, val);
			base.transform.SetPosition(null, new float?(y), null);
			t += CupheadTime.FixedDelta;
			yield return wait;
		}
		yield break;
	}

	// Token: 0x04002CE3 RID: 11491
	protected DamageDealer damageDealer;

	// Token: 0x04002CE4 RID: 11492
	protected float speed;
}
