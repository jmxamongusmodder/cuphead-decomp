using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200063A RID: 1594
public class FlyingBlimpLevelGeminiShoot : AbstractCollidableObject
{
	// Token: 0x060020B1 RID: 8369 RVA: 0x0012DC20 File Offset: 0x0012C020
	public void Init(LevelProperties.FlyingBlimp.Gemini properties, Vector2 pos)
	{
		this.properties = properties;
		base.transform.position = pos;
		this.smallRadius = base.GetComponent<CircleCollider2D>().radius;
		float num = (float)UnityEngine.Random.Range(0, 2);
		this.pointingUp = (num == 0f);
		if (this.pointingUp)
		{
			this.projectileRoot = this.projectileRootUp;
		}
		else
		{
			this.projectileRoot = this.projectileRootDown;
		}
		base.StartCoroutine(this.rotate_cr());
	}

	// Token: 0x060020B2 RID: 8370 RVA: 0x0012DCAC File Offset: 0x0012C0AC
	private IEnumerator rotate_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		AudioManager.Play("level_flying_blimp_wheel_start");
		yield return CupheadTime.WaitForSeconds(this, 1f);
		base.animator.SetBool("Attack", true);
		this.smallFXSpawning = true;
		base.StartCoroutine(this.spawn_small_fx_cr());
		AudioManager.PlayLoop("level_flying_blimp_gemini_sphere_attack");
		float pct = 0f;
		float startRotation = (float)((!Rand.Bool()) ? -360 : 360);
		while (pct <= 1f)
		{
			base.transform.SetEulerAngles(null, null, new float?(startRotation * pct));
			pct += CupheadTime.FixedDelta * this.properties.rotationSpeed;
			this.ShootBullet();
			yield return wait;
		}
		base.transform.SetEulerAngles(null, null, new float?((float)((startRotation != 360f) ? 360 : -360)));
		this.smallFXSpawning = false;
		base.animator.SetBool("Attack", false);
		base.animator.SetTrigger("Leave");
		AudioManager.Stop("level_flying_blimp_gemini_sphere_attack");
		AudioManager.Play("level_flying_blimp_wheel_end");
		yield break;
	}

	// Token: 0x060020B3 RID: 8371 RVA: 0x0012DCC8 File Offset: 0x0012C0C8
	private void ShootBullet()
	{
		float x = this.projectileRoot.position.x - base.transform.position.x;
		float y = this.projectileRoot.position.y - base.transform.position.y;
		float rotation = Mathf.Atan2(y, x) * 57.29578f;
		if (this.delayTime < this.properties.bulletDelay)
		{
			this.delayTime += 1f;
		}
		else
		{
			this.projectilePrefab.Create(this.projectileRoot.position, rotation, this.properties.bulletSpeed);
			this.delayTime = 0f;
		}
	}

	// Token: 0x060020B4 RID: 8372 RVA: 0x0012DD98 File Offset: 0x0012C198
	private IEnumerator spawn_small_fx_cr()
	{
		while (this.smallFXSpawning)
		{
			GameObject small = UnityEngine.Object.Instantiate<GameObject>(this.smallFX);
			Vector3 scale = new Vector3(1f, 1f, 1f);
			scale.x = ((!Rand.Bool()) ? (-scale.x) : scale.x);
			scale.y = ((!Rand.Bool()) ? (-scale.y) : scale.y);
			small.transform.SetScale(new float?(scale.x), new float?(scale.y), new float?(1f));
			small.transform.eulerAngles = new Vector3(0f, 0f, UnityEngine.Random.Range(0f, 360f));
			small.GetComponent<SpriteRenderer>().sortingOrder = UnityEngine.Random.Range(0, 3);
			small.transform.position = this.GetRandomPoint();
			base.StartCoroutine(this.delete_small_fx(small));
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
		}
		yield break;
	}

	// Token: 0x060020B5 RID: 8373 RVA: 0x0012DDB4 File Offset: 0x0012C1B4
	private Vector2 GetRandomPoint()
	{
		Vector2 a = base.transform.position;
		Vector2 vector = new Vector2((float)UnityEngine.Random.Range(-1, 1), (float)UnityEngine.Random.Range(-1, 1));
		Vector2 b = vector.normalized * (this.smallRadius * UnityEngine.Random.value) * 2f;
		return a + b;
	}

	// Token: 0x060020B6 RID: 8374 RVA: 0x0012DE14 File Offset: 0x0012C214
	private IEnumerator delete_small_fx(GameObject smallFX)
	{
		yield return smallFX.GetComponent<Animator>().WaitForAnimationToEnd(this, "SmallFX", false, true);
		UnityEngine.Object.Destroy(smallFX);
		yield break;
	}

	// Token: 0x060020B7 RID: 8375 RVA: 0x0012DE36 File Offset: 0x0012C236
	private void Die()
	{
		AudioManager.Play("level_flying_blimp_gemini_sphere_leave");
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04002937 RID: 10551
	private LevelProperties.FlyingBlimp.Gemini properties;

	// Token: 0x04002938 RID: 10552
	[SerializeField]
	private GameObject smallFX;

	// Token: 0x04002939 RID: 10553
	[SerializeField]
	private Transform projectileRootUp;

	// Token: 0x0400293A RID: 10554
	[SerializeField]
	private Transform projectileRootDown;

	// Token: 0x0400293B RID: 10555
	[SerializeField]
	private BasicProjectile projectilePrefab;

	// Token: 0x0400293C RID: 10556
	private float smallRadius;

	// Token: 0x0400293D RID: 10557
	private Transform projectileRoot;

	// Token: 0x0400293E RID: 10558
	private Vector3 target;

	// Token: 0x0400293F RID: 10559
	private Quaternion startRotation;

	// Token: 0x04002940 RID: 10560
	private float rotationTime;

	// Token: 0x04002941 RID: 10561
	private float delayTime;

	// Token: 0x04002942 RID: 10562
	private bool pointingUp;

	// Token: 0x04002943 RID: 10563
	private bool smallFXSpawning;

	// Token: 0x04002944 RID: 10564
	private bool halfWay;
}
