using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200060D RID: 1549
public class FlowerLevelMiniFlowerSpawn : AbstractCollidableObject
{
	// Token: 0x06001F30 RID: 7984 RVA: 0x0011E604 File Offset: 0x0011CA04
	public void OnMiniFlowerSpawn(FlowerLevelFlower parent, LevelProperties.Flower.EnemyPlants properties)
	{
		this.properties = properties;
		this.currentSpeed = this.properties.miniFlowerMovmentSpeed;
		this.currentHP = (float)this.properties.miniFlowerPlantHP;
		this.parent = parent;
		this.parent.OnDeathEvent += this.HandleEnd;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001F31 RID: 7985 RVA: 0x0011E668 File Offset: 0x0011CA68
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.currentHP -= info.damage;
		if (this.currentHP <= 0f)
		{
			if (this.isDead)
			{
				return;
			}
			this.isDead = true;
			this.parent.OnMiniFlowerDeath();
			base.animator.SetTrigger("OnDeath");
			this.explosion.GetComponent<Animator>().SetInteger("Variant", 1);
			this.explosion.GetComponent<Animator>().SetTrigger("OnDeath");
			base.GetComponent<Collider2D>().enabled = false;
			base.StartCoroutine(this.die_cr());
			this.currentSpeed = 0;
			this.explosion.Rotate(Vector3.forward, (float)UnityEngine.Random.Range(0, 360));
		}
	}

	// Token: 0x06001F32 RID: 7986 RVA: 0x0011E730 File Offset: 0x0011CB30
	public void SpawnPetals()
	{
		base.GetComponent<Collider2D>().enabled = false;
		Vector3 vector = this.spawnPoint.transform.position + Vector3.up * (float)UnityEngine.Random.Range(-10, 10);
		if (!this.isFriendly)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.petalA, vector, Quaternion.identity);
			gameObject.GetComponent<Animator>().Play("PetalA_Red", UnityEngine.Random.Range(0, 1));
			base.StartCoroutine(this.fade_cr(gameObject, 0.8f, false));
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.petalB, vector + Vector3.down * 50f, Quaternion.identity);
			gameObject.GetComponent<Animator>().Play("PetalB_Red_Loop");
			base.StartCoroutine(this.fade_cr(gameObject, 1f, false));
		}
		else
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.petalA, vector, Quaternion.identity);
			gameObject2.GetComponent<Animator>().Play("PetalA_Blue", UnityEngine.Random.Range(0, 1));
			base.StartCoroutine(this.fade_cr(gameObject2, 0.8f, false));
			gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.petalB, vector + Vector3.down * 50f, Quaternion.identity);
			gameObject2.GetComponent<Animator>().Play("PetalB_Blue_Loop");
			base.StartCoroutine(this.fade_cr(gameObject2, 1f, true));
		}
	}

	// Token: 0x06001F33 RID: 7987 RVA: 0x0011E894 File Offset: 0x0011CC94
	private IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			if (!this.isAttacking && this.isActive)
			{
				float num = Mathf.Sin(this.attackTime * (float)this.currentSpeed / 3f);
				num = Mathf.Clamp(num, -2f, 2f);
				this.attackTime += CupheadTime.FixedDelta;
				Vector3 position = Vector3.Lerp(base.transform.position, this.pivotPoint + this.flightDirection * num * 4000f * CupheadTime.FixedDelta, 0.03f * CupheadTime.GlobalSpeed);
				base.transform.position = position;
				float z = 15f * Mathf.Sin(num) * -Mathf.Sign(this.flightDirection.x);
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.Euler(0f, 0f, z), 8f);
			}
			else
			{
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.Euler(0f, 0f, 0f), 10f);
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06001F34 RID: 7988 RVA: 0x0011E8B0 File Offset: 0x0011CCB0
	private IEnumerator die_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "Death", 0, true, true);
		base.GetComponent<SpriteRenderer>().enabled = false;
		yield break;
	}

	// Token: 0x06001F35 RID: 7989 RVA: 0x0011E8CC File Offset: 0x0011CCCC
	private IEnumerator fade_cr(GameObject petal, float duration, bool lastPetal = false)
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		SpriteRenderer petalSprite = petal.GetComponent<SpriteRenderer>();
		float currentTime = duration;
		float pct = currentTime / duration;
		while (pct >= 0f)
		{
			Color c = petalSprite.material.color;
			c.a = pct;
			petalSprite.material.color = c;
			petalSprite.transform.position += Vector3.down * 100f * CupheadTime.FixedDelta;
			currentTime -= CupheadTime.FixedDelta;
			pct = currentTime / duration;
			yield return wait;
		}
		UnityEngine.Object.Destroy(petal);
		if (lastPetal)
		{
			this.Die();
		}
		yield break;
	}

	// Token: 0x06001F36 RID: 7990 RVA: 0x0011E8FC File Offset: 0x0011CCFC
	public void FriendlyFireDamage()
	{
	}

	// Token: 0x06001F37 RID: 7991 RVA: 0x0011E8FE File Offset: 0x0011CCFE
	private void HandleEnd()
	{
		if (this.isFriendly)
		{
			base.GetComponent<Collider2D>().enabled = false;
			this.StopAllCoroutines();
		}
		else
		{
			this.Die();
		}
	}

	// Token: 0x06001F38 RID: 7992 RVA: 0x0011E928 File Offset: 0x0011CD28
	private void Die()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06001F39 RID: 7993 RVA: 0x0011E948 File Offset: 0x0011CD48
	private IEnumerator initialFlight_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		while (base.transform.position.y < this.pivotPoint.y)
		{
			base.transform.position += this.flightDirection * (float)this.currentSpeed * CupheadTime.GlobalSpeed;
			yield return wait;
		}
		this.isActive = true;
		this.attackTime = 0f;
		if (base.transform.position.x < this.pivotPoint.x)
		{
			this.flightDirection = Vector3.right * (float)this.currentSpeed;
		}
		else
		{
			this.flightDirection = Vector3.left * (float)this.currentSpeed;
		}
		base.StartCoroutine(this.attackDelay_cr());
		yield return wait;
		yield break;
	}

	// Token: 0x06001F3A RID: 7994 RVA: 0x0011E964 File Offset: 0x0011CD64
	private IEnumerator attackDelay_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(this.properties.miniFlowerShootDelay.min, this.properties.miniFlowerShootDelay.max));
			if (!this.isAttacking)
			{
				base.animator.SetTrigger("OnAttack");
				this.isAttacking = true;
			}
		}
		yield break;
	}

	// Token: 0x06001F3B RID: 7995 RVA: 0x0011E980 File Offset: 0x0011CD80
	protected override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.isFriendly = false;
		this.flightDirection = Vector3.up;
		this.pivotPoint = new Vector3((float)Level.Current.Left + (float)Level.Current.Width / 2.5f, (float)(Level.Current.Ceiling - Level.Current.Height / 8), 0f);
		base.Awake();
	}

	// Token: 0x06001F3C RID: 7996 RVA: 0x0011EA18 File Offset: 0x0011CE18
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		this.damageReceiver.enabled = this.isAttacking;
	}

	// Token: 0x06001F3D RID: 7997 RVA: 0x0011EA44 File Offset: 0x0011CE44
	protected override void OnDestroy()
	{
		AudioManager.Play("flower_minion_simple_deathpop_low");
		this.emitAudioFromObject.Add("flower_minion_simple_deathpop_low");
		this.StopAllCoroutines();
		this.parent.OnDeathEvent -= this.HandleEnd;
		base.OnDestroy();
		this.bulletPrefab = null;
		this.petalA = null;
		this.petalB = null;
	}

	// Token: 0x06001F3E RID: 7998 RVA: 0x0011EAA3 File Offset: 0x0011CEA3
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001F3F RID: 7999 RVA: 0x0011EAC1 File Offset: 0x0011CEC1
	private void OnIntroEnd()
	{
		base.StartCoroutine(this.initialFlight_cr());
	}

	// Token: 0x06001F40 RID: 8000 RVA: 0x0011EAD0 File Offset: 0x0011CED0
	private void StartedShooting()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.bulletPrefab, this.spawnPoint.transform.position, Quaternion.identity);
		if (this.isFriendly)
		{
			gameObject.GetComponent<FlowerLevelMiniFlowerBullet>().OnBulletSpawned(this.parent.attackPoint.transform.position, this.properties.miniFlowerProjectileSpeed, (float)this.properties.miniFlowerProjectileDamage, true);
		}
		else
		{
			gameObject.GetComponent<FlowerLevelMiniFlowerBullet>().OnBulletSpawned(PlayerManager.GetNext().center, this.properties.miniFlowerProjectileSpeed, (float)this.properties.miniFlowerProjectileDamage, false);
		}
	}

	// Token: 0x06001F41 RID: 8001 RVA: 0x0011EB73 File Offset: 0x0011CF73
	private void EndedShooting()
	{
		this.isAttacking = false;
	}

	// Token: 0x040027C5 RID: 10181
	private const float easingValue = 0.03f;

	// Token: 0x040027C6 RID: 10182
	private const float strength = 4000f;

	// Token: 0x040027C7 RID: 10183
	private float attackTime;

	// Token: 0x040027C8 RID: 10184
	private float currentHP;

	// Token: 0x040027C9 RID: 10185
	private int currentSpeed;

	// Token: 0x040027CA RID: 10186
	private bool isFriendly;

	// Token: 0x040027CB RID: 10187
	private bool isAttacking;

	// Token: 0x040027CC RID: 10188
	private bool isActive;

	// Token: 0x040027CD RID: 10189
	private Vector3 flightDirection;

	// Token: 0x040027CE RID: 10190
	private Vector3 pivotPoint;

	// Token: 0x040027CF RID: 10191
	private FlowerLevelFlower parent;

	// Token: 0x040027D0 RID: 10192
	[SerializeField]
	private GameObject bulletPrefab;

	// Token: 0x040027D1 RID: 10193
	[SerializeField]
	private GameObject spawnPoint;

	// Token: 0x040027D2 RID: 10194
	[SerializeField]
	private Transform explosion;

	// Token: 0x040027D3 RID: 10195
	[SerializeField]
	private GameObject petalA;

	// Token: 0x040027D4 RID: 10196
	[SerializeField]
	private GameObject petalB;

	// Token: 0x040027D5 RID: 10197
	private LevelProperties.Flower.EnemyPlants properties;

	// Token: 0x040027D6 RID: 10198
	private DamageDealer damageDealer;

	// Token: 0x040027D7 RID: 10199
	private DamageReceiver damageReceiver;

	// Token: 0x040027D8 RID: 10200
	private bool isDead;
}
