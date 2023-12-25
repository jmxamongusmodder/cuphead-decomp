using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008B6 RID: 2230
public class FunhousePlatformingLevelJack : AbstractPlatformingLevelEnemy
{
	// Token: 0x1700044A RID: 1098
	// (get) Token: 0x060033FC RID: 13308 RVA: 0x001E2BD0 File Offset: 0x001E0FD0
	// (set) Token: 0x060033FD RID: 13309 RVA: 0x001E2BD8 File Offset: 0x001E0FD8
	public bool HomingEnabled { get; set; }

	// Token: 0x060033FE RID: 13310 RVA: 0x001E2BE1 File Offset: 0x001E0FE1
	protected override void OnStart()
	{
	}

	// Token: 0x060033FF RID: 13311 RVA: 0x001E2BE4 File Offset: 0x001E0FE4
	protected override void Start()
	{
		base.Start();
		bool flag = Rand.Bool();
		base.animator.Play((!flag) ? "Green_Idle_A" : "Pink_Idle_A");
		this._canParry = flag;
		this.HomingEnabled = true;
		this.player = PlayerManager.GetNext();
		this.launchVelocity = this.homingDirection * base.Properties.jackLaunchVelocity;
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.switch_cr());
	}

	// Token: 0x06003400 RID: 13312 RVA: 0x001E2C6C File Offset: 0x001E106C
	protected override void Update()
	{
		base.Update();
		this.CalculateRender();
	}

	// Token: 0x06003401 RID: 13313 RVA: 0x001E2C7A File Offset: 0x001E107A
	public void SelectDirection(bool fromBottom)
	{
		this.homingDirection = ((!fromBottom) ? Vector2.down : Vector2.up);
	}

	// Token: 0x06003402 RID: 13314 RVA: 0x001E2C98 File Offset: 0x001E1098
	private IEnumerator move_cr()
	{
		float t = 0f;
		while (t < base.Properties.jacktimeBeforeDeath + base.Properties.jackEaseTime + base.Properties.jacktimeBeforeHoming)
		{
			while (!this.HomingEnabled)
			{
				yield return null;
			}
			t += CupheadTime.FixedDelta;
			if (this.player != null && !this.player.IsDead)
			{
				Vector3 center = this.player.center;
				Vector2 direction = (center - base.transform.position).normalized;
				Quaternion b = Quaternion.Euler(0f, 0f, MathUtils.DirectionToAngle(direction));
				Quaternion a = Quaternion.Euler(0f, 0f, MathUtils.DirectionToAngle(this.homingDirection));
				this.homingDirection = MathUtils.AngleToDirection(Quaternion.Slerp(a, b, Mathf.Min(1f, CupheadTime.FixedDelta * base.Properties.jackRotationSpeed)).eulerAngles.z);
			}
			Vector2 homingVelocity = this.homingDirection * base.Properties.jackHomingMoveSpeed;
			Vector2 velocity = homingVelocity;
			if (t < base.Properties.jacktimeBeforeHoming)
			{
				velocity = this.launchVelocity;
			}
			else if (t < base.Properties.jacktimeBeforeHoming + base.Properties.jackEaseTime)
			{
				float t2 = EaseUtils.EaseOutSine(0f, 1f, (t - base.Properties.jacktimeBeforeHoming) / base.Properties.jackEaseTime);
				velocity = Vector2.Lerp(this.launchVelocity, homingVelocity, t2);
			}
			base.transform.AddPosition(velocity.x * CupheadTime.FixedDelta, velocity.y * CupheadTime.FixedDelta, 0f);
			yield return new WaitForFixedUpdate();
		}
		this.Die();
		yield break;
	}

	// Token: 0x06003403 RID: 13315 RVA: 0x001E2CB4 File Offset: 0x001E10B4
	private IEnumerator switch_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(1.5f, 3f));
			base.animator.SetTrigger("OnSwitch");
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003404 RID: 13316 RVA: 0x001E2CCF File Offset: 0x001E10CF
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.Die();
	}

	// Token: 0x06003405 RID: 13317 RVA: 0x001E2CDF File Offset: 0x001E10DF
	protected override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (hit.GetComponent<FunhousePlatformingLevelJack>() != null)
		{
			this.Die();
		}
	}

	// Token: 0x06003406 RID: 13318 RVA: 0x001E2D00 File Offset: 0x001E1100
	private void CalculateRender()
	{
		if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position) && !this._enteredScreen)
		{
			this._enteredScreen = true;
		}
		if (this._enteredScreen && !CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(100f, 100f)))
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		if (PlatformingLevel.Current != null && (base.transform.position.x < (float)PlatformingLevel.Current.Left - 100f || base.transform.position.x > (float)PlatformingLevel.Current.Right + 100f || base.transform.position.y < (float)PlatformingLevel.Current.Ground - 100f))
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06003407 RID: 13319 RVA: 0x001E2E18 File Offset: 0x001E1218
	protected override void Die()
	{
		AudioManager.Play("funhouse_jack_death");
		this.emitAudioFromObject.Add("funhouse_jack_death");
		base.Die();
	}

	// Token: 0x04003C47 RID: 15431
	private const float SCREEN_PADDING = 100f;

	// Token: 0x04003C48 RID: 15432
	private AbstractPlayerController player;

	// Token: 0x04003C49 RID: 15433
	private bool _enteredScreen;

	// Token: 0x04003C4A RID: 15434
	private Vector2 homingDirection = Vector2.down;

	// Token: 0x04003C4B RID: 15435
	private Vector2 launchVelocity;
}
