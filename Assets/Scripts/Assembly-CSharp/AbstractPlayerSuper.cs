using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000A4E RID: 2638
public abstract class AbstractPlayerSuper : AbstractCollidableObject
{
	// Token: 0x1400009E RID: 158
	// (add) Token: 0x06003ECC RID: 16076 RVA: 0x00226754 File Offset: 0x00224B54
	// (remove) Token: 0x06003ECD RID: 16077 RVA: 0x0022678C File Offset: 0x00224B8C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnEndedEvent;

	// Token: 0x1400009F RID: 159
	// (add) Token: 0x06003ECE RID: 16078 RVA: 0x002267C4 File Offset: 0x00224BC4
	// (remove) Token: 0x06003ECF RID: 16079 RVA: 0x002267FC File Offset: 0x00224BFC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnStartedEvent;

	// Token: 0x1700056D RID: 1389
	// (get) Token: 0x06003ED0 RID: 16080 RVA: 0x00226832 File Offset: 0x00224C32
	protected override bool allowCollisionPlayer
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003ED1 RID: 16081 RVA: 0x00226835 File Offset: 0x00224C35
	protected override void Awake()
	{
		base.tag = "PlayerProjectile";
		base.Awake();
	}

	// Token: 0x06003ED2 RID: 16082 RVA: 0x00226848 File Offset: 0x00224C48
	protected virtual void Start()
	{
		this.animHelper = base.GetComponent<AnimationHelper>();
		base.transform.position = this.player.transform.position;
	}

	// Token: 0x06003ED3 RID: 16083 RVA: 0x00226871 File Offset: 0x00224C71
	protected virtual void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06003ED4 RID: 16084 RVA: 0x00226889 File Offset: 0x00224C89
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x06003ED5 RID: 16085 RVA: 0x002268AB File Offset: 0x00224CAB
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.player != null)
		{
			this.player.weaponManager.OnSuperInterrupt -= this.Interrupt;
		}
	}

	// Token: 0x06003ED6 RID: 16086 RVA: 0x002268E4 File Offset: 0x00224CE4
	public AbstractPlayerSuper Create(LevelPlayerController player)
	{
		AbstractPlayerSuper abstractPlayerSuper = this.InstantiatePrefab<AbstractPlayerSuper>();
		abstractPlayerSuper.player = player;
		PlayerId id = player.id;
		if (id == PlayerId.PlayerOne || id != PlayerId.PlayerTwo)
		{
			if (!this.isChaliceSuper)
			{
				abstractPlayerSuper.spriteRenderer = ((!PlayerManager.player1IsMugman) ? this.cuphead : this.mugman);
				abstractPlayerSuper.cuphead.gameObject.SetActive(!PlayerManager.player1IsMugman);
				abstractPlayerSuper.mugman.gameObject.SetActive(PlayerManager.player1IsMugman);
			}
		}
		else if (!this.isChaliceSuper)
		{
			abstractPlayerSuper.spriteRenderer = ((!PlayerManager.player1IsMugman) ? this.mugman : this.cuphead);
			abstractPlayerSuper.cuphead.gameObject.SetActive(PlayerManager.player1IsMugman);
			abstractPlayerSuper.mugman.gameObject.SetActive(!PlayerManager.player1IsMugman);
		}
		this.interrupted = false;
		player.weaponManager.OnSuperInterrupt += abstractPlayerSuper.Interrupt;
		abstractPlayerSuper.StartSuper();
		return abstractPlayerSuper;
	}

	// Token: 0x06003ED7 RID: 16087 RVA: 0x00226A04 File Offset: 0x00224E04
	public virtual void Interrupt()
	{
		this.interrupted = true;
	}

	// Token: 0x06003ED8 RID: 16088 RVA: 0x00226A10 File Offset: 0x00224E10
	protected virtual void StartSuper()
	{
		AnimationHelper component = base.GetComponent<AnimationHelper>();
		component.IgnoreGlobal = true;
		PauseManager.Pause();
		AudioManager.HandleSnapshot(AudioManager.Snapshots.SuperStart.ToString(), 0.2f);
		AudioManager.ChangeBGMPitch(1.3f, 1.5f);
		base.transform.SetScale(new float?(this.player.transform.localScale.x), new float?(this.player.transform.localScale.y), new float?(1f));
		base.transform.position = this.player.transform.position;
		if (this.OnStartedEvent != null)
		{
			this.OnStartedEvent();
		}
		this.OnStartedEvent = null;
	}

	// Token: 0x06003ED9 RID: 16089 RVA: 0x00226AE0 File Offset: 0x00224EE0
	protected virtual void Fire()
	{
		PauseManager.Unpause();
		AudioManager.HandleSnapshot(AudioManager.Snapshots.Super.ToString(), 0.2f);
		if (this.player == null)
		{
			this.Interrupt();
		}
		else
		{
			this.player.PauseAll();
		}
		AnimationHelper component = base.GetComponent<AnimationHelper>();
		component.IgnoreGlobal = false;
	}

	// Token: 0x06003EDA RID: 16090 RVA: 0x00226B40 File Offset: 0x00224F40
	protected virtual void EndSuper(bool changePitch = true)
	{
		AudioManager.SnapshotReset(SceneLoader.SceneName, 1f);
		if (changePitch)
		{
			AudioManager.ChangeBGMPitch(1f, 2f);
		}
		if (this.player != null)
		{
			this.player.UnpauseAll(false);
		}
		if (this.OnEndedEvent != null)
		{
			this.OnEndedEvent();
		}
		this.OnEndedEvent = null;
	}

	// Token: 0x040045D5 RID: 17877
	[SerializeField]
	[Header("Player Sprites")]
	private SpriteRenderer cuphead;

	// Token: 0x040045D6 RID: 17878
	[SerializeField]
	private SpriteRenderer mugman;

	// Token: 0x040045D7 RID: 17879
	[SerializeField]
	private bool isChaliceSuper;

	// Token: 0x040045D8 RID: 17880
	protected SpriteRenderer spriteRenderer;

	// Token: 0x040045D9 RID: 17881
	protected LevelPlayerController player;

	// Token: 0x040045DA RID: 17882
	protected DamageDealer damageDealer;

	// Token: 0x040045DB RID: 17883
	protected AnimationHelper animHelper;

	// Token: 0x040045DC RID: 17884
	protected bool interrupted;
}
