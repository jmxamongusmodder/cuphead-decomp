using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200078D RID: 1933
public class RumRunnersLevelGrub : AbstractProjectile
{
	// Token: 0x170003EE RID: 1006
	// (get) Token: 0x06002AB6 RID: 10934 RVA: 0x0018F101 File Offset: 0x0018D501
	// (set) Token: 0x06002AB7 RID: 10935 RVA: 0x0018F109 File Offset: 0x0018D509
	public int x { get; private set; }

	// Token: 0x170003EF RID: 1007
	// (get) Token: 0x06002AB8 RID: 10936 RVA: 0x0018F112 File Offset: 0x0018D512
	// (set) Token: 0x06002AB9 RID: 10937 RVA: 0x0018F11A File Offset: 0x0018D51A
	public int y { get; private set; }

	// Token: 0x170003F0 RID: 1008
	// (get) Token: 0x06002ABA RID: 10938 RVA: 0x0018F123 File Offset: 0x0018D523
	// (set) Token: 0x06002ABB RID: 10939 RVA: 0x0018F12B File Offset: 0x0018D52B
	public float speed { get; private set; }

	// Token: 0x170003F1 RID: 1009
	// (get) Token: 0x06002ABC RID: 10940 RVA: 0x0018F134 File Offset: 0x0018D534
	// (set) Token: 0x06002ABD RID: 10941 RVA: 0x0018F13C File Offset: 0x0018D53C
	public bool moving { get; private set; }

	// Token: 0x170003F2 RID: 1010
	// (get) Token: 0x06002ABE RID: 10942 RVA: 0x0018F145 File Offset: 0x0018D545
	// (set) Token: 0x06002ABF RID: 10943 RVA: 0x0018F14D File Offset: 0x0018D54D
	public bool startedEntering { get; private set; }

	// Token: 0x06002AC0 RID: 10944 RVA: 0x0018F158 File Offset: 0x0018D558
	public RumRunnersLevelGrub Create(RumRunnersLevelGrubPath path, float rotation, float speed, float time, float hp, RumRunnersLevelSpider parent, int enterVariant, int variant, int spawnOrder, int x, int y)
	{
		RumRunnersLevelGrub rumRunnersLevelGrub = base.Create(path.start, rotation) as RumRunnersLevelGrub;
		rumRunnersLevelGrub.transform.localScale = new Vector3(0.3f * Mathf.Sign(path.transform.position.x - path.start.x), 0.3f);
		rumRunnersLevelGrub.path = path;
		rumRunnersLevelGrub.speed = speed;
		rumRunnersLevelGrub.time = time;
		rumRunnersLevelGrub.hp = hp;
		rumRunnersLevelGrub.parent = parent;
		rumRunnersLevelGrub.GetComponent<Collider2D>().enabled = true;
		rumRunnersLevelGrub.enterVariant = enterVariant;
		rumRunnersLevelGrub.variant = variant;
		rumRunnersLevelGrub.animator.SetInteger("Variant", enterVariant);
		rumRunnersLevelGrub.animator.SetInteger("BlinkLoops", UnityEngine.Random.Range((int)RumRunnersLevelGrub.BlinkLoopsRange.minimum, (int)RumRunnersLevelGrub.BlinkLoopsRange.maximum + 1));
		rumRunnersLevelGrub.animator.Play("Start", 0, 0f);
		rumRunnersLevelGrub.spawnOrder = spawnOrder;
		rumRunnersLevelGrub.shadowDist = this.shadowTransform.localPosition.y;
		rumRunnersLevelGrub.x = x;
		rumRunnersLevelGrub.y = y;
		return rumRunnersLevelGrub;
	}

	// Token: 0x06002AC1 RID: 10945 RVA: 0x0018F288 File Offset: 0x0018D688
	protected override void Start()
	{
		base.Start();
		this.collider = base.GetComponent<Collider2D>();
		if (base.GetComponent<DamageReceiver>())
		{
			this.damageReceiver = base.GetComponent<DamageReceiver>();
			this.damageReceiver.OnDamageTaken += this.onDamageTaken;
		}
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002AC2 RID: 10946 RVA: 0x0018F2E7 File Offset: 0x0018D6E7
	protected override void OnDieDistance()
	{
	}

	// Token: 0x06002AC3 RID: 10947 RVA: 0x0018F2E9 File Offset: 0x0018D6E9
	protected override void OnDieLifetime()
	{
	}

	// Token: 0x06002AC4 RID: 10948 RVA: 0x0018F2EC File Offset: 0x0018D6EC
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.moving)
		{
			this.horizontalSpeedEasingTime += CupheadTime.FixedDelta;
			this.basePos.x = this.basePos.x + Mathf.Lerp(0f, this.speed, this.horizontalSpeedEasingTime / 0.5f) * CupheadTime.FixedDelta;
			if (this.finishedEntering)
			{
				this.basePos.y = RumRunnersLevel.GroundWalkingPosY(this.basePos, null, this.yOffset, 200f);
			}
			if (this.basePos.x > 960f || this.basePos.x < -960f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			base.transform.position = this.basePos + this.wobblePos();
			if (this.finishedEntering)
			{
				this.shadowTransform.position = new Vector3(base.transform.position.x, this.basePos.y + this.shadowDist);
				this.wobbleTimer += this.wobbleSpeed * CupheadTime.FixedDelta;
			}
		}
	}

	// Token: 0x06002AC5 RID: 10949 RVA: 0x0018F42C File Offset: 0x0018D82C
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase == CollisionPhase.Enter)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002AC6 RID: 10950 RVA: 0x0018F454 File Offset: 0x0018D854
	private void onDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			Level.Current.RegisterMinionKilled();
			this.die(true);
		}
	}

	// Token: 0x06002AC7 RID: 10951 RVA: 0x0018F48A File Offset: 0x0018D88A
	private Vector3 wobblePos()
	{
		return new Vector3(Mathf.Sin(this.wobbleTimer * 3f) * this.wobbleX, Mathf.Sin(this.wobbleTimer * 2f) * this.wobbleY, 0f);
	}

	// Token: 0x06002AC8 RID: 10952 RVA: 0x0018F4C8 File Offset: 0x0018D8C8
	public float GetTimeToMove()
	{
		if (this.moving)
		{
			return 0f;
		}
		if (!this.startedEntering)
		{
			return (15f + RumRunnersLevelGrub.flipEndClipLength[this.variant]) * 0.041666668f;
		}
		int num = Animator.StringToHash(base.animator.GetLayerName(0) + ".Flip");
		if (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == num)
		{
			return (15f * (1f - base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime) + RumRunnersLevelGrub.flipEndClipLength[this.variant]) * 1f / 24f;
		}
		return RumRunnersLevelGrub.flipEndClipLength[this.variant] * (1f - base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime) * 1f / 24f;
	}

	// Token: 0x06002AC9 RID: 10953 RVA: 0x0018F5B0 File Offset: 0x0018D9B0
	private IEnumerator move_cr()
	{
		this.collider.enabled = false;
		float t = 0f;
		Vector3 destinationPoint = new Vector3(this.path.GetPoint(1f).x, RumRunnersLevel.GroundWalkingPosY(this.path.GetPoint(1f), null, this.yOffset, 200f) + 4f);
		float pathOffset = destinationPoint.y - this.path.GetPoint(1f).y;
		float orientation = Mathf.Sign(base.transform.position.x - destinationPoint.x);
		while (t <= 1f)
		{
			t += 0.033333335f;
			this.setSortingOrder(75 + (int)(t * 10f));
			if (t > 0.9f)
			{
				base.animator.SetTrigger("EndFlyUp");
			}
			if (t + 0.033333335f >= this.path.forceFGSet && t < this.path.forceFGSet)
			{
				this.setSortingLayer("Default");
			}
			base.transform.position = this.path.GetPoint(EaseUtils.EaseOutSine(0f, 1f, t)) + Vector2.up * pathOffset;
			base.transform.localScale = new Vector3(EaseUtils.EaseInCubic(0.3f, 0.8f, t) * orientation, EaseUtils.EaseInCubic(0.3f, 0.8f, t));
			yield return CupheadTime.WaitForSeconds(this, 0.033333335f);
		}
		base.animator.SetTrigger("EndFlyUp");
		base.transform.localScale = new Vector3(Mathf.Sign(base.transform.localScale.x) * 0.8f, 0.8f);
		base.transform.position = destinationPoint;
		Vector3 vel = destinationPoint - (this.path.GetPoint(EaseUtils.EaseOutSine(0f, 1f, t - 0.033333335f)) + Vector2.up * pathOffset);
		t = 0f;
		while (t < this.time || (this.parent && !this.parent.GrubCanEnter(base.transform.position, this.GetTimeToMove())))
		{
			base.transform.position = destinationPoint + Mathf.Sin(t * 10f) * vel * (Mathf.InverseLerp(1f, 0f, t) * 10f);
			vel = vel.magnitude * MathUtils.AngleToDirection(MathUtils.DirectionToAngle(vel) + 3f);
			yield return CupheadTime.WaitForSeconds(this, 0.033333335f);
			t += 0.033333335f;
		}
		base.transform.position = destinationPoint;
		Vector3 onGroundPoint = new Vector3(this.path.GetPoint(1f).x, RumRunnersLevel.GroundWalkingPosY(this.path.GetPoint(1f), null, this.yOffset, 200f));
		float timeToMove = this.GetTimeToMove();
		float flipLength = 0.625f;
		t = 0f;
		base.animator.SetTrigger("Enter");
		this.startedEntering = true;
		this.SFX_RUMRUN_Grub_VocalIntro();
		this.SFX_RUMRUN_Grub_FlyingLoop();
		while (t < timeToMove)
		{
			float moveTime = Mathf.Clamp(t / flipLength, 0f, 1f);
			float flipTime = Mathf.Clamp(t / flipLength, 0f, 1f);
			base.transform.position = new Vector3(base.transform.position.x, Mathf.Lerp(destinationPoint.y, onGroundPoint.y, moveTime) + Mathf.Sin(flipTime * 3.1415927f) * 30f);
			base.transform.localScale = new Vector3(Mathf.Lerp(0.8f, 1f, moveTime) * Mathf.Sign(base.transform.localScale.x), Mathf.Lerp(0.8f, 1f, moveTime));
			this.basePos = base.transform.position;
			this.shadowTransform.position = new Vector3(base.transform.position.x, onGroundPoint.y + this.shadowDist);
			yield return CupheadTime.WaitForSeconds(this, 0.033333335f);
			t += 0.033333335f;
		}
		base.transform.position = new Vector3(base.transform.position.x, onGroundPoint.y);
		this.finishedEntering = true;
		yield break;
	}

	// Token: 0x06002ACA RID: 10954 RVA: 0x0018F5CB File Offset: 0x0018D9CB
	private void die(bool playSound)
	{
		UnityEngine.Object.Destroy(base.gameObject);
		this.deathEffect.Create(base.transform.position);
		this.SFX_RUMRUN_Grub_FlyingLoopStop();
		if (playSound)
		{
			this.SFX_RUMRUN_Grub_Lackey_DiePoof();
		}
	}

	// Token: 0x06002ACB RID: 10955 RVA: 0x0018F601 File Offset: 0x0018DA01
	private void AniEvent_EnableCollision()
	{
		this.collider.enabled = true;
	}

	// Token: 0x06002ACC RID: 10956 RVA: 0x0018F60F File Offset: 0x0018DA0F
	private void AniEvent_StartMoving()
	{
		this.moving = true;
	}

	// Token: 0x06002ACD RID: 10957 RVA: 0x0018F618 File Offset: 0x0018DA18
	private void AniEvent_OnFlip()
	{
		this.setSortingLayer("Enemies");
		this.setSortingOrder(this.spawnOrder + 1);
		base.transform.localScale = new Vector3(Mathf.Abs(base.transform.localScale.x) * Mathf.Sign(base.transform.position.x - PlayerManager.GetNext().transform.position.x), base.transform.localScale.y);
		this.speed *= -base.transform.localScale.x;
		base.animator.SetInteger("Variant", this.variant);
	}

	// Token: 0x06002ACE RID: 10958 RVA: 0x0018F6E4 File Offset: 0x0018DAE4
	private void animationEvent_BlinkCompleted()
	{
		base.animator.SetInteger("BlinkLoops", UnityEngine.Random.Range((int)RumRunnersLevelGrub.BlinkLoopsRange.minimum, (int)RumRunnersLevelGrub.BlinkLoopsRange.maximum + 1));
	}

	// Token: 0x06002ACF RID: 10959 RVA: 0x0018F724 File Offset: 0x0018DB24
	private void setSortingLayer(string layerName)
	{
		this.mainRenderer.sortingLayerName = layerName;
		this.blinkRenderer.sortingLayerName = layerName;
	}

	// Token: 0x06002AD0 RID: 10960 RVA: 0x0018F73E File Offset: 0x0018DB3E
	private void setSortingOrder(int order)
	{
		this.mainRenderer.sortingOrder = order;
		this.blinkRenderer.sortingOrder = order + 1;
	}

	// Token: 0x06002AD1 RID: 10961 RVA: 0x0018F75A File Offset: 0x0018DB5A
	private void SFX_RUMRUN_Grub_Lackey_DiePoof()
	{
		AudioManager.Play("sfx_dlc_rumrun_lackey_poof");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_lackey_poof");
	}

	// Token: 0x06002AD2 RID: 10962 RVA: 0x0018F776 File Offset: 0x0018DB76
	private void SFX_RUMRUN_Grub_VocalIntro()
	{
	}

	// Token: 0x06002AD3 RID: 10963 RVA: 0x0018F778 File Offset: 0x0018DB78
	private void SFX_RUMRUN_Grub_FlyingLoop()
	{
		AudioManager.Play("sfx_dlc_rumrun_grub_flying_loop");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_grub_flying_loop");
	}

	// Token: 0x06002AD4 RID: 10964 RVA: 0x0018F794 File Offset: 0x0018DB94
	private void SFX_RUMRUN_Grub_FlyingLoopStop()
	{
		AudioManager.Stop("sfx_dlc_rumrun_grub_flying_loop");
	}

	// Token: 0x04003373 RID: 13171
	private static readonly float[] enterEndClipLength = new float[]
	{
		10f,
		11f,
		16f
	};

	// Token: 0x04003374 RID: 13172
	private const float flipClipLength = 15f;

	// Token: 0x04003375 RID: 13173
	private static readonly float[] flipEndClipLength = new float[]
	{
		17f,
		12f,
		9f,
		19f
	};

	// Token: 0x04003376 RID: 13174
	private const float START_SIZE = 0.3f;

	// Token: 0x04003377 RID: 13175
	private const float WAIT_SIZE = 0.8f;

	// Token: 0x04003378 RID: 13176
	private const float END_FLY_UP_TRIGGER = 0.9f;

	// Token: 0x04003379 RID: 13177
	private const float OVERSHOOT = 10f;

	// Token: 0x0400337A RID: 13178
	private const float FLIP_HEIGHT = 30f;

	// Token: 0x0400337B RID: 13179
	private const float TIME_TO_FULL_X_SPEED = 0.5f;

	// Token: 0x0400337C RID: 13180
	private static readonly Rangef BlinkLoopsRange = new Rangef(2f, 3f);

	// Token: 0x0400337D RID: 13181
	private const float EnterYOffset = 4f;

	// Token: 0x0400337E RID: 13182
	[SerializeField]
	private float yOffset;

	// Token: 0x0400337F RID: 13183
	[SerializeField]
	private SpriteRenderer mainRenderer;

	// Token: 0x04003380 RID: 13184
	[SerializeField]
	private SpriteRenderer blinkRenderer;

	// Token: 0x04003381 RID: 13185
	[SerializeField]
	private Transform shadowTransform;

	// Token: 0x04003382 RID: 13186
	[SerializeField]
	private float wobbleX = 10f;

	// Token: 0x04003383 RID: 13187
	[SerializeField]
	private float wobbleY = 10f;

	// Token: 0x04003384 RID: 13188
	[SerializeField]
	private float wobbleSpeed = 1f;

	// Token: 0x04003385 RID: 13189
	[SerializeField]
	private Effect deathEffect;

	// Token: 0x0400338B RID: 13195
	private float time;

	// Token: 0x0400338C RID: 13196
	private float hp;

	// Token: 0x0400338D RID: 13197
	private DamageReceiver damageReceiver;

	// Token: 0x0400338E RID: 13198
	private RumRunnersLevelSpider parent;

	// Token: 0x0400338F RID: 13199
	private Collider2D collider;

	// Token: 0x04003390 RID: 13200
	private bool finishedEntering;

	// Token: 0x04003391 RID: 13201
	private RumRunnersLevelGrubPath path;

	// Token: 0x04003392 RID: 13202
	private int enterVariant;

	// Token: 0x04003393 RID: 13203
	private int variant;

	// Token: 0x04003394 RID: 13204
	private int spawnOrder;

	// Token: 0x04003395 RID: 13205
	private float wobbleTimer;

	// Token: 0x04003396 RID: 13206
	private Vector3 basePos;

	// Token: 0x04003397 RID: 13207
	private float shadowDist;

	// Token: 0x04003398 RID: 13208
	private float horizontalSpeedEasingTime;
}
