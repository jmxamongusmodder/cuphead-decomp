using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000786 RID: 1926
public class RumRunnersLevelBarrel : LevelProperties.RumRunners.Entity
{
	// Token: 0x06002A6C RID: 10860 RVA: 0x0018C944 File Offset: 0x0018AD44
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.coll = base.GetComponent<Collider2D>();
	}

	// Token: 0x06002A6D RID: 10861 RVA: 0x0018C991 File Offset: 0x0018AD91
	public override void LevelInit(LevelProperties.RumRunners properties)
	{
		base.LevelInit(properties);
		((RumRunnersLevel)Level.Current).OnUpperBridgeDestroy += this.onUpperBridgeDestroy;
	}

	// Token: 0x06002A6E RID: 10862 RVA: 0x0018C9B8 File Offset: 0x0018ADB8
	public void Initialize(float dir, Vector3 spawnPos, RumRunnersLevelWorm parent, bool parryable, bool isCop)
	{
		this.isCop = isCop;
		this.facingDirection = dir;
		base.transform.position = spawnPos;
		base.transform.localScale = new Vector3(dir, 1f);
		this.parent = parent;
		this.runSpeed = base.properties.CurrentState.barrels.barrelSpeed;
		this.HP = (float)base.properties.CurrentState.barrels.barrelHP;
		this._canParry = parryable;
		if (isCop)
		{
			base.animator.Play("Cop");
		}
		else if (Rand.Bool())
		{
			base.animator.Play((!base.canParry) ? "DanceA" : "DanceAParry");
		}
		else
		{
			base.animator.Play((!base.canParry) ? "DanceB" : "DanceBParry");
		}
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002A6F RID: 10863 RVA: 0x0018CABE File Offset: 0x0018AEBE
	public override void OnParry(AbstractPlayerController player)
	{
		player.stats.OnParry(1f, true);
		this.Die(false, false);
		this._canParry = false;
	}

	// Token: 0x06002A70 RID: 10864 RVA: 0x0018CAE0 File Offset: 0x0018AEE0
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002A71 RID: 10865 RVA: 0x0018CAF8 File Offset: 0x0018AEF8
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.HP -= info.damage;
		if (this.HP <= 0f)
		{
			Level.Current.RegisterMinionKilled();
			this.Die(false, true);
		}
	}

	// Token: 0x06002A72 RID: 10866 RVA: 0x0018CB2F File Offset: 0x0018AF2F
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06002A73 RID: 10867 RVA: 0x0018CB48 File Offset: 0x0018AF48
	private IEnumerator move_cr()
	{
		while (base.transform.position.x * this.facingDirection < 960f)
		{
			base.transform.position += Vector3.right * this.facingDirection * this.runSpeed * CupheadTime.FixedDelta;
			base.transform.SetPosition(null, new float?(RumRunnersLevel.GroundWalkingPosY(base.transform.position, this.coll, this.verticalOffset, 200f)), null);
			if (Level.Current.mode == Level.Mode.Easy && this.parent.isDead)
			{
				this.Die(false, true);
				this._canParry = false;
			}
			yield return new WaitForFixedUpdate();
		}
		this.Die(true, true);
		yield break;
	}

	// Token: 0x06002A74 RID: 10868 RVA: 0x0018CB64 File Offset: 0x0018AF64
	public void Die(bool immediate, bool spawnShrapnel = true)
	{
		((RumRunnersLevel)Level.Current).OnUpperBridgeDestroy -= this.onUpperBridgeDestroy;
		this.StopAllCoroutines();
		if (immediate)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		if (this.isCop)
		{
			base.StartCoroutine(this.copDeath_cr());
		}
		else
		{
			if (base.transform.position.x * this.facingDirection < 960f)
			{
				Effect effect = this.deathPoof.Create(base.transform.position);
				if (!spawnShrapnel)
				{
					effect.GetComponent<Animator>().Play("Poof", 0, 0.083333336f);
				}
				this.SFX_RUMRUN_BarrelExplode();
				if (spawnShrapnel)
				{
					float num = UnityEngine.Random.Range(0f, 6.2831855f);
					for (int i = 0; i < 2; i++)
					{
						for (int j = 0; j < 4; j++)
						{
							float f = num + 6.2831855f * (float)j / 4f;
							Vector3 b = new Vector3(Mathf.Cos(f) * 50f, Mathf.Sin(f) * 50f);
							Effect effect2 = this.deathShrapnel.Create(base.transform.position + b);
							effect2.animator.SetInteger("Effect", j);
							effect2.animator.SetBool("Parry", this._canParry);
							if (i > 0)
							{
								SpriteRenderer component = effect2.GetComponent<SpriteRenderer>();
								component.sortingLayerName = "Background";
								component.sortingOrder = 95;
								component.color = new Color(0.7f, 0.7f, 0.7f, 1f);
								effect2.transform.SetScale(new float?(0.75f), new float?(0.75f), null);
							}
							SpriteDeathParts component2 = effect2.GetComponent<SpriteDeathParts>();
							if (b.x > 0f)
							{
								component2.SetVelocityX(0f, component2.VelocityXMax);
							}
							else
							{
								component2.SetVelocityX(component2.VelocityXMin, 0f);
							}
						}
					}
				}
			}
			if (!spawnShrapnel)
			{
				base.GetComponent<Collider2D>().enabled = false;
				this.runSpeed = 0f;
				base.StartCoroutine(this.destroy_with_delay_cr());
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x06002A75 RID: 10869 RVA: 0x0018CDC8 File Offset: 0x0018B1C8
	private IEnumerator destroy_with_delay_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.041666668f);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06002A76 RID: 10870 RVA: 0x0018CDE4 File Offset: 0x0018B1E4
	private IEnumerator copDeath_cr()
	{
		this.SFX_RUMRUN_Police_DiePoof();
		base.GetComponent<BoxCollider2D>().enabled = false;
		base.animator.SetTrigger("CopDeath");
		yield return base.animator.WaitForNormalizedTime(this, 1f, "CopDeath", 0, false, false, true);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06002A77 RID: 10871 RVA: 0x0018CE00 File Offset: 0x0018B200
	private void onUpperBridgeDestroy(Rangef effectRange)
	{
		if (base.transform.position.y < 0f)
		{
			return;
		}
		if (effectRange.ContainsInclusive(base.transform.position.x))
		{
			this.Die(false, true);
			this._canParry = false;
		}
	}

	// Token: 0x06002A78 RID: 10872 RVA: 0x0018CE59 File Offset: 0x0018B259
	private void SFX_RUMRUN_BarrelExplode()
	{
		AudioManager.Play("sfx_dlc_rumrun_barrel_explode");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_barrel_explode");
	}

	// Token: 0x06002A79 RID: 10873 RVA: 0x0018CE75 File Offset: 0x0018B275
	private void SFX_RUMRUN_Police_DiePoof()
	{
		AudioManager.Play("sfx_dlc_rumrun_lackey_poof");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_lackey_poof");
		AudioManager.Stop("sfx_dlc_rumrun_policegun_shoot");
	}

	// Token: 0x04003338 RID: 13112
	[SerializeField]
	private Effect deathPoof;

	// Token: 0x04003339 RID: 13113
	[SerializeField]
	private Effect deathShrapnel;

	// Token: 0x0400333A RID: 13114
	[SerializeField]
	private float verticalOffset;

	// Token: 0x0400333B RID: 13115
	private DamageDealer damageDealer;

	// Token: 0x0400333C RID: 13116
	private DamageReceiver damageReceiver;

	// Token: 0x0400333D RID: 13117
	private float runSpeed;

	// Token: 0x0400333E RID: 13118
	private float HP;

	// Token: 0x0400333F RID: 13119
	private float facingDirection;

	// Token: 0x04003340 RID: 13120
	private Collider2D coll;

	// Token: 0x04003341 RID: 13121
	private RumRunnersLevelWorm parent;

	// Token: 0x04003342 RID: 13122
	private bool isCop;
}
