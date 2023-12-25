using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000976 RID: 2422
public class MapPlayerController : MapSprite
{
	// Token: 0x14000068 RID: 104
	// (add) Token: 0x06003871 RID: 14449 RVA: 0x00203F3C File Offset: 0x0020233C
	// (remove) Token: 0x06003872 RID: 14450 RVA: 0x00203F70 File Offset: 0x00202370
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event Action OnEquipMenuOpenedEvent;

	// Token: 0x14000069 RID: 105
	// (add) Token: 0x06003873 RID: 14451 RVA: 0x00203FA4 File Offset: 0x002023A4
	// (remove) Token: 0x06003874 RID: 14452 RVA: 0x00203FD8 File Offset: 0x002023D8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event Action OnEquipMenuClosedEvent;

	// Token: 0x06003875 RID: 14453 RVA: 0x0020400C File Offset: 0x0020240C
	public static bool CanMove()
	{
		return MapDifficultySelectStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Inactive && MapConfirmStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Inactive && MapBasicStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Inactive && (!SceneLoader.Exists || (!SceneLoader.IsInIrisTransition && !SceneLoader.IsInBlurTransition)) && (!(Map.Current != null) || Map.Current.CurrentState != Map.State.Graveyard) && (!MapEventNotification.Current || !MapEventNotification.Current.showing);
	}

	// Token: 0x17000491 RID: 1169
	// (get) Token: 0x06003876 RID: 14454 RVA: 0x002040A6 File Offset: 0x002024A6
	// (set) Token: 0x06003877 RID: 14455 RVA: 0x002040AE File Offset: 0x002024AE
	public MapPlayerController.State state { get; private set; }

	// Token: 0x17000492 RID: 1170
	// (get) Token: 0x06003878 RID: 14456 RVA: 0x002040B7 File Offset: 0x002024B7
	// (set) Token: 0x06003879 RID: 14457 RVA: 0x002040BF File Offset: 0x002024BF
	public PlayerId id { get; private set; }

	// Token: 0x17000493 RID: 1171
	// (get) Token: 0x0600387A RID: 14458 RVA: 0x002040C8 File Offset: 0x002024C8
	// (set) Token: 0x0600387B RID: 14459 RVA: 0x002040D0 File Offset: 0x002024D0
	public bool EquipMenuOpen { get; private set; }

	// Token: 0x17000494 RID: 1172
	// (get) Token: 0x0600387C RID: 14460 RVA: 0x002040D9 File Offset: 0x002024D9
	// (set) Token: 0x0600387D RID: 14461 RVA: 0x002040E1 File Offset: 0x002024E1
	public PlayerInput input { get; private set; }

	// Token: 0x17000495 RID: 1173
	// (get) Token: 0x0600387E RID: 14462 RVA: 0x002040EA File Offset: 0x002024EA
	// (set) Token: 0x0600387F RID: 14463 RVA: 0x002040F2 File Offset: 0x002024F2
	public MapPlayerMotor motor { get; private set; }

	// Token: 0x17000496 RID: 1174
	// (get) Token: 0x06003880 RID: 14464 RVA: 0x002040FB File Offset: 0x002024FB
	// (set) Token: 0x06003881 RID: 14465 RVA: 0x00204103 File Offset: 0x00202503
	public MapPlayerAnimationController animationController { get; private set; }

	// Token: 0x17000497 RID: 1175
	// (get) Token: 0x06003882 RID: 14466 RVA: 0x0020410C File Offset: 0x0020250C
	// (set) Token: 0x06003883 RID: 14467 RVA: 0x00204114 File Offset: 0x00202514
	public MapPlayerLadderManager ladderManager { get; private set; }

	// Token: 0x17000498 RID: 1176
	// (get) Token: 0x06003884 RID: 14468 RVA: 0x0020411D File Offset: 0x0020251D
	public Vector2 Velocity
	{
		get
		{
			return this.motor.velocity;
		}
	}

	// Token: 0x17000499 RID: 1177
	// (get) Token: 0x06003885 RID: 14469 RVA: 0x0020412A File Offset: 0x0020252A
	public MapPlayerAnimationController.Direction Direction
	{
		get
		{
			return this.animationController.direction;
		}
	}

	// Token: 0x06003886 RID: 14470 RVA: 0x00204138 File Offset: 0x00202538
	protected override void Awake()
	{
		MapPlayerController[] array = UnityEngine.Object.FindObjectsOfType<MapPlayerController>();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].name.Contains("PlayerTwo"))
			{
				UnityEngine.Object.Destroy(array[i].gameObject);
			}
		}
		base.Awake();
		base.tag = "Player_Map";
		this.input = base.GetComponent<PlayerInput>();
		this.motor = base.GetComponent<MapPlayerMotor>();
		this.animationController = base.GetComponent<MapPlayerAnimationController>();
		this.ladderManager = base.GetComponent<MapPlayerLadderManager>();
	}

	// Token: 0x06003887 RID: 14471 RVA: 0x002041C4 File Offset: 0x002025C4
	private void Start()
	{
		MapPlayerController.OnEquipMenuOpenedEvent += this.OnEquipMenuOpened;
		MapPlayerController.OnEquipMenuClosedEvent += this.OnEquipMenuClosed;
	}

	// Token: 0x06003888 RID: 14472 RVA: 0x002041E8 File Offset: 0x002025E8
	protected override void OnDestroy()
	{
		base.OnDestroy();
		MapPlayerController.OnEquipMenuOpenedEvent -= this.OnEquipMenuOpened;
		MapPlayerController.OnEquipMenuClosedEvent -= this.OnEquipMenuClosed;
	}

	// Token: 0x06003889 RID: 14473 RVA: 0x00204214 File Offset: 0x00202614
	public static MapPlayerController Create(PlayerId playerId, MapPlayerController.InitObject init)
	{
		MapPlayerController mapPlayerController = UnityEngine.Object.Instantiate<MapPlayerController>(Map.Current.MapResources.mapPlayer);
		mapPlayerController.Init(playerId, init);
		return mapPlayerController;
	}

	// Token: 0x0600388A RID: 14474 RVA: 0x00204240 File Offset: 0x00202640
	private void Init(PlayerId playerId, MapPlayerController.InitObject init)
	{
		base.gameObject.name = playerId.ToString();
		this.id = playerId;
		this.input.Init(this.id);
		this.animationController.Init(init.pose);
		base.transform.position = init.position;
		switch (init.pose)
		{
		case MapPlayerPose.Default:
			this.state = MapPlayerController.State.Walking;
			break;
		case MapPlayerPose.Joined:
			this.state = MapPlayerController.State.Stationary;
			base.StartCoroutine(this.joined_cr());
			break;
		case MapPlayerPose.Won:
			this.state = MapPlayerController.State.Stationary;
			break;
		}
	}

	// Token: 0x0600388B RID: 14475 RVA: 0x002042F8 File Offset: 0x002026F8
	public void Disable()
	{
		this.state = MapPlayerController.State.Stationary;
	}

	// Token: 0x0600388C RID: 14476 RVA: 0x00204301 File Offset: 0x00202701
	public void Enable()
	{
		this.state = MapPlayerController.State.Walking;
	}

	// Token: 0x0600388D RID: 14477 RVA: 0x0020430A File Offset: 0x0020270A
	public void OnLeave()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600388E RID: 14478 RVA: 0x00204317 File Offset: 0x00202717
	private void OnChaliceJumpAnimationComplete()
	{
		base.transform.localScale = new Vector3(1f, 1f);
		this.OnJumpAnimationComplete();
	}

	// Token: 0x0600388F RID: 14479 RVA: 0x0020433C File Offset: 0x0020273C
	private void OnJumpAnimationComplete()
	{
		if (this.joinedMidGame)
		{
			this.joinedMidGame = false;
			if (this.id == PlayerId.PlayerTwo)
			{
				MapPlayerController mapPlayerController = Map.Current.players[0];
				if (mapPlayerController.state != MapPlayerController.State.Stationary)
				{
					this.Enable();
				}
			}
			else
			{
				this.Enable();
			}
		}
		else
		{
			this.Enable();
		}
	}

	// Token: 0x06003890 RID: 14480 RVA: 0x0020439C File Offset: 0x0020279C
	private void OnEquipMenuOpened()
	{
		this.EquipMenuOpen = true;
	}

	// Token: 0x06003891 RID: 14481 RVA: 0x002043A5 File Offset: 0x002027A5
	private void OnEquipMenuClosed()
	{
		this.EquipMenuOpen = false;
	}

	// Token: 0x1400006A RID: 106
	// (add) Token: 0x06003892 RID: 14482 RVA: 0x002043B0 File Offset: 0x002027B0
	// (remove) Token: 0x06003893 RID: 14483 RVA: 0x002043E8 File Offset: 0x002027E8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MapPlayerController.LadderEnterEventHandler LadderEnterEvent;

	// Token: 0x1400006B RID: 107
	// (add) Token: 0x06003894 RID: 14484 RVA: 0x00204420 File Offset: 0x00202820
	// (remove) Token: 0x06003895 RID: 14485 RVA: 0x00204458 File Offset: 0x00202858
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MapPlayerController.LadderExitEventHandler LadderExitEvent;

	// Token: 0x1400006C RID: 108
	// (add) Token: 0x06003896 RID: 14486 RVA: 0x00204490 File Offset: 0x00202890
	// (remove) Token: 0x06003897 RID: 14487 RVA: 0x002044C8 File Offset: 0x002028C8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action LadderEnterCompleteEvent;

	// Token: 0x1400006D RID: 109
	// (add) Token: 0x06003898 RID: 14488 RVA: 0x00204500 File Offset: 0x00202900
	// (remove) Token: 0x06003899 RID: 14489 RVA: 0x00204538 File Offset: 0x00202938
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action LadderExitCompleteEvent;

	// Token: 0x0600389A RID: 14490 RVA: 0x0020456E File Offset: 0x0020296E
	public void LadderEnter(Vector2 point, MapPlayerLadderObject ladder, MapLadder.Location location)
	{
		this.state = MapPlayerController.State.LadderEnter;
		if (this.LadderEnterEvent != null)
		{
			this.LadderEnterEvent(point, ladder, location);
		}
	}

	// Token: 0x0600389B RID: 14491 RVA: 0x00204590 File Offset: 0x00202990
	public void LadderExit(Vector2 point, Vector2 exit, MapLadder.Location location)
	{
		this.state = MapPlayerController.State.LadderExit;
		if (this.LadderExitEvent != null)
		{
			this.LadderExitEvent(point, exit, location);
		}
	}

	// Token: 0x0600389C RID: 14492 RVA: 0x002045B2 File Offset: 0x002029B2
	public void LadderEnterComplete()
	{
		this.state = MapPlayerController.State.Ladder;
		if (this.LadderEnterCompleteEvent != null)
		{
			this.LadderEnterCompleteEvent();
		}
	}

	// Token: 0x0600389D RID: 14493 RVA: 0x002045D1 File Offset: 0x002029D1
	public void LadderExitComplete()
	{
		this.state = MapPlayerController.State.Walking;
		if (this.LadderExitCompleteEvent != null)
		{
			this.LadderExitCompleteEvent();
		}
	}

	// Token: 0x0600389E RID: 14494 RVA: 0x002045F0 File Offset: 0x002029F0
	public void OnWinComplete()
	{
		this.animationController.CompleteJump();
	}

	// Token: 0x0600389F RID: 14495 RVA: 0x00204600 File Offset: 0x00202A00
	private IEnumerator joined_cr()
	{
		yield return null;
		this.joinedMidGame = true;
		this.animationController.CompleteJump();
		yield break;
	}

	// Token: 0x060038A0 RID: 14496 RVA: 0x0020461C File Offset: 0x00202A1C
	public void SecretPathEnter(bool enter)
	{
		SpriteRenderer[] componentsInChildren = base.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in componentsInChildren)
		{
			spriteRenderer.sortingOrder = ((!enter) ? 0 : -1000);
		}
		base.gameObject.layer = ((!enter) ? LayerMask.NameToLayer("Default") : LayerMask.NameToLayer("Map_Secret"));
		this.hideInteractionPrompts = enter;
	}

	// Token: 0x060038A1 RID: 14497 RVA: 0x00204692 File Offset: 0x00202A92
	public void TryActivateDjimmi()
	{
		if (!PlayerData.Data.TryActivateDjimmi())
		{
			AudioManager.Play("menu_locked");
		}
	}

	// Token: 0x060038A2 RID: 14498 RVA: 0x002046AD File Offset: 0x00202AAD
	public void JumpSFX()
	{
		AudioManager.Play("complete_bounce");
	}

	// Token: 0x0400404B RID: 16459
	public const string TAG = "Player_Map";

	// Token: 0x0400404F RID: 16463
	private bool joinedMidGame;

	// Token: 0x04004054 RID: 16468
	public bool hideInteractionPrompts;

	// Token: 0x02000977 RID: 2423
	public enum State
	{
		// Token: 0x0400405A RID: 16474
		Walking,
		// Token: 0x0400405B RID: 16475
		LadderEnter,
		// Token: 0x0400405C RID: 16476
		LadderExit,
		// Token: 0x0400405D RID: 16477
		Ladder,
		// Token: 0x0400405E RID: 16478
		Stationary
	}

	// Token: 0x02000978 RID: 2424
	[Serializable]
	public class InitObject
	{
		// Token: 0x060038A3 RID: 14499 RVA: 0x002046B9 File Offset: 0x00202AB9
		public InitObject(Vector2 position, MapPlayerPose pose)
		{
			this.position = position;
			this.pose = pose;
		}

		// Token: 0x0400405F RID: 16479
		public Vector2 position;

		// Token: 0x04004060 RID: 16480
		public MapPlayerPose pose;
	}

	// Token: 0x02000979 RID: 2425
	// (Invoke) Token: 0x060038A5 RID: 14501
	public delegate void LadderEnterEventHandler(Vector2 point, MapPlayerLadderObject ladder, MapLadder.Location location);

	// Token: 0x0200097A RID: 2426
	// (Invoke) Token: 0x060038A9 RID: 14505
	public delegate void LadderExitEventHandler(Vector2 point, Vector2 exit, MapLadder.Location location);
}
