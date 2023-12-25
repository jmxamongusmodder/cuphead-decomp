using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000629 RID: 1577
public class FlyingBirdLevelSmallBirdEgg : AbstractCollidableObject
{
	// Token: 0x1700037F RID: 895
	// (get) Token: 0x0600201C RID: 8220 RVA: 0x0012769E File Offset: 0x00125A9E
	// (set) Token: 0x0600201D RID: 8221 RVA: 0x001276A6 File Offset: 0x00125AA6
	public Transform container { get; private set; }

	// Token: 0x0600201E RID: 8222 RVA: 0x001276AF File Offset: 0x00125AAF
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		base.StartCoroutine(this.animSpeed_cr());
	}

	// Token: 0x0600201F RID: 8223 RVA: 0x001276CF File Offset: 0x00125ACF
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		this.UpdateRotation();
	}

	// Token: 0x06002020 RID: 8224 RVA: 0x001276ED File Offset: 0x00125AED
	private void UpdateRotation()
	{
	}

	// Token: 0x06002021 RID: 8225 RVA: 0x001276EF File Offset: 0x00125AEF
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002022 RID: 8226 RVA: 0x00127718 File Offset: 0x00125B18
	public void SetParent(Transform parent, LevelProperties.FlyingBird properties)
	{
		this.properties = properties;
		this.container = new GameObject("Egg Container").transform;
		this.container.SetParent(parent);
		this.container.ResetLocalPosition();
		base.transform.SetParent(this.container);
		base.transform.ResetLocalTransforms();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002023 RID: 8227 RVA: 0x00127781 File Offset: 0x00125B81
	public void Explode()
	{
		base.GetComponent<CircleCollider2D>().enabled = false;
		base.StartCoroutine(this.explode_cr());
	}

	// Token: 0x06002024 RID: 8228 RVA: 0x0012779C File Offset: 0x00125B9C
	public void OnDeathAnimComplete()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06002025 RID: 8229 RVA: 0x001277AC File Offset: 0x00125BAC
	private IEnumerator move_cr()
	{
		yield return base.TweenLocalPositionX(0f, this.properties.CurrentState.smallBird.eggRange.max, this.properties.CurrentState.smallBird.eggMoveTime, EaseUtils.EaseType.easeOutCubic);
		for (;;)
		{
			yield return base.TweenLocalPositionX(this.properties.CurrentState.smallBird.eggRange.max, this.properties.CurrentState.smallBird.eggRange.min, this.properties.CurrentState.smallBird.eggMoveTime, EaseUtils.EaseType.easeInOutSine);
			yield return base.TweenLocalPositionX(this.properties.CurrentState.smallBird.eggRange.min, this.properties.CurrentState.smallBird.eggRange.max, this.properties.CurrentState.smallBird.eggMoveTime, EaseUtils.EaseType.easeInOutSine);
		}
		yield break;
	}

	// Token: 0x06002026 RID: 8230 RVA: 0x001277C8 File Offset: 0x00125BC8
	private IEnumerator explode_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, (float)UnityEngine.Random.Range(0, 1));
		base.transform.SetLocalEulerAngles(new float?(0f), new float?(0f), new float?((float)UnityEngine.Random.Range(0, 360)));
		base.animator.Play("Explode");
		yield break;
	}

	// Token: 0x06002027 RID: 8231 RVA: 0x001277E4 File Offset: 0x00125BE4
	private IEnumerator animSpeed_cr()
	{
		yield return null;
		yield break;
	}

	// Token: 0x040028A1 RID: 10401
	private DamageDealer damageDealer;

	// Token: 0x040028A2 RID: 10402
	private LevelProperties.FlyingBird properties;
}
