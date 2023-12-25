using System;
using UnityEngine;

// Token: 0x02000A51 RID: 2641
public class PlayerSuperChaliceBounceBall : AbstractProjectile
{
	// Token: 0x06003EEB RID: 16107 RVA: 0x0022729B File Offset: 0x0022569B
	protected override void OnDieLifetime()
	{
	}

	// Token: 0x06003EEC RID: 16108 RVA: 0x0022729D File Offset: 0x0022569D
	protected override void OnDieDistance()
	{
	}

	// Token: 0x06003EED RID: 16109 RVA: 0x002272A0 File Offset: 0x002256A0
	protected override void Start()
	{
		base.Start();
		this.baseScale = base.transform.localScale;
		this.colliderSize = base.GetComponent<CircleCollider2D>().radius;
		this.rend = base.GetComponent<SpriteRenderer>();
		this.velocity.y = 0f;
		this.damageDealer.SetDamageSource(DamageDealer.DamageSource.Super);
	}

	// Token: 0x06003EEE RID: 16110 RVA: 0x00227300 File Offset: 0x00225700
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		this.velocity.y = this.velocity.y - this.GRAVITY * CupheadTime.FixedDelta;
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		this.HandleInput();
		this.HandleJiggle();
		this.CheckEdges();
		this.CheckCollisionsThenMove();
		if (!this.super.LAUNCHED_VERSION)
		{
			this.player.transform.position = base.transform.position;
		}
		this.lastHitTimer -= CupheadTime.FixedDelta;
		if (this.super.timer < 1f)
		{
			this.rend.enabled = (Time.frameCount % 2 == 0);
		}
	}

	// Token: 0x06003EEF RID: 16111 RVA: 0x002273C6 File Offset: 0x002257C6
	public override void OnLevelEnd()
	{
		this.super.CleanUp();
	}

	// Token: 0x06003EF0 RID: 16112 RVA: 0x002273D4 File Offset: 0x002257D4
	private void HandleJiggle()
	{
		if (this.jiggleTime > 0f)
		{
			base.transform.localScale = new Vector3(this.baseScale.x + Mathf.Sin(this.jiggleTime * 3.1415927f * 15f) * this.jiggleTime * 10f, this.baseScale.y + Mathf.Cos(this.jiggleTime * 3.1415927f * 15f) * this.jiggleTime * 10f, 1f);
			this.jiggleTime -= CupheadTime.FixedDelta;
		}
		else
		{
			base.transform.localScale = this.baseScale;
		}
	}

	// Token: 0x06003EF1 RID: 16113 RVA: 0x0022748E File Offset: 0x0022588E
	private void SetJiggle()
	{
		AudioManager.Play("player_jump");
		AudioManager.Play("circus_trampoline_bounce");
		this.jiggleTime = 0.2f;
	}

	// Token: 0x06003EF2 RID: 16114 RVA: 0x002274B0 File Offset: 0x002258B0
	private void CheckCollisionsThenMove()
	{
		Vector3 inNormal = Vector3.zero;
		int num = 0;
		float num2 = Vector3.Magnitude(this.velocity * CupheadTime.FixedDelta);
		int num3 = 9;
		float num4 = 3f;
		float num5 = this.baseScale.x * this.colliderSize * 0.9f;
		int num6 = 262144;
		int num7 = 1048576;
		int num8 = 524288;
		int layerMask = num6 + num8 + num7;
		int layerMask2 = 1;
		Vector3[] array = new Vector3[num3];
		while (num2 > 0f && (float)num < num4)
		{
			GameObject gameObject = null;
			bool flag = false;
			Vector3 vector = this.velocity.normalized;
			float num9 = (float)(180 / (array.Length - 1));
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = base.transform.position + Quaternion.Euler(0f, 0f, -90f + num9 * (float)i) * vector * num5;
			}
			float num10 = num2;
			for (int j = 0; j < num3; j++)
			{
				if (Physics2D.OverlapPoint(array[j], layerMask) != null && Physics2D.OverlapPoint(base.transform.position, layerMask) == null)
				{
					RaycastHit2D raycastHit2D = Physics2D.Raycast(base.transform.position, array[j] - base.transform.position, num5 * 2f, layerMask);
					if (raycastHit2D.collider != null)
					{
						array[j] = Vector3.Lerp(raycastHit2D.point, base.transform.position, 0.001f);
					}
				}
			}
			for (int k = 0; k < num3; k++)
			{
				RaycastHit2D hit = Physics2D.Raycast(array[k], this.velocity, num10, num6);
				global::Debug.DrawLine(array[k], array[k] + vector * num10, Color.red, 1f);
				if (hit.collider != null)
				{
					this.smokePuffEffect.Create(hit.point);
					if (Vector3.Distance(array[k], hit.point) <= num10)
					{
						flag = true;
						inNormal = hit.normal;
						num10 = Vector3.Distance(array[k], hit.point);
					}
				}
			}
			for (int l = 0; l < num3; l++)
			{
				RaycastHit2D hit = Physics2D.Raycast(array[l], this.velocity, num10, layerMask2);
				if (hit.collider != null && hit.collider.gameObject.CompareTag("Enemy") && (this.lastEnemyHit == null || this.lastEnemyHit != hit || this.lastHitTimer <= 0f))
				{
					this.smokePuffEffect.Create(hit.point);
					if (Vector3.Distance(array[l], hit.point) <= num10)
					{
						flag = true;
						inNormal = hit.normal;
						num10 = Vector3.Distance(array[l], hit.point);
						gameObject = hit.collider.gameObject;
					}
				}
			}
			if (this.velocity.y >= 0f)
			{
				for (int m = 0; m < num3; m++)
				{
					RaycastHit2D hit = Physics2D.Raycast(array[m], this.velocity, num10, num8);
					if (hit.collider != null)
					{
						this.smokePuffEffect.Create(hit.point);
						if (Vector3.Distance(array[m], hit.point) <= num10)
						{
							flag = true;
							inNormal = hit.normal;
							num10 = Vector3.Distance(array[m], hit.point);
						}
					}
				}
			}
			for (int n = 0; n < num3; n++)
			{
				RaycastHit2D hit = Physics2D.Raycast(array[n], this.velocity, num10, num7);
				if (hit.collider != null)
				{
					LevelPlatform component = hit.collider.gameObject.GetComponent<LevelPlatform>();
					bool flag2 = false;
					if (component != null && (base.transform.position.y < hit.point.y || this.velocity.y > 0f))
					{
						flag2 = true;
					}
					if (!flag2 && (component == null || !component.canFallThrough || this.player.input.actions.GetAxis(1) > -0.35f))
					{
						this.smokePuffEffect.Create(hit.point);
						if (Vector3.Distance(array[n], hit.point) <= num10)
						{
							flag = true;
							inNormal = hit.normal;
							num10 = Vector3.Distance(array[n], hit.point);
						}
					}
				}
			}
			num2 -= num10;
			base.transform.position += vector * num10;
			if (flag)
			{
				this.SetJiggle();
				num++;
				this.velocity = Vector3.Reflect(this.velocity, inNormal);
				if (inNormal.y > 0f)
				{
					this.velocity.y = inNormal.y * this.BOUNCE_VEL * ((!this.player.input.actions.GetButton(2)) ? this.BOUNCE_MODIFIER_NO_JUMP : 1f);
				}
				if (gameObject != null)
				{
					this.DoCollisionEnemy(gameObject);
					this.velocity.x = this.velocity.x * this.ENEMY_REBOUND_MULTIPLIER;
				}
			}
		}
	}

	// Token: 0x06003EF3 RID: 16115 RVA: 0x00227BC0 File Offset: 0x00225FC0
	protected void DoCollisionEnemy(GameObject hit)
	{
		this.lastEnemyHit = hit;
		this.lastHitTimer = this.ENEMY_MULTIHIT_DELAY;
		float num = this.damageDealer.DealDamage(hit);
		if (num > 0f)
		{
			base.animator.Play("Player_Super_Chalice_BounceBall_Flash");
			AudioManager.Play("player_parry_axe");
		}
		this.damageCount += num;
		if (this.damageCount >= this.MAX_DAMAGE)
		{
			this.super.Interrupt();
		}
	}

	// Token: 0x06003EF4 RID: 16116 RVA: 0x00227C3C File Offset: 0x0022603C
	private void HandleInput()
	{
		Trilean trilean = 0;
		Trilean trilean2 = 0;
		float axis = this.player.input.actions.GetAxis(0);
		if (axis > 0.35f || axis < -0.35f)
		{
			trilean = axis;
		}
		float move_ACCEL = this.MOVE_ACCEL;
		this.velocity.x = this.velocity.x + (float)trilean.Value * move_ACCEL * CupheadTime.FixedDelta;
		this.velocity.x = Mathf.Clamp(this.velocity.x, -this.MOVE_MAX_SPEED, this.MOVE_MAX_SPEED);
	}

	// Token: 0x06003EF5 RID: 16117 RVA: 0x00227CDC File Offset: 0x002260DC
	private void CheckEdges()
	{
		if (LevelPit.Instance != null && base.transform.position.y < LevelPit.Instance.transform.position.y && this.velocity.y < 0f)
		{
			base.transform.position += Vector3.down * 300f;
			this.super.Interrupt();
		}
		Vector2 v = base.transform.position;
		v.x = Mathf.Clamp(v.x, (float)Level.Current.Left + 30f, (float)Level.Current.Right - 30f);
		if (v.x != base.transform.position.x)
		{
			this.velocity.x = -this.velocity.x;
		}
		base.transform.position = v;
	}

	// Token: 0x040045E5 RID: 17893
	private const float PADDING_LEFT = 30f;

	// Token: 0x040045E6 RID: 17894
	private const float PADDING_RIGHT = 30f;

	// Token: 0x040045E7 RID: 17895
	private const float ANALOG_THRESHOLD = 0.35f;

	// Token: 0x040045E8 RID: 17896
	private float MAX_DAMAGE = WeaponProperties.LevelSuperChaliceBounce.maxDamage;

	// Token: 0x040045E9 RID: 17897
	private float MOVE_ACCEL = WeaponProperties.LevelSuperChaliceBounce.horizontalAcceleration;

	// Token: 0x040045EA RID: 17898
	private float MOVE_MAX_SPEED = WeaponProperties.LevelSuperChaliceBounce.maxHorizontalSpeed;

	// Token: 0x040045EB RID: 17899
	private float BOUNCE_VEL = WeaponProperties.LevelSuperChaliceBounce.bounceVelocity;

	// Token: 0x040045EC RID: 17900
	private float BOUNCE_MODIFIER_NO_JUMP = WeaponProperties.LevelSuperChaliceBounce.bounceModifierNoJump;

	// Token: 0x040045ED RID: 17901
	private float GRAVITY = WeaponProperties.LevelSuperChaliceBounce.gravity;

	// Token: 0x040045EE RID: 17902
	private float ENEMY_REBOUND_MULTIPLIER = WeaponProperties.LevelSuperChaliceBounce.enemyReboundMultiplier;

	// Token: 0x040045EF RID: 17903
	private float ENEMY_MULTIHIT_DELAY = WeaponProperties.LevelSuperChaliceBounce.enemyMultihitDelay;

	// Token: 0x040045F0 RID: 17904
	[SerializeField]
	private Effect smokePuffEffect;

	// Token: 0x040045F1 RID: 17905
	public Vector2 velocity;

	// Token: 0x040045F2 RID: 17906
	public LevelPlayerController player;

	// Token: 0x040045F3 RID: 17907
	private GameObject lastEnemyHit;

	// Token: 0x040045F4 RID: 17908
	private float lastHitTimer;

	// Token: 0x040045F5 RID: 17909
	public PlayerSuperChaliceBounce super;

	// Token: 0x040045F6 RID: 17910
	private float jiggleTime;

	// Token: 0x040045F7 RID: 17911
	private Vector3 baseScale;

	// Token: 0x040045F8 RID: 17912
	private float colliderSize;

	// Token: 0x040045F9 RID: 17913
	private SpriteRenderer rend;

	// Token: 0x040045FA RID: 17914
	private float damageCount;
}
