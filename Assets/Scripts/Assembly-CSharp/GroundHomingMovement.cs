using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000434 RID: 1076
public class GroundHomingMovement : AbstractPausableComponent
{
	// Token: 0x17000287 RID: 647
	// (get) Token: 0x06000FC5 RID: 4037 RVA: 0x0009C8F2 File Offset: 0x0009ACF2
	// (set) Token: 0x06000FC6 RID: 4038 RVA: 0x0009C8FA File Offset: 0x0009ACFA
	public AbstractPlayerController TrackingPlayer { get; set; }

	// Token: 0x17000288 RID: 648
	// (get) Token: 0x06000FC7 RID: 4039 RVA: 0x0009C903 File Offset: 0x0009AD03
	// (set) Token: 0x06000FC8 RID: 4040 RVA: 0x0009C90B File Offset: 0x0009AD0B
	public bool EnableHoming { get; set; }

	// Token: 0x17000289 RID: 649
	// (get) Token: 0x06000FC9 RID: 4041 RVA: 0x0009C914 File Offset: 0x0009AD14
	// (set) Token: 0x06000FCA RID: 4042 RVA: 0x0009C91C File Offset: 0x0009AD1C
	public GroundHomingMovement.Direction MoveDirection { get; set; }

	// Token: 0x06000FCB RID: 4043 RVA: 0x0009C925 File Offset: 0x0009AD25
	protected override void Awake()
	{
		base.Awake();
		base.StartCoroutine(this.loop_cr());
		if (this.startOnAwake)
		{
			this.EnableHoming = true;
		}
	}

	// Token: 0x06000FCC RID: 4044 RVA: 0x0009C94C File Offset: 0x0009AD4C
	private float hitPauseCoefficient()
	{
		DamageReceiver component = base.GetComponent<DamageReceiver>();
		if (component == null)
		{
			return 1f;
		}
		return (!component.IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x06000FCD RID: 4045 RVA: 0x0009C98C File Offset: 0x0009AD8C
	private IEnumerator loop_cr()
	{
		Quaternion radishRot = base.transform.localRotation;
		while (this.TrackingPlayer == null)
		{
			yield return null;
		}
		for (;;)
		{
			if (!base.enabled)
			{
				yield return null;
			}
			else
			{
				if (this.TrackingPlayer == null || this.TrackingPlayer.IsDead)
				{
					this.TrackingPlayer = PlayerManager.GetNext();
				}
				if (this.EnableHoming)
				{
					if (this.TrackingPlayer.transform.position.x > base.transform.position.x)
					{
						this.MoveDirection = GroundHomingMovement.Direction.Right;
						if (radishRot.z < 0.05235988f)
						{
							radishRot.z += 0.01f;
						}
					}
					else
					{
						this.MoveDirection = GroundHomingMovement.Direction.Left;
						if (radishRot.z > -0.05235988f)
						{
							radishRot.z -= 0.01f;
						}
					}
				}
				if (this.MoveDirection == GroundHomingMovement.Direction.Right)
				{
					this.velocityX += this.acceleration * CupheadTime.Delta * this.hitPauseCoefficient();
				}
				else
				{
					this.velocityX -= this.acceleration * CupheadTime.Delta * this.hitPauseCoefficient();
				}
				this.velocityX = Mathf.Clamp(this.velocityX, -this.maxSpeed, this.maxSpeed);
				Vector2 position = base.transform.localPosition;
				position.x += this.velocityX * CupheadTime.Delta * this.hitPauseCoefficient();
				if (this.bounceEnabled)
				{
					if (position.x < (float)Level.Current.Left + this.leftPadding)
					{
						position.x = (float)Level.Current.Left + this.leftPadding;
						this.velocityX *= -this.bounceRatio;
					}
					if (position.x > (float)Level.Current.Right - this.rightPadding)
					{
						position.x = (float)Level.Current.Right - this.rightPadding;
						this.velocityX *= -this.bounceRatio;
					}
				}
				if (this.destroyOffScreen)
				{
					SpriteRenderer component = base.GetComponent<SpriteRenderer>();
					if (position.x < (float)Level.Current.Left - component.bounds.size.x / 2f || position.x > (float)Level.Current.Right + component.bounds.size.x / 2f)
					{
						UnityEngine.Object.Destroy(base.gameObject);
					}
				}
				base.transform.localPosition = position;
				if (this.enableRadishRot)
				{
					base.transform.localRotation = radishRot;
				}
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x04001957 RID: 6487
	public bool startOnAwake;

	// Token: 0x04001958 RID: 6488
	public float maxSpeed;

	// Token: 0x04001959 RID: 6489
	public float acceleration;

	// Token: 0x0400195A RID: 6490
	public float bounceRatio;

	// Token: 0x0400195B RID: 6491
	public bool bounceEnabled;

	// Token: 0x0400195C RID: 6492
	public float leftPadding;

	// Token: 0x0400195D RID: 6493
	public float rightPadding;

	// Token: 0x0400195E RID: 6494
	public bool destroyOffScreen;

	// Token: 0x0400195F RID: 6495
	public bool enableRadishRot;

	// Token: 0x04001960 RID: 6496
	private float velocityX;

	// Token: 0x02000435 RID: 1077
	public enum Direction
	{
		// Token: 0x04001965 RID: 6501
		Left,
		// Token: 0x04001966 RID: 6502
		Right
	}
}
