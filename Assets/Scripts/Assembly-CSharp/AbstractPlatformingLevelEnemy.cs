using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200085A RID: 2138
[RequireComponent(typeof(DamageReceiver))]
[RequireComponent(typeof(HitFlash))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class AbstractPlatformingLevelEnemy : AbstractLevelEntity
{
	// Token: 0x1700042D RID: 1069
	// (get) Token: 0x06003193 RID: 12691 RVA: 0x001CED90 File Offset: 0x001CD190
	public EnemyID ID
	{
		get
		{
			return this._id;
		}
	}

	// Token: 0x1700042E RID: 1070
	// (get) Token: 0x06003194 RID: 12692 RVA: 0x001CED98 File Offset: 0x001CD198
	public float StartDelay
	{
		get
		{
			return this._startDelay;
		}
	}

	// Token: 0x1700042F RID: 1071
	// (get) Token: 0x06003195 RID: 12693 RVA: 0x001CEDA0 File Offset: 0x001CD1A0
	public EnemyProperties Properties
	{
		get
		{
			if (this._properties == null)
			{
				this._properties = EnemyDatabase.GetProperties(this._id);
			}
			return this._properties;
		}
	}

	// Token: 0x17000430 RID: 1072
	// (get) Token: 0x06003196 RID: 12694 RVA: 0x001CEDC4 File Offset: 0x001CD1C4
	// (set) Token: 0x06003197 RID: 12695 RVA: 0x001CEDCC File Offset: 0x001CD1CC
	public float Health { get; protected set; }

	// Token: 0x17000431 RID: 1073
	// (get) Token: 0x06003198 RID: 12696 RVA: 0x001CEDD5 File Offset: 0x001CD1D5
	// (set) Token: 0x06003199 RID: 12697 RVA: 0x001CEDDD File Offset: 0x001CD1DD
	public bool Dead { get; protected set; }

	// Token: 0x17000432 RID: 1074
	// (get) Token: 0x0600319A RID: 12698 RVA: 0x001CEDE6 File Offset: 0x001CD1E6
	// (set) Token: 0x0600319B RID: 12699 RVA: 0x001CEDEE File Offset: 0x001CD1EE
	private protected DamageReceiver _damageReceiver { protected get; private set; }

	// Token: 0x17000433 RID: 1075
	// (get) Token: 0x0600319C RID: 12700 RVA: 0x001CEDF7 File Offset: 0x001CD1F7
	// (set) Token: 0x0600319D RID: 12701 RVA: 0x001CEDFF File Offset: 0x001CD1FF
	private protected DamageDealer _damageDealer { protected get; private set; }

	// Token: 0x17000434 RID: 1076
	// (get) Token: 0x0600319E RID: 12702 RVA: 0x001CEE08 File Offset: 0x001CD208
	// (set) Token: 0x0600319F RID: 12703 RVA: 0x001CEE10 File Offset: 0x001CD210
	private protected bool _started { protected get; private set; }

	// Token: 0x060031A0 RID: 12704 RVA: 0x001CEE1C File Offset: 0x001CD21C
	protected override void Awake()
	{
		base.Awake();
		if (this.Properties == null)
		{
			this.Health = 10f;
			this._canParry = false;
		}
		else
		{
			this.Health = this.Properties.Health;
			this._canParry = this.Properties.CanParry;
		}
		this._damageReceiver = base.GetComponent<DamageReceiver>();
		this._damageDealer = DamageDealer.NewEnemy();
		this._damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x060031A1 RID: 12705 RVA: 0x001CEEA2 File Offset: 0x001CD2A2
	protected virtual void Start()
	{
		this.StartWithCondition(AbstractPlatformingLevelEnemy.StartCondition.Instant);
		Level.Current.OnLevelStartEvent += this.OnLevelStart;
	}

	// Token: 0x060031A2 RID: 12706 RVA: 0x001CEEC1 File Offset: 0x001CD2C1
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (Level.Current != null)
		{
			Level.Current.OnLevelStartEvent -= this.OnLevelStart;
		}
		this.explosionPrefabs = null;
		this.parryEffectPrefab = null;
	}

	// Token: 0x060031A3 RID: 12707 RVA: 0x001CEF00 File Offset: 0x001CD300
	protected virtual void Update()
	{
		if (this._startCondition == AbstractPlatformingLevelEnemy.StartCondition.TriggerVolume && !this._started)
		{
			Rect rect = RectUtils.NewFromCenter(this._triggerPosition.x, this._triggerPosition.y, this._triggerSize.x, this._triggerSize.y);
			if (rect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerOne).center) || (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null && rect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerTwo).center)))
			{
				this.StartWithCondition(AbstractPlatformingLevelEnemy.StartCondition.TriggerVolume);
			}
		}
		if (this._damageDealer != null)
		{
			this._damageDealer.Update();
		}
	}

	// Token: 0x060031A4 RID: 12708 RVA: 0x001CEFB2 File Offset: 0x001CD3B2
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this._damageDealer != null && phase != CollisionPhase.Exit)
		{
			this._damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060031A5 RID: 12709 RVA: 0x001CEFDB File Offset: 0x001CD3DB
	protected virtual void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.Health -= info.damage;
		if (this.Health <= 0f)
		{
			Level.ScoringData.pacifistRun = false;
			this.Die();
		}
	}

	// Token: 0x060031A6 RID: 12710 RVA: 0x001CF011 File Offset: 0x001CD411
	public override void OnLevelEnd()
	{
		base.OnLevelEnd();
		this.Die();
	}

	// Token: 0x060031A7 RID: 12711 RVA: 0x001CF01F File Offset: 0x001CD41F
	protected virtual void Die()
	{
		this.IdleSounds = false;
		if (this.Dead)
		{
			return;
		}
		this.Dead = true;
		this.Explode();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060031A8 RID: 12712 RVA: 0x001CF04C File Offset: 0x001CD44C
	protected void Explode()
	{
		if (this.explosionPrefabs.Length > 0 && CupheadLevelCamera.Current.ContainsPoint(base.transform.position, AbstractPlatformingLevelEnemy.CAMERA_DEATH_PADDING))
		{
			this.explosionPrefabs.RandomChoice<PlatformingLevelGenericExplosion>().Create(base.GetComponent<Collider2D>().bounds.center);
		}
	}

	// Token: 0x060031A9 RID: 12713 RVA: 0x001CF0B0 File Offset: 0x001CD4B0
	public override void OnParry(AbstractPlayerController player)
	{
		base.OnParry(player);
		if (this.parryEffectPrefab != null)
		{
			this.parryEffectPrefab.Create(base.GetComponent<Collider2D>().bounds.center);
		}
		player.stats.OnParry(1f, true);
		this.Die();
	}

	// Token: 0x060031AA RID: 12714 RVA: 0x001CF10C File Offset: 0x001CD50C
	protected virtual IEnumerator idle_audio_delayer_cr(string key, float delayMin, float delayMax)
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		for (;;)
		{
			if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, AbstractPlatformingLevelEnemy.CAMERA_DEATH_PADDING))
			{
				float delay = UnityEngine.Random.Range(delayMin, delayMax);
				yield return CupheadTime.WaitForSeconds(this, delay);
				yield return null;
				if (this.IdleSounds)
				{
					AudioManager.Play(key);
					while (AudioManager.CheckIfPlaying(key))
					{
						yield return null;
					}
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060031AB RID: 12715 RVA: 0x001CF13C File Offset: 0x001CD53C
	public void StartFromCustom()
	{
		if (!this._started)
		{
			this.StartWithCondition(AbstractPlatformingLevelEnemy.StartCondition.Custom);
		}
	}

	// Token: 0x060031AC RID: 12716 RVA: 0x001CF150 File Offset: 0x001CD550
	public void ResetStartingCondition()
	{
		this._started = false;
	}

	// Token: 0x060031AD RID: 12717
	protected abstract void OnStart();

	// Token: 0x060031AE RID: 12718 RVA: 0x001CF159 File Offset: 0x001CD559
	private void OnLevelStart()
	{
		this.StartWithCondition(AbstractPlatformingLevelEnemy.StartCondition.LevelStart);
	}

	// Token: 0x060031AF RID: 12719 RVA: 0x001CF162 File Offset: 0x001CD562
	private void StartWithCondition(AbstractPlatformingLevelEnemy.StartCondition condition)
	{
		if (this.Dead || condition != this._startCondition || this._started)
		{
			return;
		}
		this._started = true;
		base.StartCoroutine(this.startWithCondition_cr());
	}

	// Token: 0x060031B0 RID: 12720 RVA: 0x001CF19C File Offset: 0x001CD59C
	private IEnumerator startWithCondition_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this._startDelay);
		this.OnStart();
		yield break;
	}

	// Token: 0x060031B1 RID: 12721 RVA: 0x001CF1B7 File Offset: 0x001CD5B7
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x060031B2 RID: 12722 RVA: 0x001CF1CA File Offset: 0x001CD5CA
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x060031B3 RID: 12723 RVA: 0x001CF1E0 File Offset: 0x001CD5E0
	private void DrawGizmos(float a)
	{
		if (this._startCondition == AbstractPlatformingLevelEnemy.StartCondition.TriggerVolume)
		{
			Gizmos.color = new Color(0f, 1f, 0f, a);
			Gizmos.DrawWireCube(this._triggerPosition, this._triggerSize);
		}
	}

	// Token: 0x04003A05 RID: 14853
	public static readonly Vector2 CAMERA_DEATH_PADDING = new Vector2(100f, 100f);

	// Token: 0x04003A06 RID: 14854
	[SerializeField]
	private EnemyID _id;

	// Token: 0x04003A07 RID: 14855
	[SerializeField]
	protected AbstractPlatformingLevelEnemy.StartCondition _startCondition;

	// Token: 0x04003A08 RID: 14856
	[SerializeField]
	private float _startDelay;

	// Token: 0x04003A09 RID: 14857
	[SerializeField]
	protected Vector2 _triggerPosition = Vector2.zero;

	// Token: 0x04003A0A RID: 14858
	[SerializeField]
	protected Vector2 _triggerSize = Vector2.one * 100f;

	// Token: 0x04003A0B RID: 14859
	[SerializeField]
	private PlatformingLevelGenericExplosion[] explosionPrefabs;

	// Token: 0x04003A0C RID: 14860
	[SerializeField]
	private Effect parryEffectPrefab;

	// Token: 0x04003A0D RID: 14861
	private EnemyProperties _properties;

	// Token: 0x04003A13 RID: 14867
	protected bool IdleSounds = true;

	// Token: 0x0200085B RID: 2139
	public enum StartCondition
	{
		// Token: 0x04003A15 RID: 14869
		LevelStart,
		// Token: 0x04003A16 RID: 14870
		TriggerVolume,
		// Token: 0x04003A17 RID: 14871
		Instant,
		// Token: 0x04003A18 RID: 14872
		Custom
	}
}
