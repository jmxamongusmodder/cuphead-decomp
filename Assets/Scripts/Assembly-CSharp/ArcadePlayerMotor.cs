using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020009DF RID: 2527
public class ArcadePlayerMotor : AbstractArcadePlayerComponent
{
	// Token: 0x170004F9 RID: 1273
	// (get) Token: 0x06003B97 RID: 15255 RVA: 0x002156B3 File Offset: 0x00213AB3
	// (set) Token: 0x06003B98 RID: 15256 RVA: 0x002156BB File Offset: 0x00213ABB
	public Trilean2 LookDirection { get; private set; }

	// Token: 0x170004FA RID: 1274
	// (get) Token: 0x06003B99 RID: 15257 RVA: 0x002156C4 File Offset: 0x00213AC4
	// (set) Token: 0x06003B9A RID: 15258 RVA: 0x002156CC File Offset: 0x00213ACC
	public Trilean2 TrueLookDirection { get; private set; }

	// Token: 0x170004FB RID: 1275
	// (get) Token: 0x06003B9B RID: 15259 RVA: 0x002156D5 File Offset: 0x00213AD5
	// (set) Token: 0x06003B9C RID: 15260 RVA: 0x002156DD File Offset: 0x00213ADD
	public Trilean2 MoveDirection { get; private set; }

	// Token: 0x170004FC RID: 1276
	// (get) Token: 0x06003B9D RID: 15261 RVA: 0x002156E6 File Offset: 0x00213AE6
	public ArcadePlayerMotor.JumpManager.State JumpState
	{
		get
		{
			return this.jumpManager.state;
		}
	}

	// Token: 0x170004FD RID: 1277
	// (get) Token: 0x06003B9E RID: 15262 RVA: 0x002156F3 File Offset: 0x00213AF3
	public bool Dashing
	{
		get
		{
			return this.dashManager.IsDashing;
		}
	}

	// Token: 0x170004FE RID: 1278
	// (get) Token: 0x06003B9F RID: 15263 RVA: 0x00215700 File Offset: 0x00213B00
	public int DashDirection
	{
		get
		{
			return this.dashManager.direction;
		}
	}

	// Token: 0x170004FF RID: 1279
	// (get) Token: 0x06003BA0 RID: 15264 RVA: 0x0021570D File Offset: 0x00213B0D
	// (set) Token: 0x06003BA1 RID: 15265 RVA: 0x00215715 File Offset: 0x00213B15
	public bool Locked { get; private set; }

	// Token: 0x17000500 RID: 1280
	// (get) Token: 0x06003BA2 RID: 15266 RVA: 0x0021571E File Offset: 0x00213B1E
	// (set) Token: 0x06003BA3 RID: 15267 RVA: 0x00215726 File Offset: 0x00213B26
	public bool Grounded { get; private set; }

	// Token: 0x17000501 RID: 1281
	// (get) Token: 0x06003BA4 RID: 15268 RVA: 0x0021572F File Offset: 0x00213B2F
	// (set) Token: 0x06003BA5 RID: 15269 RVA: 0x00215737 File Offset: 0x00213B37
	public bool Parrying { get; private set; }

	// Token: 0x17000502 RID: 1282
	// (get) Token: 0x06003BA6 RID: 15270 RVA: 0x00215740 File Offset: 0x00213B40
	public bool IsHit
	{
		get
		{
			return this.hitManager.state == ArcadePlayerMotor.HitManager.State.Hit;
		}
	}

	// Token: 0x17000503 RID: 1283
	// (get) Token: 0x06003BA7 RID: 15271 RVA: 0x00215750 File Offset: 0x00213B50
	public bool IsUsingSuperOrEx
	{
		get
		{
			return this.superManager.state == ArcadePlayerMotor.SuperManager.State.Super || this.superManager.state == ArcadePlayerMotor.SuperManager.State.Ex;
		}
	}

	// Token: 0x14000077 RID: 119
	// (add) Token: 0x06003BA8 RID: 15272 RVA: 0x00215774 File Offset: 0x00213B74
	// (remove) Token: 0x06003BA9 RID: 15273 RVA: 0x002157AC File Offset: 0x00213BAC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnGroundedEvent;

	// Token: 0x14000078 RID: 120
	// (add) Token: 0x06003BAA RID: 15274 RVA: 0x002157E4 File Offset: 0x00213BE4
	// (remove) Token: 0x06003BAB RID: 15275 RVA: 0x0021581C File Offset: 0x00213C1C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnJumpEvent;

	// Token: 0x14000079 RID: 121
	// (add) Token: 0x06003BAC RID: 15276 RVA: 0x00215854 File Offset: 0x00213C54
	// (remove) Token: 0x06003BAD RID: 15277 RVA: 0x0021588C File Offset: 0x00213C8C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnParryEvent;

	// Token: 0x1400007A RID: 122
	// (add) Token: 0x06003BAE RID: 15278 RVA: 0x002158C4 File Offset: 0x00213CC4
	// (remove) Token: 0x06003BAF RID: 15279 RVA: 0x002158FC File Offset: 0x00213CFC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnParrySuccess;

	// Token: 0x1400007B RID: 123
	// (add) Token: 0x06003BB0 RID: 15280 RVA: 0x00215934 File Offset: 0x00213D34
	// (remove) Token: 0x06003BB1 RID: 15281 RVA: 0x0021596C File Offset: 0x00213D6C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnHitEvent;

	// Token: 0x1400007C RID: 124
	// (add) Token: 0x06003BB2 RID: 15282 RVA: 0x002159A4 File Offset: 0x00213DA4
	// (remove) Token: 0x06003BB3 RID: 15283 RVA: 0x002159DC File Offset: 0x00213DDC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnDashStartEvent;

	// Token: 0x1400007D RID: 125
	// (add) Token: 0x06003BB4 RID: 15284 RVA: 0x00215A14 File Offset: 0x00213E14
	// (remove) Token: 0x06003BB5 RID: 15285 RVA: 0x00215A4C File Offset: 0x00213E4C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnDashEndEvent;

	// Token: 0x06003BB6 RID: 15286 RVA: 0x00215A84 File Offset: 0x00213E84
	protected override void OnAwake()
	{
		base.OnAwake();
		this.properties = new ArcadePlayerMotor.Properties();
		this.MoveDirection = new Trilean2(0, 0);
		this.LookDirection = new Trilean2(1, 0);
		this.TrueLookDirection = new Trilean2(1, 0);
		this.velocityManager = new ArcadePlayerMotor.VelocityManager(this, this.properties.maxSpeedY, this.properties.yEase);
		this.jumpManager = new ArcadePlayerMotor.JumpManager();
		this.dashManager = new ArcadePlayerMotor.DashManager();
		this.parryManager = new ArcadePlayerMotor.ParryManager();
		this.directionManager = new ArcadePlayerMotor.DirectionManager();
		this.platformManager = new ArcadePlayerMotor.PlatformManager(this);
		this.hitManager = new ArcadePlayerMotor.HitManager();
		this.superManager = new ArcadePlayerMotor.SuperManager();
		this.boundsManager = new ArcadePlayerMotor.BoundsManager(base.transform);
		base.player.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.allowInput = true;
		this.allowFalling = true;
	}

	// Token: 0x06003BB7 RID: 15287 RVA: 0x00215B74 File Offset: 0x00213F74
	private void Start()
	{
		base.player.weaponManager.OnExStart += this.StartEx;
		base.player.weaponManager.OnSuperStart += this.StartSuper;
		base.player.weaponManager.OnExFire += this.OnExFired;
		base.player.weaponManager.OnSuperEnd += this.OnSuperEnd;
		base.player.weaponManager.OnExEnd += this.ResetSuperAndEx;
		base.player.weaponManager.OnSuperEnd += this.ResetSuperAndEx;
		base.player.OnReviveEvent += this.OnRevive;
	}

	// Token: 0x06003BB8 RID: 15288 RVA: 0x00215C40 File Offset: 0x00214040
	private void FixedUpdate()
	{
		if (base.player.IsDead)
		{
			return;
		}
		if (base.player.controlScheme != ArcadePlayerController.ControlScheme.Rocket)
		{
			this.HandleLooking();
		}
		if (base.player.weaponManager.FreezePosition)
		{
			return;
		}
		if (base.player.controlScheme == ArcadePlayerController.ControlScheme.Rocket)
		{
			this.RocketInput();
		}
		else
		{
			this.HandleInput();
			if (base.player.controlScheme == ArcadePlayerController.ControlScheme.Normal && this.allowFalling)
			{
				this.HandleFalling();
			}
			this.Move();
			this.HandleRaycasts();
			Vector2 a = base.transform.localPosition;
			Vector2 v = a - this.lastPositionFixed;
			v.x = (float)((int)v.x);
			v.y = (float)((int)v.y);
			this.MoveDirection = v;
			this.lastPositionFixed = new Vector2(a.x, a.y);
			this.lastPosition = base.transform.position;
		}
		this.ClampToBounds();
	}

	// Token: 0x06003BB9 RID: 15289 RVA: 0x00215D59 File Offset: 0x00214159
	private void LateUpdate()
	{
		this.ClampToBounds();
	}

	// Token: 0x06003BBA RID: 15290 RVA: 0x00215D61 File Offset: 0x00214161
	public void DisableInput()
	{
		this.allowInput = false;
		this.Locked = false;
		this.MoveDirection = new Trilean2(0, 0);
		this.velocityManager.move = 0f;
	}

	// Token: 0x06003BBB RID: 15291 RVA: 0x00215D8E File Offset: 0x0021418E
	public void EnableInput()
	{
		this.allowInput = true;
	}

	// Token: 0x06003BBC RID: 15292 RVA: 0x00215D98 File Offset: 0x00214198
	public void DisableGravity()
	{
		this.allowFalling = false;
		this.MoveDirection = new Trilean2(this.MoveDirection.x, 0);
		this.velocityManager.y = 0f;
	}

	// Token: 0x06003BBD RID: 15293 RVA: 0x00215DDB File Offset: 0x002141DB
	public void EnableGravity()
	{
		this.allowFalling = true;
		this.velocityManager.y = 0f;
	}

	// Token: 0x06003BBE RID: 15294 RVA: 0x00215DF4 File Offset: 0x002141F4
	private RaycastHit2D BoxCast(Vector2 size, Vector2 direction, int layerMask)
	{
		return this.BoxCast(size, direction, layerMask, Vector2.zero);
	}

	// Token: 0x06003BBF RID: 15295 RVA: 0x00215E04 File Offset: 0x00214204
	private RaycastHit2D BoxCast(Vector2 size, Vector2 direction, int layerMask, Vector2 offset)
	{
		return Physics2D.BoxCast(base.player.colliderManager.Center + offset, size, 0f, direction, 2000f, layerMask);
	}

	// Token: 0x06003BC0 RID: 15296 RVA: 0x00215E2F File Offset: 0x0021422F
	private RaycastHit2D CircleCast(float radius, Vector2 direction, int layerMask)
	{
		return Physics2D.CircleCast(base.player.colliderManager.Center, radius, direction, 2000f, layerMask);
	}

	// Token: 0x06003BC1 RID: 15297 RVA: 0x00215E4E File Offset: 0x0021424E
	private bool DoesRaycastHitHaveCollider(RaycastHit2D hit)
	{
		return hit.collider != null;
	}

	// Token: 0x06003BC2 RID: 15298 RVA: 0x00215E60 File Offset: 0x00214260
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

	// Token: 0x06003BC3 RID: 15299 RVA: 0x00215EBC File Offset: 0x002142BC
	private void HandleRaycasts()
	{
		bool flag = true;
		if (this.directionManager != null && this.directionManager.up != null)
		{
			flag = this.directionManager.up.able;
		}
		ArcadePlayerColliderManager colliderManager = base.player.colliderManager;
		this.directionManager.Reset();
		RaycastHit2D raycastHit = this.BoxCast(new Vector2(1f, colliderManager.Height), Vector2.left, this.wallMask);
		RaycastHit2D raycastHit2 = this.BoxCast(new Vector2(1f, colliderManager.Height), Vector2.right, this.wallMask);
		RaycastHit2D raycastHit3 = this.BoxCast(new Vector2(colliderManager.Width, 1f), Vector2.up, this.ceilingMask);
		this.RaycastObstacle(this.directionManager.left, raycastHit, base.player.colliderManager.DefaultWidth / 2f, ArcadePlayerMotor.RaycastAxis.X);
		this.RaycastObstacle(this.directionManager.right, raycastHit2, base.player.colliderManager.DefaultWidth / 2f, ArcadePlayerMotor.RaycastAxis.X);
		this.RaycastObstacle(this.directionManager.up, raycastHit3, base.player.colliderManager.Height / 2f, ArcadePlayerMotor.RaycastAxis.Y);
		Vector2 vector = colliderManager.Center + new Vector2(0f, colliderManager.DefaultHeight);
		RaycastHit2D[] array = Physics2D.BoxCastAll(vector, new Vector2(colliderManager.Width, 1f), 0f, Vector2.down, 1000f, this.groundMask);
		this.directionManager.down.pos = new Vector2(colliderManager.Center.x, -10000f);
		foreach (RaycastHit2D raycastHit2D in array)
		{
			if (raycastHit2D.point.y > this.directionManager.down.pos.y)
			{
				if (raycastHit2D.point.y <= 20f + base.transform.position.y)
				{
					float num = Math.Abs(base.transform.position.y - raycastHit2D.point.y);
					this.directionManager.down.pos = new Vector2(vector.x, raycastHit2D.point.y);
					this.directionManager.down.gameObject = raycastHit2D.collider.gameObject;
					this.directionManager.down.distance = num;
					if (num < 20f)
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
			if (!this.directionManager.up.able && this.directionManager.up.able != flag)
			{
				this.OnHitCeiling();
			}
		}
		float num2 = base.transform.position.y - this.directionManager.down.pos.y;
		if (this.Grounded && num2 > 30f)
		{
			this.LeaveGround();
		}
	}

	// Token: 0x06003BC4 RID: 15300 RVA: 0x00216280 File Offset: 0x00214680
	private float RaycastObstacle(ArcadePlayerMotor.DirectionManager.Hit directionProperties, RaycastHit2D raycastHit, float maxDistance, ArcadePlayerMotor.RaycastAxis axis)
	{
		if (!this.DoesRaycastHitHaveCollider(raycastHit))
		{
			return 1000f;
		}
		float num = (axis != ArcadePlayerMotor.RaycastAxis.X) ? Math.Abs(base.player.colliderManager.Center.y - raycastHit.point.y) : Math.Abs(base.player.colliderManager.Center.x - raycastHit.point.x);
		directionProperties.pos = raycastHit.point;
		directionProperties.gameObject = raycastHit.collider.gameObject;
		directionProperties.distance = num;
		if (num < maxDistance)
		{
			directionProperties.able = false;
		}
		return num;
	}

	// Token: 0x06003BC5 RID: 15301 RVA: 0x0021633C File Offset: 0x0021473C
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
		this.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Ready;
		this.parryManager.state = ArcadePlayerMotor.ParryManager.State.Ready;
		this.velocityManager.y = 0f;
		this.platformManager.ResetAll();
		this.Grounded = true;
		this.Parrying = false;
		this.dashManager.timeSinceGroundDash = 1000f;
		if (this.OnGroundedEvent != null)
		{
			this.OnGroundedEvent();
		}
	}

	// Token: 0x06003BC6 RID: 15302 RVA: 0x00216444 File Offset: 0x00214844
	private void LeaveGround()
	{
		this.Grounded = false;
		this.jumpManager.ableToLand = false;
		this.velocityManager.y = 0f;
		this.ClearParent();
		if (this.jumpManager.state == ArcadePlayerMotor.JumpManager.State.Ready)
		{
			this.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Used;
		}
	}

	// Token: 0x06003BC7 RID: 15303 RVA: 0x00216498 File Offset: 0x00214898
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

	// Token: 0x06003BC8 RID: 15304 RVA: 0x002164E8 File Offset: 0x002148E8
	private void Move()
	{
		this.velocityManager.Calculate();
		Vector3 a = this.velocityManager.Total;
		if (this.hitManager.state != ArcadePlayerMotor.HitManager.State.Hit && this.superManager.state == ArcadePlayerMotor.SuperManager.State.Ready)
		{
			if (this.Grounded)
			{
				a.x += this.velocityManager.GroundForce;
			}
			else
			{
				a.x += this.velocityManager.AirForce;
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
		base.transform.localPosition += a * CupheadTime.FixedDelta;
		if (this.Grounded)
		{
			Vector2 v = base.transform.position;
			v.y = this.directionManager.down.pos.y;
			base.transform.position = v;
			LevelPlatform component = this.directionManager.down.gameObject.GetComponent<LevelPlatform>();
			if (component == null && base.transform.parent != null)
			{
				this.ClearParent();
			}
			else if (component != null && (base.transform.parent == null || component.gameObject != base.transform.parent.gameObject))
			{
				this.ClearParent();
				component.AddChild(base.transform);
			}
		}
	}

	// Token: 0x06003BC9 RID: 15305 RVA: 0x002167C0 File Offset: 0x00214BC0
	private void ClampToBounds()
	{
		CupheadBounds cupheadBounds = new CupheadBounds();
		cupheadBounds.left = this.directionManager.left.pos.x + base.player.colliderManager.Width / 2f;
		cupheadBounds.right = this.directionManager.right.pos.x - base.player.colliderManager.Width / 2f;
		cupheadBounds.top = this.directionManager.up.pos.y - this.boundsManager.TopY;
		cupheadBounds.bottom = this.directionManager.down.pos.y - this.boundsManager.BottomY;
		Vector3 position = base.transform.position;
		if (!this.directionManager.left.able && base.transform.position.x < cupheadBounds.left)
		{
			position.x = cupheadBounds.left;
		}
		if (!this.directionManager.right.able && base.transform.position.x > cupheadBounds.right)
		{
			position.x = cupheadBounds.right;
		}
		if (!this.directionManager.up.able && base.transform.position.y > cupheadBounds.top)
		{
			position.y = cupheadBounds.top;
		}
		position.x = Mathf.Clamp(position.x, (float)Level.Current.Left + base.player.colliderManager.Width / 2f, (float)Level.Current.Right - base.player.colliderManager.Width / 2f);
		if (base.player.controlScheme != ArcadePlayerController.ControlScheme.Normal)
		{
			position.y = Mathf.Clamp(position.y, (float)Level.Current.Ground + base.player.colliderManager.Height / 2f, (float)Level.Current.Ceiling - base.player.colliderManager.Height / 2f);
		}
		base.transform.position = position;
	}

	// Token: 0x06003BCA RID: 15306 RVA: 0x00216A1C File Offset: 0x00214E1C
	private void ResetSuperAndEx()
	{
		if (this.superManager.state == ArcadePlayerMotor.SuperManager.State.Ready)
		{
			return;
		}
		if (this.jumpManager.state != ArcadePlayerMotor.JumpManager.State.Ready)
		{
			this.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Used;
		}
		base.StopCoroutine(this.exMove_cr());
		this.superManager.state = ArcadePlayerMotor.SuperManager.State.Ready;
		this.EnableInput();
		this.EnableGravity();
	}

	// Token: 0x06003BCB RID: 15307 RVA: 0x00216A7A File Offset: 0x00214E7A
	private void StartSuper()
	{
		this.LeaveGround();
		this.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Used;
		this.jumpManager.timer = 0f;
		this.velocityManager.y = 0f;
	}

	// Token: 0x06003BCC RID: 15308 RVA: 0x00216AAE File Offset: 0x00214EAE
	public void OnSuperEnd()
	{
		if (this.Grounded)
		{
			this.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Ready;
		}
		else
		{
			this.LeaveGround();
			this.velocityManager.y = this.properties.superKnockUp;
		}
	}

	// Token: 0x06003BCD RID: 15309 RVA: 0x00216AE8 File Offset: 0x00214EE8
	private void StartEx()
	{
		this.exFirePose = base.player.weaponManager.GetDirectionPose();
		this.DisableInput();
		this.DisableGravity();
		this.superManager.state = ArcadePlayerMotor.SuperManager.State.Ex;
	}

	// Token: 0x06003BCE RID: 15310 RVA: 0x00216B18 File Offset: 0x00214F18
	private void OnExFired()
	{
		if (this.exFirePose == ArcadePlayerWeaponManager.Pose.Up || this.exFirePose == ArcadePlayerWeaponManager.Pose.Down)
		{
			base.StartCoroutine(this.exDelay_cr());
		}
		else
		{
			base.StartCoroutine(this.exMove_cr());
		}
	}

	// Token: 0x06003BCF RID: 15311 RVA: 0x00216B54 File Offset: 0x00214F54
	private IEnumerator exDelay_cr()
	{
		while (this.superManager.state != ArcadePlayerMotor.SuperManager.State.Ready)
		{
			yield return null;
		}
		this.EnableInput();
		this.EnableGravity();
		this.superManager.state = ArcadePlayerMotor.SuperManager.State.Ready;
		yield break;
	}

	// Token: 0x06003BD0 RID: 15312 RVA: 0x00216B70 File Offset: 0x00214F70
	private IEnumerator exMove_cr()
	{
		while (this.superManager.state != ArcadePlayerMotor.SuperManager.State.Ready)
		{
			this.velocityManager.move = (float)(this.TrueLookDirection.x * -1) * this.properties.exKnockback;
			yield return null;
		}
		this.EnableInput();
		this.EnableGravity();
		this.superManager.state = ArcadePlayerMotor.SuperManager.State.Ready;
		yield break;
	}

	// Token: 0x06003BD1 RID: 15313 RVA: 0x00216B8C File Offset: 0x00214F8C
	private void HandleInput()
	{
		if (!base.player.levelStarted)
		{
			return;
		}
		this.timeSinceInputBuffered += CupheadTime.FixedDelta;
		this.dashManager.timeSinceGroundDash += CupheadTime.FixedDelta;
		if ((!this.allowInput || this.dashManager.state != ArcadePlayerMotor.DashManager.State.Ready) && this.hitManager.state == ArcadePlayerMotor.HitManager.State.Inactive)
		{
			this.BufferInputs();
		}
		if (!this.allowInput)
		{
			return;
		}
		if (!this.HandleDash())
		{
			if (this.hitManager.state == ArcadePlayerMotor.HitManager.State.Hit)
			{
				this.HandleHit();
			}
			else
			{
				if (base.player.controlScheme == ArcadePlayerController.ControlScheme.Normal)
				{
					this.HandleParry();
					this.HandleJumping();
				}
				else if (base.player.controlScheme == ArcadePlayerController.ControlScheme.Jetpack)
				{
					this.HandleJetpackJump();
				}
				this.HandleWalking();
			}
		}
	}

	// Token: 0x06003BD2 RID: 15314 RVA: 0x00216C77 File Offset: 0x00215077
	private void BufferInput(ArcadePlayerMotor.BufferedInput input)
	{
		this.bufferedInput = input;
		this.timeSinceInputBuffered = 0f;
	}

	// Token: 0x06003BD3 RID: 15315 RVA: 0x00216C8C File Offset: 0x0021508C
	public void BufferInputs()
	{
		if (base.player.input.actions.GetButtonDown(2))
		{
			this.BufferInput(ArcadePlayerMotor.BufferedInput.Jump);
		}
		else if (base.player.input.actions.GetButtonDown(7) && !this.dashManager.IsDashing)
		{
			this.BufferInput(ArcadePlayerMotor.BufferedInput.Dash);
		}
		else if (base.player.input.actions.GetButtonDown(4))
		{
			this.BufferInput(ArcadePlayerMotor.BufferedInput.Super);
		}
	}

	// Token: 0x06003BD4 RID: 15316 RVA: 0x00216D19 File Offset: 0x00215119
	public void ClearBufferedInput()
	{
		this.timeSinceInputBuffered = 0.134f;
	}

	// Token: 0x06003BD5 RID: 15317 RVA: 0x00216D26 File Offset: 0x00215126
	public bool HasBufferedInput(ArcadePlayerMotor.BufferedInput input)
	{
		return this.bufferedInput == input && this.timeSinceInputBuffered < 0.134f;
	}

	// Token: 0x06003BD6 RID: 15318 RVA: 0x00216D44 File Offset: 0x00215144
	private void HandleJumping()
	{
		if (this.jumpManager.state == ArcadePlayerMotor.JumpManager.State.Ready && (base.player.input.actions.GetButtonDown(2) || this.HasBufferedInput(ArcadePlayerMotor.BufferedInput.Jump)))
		{
			if (this.LookDirection.y < 0 && this.Grounded && base.transform.parent != null)
			{
				LevelPlatform component = base.transform.parent.GetComponent<LevelPlatform>();
				if (component.canFallThrough)
				{
					this.platformManager.Ignore(base.transform.parent);
					this.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Used;
					this.LeaveGround();
					this.jumpManager.timeSinceDownJump = 0f;
					return;
				}
			}
			AudioManager.Play("player_jump");
			this.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Hold;
			this.LeaveGround();
			this.velocityManager.y = this.properties.jumpPower;
			this.jumpManager.timer = CupheadTime.FixedDelta;
			if (this.OnJumpEvent != null)
			{
				this.OnJumpEvent();
			}
		}
		if (this.jumpManager.state == ArcadePlayerMotor.JumpManager.State.Hold)
		{
			if (!this.directionManager.up.able || (this.jumpManager.timer >= this.properties.jumpHoldMin && (base.player.input.actions.GetButtonUp(2) || !base.player.input.actions.GetButton(2))) || this.jumpManager.timer >= this.properties.jumpHoldMax)
			{
				this.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Used;
				this.jumpManager.timer = 0f;
			}
			this.velocityManager.y = this.properties.jumpPower;
			this.jumpManager.timer += CupheadTime.FixedDelta;
		}
		this.jumpManager.timeSinceDownJump += CupheadTime.FixedDelta;
	}

	// Token: 0x06003BD7 RID: 15319 RVA: 0x00216F64 File Offset: 0x00215364
	private void HandleParry()
	{
		if (this.IsHit)
		{
			return;
		}
		if (this.parryManager.state == ArcadePlayerMotor.ParryManager.State.Ready && (base.player.input.actions.GetButtonDown(2) || this.HasBufferedInput(ArcadePlayerMotor.BufferedInput.Jump)) && this.jumpManager.state != ArcadePlayerMotor.JumpManager.State.Ready && !this.IsHit)
		{
			this.hitManager.state = ArcadePlayerMotor.HitManager.State.Inactive;
			this.parryManager.state = ArcadePlayerMotor.ParryManager.State.NotReady;
			if (this.dashManager.IsDashing)
			{
				this.dashManager.state = ArcadePlayerMotor.DashManager.State.End;
			}
			this.Parrying = true;
			if (this.OnParryEvent != null)
			{
				this.OnParryEvent();
			}
		}
	}

	// Token: 0x06003BD8 RID: 15320 RVA: 0x00217020 File Offset: 0x00215420
	public void OnParryComplete()
	{
		this.LeaveGround();
		this.parryManager.state = ArcadePlayerMotor.ParryManager.State.Ready;
		this.velocityManager.y = this.properties.parryPower;
		if (this.OnParrySuccess != null)
		{
			this.OnParrySuccess();
		}
	}

	// Token: 0x06003BD9 RID: 15321 RVA: 0x00217060 File Offset: 0x00215460
	public void OnParryHit()
	{
		base.StartCoroutine(this.parryHit_cr());
	}

	// Token: 0x06003BDA RID: 15322 RVA: 0x0021706F File Offset: 0x0021546F
	public void OnParryCanceled()
	{
		this.Parrying = false;
	}

	// Token: 0x06003BDB RID: 15323 RVA: 0x00217078 File Offset: 0x00215478
	public void OnParryAnimEnd()
	{
		this.Parrying = false;
	}

	// Token: 0x06003BDC RID: 15324 RVA: 0x00217084 File Offset: 0x00215484
	private bool HandleDash()
	{
		if (this.dashManager.state == ArcadePlayerMotor.DashManager.State.Ready && (!this.Grounded || this.dashManager.timeSinceGroundDash > 0.1f) && (base.player.input.actions.GetButtonDown(7) || this.HasBufferedInput(ArcadePlayerMotor.BufferedInput.Dash)))
		{
			AudioManager.Play("player_dash");
			this.dashManager.state = ArcadePlayerMotor.DashManager.State.Start;
			this.dashManager.direction = this.TrueLookDirection.x;
			if (this.jumpManager.state == ArcadePlayerMotor.JumpManager.State.Hold)
			{
				this.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Used;
			}
			if (this.OnDashStartEvent != null)
			{
				this.OnDashStartEvent();
			}
			this.velocityManager.move = 0f;
			return true;
		}
		if (this.dashManager.state == ArcadePlayerMotor.DashManager.State.Start)
		{
			this.dashManager.state = ArcadePlayerMotor.DashManager.State.Dashing;
		}
		if (this.dashManager.state == ArcadePlayerMotor.DashManager.State.Dashing)
		{
			this.velocityManager.dash = this.properties.dashSpeed * (float)this.dashManager.direction;
			this.dashManager.timer += CupheadTime.FixedDelta;
			this.velocityManager.y = 0f;
			this.LookDirection = new Trilean2(this.LookDirection.x, this.dashManager.direction);
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
		if (this.dashManager.state == ArcadePlayerMotor.DashManager.State.End)
		{
			if (this.Grounded)
			{
				this.dashManager.state = ArcadePlayerMotor.DashManager.State.Ready;
				if (this.dashManager.groundDash)
				{
					this.dashManager.timeSinceGroundDash = 0f;
				}
			}
			else
			{
				this.dashManager.groundDash = false;
			}
		}
		return false;
	}

	// Token: 0x06003BDD RID: 15325 RVA: 0x00217298 File Offset: 0x00215698
	private void DashComplete()
	{
		this.dashManager.state = ArcadePlayerMotor.DashManager.State.End;
		this.dashManager.timer = 0f;
		this.velocityManager.dash = 0f;
		if (this.OnDashEndEvent != null)
		{
			this.OnDashEndEvent();
		}
	}

	// Token: 0x06003BDE RID: 15326 RVA: 0x002172E7 File Offset: 0x002156E7
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

	// Token: 0x06003BDF RID: 15327 RVA: 0x00217324 File Offset: 0x00215724
	private void HandleWalking()
	{
		float num = (base.player.controlScheme != ArcadePlayerController.ControlScheme.Normal) ? this.properties.jetpackMoveSpeed : this.properties.moveSpeed;
		float move = (float)base.player.input.GetAxisInt(PlayerInput.Axis.X, false, false) * num;
		this.velocityManager.move = move;
	}

	// Token: 0x06003BE0 RID: 15328 RVA: 0x00217380 File Offset: 0x00215780
	private void HandleLooking()
	{
		if (base.player.levelStarted && this.allowInput)
		{
			int axisInt = base.player.input.GetAxisInt(PlayerInput.Axis.X, false, false);
			int axisInt2 = base.player.input.GetAxisInt(PlayerInput.Axis.Y, false, false);
			this.LookDirection = new Trilean2(axisInt, axisInt2);
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

	// Token: 0x06003BE1 RID: 15329 RVA: 0x0021745C File Offset: 0x0021585C
	private void HandleFalling()
	{
		if (this.Grounded || this.dashManager.IsDashing)
		{
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
	}

	// Token: 0x06003BE2 RID: 15330 RVA: 0x002174F0 File Offset: 0x002158F0
	public float GetTimeUntilLand()
	{
		if (this.Grounded)
		{
			return 0f;
		}
		float num = this.properties.timeToMaxY * 60f;
		float num2 = this.properties.maxSpeedY / num;
		float num3 = ((float)Level.Current.Ground - base.transform.position.y) / (this.velocityManager.maxY * 2f);
		return -(this.velocityManager.y - Mathf.Sqrt(this.velocityManager.y * this.velocityManager.y - 2f * num2 * num3)) / num2;
	}

	// Token: 0x06003BE3 RID: 15331 RVA: 0x00217595 File Offset: 0x00215995
	public float GetTimeUntilDashEnd()
	{
		if (!this.Dashing)
		{
			return 0f;
		}
		return this.properties.dashTime - this.dashManager.timer;
	}

	// Token: 0x06003BE4 RID: 15332 RVA: 0x002175C0 File Offset: 0x002159C0
	private void HandleHit()
	{
		if (this.hitManager.state != ArcadePlayerMotor.HitManager.State.Hit)
		{
			return;
		}
		if (this.hitManager.timer > this.properties.hitStunTime)
		{
			this.hitManager.state = ArcadePlayerMotor.HitManager.State.Inactive;
			this.velocityManager.hit = 0f;
		}
		else
		{
			float value = this.hitManager.timer / this.properties.hitStunTime;
			this.velocityManager.hit = EaseUtils.Ease(this.properties.hitKnockbackEase, this.properties.hitKnockbackPower, 0f, value) * (float)this.hitManager.direction;
			this.hitManager.timer += CupheadTime.FixedDelta;
		}
	}

	// Token: 0x06003BE5 RID: 15333 RVA: 0x00217684 File Offset: 0x00215A84
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hitManager.state = ArcadePlayerMotor.HitManager.State.Hit;
		if (this.OnHitEvent != null)
		{
			this.OnHitEvent();
		}
		this.DashComplete();
		this.velocityManager.Clear();
		this.ResetSuperAndEx();
		int direction = this.TrueLookDirection.x * -1;
		this.hitManager.direction = direction;
		this.LeaveGround();
		this.velocityManager.y = this.properties.hitJumpPower;
		this.hitManager.timer = 0f;
	}

	// Token: 0x06003BE6 RID: 15334 RVA: 0x00217718 File Offset: 0x00215B18
	public void OnRevive(Vector3 pos)
	{
		base.transform.position = pos;
		this.hitManager.state = ArcadePlayerMotor.HitManager.State.KnockedUp;
		this.DashComplete();
		this.velocityManager.Clear();
		this.ResetSuperAndEx();
		this.hitManager.direction = 0;
		this.LeaveGround();
		this.velocityManager.y = this.properties.reviveKnockUpPower;
		this.hitManager.timer = 0f;
	}

	// Token: 0x06003BE7 RID: 15335 RVA: 0x0021778C File Offset: 0x00215B8C
	private void RocketInput()
	{
		this.HandleRocketRotation();
		this.HandleRocketBoost();
		if (!this.HandleDash() && this.hitManager.state == ArcadePlayerMotor.HitManager.State.Hit)
		{
			this.HandleHit();
		}
	}

	// Token: 0x06003BE8 RID: 15336 RVA: 0x002177CC File Offset: 0x00215BCC
	private void HandleJetpackJump()
	{
		if (base.player.input.actions.GetButtonDown(2))
		{
			this.jumpManager.state = ArcadePlayerMotor.JumpManager.State.Hold;
			this.LeaveGround();
			this.velocityManager.y = this.properties.jetpackAcceleration;
			this.jumpManager.timer = CupheadTime.FixedDelta;
		}
		else if (this.velocityManager.y < this.properties.jetpackGravityMax)
		{
			this.velocityManager.y += this.properties.jetpackGravity;
		}
	}

	// Token: 0x06003BE9 RID: 15337 RVA: 0x0021786C File Offset: 0x00215C6C
	private void HandleRocketBoost()
	{
		if (base.player.input.actions.GetButton(2))
		{
			if (this.rocketSpeed < this.properties.moveSpeed)
			{
				this.rocketSpeed += this.properties.rocketAcceleration;
			}
			else
			{
				this.rocketSpeed = this.properties.moveSpeed;
			}
		}
		else if (this.rocketSpeed > 0f)
		{
			this.rocketSpeed -= this.properties.rocketAcceleration;
		}
		else
		{
			this.rocketSpeed = 0f;
		}
		base.transform.position += base.transform.up.normalized * this.rocketSpeed * CupheadTime.FixedDelta;
	}

	// Token: 0x06003BEA RID: 15338 RVA: 0x00217953 File Offset: 0x00215D53
	private void HandleRocketRotation()
	{
		base.transform.Rotate(0f, 0f, this.properties.rocketRotation * (float)(-(float)base.player.input.GetAxisInt(PlayerInput.Axis.X, false, false)) * CupheadTime.FixedDelta, Space.Self);
	}

	// Token: 0x06003BEB RID: 15339 RVA: 0x00217992 File Offset: 0x00215D92
	public void AddForce(ArcadePlayerMotor.VelocityManager.Force force)
	{
		this.velocityManager.AddForce(force);
	}

	// Token: 0x06003BEC RID: 15340 RVA: 0x002179A0 File Offset: 0x00215DA0
	public void RemoveForce(ArcadePlayerMotor.VelocityManager.Force force)
	{
		this.velocityManager.RemoveForce(force);
	}

	// Token: 0x06003BED RID: 15341 RVA: 0x002179AE File Offset: 0x00215DAE
	private void ClearParent()
	{
		if (base.transform.parent != null)
		{
			base.transform.parent.GetComponent<LevelPlatform>().OnPlayerExit(base.transform);
		}
		base.transform.parent = null;
	}

	// Token: 0x06003BEE RID: 15342 RVA: 0x002179F0 File Offset: 0x00215DF0
	private IEnumerator parryHit_cr()
	{
		CupheadTime.GlobalSpeed = 1f;
		this.velocityManager.Clear();
		yield return null;
		PauseManager.Unpause();
		this.velocityManager.Clear();
		CupheadTime.GlobalSpeed = 1f;
		yield break;
	}

	// Token: 0x04004323 RID: 17187
	[SerializeField]
	private ArcadePlayerMotor.Properties properties;

	// Token: 0x04004324 RID: 17188
	private Vector2 lastPositionFixed;

	// Token: 0x04004325 RID: 17189
	private Vector2 lastPosition;

	// Token: 0x04004326 RID: 17190
	private ArcadePlayerMotor.VelocityManager velocityManager;

	// Token: 0x04004327 RID: 17191
	private ArcadePlayerMotor.JumpManager jumpManager;

	// Token: 0x04004328 RID: 17192
	private ArcadePlayerMotor.DashManager dashManager;

	// Token: 0x04004329 RID: 17193
	private ArcadePlayerMotor.ParryManager parryManager;

	// Token: 0x0400432A RID: 17194
	private ArcadePlayerMotor.DirectionManager directionManager;

	// Token: 0x0400432B RID: 17195
	private ArcadePlayerMotor.PlatformManager platformManager;

	// Token: 0x0400432C RID: 17196
	private ArcadePlayerMotor.HitManager hitManager;

	// Token: 0x0400432D RID: 17197
	private ArcadePlayerMotor.SuperManager superManager;

	// Token: 0x0400432E RID: 17198
	private ArcadePlayerMotor.BoundsManager boundsManager;

	// Token: 0x0400432F RID: 17199
	private bool allowInput;

	// Token: 0x04004330 RID: 17200
	private bool allowFalling;

	// Token: 0x04004331 RID: 17201
	private float rocketSpeed;

	// Token: 0x04004339 RID: 17209
	private const float RAY_DISTANCE = 2000f;

	// Token: 0x0400433A RID: 17210
	private const float MAX_GROUNDED_FALL_DISTANCE = 30f;

	// Token: 0x0400433B RID: 17211
	private readonly int wallMask = 262144;

	// Token: 0x0400433C RID: 17212
	private readonly int ceilingMask = 524288;

	// Token: 0x0400433D RID: 17213
	private readonly int groundMask = 1048576;

	// Token: 0x0400433E RID: 17214
	private ArcadePlayerWeaponManager.Pose exFirePose;

	// Token: 0x0400433F RID: 17215
	private ArcadePlayerMotor.BufferedInput bufferedInput;

	// Token: 0x04004340 RID: 17216
	private float timeSinceInputBuffered = 0.134f;

	// Token: 0x020009E0 RID: 2528
	private enum RaycastAxis
	{
		// Token: 0x04004342 RID: 17218
		X,
		// Token: 0x04004343 RID: 17219
		Y
	}

	// Token: 0x020009E1 RID: 2529
	public enum BufferedInput
	{
		// Token: 0x04004345 RID: 17221
		Jump,
		// Token: 0x04004346 RID: 17222
		Dash,
		// Token: 0x04004347 RID: 17223
		Super
	}

	// Token: 0x020009E2 RID: 2530
	public class Properties
	{
		// Token: 0x04004348 RID: 17224
		public float rocketRotation = 300f;

		// Token: 0x04004349 RID: 17225
		public float rocketMaxSpeed = 400f;

		// Token: 0x0400434A RID: 17226
		public float rocketAcceleration = 2.5f;

		// Token: 0x0400434B RID: 17227
		public float jetpackAcceleration = -0.1f;

		// Token: 0x0400434C RID: 17228
		public float jetpackGravity = 0.001f;

		// Token: 0x0400434D RID: 17229
		public float jetpackGravityMax = 0.1f;

		// Token: 0x0400434E RID: 17230
		private const float speedScale = 0.75f;

		// Token: 0x0400434F RID: 17231
		public float moveSpeed = 367.5f;

		// Token: 0x04004350 RID: 17232
		public float jetpackMoveSpeed = 187.5f;

		// Token: 0x04004351 RID: 17233
		public float maxSpeedY = 1215f;

		// Token: 0x04004352 RID: 17234
		public float timeToMaxY = 7.3f;

		// Token: 0x04004353 RID: 17235
		public EaseUtils.EaseType yEase = EaseUtils.EaseType.linear;

		// Token: 0x04004354 RID: 17236
		public float jumpHoldMin = 0.01f;

		// Token: 0x04004355 RID: 17237
		public float jumpHoldMax = 0.16f;

		// Token: 0x04004356 RID: 17238
		[Range(0f, -1f)]
		public float jumpPower = -0.56624997f;

		// Token: 0x04004357 RID: 17239
		public float dashSpeed = 825f;

		// Token: 0x04004358 RID: 17240
		public float dashTime = 0.3f;

		// Token: 0x04004359 RID: 17241
		public float dashEndTime = 0.21f;

		// Token: 0x0400435A RID: 17242
		public EaseUtils.EaseType dashEase = EaseUtils.EaseType.easeOutSine;

		// Token: 0x0400435B RID: 17243
		public float platformIgnoreTime = 1f;

		// Token: 0x0400435C RID: 17244
		public float hitStunTime = 0.3f;

		// Token: 0x0400435D RID: 17245
		public float hitFalloff = 0.25f;

		// Token: 0x0400435E RID: 17246
		[Range(0f, -1f)]
		public float hitJumpPower = -0.6f;

		// Token: 0x0400435F RID: 17247
		public float hitKnockbackPower = 225f;

		// Token: 0x04004360 RID: 17248
		public EaseUtils.EaseType hitKnockbackEase = EaseUtils.EaseType.linear;

		// Token: 0x04004361 RID: 17249
		public float knockUpStunTime = 0.2f;

		// Token: 0x04004362 RID: 17250
		public float parryPower = -0.75f;

		// Token: 0x04004363 RID: 17251
		public float deathSpeed = 3.75f;

		// Token: 0x04004364 RID: 17252
		public float reviveKnockUpPower = -0.75f;

		// Token: 0x04004365 RID: 17253
		public float exKnockback = 172.5f;

		// Token: 0x04004366 RID: 17254
		public float superKnockUp = -0.45000002f;
	}

	// Token: 0x020009E3 RID: 2531
	public class VelocityManager
	{
		// Token: 0x06003BF0 RID: 15344 RVA: 0x00217B60 File Offset: 0x00215F60
		public VelocityManager(ArcadePlayerMotor motor, float maxY, EaseUtils.EaseType yEase)
		{
			this.maxY = maxY;
			this.yEase = yEase;
			this.forces = new List<ArcadePlayerMotor.VelocityManager.Force>();
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06003BF1 RID: 15345 RVA: 0x00217B81 File Offset: 0x00215F81
		// (set) Token: 0x06003BF2 RID: 15346 RVA: 0x00217B89 File Offset: 0x00215F89
		public float GroundForce { get; private set; }

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06003BF3 RID: 15347 RVA: 0x00217B92 File Offset: 0x00215F92
		// (set) Token: 0x06003BF4 RID: 15348 RVA: 0x00217B9A File Offset: 0x00215F9A
		public float AirForce { get; private set; }

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06003BF5 RID: 15349 RVA: 0x00217BA3 File Offset: 0x00215FA3
		// (set) Token: 0x06003BF6 RID: 15350 RVA: 0x00217BC6 File Offset: 0x00215FC6
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

		// Token: 0x06003BF7 RID: 15351 RVA: 0x00217BE0 File Offset: 0x00215FE0
		public void Calculate()
		{
			this.GroundForce = 0f;
			this.AirForce = 0f;
			foreach (ArcadePlayerMotor.VelocityManager.Force force in this.forces)
			{
				if (force.enabled)
				{
					ArcadePlayerMotor.VelocityManager.Force.Type type = force.type;
					if (type != ArcadePlayerMotor.VelocityManager.Force.Type.All)
					{
						if (type != ArcadePlayerMotor.VelocityManager.Force.Type.Air)
						{
							if (type == ArcadePlayerMotor.VelocityManager.Force.Type.Ground)
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
				}
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06003BF8 RID: 15352 RVA: 0x00217CD8 File Offset: 0x002160D8
		public Vector2 Total
		{
			get
			{
				float value = this.y / 2f + 0.5f;
				Vector2 result = default(Vector2);
				result.y = EaseUtils.Ease(this.yEase, this.maxY, -this.maxY, value);
				result.x += this.move + this.dash + this.hit;
				return result;
			}
		}

		// Token: 0x06003BF9 RID: 15353 RVA: 0x00217D43 File Offset: 0x00216143
		public void Clear()
		{
			this.move = 0f;
			this.dash = 0f;
			this.hit = 0f;
			this.y = 0f;
		}

		// Token: 0x06003BFA RID: 15354 RVA: 0x00217D71 File Offset: 0x00216171
		public void AddForce(ArcadePlayerMotor.VelocityManager.Force force)
		{
			if (this.forces.Contains(force))
			{
				return;
			}
			this.forces.Add(force);
		}

		// Token: 0x06003BFB RID: 15355 RVA: 0x00217D91 File Offset: 0x00216191
		public void RemoveForce(ArcadePlayerMotor.VelocityManager.Force force)
		{
			if (this.forces.Contains(force))
			{
				this.forces.Remove(force);
			}
		}

		// Token: 0x04004369 RID: 17257
		public float move;

		// Token: 0x0400436A RID: 17258
		public float dash;

		// Token: 0x0400436B RID: 17259
		public float hit;

		// Token: 0x0400436C RID: 17260
		private List<ArcadePlayerMotor.VelocityManager.Force> forces;

		// Token: 0x0400436D RID: 17261
		private EaseUtils.EaseType yEase;

		// Token: 0x0400436E RID: 17262
		public float maxY;

		// Token: 0x0400436F RID: 17263
		private float _y;

		// Token: 0x020009E4 RID: 2532
		public class Force
		{
			// Token: 0x06003BFC RID: 15356 RVA: 0x00217DB1 File Offset: 0x002161B1
			public Force()
			{
				this.type = ArcadePlayerMotor.VelocityManager.Force.Type.All;
				this.value = 0f;
			}

			// Token: 0x06003BFD RID: 15357 RVA: 0x00217DD2 File Offset: 0x002161D2
			public Force(ArcadePlayerMotor.VelocityManager.Force.Type type)
			{
				this.type = type;
				this.value = 0f;
			}

			// Token: 0x06003BFE RID: 15358 RVA: 0x00217DF3 File Offset: 0x002161F3
			public Force(ArcadePlayerMotor.VelocityManager.Force.Type type, float force)
			{
				this.type = type;
				this.value = force;
			}

			// Token: 0x04004370 RID: 17264
			public bool enabled = true;

			// Token: 0x04004371 RID: 17265
			public readonly ArcadePlayerMotor.VelocityManager.Force.Type type;

			// Token: 0x04004372 RID: 17266
			public float value;

			// Token: 0x020009E5 RID: 2533
			public enum Type
			{
				// Token: 0x04004374 RID: 17268
				All,
				// Token: 0x04004375 RID: 17269
				Ground,
				// Token: 0x04004376 RID: 17270
				Air
			}
		}
	}

	// Token: 0x020009E6 RID: 2534
	public class JumpManager
	{
		// Token: 0x04004377 RID: 17271
		public ArcadePlayerMotor.JumpManager.State state;

		// Token: 0x04004378 RID: 17272
		public float timer;

		// Token: 0x04004379 RID: 17273
		public float timeSinceDownJump = 1000f;

		// Token: 0x0400437A RID: 17274
		public bool ableToLand;

		// Token: 0x020009E7 RID: 2535
		public enum State
		{
			// Token: 0x0400437C RID: 17276
			Ready,
			// Token: 0x0400437D RID: 17277
			Hold,
			// Token: 0x0400437E RID: 17278
			Used
		}
	}

	// Token: 0x020009E8 RID: 2536
	public class DashManager
	{
		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06003C01 RID: 15361 RVA: 0x00217E38 File Offset: 0x00216238
		public bool IsDashing
		{
			get
			{
				ArcadePlayerMotor.DashManager.State state = this.state;
				return state == ArcadePlayerMotor.DashManager.State.Start || state == ArcadePlayerMotor.DashManager.State.Dashing || state == ArcadePlayerMotor.DashManager.State.Ending;
			}
		}

		// Token: 0x0400437F RID: 17279
		public ArcadePlayerMotor.DashManager.State state;

		// Token: 0x04004380 RID: 17280
		public int direction;

		// Token: 0x04004381 RID: 17281
		public float timer;

		// Token: 0x04004382 RID: 17282
		public const float DASH_COOLDOWN_DURATION = 0.1f;

		// Token: 0x04004383 RID: 17283
		public float timeSinceGroundDash = 0.1f;

		// Token: 0x04004384 RID: 17284
		public bool groundDash;

		// Token: 0x020009E9 RID: 2537
		public enum State
		{
			// Token: 0x04004386 RID: 17286
			Ready,
			// Token: 0x04004387 RID: 17287
			Start,
			// Token: 0x04004388 RID: 17288
			Dashing,
			// Token: 0x04004389 RID: 17289
			Ending,
			// Token: 0x0400438A RID: 17290
			End
		}
	}

	// Token: 0x020009EA RID: 2538
	public class ParryManager
	{
		// Token: 0x0400438B RID: 17291
		public ArcadePlayerMotor.ParryManager.State state;

		// Token: 0x020009EB RID: 2539
		public enum State
		{
			// Token: 0x0400438D RID: 17293
			Ready,
			// Token: 0x0400438E RID: 17294
			NotReady
		}
	}

	// Token: 0x020009EC RID: 2540
	public class PlatformManager
	{
		// Token: 0x06003C03 RID: 15363 RVA: 0x00217E71 File Offset: 0x00216271
		public PlatformManager(ArcadePlayerMotor motor)
		{
			this.ignoredPlatforms = new List<Transform>();
			this.motor = motor;
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06003C04 RID: 15364 RVA: 0x00217E8B File Offset: 0x0021628B
		public bool OnPlatform
		{
			get
			{
				return this.motor.transform.parent != null;
			}
		}

		// Token: 0x06003C05 RID: 15365 RVA: 0x00217EA3 File Offset: 0x002162A3
		public void Ignore(Transform platform)
		{
			this.StopCoroutine();
			this.ignoreCoroutine = this.ignorePlatform_cr(platform);
			this.motor.StartCoroutine(this.ignoreCoroutine);
		}

		// Token: 0x06003C06 RID: 15366 RVA: 0x00217ECA File Offset: 0x002162CA
		public void StopCoroutine()
		{
			if (this.ignoreCoroutine != null)
			{
				this.motor.StopCoroutine(this.ignoreCoroutine);
			}
			this.ignoreCoroutine = null;
		}

		// Token: 0x06003C07 RID: 15367 RVA: 0x00217EEF File Offset: 0x002162EF
		public void Add(Transform platform)
		{
			this.ignoredPlatforms.Add(platform);
		}

		// Token: 0x06003C08 RID: 15368 RVA: 0x00217EFD File Offset: 0x002162FD
		public void Remove(Transform platform)
		{
			this.ignoredPlatforms.Remove(platform);
		}

		// Token: 0x06003C09 RID: 15369 RVA: 0x00217F0C File Offset: 0x0021630C
		public bool IsPlatformIgnored(Transform platform)
		{
			return this.ignoredPlatforms.Contains(platform);
		}

		// Token: 0x06003C0A RID: 15370 RVA: 0x00217F1A File Offset: 0x0021631A
		public void ResetAll()
		{
			this.StopCoroutine();
			this.ignoredPlatforms = new List<Transform>();
		}

		// Token: 0x06003C0B RID: 15371 RVA: 0x00217F30 File Offset: 0x00216330
		private IEnumerator ignorePlatform_cr(Transform platform)
		{
			this.Add(platform);
			yield return CupheadTime.WaitForSeconds(this.motor, this.motor.properties.platformIgnoreTime);
			this.Remove(platform);
			yield break;
		}

		// Token: 0x0400438F RID: 17295
		private List<Transform> ignoredPlatforms;

		// Token: 0x04004390 RID: 17296
		private ArcadePlayerMotor motor;

		// Token: 0x04004391 RID: 17297
		private IEnumerator ignoreCoroutine;
	}

	// Token: 0x020009ED RID: 2541
	public class DirectionManager
	{
		// Token: 0x06003C0C RID: 15372 RVA: 0x0021801C File Offset: 0x0021641C
		public DirectionManager()
		{
			this.Reset();
		}

		// Token: 0x06003C0D RID: 15373 RVA: 0x00218056 File Offset: 0x00216456
		public void Reset()
		{
			this.up.Reset();
			this.down.Reset();
			this.left.Reset();
			this.right.Reset();
		}

		// Token: 0x04004392 RID: 17298
		public ArcadePlayerMotor.DirectionManager.Hit up = new ArcadePlayerMotor.DirectionManager.Hit();

		// Token: 0x04004393 RID: 17299
		public ArcadePlayerMotor.DirectionManager.Hit down = new ArcadePlayerMotor.DirectionManager.Hit();

		// Token: 0x04004394 RID: 17300
		public ArcadePlayerMotor.DirectionManager.Hit left = new ArcadePlayerMotor.DirectionManager.Hit();

		// Token: 0x04004395 RID: 17301
		public ArcadePlayerMotor.DirectionManager.Hit right = new ArcadePlayerMotor.DirectionManager.Hit();

		// Token: 0x020009EE RID: 2542
		public class Hit
		{
			// Token: 0x06003C0E RID: 15374 RVA: 0x00218084 File Offset: 0x00216484
			public Hit()
			{
				this.Reset();
			}

			// Token: 0x06003C0F RID: 15375 RVA: 0x00218092 File Offset: 0x00216492
			public Hit(bool able, Vector2 pos, GameObject gameObject, float distance)
			{
				this.able = able;
				this.pos = pos;
				this.gameObject = gameObject;
				this.distance = distance;
			}

			// Token: 0x06003C10 RID: 15376 RVA: 0x002180B7 File Offset: 0x002164B7
			public void Reset()
			{
				this.able = true;
				this.pos = Vector2.zero;
				this.gameObject = null;
				this.distance = -1f;
			}

			// Token: 0x04004396 RID: 17302
			public bool able;

			// Token: 0x04004397 RID: 17303
			public Vector2 pos;

			// Token: 0x04004398 RID: 17304
			public GameObject gameObject;

			// Token: 0x04004399 RID: 17305
			public float distance;
		}
	}

	// Token: 0x020009EF RID: 2543
	public class HitManager
	{
		// Token: 0x06003C12 RID: 15378 RVA: 0x002180E5 File Offset: 0x002164E5
		public void Reset()
		{
			this.state = ArcadePlayerMotor.HitManager.State.Inactive;
			this.timer = 0f;
			this.direction = 0;
		}

		// Token: 0x0400439A RID: 17306
		public ArcadePlayerMotor.HitManager.State state;

		// Token: 0x0400439B RID: 17307
		public float timer;

		// Token: 0x0400439C RID: 17308
		public int direction;

		// Token: 0x020009F0 RID: 2544
		public enum State
		{
			// Token: 0x0400439E RID: 17310
			Inactive,
			// Token: 0x0400439F RID: 17311
			Hit,
			// Token: 0x040043A0 RID: 17312
			KnockedUp
		}
	}

	// Token: 0x020009F1 RID: 2545
	public class SuperManager
	{
		// Token: 0x040043A1 RID: 17313
		public ArcadePlayerMotor.SuperManager.State state;

		// Token: 0x020009F2 RID: 2546
		public enum State
		{
			// Token: 0x040043A3 RID: 17315
			Ready,
			// Token: 0x040043A4 RID: 17316
			Ex,
			// Token: 0x040043A5 RID: 17317
			Super
		}
	}

	// Token: 0x020009F3 RID: 2547
	public class BoundsManager
	{
		// Token: 0x06003C14 RID: 15380 RVA: 0x00218108 File Offset: 0x00216508
		public BoundsManager(Transform playerTransform)
		{
			this.transform = playerTransform;
			this.boxCollider = (this.transform.GetComponent<Collider2D>() as BoxCollider2D);
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06003C15 RID: 15381 RVA: 0x00218130 File Offset: 0x00216530
		public Vector3 Top
		{
			get
			{
				return new Vector3(this.Center.x, this.Center.y + this.boxCollider.size.y / 2f, 0f);
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06003C16 RID: 15382 RVA: 0x00218180 File Offset: 0x00216580
		public Vector3 TopLeft
		{
			get
			{
				return new Vector3(this.Center.x - this.boxCollider.size.x / 2f, this.Center.y + this.boxCollider.size.y / 2f, 0f);
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06003C17 RID: 15383 RVA: 0x002181E8 File Offset: 0x002165E8
		public Vector3 TopRight
		{
			get
			{
				return new Vector3(this.Center.x + this.boxCollider.size.x / 2f, this.Center.y + this.boxCollider.size.y / 2f, 0f);
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06003C18 RID: 15384 RVA: 0x00218250 File Offset: 0x00216650
		public Vector3 CenterLeft
		{
			get
			{
				return new Vector3(this.Center.x - this.boxCollider.size.x / 2f, this.Center.y, 0f);
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06003C19 RID: 15385 RVA: 0x002182A0 File Offset: 0x002166A0
		public Vector3 CenterRight
		{
			get
			{
				return new Vector3(this.Center.x + this.boxCollider.size.x / 2f, this.Center.y, 0f);
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06003C1A RID: 15386 RVA: 0x002182ED File Offset: 0x002166ED
		public Vector2 Center
		{
			get
			{
				return this.transform.position + this.boxCollider.offset;
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06003C1B RID: 15387 RVA: 0x00218310 File Offset: 0x00216710
		public Vector3 Bottom
		{
			get
			{
				return new Vector3(this.Center.x, this.Center.y - this.boxCollider.size.y / 2f, 0f);
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06003C1C RID: 15388 RVA: 0x00218360 File Offset: 0x00216760
		public Vector3 BottomLeft
		{
			get
			{
				return new Vector3(this.Center.x - this.boxCollider.size.x / 2f, this.Center.y - this.boxCollider.size.y / 2f, 0f);
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06003C1D RID: 15389 RVA: 0x002183C8 File Offset: 0x002167C8
		public Vector3 BottomRight
		{
			get
			{
				return new Vector3(this.Center.x + this.boxCollider.size.x / 2f, this.Center.y - this.boxCollider.size.y / 2f, 0f);
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06003C1E RID: 15390 RVA: 0x00218430 File Offset: 0x00216830
		public float TopY
		{
			get
			{
				return this.Top.y - this.transform.position.y;
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06003C1F RID: 15391 RVA: 0x00218460 File Offset: 0x00216860
		public float BottomY
		{
			get
			{
				return this.Bottom.y - this.transform.position.y;
			}
		}

		// Token: 0x040043A6 RID: 17318
		private readonly Transform transform;

		// Token: 0x040043A7 RID: 17319
		private BoxCollider2D boxCollider;
	}
}
