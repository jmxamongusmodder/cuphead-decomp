using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020009D2 RID: 2514
public abstract class AbstractPlayerController : AbstractPausableComponent
{
	// Token: 0x06003B0F RID: 15119 RVA: 0x00213868 File Offset: 0x00211C68
	public static AbstractPlayerController Create(PlayerId id, Vector2 pos, PlayerMode mode)
	{
		AbstractPlayerController abstractPlayerController;
		switch (mode)
		{
		default:
			abstractPlayerController = UnityEngine.Object.Instantiate<LevelPlayerController>(Level.Current.LevelResources.levelPlayer);
			break;
		case PlayerMode.Plane:
			abstractPlayerController = UnityEngine.Object.Instantiate<PlanePlayerController>(Level.Current.LevelResources.planePlayer);
			break;
		case PlayerMode.Arcade:
			abstractPlayerController = UnityEngine.Object.Instantiate<ArcadePlayerController>(((RetroArcadeLevel)Level.Current).playerPrefab);
			break;
		}
		abstractPlayerController.transform.position = pos;
		abstractPlayerController.mode = mode;
		abstractPlayerController.LevelInit(id);
		return abstractPlayerController;
	}

	// Token: 0x170004D5 RID: 1237
	// (get) Token: 0x06003B10 RID: 15120 RVA: 0x002138FA File Offset: 0x00211CFA
	public PlayerInput input
	{
		get
		{
			if (this._input == null)
			{
				this._input = base.GetComponent<PlayerInput>();
			}
			return this._input;
		}
	}

	// Token: 0x170004D6 RID: 1238
	// (get) Token: 0x06003B11 RID: 15121 RVA: 0x0021391F File Offset: 0x00211D1F
	public PlayerStatsManager stats
	{
		get
		{
			if (this._stats == null)
			{
				this._stats = base.GetComponent<PlayerStatsManager>();
			}
			return this._stats;
		}
	}

	// Token: 0x170004D7 RID: 1239
	// (get) Token: 0x06003B12 RID: 15122 RVA: 0x00213944 File Offset: 0x00211D44
	public PlayerDamageReceiver damageReceiver
	{
		get
		{
			if (this._damageReceiver == null)
			{
				this._damageReceiver = base.GetComponent<PlayerDamageReceiver>();
			}
			return this._damageReceiver;
		}
	}

	// Token: 0x170004D8 RID: 1240
	// (get) Token: 0x06003B13 RID: 15123 RVA: 0x00213969 File Offset: 0x00211D69
	public PlayerCameraController cameraController
	{
		get
		{
			if (this._cameraController == null)
			{
				this._cameraController = base.GetComponent<PlayerCameraController>();
			}
			return this._cameraController;
		}
	}

	// Token: 0x170004D9 RID: 1241
	// (get) Token: 0x06003B14 RID: 15124 RVA: 0x0021398E File Offset: 0x00211D8E
	// (set) Token: 0x06003B15 RID: 15125 RVA: 0x00213996 File Offset: 0x00211D96
	public PlayerId id { get; private set; }

	// Token: 0x170004DA RID: 1242
	// (get) Token: 0x06003B16 RID: 15126 RVA: 0x0021399F File Offset: 0x00211D9F
	// (set) Token: 0x06003B17 RID: 15127 RVA: 0x002139A7 File Offset: 0x00211DA7
	public PlayerMode mode { get; private set; }

	// Token: 0x170004DB RID: 1243
	// (get) Token: 0x06003B18 RID: 15128 RVA: 0x002139B0 File Offset: 0x00211DB0
	public bool IsDead
	{
		get
		{
			return !this._isReviving && this.stats.Health <= 0;
		}
	}

	// Token: 0x170004DC RID: 1244
	// (get) Token: 0x06003B19 RID: 15129 RVA: 0x002139D0 File Offset: 0x00211DD0
	// (set) Token: 0x06003B1A RID: 15130 RVA: 0x002139D8 File Offset: 0x00211DD8
	public bool levelStarted { get; protected set; }

	// Token: 0x170004DD RID: 1245
	// (get) Token: 0x06003B1B RID: 15131 RVA: 0x002139E1 File Offset: 0x00211DE1
	// (set) Token: 0x06003B1C RID: 15132 RVA: 0x002139E9 File Offset: 0x00211DE9
	public bool levelEnded { get; protected set; }

	// Token: 0x170004DE RID: 1246
	// (get) Token: 0x06003B1D RID: 15133 RVA: 0x002139F2 File Offset: 0x00211DF2
	public BoxCollider2D collider
	{
		get
		{
			if (this._collider == null)
			{
				this._collider = base.GetComponent<BoxCollider2D>();
			}
			return this._collider;
		}
	}

	// Token: 0x170004DF RID: 1247
	// (get) Token: 0x06003B1E RID: 15134 RVA: 0x00213A17 File Offset: 0x00211E17
	public BoxCollider2D collider2D
	{
		get
		{
			return this.collider;
		}
	}

	// Token: 0x170004E0 RID: 1248
	// (get) Token: 0x06003B1F RID: 15135 RVA: 0x00213A1F File Offset: 0x00211E1F
	public virtual Vector3 center
	{
		get
		{
			if (base.transform == null)
			{
				return Vector3.zero;
			}
			return base.transform.position + this.collider.offset;
		}
	}

	// Token: 0x170004E1 RID: 1249
	// (get) Token: 0x06003B20 RID: 15136 RVA: 0x00213A58 File Offset: 0x00211E58
	public virtual Vector3 CameraCenter
	{
		get
		{
			return this.cameraController.center;
		}
	}

	// Token: 0x170004E2 RID: 1250
	// (get) Token: 0x06003B21 RID: 15137 RVA: 0x00213A6C File Offset: 0x00211E6C
	public float left
	{
		get
		{
			return this.center.x - this.collider.size.x * 0.5f;
		}
	}

	// Token: 0x170004E3 RID: 1251
	// (get) Token: 0x06003B22 RID: 15138 RVA: 0x00213AA4 File Offset: 0x00211EA4
	public float right
	{
		get
		{
			return this.center.x + this.collider.size.x * 0.5f;
		}
	}

	// Token: 0x170004E4 RID: 1252
	// (get) Token: 0x06003B23 RID: 15139 RVA: 0x00213ADC File Offset: 0x00211EDC
	public float top
	{
		get
		{
			return this.center.y + this.collider.size.y * 0.5f;
		}
	}

	// Token: 0x170004E5 RID: 1253
	// (get) Token: 0x06003B24 RID: 15140 RVA: 0x00213B14 File Offset: 0x00211F14
	public float bottom
	{
		get
		{
			return this.center.y - this.collider.size.y * 0.5f;
		}
	}

	// Token: 0x170004E6 RID: 1254
	// (get) Token: 0x06003B25 RID: 15141 RVA: 0x00213B49 File Offset: 0x00211F49
	public float width
	{
		get
		{
			return this.right - this.left;
		}
	}

	// Token: 0x170004E7 RID: 1255
	// (get) Token: 0x06003B26 RID: 15142 RVA: 0x00213B58 File Offset: 0x00211F58
	public float height
	{
		get
		{
			return this.top - this.bottom;
		}
	}

	// Token: 0x170004E8 RID: 1256
	// (get) Token: 0x06003B27 RID: 15143
	public abstract bool CanTakeDamage { get; }

	// Token: 0x14000074 RID: 116
	// (add) Token: 0x06003B28 RID: 15144 RVA: 0x00213B68 File Offset: 0x00211F68
	// (remove) Token: 0x06003B29 RID: 15145 RVA: 0x00213BA0 File Offset: 0x00211FA0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnPlayIntroEvent;

	// Token: 0x06003B2A RID: 15146 RVA: 0x00213BD6 File Offset: 0x00211FD6
	public virtual void PlayIntro()
	{
		if (this.OnPlayIntroEvent != null)
		{
			this.OnPlayIntroEvent();
		}
	}

	// Token: 0x14000075 RID: 117
	// (add) Token: 0x06003B2B RID: 15147 RVA: 0x00213BF0 File Offset: 0x00211FF0
	// (remove) Token: 0x06003B2C RID: 15148 RVA: 0x00213C28 File Offset: 0x00212028
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnPlatformingLevelAwakeEvent;

	// Token: 0x06003B2D RID: 15149 RVA: 0x00213C5E File Offset: 0x0021205E
	public virtual void OnPlatformingLevelAwake()
	{
		if (this.OnPlatformingLevelAwakeEvent != null)
		{
			this.OnPlatformingLevelAwakeEvent();
		}
	}

	// Token: 0x14000076 RID: 118
	// (add) Token: 0x06003B2E RID: 15150 RVA: 0x00213C78 File Offset: 0x00212078
	// (remove) Token: 0x06003B2F RID: 15151 RVA: 0x00213CB0 File Offset: 0x002120B0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event AbstractPlayerController.OnReviveHandler OnReviveEvent;

	// Token: 0x06003B30 RID: 15152 RVA: 0x00213CE8 File Offset: 0x002120E8
	protected override void Awake()
	{
		AbstractPlayerController[] array = UnityEngine.Object.FindObjectsOfType<AbstractPlayerController>();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].name.Contains("PlayerTwo"))
			{
				UnityEngine.Object.Destroy(array[i].gameObject);
			}
		}
		base.Awake();
		if (Level.Current == null || !Level.Current.PlayersCreated)
		{
			if (base.gameObject != null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			return;
		}
	}

	// Token: 0x06003B31 RID: 15153 RVA: 0x00213D78 File Offset: 0x00212178
	protected virtual void LevelInit(PlayerId id)
	{
		this.id = id;
		base.name = id.ToString();
		PlayerManager.SetPlayer(id, this);
		this.input.Init(this.id);
		this.cameraController.LevelInit();
		this.stats.LevelInit();
		this.stats.OnPlayerDeathEvent += this.OnDeath;
	}

	// Token: 0x06003B32 RID: 15154 RVA: 0x00213DE5 File Offset: 0x002121E5
	public virtual void OnLevelWin()
	{
	}

	// Token: 0x06003B33 RID: 15155 RVA: 0x00213DE8 File Offset: 0x002121E8
	public virtual void LevelStart()
	{
		foreach (AbstractPlayerComponent abstractPlayerComponent in base.GetComponentsInChildren<AbstractPlayerComponent>())
		{
			abstractPlayerComponent.OnLevelStart();
		}
		this.levelStarted = true;
	}

	// Token: 0x06003B34 RID: 15156 RVA: 0x00213E21 File Offset: 0x00212221
	public override void OnLevelEnd()
	{
		base.OnLevelEnd();
		this.levelEnded = true;
	}

	// Token: 0x06003B35 RID: 15157 RVA: 0x00213E30 File Offset: 0x00212230
	protected virtual void OnDeath(PlayerId playerId)
	{
		this._isReviving = false;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06003B36 RID: 15158 RVA: 0x00213E45 File Offset: 0x00212245
	public virtual void OnLeave(PlayerId playerId)
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003B37 RID: 15159 RVA: 0x00213E52 File Offset: 0x00212252
	protected virtual void OnPreRevive(Vector3 pos)
	{
		this.stats.OnPreRevive();
		this._isReviving = true;
		base.transform.position = pos;
	}

	// Token: 0x06003B38 RID: 15160 RVA: 0x00213E74 File Offset: 0x00212274
	public virtual void LevelJoin(Vector3 pos)
	{
		this.LevelStart();
		base.gameObject.SetActive(false);
		Vector3 position = base.transform.position;
		PlayerJoinEffect playerJoinEffect = PlayerJoinEffect.Create(this.id, base.transform.position, this.mode, this.stats.isChalice);
		playerJoinEffect.OnPreReviveEvent += this.OnPreRevive;
		playerJoinEffect.OnReviveEvent += this.OnRevive;
		this.OnPreRevive(pos);
	}

	// Token: 0x06003B39 RID: 15161 RVA: 0x00213EF9 File Offset: 0x002122F9
	public virtual void BufferInputs()
	{
	}

	// Token: 0x06003B3A RID: 15162 RVA: 0x00213EFC File Offset: 0x002122FC
	protected virtual void OnRevive(Vector3 pos)
	{
		this.reviveHelper = new GameObjectHelper("Revive Helper");
		this.reviveHelper.events.StartCoroutine(this.reviveDelay_cr(1, pos));
		this.stats.OnRevive();
		base.transform.position = pos;
	}

	// Token: 0x06003B3B RID: 15163 RVA: 0x00213F4C File Offset: 0x0021234C
	private IEnumerator reviveDelay_cr(int frameDelay, Vector3 pos)
	{
		for (int i = 0; i < frameDelay; i++)
		{
			yield return null;
		}
		this._isReviving = false;
		base.gameObject.SetActive(true);
		if (this.OnReviveEvent != null)
		{
			this.OnReviveEvent(pos);
		}
		yield break;
	}

	// Token: 0x040042C7 RID: 17095
	private PlayerInput _input;

	// Token: 0x040042C8 RID: 17096
	private PlayerStatsManager _stats;

	// Token: 0x040042C9 RID: 17097
	private PlayerDamageReceiver _damageReceiver;

	// Token: 0x040042CA RID: 17098
	private PlayerCameraController _cameraController;

	// Token: 0x040042CD RID: 17101
	private bool _isReviving;

	// Token: 0x040042D0 RID: 17104
	private BoxCollider2D _collider;

	// Token: 0x040042D4 RID: 17108
	private GameObjectHelper reviveHelper;

	// Token: 0x020009D3 RID: 2515
	// (Invoke) Token: 0x06003B3D RID: 15165
	public delegate void OnReviveHandler(Vector3 pos);
}
