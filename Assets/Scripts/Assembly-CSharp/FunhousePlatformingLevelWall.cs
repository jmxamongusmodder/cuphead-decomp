using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008C1 RID: 2241
public class FunhousePlatformingLevelWall : PlatformingLevelBigEnemy
{
	// Token: 0x1700044D RID: 1101
	// (get) Token: 0x06003455 RID: 13397 RVA: 0x001E5EFC File Offset: 0x001E42FC
	public bool IsDead
	{
		get
		{
			return this.isDead;
		}
	}

	// Token: 0x06003456 RID: 13398 RVA: 0x001E5F04 File Offset: 0x001E4304
	protected override void OnLock()
	{
		base.OnLock();
		base.StartCoroutine(this.slide_camera_cr());
	}

	// Token: 0x06003457 RID: 13399 RVA: 0x001E5F1C File Offset: 0x001E431C
	private IEnumerator slide_camera_cr()
	{
		base.GetComponent<Collider2D>().enabled = true;
		CupheadLevelCamera.Current.SetAutoScroll(true);
		CupheadLevelCamera.Current.LockCamera(false);
		float dist = CupheadLevelCamera.Current.transform.position.x - base.transform.position.x;
		while (dist < -500f)
		{
			dist = CupheadLevelCamera.Current.transform.position.x - base.transform.position.x;
			yield return null;
		}
		CupheadLevelCamera.Current.SetAutoScroll(false);
		CupheadLevelCamera.Current.LockCamera(true);
		yield return null;
		yield break;
	}

	// Token: 0x06003458 RID: 13400 RVA: 0x001E5F38 File Offset: 0x001E4338
	protected override void Start()
	{
		base.Start();
		this.LockDistance = 800f;
		base.StartCoroutine(this.shoot_projectiles_cr(base.Properties.funWallTopDelayRange, true));
		base.StartCoroutine(this.shoot_projectiles_cr(base.Properties.funWallBottomDelayRange, false));
		if (this.isTongue)
		{
			base.StartCoroutine(this.spawn_tongue_cr());
		}
		else
		{
			base.StartCoroutine(this.spawn_cars_cr());
		}
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x06003459 RID: 13401 RVA: 0x001E5FBE File Offset: 0x001E43BE
	protected override void Shoot()
	{
		if (this.isDead)
		{
			return;
		}
	}

	// Token: 0x0600345A RID: 13402 RVA: 0x001E5FCC File Offset: 0x001E43CC
	protected override void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.isDead)
		{
			return;
		}
		base.OnDamageTaken(info);
		base.animator.SetTrigger("eyeHit");
		if (!AudioManager.CheckIfPlaying("funhouse_wall1_eye_hit"))
		{
			AudioManager.Play("funhouse_wall1_eye_hit");
			this.emitAudioFromObject.Add("funhouse_wall1_eye_hit");
		}
	}

	// Token: 0x0600345B RID: 13403 RVA: 0x001E6028 File Offset: 0x001E4428
	private IEnumerator shoot_projectiles_cr(MinMax delay, bool isTop)
	{
		while (!this.bigEnemyCameraLock)
		{
			yield return null;
		}
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, delay.RandomFloat());
			string name = (!isTop) ? "Bottom" : "Top";
			base.animator.SetTrigger("horn" + name);
			AudioManager.Play("funhouse_wall1_horn_attack");
			this.emitAudioFromObject.Add("funhouse_wall1_horn_attack");
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600345C RID: 13404 RVA: 0x001E6054 File Offset: 0x001E4454
	private void ShootProjectileTop()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		Vector3 v = next.transform.position - this.topProjectileRoot.transform.position;
		this.hornEffect.Create(this.topProjectileRoot.transform.position);
		this.shootProjectile.Create(this.topProjectileRoot.transform.position, MathUtils.DirectionToAngle(v), base.Properties.funWallProjectileSpeed);
	}

	// Token: 0x0600345D RID: 13405 RVA: 0x001E60DC File Offset: 0x001E44DC
	private void ShootProjectileBottom()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		Vector3 v = next.transform.position - this.bottomProjectileRoot.transform.position;
		this.hornEffect.Create(this.bottomProjectileRoot.transform.position);
		this.shootProjectile.Create(this.bottomProjectileRoot.transform.position, MathUtils.DirectionToAngle(v), base.Properties.funWallProjectileSpeed);
	}

	// Token: 0x0600345E RID: 13406 RVA: 0x001E6164 File Offset: 0x001E4564
	private IEnumerator spawn_cars_cr()
	{
		while (!this.bigEnemyCameraLock)
		{
			yield return null;
		}
		int typeIndex = 0;
		bool isTop = Rand.Bool();
		Vector3 pos = (!isTop) ? this.bottomTransform.position : this.topTransform.position;
		for (;;)
		{
			GameObject blockage = (!isTop) ? this.mouthBlockageBottom : this.mouthBlockageTop;
			base.animator.SetBool("isTop", isTop);
			yield return CupheadTime.WaitForSeconds(this, base.Properties.funWallCarDelayRange.RandomFloat());
			base.animator.SetBool("isOpen", true);
			string name = (!isTop) ? "Bottom" : "Top";
			yield return base.animator.WaitForAnimationToStart(this, name + "_Open_Start", false);
			AudioManager.Play("funhouse_wall1_wall_open_start");
			this.emitAudioFromObject.Add("funhouse_wall1_wall_open_start");
			AudioManager.Play("funhouse_car_honk_sweet");
			this.SpawnHonk((!isTop) ? this.bottomTransform.position.y : this.topTransform.position.y);
			yield return base.animator.WaitForAnimationToEnd(this, name + "_Open_Start", false, true);
			blockage.SetActive(false);
			for (int i = 0; i < 2; i++)
			{
				FunhousePlatformingLevelCar car = UnityEngine.Object.Instantiate<FunhousePlatformingLevelCar>(this.carPrefab);
				car.Init(pos, 180f, base.Properties.funWallCarSpeed, typeIndex, true, true);
				car.transform.SetScale(null, new float?(isTop ? car.transform.localScale.y : (-car.transform.localScale.y)), null);
				typeIndex = ((typeIndex >= 3) ? 0 : (typeIndex + 1));
				yield return CupheadTime.WaitForSeconds(this, this.carDelay);
			}
			yield return CupheadTime.WaitForSeconds(this, base.Properties.funWallMouthOpenTime);
			base.animator.SetBool("isOpen", false);
			AudioManager.Play("funhouse_wall1_wall_close");
			this.emitAudioFromObject.Add("funhouse_wall1_wall_close");
			blockage.SetActive(true);
			isTop = !isTop;
			pos = ((!isTop) ? this.bottomTransform.position : this.topTransform.position);
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600345F RID: 13407 RVA: 0x001E6180 File Offset: 0x001E4580
	private void SpawnHonk(float rootY)
	{
		Vector2 v = new Vector2(CupheadLevelCamera.Current.Bounds.xMax, rootY);
		this.honkEffect.Create(v).transform.parent = CupheadLevelCamera.Current.transform;
	}

	// Token: 0x06003460 RID: 13408 RVA: 0x001E61CC File Offset: 0x001E45CC
	private IEnumerator spawn_tongue_cr()
	{
		while (!this.bigEnemyCameraLock)
		{
			yield return null;
		}
		bool isTop = Rand.Bool();
		Vector3 pos = (!isTop) ? this.bottomTransform.position : this.topTransform.position;
		for (;;)
		{
			GameObject blockage = (!(pos == this.bottomTransform.position)) ? this.mouthBlockageTop : this.mouthBlockageBottom;
			base.animator.SetBool("isTop", isTop);
			yield return CupheadTime.WaitForSeconds(this, base.Properties.funWallTongueDelayRange.RandomFloat());
			base.animator.SetBool("isOpen", true);
			string name = (!isTop) ? "Bottom" : "Top";
			yield return CupheadTime.WaitForSeconds(this, 0.8f);
			base.animator.SetTrigger("Continue");
			yield return base.animator.WaitForAnimationToEnd(this, name + "_Open_Start", false, true);
			AudioManager.Play("funhouse_wall1_wall_open_start");
			this.emitAudioFromObject.Add("funhouse_wall1_wall_open_start");
			blockage.SetActive(false);
			this.tongue.transform.SetScale(null, new float?((float)((!isTop) ? 1 : -1)), null);
			this.tongue.transform.position = pos;
			this.tongue.GetComponent<Animator>().SetBool("IsTongue", true);
			AudioManager.Play("funhouse_funwall_tounge_intro");
			this.emitAudioFromObject.Add("funhouse_funwall_tounge_intro");
			yield return CupheadTime.WaitForSeconds(this, base.Properties.funWallTongueLoopTime);
			this.tongue.GetComponent<Animator>().SetBool("IsTongue", false);
			AudioManager.Play("funhouse_funwall_tounge_outro");
			this.emitAudioFromObject.Add("funhouse_funwall_tounge_outro");
			yield return this.tongue.GetComponent<Animator>().WaitForAnimationToEnd(this, "Outro", false, true);
			yield return CupheadTime.WaitForSeconds(this, base.Properties.funWallMouthOpenTime);
			base.animator.SetBool("isOpen", false);
			AudioManager.Play("funhouse_wall1_wall_close");
			this.emitAudioFromObject.Add("funhouse_wall1_wall_close");
			blockage.SetActive(true);
			isTop = !isTop;
			pos = ((!isTop) ? this.bottomTransform.position : this.topTransform.position);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003461 RID: 13409 RVA: 0x001E61E7 File Offset: 0x001E45E7
	protected override void OnPass()
	{
		base.OnPass();
		this.StopAllCoroutines();
		this.Die();
	}

	// Token: 0x06003462 RID: 13410 RVA: 0x001E61FC File Offset: 0x001E45FC
	protected override void Die()
	{
		this.isDead = true;
		base.GetComponent<Collider2D>().enabled = false;
		this.deadBlockage.SetActive(true);
		if (CupheadLevelCamera.Current.autoScrolling)
		{
			CupheadLevelCamera.Current.SetAutoScroll(false);
		}
		CupheadLevelCamera.Current.LockCamera(false);
		this.mouthBlockageBottom.SetActive(false);
		this.mouthBlockageTop.SetActive(false);
		this.middleBlockage.SetActive(false);
		if (this.tongue != null)
		{
			this.tongue.gameObject.SetActive(false);
		}
		this.StopAllCoroutines();
		base.StartCoroutine(this.explode_cr());
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetTrigger("Dead");
		AudioManager.Play("funhouse_wall_death");
		this.emitAudioFromObject.Add("funhouse_wall_death");
	}

	// Token: 0x06003463 RID: 13411 RVA: 0x001E62DC File Offset: 0x001E46DC
	private IEnumerator explode_cr()
	{
		this.explosion.StartExplosion();
		yield return CupheadTime.WaitForSeconds(this, 3f);
		this.explosion.StopExplosions();
		yield break;
	}

	// Token: 0x06003464 RID: 13412 RVA: 0x001E62F7 File Offset: 0x001E46F7
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.carPrefab = null;
		this.shootProjectile = null;
		this.hornEffect = null;
		this.honkEffect = null;
	}

	// Token: 0x04003C85 RID: 15493
	[SerializeField]
	private Effect hornEffect;

	// Token: 0x04003C86 RID: 15494
	[SerializeField]
	private Effect honkEffect;

	// Token: 0x04003C87 RID: 15495
	[SerializeField]
	private bool isTongue;

	// Token: 0x04003C88 RID: 15496
	[SerializeField]
	private FunhousePlatformingLevelCar carPrefab;

	// Token: 0x04003C89 RID: 15497
	[SerializeField]
	private BasicProjectile shootProjectile;

	// Token: 0x04003C8A RID: 15498
	[SerializeField]
	private GameObject mouthBlockageTop;

	// Token: 0x04003C8B RID: 15499
	[SerializeField]
	private GameObject mouthBlockageBottom;

	// Token: 0x04003C8C RID: 15500
	[SerializeField]
	private GameObject middleBlockage;

	// Token: 0x04003C8D RID: 15501
	[SerializeField]
	private GameObject deadBlockage;

	// Token: 0x04003C8E RID: 15502
	[SerializeField]
	private Transform tongue;

	// Token: 0x04003C8F RID: 15503
	[SerializeField]
	private Transform topTransform;

	// Token: 0x04003C90 RID: 15504
	[SerializeField]
	private Transform bottomTransform;

	// Token: 0x04003C91 RID: 15505
	[SerializeField]
	private Transform topProjectileRoot;

	// Token: 0x04003C92 RID: 15506
	[SerializeField]
	private Transform bottomProjectileRoot;

	// Token: 0x04003C93 RID: 15507
	[SerializeField]
	private LevelBossDeathExploder explosion;

	// Token: 0x04003C94 RID: 15508
	private float carDelay = 0.7f;
}
