using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008AD RID: 2221
public class CircusPlatformingLevelTrampoline : AbstractCollidableObject
{
	// Token: 0x17000447 RID: 1095
	// (get) Token: 0x060033C0 RID: 13248 RVA: 0x001E0B40 File Offset: 0x001DEF40
	// (set) Token: 0x060033C1 RID: 13249 RVA: 0x001E0B48 File Offset: 0x001DEF48
	public CircusPlatformingLevelTrampoline.Direction MoveDirection { get; set; }

	// Token: 0x17000448 RID: 1096
	// (get) Token: 0x060033C2 RID: 13250 RVA: 0x001E0B51 File Offset: 0x001DEF51
	// (set) Token: 0x060033C3 RID: 13251 RVA: 0x001E0B59 File Offset: 0x001DEF59
	public AbstractPlayerController TrackingPlayer { get; set; }

	// Token: 0x060033C4 RID: 13252 RVA: 0x001E0B62 File Offset: 0x001DEF62
	private void Start()
	{
		this.startPos = base.transform.position;
		base.StartCoroutine(this.loop_cr());
		base.StartCoroutine(this.sleep_sfx_cr());
	}

	// Token: 0x060033C5 RID: 13253 RVA: 0x001E0B94 File Offset: 0x001DEF94
	protected override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (phase == CollisionPhase.Enter || phase == CollisionPhase.Stay)
		{
			LevelPlayerMotor component = hit.GetComponent<LevelPlayerMotor>();
			if (component != null && component.Grounded)
			{
				base.animator.SetTrigger("Bounce");
				component.OnTrampolineKnockUp(this.knockUpHeight);
			}
		}
	}

	// Token: 0x060033C6 RID: 13254 RVA: 0x001E0BF0 File Offset: 0x001DEFF0
	private IEnumerator loop_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		this.TrackingPlayer = PlayerManager.GetNext();
		while (this.TrackingPlayer == null)
		{
			yield return null;
		}
		float minBounds = base.transform.localPosition.x - this.bounds;
		float maxBounds = base.transform.localPosition.x + this.bounds;
		for (;;)
		{
			if (!base.enabled)
			{
				yield return null;
			}
			else
			{
				if (this.TrackingPlayer == null)
				{
					this.TrackingPlayer = PlayerManager.GetNext();
				}
				if (this.TrackingPlayer.transform.position.x > base.transform.position.x)
				{
					this.MoveDirection = CircusPlatformingLevelTrampoline.Direction.Right;
				}
				else
				{
					this.MoveDirection = CircusPlatformingLevelTrampoline.Direction.Left;
				}
				if (this.MoveDirection == CircusPlatformingLevelTrampoline.Direction.Right)
				{
					if (base.transform.position.x < this.startPos.x + this.bounds)
					{
						this.velocityX += this.acceleration * CupheadTime.Delta;
					}
					else
					{
						this.velocityX = 0f;
					}
				}
				else if (base.transform.position.x > this.startPos.x - this.bounds)
				{
					this.velocityX -= this.acceleration * CupheadTime.Delta;
				}
				else
				{
					this.velocityX = 0f;
				}
				this.velocityX = Mathf.Clamp(this.velocityX, -this.maxSpeed, this.maxSpeed);
				this.position = base.transform.localPosition;
				this.position.x = this.position.x + this.velocityX * CupheadTime.Delta;
				if (this.position.x < minBounds)
				{
					this.position.x = minBounds;
					this.velocityX = 0f;
				}
				else if (this.position.x > maxBounds)
				{
					this.position.x = maxBounds;
					this.velocityX = 0f;
				}
				base.transform.localPosition = this.position;
				if (this.TrackingPlayer.IsDead)
				{
					this.TrackingPlayer = PlayerManager.GetNext();
				}
				this.CheckIfShouldSleep();
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x060033C7 RID: 13255 RVA: 0x001E0C0C File Offset: 0x001DF00C
	private void CheckIfShouldSleep()
	{
		Transform transform = PlayerManager.GetPlayer(PlayerId.PlayerOne).transform;
		if (this.IsInBounds(transform))
		{
			base.animator.SetBool("Sleep", false);
			if (PlayerManager.Multiplayer && this.IsInBounds(PlayerManager.GetPlayer(PlayerId.PlayerTwo).transform))
			{
				this.TrackingPlayer = PlayerManager.GetNext();
			}
			else
			{
				this.TrackingPlayer = PlayerManager.GetPlayer(PlayerId.PlayerOne);
			}
		}
		else if (PlayerManager.Multiplayer && this.IsInBounds(PlayerManager.GetPlayer(PlayerId.PlayerTwo).transform))
		{
			base.animator.SetBool("Sleep", false);
			this.TrackingPlayer = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		}
		else
		{
			if (base.transform.position.x >= this.startPos.x + this.bounds || base.transform.position.x <= this.startPos.x - this.bounds)
			{
				base.animator.SetBool("Sleep", true);
			}
			this.TrackingPlayer = PlayerManager.GetNext();
		}
	}

	// Token: 0x060033C8 RID: 13256 RVA: 0x001E0D38 File Offset: 0x001DF138
	private IEnumerator sleep_sfx_cr()
	{
		for (;;)
		{
			if (base.animator.GetBool("Sleep"))
			{
				yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(2f, 5f));
				if (base.animator.GetBool("Sleep"))
				{
					AudioManager.Play("circus_trampoline_sleep_boil");
					this.emitAudioFromObject.Add("circus_trampoline_sleep_boil");
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060033C9 RID: 13257 RVA: 0x001E0D54 File Offset: 0x001DF154
	private bool IsInBounds(Transform other)
	{
		float num = this.startPos.x - this.bounds;
		float num2 = this.startPos.x + this.bounds;
		return other.position.x < num2 + this.AwakeningZone && other.position.x > num - this.AwakeningZone;
	}

	// Token: 0x060033CA RID: 13258 RVA: 0x001E0DC0 File Offset: 0x001DF1C0
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawLine(new Vector2(this.startPos.x + this.bounds, this.startPos.y), new Vector2(this.startPos.x - this.bounds, this.startPos.y));
	}

	// Token: 0x060033CB RID: 13259 RVA: 0x001E0E28 File Offset: 0x001DF228
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Gizmos.DrawLine(new Vector2(base.transform.position.x + this.bounds, base.transform.position.y), new Vector2(base.transform.position.x - this.bounds, base.transform.position.y));
	}

	// Token: 0x060033CC RID: 13260 RVA: 0x001E0EAE File Offset: 0x001DF2AE
	private void TrampolineBounceSFX()
	{
		AudioManager.Play("circus_trampoline_bounce");
		this.emitAudioFromObject.Add("circus_trampoline_bounce");
	}

	// Token: 0x060033CD RID: 13261 RVA: 0x001E0ECA File Offset: 0x001DF2CA
	private void TrampolineIntroSFX()
	{
		AudioManager.Stop("circus_trampoline_idle_loop");
		AudioManager.Play("circus_trampoline_sleep_intro");
		this.emitAudioFromObject.Add("circus_trampoline_sleep_intro");
	}

	// Token: 0x060033CE RID: 13262 RVA: 0x001E0EF0 File Offset: 0x001DF2F0
	private void TrampolineOutroSFX()
	{
		AudioManager.Play("circus_trampoline_sleep_outro");
		this.emitAudioFromObject.Add("circus_trampoline_sleep_outro");
	}

	// Token: 0x060033CF RID: 13263 RVA: 0x001E0F0C File Offset: 0x001DF30C
	private void TrampolineStartIdleSFX()
	{
		AudioManager.PlayLoop("circus_trampoline_idle_loop");
		this.emitAudioFromObject.Add("circus_trampoline_idle_loop");
	}

	// Token: 0x04003C0F RID: 15375
	private const string BounceTrigger = "Bounce";

	// Token: 0x04003C10 RID: 15376
	private const string Sleep = "Sleep";

	// Token: 0x04003C11 RID: 15377
	[SerializeField]
	private float bounds;

	// Token: 0x04003C12 RID: 15378
	[SerializeField]
	private float AwakeningZone = 500f;

	// Token: 0x04003C13 RID: 15379
	public float maxSpeed;

	// Token: 0x04003C14 RID: 15380
	public float acceleration;

	// Token: 0x04003C15 RID: 15381
	public float knockUpHeight = -1.95f;

	// Token: 0x04003C16 RID: 15382
	private float velocityX;

	// Token: 0x04003C17 RID: 15383
	private Vector2 startPos;

	// Token: 0x04003C18 RID: 15384
	private Vector2 position;

	// Token: 0x020008AE RID: 2222
	public enum Direction
	{
		// Token: 0x04003C1C RID: 15388
		Left,
		// Token: 0x04003C1D RID: 15389
		Right
	}
}
