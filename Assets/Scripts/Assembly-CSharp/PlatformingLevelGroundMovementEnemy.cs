using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000861 RID: 2145
public class PlatformingLevelGroundMovementEnemy : AbstractPlatformingLevelEnemy
{
	// Token: 0x060031D5 RID: 12757 RVA: 0x001D1740 File Offset: 0x001CFB40
	public PlatformingLevelGroundMovementEnemy Spawn(Vector3 position, PlatformingLevelGroundMovementEnemy.Direction startDirection, bool destroyEnemyAfterLeavingScreen)
	{
		PlatformingLevelGroundMovementEnemy platformingLevelGroundMovementEnemy = this.InstantiatePrefab<PlatformingLevelGroundMovementEnemy>();
		platformingLevelGroundMovementEnemy.transform.position = position;
		platformingLevelGroundMovementEnemy._destroyEnemyAfterLeavingScreen = destroyEnemyAfterLeavingScreen;
		platformingLevelGroundMovementEnemy._startCondition = AbstractPlatformingLevelEnemy.StartCondition.Instant;
		platformingLevelGroundMovementEnemy._direction = startDirection;
		return platformingLevelGroundMovementEnemy;
	}

	// Token: 0x17000435 RID: 1077
	// (get) Token: 0x060031D6 RID: 12758 RVA: 0x001D1776 File Offset: 0x001CFB76
	public PlatformingLevelGroundMovementEnemy.Direction direction
	{
		get
		{
			return this._direction;
		}
	}

	// Token: 0x17000436 RID: 1078
	// (get) Token: 0x060031D7 RID: 12759 RVA: 0x001D177E File Offset: 0x001CFB7E
	// (set) Token: 0x060031D8 RID: 12760 RVA: 0x001D1786 File Offset: 0x001CFB86
	public bool Grounded { get; private set; }

	// Token: 0x17000437 RID: 1079
	// (get) Token: 0x060031D9 RID: 12761 RVA: 0x001D178F File Offset: 0x001CFB8F
	protected virtual Collider2D collider
	{
		get
		{
			return this._collider;
		}
	}

	// Token: 0x060031DA RID: 12762 RVA: 0x001D1798 File Offset: 0x001CFB98
	protected override void Awake()
	{
		base.Awake();
		this._collider = base.GetComponent<Collider2D>();
		this.directionManager = new PlatformingLevelGroundMovementEnemy.DirectionManager();
		this.jumpManager = new PlatformingLevelGroundMovementEnemy.JumpManager();
		this.timeSinceTurn = 10000f;
		if (this.shadow != null)
		{
			this.shadow.parent = null;
		}
		this.SetTurnTarget("Turn");
	}

	// Token: 0x060031DB RID: 12763 RVA: 0x001D1800 File Offset: 0x001CFC00
	protected override void OnStart()
	{
	}

	// Token: 0x060031DC RID: 12764 RVA: 0x001D1804 File Offset: 0x001CFC04
	public void GoToGround(bool despawnOnPit = true, string groundStateName = "Run")
	{
		base.animator.Play(groundStateName);
		Bounds bounds = this.collider.bounds;
		Vector2 b = bounds.center - base.transform.position;
		if (!this.gravityReversed)
		{
			this.hits = this.BoxCastAll(new Vector2(bounds.size.x, 1f), Vector2.down, this.groundMask, new Vector2(0f, -bounds.size.y / 4f));
		}
		else
		{
			this.hits = this.BoxCastAll(new Vector2(bounds.size.x, 1f), Vector2.up, this.ceilingMask, new Vector2(0f, bounds.size.y));
		}
		Vector2 vector = base.transform.position;
		bool flag = false;
		foreach (RaycastHit2D raycastHit2D in this.hits)
		{
			LevelPlatform component = raycastHit2D.collider.gameObject.GetComponent<LevelPlatform>();
			if (raycastHit2D.collider != null && (this.canSpawnOnPlatforms || component == null || !component.canFallThrough))
			{
				vector = raycastHit2D.point;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		base.transform.SetPosition(null, new float?(vector.y), null);
		this.HandleRaycasts();
		this.jumpManager.ableToLand = true;
		this.OnGrounded();
		if (despawnOnPit)
		{
			Vector2 a = base.transform.position + b + new Vector2((this.direction != PlatformingLevelGroundMovementEnemy.Direction.Left) ? (-bounds.size.x / 2f) : (bounds.size.x / 2f), this.turnaroundDistance / 2f - bounds.size.y / 2f);
			Vector2 b2 = base.transform.position + b + new Vector2((this.direction != PlatformingLevelGroundMovementEnemy.Direction.Left) ? this.turnaroundDistance : (-this.turnaroundDistance), this.turnaroundDistance / 2f - bounds.size.y / 2f);
			for (int j = 0; j <= 10; j++)
			{
				float t = (float)j / 10f;
				if (!this.gravityReversed)
				{
					if (Physics2D.Raycast(Vector2.Lerp(a, b2, t), Vector2.down, 30f + this.turnaroundDistance, this.groundMask).collider == null)
					{
						UnityEngine.Object.Destroy(base.gameObject);
						return;
					}
				}
				else if (Physics2D.Raycast(Vector2.Lerp(a, b2, t), Vector2.up, 30f + this.turnaroundDistance, this.ceilingMask).collider == null)
				{
					UnityEngine.Object.Destroy(base.gameObject);
					return;
				}
			}
		}
	}

	// Token: 0x060031DD RID: 12765 RVA: 0x001D1B92 File Offset: 0x001CFF92
	public void Float(bool playAnim = true)
	{
		if (playAnim)
		{
			base.animator.Play("Float", 0, UnityEngine.Random.Range(0f, 1f));
		}
		this.playFloatAnim = playAnim;
		this.floating = true;
	}

	// Token: 0x060031DE RID: 12766 RVA: 0x001D1BC8 File Offset: 0x001CFFC8
	protected override void Update()
	{
		base.Update();
		this.CalculateDirection();
		this.CalculateRender();
		if (this.shadow != null)
		{
			this.UpdateShadow();
		}
	}

	// Token: 0x060031DF RID: 12767 RVA: 0x001D1BF3 File Offset: 0x001CFFF3
	protected virtual void FixedUpdate()
	{
		if (base.Dead || base.GetComponent<DamageReceiver>().IsHitPaused)
		{
			return;
		}
		this.HandleRaycasts();
		this.HandleFalling();
		this.Move();
	}

	// Token: 0x060031E0 RID: 12768 RVA: 0x001D1C24 File Offset: 0x001D0024
	private void HandleFalling()
	{
		if (this.Grounded)
		{
			return;
		}
		if (this.floating)
		{
			this.velocity = new Vector2(0f, -base.Properties.floatSpeed);
		}
		else if (!this.gravityReversed)
		{
			this.velocity.y = this.velocity.y - base.Properties.gravity * CupheadTime.FixedDelta;
			this.jumpManager.ableToLand = (this.velocity.y < 0f);
		}
		else
		{
			this.velocity.y = this.velocity.y + base.Properties.gravity * CupheadTime.FixedDelta;
			this.jumpManager.ableToLand = (this.velocity.y < 0f);
		}
	}

	// Token: 0x060031E1 RID: 12769 RVA: 0x001D1CF9 File Offset: 0x001D00F9
	protected virtual float GetMoveSpeed()
	{
		if (this.moveSpeed == 0f)
		{
			this.moveSpeed = base.Properties.MoveSpeed;
		}
		return this.moveSpeed;
	}

	// Token: 0x060031E2 RID: 12770 RVA: 0x001D1D22 File Offset: 0x001D0122
	protected virtual void SetMoveSpeed(float moveSpeed)
	{
		this.moveSpeed = moveSpeed;
	}

	// Token: 0x060031E3 RID: 12771 RVA: 0x001D1D2C File Offset: 0x001D012C
	private void Move()
	{
		if (this.turning || this.landing || (this.jumping && this.Grounded))
		{
			return;
		}
		this.timeSinceTurn += CupheadTime.FixedDelta;
		float num = (float)((this._direction != PlatformingLevelGroundMovementEnemy.Direction.Right) ? -1 : 1);
		if (this.jumpManager.state == PlatformingLevelGroundMovementEnemy.JumpManager.State.Ready && !this.floating)
		{
			this.velocity.x = this.GetMoveSpeed() * num;
		}
		base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
		if (!this.gravityReversed)
		{
			if (this.Grounded && base.transform.position.y - this.directionManager.down.pos.y < 30f)
			{
				Vector2 v = base.transform.position;
				v.y = this.directionManager.down.pos.y;
				base.transform.position = v;
			}
		}
		else if (this.Grounded && base.transform.position.y + this.directionManager.up.pos.y > 30f)
		{
			Vector2 v2 = base.transform.position;
			v2.y = this.directionManager.up.pos.y;
			base.transform.position = v2;
		}
	}

	// Token: 0x060031E4 RID: 12772 RVA: 0x001D1EF8 File Offset: 0x001D02F8
	private void CalculateRender()
	{
		if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position) && !this._enteredScreen)
		{
			this._enteredScreen = true;
		}
		if (this._enteredScreen && this._destroyEnemyAfterLeavingScreen && !CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(100f, 100f)))
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		if (PlatformingLevel.Current != null && (base.transform.position.x < (float)PlatformingLevel.Current.Left - 100f || base.transform.position.x > (float)PlatformingLevel.Current.Right + 100f || base.transform.position.y < (float)PlatformingLevel.Current.Ground - 100f))
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060031E5 RID: 12773 RVA: 0x001D201B File Offset: 0x001D041B
	private void LateUpdate()
	{
		this.CalculateDirection();
	}

	// Token: 0x060031E6 RID: 12774 RVA: 0x001D2024 File Offset: 0x001D0424
	protected virtual void CalculateDirection()
	{
		if (this._direction == PlatformingLevelGroundMovementEnemy.Direction.Right)
		{
			base.transform.SetScale(new float?(-1f), null, null);
		}
		else
		{
			base.transform.SetScale(new float?(1f), null, null);
		}
	}

	// Token: 0x060031E7 RID: 12775 RVA: 0x001D2090 File Offset: 0x001D0490
	protected override void Die()
	{
		if (this.shadow != null)
		{
			UnityEngine.Object.Destroy(this.shadow.gameObject);
		}
		base.Die();
	}

	// Token: 0x060031E8 RID: 12776 RVA: 0x001D20B9 File Offset: 0x001D04B9
	protected virtual Coroutine Turn()
	{
		this.turning = true;
		this.timeSinceTurn = 0f;
		return base.StartCoroutine(this.turn_cr());
	}

	// Token: 0x060031E9 RID: 12777 RVA: 0x001D20DC File Offset: 0x001D04DC
	private IEnumerator turn_cr()
	{
		if (this.hasTurnAnimation && base.animator != null)
		{
			base.animator.Play("Turn");
			int target = Animator.StringToHash(base.animator.GetLayerName(0) + "." + this.turnTarget);
			while (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash != target)
			{
				yield return null;
			}
			float animLength = base.animator.GetCurrentAnimatorStateInfo(0).length;
			while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= (animLength - CupheadTime.Delta) / animLength)
			{
				yield return null;
			}
		}
		if (this._direction == PlatformingLevelGroundMovementEnemy.Direction.Right)
		{
			this._direction = PlatformingLevelGroundMovementEnemy.Direction.Left;
		}
		else
		{
			this._direction = PlatformingLevelGroundMovementEnemy.Direction.Right;
		}
		this.CalculateDirection();
		this.turning = false;
		yield break;
	}

	// Token: 0x060031EA RID: 12778 RVA: 0x001D20F7 File Offset: 0x001D04F7
	protected virtual void SetTurnTarget(string turnTarget)
	{
		this.turnTarget = turnTarget;
	}

	// Token: 0x060031EB RID: 12779 RVA: 0x001D2100 File Offset: 0x001D0500
	private IEnumerator floatLand_cr()
	{
		this.floating = false;
		this.landing = true;
		if (!this.lockDirectionWhenLanding)
		{
			this._direction = ((PlayerManager.GetNext().center.x <= base.transform.position.x) ? PlatformingLevelGroundMovementEnemy.Direction.Left : PlatformingLevelGroundMovementEnemy.Direction.Right);
		}
		base.transform.SetPosition(null, new float?(this.directionManager.down.pos.y), null);
		if (this.playFloatAnim)
		{
			base.animator.Play("Land");
		}
		this.playFloatAnim = false;
		yield return base.animator.WaitForAnimationToEnd(this, "Land", false, true);
		this.velocity.y = 0f;
		this.landing = false;
		yield break;
	}

	// Token: 0x060031EC RID: 12780 RVA: 0x001D211B File Offset: 0x001D051B
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.jumpLandEffectPrefab = null;
	}

	// Token: 0x060031ED RID: 12781 RVA: 0x001D212C File Offset: 0x001D052C
	public void Jump()
	{
		if (base.Properties.canJump && this.jumpManager.state == PlatformingLevelGroundMovementEnemy.JumpManager.State.Ready && !this.turning)
		{
			this.jumpManager.state = PlatformingLevelGroundMovementEnemy.JumpManager.State.Used;
			base.StartCoroutine(this.jump_cr());
			this.jumping = true;
		}
	}

	// Token: 0x060031EE RID: 12782 RVA: 0x001D2184 File Offset: 0x001D0584
	private IEnumerator jump_cr()
	{
		if (this.hasJumpAnimation && base.animator != null)
		{
			base.animator.Play("Jump");
			int target = Animator.StringToHash(base.animator.GetLayerName(0) + ".Jump");
			while (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash != target)
			{
				yield return null;
			}
			float animLength = base.animator.GetCurrentAnimatorStateInfo(0).length;
			while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= (animLength - CupheadTime.Delta) / animLength)
			{
				yield return null;
			}
		}
		float directionSign = (float)((this._direction != PlatformingLevelGroundMovementEnemy.Direction.Right) ? -1 : 1);
		float timeToApex = Mathf.Sqrt(2f * base.Properties.jumpHeight / base.Properties.gravity);
		float x = (!this.manuallySetJumpX) ? base.Properties.jumpLength : this.GetMoveSpeed();
		this.LeaveGround();
		this.velocity.y = base.Properties.gravity * timeToApex;
		this.velocity.x = directionSign * x / (2f * timeToApex);
		if (this.hasJumpAnimation && base.animator != null)
		{
			yield return CupheadTime.WaitForSeconds(this, timeToApex);
			base.animator.SetTrigger("Apex");
		}
		while (this.jumping)
		{
			yield return null;
		}
		this.landing = true;
		if (this.directionManager.down != null)
		{
			base.transform.SetPosition(null, new float?(this.directionManager.down.pos.y), null);
		}
		if (this.jumpLandEffectPrefab != null)
		{
			this.jumpLandEffectPrefab.Create(base.transform.position);
		}
		if (this.hasJumpAnimation && base.animator != null)
		{
			base.animator.SetTrigger("Land");
			yield return base.animator.WaitForAnimationToEnd(this, "Jump_Land", false, true);
		}
		this.landing = false;
		yield break;
	}

	// Token: 0x060031EF RID: 12783 RVA: 0x001D219F File Offset: 0x001D059F
	private RaycastHit2D BoxCast(Vector2 size, Vector2 direction, int layerMask)
	{
		return this.BoxCast(size, direction, layerMask, Vector2.zero);
	}

	// Token: 0x060031F0 RID: 12784 RVA: 0x001D21B0 File Offset: 0x001D05B0
	private RaycastHit2D BoxCast(Vector2 size, Vector2 direction, int layerMask, Vector2 offset)
	{
		return Physics2D.BoxCast(this.collider.bounds.center + offset, size, 0f, direction, 2000f, layerMask);
	}

	// Token: 0x060031F1 RID: 12785 RVA: 0x001D21F0 File Offset: 0x001D05F0
	private RaycastHit2D[] BoxCastAll(Vector2 size, Vector2 direction, int layerMask, Vector2 offset)
	{
		return Physics2D.BoxCastAll(this.collider.bounds.center + offset, size, 0f, direction, 2000f, layerMask);
	}

	// Token: 0x060031F2 RID: 12786 RVA: 0x001D2230 File Offset: 0x001D0630
	private RaycastHit2D CircleCast(float radius, Vector2 direction, int layerMask)
	{
		return Physics2D.CircleCast(this.collider.bounds.center, radius, direction, 2000f, layerMask);
	}

	// Token: 0x060031F3 RID: 12787 RVA: 0x001D2262 File Offset: 0x001D0662
	private bool DoesRaycastHitHaveCollider(RaycastHit2D hit)
	{
		return hit.collider != null;
	}

	// Token: 0x060031F4 RID: 12788 RVA: 0x001D2274 File Offset: 0x001D0674
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (Application.isPlaying)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(base.transform.position, 5f);
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(base.transform.position, 5f);
		}
	}

	// Token: 0x060031F5 RID: 12789 RVA: 0x001D22D0 File Offset: 0x001D06D0
	private void HandleRaycasts()
	{
		if (this.fallInPit)
		{
			return;
		}
		bool flag = true;
		if (this.directionManager != null && this.directionManager.up != null)
		{
			flag = this.directionManager.up.able;
		}
		Bounds bounds = this.collider.bounds;
		this.directionManager.Reset();
		RaycastHit2D raycastHit = this.BoxCast(new Vector2(bounds.size.x, 1f), Vector2.up, this.ceilingMask);
		RaycastHit2D raycastHit2 = this.BoxCast(new Vector2(bounds.size.x, 1f), Vector2.down, this.groundMask, new Vector2(base.transform.position.x - bounds.center.x, base.transform.position.y + 30f - bounds.center.y));
		RaycastHit2D raycastHit2D = Physics2D.Raycast(base.transform.position + new Vector2((this.direction != PlatformingLevelGroundMovementEnemy.Direction.Left) ? this.turnaroundDistance : (-this.turnaroundDistance), this.turnaroundDistance / 2f), Vector2.down, 30f + this.turnaroundDistance, this.groundMask);
		RaycastHit2D raycastHit2D2 = Physics2D.Raycast(base.transform.position + new Vector2((this.direction != PlatformingLevelGroundMovementEnemy.Direction.Left) ? this.turnaroundDistance : (-this.turnaroundDistance), this.turnaroundDistance / 2f), Vector2.up, 30f + this.turnaroundDistance, this.ceilingMask);
		this.RaycastObstacle(this.directionManager.up, raycastHit, bounds.size.y / 2f, PlatformingLevelGroundMovementEnemy.RaycastAxis.Y, bounds.center);
		this.RaycastObstacle(this.directionManager.down, raycastHit2, 30f, PlatformingLevelGroundMovementEnemy.RaycastAxis.Y, new Vector2(base.transform.position.x, base.transform.position.y + 30f));
		if (!this.Grounded)
		{
			if (!this.directionManager.down.able)
			{
				this.OnGrounded();
				this.directionManager.left.able = true;
				this.directionManager.right.able = true;
				if (this.floating)
				{
					base.StartCoroutine(this.floatLand_cr());
				}
			}
			if (!this.directionManager.up.able && this.directionManager.up.able != flag)
			{
				this.OnHitCeiling();
			}
		}
		RaycastHit2D raycastHit2D3 = this.gravityReversed ? raycastHit2D2 : raycastHit2D;
		if (this.Grounded && raycastHit2D3.collider == null && this.timeSinceTurn > 0.1f && !this.jumping)
		{
			if (!this.noTurn)
			{
				this.Turn();
			}
			else
			{
				this.LeaveGround();
			}
		}
	}

	// Token: 0x060031F6 RID: 12790 RVA: 0x001D2620 File Offset: 0x001D0A20
	private float RaycastObstacle(PlatformingLevelGroundMovementEnemy.DirectionManager.Hit directionProperties, RaycastHit2D raycastHit, float maxDistance, PlatformingLevelGroundMovementEnemy.RaycastAxis axis, Vector2 origin)
	{
		if (!this.DoesRaycastHitHaveCollider(raycastHit))
		{
			return 1000f;
		}
		float num = (axis != PlatformingLevelGroundMovementEnemy.RaycastAxis.X) ? Mathf.Abs(origin.y - raycastHit.point.y) : Mathf.Abs(origin.x - raycastHit.point.x);
		directionProperties.pos = raycastHit.point;
		directionProperties.gameObject = raycastHit.collider.gameObject;
		directionProperties.distance = num;
		if (num < maxDistance)
		{
			directionProperties.able = false;
		}
		return num;
	}

	// Token: 0x060031F7 RID: 12791 RVA: 0x001D26B9 File Offset: 0x001D0AB9
	private void ValidateRaycast()
	{
	}

	// Token: 0x060031F8 RID: 12792 RVA: 0x001D26BC File Offset: 0x001D0ABC
	private void OnGrounded()
	{
		if (this.Grounded || !this.jumpManager.ableToLand)
		{
			return;
		}
		LevelPlatform levelPlatform = (!(this.directionManager.down.gameObject == null)) ? this.directionManager.down.gameObject.GetComponent<LevelPlatform>() : null;
		LevelPlatform levelPlatform2 = (!(this.directionManager.up.gameObject == null)) ? this.directionManager.up.gameObject.GetComponent<LevelPlatform>() : null;
		LevelPlatform levelPlatform3 = this.gravityReversed ? levelPlatform2 : levelPlatform;
		if (levelPlatform3 != null)
		{
			levelPlatform3.AddChild(base.transform);
		}
		this.jumpManager.state = PlatformingLevelGroundMovementEnemy.JumpManager.State.Ready;
		this.velocity.y = 0f;
		this.Grounded = true;
		if (this.jumping)
		{
			this.jumping = false;
		}
	}

	// Token: 0x060031F9 RID: 12793 RVA: 0x001D27B4 File Offset: 0x001D0BB4
	private void LeaveGround()
	{
		this.Grounded = false;
		this.jumpManager.ableToLand = false;
		this.velocity.y = 0f;
		this.ClearParent();
		if (this.jumpManager.state == PlatformingLevelGroundMovementEnemy.JumpManager.State.Ready)
		{
			this.jumpManager.state = PlatformingLevelGroundMovementEnemy.JumpManager.State.Used;
		}
	}

	// Token: 0x060031FA RID: 12794 RVA: 0x001D2808 File Offset: 0x001D0C08
	private void OnHitCeiling()
	{
		if (!this.gravityReversed)
		{
			if (this.jumpManager.ableToLand)
			{
				return;
			}
		}
		else
		{
			if (this.Grounded)
			{
				return;
			}
			this.jumpManager.state = PlatformingLevelGroundMovementEnemy.JumpManager.State.Ready;
			LevelPlatform levelPlatform = (!(this.directionManager.up.gameObject == null)) ? this.directionManager.up.gameObject.GetComponent<LevelPlatform>() : null;
			if (levelPlatform != null)
			{
				levelPlatform.AddChild(base.transform);
			}
			this.Grounded = true;
			if (this.jumping)
			{
				this.jumping = false;
			}
		}
		this.velocity.y = 0f;
		this.directionManager.left.able = true;
		this.directionManager.right.able = true;
	}

	// Token: 0x060031FB RID: 12795 RVA: 0x001D28E8 File Offset: 0x001D0CE8
	private void ClearParent()
	{
		if (base.transform.parent != null)
		{
			base.transform.parent.GetComponent<LevelPlatform>().OnPlayerExit(base.transform);
		}
		base.transform.parent = null;
	}

	// Token: 0x060031FC RID: 12796 RVA: 0x001D2928 File Offset: 0x001D0D28
	private void UpdateShadow()
	{
		if (this.Grounded)
		{
			this.shadow.gameObject.SetActive(false);
			return;
		}
		RaycastHit2D raycastHit2D = Physics2D.BoxCast(base.transform.position, new Vector2(this.collider.bounds.size.x, 1f), 0f, Vector2.down, this.maxShadowDistance, this.groundMask);
		if (raycastHit2D.collider == null)
		{
			this.shadow.gameObject.SetActive(false);
			return;
		}
		this.shadow.gameObject.SetActive(true);
		this.shadow.SetPosition(new float?(base.transform.position.x), new float?(raycastHit2D.point.y), null);
		float num = base.transform.position.y - this.shadow.position.y;
		this.shadow.GetComponent<Animator>().Play("Idle", 0, num / this.maxShadowDistance);
		this.shadow.GetComponent<Animator>().speed = 0f;
	}

	// Token: 0x04003A32 RID: 14898
	private const float SCREEN_PADDING = 100f;

	// Token: 0x04003A33 RID: 14899
	private const float DOWN_BOXCAST_Y = 30f;

	// Token: 0x04003A34 RID: 14900
	public float startPosition = 0.5f;

	// Token: 0x04003A35 RID: 14901
	[SerializeField]
	protected PlatformingLevelGroundMovementEnemy.Direction _direction = PlatformingLevelGroundMovementEnemy.Direction.Right;

	// Token: 0x04003A36 RID: 14902
	[SerializeField]
	private bool hasJumpAnimation;

	// Token: 0x04003A37 RID: 14903
	[SerializeField]
	private bool hasTurnAnimation;

	// Token: 0x04003A38 RID: 14904
	[SerializeField]
	private bool canSpawnOnPlatforms;

	// Token: 0x04003A39 RID: 14905
	[SerializeField]
	private float turnaroundDistance = 10f;

	// Token: 0x04003A3A RID: 14906
	[SerializeField]
	private Transform shadow;

	// Token: 0x04003A3B RID: 14907
	[SerializeField]
	private float maxShadowDistance;

	// Token: 0x04003A3C RID: 14908
	[SerializeField]
	private Effect jumpLandEffectPrefab;

	// Token: 0x04003A3D RID: 14909
	[SerializeField]
	protected bool noTurn;

	// Token: 0x04003A3E RID: 14910
	[SerializeField]
	protected bool lockDirectionWhenLanding;

	// Token: 0x04003A3F RID: 14911
	[SerializeField]
	protected bool gravityReversed;

	// Token: 0x04003A41 RID: 14913
	private Collider2D _collider;

	// Token: 0x04003A42 RID: 14914
	private bool _destroyEnemyAfterLeavingScreen;

	// Token: 0x04003A43 RID: 14915
	private bool _enteredScreen;

	// Token: 0x04003A44 RID: 14916
	private PlatformingLevelGroundMovementEnemy.DirectionManager directionManager;

	// Token: 0x04003A45 RID: 14917
	private PlatformingLevelGroundMovementEnemy.JumpManager jumpManager;

	// Token: 0x04003A46 RID: 14918
	protected bool turning;

	// Token: 0x04003A47 RID: 14919
	protected bool floating;

	// Token: 0x04003A48 RID: 14920
	protected bool manuallySetJumpX;

	// Token: 0x04003A49 RID: 14921
	protected float timeSinceTurn;

	// Token: 0x04003A4A RID: 14922
	private string turnTarget;

	// Token: 0x04003A4B RID: 14923
	private float moveSpeed;

	// Token: 0x04003A4C RID: 14924
	private bool jumping;

	// Token: 0x04003A4D RID: 14925
	protected bool landing;

	// Token: 0x04003A4E RID: 14926
	protected bool fallInPit;

	// Token: 0x04003A4F RID: 14927
	private bool playFloatAnim;

	// Token: 0x04003A50 RID: 14928
	private Vector2 velocity = Vector2.zero;

	// Token: 0x04003A51 RID: 14929
	private RaycastHit2D[] hits;

	// Token: 0x04003A52 RID: 14930
	private const float RAY_DISTANCE = 2000f;

	// Token: 0x04003A53 RID: 14931
	private const float MAX_GROUNDED_FALL_DISTANCE = 30f;

	// Token: 0x04003A54 RID: 14932
	private readonly int ceilingMask = 524288;

	// Token: 0x04003A55 RID: 14933
	private readonly int groundMask = 1048576;

	// Token: 0x02000862 RID: 2146
	public enum Direction
	{
		// Token: 0x04003A57 RID: 14935
		Right = 1,
		// Token: 0x04003A58 RID: 14936
		Left = -1
	}

	// Token: 0x02000863 RID: 2147
	private enum RaycastAxis
	{
		// Token: 0x04003A5A RID: 14938
		X,
		// Token: 0x04003A5B RID: 14939
		Y
	}

	// Token: 0x02000864 RID: 2148
	public class DirectionManager
	{
		// Token: 0x060031FD RID: 12797 RVA: 0x001D2A79 File Offset: 0x001D0E79
		public DirectionManager()
		{
			this.Reset();
		}

		// Token: 0x060031FE RID: 12798 RVA: 0x001D2AB3 File Offset: 0x001D0EB3
		public void Reset()
		{
			this.up.Reset();
			this.down.Reset();
			this.left.Reset();
			this.right.Reset();
		}

		// Token: 0x04003A5C RID: 14940
		public PlatformingLevelGroundMovementEnemy.DirectionManager.Hit up = new PlatformingLevelGroundMovementEnemy.DirectionManager.Hit();

		// Token: 0x04003A5D RID: 14941
		public PlatformingLevelGroundMovementEnemy.DirectionManager.Hit down = new PlatformingLevelGroundMovementEnemy.DirectionManager.Hit();

		// Token: 0x04003A5E RID: 14942
		public PlatformingLevelGroundMovementEnemy.DirectionManager.Hit left = new PlatformingLevelGroundMovementEnemy.DirectionManager.Hit();

		// Token: 0x04003A5F RID: 14943
		public PlatformingLevelGroundMovementEnemy.DirectionManager.Hit right = new PlatformingLevelGroundMovementEnemy.DirectionManager.Hit();

		// Token: 0x02000865 RID: 2149
		public class Hit
		{
			// Token: 0x060031FF RID: 12799 RVA: 0x001D2AE1 File Offset: 0x001D0EE1
			public Hit()
			{
				this.Reset();
			}

			// Token: 0x06003200 RID: 12800 RVA: 0x001D2AEF File Offset: 0x001D0EEF
			public Hit(bool able, Vector2 pos, GameObject gameObject, float distance)
			{
				this.able = able;
				this.pos = pos;
				this.gameObject = gameObject;
				this.distance = distance;
			}

			// Token: 0x06003201 RID: 12801 RVA: 0x001D2B14 File Offset: 0x001D0F14
			public void Reset()
			{
				this.able = true;
				this.pos = Vector2.zero;
				this.gameObject = null;
				this.distance = -1f;
			}

			// Token: 0x04003A60 RID: 14944
			public bool able;

			// Token: 0x04003A61 RID: 14945
			public Vector2 pos;

			// Token: 0x04003A62 RID: 14946
			public GameObject gameObject;

			// Token: 0x04003A63 RID: 14947
			public float distance;
		}
	}

	// Token: 0x02000866 RID: 2150
	public class JumpManager
	{
		// Token: 0x04003A64 RID: 14948
		public PlatformingLevelGroundMovementEnemy.JumpManager.State state;

		// Token: 0x04003A65 RID: 14949
		public bool ableToLand;

		// Token: 0x02000867 RID: 2151
		public enum State
		{
			// Token: 0x04003A67 RID: 14951
			Ready,
			// Token: 0x04003A68 RID: 14952
			Used
		}
	}
}
