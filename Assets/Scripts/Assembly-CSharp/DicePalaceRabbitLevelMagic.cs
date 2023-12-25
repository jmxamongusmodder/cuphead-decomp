using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005DE RID: 1502
public class DicePalaceRabbitLevelMagic : AbstractProjectile
{
	// Token: 0x1700036F RID: 879
	// (get) Token: 0x06001DA9 RID: 7593 RVA: 0x00110D28 File Offset: 0x0010F128
	// (set) Token: 0x06001DAA RID: 7594 RVA: 0x00110D30 File Offset: 0x0010F130
	public float AppearTime { get; set; }

	// Token: 0x06001DAB RID: 7595 RVA: 0x00110D39 File Offset: 0x0010F139
	protected override void Start()
	{
		base.Start();
		this.initialPosition = base.transform.position;
		this.idleRoutine = this.wait_activation_cr();
		base.StartCoroutine(this.idleRoutine);
		base.StartCoroutine(this.FadeIn());
	}

	// Token: 0x06001DAC RID: 7596 RVA: 0x00110D78 File Offset: 0x0010F178
	protected override void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		base.Update();
	}

	// Token: 0x06001DAD RID: 7597 RVA: 0x00110D96 File Offset: 0x0010F196
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001DAE RID: 7598 RVA: 0x00110DB4 File Offset: 0x0010F1B4
	public override void SetParryable(bool parryable)
	{
		base.SetParryable(parryable);
		base.animator.SetBool("CanParry", parryable);
	}

	// Token: 0x06001DAF RID: 7599 RVA: 0x00110DD0 File Offset: 0x0010F1D0
	public void ActivateOrb()
	{
		this.circleCollider.enabled = true;
		Color color = this.spriteRenderer.color;
		color.a = 1f;
		this.spriteRenderer.color = color;
		base.StopCoroutine(this.idleRoutine);
		base.transform.position = this.initialPosition;
		base.animator.SetTrigger("Attack");
		if (!this.StartMagicLaserSFX)
		{
			AudioManager.Play("projectile_laser");
			this.emitAudioFromObject.Add("projectile_laser");
			this.StartMagicLaserSFX = true;
		}
	}

	// Token: 0x06001DB0 RID: 7600 RVA: 0x00110E66 File Offset: 0x0010F266
	public void Move(float startY, bool down, float speed)
	{
		base.StartCoroutine(this.move_cr(startY, down, speed));
	}

	// Token: 0x06001DB1 RID: 7601 RVA: 0x00110E78 File Offset: 0x0010F278
	private IEnumerator wait_activation_cr()
	{
		for (;;)
		{
			Vector3 vector = new Vector3((float)UnityEngine.Random.Range(-1, 1), (float)UnityEngine.Random.Range(-1, 1), 0f);
			Vector3 newDirection = vector.normalized * 10f;
			float progress = 0f;
			while (progress < 0.1f)
			{
				base.transform.position += newDirection * CupheadTime.Delta;
				progress += CupheadTime.Delta;
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001DB2 RID: 7602 RVA: 0x00110E94 File Offset: 0x0010F294
	private IEnumerator move_cr(float startY, bool down, float speed)
	{
		this.StartMagicSFX = false;
		this.StartMagicLaserSFX = false;
		Vector3 velocity = speed * ((!down) ? Vector3.up : Vector3.down);
		float progress = 0f;
		while ((!down && startY + progress < 360f) || (down && startY + progress > -360f))
		{
			base.transform.position += velocity * CupheadTime.Delta;
			progress += velocity.y * CupheadTime.Delta;
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06001DB3 RID: 7603 RVA: 0x00110EC4 File Offset: 0x0010F2C4
	private IEnumerator FadeIn()
	{
		if (!this.StartMagicSFX)
		{
			AudioManager.Play("projectile_magic_start");
			this.emitAudioFromObject.Add("projectile_magic_start");
			this.StartMagicSFX = true;
		}
		while (this.spriteRenderer.color.a < 1f)
		{
			Color c = this.spriteRenderer.color;
			c.a += CupheadTime.Delta / this.AppearTime;
			this.spriteRenderer.color = c;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001DB4 RID: 7604 RVA: 0x00110EDF File Offset: 0x0010F2DF
	public void SetSuit(int suit)
	{
		base.animator.SetInteger("Suit", suit);
	}

	// Token: 0x06001DB5 RID: 7605 RVA: 0x00110EF2 File Offset: 0x0010F2F2
	public void IsOffset(bool offset)
	{
		base.animator.SetFloat("CycleOffset", (!offset) ? 0f : 0.5f);
	}

	// Token: 0x0400268C RID: 9868
	private const float IdleSpeed = 10f;

	// Token: 0x0400268D RID: 9869
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	// Token: 0x0400268E RID: 9870
	[SerializeField]
	private CircleCollider2D circleCollider;

	// Token: 0x0400268F RID: 9871
	private IEnumerator idleRoutine;

	// Token: 0x04002690 RID: 9872
	private Vector3 initialPosition;

	// Token: 0x04002692 RID: 9874
	private bool StartMagicSFX;

	// Token: 0x04002693 RID: 9875
	private bool StartMagicLaserSFX;
}
