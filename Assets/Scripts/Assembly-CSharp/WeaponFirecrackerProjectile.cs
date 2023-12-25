using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A7B RID: 2683
public class WeaponFirecrackerProjectile : BasicProjectile
{
	// Token: 0x0600401D RID: 16413 RVA: 0x0022F4A0 File Offset: 0x0022D8A0
	protected override void Update()
	{
		base.Update();
		if (this.parent != null && !this.brokeOffFromParent && this.parent.transform.localScale.x != this.player.motor.LookDirection.x)
		{
			base.transform.SetParent(null, true);
			this.brokeOffFromParent = true;
		}
	}

	// Token: 0x0600401E RID: 16414 RVA: 0x0022F51D File Offset: 0x0022D91D
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		this.hitEnemy = true;
		this.animator.Play("Hit");
		this.collider.enabled = false;
	}

	// Token: 0x0600401F RID: 16415 RVA: 0x0022F54C File Offset: 0x0022D94C
	public void SetupFirecracker(Transform parent, LevelPlayerController player, bool isTypeB)
	{
		base.transform.SetParent(parent, true);
		this.parent = parent;
		this.player = player;
		this.distanceTraveled = 0f;
		if (isTypeB)
		{
			base.StartCoroutine(this.bullet_life_B_cr());
		}
		else
		{
			base.StartCoroutine(this.bullet_life_cr());
		}
	}

	// Token: 0x06004020 RID: 16416 RVA: 0x0022F5A4 File Offset: 0x0022D9A4
	public void StillBullet()
	{
		this.move = false;
		base.StartCoroutine(this.bullet_slice_life_cr());
	}

	// Token: 0x06004021 RID: 16417 RVA: 0x0022F5BC File Offset: 0x0022D9BC
	private IEnumerator bullet_life_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.bulletLife);
		this.move = false;
		this.collider.enabled = true;
		base.transform.SetScale(new float?(this.explosionSize), new float?(this.explosionSize), null);
		yield return CupheadTime.WaitForSeconds(this, this.explosionDuration);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06004022 RID: 16418 RVA: 0x0022F5D8 File Offset: 0x0022D9D8
	private IEnumerator bullet_life_B_cr()
	{
		float explodeDistance = this.bulletLife * this.Speed;
		while (this.distanceTraveled < explodeDistance)
		{
			yield return null;
		}
		this.move = false;
		WeaponFirecrackerProjectile slice = UnityEngine.Object.Instantiate<WeaponFirecrackerProjectile>(this.projectile);
		Vector3 dir = MathUtils.AngleToDirection(this.explosionAngle);
		slice.transform.position = base.transform.position + dir * this.explosionRadiusSize;
		slice.collider.enabled = true;
		slice.collider.transform.SetScale(new float?(this.explosionSize), new float?(this.explosionSize), null);
		slice.DamageRate = this.DamageRate;
		slice.StillBullet();
		slice.gameObject.name = "FirecrackerExplosion";
		slice.transform.eulerAngles = new Vector3(0f, 0f, this.explosionAngle);
		UnityEngine.Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x06004023 RID: 16419 RVA: 0x0022F5F4 File Offset: 0x0022D9F4
	private IEnumerator bullet_slice_life_cr()
	{
		this.hitEnemy = false;
		this.animator.Play("Die");
		yield return CupheadTime.WaitForSeconds(this, this.explosionDuration);
		if (this.hitEnemy)
		{
			SpriteRenderer sprite = base.GetComponent<SpriteRenderer>();
			while (sprite.enabled)
			{
				yield return null;
			}
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x06004024 RID: 16420 RVA: 0x0022F610 File Offset: 0x0022DA10
	protected override void Move()
	{
		this.moveVector = base.transform.right * this.Speed * CupheadTime.FixedDelta - new Vector3(0f, this._accumulativeGravity * CupheadTime.FixedDelta, 0f);
		base.transform.position += this.moveVector;
		this.distanceTraveled += this.moveVector.magnitude;
	}

	// Token: 0x040046E0 RID: 18144
	[SerializeField]
	private WeaponFirecrackerProjectile projectile;

	// Token: 0x040046E1 RID: 18145
	public float bulletLife;

	// Token: 0x040046E2 RID: 18146
	public float explosionSize;

	// Token: 0x040046E3 RID: 18147
	public float explosionDuration;

	// Token: 0x040046E4 RID: 18148
	public float explosionRadiusSize;

	// Token: 0x040046E5 RID: 18149
	public float explosionAngle;

	// Token: 0x040046E6 RID: 18150
	private Transform parent;

	// Token: 0x040046E7 RID: 18151
	private LevelPlayerController player;

	// Token: 0x040046E8 RID: 18152
	private float parentScaleX;

	// Token: 0x040046E9 RID: 18153
	private bool brokeOffFromParent;

	// Token: 0x040046EA RID: 18154
	private Vector3 moveVector;

	// Token: 0x040046EB RID: 18155
	private float distanceTraveled;

	// Token: 0x040046EC RID: 18156
	public Collider2D collider;

	// Token: 0x040046ED RID: 18157
	public new Animator animator;

	// Token: 0x040046EE RID: 18158
	private bool hitEnemy;
}
