using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005A2 RID: 1442
public class DicePalaceBoozeLevelOlive : AbstractCollidableObject
{
	// Token: 0x06001BB6 RID: 7094 RVA: 0x000FC954 File Offset: 0x000FAD54
	protected override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.moving = false;
		this.shotCount = 0;
		this.moveCount = 0;
		this.nextPlayerTarget = PlayerId.PlayerOne;
		base.Awake();
	}

	// Token: 0x06001BB7 RID: 7095 RVA: 0x000FC9B1 File Offset: 0x000FADB1
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001BB8 RID: 7096 RVA: 0x000FC9C9 File Offset: 0x000FADC9
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001BB9 RID: 7097 RVA: 0x000FC9E7 File Offset: 0x000FADE7
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f && !this.isDead)
		{
			this.isDead = true;
			this.OnDeath();
		}
	}

	// Token: 0x06001BBA RID: 7098 RVA: 0x000FCA24 File Offset: 0x000FAE24
	public void InitOlive(LevelProperties.DicePalaceBooze properties, int maxShotCount, string yCoordinates, string xCoordinates)
	{
		this.properties = properties;
		this.shotCountMax = maxShotCount;
		this.health = (float)properties.CurrentState.martini.oliveHP;
		this.yCoordinates = yCoordinates;
		this.xCoordinates = xCoordinates;
		this.moveCountMaxIndex = UnityEngine.Random.Range(0, properties.CurrentState.martini.moveString.Split(new char[]
		{
			','
		}).Length);
		this.moveCountMax = Parser.IntParse(properties.CurrentState.martini.moveString.Split(new char[]
		{
			','
		})[this.moveCountMaxIndex]);
		this.verticalCoordinateIndex = UnityEngine.Random.Range(0, yCoordinates.Split(new char[]
		{
			','
		}).Length);
		this.horizontalCoordinateindex = UnityEngine.Random.Range(0, xCoordinates.Split(new char[]
		{
			','
		}).Length);
		this.moveToTarget.y = (float)(Level.Current.Ground + Parser.IntParse(yCoordinates.Split(new char[]
		{
			','
		})[this.verticalCoordinateIndex]));
		this.moveToTarget.x = (float)(Level.Current.Left + 50 + Parser.IntParse(xCoordinates.Split(new char[]
		{
			','
		})[this.horizontalCoordinateindex]));
		Level.Current.OnWinEvent += this.OnDeath;
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x06001BBB RID: 7099 RVA: 0x000FCB94 File Offset: 0x000FAF94
	public void ResetOlive(int maxShotCount)
	{
		base.GetComponent<Collider2D>().enabled = true;
		this.shotCountMax = maxShotCount;
		this.health = (float)this.properties.CurrentState.martini.oliveHP;
		base.StartCoroutine(this.attack_cr());
		this.isDead = false;
	}

	// Token: 0x06001BBC RID: 7100 RVA: 0x000FCBE4 File Offset: 0x000FAFE4
	private IEnumerator attack_cr()
	{
		for (;;)
		{
			if (this.moveCount < this.moveCountMax)
			{
				this.GetNextTarget();
				base.StartCoroutine(this.move_cr());
				this.moveCount++;
				while (this.moving)
				{
					yield return null;
				}
				yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.martini.oliveStopDuration);
			}
			else
			{
				AudioManager.Play("booze_olive_attack");
				this.emitAudioFromObject.Add("booze_olive_attack");
				base.animator.SetTrigger("OnAttack");
				yield return base.animator.WaitForAnimationToEnd(this, "Attack", false, true);
				yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.martini.oliveHesitateAfterShooting);
			}
		}
		yield break;
	}

	// Token: 0x06001BBD RID: 7101 RVA: 0x000FCBFF File Offset: 0x000FAFFF
	private void Shoot()
	{
		base.StartCoroutine(this.shoot_cr());
	}

	// Token: 0x06001BBE RID: 7102 RVA: 0x000FCC10 File Offset: 0x000FB010
	private IEnumerator shoot_cr()
	{
		this.moveCount = 0;
		Vector3 target = PlayerManager.GetPlayer(this.nextPlayerTarget).center - base.transform.position;
		BasicProjectile proj = this.pimentoPrefab.Create(base.transform.position, 0f, this.properties.CurrentState.martini.bulletSpeed);
		proj.animator.SetBool("Reverse", Rand.Bool());
		proj.transform.right = target;
		IEnumerator enumerator = proj.GetComponentInChildren<Transform>().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		this.shotCount++;
		if (this.shotCount > this.shotCountMax)
		{
			proj.SetParryable(true);
			this.shotCount = 0;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001BBF RID: 7103 RVA: 0x000FCC2C File Offset: 0x000FB02C
	private IEnumerator move_cr()
	{
		this.moving = true;
		while (Vector3.Distance(base.transform.position, this.moveToTarget) > 5f)
		{
			Vector3 dir = (this.moveToTarget - base.transform.position).normalized;
			base.transform.position += dir * this.properties.CurrentState.martini.oliveSpeed * CupheadTime.Delta;
			yield return null;
		}
		this.moving = false;
		yield break;
	}

	// Token: 0x06001BC0 RID: 7104 RVA: 0x000FCC48 File Offset: 0x000FB048
	private void GetNextTarget()
	{
		this.verticalCoordinateIndex++;
		if (this.verticalCoordinateIndex >= this.yCoordinates.Split(new char[]
		{
			','
		}).Length)
		{
			this.verticalCoordinateIndex = 0;
		}
		this.horizontalCoordinateindex++;
		if (this.horizontalCoordinateindex >= this.xCoordinates.Split(new char[]
		{
			','
		}).Length)
		{
			this.horizontalCoordinateindex = 0;
		}
		this.moveToTarget.y = (float)(Level.Current.Ground + Parser.IntParse(this.yCoordinates.Split(new char[]
		{
			','
		})[this.verticalCoordinateIndex]));
		this.moveToTarget.x = (float)(Level.Current.Left + 50 + Parser.IntParse(this.xCoordinates.Split(new char[]
		{
			','
		})[this.horizontalCoordinateindex]));
	}

	// Token: 0x06001BC1 RID: 7105 RVA: 0x000FCD3A File Offset: 0x000FB13A
	protected override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
		this.pimentoPrefab = null;
	}

	// Token: 0x06001BC2 RID: 7106 RVA: 0x000FCD50 File Offset: 0x000FB150
	private void OnDeath()
	{
		AudioManager.Play("booze_olive_death");
		this.emitAudioFromObject.Add("booze_olive_death");
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		if (base.gameObject.activeInHierarchy)
		{
			base.animator.SetTrigger("OnDeath");
		}
	}

	// Token: 0x06001BC3 RID: 7107 RVA: 0x000FCDA9 File Offset: 0x000FB1A9
	private void Deactivate()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x040024C2 RID: 9410
	[SerializeField]
	private BasicProjectile pimentoPrefab;

	// Token: 0x040024C3 RID: 9411
	private int verticalCoordinateIndex;

	// Token: 0x040024C4 RID: 9412
	private int horizontalCoordinateindex;

	// Token: 0x040024C5 RID: 9413
	private float health;

	// Token: 0x040024C6 RID: 9414
	private int shotCount;

	// Token: 0x040024C7 RID: 9415
	private int shotCountMax;

	// Token: 0x040024C8 RID: 9416
	private int moveCount;

	// Token: 0x040024C9 RID: 9417
	private int moveCountMaxIndex;

	// Token: 0x040024CA RID: 9418
	private int moveCountMax;

	// Token: 0x040024CB RID: 9419
	private bool isDead;

	// Token: 0x040024CC RID: 9420
	private bool moving;

	// Token: 0x040024CD RID: 9421
	private string yCoordinates;

	// Token: 0x040024CE RID: 9422
	private string xCoordinates;

	// Token: 0x040024CF RID: 9423
	private PlayerId nextPlayerTarget;

	// Token: 0x040024D0 RID: 9424
	private Vector3 moveToTarget;

	// Token: 0x040024D1 RID: 9425
	private LevelProperties.DicePalaceBooze properties;

	// Token: 0x040024D2 RID: 9426
	private DamageReceiver damageReceiver;

	// Token: 0x040024D3 RID: 9427
	private DamageDealer damageDealer;
}
