using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000AE3 RID: 2787
public abstract class AbstractProjectile : AbstractCollidableObject
{
	// Token: 0x170005F4 RID: 1524
	// (get) Token: 0x0600433F RID: 17215 RVA: 0x000ACF20 File Offset: 0x000AB320
	public bool CanParry
	{
		get
		{
			return this._canParry;
		}
	}

	// Token: 0x170005F5 RID: 1525
	// (get) Token: 0x06004340 RID: 17216 RVA: 0x000ACF28 File Offset: 0x000AB328
	public bool CountParryTowardsScore
	{
		get
		{
			return this._countParryTowardsScore;
		}
	}

	// Token: 0x170005F6 RID: 1526
	// (get) Token: 0x06004341 RID: 17217 RVA: 0x000ACF30 File Offset: 0x000AB330
	// (set) Token: 0x06004342 RID: 17218 RVA: 0x000ACF38 File Offset: 0x000AB338
	private protected float distance { protected get; private set; }

	// Token: 0x170005F7 RID: 1527
	// (get) Token: 0x06004343 RID: 17219 RVA: 0x000ACF41 File Offset: 0x000AB341
	// (set) Token: 0x06004344 RID: 17220 RVA: 0x000ACF49 File Offset: 0x000AB349
	private protected float lifetime { protected get; private set; }

	// Token: 0x170005F8 RID: 1528
	// (get) Token: 0x06004345 RID: 17221 RVA: 0x000ACF52 File Offset: 0x000AB352
	// (set) Token: 0x06004346 RID: 17222 RVA: 0x000ACF5A File Offset: 0x000AB35A
	public bool dead { get; private set; }

	// Token: 0x170005F9 RID: 1529
	// (get) Token: 0x06004347 RID: 17223 RVA: 0x000ACF63 File Offset: 0x000AB363
	// (set) Token: 0x06004348 RID: 17224 RVA: 0x000ACF6B File Offset: 0x000AB36B
	public float StoneTime { get; private set; }

	// Token: 0x170005FA RID: 1530
	// (get) Token: 0x06004349 RID: 17225 RVA: 0x000ACF74 File Offset: 0x000AB374
	public virtual float ParryMeterMultiplier
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170005FB RID: 1531
	// (get) Token: 0x0600434A RID: 17226 RVA: 0x000ACF7B File Offset: 0x000AB37B
	// (set) Token: 0x0600434B RID: 17227 RVA: 0x000ACF83 File Offset: 0x000AB383
	public DamageDealer.DamageSource DamageSource
	{
		get
		{
			return this.damageSource;
		}
		set
		{
			this.damageSource = value;
		}
	}

	// Token: 0x170005FC RID: 1532
	// (get) Token: 0x0600434C RID: 17228 RVA: 0x000ACF8C File Offset: 0x000AB38C
	public float DamageMultiplier
	{
		get
		{
			float num = PlayerManager.DamageMultiplier;
			if (base.tag == "PlayerProjectile")
			{
				if (PlayerManager.GetPlayer(this.PlayerId).stats.Loadout.charm == Charm.charm_health_up_1)
				{
					num *= 1f - WeaponProperties.CharmHealthUpOne.weaponDebuff;
				}
				else if (PlayerManager.GetPlayer(this.PlayerId).stats.Loadout.charm == Charm.charm_health_up_2)
				{
					num *= 1f - WeaponProperties.CharmHealthUpTwo.weaponDebuff;
				}
				else if (PlayerManager.GetPlayer(this.PlayerId).stats.Loadout.charm == Charm.charm_EX && Level.Current.playerMode == PlayerMode.Plane && this is PlaneWeaponPeashotExProjectile)
				{
					num *= 1f - WeaponProperties.CharmEXCharm.planePeashotEXDebuff;
				}
			}
			return num;
		}
	}

	// Token: 0x170005FD RID: 1533
	// (get) Token: 0x0600434D RID: 17229 RVA: 0x000AD070 File Offset: 0x000AB470
	protected virtual float DestroyLifetime
	{
		get
		{
			return 20f;
		}
	}

	// Token: 0x170005FE RID: 1534
	// (get) Token: 0x0600434E RID: 17230 RVA: 0x000AD077 File Offset: 0x000AB477
	protected virtual bool DestroyedAfterLeavingScreen
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170005FF RID: 1535
	// (get) Token: 0x0600434F RID: 17231 RVA: 0x000AD07A File Offset: 0x000AB47A
	protected virtual float SafeTime
	{
		get
		{
			return 0.005f;
		}
	}

	// Token: 0x17000600 RID: 1536
	// (get) Token: 0x06004350 RID: 17232 RVA: 0x000AD081 File Offset: 0x000AB481
	protected virtual float PlayerSafeTime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000601 RID: 1537
	// (get) Token: 0x06004351 RID: 17233 RVA: 0x000AD088 File Offset: 0x000AB488
	protected virtual float EnemySafeTime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x140000BC RID: 188
	// (add) Token: 0x06004352 RID: 17234 RVA: 0x000AD090 File Offset: 0x000AB490
	// (remove) Token: 0x06004353 RID: 17235 RVA: 0x000AD0C8 File Offset: 0x000AB4C8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event DamageDealer.OnDealDamageHandler OnDealDamageEvent;

	// Token: 0x140000BD RID: 189
	// (add) Token: 0x06004354 RID: 17236 RVA: 0x000AD100 File Offset: 0x000AB500
	// (remove) Token: 0x06004355 RID: 17237 RVA: 0x000AD138 File Offset: 0x000AB538
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<AbstractProjectile> OnDie;

	// Token: 0x06004356 RID: 17238 RVA: 0x000AD170 File Offset: 0x000AB570
	protected override void Awake()
	{
		base.Awake();
		this.distance = 0f;
		this.lifetime = 0f;
		this.StoneTime = -1f;
		if (base.CompareTag("PlayerProjectile") || !base.CompareTag("EnemyProjectile"))
		{
		}
		if (base.gameObject.layer != 12)
		{
			base.gameObject.layer = 12;
		}
		this.RandomizeVariant();
		if (Level.Current != null && Level.Current.CurrentScene == Scenes.scene_level_airplane)
		{
			this._setYPadding = -600f;
		}
	}

	// Token: 0x06004357 RID: 17239 RVA: 0x000AD218 File Offset: 0x000AB618
	protected virtual void Start()
	{
		this.damageDealer = new DamageDealer(this);
		this.damageDealer.OnDealDamage += this.OnDealDamage;
		this.damageDealer.SetStoneTime(this.StoneTime);
		this.damageDealer.PlayerId = this.PlayerId;
		if (this.tracker != null)
		{
			this.tracker.Add(this.damageDealer);
		}
	}

	// Token: 0x06004358 RID: 17240 RVA: 0x000AD288 File Offset: 0x000AB688
	protected virtual void Update()
	{
		Vector3 position = base.transform.position;
		if (this.lifetime == 0f)
		{
			this.lastPosition = (this.startPosition = position);
		}
		if (this.DestroyDistance > 0f && Vector3.Distance(this.startPosition, position) >= this.DestroyDistance)
		{
			this.OnDieDistance();
		}
		this.distance += Vector3.Distance(this.lastPosition, position);
		this.lastPosition = position;
		if (this.DestroyLifetime > 0f && this.lifetime >= this.DestroyLifetime)
		{
			this.OnDieLifetime();
		}
		this.lifetime += Time.deltaTime;
		if (this.DestroyedAfterLeavingScreen)
		{
			bool flag = CupheadLevelCamera.Current.ContainsPoint(position, new Vector2(150f, this._setYPadding));
			if (this.hasBeenRendered && !flag)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			if (!this.hasBeenRendered)
			{
				this.hasBeenRendered = flag;
			}
		}
	}

	// Token: 0x06004359 RID: 17241 RVA: 0x000AD3A1 File Offset: 0x000AB7A1
	protected void ResetLifetime()
	{
		this.lifetime = 0f;
	}

	// Token: 0x0600435A RID: 17242 RVA: 0x000AD3AE File Offset: 0x000AB7AE
	protected void ResetDistance()
	{
		this.distance = 0f;
	}

	// Token: 0x0600435B RID: 17243 RVA: 0x000AD3BB File Offset: 0x000AB7BB
	protected override void checkCollision(Collider2D col, CollisionPhase phase)
	{
		if (this.lifetime < this.SafeTime)
		{
			return;
		}
		base.checkCollision(col, phase);
	}

	// Token: 0x17000602 RID: 1538
	// (get) Token: 0x0600435C RID: 17244 RVA: 0x000AD3D7 File Offset: 0x000AB7D7
	protected override bool allowCollisionPlayer
	{
		get
		{
			return this.lifetime > this.PlayerSafeTime;
		}
	}

	// Token: 0x17000603 RID: 1539
	// (get) Token: 0x0600435D RID: 17245 RVA: 0x000AD3E7 File Offset: 0x000AB7E7
	protected override bool allowCollisionEnemy
	{
		get
		{
			return this.lifetime > this.EnemySafeTime;
		}
	}

	// Token: 0x0600435E RID: 17246 RVA: 0x000AD3F8 File Offset: 0x000AB7F8
	public virtual AbstractProjectile Create()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
		return gameObject.GetComponent<AbstractProjectile>();
	}

	// Token: 0x0600435F RID: 17247 RVA: 0x000AD41C File Offset: 0x000AB81C
	public virtual AbstractProjectile Create(Vector2 position)
	{
		AbstractProjectile abstractProjectile = this.Create();
		abstractProjectile.transform.position = position;
		return abstractProjectile;
	}

	// Token: 0x06004360 RID: 17248 RVA: 0x000AD444 File Offset: 0x000AB844
	public virtual AbstractProjectile Create(Vector2 position, float rotation)
	{
		AbstractProjectile abstractProjectile = this.Create(position);
		abstractProjectile.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(rotation));
		return abstractProjectile;
	}

	// Token: 0x06004361 RID: 17249 RVA: 0x000AD480 File Offset: 0x000AB880
	public virtual AbstractProjectile Create(Vector2 position, float rotation, Vector2 scale)
	{
		AbstractProjectile abstractProjectile = this.Create(position, rotation);
		abstractProjectile.transform.SetScale(new float?(scale.x), new float?(scale.y), new float?(1f));
		return abstractProjectile;
	}

	// Token: 0x06004362 RID: 17250 RVA: 0x000AD4C4 File Offset: 0x000AB8C4
	protected virtual void OnDealDamage(float damage, DamageReceiver receiver, DamageDealer damageDealer)
	{
		if (this.OnDealDamageEvent != null)
		{
			this.OnDealDamageEvent(damage, receiver, damageDealer);
		}
	}

	// Token: 0x06004363 RID: 17251 RVA: 0x000AD4DF File Offset: 0x000AB8DF
	public bool GetDamagesType(DamageReceiver.Type type)
	{
		return this.DamagesType.GetType(type);
	}

	// Token: 0x06004364 RID: 17252 RVA: 0x000AD4ED File Offset: 0x000AB8ED
	public virtual void SetParryable(bool parryable)
	{
		this._canParry = parryable;
		this.SetBool(AbstractProjectile.Parry, parryable);
	}

	// Token: 0x06004365 RID: 17253 RVA: 0x000AD502 File Offset: 0x000AB902
	public void SetStoneTime(float stoneTime)
	{
		this.StoneTime = stoneTime;
		if (this.damageDealer != null)
		{
			this.damageDealer.SetStoneTime(stoneTime);
		}
	}

	// Token: 0x06004366 RID: 17254 RVA: 0x000AD522 File Offset: 0x000AB922
	protected override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		if (this.CollisionDeath.Walls)
		{
			this.OnCollisionDie(hit, phase);
		}
	}

	// Token: 0x06004367 RID: 17255 RVA: 0x000AD53C File Offset: 0x000AB93C
	protected override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		if (this.CollisionDeath.Ceiling)
		{
			this.OnCollisionDie(hit, phase);
		}
	}

	// Token: 0x06004368 RID: 17256 RVA: 0x000AD556 File Offset: 0x000AB956
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		if (this.CollisionDeath.Ground)
		{
			this.OnCollisionDie(hit, phase);
		}
	}

	// Token: 0x06004369 RID: 17257 RVA: 0x000AD570 File Offset: 0x000AB970
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		this.missed = false;
		if (this.CollisionDeath.Enemies)
		{
			this.OnCollisionDie(hit, phase);
		}
	}

	// Token: 0x0600436A RID: 17258 RVA: 0x000AD591 File Offset: 0x000AB991
	protected override void OnCollisionEnemyProjectile(GameObject hit, CollisionPhase phase)
	{
		if (this.CollisionDeath.EnemyProjectiles)
		{
			this.OnCollisionDie(hit, phase);
		}
	}

	// Token: 0x0600436B RID: 17259 RVA: 0x000AD5AB File Offset: 0x000AB9AB
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.CollisionDeath.Player)
		{
			this.OnCollisionDie(hit, phase);
		}
	}

	// Token: 0x0600436C RID: 17260 RVA: 0x000AD5C5 File Offset: 0x000AB9C5
	protected override void OnCollisionPlayerProjectile(GameObject hit, CollisionPhase phase)
	{
		if (this.CollisionDeath.PlayerProjectiles)
		{
			this.OnCollisionDie(hit, phase);
		}
	}

	// Token: 0x0600436D RID: 17261 RVA: 0x000AD5DF File Offset: 0x000AB9DF
	protected override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		if (this.CollisionDeath.Other)
		{
			this.OnCollisionDie(hit, phase);
		}
	}

	// Token: 0x0600436E RID: 17262 RVA: 0x000AD5F9 File Offset: 0x000AB9F9
	public virtual void OnCollisionWideShotEX(GameObject hit, CollisionPhase phase)
	{
		this.OnCollisionDie(hit, phase);
	}

	// Token: 0x0600436F RID: 17263 RVA: 0x000AD603 File Offset: 0x000ABA03
	protected virtual void OnCollisionDie(GameObject hit, CollisionPhase phase)
	{
		if (!this.dead)
		{
			this.Die();
		}
	}

	// Token: 0x06004370 RID: 17264 RVA: 0x000AD616 File Offset: 0x000ABA16
	protected virtual void OnDieAnimationComplete()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06004371 RID: 17265 RVA: 0x000AD623 File Offset: 0x000ABA23
	public virtual void OnParry(AbstractPlayerController player)
	{
		if (this.CanParry)
		{
			this.OnParryDie();
		}
	}

	// Token: 0x06004372 RID: 17266 RVA: 0x000AD636 File Offset: 0x000ABA36
	public virtual void OnParryDie()
	{
		UnityEngine.Object.Destroy(base.gameObject);
		this.Die();
	}

	// Token: 0x06004373 RID: 17267 RVA: 0x000AD649 File Offset: 0x000ABA49
	public override void OnLevelEnd()
	{
		base.OnLevelEnd();
		this.Die();
	}

	// Token: 0x06004374 RID: 17268 RVA: 0x000AD658 File Offset: 0x000ABA58
	protected virtual void Die()
	{
		this.dead = true;
		if (base.GetComponent<Collider2D>() != null)
		{
			base.GetComponent<Collider2D>().enabled = false;
		}
		this.RandomizeVariant();
		this.SetTrigger(AbstractProjectile.OnDeathTrigger);
		if (this.OnDie != null)
		{
			this.OnDie(this);
		}
	}

	// Token: 0x06004375 RID: 17269 RVA: 0x000AD6B1 File Offset: 0x000ABAB1
	protected virtual void OnDieDistance()
	{
		if (this.DestroyDistanceAnimated)
		{
			this.Die();
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06004376 RID: 17270 RVA: 0x000AD6D4 File Offset: 0x000ABAD4
	protected virtual void OnDieLifetime()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06004377 RID: 17271 RVA: 0x000AD6E1 File Offset: 0x000ABAE1
	protected virtual void SetTrigger(string trigger)
	{
		base.animator.SetTrigger(trigger);
	}

	// Token: 0x06004378 RID: 17272 RVA: 0x000AD6EF File Offset: 0x000ABAEF
	protected virtual void SetInt(string integer, int i)
	{
		base.animator.SetInteger(integer, i);
	}

	// Token: 0x06004379 RID: 17273 RVA: 0x000AD6FE File Offset: 0x000ABAFE
	protected virtual void SetBool(string boolean, bool b)
	{
		base.animator.SetBool(boolean, b);
	}

	// Token: 0x0600437A RID: 17274 RVA: 0x000AD70D File Offset: 0x000ABB0D
	protected virtual int GetVariants()
	{
		return base.animator.GetInteger(AbstractProjectile.MaxVariants);
	}

	// Token: 0x0600437B RID: 17275 RVA: 0x000AD720 File Offset: 0x000ABB20
	protected virtual void RandomizeVariant()
	{
		int i = UnityEngine.Random.Range(0, this.GetVariants());
		this.SetInt(AbstractProjectile.Variant, i);
	}

	// Token: 0x0600437C RID: 17276 RVA: 0x000AD746 File Offset: 0x000ABB46
	public void AddFiringHitbox(LevelPlayerWeaponFiringHitbox hitbox)
	{
		this.firingHitbox = hitbox;
		base.RegisterCollisionChild(hitbox);
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x0600437D RID: 17277 RVA: 0x000AD764 File Offset: 0x000ABB64
	protected virtual void FixedUpdate()
	{
		if (this.firingHitbox != null)
		{
			if (this.firstUpdate)
			{
				this.firstUpdate = false;
			}
			else
			{
				if (!this.dead)
				{
					base.GetComponent<Collider2D>().enabled = true;
				}
				UnityEngine.Object.Destroy(this.firingHitbox.gameObject);
				this.firingHitbox = null;
			}
		}
		if (this.damageDealer != null)
		{
			this.damageDealer.FixedUpdate();
		}
	}

	// Token: 0x0600437E RID: 17278 RVA: 0x000AD7DD File Offset: 0x000ABBDD
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.firingHitbox != null)
		{
			UnityEngine.Object.Destroy(this.firingHitbox.gameObject);
		}
	}

	// Token: 0x0600437F RID: 17279 RVA: 0x000AD806 File Offset: 0x000ABC06
	public virtual void AddToMeterScoreTracker(MeterScoreTracker tracker)
	{
		this.tracker = tracker;
		if (this.damageDealer != null)
		{
			tracker.Add(this.damageDealer);
		}
	}

	// Token: 0x06004380 RID: 17280 RVA: 0x000AD828 File Offset: 0x000ABC28
	public static IEnumerable<DamageReceiver> FindOverlapScreenDamageReceivers()
	{
		AbstractProjectile.DamageReceiverComponentBuffer.Clear();
		AbstractProjectile.DamageReceiverSearchSet.Clear();
		Vector2 padding = new Vector2(100f, 100f);
		Rect rect = CupheadLevelCamera.Current.CalculateContainsBounds(padding);
		int num = Physics2D.OverlapBoxNonAlloc(rect.center, rect.size, 0f, AbstractProjectile.ColliderBuffer);
		for (int i = 0; i < num; i++)
		{
			AbstractProjectile.DamageReceiverComponentBuffer.Clear();
			Collider2D collider2D = AbstractProjectile.ColliderBuffer[i];
			collider2D.GetComponentsInParent<DamageReceiver>(true, AbstractProjectile.DamageReceiverComponentBuffer);
			AbstractProjectile.DamageReceiverSearchSet.UnionWith(AbstractProjectile.DamageReceiverComponentBuffer);
		}
		return AbstractProjectile.DamageReceiverSearchSet;
	}

	// Token: 0x0400492B RID: 18731
	protected static string Variant = "Variant";

	// Token: 0x0400492C RID: 18732
	protected static string MaxVariants = "MaxVariants";

	// Token: 0x0400492D RID: 18733
	protected static string OnDeathTrigger = "OnDeath";

	// Token: 0x0400492E RID: 18734
	protected static string Parry = "Parry";

	// Token: 0x0400492F RID: 18735
	private Vector3 startPosition;

	// Token: 0x04004930 RID: 18736
	private Vector3 lastPosition;

	// Token: 0x04004931 RID: 18737
	protected MeterScoreTracker tracker;

	// Token: 0x04004932 RID: 18738
	private bool hasBeenRendered;

	// Token: 0x04004933 RID: 18739
	[SerializeField]
	private bool _canParry;

	// Token: 0x04004934 RID: 18740
	protected bool _countParryTowardsScore = true;

	// Token: 0x04004938 RID: 18744
	protected bool missed = true;

	// Token: 0x04004939 RID: 18745
	protected DamageDealer damageDealer;

	// Token: 0x0400493B RID: 18747
	private float _setYPadding = 150f;

	// Token: 0x0400493C RID: 18748
	private DamageDealer.DamageSource damageSource;

	// Token: 0x0400493D RID: 18749
	public float Damage = 1f;

	// Token: 0x0400493E RID: 18750
	public float DamageRate;

	// Token: 0x0400493F RID: 18751
	public PlayerId PlayerId = PlayerId.None;

	// Token: 0x04004940 RID: 18752
	public DamageDealer.DamageTypesManager DamagesType;

	// Token: 0x04004941 RID: 18753
	public AbstractProjectile.CollisionProperties CollisionDeath;

	// Token: 0x04004942 RID: 18754
	[NonSerialized]
	public float DestroyDistance = 3000f;

	// Token: 0x04004943 RID: 18755
	[NonSerialized]
	public bool DestroyDistanceAnimated;

	// Token: 0x04004946 RID: 18758
	private LevelPlayerWeaponFiringHitbox firingHitbox;

	// Token: 0x04004947 RID: 18759
	private bool firstUpdate = true;

	// Token: 0x04004948 RID: 18760
	private static readonly Collider2D[] ColliderBuffer = new Collider2D[500];

	// Token: 0x04004949 RID: 18761
	private static HashSet<DamageReceiver> DamageReceiverSearchSet = new HashSet<DamageReceiver>();

	// Token: 0x0400494A RID: 18762
	private static List<DamageReceiver> DamageReceiverComponentBuffer = new List<DamageReceiver>();

	// Token: 0x02000AE4 RID: 2788
	[Serializable]
	public class CollisionProperties
	{
		// Token: 0x06004383 RID: 17283 RVA: 0x000AD941 File Offset: 0x000ABD41
		public AbstractProjectile.CollisionProperties Copy()
		{
			return base.MemberwiseClone() as AbstractProjectile.CollisionProperties;
		}

		// Token: 0x06004384 RID: 17284 RVA: 0x000AD94E File Offset: 0x000ABD4E
		public void SetAll(bool b)
		{
			this.Walls = b;
			this.Ceiling = b;
			this.Ground = b;
			this.Enemies = b;
			this.EnemyProjectiles = b;
			this.Player = b;
			this.PlayerProjectiles = b;
			this.Other = b;
		}

		// Token: 0x06004385 RID: 17285 RVA: 0x000AD988 File Offset: 0x000ABD88
		public void All()
		{
			this.SetAll(true);
		}

		// Token: 0x06004386 RID: 17286 RVA: 0x000AD991 File Offset: 0x000ABD91
		public void None()
		{
			this.SetAll(false);
		}

		// Token: 0x06004387 RID: 17287 RVA: 0x000AD99A File Offset: 0x000ABD9A
		public void OnlyPlayer()
		{
			this.SetAll(false);
			this.Player = true;
		}

		// Token: 0x06004388 RID: 17288 RVA: 0x000AD9AA File Offset: 0x000ABDAA
		public void OnlyEnemies()
		{
			this.SetAll(false);
			this.Player = true;
		}

		// Token: 0x06004389 RID: 17289 RVA: 0x000AD9BA File Offset: 0x000ABDBA
		public void OnlyBounds()
		{
			this.SetAll(false);
			this.SetBounds(true);
		}

		// Token: 0x0600438A RID: 17290 RVA: 0x000AD9CA File Offset: 0x000ABDCA
		public void SetBounds(bool b)
		{
			this.Walls = b;
			this.Ceiling = b;
			this.Ground = b;
		}

		// Token: 0x0600438B RID: 17291 RVA: 0x000AD9E1 File Offset: 0x000ABDE1
		public void PlayerProjectileDefault()
		{
			this.SetAll(false);
			this.SetBounds(true);
			this.Enemies = true;
			this.Other = true;
		}

		// Token: 0x0400494B RID: 18763
		public bool Walls = true;

		// Token: 0x0400494C RID: 18764
		public bool Ceiling = true;

		// Token: 0x0400494D RID: 18765
		public bool Ground = true;

		// Token: 0x0400494E RID: 18766
		public bool Enemies;

		// Token: 0x0400494F RID: 18767
		public bool EnemyProjectiles;

		// Token: 0x04004950 RID: 18768
		public bool Player;

		// Token: 0x04004951 RID: 18769
		public bool PlayerProjectiles;

		// Token: 0x04004952 RID: 18770
		public bool Other;
	}
}
