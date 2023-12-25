using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005CC RID: 1484
public class DicePalaceFlyingMemoryLevelStuffedToy : LevelProperties.DicePalaceFlyingMemory.Entity
{
	// Token: 0x06001D0C RID: 7436 RVA: 0x0010A9D0 File Offset: 0x00108DD0
	protected override void Awake()
	{
		base.Awake();
		this.state = DicePalaceFlyingMemoryLevelStuffedToy.State.Closed;
		base.GetComponent<DamageReceiver>().enabled = false;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06001D0D RID: 7437 RVA: 0x0010AA24 File Offset: 0x00108E24
	public override void LevelInit(LevelProperties.DicePalaceFlyingMemory properties)
	{
		base.LevelInit(properties);
		this.shotPattern = properties.CurrentState.stuffedToy.shotType.GetRandom<string>().Split(new char[]
		{
			','
		});
		this.shotIndex = 0;
		Level.Current.OnWinEvent += this.OnDeath;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001D0E RID: 7438 RVA: 0x0010AA8D File Offset: 0x00108E8D
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001D0F RID: 7439 RVA: 0x0010AAAB File Offset: 0x00108EAB
	protected void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001D10 RID: 7440 RVA: 0x0010AAC3 File Offset: 0x00108EC3
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (base.properties.CurrentHealth > 0f)
		{
			base.properties.DealDamage(info.damage);
		}
	}

	// Token: 0x06001D11 RID: 7441 RVA: 0x0010AAEB File Offset: 0x00108EEB
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.projectile = null;
		this.spiralProjectile = null;
	}

	// Token: 0x06001D12 RID: 7442 RVA: 0x0010AB04 File Offset: 0x00108F04
	private IEnumerator intro_cr()
	{
		float t = 0f;
		float time = 1.5f;
		Vector3 end = new Vector3(base.transform.position.x, 0f);
		Vector3 start = base.transform.position;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, end, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = end;
		base.animator.SetTrigger("Continue");
		AudioManager.Play("dice_palace_memory_monkey_intro");
		this.emitAudioFromObject.Add("dice_palace_memory_monkey_intro");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", false, true);
		base.StartCoroutine(this.check_boundaries_cr());
		base.StartCoroutine(this.pick_angle_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06001D13 RID: 7443 RVA: 0x0010AB20 File Offset: 0x00108F20
	private void FireSingle(float speed)
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		Vector3 v = next.transform.position - base.transform.position;
		float rotation = MathUtils.DirectionToAngle(v);
		this.projectile.Create(this.projectileRoot.transform.position, rotation, speed);
	}

	// Token: 0x06001D14 RID: 7444 RVA: 0x0010AB80 File Offset: 0x00108F80
	private void FireSpreadshot()
	{
		LevelProperties.DicePalaceFlyingMemory.StuffedToy stuffedToy = base.properties.CurrentState.stuffedToy;
		for (int i = 0; i < stuffedToy.spreadBullets; i++)
		{
			float floatAt = stuffedToy.spreadAngle.GetFloatAt((float)i / ((float)stuffedToy.spreadBullets - 1f));
			this.projectile.Create(this.projectileRoot.transform.position, floatAt, stuffedToy.spreadSpeed, stuffedToy.musicDeathTimer);
		}
	}

	// Token: 0x06001D15 RID: 7445 RVA: 0x0010ABFC File Offset: 0x00108FFC
	private void FireSpiral()
	{
		LevelProperties.DicePalaceFlyingMemory.StuffedToy stuffedToy = base.properties.CurrentState.stuffedToy;
		this.spiralProjectile.Create(this.projectileRoot.transform.position, 0f, stuffedToy.spiralSpeed, stuffedToy.spiralMovementRate, 1);
	}

	// Token: 0x06001D16 RID: 7446 RVA: 0x0010AC50 File Offset: 0x00109050
	private IEnumerator punishment_cr()
	{
		this.timer = 0f;
		LevelProperties.DicePalaceFlyingMemory.StuffedToy p = base.properties.CurrentState.stuffedToy;
		bool speedUp = true;
		this.startedPunishment = true;
		base.animator.SetTrigger("OnNoMatch");
		AudioManager.PlayLoop("dice_palace_memory_monkey_shake");
		this.emitAudioFromObject.Add("dice_palace_memory_monkey_shake");
		while (speedUp)
		{
			if (this.speed >= p.punishSpeed)
			{
				speedUp = false;
				break;
			}
			this.speed += p.incrementSpeedBy;
			yield return null;
		}
		this.speed = p.punishSpeed;
		while (this.timer < p.punishTime && this.state == DicePalaceFlyingMemoryLevelStuffedToy.State.Closed)
		{
			this.timer += CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetTrigger("Continue");
		while (this.speed > p.bounceSpeed)
		{
			this.speed -= p.incrementSpeedBy;
			yield return null;
		}
		AudioManager.Stop("dice_palace_memory_monkey_shake");
		this.speed = p.bounceSpeed;
		this.startedPunishment = false;
		this.SFXAllowAnticipation();
		yield return null;
		yield break;
	}

	// Token: 0x06001D17 RID: 7447 RVA: 0x0010AC6C File Offset: 0x0010906C
	private IEnumerator pick_angle_cr()
	{
		LevelProperties.DicePalaceFlyingMemory.StuffedToy p = base.properties.CurrentState.stuffedToy;
		string[] angleString = p.angleString.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] countString = p.bounceCount.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] angleAddString = p.angleAdditionString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int angleIndex = UnityEngine.Random.Range(0, angleString.Length);
		int maxCountIndex = UnityEngine.Random.Range(0, countString.Length);
		int angleAddIndex = UnityEngine.Random.Range(0, angleAddString.Length);
		float chosenAngle = 0f;
		float angle = 0f;
		float angleAdd = 0f;
		float t = 0f;
		float dirChangeTime = p.directionChangeDelay;
		Parser.FloatTryParse(angleString[angleIndex], out angle);
		Parser.FloatTryParse(countString[maxCountIndex], out this.maxCount);
		Parser.FloatTryParse(angleAddString[angleAddIndex], out angleAdd);
		this.maxCount = 0f;
		this.currentAngle = angle;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(angle));
		this.sprite.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
		base.StartCoroutine(this.move_cr());
		yield return null;
		for (;;)
		{
			if (((float)this.bounceCounter >= this.maxCount && this.state == DicePalaceFlyingMemoryLevelStuffedToy.State.Closed) || this.guessedWrong)
			{
				if (this.guessedWrong)
				{
					if (!this.startedPunishment)
					{
						base.StartCoroutine(this.punishment_cr());
					}
					else
					{
						this.timer = 0f;
					}
					while (this.currentlyColliding)
					{
						yield return new WaitForEndOfFrame();
					}
					this.isMoving = false;
					while (t < dirChangeTime)
					{
						t += CupheadTime.FixedDelta;
						yield return new WaitForFixedUpdate();
					}
					this.isMoving = true;
					while (this.currentlyColliding)
					{
						yield return new WaitForEndOfFrame();
					}
					angleIndex = (angleIndex + 1) % angleString.Length;
					Parser.FloatTryParse(angleString[angleIndex], out angle);
					t = 0f;
				}
				angleAddIndex = (angleAddIndex + 1) % angleAddString.Length;
				maxCountIndex = (maxCountIndex + 1) % countString.Length;
				Parser.FloatTryParse(countString[maxCountIndex], out this.maxCount);
				Parser.FloatTryParse(angleAddString[angleAddIndex], out angleAdd);
				if (!this.guessedWrong)
				{
					chosenAngle = this.currentAngle + angleAdd;
				}
				else
				{
					chosenAngle = angle;
				}
				this.bounceCounter = 0;
				this.guessedWrong = false;
				base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(chosenAngle));
				this.sprite.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
				this.velocity = base.transform.right;
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06001D18 RID: 7448 RVA: 0x0010AC87 File Offset: 0x00109087
	public void Open()
	{
		this.state = DicePalaceFlyingMemoryLevelStuffedToy.State.Open;
		base.animator.SetTrigger("OnMatch");
		base.animator.SetBool("OnClosing", false);
		base.StartCoroutine(this.open_cr());
	}

	// Token: 0x06001D19 RID: 7449 RVA: 0x0010ACC0 File Offset: 0x001090C0
	private IEnumerator open_cr()
	{
		LevelProperties.DicePalaceFlyingMemory.StuffedToy p = base.properties.CurrentState.stuffedToy;
		int shot = 0;
		AudioManager.Stop("dice_palace_memory_monkey_shake");
		yield return base.animator.WaitForAnimationToStart(this, "Open_Attack_A", false);
		this.isMoving = false;
		base.GetComponent<DamageReceiver>().enabled = true;
		while (this.currentlyColliding)
		{
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, p.directionChangeDelay);
		yield return CupheadTime.WaitForSeconds(this, p.attackAnti);
		this.isMoving = true;
		base.animator.SetTrigger("Continue");
		while (this.state == DicePalaceFlyingMemoryLevelStuffedToy.State.Open)
		{
			Parser.IntTryParse(this.shotPattern[this.shotIndex], out shot);
			switch (shot)
			{
			case 1:
				this.FireSingle(p.regularSpeed);
				break;
			case 2:
				this.FireSpreadshot();
				break;
			case 3:
				this.FireSpiral();
				break;
			}
			yield return null;
			this.shotIndex = (this.shotIndex + 1) % this.shotPattern.Length;
			yield return CupheadTime.WaitForSeconds(this, p.shotDelayRange);
			if (this.state != DicePalaceFlyingMemoryLevelStuffedToy.State.Open)
			{
				break;
			}
			base.animator.SetTrigger("OnAttack");
			yield return base.animator.WaitForAnimationToStart(this, "Open_Attack_B", false);
			this.isMoving = false;
			yield return null;
			yield return CupheadTime.WaitForSeconds(this, p.attackAnti);
			base.animator.SetTrigger("Continue");
			this.isMoving = true;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001D1A RID: 7450 RVA: 0x0010ACDB File Offset: 0x001090DB
	public void Closed()
	{
		base.StartCoroutine(this.closed_cr());
	}

	// Token: 0x06001D1B RID: 7451 RVA: 0x0010ACEC File Offset: 0x001090EC
	private IEnumerator closed_cr()
	{
		this.state = DicePalaceFlyingMemoryLevelStuffedToy.State.Closed;
		base.animator.SetBool("OnClosing", true);
		yield return base.animator.WaitForAnimationToStart(this, "Idle_Closed", false);
		base.GetComponent<DamageReceiver>().enabled = false;
		yield return null;
		yield break;
	}

	// Token: 0x06001D1C RID: 7452 RVA: 0x0010AD07 File Offset: 0x00109107
	private void DisableDamageReceiver()
	{
		base.GetComponent<DamageReceiver>().enabled = false;
	}

	// Token: 0x06001D1D RID: 7453 RVA: 0x0010AD15 File Offset: 0x00109115
	private void ChangeLayer(int layer)
	{
		this.hand.GetComponent<SpriteRenderer>().sortingOrder = layer;
	}

	// Token: 0x06001D1E RID: 7454 RVA: 0x0010AD28 File Offset: 0x00109128
	protected IEnumerator move_cr()
	{
		bool soundLooping = true;
		this.isMoving = true;
		this.velocity = base.transform.right;
		this.speed = base.properties.CurrentState.stuffedToy.bounceSpeed;
		AudioManager.Stop("dice_palace_memory_monkey_shake");
		AudioManager.PlayLoop("dice_palace_memory_monkey_crane_movement");
		this.emitAudioFromObject.Add("dice_palace_memory_monkey_crane_movement");
		for (;;)
		{
			if (this.isMoving)
			{
				if (!soundLooping)
				{
					AudioManager.PlayLoop("dice_palace_memory_monkey_crane_movement");
					this.emitAudioFromObject.Add("dice_palace_memory_monkey_crane_movement");
					soundLooping = true;
				}
				base.transform.position += base.transform.right * this.speed * CupheadTime.FixedDelta;
			}
			else if (soundLooping)
			{
				AudioManager.Stop("dice_palace_memory_monkey_crane_movement");
				soundLooping = false;
				this.SFXAnticipationActive = false;
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06001D1F RID: 7455 RVA: 0x0010AD44 File Offset: 0x00109144
	protected override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter || phase == CollisionPhase.Stay)
		{
			this.currentlyColliding = true;
		}
		if (phase == CollisionPhase.Exit)
		{
			this.currentlyColliding = false;
		}
		if (this.currentlyColliding)
		{
			Vector3 newVelocity = this.velocity;
			newVelocity.y = Mathf.Min(newVelocity.y, -newVelocity.y);
			this.ChangeDir(newVelocity);
		}
	}

	// Token: 0x06001D20 RID: 7456 RVA: 0x0010ADB0 File Offset: 0x001091B0
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		if (phase == CollisionPhase.Enter || phase == CollisionPhase.Stay)
		{
			this.currentlyColliding = true;
		}
		if (phase == CollisionPhase.Exit)
		{
			this.currentlyColliding = false;
		}
		if (this.currentlyColliding)
		{
			Vector3 newVelocity = this.velocity;
			newVelocity.y = Mathf.Max(newVelocity.y, -newVelocity.y);
			this.ChangeDir(newVelocity);
		}
	}

	// Token: 0x06001D21 RID: 7457 RVA: 0x0010AE1C File Offset: 0x0010921C
	protected override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionWalls(hit, phase);
		if (phase == CollisionPhase.Enter || phase == CollisionPhase.Stay)
		{
			this.currentlyColliding = true;
		}
		if (phase == CollisionPhase.Exit)
		{
			this.currentlyColliding = false;
		}
		if (this.currentlyColliding)
		{
			Vector3 newVelocity = this.velocity;
			if (base.transform.position.x > 0f)
			{
				newVelocity.x = Mathf.Min(newVelocity.x, -newVelocity.x);
				this.ChangeDir(newVelocity);
			}
			else
			{
				newVelocity.x = Mathf.Max(newVelocity.x, -newVelocity.x);
				this.ChangeDir(newVelocity);
			}
		}
	}

	// Token: 0x06001D22 RID: 7458 RVA: 0x0010AECC File Offset: 0x001092CC
	protected void ChangeDir(Vector3 newVelocity)
	{
		if (this.state == DicePalaceFlyingMemoryLevelStuffedToy.State.Closed)
		{
			this.bounceCounter++;
		}
		this.velocity = newVelocity;
		this.currentAngle = Mathf.Atan2(this.velocity.y, this.velocity.x) * 57.29578f;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.currentAngle));
		this.sprite.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
	}

	// Token: 0x06001D23 RID: 7459 RVA: 0x0010AF79 File Offset: 0x00109379
	private void OnDeath()
	{
		this.StopAllCoroutines();
		AudioManager.PlayLoop("dice_palace_memory_monkey_death");
		this.emitAudioFromObject.Add("dice_palace_memory_monkey_death");
		base.animator.SetTrigger("OnDeath");
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x06001D24 RID: 7460 RVA: 0x0010AFB8 File Offset: 0x001093B8
	private IEnumerator check_boundaries_cr()
	{
		while (base.transform.position.y <= 720f && base.transform.position.y >= -720f && base.transform.position.x >= -1280f && base.transform.position.x <= 1280f)
		{
			yield return null;
		}
		base.properties.DealDamage(base.properties.CurrentHealth);
		yield break;
	}

	// Token: 0x06001D25 RID: 7461 RVA: 0x0010AFD3 File Offset: 0x001093D3
	private void AttackSFX()
	{
		AudioManager.Play("dice_palace_memory_monkey_open_attack");
		this.emitAudioFromObject.Add("dice_palace_memory_monkey_open_attack");
		this.VOXAngryActive = false;
	}

	// Token: 0x06001D26 RID: 7462 RVA: 0x0010AFF6 File Offset: 0x001093F6
	private void AttackEndSFX()
	{
		AudioManager.Play("dice_palace_memory_monkey_attack_end");
		this.emitAudioFromObject.Add("dice_palace_memory_monkey_attack_end");
	}

	// Token: 0x06001D27 RID: 7463 RVA: 0x0010B012 File Offset: 0x00109412
	private void SFXOpentoClose()
	{
		AudioManager.Play("dice_palace_memory_monkey_open_to_close");
		this.emitAudioFromObject.Add("dice_palace_memory_monkey_open_to_close");
	}

	// Token: 0x06001D28 RID: 7464 RVA: 0x0010B02E File Offset: 0x0010942E
	private void SFXShake()
	{
		AudioManager.PlayLoop("shake_sound");
		this.emitAudioFromObject.Add("shake_sound");
	}

	// Token: 0x06001D29 RID: 7465 RVA: 0x0010B04A File Offset: 0x0010944A
	private void SFXShakeStop()
	{
		AudioManager.Stop("shake_sound");
	}

	// Token: 0x06001D2A RID: 7466 RVA: 0x0010B056 File Offset: 0x00109456
	private void SFXVOXAngry()
	{
		if (!this.VOXAngryActive)
		{
			AudioManager.Play("vox_angry");
			this.emitAudioFromObject.Add("vox_angry");
			this.VOXAngryActive = true;
		}
	}

	// Token: 0x06001D2B RID: 7467 RVA: 0x0010B084 File Offset: 0x00109484
	private void SFXVOXAngryAnim()
	{
		AudioManager.Play("vox_angry");
		this.emitAudioFromObject.Add("vox_angry");
	}

	// Token: 0x06001D2C RID: 7468 RVA: 0x0010B0A0 File Offset: 0x001094A0
	private void SFXVOXAnticipation()
	{
		if (!this.SFXAnticipationActive)
		{
			AudioManager.Play("vox_anticipation");
			this.emitAudioFromObject.Add("vox_anticipation");
			this.SFXAnticipationActive = true;
		}
	}

	// Token: 0x06001D2D RID: 7469 RVA: 0x0010B0CE File Offset: 0x001094CE
	private void SFXAllowAnticipation()
	{
		this.SFXAnticipationActive = false;
	}

	// Token: 0x04002603 RID: 9731
	[SerializeField]
	private Transform projectileRoot;

	// Token: 0x04002604 RID: 9732
	[SerializeField]
	private DicePalaceFlyingMemoryMusicNote projectile;

	// Token: 0x04002605 RID: 9733
	[SerializeField]
	private DicePalaceFlyingMemoryLevelSpiralProjectile spiralProjectile;

	// Token: 0x04002606 RID: 9734
	[SerializeField]
	private GameObject sprite;

	// Token: 0x04002607 RID: 9735
	[SerializeField]
	private SpriteRenderer hand;

	// Token: 0x04002608 RID: 9736
	public bool guessedWrong;

	// Token: 0x04002609 RID: 9737
	public DicePalaceFlyingMemoryLevelStuffedToy.State state;

	// Token: 0x0400260A RID: 9738
	private DamageDealer damageDealer;

	// Token: 0x0400260B RID: 9739
	private DamageReceiver damageReceiver;

	// Token: 0x0400260C RID: 9740
	private Vector3 velocity;

	// Token: 0x0400260D RID: 9741
	private int bounceCounter;

	// Token: 0x0400260E RID: 9742
	private int shotIndex;

	// Token: 0x0400260F RID: 9743
	private float speed;

	// Token: 0x04002610 RID: 9744
	private float newAngle;

	// Token: 0x04002611 RID: 9745
	private float currentAngle;

	// Token: 0x04002612 RID: 9746
	private float maxCount;

	// Token: 0x04002613 RID: 9747
	private float timer;

	// Token: 0x04002614 RID: 9748
	private bool isMoving;

	// Token: 0x04002615 RID: 9749
	private bool startedPunishment;

	// Token: 0x04002616 RID: 9750
	public bool currentlyColliding;

	// Token: 0x04002617 RID: 9751
	private string[] shotPattern;

	// Token: 0x04002618 RID: 9752
	private bool VOXAngryActive;

	// Token: 0x04002619 RID: 9753
	private bool SFXAnticipationActive;

	// Token: 0x020005CD RID: 1485
	public enum State
	{
		// Token: 0x0400261B RID: 9755
		Open,
		// Token: 0x0400261C RID: 9756
		Closed
	}
}
