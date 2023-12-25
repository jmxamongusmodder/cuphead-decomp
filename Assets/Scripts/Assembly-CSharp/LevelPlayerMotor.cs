using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000A1F RID: 2591
public class LevelPlayerMotor : AbstractLevelPlayerComponent
{
	// Token: 0x1700053A RID: 1338
	// (get) Token: 0x06003D73 RID: 15731 RVA: 0x0021F0D0 File Offset: 0x0021D4D0
	// (set) Token: 0x06003D74 RID: 15732 RVA: 0x0021F0D8 File Offset: 0x0021D4D8
	public Trilean2 LookDirection { get; private set; }

	// Token: 0x1700053B RID: 1339
	// (get) Token: 0x06003D75 RID: 15733 RVA: 0x0021F0E1 File Offset: 0x0021D4E1
	// (set) Token: 0x06003D76 RID: 15734 RVA: 0x0021F0E9 File Offset: 0x0021D4E9
	public Trilean2 TrueLookDirection { get; private set; }

	// Token: 0x1700053C RID: 1340
	// (get) Token: 0x06003D77 RID: 15735 RVA: 0x0021F0F2 File Offset: 0x0021D4F2
	// (set) Token: 0x06003D78 RID: 15736 RVA: 0x0021F0FA File Offset: 0x0021D4FA
	public Trilean2 MoveDirection { get; private set; }

	// Token: 0x1700053D RID: 1341
	// (get) Token: 0x06003D79 RID: 15737 RVA: 0x0021F103 File Offset: 0x0021D503
	public LevelPlayerMotor.JumpManager.State JumpState
	{
		get
		{
			return this.jumpManager.state;
		}
	}

	// Token: 0x1700053E RID: 1342
	// (get) Token: 0x06003D7A RID: 15738 RVA: 0x0021F110 File Offset: 0x0021D510
	public bool Dashing
	{
		get
		{
			return this.dashManager.IsDashing;
		}
	}

	// Token: 0x1700053F RID: 1343
	// (get) Token: 0x06003D7B RID: 15739 RVA: 0x0021F11D File Offset: 0x0021D51D
	public int DashDirection
	{
		get
		{
			return this.dashManager.direction;
		}
	}

	// Token: 0x17000540 RID: 1344
	// (get) Token: 0x06003D7C RID: 15740 RVA: 0x0021F12A File Offset: 0x0021D52A
	public LevelPlayerMotor.DashManager.State DashState
	{
		get
		{
			return this.dashManager.state;
		}
	}

	// Token: 0x17000541 RID: 1345
	// (get) Token: 0x06003D7D RID: 15741 RVA: 0x0021F137 File Offset: 0x0021D537
	// (set) Token: 0x06003D7E RID: 15742 RVA: 0x0021F13F File Offset: 0x0021D53F
	public bool Locked { get; private set; }

	// Token: 0x17000542 RID: 1346
	// (get) Token: 0x06003D7F RID: 15743 RVA: 0x0021F148 File Offset: 0x0021D548
	// (set) Token: 0x06003D80 RID: 15744 RVA: 0x0021F150 File Offset: 0x0021D550
	public bool Grounded { get; private set; }

	// Token: 0x17000543 RID: 1347
	// (get) Token: 0x06003D81 RID: 15745 RVA: 0x0021F159 File Offset: 0x0021D559
	// (set) Token: 0x06003D82 RID: 15746 RVA: 0x0021F161 File Offset: 0x0021D561
	public bool Parrying { get; private set; }

	// Token: 0x17000544 RID: 1348
	// (get) Token: 0x06003D83 RID: 15747 RVA: 0x0021F16C File Offset: 0x0021D56C
	public bool Ducking
	{
		get
		{
			return this.LookDirection.y < 0 && !this.Locked && this.Grounded;
		}
	}

	// Token: 0x17000545 RID: 1349
	// (get) Token: 0x06003D84 RID: 15748 RVA: 0x0021F1A6 File Offset: 0x0021D5A6
	public bool IsHit
	{
		get
		{
			return this.hitManager.state == LevelPlayerMotor.HitManager.State.Hit;
		}
	}

	// Token: 0x17000546 RID: 1350
	// (get) Token: 0x06003D85 RID: 15749 RVA: 0x0021F1B6 File Offset: 0x0021D5B6
	public bool IsUsingSuperOrEx
	{
		get
		{
			return this.superManager.state == LevelPlayerMotor.SuperManager.State.Super || this.superManager.state == LevelPlayerMotor.SuperManager.State.Ex;
		}
	}

	// Token: 0x17000547 RID: 1351
	// (get) Token: 0x06003D86 RID: 15750 RVA: 0x0021F1DA File Offset: 0x0021D5DA
	// (set) Token: 0x06003D87 RID: 15751 RVA: 0x0021F1E2 File Offset: 0x0021D5E2
	public bool GravityReversed { get; private set; }

	// Token: 0x17000548 RID: 1352
	// (get) Token: 0x06003D88 RID: 15752 RVA: 0x0021F1EB File Offset: 0x0021D5EB
	// (set) Token: 0x06003D89 RID: 15753 RVA: 0x0021F1F3 File Offset: 0x0021D5F3
	public bool ChaliceDoubleJumped { get; private set; }

	// Token: 0x17000549 RID: 1353
	// (get) Token: 0x06003D8A RID: 15754 RVA: 0x0021F1FC File Offset: 0x0021D5FC
	// (set) Token: 0x06003D8B RID: 15755 RVA: 0x0021F204 File Offset: 0x0021D604
	public bool ChaliceDuckDashed { get; private set; }

	// Token: 0x1700054A RID: 1354
	// (get) Token: 0x06003D8C RID: 15756 RVA: 0x0021F20D File Offset: 0x0021D60D
	public float GravityReversalMultiplier
	{
		get
		{
			return (float)((!this.GravityReversed) ? 1 : -1);
		}
	}

	// Token: 0x1400008B RID: 139
	// (add) Token: 0x06003D8D RID: 15757 RVA: 0x0021F224 File Offset: 0x0021D624
	// (remove) Token: 0x06003D8E RID: 15758 RVA: 0x0021F25C File Offset: 0x0021D65C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnGroundedEvent;

	// Token: 0x1400008C RID: 140
	// (add) Token: 0x06003D8F RID: 15759 RVA: 0x0021F294 File Offset: 0x0021D694
	// (remove) Token: 0x06003D90 RID: 15760 RVA: 0x0021F2CC File Offset: 0x0021D6CC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnJumpEvent;

	// Token: 0x1400008D RID: 141
	// (add) Token: 0x06003D91 RID: 15761 RVA: 0x0021F304 File Offset: 0x0021D704
	// (remove) Token: 0x06003D92 RID: 15762 RVA: 0x0021F33C File Offset: 0x0021D73C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnDoubleJumpEvent;

	// Token: 0x1400008E RID: 142
	// (add) Token: 0x06003D93 RID: 15763 RVA: 0x0021F374 File Offset: 0x0021D774
	// (remove) Token: 0x06003D94 RID: 15764 RVA: 0x0021F3AC File Offset: 0x0021D7AC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnParryEvent;

	// Token: 0x1400008F RID: 143
	// (add) Token: 0x06003D95 RID: 15765 RVA: 0x0021F3E4 File Offset: 0x0021D7E4
	// (remove) Token: 0x06003D96 RID: 15766 RVA: 0x0021F41C File Offset: 0x0021D81C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnParrySuccess;

	// Token: 0x14000090 RID: 144
	// (add) Token: 0x06003D97 RID: 15767 RVA: 0x0021F454 File Offset: 0x0021D854
	// (remove) Token: 0x06003D98 RID: 15768 RVA: 0x0021F48C File Offset: 0x0021D88C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnHitEvent;

	// Token: 0x14000091 RID: 145
	// (add) Token: 0x06003D99 RID: 15769 RVA: 0x0021F4C4 File Offset: 0x0021D8C4
	// (remove) Token: 0x06003D9A RID: 15770 RVA: 0x0021F4FC File Offset: 0x0021D8FC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnDashStartEvent;

	// Token: 0x14000092 RID: 146
	// (add) Token: 0x06003D9B RID: 15771 RVA: 0x0021F534 File Offset: 0x0021D934
	// (remove) Token: 0x06003D9C RID: 15772 RVA: 0x0021F56C File Offset: 0x0021D96C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnDashEndEvent;

	// Token: 0x1700054B RID: 1355
	// (get) Token: 0x06003D9D RID: 15773 RVA: 0x0021F5A2 File Offset: 0x0021D9A2
	// (set) Token: 0x06003D9E RID: 15774 RVA: 0x0021F5AA File Offset: 0x0021D9AA
	public bool isFloating { get; private set; }

	// Token: 0x06003D9F RID: 15775 RVA: 0x0021F5B4 File Offset: 0x0021D9B4
	protected override void OnAwake()
	{
		base.OnAwake();
		this.properties = new LevelPlayerMotor.Properties();
		this.MoveDirection = new Trilean2(0, 0);
		this.LookDirection = new Trilean2(1, 0);
		this.TrueLookDirection = new Trilean2(1, 0);
		this.velocityManager = new LevelPlayerMotor.VelocityManager(this, this.properties.maxSpeedY, this.properties.yEase);
		this.jumpManager = new LevelPlayerMotor.JumpManager();
		this.dashManager = new LevelPlayerMotor.DashManager();
		this.parryManager = new LevelPlayerMotor.ParryManager();
		this.directionManager = new LevelPlayerMotor.DirectionManager();
		this.platformManager = new LevelPlayerMotor.PlatformManager(this);
		this.hitManager = new LevelPlayerMotor.HitManager();
		this.superManager = new LevelPlayerMotor.SuperManager();
		this.boundsManager = new LevelPlayerMotor.BoundsManager(this);
		base.player.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.allowInput = true;
		this.allowFalling = true;
		this.allowJumping = true;
		this.forceLaunchUp = false;
	}

	// Token: 0x06003DA0 RID: 15776 RVA: 0x0021F6B0 File Offset: 0x0021DAB0
	private void Start()
	{
		base.player.weaponManager.OnExStart += this.StartEx;
		base.player.weaponManager.OnSuperStart += this.StartSuper;
		base.player.weaponManager.OnExFire += this.OnExFired;
		base.player.weaponManager.OnSuperEnd += this.OnSuperEnd;
		base.player.weaponManager.OnExEnd += this.ResetSuperAndEx;
		base.player.weaponManager.OnSuperEnd += this.ResetSuperAndEx;
		base.player.OnReviveEvent += this.OnRevive;
		this.parryController = base.player.GetComponent<LevelPlayerParryController>();
		this.jumpPower = (base.player.stats.isChalice ? this.properties.chaliceFirstJumpPower : this.properties.jumpPower);
	}

	// Token: 0x06003DA1 RID: 15777 RVA: 0x0021F7C4 File Offset: 0x0021DBC4
	private void FixedUpdate()
	{
		if (base.player.IsDead)
		{
			return;
		}
		this.HandleLooking();
		if (base.player.weaponManager.FreezePosition)
		{
			return;
		}
		this.HandleInput();
		if (this.allowFalling)
		{
			this.HandleFalling();
		}
		if (!this.Grounded)
		{
			this.jumpManager.timeInAir += CupheadTime.FixedDelta;
			if (this.jumpManager.state == LevelPlayerMotor.JumpManager.State.Ready && this.jumpManager.timeInAir > 0.0834f)
			{
				this.jumpManager.state = LevelPlayerMotor.JumpManager.State.Used;
			}
		}
		this.Move();
		this.HandleRaycasts();
		Vector2 a = base.transform.localPosition;
		Vector2 v = a - ((!this.platformManager.OnPlatform && base.player.stats.isChalice) ? this.lastPosition : this.lastPositionFixed);
		v.x = (float)((int)v.x);
		v.y = (float)((int)v.y);
		this.MoveDirection = v;
		this.lastPositionFixed = new Vector2(a.x, a.y);
		this.lastPosition = base.transform.position;
		this.ClampToBounds();
	}

	// Token: 0x06003DA2 RID: 15778 RVA: 0x0021F924 File Offset: 0x0021DD24
	public void DisableInput()
	{
		this.allowInput = false;
		this.Locked = false;
		this.MoveDirection = new Trilean2(0, 0);
		this.velocityManager.move = 0f;
		this.velocityManager.dash = 0f;
		this.velocityManager.verticalDash = 0f;
	}

	// Token: 0x06003DA3 RID: 15779 RVA: 0x0021F97C File Offset: 0x0021DD7C
	public void EnableInput()
	{
		this.allowInput = true;
	}

	// Token: 0x06003DA4 RID: 15780 RVA: 0x0021F985 File Offset: 0x0021DD85
	public void DisableJump()
	{
		this.allowJumping = false;
	}

	// Token: 0x06003DA5 RID: 15781 RVA: 0x0021F98E File Offset: 0x0021DD8E
	public void EnableJump()
	{
		this.allowJumping = true;
	}

	// Token: 0x06003DA6 RID: 15782 RVA: 0x0021F998 File Offset: 0x0021DD98
	public void DisableGravity()
	{
		this.allowFalling = false;
		this.MoveDirection = new Trilean2(this.MoveDirection.x, 0);
		this.velocityManager.y = 0f;
	}

	// Token: 0x06003DA7 RID: 15783 RVA: 0x0021F9DB File Offset: 0x0021DDDB
	public void EnableGravity()
	{
		this.allowFalling = true;
		this.velocityManager.y = 0f;
	}

	// Token: 0x06003DA8 RID: 15784 RVA: 0x0021F9F4 File Offset: 0x0021DDF4
	public void SetGravityReversed(bool reversed)
	{
		if (reversed != this.GravityReversed)
		{
			this.GravityReversed = reversed;
			base.player.animationController.OnGravityReversed();
			base.transform.AddPosition(0f, -(base.player.center.y - base.transform.position.y) * (float)((!base.player.stats.isChalice) ? 2 : 4), 0f);
			this.reversingGravity = true;
		}
	}

	// Token: 0x06003DA9 RID: 15785 RVA: 0x0021FA86 File Offset: 0x0021DE86
	private RaycastHit2D BoxCast(Vector2 size, Vector2 direction, int layerMask)
	{
		return this.BoxCast(size, direction, layerMask, Vector2.zero);
	}

	// Token: 0x06003DAA RID: 15786 RVA: 0x0021FA96 File Offset: 0x0021DE96
	private RaycastHit2D BoxCast(Vector2 size, Vector2 direction, int layerMask, Vector2 offset)
	{
		return Physics2D.BoxCast(base.player.colliderManager.DefaultCenter + offset, size, 0f, direction, 2000f, layerMask);
	}

	// Token: 0x06003DAB RID: 15787 RVA: 0x0021FAC1 File Offset: 0x0021DEC1
	private RaycastHit2D CircleCast(float radius, Vector2 direction, int layerMask)
	{
		return Physics2D.CircleCast(base.player.colliderManager.DefaultCenter, radius, direction, 2000f, layerMask);
	}

	// Token: 0x06003DAC RID: 15788 RVA: 0x0021FAE0 File Offset: 0x0021DEE0
	private bool DoesRaycastHitHaveCollider(RaycastHit2D hit)
	{
		return hit.collider != null;
	}

	// Token: 0x06003DAD RID: 15789 RVA: 0x0021FAF0 File Offset: 0x0021DEF0
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (Application.isPlaying)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(base.player.center, 5f);
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(base.transform.position, 5f);
		}
	}

	// Token: 0x06003DAE RID: 15790 RVA: 0x0021FB4C File Offset: 0x0021DF4C
	private void HandleRaycasts()
	{
		bool flag = true;
		if (this.directionManager != null && this.directionManager.up != null)
		{
			flag = this.directionManager.up.able;
		}
		LevelPlayerColliderManager colliderManager = base.player.colliderManager;
		this.directionManager.Reset();
		RaycastHit2D raycastHit = this.BoxCast(new Vector2(1f, (!flag && base.player.stats.isChalice) ? 1f : colliderManager.DefaultHeight), Vector2.left, this.wallMask);
		RaycastHit2D raycastHit2 = this.BoxCast(new Vector2(1f, (!flag && base.player.stats.isChalice) ? 1f : colliderManager.DefaultHeight), Vector2.right, this.wallMask);
		RaycastHit2D raycastHit3 = this.BoxCast(new Vector2(colliderManager.DefaultWidth, 1f), (!this.GravityReversed) ? Vector2.up : Vector2.down, (!this.GravityReversed) ? this.ceilingMask : this.groundMask);
		this.RaycastObstacle(this.directionManager.left, raycastHit, colliderManager.DefaultWidth / 2f, LevelPlayerMotor.RaycastAxis.X);
		this.RaycastObstacle(this.directionManager.right, raycastHit2, colliderManager.DefaultWidth / 2f, LevelPlayerMotor.RaycastAxis.X);
		this.RaycastObstacle(this.directionManager.up, raycastHit3, colliderManager.DefaultHeight / 2f, LevelPlayerMotor.RaycastAxis.Y);
		Vector2 vector = colliderManager.DefaultCenter + new Vector2(0f, colliderManager.DefaultHeight * this.GravityReversalMultiplier);
		int num = Physics2D.BoxCastNonAlloc(vector, new Vector2(colliderManager.DefaultWidth, 1f), 0f, (!this.GravityReversed) ? Vector2.down : Vector2.up, this.hitBuffer, 1000f, (!this.GravityReversed) ? this.groundMask : this.ceilingMask);
		this.directionManager.down.pos = new Vector2(colliderManager.DefaultCenter.x, -10000f * this.GravityReversalMultiplier);
		for (int i = 0; i < num; i++)
		{
			RaycastHit2D raycastHit2D = this.hitBuffer[i];
			if ((!this.GravityReversed) ? (raycastHit2D.point.y > this.directionManager.down.pos.y) : (raycastHit2D.point.y < this.directionManager.down.pos.y))
			{
				if (!((!this.GravityReversed) ? (raycastHit2D.point.y > 20f + base.transform.position.y) : (raycastHit2D.point.y < -20f + base.transform.position.y)))
				{
					float num2 = Math.Abs(base.transform.position.y - raycastHit2D.point.y);
					this.directionManager.down.pos = new Vector2(vector.x, raycastHit2D.point.y);
					this.directionManager.down.gameObject = raycastHit2D.collider.gameObject;
					this.directionManager.down.distance = num2;
					if (num2 < 20f)
					{
						this.directionManager.down.able = false;
					}
					global::Debug.DrawLine(vector, this.directionManager.down.pos, Color.red);
				}
			}
		}
		if (!this.Grounded)
		{
			if (!this.directionManager.down.able)
			{
				this.OnGrounded();
				this.directionManager.left.able = true;
				this.directionManager.right.able = true;
			}
			if (!this.directionManager.up.able && (this.reversingGravity || this.directionManager.up.able != flag))
			{
				GameObject gameObject = this.directionManager.up.gameObject;
				LevelPlatform levelPlatform = (!(gameObject == null)) ? gameObject.GetComponent<LevelPlatform>() : null;
				if (!this.GravityReversed || levelPlatform == null || !levelPlatform.canFallThrough)
				{
					this.OnHitCeiling();
				}
			}
		}
		float num3 = Mathf.Abs(base.transform.position.y - this.directionManager.down.pos.y);
		if (this.Grounded && num3 > 30f)
		{
			this.LeaveGround(true);
		}
	}

	// Token: 0x06003DAF RID: 15791 RVA: 0x0022007C File Offset: 0x0021E47C
	private float RaycastObstacle(LevelPlayerMotor.DirectionManager.Hit directionProperties, RaycastHit2D raycastHit, float maxDistance, LevelPlayerMotor.RaycastAxis axis)
	{
		if (!this.DoesRaycastHitHaveCollider(raycastHit))
		{
			return 1000f;
		}
		float num = (axis != LevelPlayerMotor.RaycastAxis.X) ? Math.Abs(base.player.colliderManager.DefaultCenter.y - raycastHit.point.y) : Math.Abs(base.player.colliderManager.DefaultCenter.x - raycastHit.point.x);
		directionProperties.pos = raycastHit.point;
		directionProperties.gameObject = raycastHit.collider.gameObject;
		directionProperties.distance = num;
		if (num < maxDistance)
		{
			directionProperties.able = false;
		}
		return num;
	}

	// Token: 0x06003DB0 RID: 15792 RVA: 0x00220138 File Offset: 0x0021E538
	private void OnGrounded()
	{
		if (this.Grounded || !this.jumpManager.ableToLand)
		{
			return;
		}
		if (this.platformManager.IsPlatformIgnored(this.directionManager.down.gameObject.transform))
		{
			return;
		}
		LevelPlatform component = this.directionManager.down.gameObject.GetComponent<LevelPlatform>();
		if (component != null)
		{
			if (component.canFallThrough && this.jumpManager.timeSinceDownJump < 0.1f)
			{
				return;
			}
			component.AddChild(base.transform);
		}
		if (this.jumpManager.doubleJumped)
		{
			this.jumpManager.doubleJumped = false;
		}
		this.jumpManager.state = LevelPlayerMotor.JumpManager.State.Ready;
		this.parryManager.state = LevelPlayerMotor.ParryManager.State.Ready;
		this.velocityManager.y = 0f;
		this.platformManager.ResetAll();
		this.Grounded = true;
		this.Parrying = false;
		this.reversingGravity = false;
		this.dashManager.timeSinceGroundDash = 1000f;
		if (base.player.stats.isChalice)
		{
			this.ChaliceDoubleJumped = false;
		}
		if (this.jumpManager.timeInAir > this.jumpManager.longestTimeInAir)
		{
			this.jumpManager.longestTimeInAir = this.jumpManager.timeInAir;
			OnlineManager.Instance.Interface.SetStat(base.player.id, "HangTime", this.jumpManager.timeInAir);
		}
		if (this.OnGroundedEvent != null)
		{
			this.OnGroundedEvent();
		}
	}

	// Token: 0x06003DB1 RID: 15793 RVA: 0x002202D8 File Offset: 0x0021E6D8
	private void LeaveGround(bool allowLateJump = false)
	{
		if (!this.Dashing && base.player.stats.Loadout.charm == Charm.charm_parry_plus && !Level.IsChessBoss)
		{
			this.ForceParry();
		}
		if (this.Grounded)
		{
			this.Grounded = false;
			this.jumpManager.ableToLand = false;
			this.jumpManager.timeInAir = 0f;
			base.player.stats.ResetJumpParries();
			this.ResetSuperAndEx();
			base.player.weaponManager.ResetEx();
		}
		this.velocityManager.y = 0f;
		this.ClearParent();
		if (this.jumpManager.state == LevelPlayerMotor.JumpManager.State.Ready && !allowLateJump)
		{
			this.jumpManager.state = LevelPlayerMotor.JumpManager.State.Used;
		}
	}

	// Token: 0x06003DB2 RID: 15794 RVA: 0x002203AC File Offset: 0x0021E7AC
	private void OnHitCeiling()
	{
		if (this.jumpManager.ableToLand)
		{
			return;
		}
		this.velocityManager.y = 0f;
		this.directionManager.left.able = true;
		this.directionManager.right.able = true;
	}

	// Token: 0x06003DB3 RID: 15795 RVA: 0x002203FC File Offset: 0x0021E7FC
	public IEnumerator MoveToX_cr(float x, int endingLookDirection = 1)
	{
		if (base.transform.position.x == x)
		{
			yield break;
		}
		float walk = 0f;
		this.MoveDirection = new Trilean2(0, 0);
		this.LookDirection = new Trilean2(1, 0);
		bool left = base.transform.position.x < x;
		if (!left)
		{
			this.LookDirection = new Trilean2(-1, 0);
		}
		while ((!left) ? (base.transform.position.x > x) : (base.transform.position.x < x))
		{
			if ((this.LookDirection.y >= 0 || !this.Grounded) && !this.Locked)
			{
				walk = (float)((!left) ? -1 : 1) * this.properties.moveSpeed;
			}
			this.velocityManager.move = walk;
			yield return null;
		}
		walk = 0f;
		this.velocityManager.move = walk;
		this.MoveDirection = new Trilean2(0, 0);
		this.LookDirection = new Trilean2(endingLookDirection, 0);
		yield return null;
		this.LookDirection = new Trilean2(0, 0);
		yield break;
	}

	// Token: 0x06003DB4 RID: 15796 RVA: 0x00220428 File Offset: 0x0021E828
	private void Move()
	{
		this.velocityManager.Calculate();
		Vector3 a = this.velocityManager.Total;
		if (this.hitManager.state != LevelPlayerMotor.HitManager.State.Hit && this.superManager.state == LevelPlayerMotor.SuperManager.State.Ready)
		{
			if (!this.velocityManager.yAxisForce)
			{
				this.forceLaunchUp = false;
				if (this.Grounded)
				{
					a.x += this.velocityManager.GroundForce;
				}
				else
				{
					a.x += this.velocityManager.AirForce;
				}
			}
			else if (this.Grounded)
			{
				if (!this.forceLaunchUp)
				{
					this.LeaveGround(false);
					this.velocityManager.y = this.properties.jumpPower * 2f;
					this.DisableGravity();
					this.forceLaunchUp = true;
				}
			}
			else
			{
				a.y += this.velocityManager.AirForce;
				base.FrameDelayedCallback(new Action(this.EnableGravity), 1);
			}
		}
		if (a.x > 0f && !this.directionManager.right.able)
		{
			a.x = 0f;
		}
		if (a.x < 0f && !this.directionManager.left.able)
		{
			a.x = 0f;
		}
		if (this.platformManager.OnPlatform)
		{
			if (!this.directionManager.right.able && this.MoveDirection.x > 0)
			{
				a.x = 0f;
				base.transform.SetPosition(new float?(this.lastPosition.x), null, null);
			}
			if (!this.directionManager.left.able && this.MoveDirection.x < 0)
			{
				a.x = 0f;
				base.transform.SetPosition(new float?(this.lastPosition.x), null, null);
			}
		}
		if (this.GravityReversed)
		{
			a.y *= -1f;
		}
		base.transform.localPosition += a * CupheadTime.FixedDelta;
		if (this.Grounded)
		{
			Vector2 v = base.transform.position;
			v.y = this.directionManager.down.pos.y;
			base.transform.position = v;
			LevelPlatform levelPlatform = null;
			if (this.directionManager.down.gameObject != null)
			{
				levelPlatform = this.directionManager.down.gameObject.GetComponent<LevelPlatform>();
			}
			if (levelPlatform == null && base.transform.parent != null)
			{
				this.ClearParent();
			}
			else if (levelPlatform != null && (base.transform.parent == null || levelPlatform.gameObject != base.transform.parent.gameObject))
			{
				this.ClearParent();
				levelPlatform.AddChild(base.transform);
			}
		}
	}

	// Token: 0x06003DB5 RID: 15797 RVA: 0x002207D0 File Offset: 0x0021EBD0
	private void ClampToBounds()
	{
		float num = base.player.colliderManager.Width / 2f;
		float num2 = this.directionManager.left.pos.x + ((!this.reversingGravity) ? num : (-num));
		float num3 = this.directionManager.right.pos.x - ((!this.reversingGravity) ? num : (-num));
		float num4 = this.directionManager.up.pos.y - ((!this.GravityReversed) ? this.boundsManager.TopY : this.boundsManager.BottomY);
		float num5 = this.directionManager.down.pos.y - ((!this.GravityReversed) ? this.boundsManager.BottomY : this.boundsManager.TopY);
		GameObject gameObject = this.directionManager.up.gameObject;
		LevelPlatform levelPlatform = (!(gameObject == null)) ? gameObject.GetComponent<LevelPlatform>() : null;
		bool flag = !this.GravityReversed || levelPlatform == null || !levelPlatform.canFallThrough;
		Vector3 position = base.transform.position;
		if (!this.directionManager.left.able && base.transform.position.x < num2)
		{
			position.x = num2;
		}
		if (!this.directionManager.right.able && base.transform.position.x > num3)
		{
			position.x = num3;
		}
		if (!this.directionManager.up.able && flag && ((!this.GravityReversed) ? (base.transform.position.y > num4) : (base.transform.position.y < num4)))
		{
			position.y = num4;
		}
		position.x = Mathf.Clamp(position.x, (float)Level.Current.Left + num, (float)Level.Current.Right - num);
		base.transform.position = position;
	}

	// Token: 0x06003DB6 RID: 15798 RVA: 0x00220A34 File Offset: 0x0021EE34
	private void ResetSuperAndEx()
	{
		if (this.superManager.state == LevelPlayerMotor.SuperManager.State.Ready)
		{
			return;
		}
		if (this.jumpManager.state != LevelPlayerMotor.JumpManager.State.Ready)
		{
			this.jumpManager.state = LevelPlayerMotor.JumpManager.State.Used;
		}
		base.StopCoroutine(this.exMove_cr());
		this.superManager.state = LevelPlayerMotor.SuperManager.State.Ready;
		this.EnableInput();
		this.EnableGravity();
	}

	// Token: 0x06003DB7 RID: 15799 RVA: 0x00220A92 File Offset: 0x0021EE92
	public void StartSuper()
	{
		this.LeaveGround(false);
		this.jumpManager.state = LevelPlayerMotor.JumpManager.State.Used;
		this.jumpManager.timer = 0f;
		this.velocityManager.y = 0f;
	}

	// Token: 0x06003DB8 RID: 15800 RVA: 0x00220AC7 File Offset: 0x0021EEC7
	public void OnSuperEnd()
	{
		if (this.Grounded)
		{
			this.jumpManager.state = LevelPlayerMotor.JumpManager.State.Ready;
		}
		else
		{
			this.DoPostSuperHop();
		}
	}

	// Token: 0x06003DB9 RID: 15801 RVA: 0x00220AEC File Offset: 0x0021EEEC
	public void DoPostSuperHop()
	{
		this.LeaveGround(false);
		this.velocityManager.y = ((base.player.stats.Loadout.super != Super.level_super_invincible) ? this.properties.superKnockUp : this.properties.superInvincibleKnockUp);
	}

	// Token: 0x06003DBA RID: 15802 RVA: 0x00220B45 File Offset: 0x0021EF45
	public void CheckForPostSuperHop()
	{
		this.HandleRaycasts();
		if (!this.Grounded)
		{
			this.DoPostSuperHop();
			base.player.animator.Play("Jump_Launch");
		}
	}

	// Token: 0x06003DBB RID: 15803 RVA: 0x00220B73 File Offset: 0x0021EF73
	private void StartEx()
	{
		this.exFirePose = base.player.weaponManager.GetDirectionPose();
		this.DisableInput();
		this.DisableGravity();
		this.superManager.state = LevelPlayerMotor.SuperManager.State.Ex;
	}

	// Token: 0x06003DBC RID: 15804 RVA: 0x00220BA3 File Offset: 0x0021EFA3
	private void OnExFired()
	{
		if (this.exFirePose == LevelPlayerWeaponManager.Pose.Up || this.exFirePose == LevelPlayerWeaponManager.Pose.Down)
		{
			base.StartCoroutine(this.exDelay_cr());
		}
		else
		{
			base.StartCoroutine(this.exMove_cr());
		}
	}

	// Token: 0x06003DBD RID: 15805 RVA: 0x00220BDC File Offset: 0x0021EFDC
	private IEnumerator exDelay_cr()
	{
		while (this.superManager.state != LevelPlayerMotor.SuperManager.State.Ready)
		{
			yield return null;
		}
		this.EnableInput();
		this.EnableGravity();
		this.superManager.state = LevelPlayerMotor.SuperManager.State.Ready;
		yield break;
	}

	// Token: 0x06003DBE RID: 15806 RVA: 0x00220BF8 File Offset: 0x0021EFF8
	private IEnumerator exMove_cr()
	{
		while (this.superManager.state != LevelPlayerMotor.SuperManager.State.Ready)
		{
			this.velocityManager.move = (float)(this.TrueLookDirection.x * -1) * this.properties.exKnockback;
			yield return null;
		}
		this.EnableInput();
		this.EnableGravity();
		this.superManager.state = LevelPlayerMotor.SuperManager.State.Ready;
		yield break;
	}

	// Token: 0x06003DBF RID: 15807 RVA: 0x00220C14 File Offset: 0x0021F014
	private void HandleInput()
	{
		if (!base.player.levelStarted)
		{
			return;
		}
		this.timeSinceInputBuffered += CupheadTime.FixedDelta;
		this.dashManager.timeSinceGroundDash += CupheadTime.FixedDelta;
		if ((!this.allowInput || this.dashManager.IsDashing) && this.hitManager.state == LevelPlayerMotor.HitManager.State.Inactive)
		{
			this.BufferInputs();
		}
		if (!this.allowInput)
		{
			return;
		}
		if (!this.HandleDash())
		{
			if (this.hitManager.state == LevelPlayerMotor.HitManager.State.Hit)
			{
				this.HandleHit();
			}
			else
			{
				if (this.hitManager.state != LevelPlayerMotor.HitManager.State.KnockedUp)
				{
					this.HandleParry();
					this.HandleJumping();
					this.HandleLocked();
				}
				else
				{
					this.HandlePitKnockUp();
				}
				this.HandleWalking();
			}
		}
	}

	// Token: 0x06003DC0 RID: 15808 RVA: 0x00220CF5 File Offset: 0x0021F0F5
	private void BufferInput(LevelPlayerMotor.BufferedInput input)
	{
		this.bufferedInput = input;
		this.timeSinceInputBuffered = 0f;
	}

	// Token: 0x06003DC1 RID: 15809 RVA: 0x00220D0C File Offset: 0x0021F10C
	public void BufferInputs()
	{
		if (base.player.input.actions.GetButtonDown(2))
		{
			this.BufferInput(LevelPlayerMotor.BufferedInput.Jump);
		}
		else if (base.player.input.actions.GetButtonDown(7) && !this.dashManager.IsDashing)
		{
			this.BufferInput(LevelPlayerMotor.BufferedInput.Dash);
		}
		else if (base.player.input.actions.GetButtonDown(4))
		{
			this.BufferInput(LevelPlayerMotor.BufferedInput.Super);
		}
	}

	// Token: 0x06003DC2 RID: 15810 RVA: 0x00220D99 File Offset: 0x0021F199
	public void ClearBufferedInput()
	{
		this.timeSinceInputBuffered = 0.134f;
	}

	// Token: 0x06003DC3 RID: 15811 RVA: 0x00220DA6 File Offset: 0x0021F1A6
	public bool HasBufferedInput(LevelPlayerMotor.BufferedInput input)
	{
		return this.bufferedInput == input && this.timeSinceInputBuffered < 0.134f;
	}

	// Token: 0x06003DC4 RID: 15812 RVA: 0x00220DC4 File Offset: 0x0021F1C4
	private void HandleJumping()
	{
		if (this.allowJumping)
		{
			if (this.jumpManager.state == LevelPlayerMotor.JumpManager.State.Ready && (base.player.input.actions.GetButtonDown(2) || this.HasBufferedInput(LevelPlayerMotor.BufferedInput.Jump)))
			{
				this.hardExitParry = false;
				this.ClearBufferedInput();
				if (((base.player.stats.ReverseTime > 0f) ? (this.LookDirection.y > 0) : (this.LookDirection.y < 0)) && this.Grounded && base.transform.parent != null)
				{
					LevelPlatform component = base.transform.parent.GetComponent<LevelPlatform>();
					if (component.canFallThrough)
					{
						this.platformManager.Ignore(base.transform.parent);
						this.jumpManager.state = LevelPlayerMotor.JumpManager.State.Used;
						this.LeaveGround(false);
						this.jumpManager.timeSinceDownJump = 0f;
						return;
					}
				}
				AudioManager.Play("player_jump");
				this.jumpManager.state = LevelPlayerMotor.JumpManager.State.Hold;
				this.LeaveGround(false);
				this.velocityManager.y = this.jumpPower;
				this.jumpManager.timer = CupheadTime.FixedDelta;
				if (this.OnJumpEvent != null)
				{
					this.OnJumpEvent();
				}
			}
			if (this.jumpManager.state == LevelPlayerMotor.JumpManager.State.Hold)
			{
				if (!this.directionManager.up.able || (this.jumpManager.timer >= this.properties.jumpHoldMin && (base.player.input.actions.GetButtonUp(2) || !base.player.input.actions.GetButton(2))) || this.jumpManager.timer >= this.properties.jumpHoldMax)
				{
					this.jumpManager.state = LevelPlayerMotor.JumpManager.State.Used;
					this.jumpManager.timer = 0f;
				}
				if (base.player.stats.isChalice)
				{
					this.velocityManager.y = ((!this.jumpManager.doubleJumped) ? this.properties.chaliceFirstJumpPower : this.properties.chaliceSecondJumpPower);
				}
				else
				{
					this.velocityManager.y = this.jumpPower;
				}
				this.jumpManager.timer += CupheadTime.FixedDelta;
			}
			this.jumpManager.timeSinceDownJump += CupheadTime.FixedDelta;
			if (base.player.stats.isChalice && !this.jumpManager.doubleJumped)
			{
				this.ChaliceDoubleJump();
			}
		}
	}

	// Token: 0x06003DC5 RID: 15813 RVA: 0x002210A3 File Offset: 0x0021F4A3
	public void OnChaliceRevive()
	{
		this.ChaliceDoubleJumped = true;
	}

	// Token: 0x06003DC6 RID: 15814 RVA: 0x002210AC File Offset: 0x0021F4AC
	private void ChaliceDoubleJump()
	{
		if ((base.player.input.actions.GetButtonDown(2) || this.HasBufferedInput(LevelPlayerMotor.BufferedInput.Jump)) && this.jumpManager.state == LevelPlayerMotor.JumpManager.State.Used && !this.IsHit)
		{
			this.hardExitParry = false;
			this.ClearBufferedInput();
			if (this.dashManager.state == LevelPlayerMotor.DashManager.State.End && this.parryManager.state == LevelPlayerMotor.ParryManager.State.Ready)
			{
				this.dashManager.state = LevelPlayerMotor.DashManager.State.Ready;
			}
			AudioManager.Play("chalice_doublejump");
			this.jumpManager.state = LevelPlayerMotor.JumpManager.State.Hold;
			this.LeaveGround(false);
			this.jumpManager.doubleJumped = true;
			this.velocityManager.y = this.properties.chaliceSecondJumpPower;
			this.jumpManager.timer = CupheadTime.FixedDelta;
			this.ChaliceDoubleJumped = true;
			this.platformManager.ResetAll();
			if (this.OnJumpEvent != null)
			{
				this.OnJumpEvent();
			}
			if (this.OnDoubleJumpEvent != null)
			{
				this.OnDoubleJumpEvent();
			}
		}
	}

	// Token: 0x06003DC7 RID: 15815 RVA: 0x002211C4 File Offset: 0x0021F5C4
	private void HandleParry()
	{
		if (base.player.stats.isChalice)
		{
			return;
		}
		if (this.IsHit)
		{
			return;
		}
		if (this.parryManager.state == LevelPlayerMotor.ParryManager.State.Ready && (base.player.input.actions.GetButtonDown(2) || this.HasBufferedInput(LevelPlayerMotor.BufferedInput.Jump)) && this.jumpManager.state != LevelPlayerMotor.JumpManager.State.Ready && !this.IsHit)
		{
			this.ClearBufferedInput();
			this.hitManager.state = LevelPlayerMotor.HitManager.State.Inactive;
			this.parryManager.state = LevelPlayerMotor.ParryManager.State.NotReady;
			if (this.dashManager.IsDashing)
			{
				this.dashManager.state = LevelPlayerMotor.DashManager.State.End;
			}
			this.Parrying = true;
			if (this.OnParryEvent != null)
			{
				this.OnParryEvent();
			}
		}
	}

	// Token: 0x06003DC8 RID: 15816 RVA: 0x0022129C File Offset: 0x0021F69C
	public void OnParryComplete()
	{
		if (base.player.stats.Loadout.charm == Charm.charm_parry_plus && !Level.IsChessBoss)
		{
			this.hardExitParry = true;
		}
		this.LeaveGround(false);
		this.parryManager.state = LevelPlayerMotor.ParryManager.State.Ready;
		this.velocityManager.y = ((!this.parryController.HasHitEnemy) ? this.properties.parryPower : this.properties.parryAttackBounce);
		if (this.OnParrySuccess != null)
		{
			this.OnParrySuccess();
		}
		if (base.player.stats.isChalice)
		{
			this.dashManager.chaliceParryCoolDown = true;
			this.DashComplete();
		}
		this.platformManager.ResetAll();
	}

	// Token: 0x06003DC9 RID: 15817 RVA: 0x0022136A File Offset: 0x0021F76A
	public void OnParryHit()
	{
		base.StartCoroutine(this.parryHit_cr());
	}

	// Token: 0x06003DCA RID: 15818 RVA: 0x00221379 File Offset: 0x0021F779
	public void OnParryCanceled()
	{
		this.Parrying = false;
	}

	// Token: 0x06003DCB RID: 15819 RVA: 0x00221382 File Offset: 0x0021F782
	public void OnParryAnimEnd()
	{
		this.Parrying = false;
	}

	// Token: 0x06003DCC RID: 15820 RVA: 0x0022138C File Offset: 0x0021F78C
	private bool HandleDash()
	{
		if (this.dashManager.state == LevelPlayerMotor.DashManager.State.Ready && (!this.Grounded || this.dashManager.timeSinceGroundDash > 0.1f) && (base.player.input.actions.GetButtonDown(7) || this.HasBufferedInput(LevelPlayerMotor.BufferedInput.Dash)))
		{
			this.ClearBufferedInput();
			AudioManager.Play("player_dash");
			this.dashManager.state = LevelPlayerMotor.DashManager.State.Start;
			this.dashManager.direction = this.TrueLookDirection.x;
			this.dashManager.groundDash = this.Grounded;
			this.ChaliceDuckDashed = (base.player.stats.isChalice && this.Ducking);
			if (this.jumpManager.state == LevelPlayerMotor.JumpManager.State.Hold)
			{
				this.jumpManager.state = LevelPlayerMotor.JumpManager.State.Used;
			}
			if (this.OnDashStartEvent != null)
			{
				this.OnDashStartEvent();
			}
			this.velocityManager.move = 0f;
			return true;
		}
		if (this.dashManager.state == LevelPlayerMotor.DashManager.State.Start)
		{
			this.dashManager.state = LevelPlayerMotor.DashManager.State.Dashing;
		}
		if (base.player.stats.isChalice && !this.ChaliceDuckDashed)
		{
			this.ChaliceDashParry();
		}
		if (this.dashManager.state == LevelPlayerMotor.DashManager.State.Dashing)
		{
			this.velocityManager.dash = this.properties.dashSpeed * (float)this.dashManager.direction;
			this.dashManager.timer += CupheadTime.FixedDelta;
			this.velocityManager.y = 0f;
			if (this.dashManager.timer >= this.properties.dashTime)
			{
				this.DashComplete();
			}
			if (!this.Grounded)
			{
				this.jumpManager.ableToLand = true;
			}
			return true;
		}
		if (this.dashManager.state == LevelPlayerMotor.DashManager.State.End)
		{
			if (this.Grounded)
			{
				this.dashManager.state = LevelPlayerMotor.DashManager.State.Ready;
				if (this.dashManager.groundDash)
				{
					this.dashManager.timeSinceGroundDash = 0f;
				}
				if (base.player.stats.isChalice)
				{
					this.dashManager.chaliceParryCoolDown = false;
					this.dashManager.chaliceParryCoolDownTimer = 0f;
				}
			}
			else
			{
				this.dashManager.groundDash = false;
			}
			this.ChaliceDuckDashed = false;
			if (base.player.stats.isChalice && !this.dashManager.chaliceParryCoolDown)
			{
				this.dashManager.state = LevelPlayerMotor.DashManager.State.Ready;
			}
			if (base.player.stats.isChalice)
			{
				this.ChaliceDashCooldownCheck();
			}
		}
		return false;
	}

	// Token: 0x06003DCD RID: 15821 RVA: 0x0022165C File Offset: 0x0021FA5C
	public void DashComplete()
	{
		if (base.player.stats.Loadout.charm == Charm.charm_parry_plus && !Level.IsChessBoss)
		{
			this.ForceParry();
		}
		this.dashManager.state = LevelPlayerMotor.DashManager.State.End;
		this.dashManager.timer = 0f;
		this.velocityManager.dash = 0f;
		this.velocityManager.verticalDash = 0f;
		if (this.OnDashEndEvent != null)
		{
			this.OnDashEndEvent();
		}
	}

	// Token: 0x06003DCE RID: 15822 RVA: 0x002216EC File Offset: 0x0021FAEC
	private void ForceParry()
	{
		if (this.hitManager.state != LevelPlayerMotor.HitManager.State.Hit && !this.hardExitParry)
		{
			this.hitManager.state = LevelPlayerMotor.HitManager.State.Inactive;
			this.parryManager.state = LevelPlayerMotor.ParryManager.State.NotReady;
			this.Parrying = true;
			if (this.OnParryEvent != null)
			{
				this.OnParryEvent();
			}
		}
	}

	// Token: 0x06003DCF RID: 15823 RVA: 0x0022174C File Offset: 0x0021FB4C
	private void ChaliceDashParry()
	{
		if (this.dashManager.IsDashing && !this.dashManager.chaliceParryCoolDown && this.hitManager.state != LevelPlayerMotor.HitManager.State.Hit && !this.hardExitParry)
		{
			this.hitManager.state = LevelPlayerMotor.HitManager.State.Inactive;
			this.parryManager.state = LevelPlayerMotor.ParryManager.State.NotReady;
			this.Parrying = true;
			if (this.OnParryEvent != null)
			{
				this.OnParryEvent();
			}
			this.dashManager.chaliceParryCoolDown = true;
		}
	}

	// Token: 0x06003DD0 RID: 15824 RVA: 0x002217D6 File Offset: 0x0021FBD6
	public void ResetChaliceDoubleJump()
	{
		this.jumpManager.doubleJumped = false;
		if (base.player.stats.isChalice)
		{
			this.dashManager.chaliceParryCoolDown = false;
			this.dashManager.chaliceParryCoolDownTimer = 0f;
		}
	}

	// Token: 0x06003DD1 RID: 15825 RVA: 0x00221818 File Offset: 0x0021FC18
	private void ChaliceDashCooldownCheck()
	{
		if (this.dashManager.chaliceParryCoolDown)
		{
			this.dashManager.chaliceParryCoolDownTimer += CupheadTime.FixedDelta;
			if (this.dashManager.chaliceParryCoolDownTimer >= this.properties.dashParryCooldownTime)
			{
				this.dashManager.chaliceParryCoolDown = false;
				this.dashManager.chaliceParryCoolDownTimer = 0f;
			}
		}
	}

	// Token: 0x06003DD2 RID: 15826 RVA: 0x00221883 File Offset: 0x0021FC83
	public float DistanceToGround()
	{
		this.HandleRaycasts();
		return this.directionManager.down.distance;
	}

	// Token: 0x06003DD3 RID: 15827 RVA: 0x0022189B File Offset: 0x0021FC9B
	private void HandleLocked()
	{
		if (base.player.input.actions.GetButton(6) && this.Grounded)
		{
			this.Locked = true;
		}
		else
		{
			this.Locked = false;
		}
	}

	// Token: 0x06003DD4 RID: 15828 RVA: 0x002218D8 File Offset: 0x0021FCD8
	private void HandleWalking()
	{
		float move = 0f;
		if ((this.LookDirection.y >= 0 || !this.Grounded) && !this.Locked)
		{
			int num = (base.player.stats.ReverseTime > 0f) ? (-base.player.input.GetAxisInt(PlayerInput.Axis.X, false, false)) : base.player.input.GetAxisInt(PlayerInput.Axis.X, false, false);
			move = (float)num * this.properties.moveSpeed;
		}
		this.velocityManager.move = move;
	}

	// Token: 0x06003DD5 RID: 15829 RVA: 0x0022197C File Offset: 0x0021FD7C
	private void HandleLooking()
	{
		if (base.player.levelStarted && this.allowInput)
		{
			int num = base.player.input.GetAxisInt(PlayerInput.Axis.X, false, false);
			num = ((base.player.stats.ReverseTime > 0f) ? (-num) : num);
			int num2 = base.player.input.GetAxisInt(PlayerInput.Axis.Y, true, this.Grounded && !this.Locked && !this.IsUsingSuperOrEx);
			num2 = ((base.player.stats.ReverseTime > 0f) ? (-num2) : num2);
			if (this.GravityReversed)
			{
				num2 *= -1;
			}
			this.LookDirection = new Trilean2(num, num2);
		}
		int x = this.TrueLookDirection.x;
		int y = this.TrueLookDirection.y;
		if (this.LookDirection.x != 0)
		{
			x = this.LookDirection.x;
		}
		y = this.LookDirection.y;
		this.TrueLookDirection = new Trilean2(x, y);
	}

	// Token: 0x06003DD6 RID: 15830 RVA: 0x00221ACB File Offset: 0x0021FECB
	public void ForceLooking(Trilean2 direction)
	{
		this.LookDirection = direction;
		this.TrueLookDirection = direction;
		base.GetComponent<LevelPlayerAnimationController>().ForceDirection();
	}

	// Token: 0x06003DD7 RID: 15831 RVA: 0x00221AE8 File Offset: 0x0021FEE8
	private void HandleFalling()
	{
		if (this.Grounded || this.dashManager.IsDashing)
		{
			this.isFloating = false;
			this.jumpManager.floatTimer = 0f;
			return;
		}
		if (Level.Current.LevelTime < 0.2f)
		{
			return;
		}
		float num = this.properties.timeToMaxY * 60f;
		float num2 = this.properties.maxSpeedY / num * CupheadTime.FixedDelta;
		this.velocityManager.y += num2;
		this.jumpManager.ableToLand = (this.velocityManager.y > 0f);
		if (base.player.stats.Loadout.charm == Charm.charm_float && this.jumpManager.ableToLand && base.player.input.actions.GetButton(2) && this.jumpManager.floatTimer < WeaponProperties.CharmFloat.maxTime)
		{
			this.isFloating = true;
			float value = Mathf.Clamp(this.jumpManager.floatTimer - WeaponProperties.CharmFloat.falloffStartTime, 0f, WeaponProperties.CharmFloat.maxTime - WeaponProperties.CharmFloat.falloffStartTime);
			value = Mathf.InverseLerp(0f, WeaponProperties.CharmFloat.maxTime - WeaponProperties.CharmFloat.falloffStartTime, value);
			this.velocityManager.y = Mathf.Clamp(this.velocityManager.y, 0f, EaseUtils.EaseInSine(WeaponProperties.CharmFloat.minFallSpeed, WeaponProperties.CharmFloat.maxFallSpeed, value));
			this.jumpManager.floatTimer += CupheadTime.FixedDelta;
		}
		else
		{
			this.isFloating = false;
		}
	}

	// Token: 0x06003DD8 RID: 15832 RVA: 0x00221C8C File Offset: 0x0022008C
	public void HandlePitKnockUp()
	{
		if (this.hitManager.state != LevelPlayerMotor.HitManager.State.KnockedUp)
		{
			return;
		}
		if (this.hitManager.timer > this.properties.knockUpStunTime)
		{
			this.hitManager.state = LevelPlayerMotor.HitManager.State.Inactive;
			this.velocityManager.hit = 0f;
		}
		else
		{
			this.hitManager.timer += CupheadTime.FixedDelta;
		}
	}

	// Token: 0x06003DD9 RID: 15833 RVA: 0x00221D00 File Offset: 0x00220100
	private void HandleHit()
	{
		if (this.hitManager.state != LevelPlayerMotor.HitManager.State.Hit)
		{
			return;
		}
		if (this.hitManager.timer > this.properties.hitStunTime)
		{
			this.hitManager.state = LevelPlayerMotor.HitManager.State.Inactive;
			this.velocityManager.hit = 0f;
		}
		else
		{
			float value = this.hitManager.timer / this.properties.hitStunTime;
			this.velocityManager.hit = EaseUtils.Ease(this.properties.hitKnockbackEase, this.properties.hitKnockbackPower, 0f, value) * (float)this.hitManager.direction;
			this.hitManager.timer += CupheadTime.FixedDelta;
		}
	}

	// Token: 0x06003DDA RID: 15834 RVA: 0x00221DC4 File Offset: 0x002201C4
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (base.player.stats.SuperInvincible)
		{
			return;
		}
		this.hitManager.state = LevelPlayerMotor.HitManager.State.Hit;
		if (this.OnHitEvent != null)
		{
			this.OnHitEvent();
		}
		this.DashComplete();
		this.velocityManager.Clear();
		this.ResetSuperAndEx();
		int direction = this.TrueLookDirection.x * -1;
		this.hitManager.direction = direction;
		this.LeaveGround(false);
		this.velocityManager.y = this.properties.hitJumpPower;
		this.hitManager.timer = 0f;
	}

	// Token: 0x06003DDB RID: 15835 RVA: 0x00221E70 File Offset: 0x00220270
	public void OnPitKnockUp(float y, float velocityScale = 1f)
	{
		if (base.player.IsDead)
		{
			base.transform.SetPosition(null, new float?(y + 200f * this.GravityReversalMultiplier), null);
			return;
		}
		if (!base.player.stats.isChalice)
		{
			this.hardExitParry = true;
		}
		base.transform.SetPosition(null, new float?(y), null);
		this.hitManager.state = LevelPlayerMotor.HitManager.State.KnockedUp;
		this.DashComplete();
		this.velocityManager.Clear();
		this.ResetSuperAndEx();
		this.hitManager.direction = 0;
		this.LeaveGround(false);
		if (Level.Current.LevelType == Level.Type.Platforming)
		{
			this.velocityManager.y = this.properties.platformingPitKnockUpPower * velocityScale;
		}
		else
		{
			this.velocityManager.y = this.properties.pitKnockUpPower * velocityScale;
		}
		this.hitManager.timer = 0f;
		this.dashManager.state = LevelPlayerMotor.DashManager.State.Ready;
		this.parryManager.state = LevelPlayerMotor.ParryManager.State.Ready;
	}

	// Token: 0x06003DDC RID: 15836 RVA: 0x00221FA0 File Offset: 0x002203A0
	public void OnTrampolineKnockUp(float y)
	{
		if (base.player.IsDead)
		{
			base.transform.SetPosition(null, new float?(y * this.GravityReversalMultiplier), null);
			return;
		}
		this.LeaveGround(false);
		this.hitManager.state = LevelPlayerMotor.HitManager.State.KnockedUp;
		this.DashComplete();
		this.velocityManager.Clear();
		this.ResetSuperAndEx();
		this.hitManager.direction = 0;
		this.velocityManager.y = y;
		this.hitManager.timer = 0f;
		this.dashManager.state = LevelPlayerMotor.DashManager.State.Ready;
		this.parryManager.state = LevelPlayerMotor.ParryManager.State.Ready;
		this.jumpManager.state = LevelPlayerMotor.JumpManager.State.Ready;
	}

	// Token: 0x06003DDD RID: 15837 RVA: 0x00222060 File Offset: 0x00220460
	private IEnumerator launch_player_cr(float end)
	{
		float time = 0.1f;
		float t = 0f;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			float posY = Mathf.Lerp(base.transform.position.y, end, t / time);
			base.transform.SetPosition(null, new float?(posY), null);
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06003DDE RID: 15838 RVA: 0x00222084 File Offset: 0x00220484
	public void OnRevive(Vector3 pos)
	{
		if (this.GravityReversed)
		{
			pos.y -= (base.player.center.y - base.transform.position.y) * 2f;
		}
		base.transform.position = pos;
		this.hitManager.state = LevelPlayerMotor.HitManager.State.KnockedUp;
		this.DashComplete();
		this.velocityManager.Clear();
		this.ResetSuperAndEx();
		this.hitManager.direction = 0;
		this.LeaveGround(false);
		this.velocityManager.y = this.properties.reviveKnockUpPower;
		this.hitManager.timer = 0f;
		base.player.animationController.UpdateAnimator();
	}

	// Token: 0x06003DDF RID: 15839 RVA: 0x0022214F File Offset: 0x0022054F
	public void CancelReviveBounce()
	{
		this.velocityManager.y = 0f;
	}

	// Token: 0x06003DE0 RID: 15840 RVA: 0x00222161 File Offset: 0x00220561
	public void AddForce(LevelPlayerMotor.VelocityManager.Force force)
	{
		this.velocityManager.AddForce(force);
	}

	// Token: 0x06003DE1 RID: 15841 RVA: 0x0022216F File Offset: 0x0022056F
	public void RemoveForce(LevelPlayerMotor.VelocityManager.Force force)
	{
		this.velocityManager.RemoveForce(force);
		force.yAxisForce = false;
	}

	// Token: 0x06003DE2 RID: 15842 RVA: 0x00222184 File Offset: 0x00220584
	private void ClearParent()
	{
		if (base.transform.parent != null)
		{
			base.transform.parent.GetComponent<LevelPlatform>().OnPlayerExit(base.transform);
		}
		base.transform.parent = null;
		Vector3 localScale = base.transform.localScale;
		localScale.y = 1f * this.GravityReversalMultiplier;
		base.transform.localScale = localScale;
	}

	// Token: 0x06003DE3 RID: 15843 RVA: 0x002221F9 File Offset: 0x002205F9
	public void OnPlatformingLevelExit()
	{
		base.StartCoroutine(this.platformingExit_cr());
	}

	// Token: 0x06003DE4 RID: 15844 RVA: 0x00222208 File Offset: 0x00220608
	private IEnumerator platformingExit_cr()
	{
		for (;;)
		{
			if (this.Dashing)
			{
				this.DashComplete();
			}
			this.allowInput = false;
			this.Locked = false;
			this.LookDirection = new Trilean2(1, 0);
			this.velocityManager.move = this.properties.moveSpeed;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06003DE5 RID: 15845 RVA: 0x00222224 File Offset: 0x00220624
	private IEnumerator parryHit_cr()
	{
		this.velocityManager.Clear();
		yield return null;
		this.velocityManager.Clear();
		yield break;
	}

	// Token: 0x040044BE RID: 17598
	[SerializeField]
	private LevelPlayerMotor.Properties properties;

	// Token: 0x040044BF RID: 17599
	private Vector2 lastPositionFixed;

	// Token: 0x040044C0 RID: 17600
	private Vector2 lastPosition;

	// Token: 0x040044C1 RID: 17601
	private LevelPlayerMotor.VelocityManager velocityManager;

	// Token: 0x040044C2 RID: 17602
	private LevelPlayerMotor.JumpManager jumpManager;

	// Token: 0x040044C3 RID: 17603
	private LevelPlayerMotor.DashManager dashManager;

	// Token: 0x040044C4 RID: 17604
	private LevelPlayerMotor.ParryManager parryManager;

	// Token: 0x040044C5 RID: 17605
	private LevelPlayerMotor.DirectionManager directionManager;

	// Token: 0x040044C6 RID: 17606
	private LevelPlayerMotor.PlatformManager platformManager;

	// Token: 0x040044C7 RID: 17607
	private LevelPlayerMotor.HitManager hitManager;

	// Token: 0x040044C8 RID: 17608
	private LevelPlayerMotor.SuperManager superManager;

	// Token: 0x040044C9 RID: 17609
	private LevelPlayerMotor.BoundsManager boundsManager;

	// Token: 0x040044CA RID: 17610
	private bool allowInput;

	// Token: 0x040044CB RID: 17611
	private bool allowJumping;

	// Token: 0x040044CC RID: 17612
	private bool allowFalling;

	// Token: 0x040044CD RID: 17613
	private bool forceLaunchUp;

	// Token: 0x040044CE RID: 17614
	private bool hardExitParry;

	// Token: 0x040044CF RID: 17615
	private bool reversingGravity;

	// Token: 0x040044D0 RID: 17616
	private float jumpPower;

	// Token: 0x040044D1 RID: 17617
	private RaycastHit2D[] hitBuffer = new RaycastHit2D[25];

	// Token: 0x040044DA RID: 17626
	private LevelPlayerParryController parryController;

	// Token: 0x040044DC RID: 17628
	private const float RAY_DISTANCE = 2000f;

	// Token: 0x040044DD RID: 17629
	private const float MAX_GROUNDED_FALL_DISTANCE = 30f;

	// Token: 0x040044DE RID: 17630
	private readonly int wallMask = 262144;

	// Token: 0x040044DF RID: 17631
	private readonly int ceilingMask = 524288;

	// Token: 0x040044E0 RID: 17632
	private readonly int groundMask = 1048576;

	// Token: 0x040044E1 RID: 17633
	private LevelPlayerWeaponManager.Pose exFirePose;

	// Token: 0x040044E2 RID: 17634
	private const float JUMP_BUFFER_TIME = 0.0834f;

	// Token: 0x040044E3 RID: 17635
	public const float INPUT_BUFFER_TIME = 0.134f;

	// Token: 0x040044E4 RID: 17636
	private LevelPlayerMotor.BufferedInput bufferedInput;

	// Token: 0x040044E5 RID: 17637
	private float timeSinceInputBuffered = 0.134f;

	// Token: 0x02000A20 RID: 2592
	private enum RaycastAxis
	{
		// Token: 0x040044E7 RID: 17639
		X,
		// Token: 0x040044E8 RID: 17640
		Y
	}

	// Token: 0x02000A21 RID: 2593
	public enum BufferedInput
	{
		// Token: 0x040044EA RID: 17642
		Jump,
		// Token: 0x040044EB RID: 17643
		Dash,
		// Token: 0x040044EC RID: 17644
		Super
	}

	// Token: 0x02000A22 RID: 2594
	public class Properties
	{
		// Token: 0x040044ED RID: 17645
		public float moveSpeed = 490f;

		// Token: 0x040044EE RID: 17646
		public float maxSpeedY = 1620f;

		// Token: 0x040044EF RID: 17647
		public float timeToMaxY = 7.3f;

		// Token: 0x040044F0 RID: 17648
		public EaseUtils.EaseType yEase = EaseUtils.EaseType.linear;

		// Token: 0x040044F1 RID: 17649
		public float jumpHoldMin = 0.01f;

		// Token: 0x040044F2 RID: 17650
		public float jumpHoldMax = 0.16f;

		// Token: 0x040044F3 RID: 17651
		[Range(0f, -1f)]
		public float jumpPower = -0.755f;

		// Token: 0x040044F4 RID: 17652
		public float chaliceFirstJumpPower = -0.63f;

		// Token: 0x040044F5 RID: 17653
		public float chaliceSecondJumpPower = -0.55f;

		// Token: 0x040044F6 RID: 17654
		public float dashSpeed = 1100f;

		// Token: 0x040044F7 RID: 17655
		public float verticalDashSpeed = 935f;

		// Token: 0x040044F8 RID: 17656
		public float dashTime = 0.3f;

		// Token: 0x040044F9 RID: 17657
		public float dashEndTime = 0.21f;

		// Token: 0x040044FA RID: 17658
		public EaseUtils.EaseType dashEase = EaseUtils.EaseType.easeOutSine;

		// Token: 0x040044FB RID: 17659
		public float dashParryCooldownTime = 0.3f;

		// Token: 0x040044FC RID: 17660
		public float platformIgnoreTime = 1f;

		// Token: 0x040044FD RID: 17661
		public float hitStunTime = 0.3f;

		// Token: 0x040044FE RID: 17662
		public float hitFalloff = 0.25f;

		// Token: 0x040044FF RID: 17663
		[Range(0f, -1f)]
		public float hitJumpPower = -0.6f;

		// Token: 0x04004500 RID: 17664
		public float hitKnockbackPower = 300f;

		// Token: 0x04004501 RID: 17665
		public EaseUtils.EaseType hitKnockbackEase = EaseUtils.EaseType.linear;

		// Token: 0x04004502 RID: 17666
		public float knockUpStunTime = 0.2f;

		// Token: 0x04004503 RID: 17667
		[Range(0f, -3f)]
		public float pitKnockUpPower = -1.5f;

		// Token: 0x04004504 RID: 17668
		[Range(0f, -3f)]
		public float platformingPitKnockUpPower = -1.5f;

		// Token: 0x04004505 RID: 17669
		public float parryPower = -1f;

		// Token: 0x04004506 RID: 17670
		public float parryAttackBounce = -1f;

		// Token: 0x04004507 RID: 17671
		public float deathSpeed = 5f;

		// Token: 0x04004508 RID: 17672
		public float reviveKnockUpPower = -1f;

		// Token: 0x04004509 RID: 17673
		public float exKnockback = 230f;

		// Token: 0x0400450A RID: 17674
		public float superKnockUp = -0.6f;

		// Token: 0x0400450B RID: 17675
		public float superInvincibleKnockUp = -1.2f;
	}

	// Token: 0x02000A23 RID: 2595
	public class VelocityManager
	{
		// Token: 0x06003DE7 RID: 15847 RVA: 0x0022239F File Offset: 0x0022079F
		public VelocityManager(LevelPlayerMotor motor, float maxY, EaseUtils.EaseType yEase)
		{
			this.maxY = maxY;
			this.yEase = yEase;
			this.forces = new List<LevelPlayerMotor.VelocityManager.Force>();
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06003DE8 RID: 15848 RVA: 0x002223C0 File Offset: 0x002207C0
		// (set) Token: 0x06003DE9 RID: 15849 RVA: 0x002223C8 File Offset: 0x002207C8
		public bool yAxisForce { get; private set; }

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06003DEA RID: 15850 RVA: 0x002223D1 File Offset: 0x002207D1
		// (set) Token: 0x06003DEB RID: 15851 RVA: 0x002223D9 File Offset: 0x002207D9
		public float GroundForce { get; private set; }

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06003DEC RID: 15852 RVA: 0x002223E2 File Offset: 0x002207E2
		// (set) Token: 0x06003DED RID: 15853 RVA: 0x002223EA File Offset: 0x002207EA
		public float AirForce { get; private set; }

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06003DEE RID: 15854 RVA: 0x002223F3 File Offset: 0x002207F3
		// (set) Token: 0x06003DEF RID: 15855 RVA: 0x00222416 File Offset: 0x00220816
		public float y
		{
			get
			{
				this._y = Mathf.Clamp(this._y, -10f, 1f);
				return this._y;
			}
			set
			{
				this._y = Mathf.Clamp(value, -10f, 1f);
			}
		}

		// Token: 0x06003DF0 RID: 15856 RVA: 0x00222430 File Offset: 0x00220830
		public void Calculate()
		{
			this.GroundForce = 0f;
			this.AirForce = 0f;
			foreach (LevelPlayerMotor.VelocityManager.Force force in this.forces)
			{
				if (force.enabled)
				{
					LevelPlayerMotor.VelocityManager.Force.Type type = force.type;
					if (type != LevelPlayerMotor.VelocityManager.Force.Type.All)
					{
						if (type != LevelPlayerMotor.VelocityManager.Force.Type.Air)
						{
							if (type == LevelPlayerMotor.VelocityManager.Force.Type.Ground)
							{
								this.GroundForce += force.value;
							}
						}
						else
						{
							this.AirForce += force.value;
						}
					}
					else
					{
						this.AirForce += force.value;
						this.GroundForce += force.value;
					}
					if (force.yAxisForce)
					{
						this.yAxisForce = true;
					}
				}
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06003DF1 RID: 15857 RVA: 0x00222538 File Offset: 0x00220938
		public Vector2 Total
		{
			get
			{
				float value = this.y / 2f + 0.5f;
				Vector2 result = default(Vector2);
				result.y = EaseUtils.Ease(this.yEase, this.maxY, -this.maxY, value) + this.verticalDash;
				result.x += this.move + this.dash + this.hit;
				return result;
			}
		}

		// Token: 0x06003DF2 RID: 15858 RVA: 0x002225AA File Offset: 0x002209AA
		public void Clear()
		{
			this.move = 0f;
			this.dash = 0f;
			this.hit = 0f;
			this.y = 0f;
		}

		// Token: 0x06003DF3 RID: 15859 RVA: 0x002225D8 File Offset: 0x002209D8
		public void AddForce(LevelPlayerMotor.VelocityManager.Force force)
		{
			if (this.forces.Contains(force))
			{
				return;
			}
			this.forces.Add(force);
		}

		// Token: 0x06003DF4 RID: 15860 RVA: 0x002225F8 File Offset: 0x002209F8
		public void RemoveForce(LevelPlayerMotor.VelocityManager.Force force)
		{
			this.yAxisForce = false;
			if (this.forces.Contains(force))
			{
				this.forces.Remove(force);
			}
		}

		// Token: 0x0400450F RID: 17679
		public float move;

		// Token: 0x04004510 RID: 17680
		public float dash;

		// Token: 0x04004511 RID: 17681
		public float verticalDash;

		// Token: 0x04004512 RID: 17682
		public float hit;

		// Token: 0x04004513 RID: 17683
		private List<LevelPlayerMotor.VelocityManager.Force> forces;

		// Token: 0x04004514 RID: 17684
		private EaseUtils.EaseType yEase;

		// Token: 0x04004515 RID: 17685
		private float maxY;

		// Token: 0x04004516 RID: 17686
		private float _y;

		// Token: 0x02000A24 RID: 2596
		public class Force
		{
			// Token: 0x06003DF5 RID: 15861 RVA: 0x0022261F File Offset: 0x00220A1F
			public Force()
			{
				this.type = LevelPlayerMotor.VelocityManager.Force.Type.All;
				this.value = 0f;
			}

			// Token: 0x06003DF6 RID: 15862 RVA: 0x00222640 File Offset: 0x00220A40
			public Force(LevelPlayerMotor.VelocityManager.Force.Type type)
			{
				this.type = type;
				this.value = 0f;
			}

			// Token: 0x06003DF7 RID: 15863 RVA: 0x00222661 File Offset: 0x00220A61
			public Force(LevelPlayerMotor.VelocityManager.Force.Type type, float force)
			{
				this.type = type;
				this.value = force;
			}

			// Token: 0x06003DF8 RID: 15864 RVA: 0x0022267E File Offset: 0x00220A7E
			public Force(LevelPlayerMotor.VelocityManager.Force.Type type, float force, bool yAxis)
			{
				this.type = type;
				this.value = force;
				this.yAxisForce = yAxis;
			}

			// Token: 0x04004517 RID: 17687
			public bool yAxisForce;

			// Token: 0x04004518 RID: 17688
			public bool enabled = true;

			// Token: 0x04004519 RID: 17689
			public readonly LevelPlayerMotor.VelocityManager.Force.Type type;

			// Token: 0x0400451A RID: 17690
			public float value;

			// Token: 0x02000A25 RID: 2597
			public enum Type
			{
				// Token: 0x0400451C RID: 17692
				All,
				// Token: 0x0400451D RID: 17693
				Ground,
				// Token: 0x0400451E RID: 17694
				Air
			}
		}
	}

	// Token: 0x02000A26 RID: 2598
	public class JumpManager
	{
		// Token: 0x0400451F RID: 17695
		public LevelPlayerMotor.JumpManager.State state;

		// Token: 0x04004520 RID: 17696
		public float timer;

		// Token: 0x04004521 RID: 17697
		public float timeSinceDownJump = 1000f;

		// Token: 0x04004522 RID: 17698
		public float timeInAir;

		// Token: 0x04004523 RID: 17699
		public float longestTimeInAir;

		// Token: 0x04004524 RID: 17700
		public bool ableToLand;

		// Token: 0x04004525 RID: 17701
		public float floatTimer;

		// Token: 0x04004526 RID: 17702
		public bool doubleJumped;

		// Token: 0x02000A27 RID: 2599
		public enum State
		{
			// Token: 0x04004528 RID: 17704
			Ready,
			// Token: 0x04004529 RID: 17705
			Hold,
			// Token: 0x0400452A RID: 17706
			Used
		}
	}

	// Token: 0x02000A28 RID: 2600
	public class DashManager
	{
		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06003DFB RID: 15867 RVA: 0x002226C8 File Offset: 0x00220AC8
		public bool IsDashing
		{
			get
			{
				LevelPlayerMotor.DashManager.State state = this.state;
				return state == LevelPlayerMotor.DashManager.State.Start || state == LevelPlayerMotor.DashManager.State.Dashing || state == LevelPlayerMotor.DashManager.State.Ending;
			}
		}

		// Token: 0x0400452B RID: 17707
		public LevelPlayerMotor.DashManager.State state;

		// Token: 0x0400452C RID: 17708
		public int direction;

		// Token: 0x0400452D RID: 17709
		public float timer;

		// Token: 0x0400452E RID: 17710
		public const float DASH_COOLDOWN_DURATION = 0.1f;

		// Token: 0x0400452F RID: 17711
		public float timeSinceGroundDash = 0.1f;

		// Token: 0x04004530 RID: 17712
		public bool groundDash;

		// Token: 0x04004531 RID: 17713
		public float chaliceParryCoolDownTimer;

		// Token: 0x04004532 RID: 17714
		public bool chaliceParryCoolDown;

		// Token: 0x02000A29 RID: 2601
		public enum State
		{
			// Token: 0x04004534 RID: 17716
			Ready,
			// Token: 0x04004535 RID: 17717
			Start,
			// Token: 0x04004536 RID: 17718
			Dashing,
			// Token: 0x04004537 RID: 17719
			Ending,
			// Token: 0x04004538 RID: 17720
			End
		}
	}

	// Token: 0x02000A2A RID: 2602
	public class ParryManager
	{
		// Token: 0x04004539 RID: 17721
		public LevelPlayerMotor.ParryManager.State state;

		// Token: 0x02000A2B RID: 2603
		public enum State
		{
			// Token: 0x0400453B RID: 17723
			Ready,
			// Token: 0x0400453C RID: 17724
			NotReady
		}
	}

	// Token: 0x02000A2C RID: 2604
	public class PlatformManager
	{
		// Token: 0x06003DFD RID: 15869 RVA: 0x00222701 File Offset: 0x00220B01
		public PlatformManager(LevelPlayerMotor motor)
		{
			this.ignoredPlatforms = new List<Transform>();
			this.motor = motor;
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06003DFE RID: 15870 RVA: 0x0022271B File Offset: 0x00220B1B
		public bool OnPlatform
		{
			get
			{
				return this.motor.transform.parent != null;
			}
		}

		// Token: 0x06003DFF RID: 15871 RVA: 0x00222733 File Offset: 0x00220B33
		public void Ignore(Transform platform)
		{
			this.StopCoroutine();
			this.ignoreCoroutine = this.ignorePlatform_cr(platform);
			this.motor.StartCoroutine(this.ignoreCoroutine);
		}

		// Token: 0x06003E00 RID: 15872 RVA: 0x0022275A File Offset: 0x00220B5A
		public void StopCoroutine()
		{
			if (this.ignoreCoroutine != null)
			{
				this.motor.StopCoroutine(this.ignoreCoroutine);
			}
			this.ignoreCoroutine = null;
		}

		// Token: 0x06003E01 RID: 15873 RVA: 0x0022277F File Offset: 0x00220B7F
		public void Add(Transform platform)
		{
			this.ignoredPlatforms.Add(platform);
		}

		// Token: 0x06003E02 RID: 15874 RVA: 0x0022278D File Offset: 0x00220B8D
		public void Remove(Transform platform)
		{
			this.ignoredPlatforms.Remove(platform);
		}

		// Token: 0x06003E03 RID: 15875 RVA: 0x0022279C File Offset: 0x00220B9C
		public bool IsPlatformIgnored(Transform platform)
		{
			return this.ignoredPlatforms.Contains(platform);
		}

		// Token: 0x06003E04 RID: 15876 RVA: 0x002227AA File Offset: 0x00220BAA
		public void ResetAll()
		{
			this.StopCoroutine();
			this.ignoredPlatforms = new List<Transform>();
		}

		// Token: 0x06003E05 RID: 15877 RVA: 0x002227C0 File Offset: 0x00220BC0
		private IEnumerator ignorePlatform_cr(Transform platform)
		{
			this.Add(platform);
			yield return CupheadTime.WaitForSeconds(this.motor, this.motor.properties.platformIgnoreTime);
			this.Remove(platform);
			yield break;
		}

		// Token: 0x0400453D RID: 17725
		private List<Transform> ignoredPlatforms;

		// Token: 0x0400453E RID: 17726
		private LevelPlayerMotor motor;

		// Token: 0x0400453F RID: 17727
		private IEnumerator ignoreCoroutine;
	}

	// Token: 0x02000A2D RID: 2605
	public class DirectionManager
	{
		// Token: 0x06003E06 RID: 15878 RVA: 0x002228AC File Offset: 0x00220CAC
		public DirectionManager()
		{
			this.Reset();
		}

		// Token: 0x06003E07 RID: 15879 RVA: 0x002228E6 File Offset: 0x00220CE6
		public void Reset()
		{
			this.up.Reset();
			this.down.Reset();
			this.left.Reset();
			this.right.Reset();
		}

		// Token: 0x04004540 RID: 17728
		public LevelPlayerMotor.DirectionManager.Hit up = new LevelPlayerMotor.DirectionManager.Hit();

		// Token: 0x04004541 RID: 17729
		public LevelPlayerMotor.DirectionManager.Hit down = new LevelPlayerMotor.DirectionManager.Hit();

		// Token: 0x04004542 RID: 17730
		public LevelPlayerMotor.DirectionManager.Hit left = new LevelPlayerMotor.DirectionManager.Hit();

		// Token: 0x04004543 RID: 17731
		public LevelPlayerMotor.DirectionManager.Hit right = new LevelPlayerMotor.DirectionManager.Hit();

		// Token: 0x02000A2E RID: 2606
		public class Hit
		{
			// Token: 0x06003E08 RID: 15880 RVA: 0x00222914 File Offset: 0x00220D14
			public Hit()
			{
				this.Reset();
			}

			// Token: 0x06003E09 RID: 15881 RVA: 0x00222922 File Offset: 0x00220D22
			public Hit(bool able, Vector2 pos, GameObject gameObject, float distance)
			{
				this.able = able;
				this.pos = pos;
				this.gameObject = gameObject;
				this.distance = distance;
			}

			// Token: 0x06003E0A RID: 15882 RVA: 0x00222947 File Offset: 0x00220D47
			public void Reset()
			{
				this.able = true;
				this.pos = Vector2.zero;
				this.gameObject = null;
				this.distance = -1f;
			}

			// Token: 0x04004544 RID: 17732
			public bool able;

			// Token: 0x04004545 RID: 17733
			public Vector2 pos;

			// Token: 0x04004546 RID: 17734
			public GameObject gameObject;

			// Token: 0x04004547 RID: 17735
			public float distance;
		}
	}

	// Token: 0x02000A2F RID: 2607
	public class HitManager
	{
		// Token: 0x06003E0C RID: 15884 RVA: 0x00222975 File Offset: 0x00220D75
		public void Reset()
		{
			this.state = LevelPlayerMotor.HitManager.State.Inactive;
			this.timer = 0f;
			this.direction = 0;
		}

		// Token: 0x04004548 RID: 17736
		public LevelPlayerMotor.HitManager.State state;

		// Token: 0x04004549 RID: 17737
		public float timer;

		// Token: 0x0400454A RID: 17738
		public int direction;

		// Token: 0x02000A30 RID: 2608
		public enum State
		{
			// Token: 0x0400454C RID: 17740
			Inactive,
			// Token: 0x0400454D RID: 17741
			Hit,
			// Token: 0x0400454E RID: 17742
			KnockedUp
		}
	}

	// Token: 0x02000A31 RID: 2609
	public class SuperManager
	{
		// Token: 0x0400454F RID: 17743
		public LevelPlayerMotor.SuperManager.State state;

		// Token: 0x02000A32 RID: 2610
		public enum State
		{
			// Token: 0x04004551 RID: 17745
			Ready,
			// Token: 0x04004552 RID: 17746
			Ex,
			// Token: 0x04004553 RID: 17747
			Super
		}
	}

	// Token: 0x02000A33 RID: 2611
	public class BoundsManager
	{
		// Token: 0x06003E0E RID: 15886 RVA: 0x00222998 File Offset: 0x00220D98
		public BoundsManager(LevelPlayerMotor motor)
		{
			this.Motor = motor;
			this.transform = motor.transform;
			this.boxCollider = (this.transform.GetComponent<Collider2D>() as BoxCollider2D);
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06003E0F RID: 15887 RVA: 0x002229C9 File Offset: 0x00220DC9
		// (set) Token: 0x06003E10 RID: 15888 RVA: 0x002229D1 File Offset: 0x00220DD1
		public LevelPlayerMotor Motor { get; set; }

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06003E11 RID: 15889 RVA: 0x002229DC File Offset: 0x00220DDC
		public Vector3 Top
		{
			get
			{
				return new Vector3(this.Center.x, this.Center.y + this.boxCollider.size.y / 2f, 0f);
			}
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06003E12 RID: 15890 RVA: 0x00222A2C File Offset: 0x00220E2C
		public Vector3 TopLeft
		{
			get
			{
				return new Vector3(this.Center.x - this.boxCollider.size.x / 2f, this.Center.y + this.boxCollider.size.y / 2f, 0f);
			}
		}

		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x06003E13 RID: 15891 RVA: 0x00222A94 File Offset: 0x00220E94
		public Vector3 TopRight
		{
			get
			{
				return new Vector3(this.Center.x + this.boxCollider.size.x / 2f, this.Center.y + this.boxCollider.size.y / 2f, 0f);
			}
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06003E14 RID: 15892 RVA: 0x00222AFC File Offset: 0x00220EFC
		public Vector3 CenterLeft
		{
			get
			{
				return new Vector3(this.Center.x - this.boxCollider.size.x / 2f, this.Center.y, 0f);
			}
		}

		// Token: 0x17000558 RID: 1368
		// (get) Token: 0x06003E15 RID: 15893 RVA: 0x00222B4C File Offset: 0x00220F4C
		public Vector3 CenterRight
		{
			get
			{
				return new Vector3(this.Center.x + this.boxCollider.size.x / 2f, this.Center.y, 0f);
			}
		}

		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x06003E16 RID: 15894 RVA: 0x00222B9C File Offset: 0x00220F9C
		public Vector2 Center
		{
			get
			{
				return this.transform.position + new Vector2(this.boxCollider.offset.x, this.Motor.GravityReversalMultiplier * this.boxCollider.offset.y);
			}
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06003E17 RID: 15895 RVA: 0x00222BF8 File Offset: 0x00220FF8
		public Vector3 Bottom
		{
			get
			{
				return new Vector3(this.Center.x, this.Center.y - this.boxCollider.size.y / 2f, 0f);
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06003E18 RID: 15896 RVA: 0x00222C48 File Offset: 0x00221048
		public Vector3 BottomLeft
		{
			get
			{
				return new Vector3(this.Center.x - this.boxCollider.size.x / 2f, this.Center.y - this.boxCollider.size.y / 2f, 0f);
			}
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06003E19 RID: 15897 RVA: 0x00222CB0 File Offset: 0x002210B0
		public Vector3 BottomRight
		{
			get
			{
				return new Vector3(this.Center.x + this.boxCollider.size.x / 2f, this.Center.y - this.boxCollider.size.y / 2f, 0f);
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06003E1A RID: 15898 RVA: 0x00222D18 File Offset: 0x00221118
		public float TopY
		{
			get
			{
				return this.Top.y - this.transform.position.y;
			}
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06003E1B RID: 15899 RVA: 0x00222D48 File Offset: 0x00221148
		public float BottomY
		{
			get
			{
				return this.Bottom.y - this.transform.position.y;
			}
		}

		// Token: 0x04004554 RID: 17748
		private readonly Transform transform;

		// Token: 0x04004555 RID: 17749
		private BoxCollider2D boxCollider;
	}
}
