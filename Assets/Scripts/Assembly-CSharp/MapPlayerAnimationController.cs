using System;
using System.Linq;
using UnityEngine;

// Token: 0x02000973 RID: 2419
public class MapPlayerAnimationController : AbstractMapPlayerComponent
{
	// Token: 0x1700048E RID: 1166
	// (get) Token: 0x0600385B RID: 14427 RVA: 0x00203581 File Offset: 0x00201981
	// (set) Token: 0x0600385C RID: 14428 RVA: 0x00203589 File Offset: 0x00201989
	public MapPlayerAnimationController.Direction direction { get; private set; }

	// Token: 0x1700048F RID: 1167
	// (get) Token: 0x0600385D RID: 14429 RVA: 0x00203592 File Offset: 0x00201992
	// (set) Token: 0x0600385E RID: 14430 RVA: 0x0020359A File Offset: 0x0020199A
	public MapPlayerAnimationController.State state { get; private set; }

	// Token: 0x17000490 RID: 1168
	// (get) Token: 0x0600385F RID: 14431 RVA: 0x002035A3 File Offset: 0x002019A3
	// (set) Token: 0x06003860 RID: 14432 RVA: 0x002035AB File Offset: 0x002019AB
	public SpriteRenderer spriteRenderer { get; set; }

	// Token: 0x06003861 RID: 14433 RVA: 0x002035B4 File Offset: 0x002019B4
	public void Init(MapPlayerPose pose)
	{
		this.Cuphead.enabled = false;
		this.Mugman.enabled = false;
		this.ghostInPortal[0].sortingLayerName = "Effects";
		this.ghostInPortal[1].sortingLayerName = "Effects";
		this.portal.sortingLayerName = "Effects";
		PlayerId id = base.player.id;
		if (id == PlayerId.PlayerOne || id != PlayerId.PlayerTwo)
		{
			this.spriteRenderer = ((!PlayerManager.player1IsMugman) ? this.Cuphead : this.Mugman);
			base.animator.SetInteger("Player", 0);
		}
		else
		{
			base.animator.SetInteger("Player", 1);
			this.spriteRenderer = ((!PlayerManager.player1IsMugman) ? this.Mugman : this.Cuphead);
		}
		this.spriteRenderer.enabled = true;
		switch (pose)
		{
		case MapPlayerPose.Default:
			this.state = MapPlayerAnimationController.State.Idle;
			break;
		case MapPlayerPose.Joined:
		case MapPlayerPose.Won:
			base.animator.Play((!PlayerManager.playerWasChalice[(int)base.player.id]) ? "Jump" : "WinChalice_Loop");
			if (PlayerManager.playerWasChalice[(int)base.player.id])
			{
				if (PlayerManager.player1IsMugman)
				{
					this.ghostInPortal[(int)base.player.id].enabled = false;
				}
				else
				{
					this.ghostInPortal[(int)(PlayerId.PlayerTwo - base.player.id)].enabled = false;
				}
				if (base.player.id == PlayerId.PlayerTwo)
				{
					base.transform.localScale = new Vector3(-1f, 1f);
				}
			}
			break;
		}
		this.SetProperties();
	}

	// Token: 0x06003862 RID: 14434 RVA: 0x0020378A File Offset: 0x00201B8A
	private void MovePortalSwapToFront()
	{
		this.Chalice.sortingLayerName = "Effects";
	}

	// Token: 0x06003863 RID: 14435 RVA: 0x0020379C File Offset: 0x00201B9C
	private void Update()
	{
		if (base.player.state == MapPlayerController.State.Stationary)
		{
			this.SetStationary();
			return;
		}
		if (!MapPlayerController.CanMove())
		{
			this.SetStationary();
			return;
		}
		Vector2 vector = new Vector2(base.player.input.actions.GetAxis(0), base.player.input.actions.GetAxis(1));
		this.state = ((vector.magnitude <= 0.3f) ? MapPlayerAnimationController.State.Idle : MapPlayerAnimationController.State.Walk);
		this.SetProperties();
		this.UpdateDjimmiCodeTimer();
	}

	// Token: 0x06003864 RID: 14436 RVA: 0x0020382F File Offset: 0x00201C2F
	private void SetStationary()
	{
		this.state = MapPlayerAnimationController.State.Idle;
		this.axis.x = 0;
		this.axis.y = 0;
		this.SetProperties();
	}

	// Token: 0x06003865 RID: 14437 RVA: 0x00203860 File Offset: 0x00201C60
	public void CompleteJump()
	{
		base.animator.SetTrigger("OnJumpComplete");
	}

	// Token: 0x06003866 RID: 14438 RVA: 0x00203874 File Offset: 0x00201C74
	private void SetProperties()
	{
		if (this.state == MapPlayerAnimationController.State.Walk)
		{
			this.axis.x = base.player.input.GetAxisInt(PlayerInput.Axis.X, false, false);
			this.axis.y = base.player.input.GetAxisInt(PlayerInput.Axis.Y, false, false);
			if (this.axis.x == -1)
			{
				this.spriteRenderer.transform.SetScale(new float?(-1f), null, null);
			}
			else
			{
				this.spriteRenderer.transform.SetScale(new float?(1f), null, null);
			}
		}
		base.animator.SetInteger("X", this.axis.x);
		base.animator.SetInteger("Y", this.axis.y);
		base.animator.SetInteger("Speed", (this.state != MapPlayerAnimationController.State.Idle) ? 1 : 0);
		this.SetDirectionRotation();
	}

	// Token: 0x06003867 RID: 14439 RVA: 0x002039B0 File Offset: 0x00201DB0
	private void SetDirectionRotation()
	{
		this.facingUpwards = (this.axis.y > 0);
		if (this.axis.x == 1 && this.axis.y == 1)
		{
			this.directionRotation = -45f;
		}
		else if (this.axis.x == 1 && this.axis.y == 0)
		{
			this.directionRotation = -90f;
		}
		else if (this.axis.x == 1 && this.axis.y == -1)
		{
			this.directionRotation = -135f;
		}
		else if (this.axis.x == 0 && this.axis.y == 1)
		{
			this.directionRotation = 0f;
		}
		else if (this.axis.x == 0 && this.axis.y == 0)
		{
			this.directionRotation = 0f;
		}
		else if (this.axis.x == 0 && this.axis.y == -1)
		{
			this.directionRotation = -180f;
		}
		else if (this.axis.x == -1 && this.axis.y == 1)
		{
			this.directionRotation = 45f;
		}
		else if (this.axis.x == -1 && this.axis.y == 0)
		{
			this.directionRotation = 90f;
		}
		else if (this.axis.x == -1 && this.axis.y == -1)
		{
			this.directionRotation = 135f;
		}
		this.UpdateDjimmiCode((int)this.directionRotation);
	}

	// Token: 0x06003868 RID: 14440 RVA: 0x00203BF4 File Offset: 0x00201FF4
	private void UpdateDjimmiCode(int direction)
	{
		if (direction == -45 || direction == -135 || direction == 45 || direction == 135)
		{
			return;
		}
		if (direction == this.djimmiCodeEntry[this.djimmiCodeEntry.Length - 1])
		{
			return;
		}
		for (int i = 0; i < this.djimmiCodeEntry.Length - 1; i++)
		{
			this.djimmiCodeEntry[i] = this.djimmiCodeEntry[i + 1];
			this.djimmiCodeTimeStamp[i] = this.djimmiCodeTimeStamp[i + 1];
		}
		this.djimmiCodeEntry[this.djimmiCodeEntry.Length - 1] = direction;
		this.djimmiCodeTimeStamp[this.djimmiCodeEntry.Length - 1] = 2f;
		if (this.djimmiCodeTimeStamp[0] > 0f && (this.djimmiCodeEntry.SequenceEqual(this.djimmiCodeA) || this.djimmiCodeEntry.SequenceEqual(this.djimmiCodeB)))
		{
			for (int j = 0; j < this.djimmiCodeEntry.Length; j++)
			{
				this.djimmiCodeEntry[j] = 0;
				this.djimmiCodeTimeStamp[j] = 0f;
			}
			base.player.TryActivateDjimmi();
		}
	}

	// Token: 0x06003869 RID: 14441 RVA: 0x00203D20 File Offset: 0x00202120
	private void UpdateDjimmiCodeTimer()
	{
		for (int i = 0; i < this.djimmiCodeTimeStamp.Length; i++)
		{
			this.djimmiCodeTimeStamp[i] -= CupheadTime.Delta;
		}
	}

	// Token: 0x0600386A RID: 14442 RVA: 0x00203D60 File Offset: 0x00202160
	private void WalkStepLeft()
	{
		if (this.spriteRenderer == this.Cuphead)
		{
			if (this.current != null)
			{
				this.current.PlaySoundRight(true);
			}
			else
			{
				AudioManager.Play("player_map_walk_one_p1");
			}
		}
		else if (this.current != null)
		{
			this.current.PlaySoundRight(false);
		}
		else
		{
			AudioManager.Play("player_map_walk_one_p2");
		}
		this.dustEffect.Create(base.transform.position, this.directionRotation, true, this.spriteRenderer.sortingOrder);
	}

	// Token: 0x0600386B RID: 14443 RVA: 0x00203E0C File Offset: 0x0020220C
	private void WalkStepRight()
	{
		if (this.spriteRenderer == this.Cuphead)
		{
			if (this.current != null)
			{
				this.current.PlaySoundRight(true);
			}
			else
			{
				AudioManager.Play("player_map_walk_one_p1");
			}
		}
		else if (this.current != null)
		{
			this.current.PlaySoundRight(false);
		}
		else
		{
			AudioManager.Play("player_map_walk_two_p2");
		}
		this.dustEffect.Create(base.transform.position, this.directionRotation, false, this.spriteRenderer.sortingOrder);
	}

	// Token: 0x0600386C RID: 14444 RVA: 0x00203EB5 File Offset: 0x002022B5
	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.GetComponent<MapSpritePlaySound>())
		{
			this.current = collider.GetComponent<MapSpritePlaySound>();
		}
	}

	// Token: 0x0600386D RID: 14445 RVA: 0x00203ED3 File Offset: 0x002022D3
	private void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.GetComponent<MapSpritePlaySound>())
		{
			this.current = null;
		}
	}

	// Token: 0x0600386E RID: 14446 RVA: 0x00203EEC File Offset: 0x002022EC
	private void AniEvent_YawnSFX()
	{
		if (base.player.id == PlayerId.PlayerOne)
		{
			AudioManager.Play("worldmap_playeryawn");
			this.emitAudioFromObject.Add("worldmap_playeryawn");
		}
	}

	// Token: 0x0600386F RID: 14447 RVA: 0x00203F18 File Offset: 0x00202318
	private void AniEvent_GhostSwapSFX()
	{
		AudioManager.Play("sfx_DLC_WorldMap_GhostSwap");
		this.emitAudioFromObject.Add("sfx_DLC_WorldMap_GhostSwap");
	}

	// Token: 0x0400402F RID: 16431
	private const int DJIMMI_CODE_LENGTH = 16;

	// Token: 0x04004030 RID: 16432
	private const float MAX_TIME_FOR_DJIMMI_CODE = 2f;

	// Token: 0x04004031 RID: 16433
	private int[] djimmiCodeA = new int[]
	{
		0,
		90,
		-180,
		-90,
		0,
		90,
		-180,
		-90,
		0,
		90,
		-180,
		-90,
		0,
		90,
		-180,
		-90
	};

	// Token: 0x04004032 RID: 16434
	private int[] djimmiCodeB = new int[]
	{
		0,
		-90,
		-180,
		90,
		0,
		-90,
		-180,
		90,
		0,
		-90,
		-180,
		90,
		0,
		-90,
		-180,
		90
	};

	// Token: 0x04004033 RID: 16435
	public bool facingUpwards;

	// Token: 0x04004034 RID: 16436
	[SerializeField]
	private SpriteRenderer Cuphead;

	// Token: 0x04004035 RID: 16437
	[SerializeField]
	private SpriteRenderer Mugman;

	// Token: 0x04004036 RID: 16438
	[SerializeField]
	private SpriteRenderer Chalice;

	// Token: 0x04004037 RID: 16439
	[SerializeField]
	private SpriteRenderer[] ghostInPortal;

	// Token: 0x04004038 RID: 16440
	[SerializeField]
	private SpriteRenderer portal;

	// Token: 0x04004039 RID: 16441
	[SerializeField]
	private MapPlayerDust dustEffect;

	// Token: 0x0400403D RID: 16445
	private MapSpritePlaySound current;

	// Token: 0x0400403E RID: 16446
	private Trilean2 axis;

	// Token: 0x0400403F RID: 16447
	private bool onBridge;

	// Token: 0x04004040 RID: 16448
	private float directionRotation;

	// Token: 0x04004041 RID: 16449
	private int[] djimmiCodeEntry = new int[16];

	// Token: 0x04004042 RID: 16450
	private float[] djimmiCodeTimeStamp = new float[16];

	// Token: 0x02000974 RID: 2420
	public enum Direction
	{
		// Token: 0x04004044 RID: 16452
		Left,
		// Token: 0x04004045 RID: 16453
		Right
	}

	// Token: 0x02000975 RID: 2421
	public enum State
	{
		// Token: 0x04004047 RID: 16455
		Idle,
		// Token: 0x04004048 RID: 16456
		Walk
	}
}
