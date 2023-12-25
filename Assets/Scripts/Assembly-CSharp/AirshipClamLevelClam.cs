using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004CD RID: 1229
public class AirshipClamLevelClam : LevelProperties.AirshipClam.Entity
{
	// Token: 0x060014DE RID: 5342 RVA: 0x000BAB03 File Offset: 0x000B8F03
	protected override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.Awake();
	}

	// Token: 0x060014DF RID: 5343 RVA: 0x000BAB39 File Offset: 0x000B8F39
	private void Update()
	{
		this.damageDealer.Update();
	}

	// Token: 0x060014E0 RID: 5344 RVA: 0x000BAB46 File Offset: 0x000B8F46
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x060014E1 RID: 5345 RVA: 0x000BAB5C File Offset: 0x000B8F5C
	public override void LevelInit(LevelProperties.AirshipClam properties)
	{
		base.LevelInit(properties);
		this.pivotPoint.x = (float)(Level.Current.Left + Level.Current.Width / 2);
		this.pivotPoint.y = (float)Level.Current.Ground + (float)Level.Current.Height * 0.65f;
		this.pivotPoint.z = 0f;
		this.attacking = false;
		this.clamOut = false;
		this.time = 0f;
		this.idleSpeed = properties.CurrentState.spit.movementSpeedScale;
		this.pShotAttackDelayIndex = UnityEngine.Random.Range(0, properties.CurrentState.spit.attackDelayString.Split(new char[]
		{
			','
		}).Length);
		this.barnacleAttackDelayIndex = UnityEngine.Random.Range(0, properties.CurrentState.barnacles.attackDelayString.Split(new char[]
		{
			','
		}).Length);
		this.barnacleTypeIndex = UnityEngine.Random.Range(0, properties.CurrentState.barnacles.typeString.Split(new char[]
		{
			','
		}).Length);
		this.clamOutShotCountIndex = UnityEngine.Random.Range(0, properties.CurrentState.clamOut.shotString.Split(new char[]
		{
			','
		}).Length);
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060014E2 RID: 5346 RVA: 0x000BACC0 File Offset: 0x000B90C0
	private IEnumerator move_cr()
	{
		for (;;)
		{
			if (!this.attacking)
			{
				Vector3 pos = this.pivotPoint + Vector3.right * Mathf.Sin(this.time * this.idleSpeed) * 300f;
				base.transform.position = pos + Vector3.up * Mathf.Sin(this.time * (this.idleSpeed * 4f)) * 50f;
				this.time += CupheadTime.Delta;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060014E3 RID: 5347 RVA: 0x000BACDB File Offset: 0x000B90DB
	public void OnSpitStart(Action callback)
	{
		this.callback = callback;
		base.StartCoroutine(this.spit_cr());
	}

	// Token: 0x060014E4 RID: 5348 RVA: 0x000BACF4 File Offset: 0x000B90F4
	private IEnumerator spit_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.spit.initialShotDelay);
		this.attacking = true;
		base.animator.SetTrigger("OnPearlShot");
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.spit.preShotDelay);
		Vector3 target = PlayerManager.GetNext().center + Vector3.up * 50f;
		float rotation = Vector3.Angle(Vector3.down, base.transform.position - target);
		if (target.x > base.transform.position.x)
		{
			rotation += 270f;
		}
		else
		{
			rotation = 270f - rotation;
		}
		this.pearlPrefab.Create(this.spawnPoints[0].position, -rotation, base.properties.CurrentState.spit.bulletSpeed);
		base.animator.SetTrigger("OnPearlShot");
		yield return base.animator.WaitForAnimationToEnd(this, true);
		this.attacking = false;
		yield return CupheadTime.WaitForSeconds(this, Parser.FloatParse(base.properties.CurrentState.spit.attackDelayString.Split(new char[]
		{
			','
		})[this.pShotAttackDelayIndex]));
		this.pShotAttackDelayIndex++;
		if (this.pShotAttackDelayIndex >= base.properties.CurrentState.spit.attackDelayString.Split(new char[]
		{
			','
		}).Length)
		{
			this.pShotAttackDelayIndex = 0;
		}
		if (this.callback != null)
		{
			this.callback();
		}
		yield break;
	}

	// Token: 0x060014E5 RID: 5349 RVA: 0x000BAD0F File Offset: 0x000B910F
	public void OnBarnaclesStart(Action callback)
	{
		this.callback = callback;
		base.StartCoroutine(this.spawnBarnacles_cr());
	}

	// Token: 0x060014E6 RID: 5350 RVA: 0x000BAD28 File Offset: 0x000B9128
	private IEnumerator spawnBarnacles_cr()
	{
		bool parryable = false;
		for (float duration = base.properties.CurrentState.barnacles.attackDuration.RandomFloat(); duration > 0f; duration -= Parser.FloatParse(base.properties.CurrentState.barnacles.attackDelayString.Split(new char[]
		{
			','
		})[this.barnacleAttackDelayIndex]))
		{
			if (base.properties.CurrentState.barnacles.typeString.Split(new char[]
			{
				','
			})[this.barnacleTypeIndex][0] == 'P')
			{
				parryable = true;
			}
			this.barnacleTypeIndex++;
			if (this.barnacleTypeIndex >= base.properties.CurrentState.barnacles.typeString.Split(new char[]
			{
				','
			}).Length)
			{
				this.barnacleTypeIndex = 0;
			}
			if (!parryable)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.barnaclePrefab.gameObject, this.spawnPoints[0].position, Quaternion.identity);
				gameObject.transform.localScale = Vector3.one * base.properties.CurrentState.barnacles.barnacleScale;
				gameObject.GetComponent<AirshipClamLevelBarnacle>().InitBarnacle(-1, base.properties);
				gameObject = UnityEngine.Object.Instantiate<GameObject>(this.barnaclePrefab.gameObject, this.spawnPoints[1].position, Quaternion.identity);
				gameObject.transform.localScale = Vector3.one * base.properties.CurrentState.barnacles.barnacleScale;
				gameObject.GetComponent<AirshipClamLevelBarnacle>().InitBarnacle(1, base.properties);
			}
			else
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.barnacleParryablePrefab.gameObject, this.spawnPoints[0].position, Quaternion.identity);
				gameObject2.transform.localScale = Vector3.one * base.properties.CurrentState.barnacles.barnacleScale;
				gameObject2.GetComponent<AirshipClamLevelBarnacleParryable>().InitBarnacle(-1, base.properties);
				gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.barnacleParryablePrefab.gameObject, this.spawnPoints[1].position, Quaternion.identity);
				gameObject2.transform.localScale = Vector3.one * base.properties.CurrentState.barnacles.barnacleScale;
				gameObject2.GetComponent<AirshipClamLevelBarnacleParryable>().InitBarnacle(1, base.properties);
			}
			yield return CupheadTime.WaitForSeconds(this, Parser.FloatParse(base.properties.CurrentState.barnacles.attackDelayString.Split(new char[]
			{
				','
			})[this.barnacleAttackDelayIndex]));
		}
		this.barnacleAttackDelayIndex++;
		if (this.barnacleAttackDelayIndex >= base.properties.CurrentState.barnacles.attackDelayString.Split(new char[]
		{
			','
		}).Length)
		{
			this.barnacleAttackDelayIndex = 0;
		}
		if (!this.clamOut && this.callback != null)
		{
			this.callback();
		}
		yield break;
	}

	// Token: 0x060014E7 RID: 5351 RVA: 0x000BAD43 File Offset: 0x000B9143
	private void OnStringShot()
	{
		base.animator.SetBool("OnStringShot", true);
		base.StartCoroutine(this.clamOut_cr());
	}

	// Token: 0x060014E8 RID: 5352 RVA: 0x000BAD64 File Offset: 0x000B9164
	private IEnumerator clamOut_cr()
	{
		this.damageReceiver.enabled = true;
		int max = Parser.IntParse(base.properties.CurrentState.clamOut.shotString.Split(new char[]
		{
			','
		})[this.clamOutShotCountIndex]);
		Vector3 target = PlayerManager.GetNext().center + Vector3.up * 50f;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.clamOut.preShotDelay);
		for (int i = 0; i < max; i++)
		{
			if (i != 0)
			{
				yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.clamOut.bulletRepeatDelay);
			}
			target = PlayerManager.GetNext().center;
			target = PlayerManager.GetNext().center + Vector3.up * 50f;
			float rotation = Vector3.Angle(Vector3.down, base.transform.position - target);
			if (target.x > base.transform.position.x)
			{
				rotation += 270f;
			}
			else
			{
				rotation = 270f - rotation;
			}
			this.pearlPrefab.Create(this.spawnPoints[0].position, -rotation, base.properties.CurrentState.spit.bulletSpeed);
		}
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.clamOut.bulletMainDelay);
		this.clamOutShotCountIndex++;
		if (this.clamOutShotCountIndex >= base.properties.CurrentState.clamOut.shotString.Split(new char[]
		{
			','
		}).Length)
		{
			this.clamOutShotCountIndex = 0;
		}
		base.animator.SetBool("OnStringShot", false);
		yield return base.animator.WaitForAnimationToEnd(this, true);
		this.damageReceiver.enabled = false;
		if (this.callback != null)
		{
			this.callback();
		}
		yield break;
	}

	// Token: 0x060014E9 RID: 5353 RVA: 0x000BAD7F File Offset: 0x000B917F
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060014EA RID: 5354 RVA: 0x000BADA4 File Offset: 0x000B91A4
	protected override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		if (!this.attacking)
		{
			AirshipClamLevelBarnacleParryable component = hit.GetComponent<AirshipClamLevelBarnacleParryable>();
			if (component != null && component.parried)
			{
				base.animator.SetBool("OnBarnacles", false);
				this.OnStringShot();
				UnityEngine.Object.Destroy(hit.gameObject);
			}
		}
		base.OnCollisionOther(hit, phase);
	}

	// Token: 0x060014EB RID: 5355 RVA: 0x000BAE04 File Offset: 0x000B9204
	protected override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
	}

	// Token: 0x04001E3C RID: 7740
	[SerializeField]
	private BasicProjectile pearlPrefab;

	// Token: 0x04001E3D RID: 7741
	[SerializeField]
	private AirshipClamLevelBarnacle barnaclePrefab;

	// Token: 0x04001E3E RID: 7742
	[SerializeField]
	private AirshipClamLevelBarnacleParryable barnacleParryablePrefab;

	// Token: 0x04001E3F RID: 7743
	private bool attacking;

	// Token: 0x04001E40 RID: 7744
	private float idleSpeed;

	// Token: 0x04001E41 RID: 7745
	private int pShotAttackDelayIndex;

	// Token: 0x04001E42 RID: 7746
	private int barnacleAttackDelayIndex;

	// Token: 0x04001E43 RID: 7747
	private int barnacleTypeIndex;

	// Token: 0x04001E44 RID: 7748
	private bool clamOut;

	// Token: 0x04001E45 RID: 7749
	private int clamOutShotCountIndex;

	// Token: 0x04001E46 RID: 7750
	private Vector3 pivotPoint;

	// Token: 0x04001E47 RID: 7751
	[SerializeField]
	private Transform[] spawnPoints;

	// Token: 0x04001E48 RID: 7752
	private Action callback;

	// Token: 0x04001E49 RID: 7753
	private float time;

	// Token: 0x04001E4A RID: 7754
	private DamageDealer damageDealer;

	// Token: 0x04001E4B RID: 7755
	private DamageReceiver damageReceiver;
}
