using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004E7 RID: 1255
public class BaronessLevelCandyCorn : BaronessLevelMiniBossBase
{
	// Token: 0x1700031E RID: 798
	// (get) Token: 0x060015C0 RID: 5568 RVA: 0x000C37B4 File Offset: 0x000C1BB4
	// (set) Token: 0x060015C1 RID: 5569 RVA: 0x000C37BC File Offset: 0x000C1BBC
	public BaronessLevelCandyCorn.State state { get; private set; }

	// Token: 0x060015C2 RID: 5570 RVA: 0x000C37C8 File Offset: 0x000C1BC8
	protected override void Awake()
	{
		base.Awake();
		this.firstTime = true;
		this.isDying = false;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.state = BaronessLevelCandyCorn.State.Move;
	}

	// Token: 0x060015C3 RID: 5571 RVA: 0x000C381F File Offset: 0x000C1C1F
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060015C4 RID: 5572 RVA: 0x000C3840 File Offset: 0x000C1C40
	public void Init(LevelProperties.Baroness.CandyCorn properties, Vector2 pos, float speed, float health)
	{
		this.properties = properties;
		this.speed = speed;
		this.health = health;
		base.transform.position = pos;
		this.bottomPoint = pos.y;
		this.isTop = false;
		this.movingLeft = true;
		if (this.properties.spawnMinis)
		{
			base.StartCoroutine(this.spawnMinis_cr());
		}
		base.StartCoroutine(this.switchLayer_cr());
	}

	// Token: 0x060015C5 RID: 5573 RVA: 0x000C38B9 File Offset: 0x000C1CB9
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060015C6 RID: 5574 RVA: 0x000C38D1 File Offset: 0x000C1CD1
	private void FixedUpdate()
	{
		if (this.state == BaronessLevelCandyCorn.State.Move)
		{
			if (this.moveY)
			{
				this.MoveAlongY();
			}
			else
			{
				this.MoveAlongX();
			}
		}
	}

	// Token: 0x060015C7 RID: 5575 RVA: 0x000C38FC File Offset: 0x000C1CFC
	protected override void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.health > 0f)
		{
			base.OnDamageTaken(info);
		}
		this.health -= info.damage;
		if (this.health <= 0f && this.state != BaronessLevelCandyCorn.State.Dying)
		{
			DamageDealer.DamageInfo info2 = new DamageDealer.DamageInfo(this.health, info.direction, info.origin, info.damageSource);
			base.OnDamageTaken(info2);
			this.state = BaronessLevelCandyCorn.State.Dying;
			this.StartDeath();
		}
	}

	// Token: 0x060015C8 RID: 5576 RVA: 0x000C3984 File Offset: 0x000C1D84
	private IEnumerator switchLayer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 3f);
		base.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Enemies.ToString();
		base.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
		yield break;
	}

	// Token: 0x060015C9 RID: 5577 RVA: 0x000C399F File Offset: 0x000C1D9F
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.miniCandyPrefab = null;
	}

	// Token: 0x060015CA RID: 5578 RVA: 0x000C39B0 File Offset: 0x000C1DB0
	private IEnumerator spawnMinis_cr()
	{
		this.targetPos = base.transform;
		Transform targetPos2 = this.targetPos;
		SpriteRenderer thisRenderer = base.gameObject.GetComponent<SpriteRenderer>();
		for (;;)
		{
			if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Turn_A") || base.animator.GetCurrentAnimatorStateInfo(0).IsName("Turn_B"))
			{
				BaronessLevelCandyCornMini miniCandyCorn = UnityEngine.Object.Instantiate<BaronessLevelCandyCornMini>(this.miniCandyPrefab);
				miniCandyCorn.Init(base.transform.position, this.properties.miniCornMovementSpeed, (float)this.properties.miniCornHP);
				targetPos2 = miniCandyCorn.transform;
				SpriteRenderer r = miniCandyCorn.GetComponent<SpriteRenderer>();
				r.sortingLayerName = thisRenderer.sortingLayerName;
				r.sortingOrder = thisRenderer.sortingOrder - 1;
				yield return CupheadTime.WaitForSeconds(this, this.properties.miniCornSpawnDelay);
			}
			else
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x060015CB RID: 5579 RVA: 0x000C39CC File Offset: 0x000C1DCC
	private void MoveAlongX()
	{
		float num = 50f;
		float num2 = 10f;
		Vector3 a = new Vector3(this.properties.centerPosition, 0f, 0f);
		Vector3 vector = a - base.transform.position;
		if (this.movingLeft)
		{
			if (base.transform.position.x > -640f + num)
			{
				base.transform.position -= base.transform.right * (this.speed * CupheadTime.FixedDelta * this.hitPauseCoefficient());
				if (vector.x < num2 && vector.x > -num2 && !this.justSwitchedMiddle)
				{
					this.checkIfSwitch();
				}
			}
			else
			{
				this.moveY = true;
				this.justSwitchedMiddle = false;
				this.movingLeft = false;
			}
		}
		else if (!this.movingLeft)
		{
			if (base.transform.position.x < (float)Level.Current.Right - num)
			{
				base.transform.position += base.transform.right * (this.speed * CupheadTime.FixedDelta * this.hitPauseCoefficient());
				if (vector.x < num2 && vector.x > -num2 && !this.justSwitchedMiddle)
				{
					this.checkIfSwitch();
				}
			}
			else
			{
				this.moveY = true;
				this.justSwitchedMiddle = false;
				this.movingLeft = true;
			}
		}
	}

	// Token: 0x060015CC RID: 5580 RVA: 0x000C3B74 File Offset: 0x000C1F74
	private void MoveAlongY()
	{
		float num = 125f;
		if (!this.isTop)
		{
			if (base.transform.position.y < 360f - num)
			{
				base.transform.position += base.transform.up * (this.speed * CupheadTime.FixedDelta * this.hitPauseCoefficient());
			}
			else
			{
				this.isTop = true;
				this.moveY = false;
			}
		}
		else if (base.transform.position.y > this.bottomPoint)
		{
			base.transform.position -= base.transform.up * (this.speed * CupheadTime.FixedDelta * this.hitPauseCoefficient());
		}
		else
		{
			this.isTop = false;
			this.moveY = false;
		}
	}

	// Token: 0x060015CD RID: 5581 RVA: 0x000C3C6C File Offset: 0x000C206C
	private void checkIfSwitch()
	{
		base.StartCoroutine(this.switch_cr());
	}

	// Token: 0x060015CE RID: 5582 RVA: 0x000C3C7C File Offset: 0x000C207C
	private IEnumerator switch_cr()
	{
		string[] pattern = this.properties.changeLevelString.GetRandom<string>().Split(new char[]
		{
			','
		});
		if (this.firstTime)
		{
			this.firstIndex = UnityEngine.Random.Range(0, pattern.Length);
			this.firstTime = false;
		}
		if (pattern[this.firstIndex][0] == 'Y')
		{
			this.moveY = true;
			this.justSwitchedMiddle = true;
		}
		else if (pattern[this.firstIndex][0] == 'N')
		{
			this.moveY = false;
			this.justSwitchedMiddle = true;
		}
		if (this.firstIndex < pattern.Length - 1)
		{
			this.firstIndex++;
		}
		else
		{
			this.firstIndex = 0;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060015CF RID: 5583 RVA: 0x000C3C97 File Offset: 0x000C2097
	private void StartDeath()
	{
		this.state = BaronessLevelCandyCorn.State.Dying;
		this.StopAllCoroutines();
		base.StartCoroutine(this.death_cr());
	}

	// Token: 0x060015D0 RID: 5584 RVA: 0x000C3CB4 File Offset: 0x000C20B4
	private IEnumerator death_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float speed = this.properties.deathMoveSpeed;
		this.StartExplosions();
		this.isDying = true;
		base.animator.SetTrigger("Death");
		base.GetComponent<Collider2D>().enabled = false;
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		while (base.transform.position.y < 560f)
		{
			base.transform.position += Vector3.up * speed * CupheadTime.FixedDelta;
			speed += this.properties.deathAcceleration;
			yield return wait;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x060015D1 RID: 5585 RVA: 0x000C3CCF File Offset: 0x000C20CF
	private void SoundCandyCornBite()
	{
		AudioManager.Play("level_baroness_candycorn_bite");
		this.emitAudioFromObject.Add("level_baroness_candycorn_bite");
	}

	// Token: 0x04001F16 RID: 7958
	[SerializeField]
	private BaronessLevelCandyCornMini miniCandyPrefab;

	// Token: 0x04001F17 RID: 7959
	private LevelProperties.Baroness.CandyCorn properties;

	// Token: 0x04001F18 RID: 7960
	private Transform targetPos;

	// Token: 0x04001F19 RID: 7961
	private DamageDealer damageDealer;

	// Token: 0x04001F1A RID: 7962
	private DamageReceiver damageReceiver;

	// Token: 0x04001F1B RID: 7963
	private float health;

	// Token: 0x04001F1C RID: 7964
	private float speed;

	// Token: 0x04001F1D RID: 7965
	private float bottomPoint;

	// Token: 0x04001F1E RID: 7966
	private int firstIndex;

	// Token: 0x04001F1F RID: 7967
	private bool isTop;

	// Token: 0x04001F20 RID: 7968
	private bool moveY;

	// Token: 0x04001F21 RID: 7969
	private bool firstTime;

	// Token: 0x04001F22 RID: 7970
	private bool justSwitchedMiddle;

	// Token: 0x04001F23 RID: 7971
	private bool movingLeft;

	// Token: 0x020004E8 RID: 1256
	public enum State
	{
		// Token: 0x04001F25 RID: 7973
		Move,
		// Token: 0x04001F26 RID: 7974
		Dying
	}
}
