using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200080A RID: 2058
public class TrainLevelBlindSpecter : LevelProperties.Train.Entity
{
	// Token: 0x1400004F RID: 79
	// (add) Token: 0x06002F9B RID: 12187 RVA: 0x001C3B6C File Offset: 0x001C1F6C
	// (remove) Token: 0x06002F9C RID: 12188 RVA: 0x001C3BA4 File Offset: 0x001C1FA4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event TrainLevelBlindSpecter.OnDamageTakenHandler OnDamageTakenEvent;

	// Token: 0x14000050 RID: 80
	// (add) Token: 0x06002F9D RID: 12189 RVA: 0x001C3BDC File Offset: 0x001C1FDC
	// (remove) Token: 0x06002F9E RID: 12190 RVA: 0x001C3C14 File Offset: 0x001C2014
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnDeathEvent;

	// Token: 0x06002F9F RID: 12191 RVA: 0x001C3C4C File Offset: 0x001C204C
	protected override void Awake()
	{
		base.Awake();
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageDealer = DamageDealer.NewEnemy();
		base.animator.enabled = false;
		this.spriteRenderer.enabled = false;
	}

	// Token: 0x06002FA0 RID: 12192 RVA: 0x001C3CB1 File Offset: 0x001C20B1
	private void Start()
	{
		Level.Current.OnIntroEvent += this.OnIntro;
	}

	// Token: 0x06002FA1 RID: 12193 RVA: 0x001C3CC9 File Offset: 0x001C20C9
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002FA2 RID: 12194 RVA: 0x001C3CE1 File Offset: 0x001C20E1
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002FA3 RID: 12195 RVA: 0x001C3D0C File Offset: 0x001C210C
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.dead)
		{
			return;
		}
		if (this.OnDamageTakenEvent != null)
		{
			this.OnDamageTakenEvent(info.damage);
		}
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06002FA4 RID: 12196 RVA: 0x001C3D6A File Offset: 0x001C216A
	public override void LevelInit(LevelProperties.Train properties)
	{
		base.LevelInit(properties);
		this.health = (float)properties.CurrentState.blindSpecter.health;
	}

	// Token: 0x06002FA5 RID: 12197 RVA: 0x001C3D8A File Offset: 0x001C218A
	private void OnIntro()
	{
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x06002FA6 RID: 12198 RVA: 0x001C3D9C File Offset: 0x001C219C
	private void Die()
	{
		if (this.dead)
		{
			return;
		}
		base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		this.dead = true;
		this.damageReceiver.enabled = false;
		this.StopAllCoroutines();
		base.animator.Play("Death");
		AudioManager.Play("train_blindspector_death");
		this.emitAudioFromObject.Add("train_blindspector_death");
	}

	// Token: 0x06002FA7 RID: 12199 RVA: 0x001C3E03 File Offset: 0x001C2203
	private void OnDeathAnimComplete()
	{
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002FA8 RID: 12200 RVA: 0x001C3E26 File Offset: 0x001C2226
	private void SfxIntro()
	{
		AudioManager.Play("level_train_blindspecter_intro");
	}

	// Token: 0x06002FA9 RID: 12201 RVA: 0x001C3E34 File Offset: 0x001C2234
	private void FireEyeball()
	{
		AudioManager.Play("train_blindspector_attack");
		this.emitAudioFromObject.Add("train_blindspector_attack");
		float value = UnityEngine.Random.value;
		Vector2 time = new Vector2(this.blindSpecterProperties.timeX.RandomFloat(), this.blindSpecterProperties.timeY.GetFloatAt(value));
		this.eyePrefab.Create(this.eyeRoot.position, time, this.blindSpecterProperties.heightMax.GetFloatAt(value), this.shots % 2 > 0, this.blindSpecterProperties.eyeHealth);
		this.shots++;
	}

	// Token: 0x06002FAA RID: 12202 RVA: 0x001C3EDC File Offset: 0x001C22DC
	private IEnumerator loop_cr()
	{
		base.animator.enabled = true;
		this.spriteRenderer.enabled = true;
		base.animator.Play("Intro");
		this.blindSpecterProperties = base.properties.CurrentState.blindSpecter;
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", false, true);
		yield return CupheadTime.WaitForSeconds(this, 2f);
		for (;;)
		{
			this.shots = 0;
			base.animator.Play("Attack_Start");
			while (this.shots < this.blindSpecterProperties.attackLoops * 2)
			{
				yield return null;
			}
			base.animator.SetTrigger("Continue");
			yield return base.animator.WaitForAnimationToEnd(this, "Attack_End", false, true);
			yield return CupheadTime.WaitForSeconds(this, this.blindSpecterProperties.hesitate);
		}
		yield break;
	}

	// Token: 0x06002FAB RID: 12203 RVA: 0x001C3EF7 File Offset: 0x001C22F7
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.eyePrefab = null;
	}

	// Token: 0x04003881 RID: 14465
	[SerializeField]
	private Transform eyeRoot;

	// Token: 0x04003882 RID: 14466
	[SerializeField]
	private TrainLevelBlindSpecterEyeProjectile eyePrefab;

	// Token: 0x04003883 RID: 14467
	private SpriteRenderer spriteRenderer;

	// Token: 0x04003884 RID: 14468
	private LevelProperties.Train.BlindSpecter blindSpecterProperties;

	// Token: 0x04003885 RID: 14469
	private int shots;

	// Token: 0x04003886 RID: 14470
	private DamageDealer damageDealer;

	// Token: 0x04003887 RID: 14471
	private DamageReceiver damageReceiver;

	// Token: 0x04003888 RID: 14472
	private float health;

	// Token: 0x04003889 RID: 14473
	private bool dead;

	// Token: 0x0200080B RID: 2059
	// (Invoke) Token: 0x06002FAD RID: 12205
	public delegate void OnDamageTakenHandler(float damage);
}
