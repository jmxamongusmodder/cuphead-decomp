using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000718 RID: 1816
public class PirateLevelBarrel : LevelProperties.Pirate.Entity
{
	// Token: 0x06002782 RID: 10114 RVA: 0x00172AA4 File Offset: 0x00170EA4
	public override void LevelInit(LevelProperties.Pirate properties)
	{
		base.LevelInit(properties);
		Level.Current.OnStateChangedEvent += this.OnStateChanged;
		Level.Current.OnLevelStartEvent += this.OnLevelStart;
		this.damageDealer = new DamageDealer(base.properties.CurrentState.barrel.damage, 1f);
		this.damageDealer.SetDirection(DamageDealer.Direction.Neutral, base.transform);
		this.state = PirateLevelBarrel.State.Move;
	}

	// Token: 0x06002783 RID: 10115 RVA: 0x00172B22 File Offset: 0x00170F22
	private void OnLevelStart()
	{
		this.moveCoroutine = this.move_cr();
		base.StartCoroutine(this.moveCoroutine);
	}

	// Token: 0x06002784 RID: 10116 RVA: 0x00172B40 File Offset: 0x00170F40
	private void OnStateChanged()
	{
		this.damageDealer.SetDamage(base.properties.CurrentState.barrel.damage);
		base.StopCoroutine(this.moveCoroutine);
		this.moveCoroutine = this.move_cr();
		base.StartCoroutine(this.moveCoroutine);
	}

	// Token: 0x06002785 RID: 10117 RVA: 0x00172B94 File Offset: 0x00170F94
	private void Update()
	{
		AbstractPlayerController[] array = new AbstractPlayerController[]
		{
			PlayerManager.GetPlayer(PlayerId.PlayerOne),
			PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		};
		if (this.state == PirateLevelBarrel.State.Move)
		{
			float num = base.transform.position.x - 60f;
			float num2 = base.transform.position.x + 60f;
			foreach (AbstractPlayerController abstractPlayerController in array)
			{
				if (!(abstractPlayerController == null) && !(abstractPlayerController.transform == null) && !abstractPlayerController.IsDead)
				{
					if (abstractPlayerController.center.x > num && abstractPlayerController.center.x < num2)
					{
						this.PlayerFound();
						break;
					}
				}
			}
		}
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002786 RID: 10118 RVA: 0x00172C9B File Offset: 0x0017109B
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002787 RID: 10119 RVA: 0x00172CB9 File Offset: 0x001710B9
	private void PlayerFound()
	{
		this.state = PirateLevelBarrel.State.Fall;
		base.StartCoroutine(this.fall_cr());
	}

	// Token: 0x06002788 RID: 10120 RVA: 0x00172CD0 File Offset: 0x001710D0
	private IEnumerator move_cr()
	{
		float time = base.properties.CurrentState.barrel.moveTime;
		float p = (base.transform.position.x - -570f) / 805f;
		if (this.direction == PirateLevelBarrel.Direction.Left)
		{
			p = 1f - p;
		}
		float t = time * p;
		for (;;)
		{
			if (this.direction == PirateLevelBarrel.Direction.Right)
			{
				while (t < time)
				{
					yield return base.StartCoroutine(this.waitForMove_cr());
					float val = t / time;
					float x = EaseUtils.Ease(EaseUtils.EaseType.linear, -570f, 235f, val);
					base.transform.SetPosition(new float?(x), null, null);
					t += CupheadTime.Delta;
					yield return null;
				}
				t = 0f;
				this.direction = PirateLevelBarrel.Direction.Left;
			}
			if (this.direction == PirateLevelBarrel.Direction.Left)
			{
				while (t < time)
				{
					yield return base.StartCoroutine(this.waitForMove_cr());
					float val2 = t / time;
					float x2 = EaseUtils.Ease(EaseUtils.EaseType.linear, 235f, -570f, val2);
					base.transform.SetPosition(new float?(x2), null, null);
					t += CupheadTime.Delta;
					yield return null;
				}
				t = 0f;
				this.direction = PirateLevelBarrel.Direction.Right;
			}
		}
		yield break;
	}

	// Token: 0x06002789 RID: 10121 RVA: 0x00172CEC File Offset: 0x001710EC
	private IEnumerator waitForMove_cr()
	{
		while (this.state != PirateLevelBarrel.State.Move && this.state != PirateLevelBarrel.State.Safe)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600278A RID: 10122 RVA: 0x00172D08 File Offset: 0x00171108
	private IEnumerator fall_cr()
	{
		AudioManager.Play("level_pirate_barrel_drop_attack");
		this.emitAudioFromObject.Add("level_pirate_barrel_drop_attack");
		base.animator.SetTrigger("OnFall");
		this.state = PirateLevelBarrel.State.Fall;
		base.GetComponent<Collider2D>().enabled = true;
		LevelProperties.Pirate.State properties = base.properties.CurrentState;
		float t = 0f;
		float time = properties.barrel.fallTime;
		while (t < time)
		{
			float val = t / time;
			float y = EaseUtils.Ease(EaseUtils.EaseType.easeInQuart, 250f, -225f, val);
			base.transform.SetPosition(null, new float?(y), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetPosition(null, new float?(-225f), null);
		this.dustEffect.Create(base.transform.position);
		this.particlesEffect.Create(base.transform.position);
		base.animator.SetTrigger("OnSmash");
		this.state = PirateLevelBarrel.State.Hold;
		CupheadLevelCamera.Current.Shake(8f, 0.6f, false);
		yield return CupheadTime.WaitForSeconds(this, properties.barrel.groundHold);
		t = 0f;
		time = properties.barrel.riseTime;
		base.animator.SetTrigger("OnUp");
		this.state = PirateLevelBarrel.State.Up;
		while (t < time)
		{
			float val2 = t / time;
			float y2 = EaseUtils.Ease(EaseUtils.EaseType.linear, -225f, 250f, val2);
			base.transform.SetPosition(null, new float?(y2), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetPosition(null, new float?(250f), null);
		base.animator.SetTrigger("OnSafe");
		this.state = PirateLevelBarrel.State.Safe;
		yield return CupheadTime.WaitForSeconds(this, properties.barrel.safeTime);
		base.animator.SetTrigger("OnReady");
		this.state = PirateLevelBarrel.State.Move;
		yield break;
	}

	// Token: 0x0600278B RID: 10123 RVA: 0x00172D23 File Offset: 0x00171123
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.dustEffect = null;
		this.particlesEffect = null;
	}

	// Token: 0x04003042 RID: 12354
	public const float MIN_X = -570f;

	// Token: 0x04003043 RID: 12355
	public const float MAX_X = 235f;

	// Token: 0x04003044 RID: 12356
	public const float UP_Y = 250f;

	// Token: 0x04003045 RID: 12357
	public const float DOWN_Y = -225f;

	// Token: 0x04003046 RID: 12358
	public const float RANGE = 120f;

	// Token: 0x04003047 RID: 12359
	[SerializeField]
	private Effect particlesEffect;

	// Token: 0x04003048 RID: 12360
	[SerializeField]
	private Effect dustEffect;

	// Token: 0x04003049 RID: 12361
	private PirateLevelBarrel.State state;

	// Token: 0x0400304A RID: 12362
	private PirateLevelBarrel.Direction direction = PirateLevelBarrel.Direction.Left;

	// Token: 0x0400304B RID: 12363
	private IEnumerator moveCoroutine;

	// Token: 0x0400304C RID: 12364
	private DamageDealer damageDealer;

	// Token: 0x02000719 RID: 1817
	public enum State
	{
		// Token: 0x0400304E RID: 12366
		Init,
		// Token: 0x0400304F RID: 12367
		Move,
		// Token: 0x04003050 RID: 12368
		Fall,
		// Token: 0x04003051 RID: 12369
		Hold,
		// Token: 0x04003052 RID: 12370
		Up,
		// Token: 0x04003053 RID: 12371
		Safe
	}

	// Token: 0x0200071A RID: 1818
	public enum Direction
	{
		// Token: 0x04003055 RID: 12373
		Right,
		// Token: 0x04003056 RID: 12374
		Left
	}
}
