using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000587 RID: 1415
public class DevilLevelSwooper : AbstractCollidableObject
{
	// Token: 0x06001AFF RID: 6911 RVA: 0x000F8010 File Offset: 0x000F6410
	public DevilLevelSwooper Create(DevilLevelGiantHead parent, LevelProperties.Devil.Swoopers properties, Vector3 spawnPos, float xPos)
	{
		DevilLevelSwooper devilLevelSwooper = this.InstantiatePrefab<DevilLevelSwooper>();
		devilLevelSwooper.parent = parent;
		devilLevelSwooper.properties = properties;
		devilLevelSwooper.state = DevilLevelSwooper.State.Intro;
		devilLevelSwooper.transform.position = spawnPos;
		devilLevelSwooper.yPos = properties.yIdlePos.RandomFloat();
		devilLevelSwooper.StartCoroutine(devilLevelSwooper.spawn_cr(xPos));
		return devilLevelSwooper;
	}

	// Token: 0x06001B00 RID: 6912 RVA: 0x000F8066 File Offset: 0x000F6466
	protected override void Awake()
	{
		base.Awake();
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06001B01 RID: 6913 RVA: 0x000F8090 File Offset: 0x000F6490
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001B02 RID: 6914 RVA: 0x000F80A8 File Offset: 0x000F64A8
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001B03 RID: 6915 RVA: 0x000F80D1 File Offset: 0x000F64D1
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp < 0f && this.state != DevilLevelSwooper.State.Dying)
		{
			this.Die();
		}
	}

	// Token: 0x06001B04 RID: 6916 RVA: 0x000F8108 File Offset: 0x000F6508
	private IEnumerator spawn_cr(float xPos)
	{
		this.hp = this.properties.hp;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
		yield return base.animator.WaitForAnimationToEnd(this, "Spawn", false, true);
		while (base.transform.position.y < CupheadLevelCamera.Current.Bounds.yMax + 50f)
		{
			base.transform.position += Vector3.up * 200f * CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetTrigger("Continue");
		float t = 0f;
		Vector2 start = base.transform.position;
		Vector2 end = new Vector3(xPos, this.yPos);
		while (t < 2f)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / 2f);
			base.transform.position = Vector3.Lerp(start, end, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetPosition(new float?(xPos), new float?(this.yPos), null);
		this.state = DevilLevelSwooper.State.Idle;
		yield break;
	}

	// Token: 0x06001B05 RID: 6917 RVA: 0x000F812A File Offset: 0x000F652A
	public void Swoop()
	{
		this.state = DevilLevelSwooper.State.Swooping;
		base.StartCoroutine(this.swoop_cr());
		AudioManager.Play("mini_devil_attack");
	}

	// Token: 0x06001B06 RID: 6918 RVA: 0x000F814C File Offset: 0x000F654C
	private IEnumerator swoop_cr()
	{
		float bestDistance = float.MaxValue;
		Vector2 bestVelocity = Vector2.zero;
		Vector3 target = PlayerManager.GetNext().center;
		Vector2 relativeTargetPos = target - base.transform.position;
		relativeTargetPos.x = Mathf.Abs(relativeTargetPos.x);
		if (target.x > base.transform.position.x)
		{
			base.animator.SetTrigger("OnTurn");
			yield return base.animator.WaitForAnimationToEnd(this, "Turn", false, true);
		}
		base.animator.SetBool("Spinning", true);
		this.AttackSFX();
		for (float num = 0f; num < 1f; num += 0.01f)
		{
			float angle = -this.properties.launchAngle.GetFloatAt(num);
			float floatAt = this.properties.launchSpeed.GetFloatAt(num);
			Vector2 vector = MathUtils.AngleToDirection(angle) * floatAt;
			float num2 = relativeTargetPos.x / vector.x;
			float num3 = vector.y * num2 + 0.5f * this.properties.gravity * num2 * num2;
			float num4 = Mathf.Abs(relativeTargetPos.y - num3);
			float num5 = vector.y + this.properties.gravity * num2;
			if (num5 >= 0f)
			{
				if (num4 < bestDistance)
				{
					bestDistance = num4;
					bestVelocity = vector;
				}
			}
		}
		if (target.x < base.transform.position.x)
		{
			bestVelocity.x *= -1f;
		}
		Vector2 velocity = bestVelocity;
		while (base.transform.position.y < (float)(Level.Current.Ceiling + 150))
		{
			velocity.y += this.properties.gravity * CupheadTime.FixedDelta;
			base.transform.AddPosition(velocity.x * CupheadTime.FixedDelta, velocity.y * CupheadTime.FixedDelta, 0f);
			yield return new WaitForFixedUpdate();
		}
		this.state = DevilLevelSwooper.State.Returning;
		float xPos = this.parent.PutSwooperInSlot(this);
		base.transform.SetPosition(new float?(xPos), null, null);
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		float moveTime = 1.5f;
		float t = 0f;
		while (t < moveTime)
		{
			base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, (float)(Level.Current.Ceiling + 150), this.yPos, t / moveTime)), null);
			t += CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		this.state = DevilLevelSwooper.State.Idle;
		base.transform.SetPosition(null, new float?(this.yPos), null);
		base.animator.SetBool("Spinning", false);
		this.AttackSFXEnd();
		yield break;
	}

	// Token: 0x06001B07 RID: 6919 RVA: 0x000F8168 File Offset: 0x000F6568
	public void Die()
	{
		if (this.finalSwooping)
		{
			AudioManager.Stop("swooper_spin");
		}
		if (this.state == DevilLevelSwooper.State.Dying)
		{
			return;
		}
		this.state = DevilLevelSwooper.State.Dying;
		this.StopAllCoroutines();
		this.parent.OnSwooperDeath(this);
		base.StartCoroutine(this.death_cr());
		AudioManager.Play("mini_devil_die");
		this.emitAudioFromObject.Add("mini_devil_die");
		AudioManager.Stop("swooper_spin");
	}

	// Token: 0x06001B08 RID: 6920 RVA: 0x000F81E4 File Offset: 0x000F65E4
	private IEnumerator death_cr()
	{
		while (this.state == DevilLevelSwooper.State.Intro)
		{
			yield return null;
		}
		foreach (Effect effect in this.explosions)
		{
			effect.Create(base.transform.position);
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06001B09 RID: 6921 RVA: 0x000F8200 File Offset: 0x000F6600
	private void OnTurn()
	{
		base.transform.SetScale(new float?(-base.transform.localScale.x), null, null);
	}

	// Token: 0x06001B0A RID: 6922 RVA: 0x000F8243 File Offset: 0x000F6643
	private void AttackSFX()
	{
		AudioManager.PlayLoop("swooper_spin");
		this.emitAudioFromObject.Add("swooper_spin_end");
	}

	// Token: 0x06001B0B RID: 6923 RVA: 0x000F825F File Offset: 0x000F665F
	private void AttackSFXEnd()
	{
		if (this.finalSwooping)
		{
			AudioManager.Stop("swooper_spin");
		}
		AudioManager.Play("swooper_spin_end");
		this.emitAudioFromObject.Add("swooper_spin_end");
		this.finalSwooping = false;
	}

	// Token: 0x0400243C RID: 9276
	[SerializeField]
	private Effect[] explosions;

	// Token: 0x0400243D RID: 9277
	private const float SPAWN_X_RATIO = 0.5f;

	// Token: 0x0400243E RID: 9278
	public DevilLevelSwooper.State state;

	// Token: 0x0400243F RID: 9279
	private DevilLevelGiantHead parent;

	// Token: 0x04002440 RID: 9280
	private LevelProperties.Devil.Swoopers properties;

	// Token: 0x04002441 RID: 9281
	private DamageDealer damageDealer;

	// Token: 0x04002442 RID: 9282
	private float hp;

	// Token: 0x04002443 RID: 9283
	public bool finalSwooping;

	// Token: 0x04002444 RID: 9284
	private float yPos;

	// Token: 0x02000588 RID: 1416
	public enum State
	{
		// Token: 0x04002446 RID: 9286
		Intro,
		// Token: 0x04002447 RID: 9287
		Idle,
		// Token: 0x04002448 RID: 9288
		Swooping,
		// Token: 0x04002449 RID: 9289
		Returning,
		// Token: 0x0400244A RID: 9290
		Dying
	}
}
