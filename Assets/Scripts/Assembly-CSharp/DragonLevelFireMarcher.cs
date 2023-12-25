using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005F0 RID: 1520
public class DragonLevelFireMarcher : AbstractCollidableObject
{
	// Token: 0x06001E37 RID: 7735 RVA: 0x00115FB3 File Offset: 0x001143B3
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		if (this.canJump)
		{
			base.animator.Play("Idle", 0, UnityEngine.Random.Range(0f, 1f));
		}
	}

	// Token: 0x06001E38 RID: 7736 RVA: 0x00115FF1 File Offset: 0x001143F1
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001E39 RID: 7737 RVA: 0x00116009 File Offset: 0x00114409
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001E3A RID: 7738 RVA: 0x00116034 File Offset: 0x00114434
	public DragonLevelFireMarcher Create(Transform root, LevelProperties.Dragon.FireMarchers properties)
	{
		DragonLevelFireMarcher dragonLevelFireMarcher = this.InstantiatePrefab<DragonLevelFireMarcher>();
		dragonLevelFireMarcher.transform.parent = root;
		dragonLevelFireMarcher.transform.ResetLocalPosition();
		dragonLevelFireMarcher.properties = properties;
		dragonLevelFireMarcher.StartCoroutine(dragonLevelFireMarcher.move_cr());
		return dragonLevelFireMarcher;
	}

	// Token: 0x06001E3B RID: 7739 RVA: 0x00116074 File Offset: 0x00114474
	private IEnumerator move_cr()
	{
		float initialYOffset = Mathf.Sin(0.9773844f) * 5f;
		float timeSlowed = 0f;
		while (base.transform.position.x < (float)(Level.Current.Right + 100))
		{
			float speed = this.properties.moveSpeed;
			if (this.slowing)
			{
				timeSlowed += CupheadTime.Delta;
				if (timeSlowed > 0.25f)
				{
					yield break;
				}
				speed = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, speed, 0f, timeSlowed / 0.25f);
			}
			float x = base.transform.localPosition.x + speed * CupheadTime.Delta;
			float y = Mathf.Sin((70f + base.transform.localPosition.x) * 2f * 3.1415927f / 450f) * 5f - initialYOffset;
			y += Mathf.Min(1f, x / 300f) * -23f;
			if (x < this.squeezeDistance)
			{
				base.transform.SetScale(new float?(x / this.squeezeDistance), null, null);
			}
			else
			{
				base.transform.SetScale(new float?(1f), null, null);
			}
			base.transform.SetLocalPosition(new float?(x), new float?(y), null);
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06001E3C RID: 7740 RVA: 0x00116090 File Offset: 0x00114490
	public bool CanJump()
	{
		if (!this.canJump || this.wantsToJump)
		{
			return false;
		}
		AnimatorStateInfo currentAnimatorStateInfo = base.animator.GetCurrentAnimatorStateInfo(0);
		float num = base.transform.localPosition.x + (1f - currentAnimatorStateInfo.normalizedTime % 1f) * currentAnimatorStateInfo.length * this.properties.moveSpeed;
		return num > this.properties.jumpX.min && num < this.properties.jumpX.max;
	}

	// Token: 0x06001E3D RID: 7741 RVA: 0x0011612A File Offset: 0x0011452A
	public void StartJump(AbstractPlayerController targetPlayer)
	{
		this.targetPlayer = targetPlayer;
		this.wantsToJump = true;
		base.StartCoroutine(this.jump_cr());
	}

	// Token: 0x06001E3E RID: 7742 RVA: 0x00116148 File Offset: 0x00114548
	private IEnumerator jump_cr()
	{
		base.animator.SetTrigger("StartJump");
		yield return base.animator.WaitForAnimationToStart(this, "Crouch_Start", false);
		AudioManager.Play("level_dragon_fire_marcher_b_couch_start");
		this.emitAudioFromObject.Add("level_dragon_fire_marcher_b_couch_start");
		this.slowing = true;
		yield return base.animator.WaitForAnimationToStart(this, "Crouch_Loop", false);
		Vector2 targetPos = this.targetPlayer.center;
		if (targetPos.x < base.transform.position.x)
		{
			base.transform.SetScale(new float?(-1f), null, null);
		}
		float bestDistance = float.MaxValue;
		Vector2 bestLaunchVelocity = Vector2.zero;
		Vector2 relativeTargetPos = targetPos - base.transform.position;
		relativeTargetPos.x = Mathf.Abs(relativeTargetPos.x);
		for (float num = 0f; num < 1f; num += 0.01f)
		{
			float floatAt = this.properties.jumpAngle.GetFloatAt(num);
			float floatAt2 = this.properties.jumpSpeed.GetFloatAt(num);
			Vector2 vector = MathUtils.AngleToDirection(floatAt) * floatAt2;
			float num2 = relativeTargetPos.x / vector.x;
			float num3 = vector.y * num2 - 0.5f * this.properties.gravity * num2 * num2;
			float num4 = Mathf.Abs(relativeTargetPos.y - num3);
			if (num4 < bestDistance)
			{
				bestDistance = num4;
				bestLaunchVelocity = vector;
			}
		}
		yield return CupheadTime.WaitForSeconds(this, this.properties.crouchTime);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToStart(this, "Jump_Start", false);
		AudioManager.Play("level_dragon_fire_marcher_b_jump_start");
		this.emitAudioFromObject.Add("level_dragon_fire_marcher_b_jump_start");
		Vector2 velocity = bestLaunchVelocity;
		velocity.x *= base.transform.localScale.x;
		float t = 0f;
		Vector2 initialPos = base.transform.localPosition;
		while (base.transform.position.y > -400f)
		{
			t += CupheadTime.FixedDelta;
			base.transform.SetLocalPosition(new float?(initialPos.x + t * velocity.x), new float?(initialPos.y + t * velocity.y - 0.5f * this.properties.gravity * t * t), null);
			yield return new WaitForFixedUpdate();
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x040026FA RID: 9978
	private const float sinOffset = 70f;

	// Token: 0x040026FB RID: 9979
	private const float sinPeriod = 450f;

	// Token: 0x040026FC RID: 9980
	private const float sinHeight = 5f;

	// Token: 0x040026FD RID: 9981
	private const float linearOffset = -23f;

	// Token: 0x040026FE RID: 9982
	private const float linearOffsetDistance = 300f;

	// Token: 0x040026FF RID: 9983
	private const float minJumpX = 50f;

	// Token: 0x04002700 RID: 9984
	private const float maxJumpX = 590f;

	// Token: 0x04002701 RID: 9985
	private DamageDealer damageDealer;

	// Token: 0x04002702 RID: 9986
	private LevelProperties.Dragon.FireMarchers properties;

	// Token: 0x04002703 RID: 9987
	[SerializeField]
	private float squeezeDistance;

	// Token: 0x04002704 RID: 9988
	[SerializeField]
	private bool canJump;

	// Token: 0x04002705 RID: 9989
	private bool wantsToJump;

	// Token: 0x04002706 RID: 9990
	private bool slowing;

	// Token: 0x04002707 RID: 9991
	private AbstractPlayerController targetPlayer;
}
