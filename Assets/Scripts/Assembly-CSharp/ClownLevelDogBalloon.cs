using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000565 RID: 1381
public class ClownLevelDogBalloon : AbstractProjectile
{
	// Token: 0x17000349 RID: 841
	// (get) Token: 0x060019FA RID: 6650 RVA: 0x000ED8CD File Offset: 0x000EBCCD
	// (set) Token: 0x060019FB RID: 6651 RVA: 0x000ED8D5 File Offset: 0x000EBCD5
	public ClownLevelDogBalloon.State state { get; private set; }

	// Token: 0x060019FC RID: 6652 RVA: 0x000ED8E0 File Offset: 0x000EBCE0
	protected override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		AudioManager.Play("clown_dog_balloon_regular_intro");
		this.emitAudioFromObject.Add("clown_dog_balloon_regular_intro");
	}

	// Token: 0x060019FD RID: 6653 RVA: 0x000ED930 File Offset: 0x000EBD30
	public void Init(float HP, Vector2 pos, float velocity, AbstractPlayerController player, LevelProperties.Clown.HeliumClown properties, bool flipped)
	{
		base.transform.position = pos;
		this.properties = properties;
		this.player = player;
		this.velocity = velocity;
		this.health = HP;
		if (flipped)
		{
			base.transform.SetScale(new float?(1f), new float?(-base.transform.localScale.y), new float?(1f));
		}
		this.CalculateDirection();
		this.CalculateSin();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060019FE RID: 6654 RVA: 0x000ED9C4 File Offset: 0x000EBDC4
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060019FF RID: 6655 RVA: 0x000ED9E4 File Offset: 0x000EBDE4
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		if (this.properties.dogDieOnGround && phase == CollisionPhase.Enter && this.state != ClownLevelDogBalloon.State.Unspawned)
		{
			this.state = ClownLevelDogBalloon.State.Unspawned;
			this.StopAllCoroutines();
			base.animator.SetTrigger("Death");
			AudioManager.Play("clown_dog_balloon_regular_death");
			this.emitAudioFromObject.Add("clown_dog_balloon_regular_death");
		}
	}

	// Token: 0x06001A00 RID: 6656 RVA: 0x000EDA54 File Offset: 0x000EBE54
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f && this.state != ClownLevelDogBalloon.State.Unspawned)
		{
			this.state = ClownLevelDogBalloon.State.Unspawned;
			this.StopAllCoroutines();
			base.animator.SetTrigger("Death");
			AudioManager.Play("clown_dog_balloon_regular_death");
			this.emitAudioFromObject.Add("clown_dog_balloon_regular_death");
		}
	}

	// Token: 0x06001A01 RID: 6657 RVA: 0x000EDAC7 File Offset: 0x000EBEC7
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001A02 RID: 6658 RVA: 0x000EDAE8 File Offset: 0x000EBEE8
	private void CalculateSin()
	{
		Vector2 zero = Vector2.zero;
		zero.x = (this.player.transform.position.x + base.transform.position.x) / 2f;
		zero.y = (this.player.transform.position.y + base.transform.position.y) / 2f;
		float num = -((this.player.transform.position.x - base.transform.position.x) / (this.player.transform.position.y - base.transform.position.y));
		float num2 = zero.y - num * zero.x;
		Vector2 zero2 = Vector2.zero;
		zero2.x = zero.x + 1f;
		zero2.y = num * zero2.x + num2;
		this.normalized = Vector3.zero;
		this.normalized = zero2 - zero;
		this.normalized.Normalize();
	}

	// Token: 0x06001A03 RID: 6659 RVA: 0x000EDC3C File Offset: 0x000EC03C
	private void CalculateDirection()
	{
		float x = this.player.transform.position.x - base.transform.position.x;
		float y = this.player.transform.position.y - base.transform.position.y;
		float value = Mathf.Atan2(y, x) * 57.29578f;
		this.pointAtPlayer = MathUtils.AngleToDirection(value);
		base.transform.SetEulerAngles(null, null, new float?(value));
	}

	// Token: 0x06001A04 RID: 6660 RVA: 0x000EDCEC File Offset: 0x000EC0EC
	private IEnumerator move_cr()
	{
		Vector3 pos = base.transform.position;
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", false, true);
		while (base.transform.position.y > -560f)
		{
			this.angle += 10f * CupheadTime.Delta;
			if (CupheadTime.Delta != 0f)
			{
				pos += this.normalized * Mathf.Sin(this.angle) * 2f;
			}
			pos += this.pointAtPlayer * this.velocity * CupheadTime.Delta;
			base.transform.position = pos;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001A05 RID: 6661 RVA: 0x000EDD07 File Offset: 0x000EC107
	private void ChompSound()
	{
		AudioManager.Play("clown_dog_balloon_regular_chomp");
		this.emitAudioFromObject.Add("clown_dog_balloon_regular_chomp");
	}

	// Token: 0x06001A06 RID: 6662 RVA: 0x000EDD23 File Offset: 0x000EC123
	protected override void Die()
	{
		AudioManager.Play("clown_dog_balloon_regular_death");
		this.emitAudioFromObject.Add("clown_dog_balloon_regular_death");
		base.Die();
		this.StopAllCoroutines();
		base.GetComponent<SpriteRenderer>().enabled = false;
	}

	// Token: 0x04002320 RID: 8992
	public LevelProperties.Clown.HeliumClown properties;

	// Token: 0x04002321 RID: 8993
	private AbstractPlayerController player;

	// Token: 0x04002322 RID: 8994
	private Vector3 pointAtPlayer;

	// Token: 0x04002323 RID: 8995
	private Vector3 normalized;

	// Token: 0x04002324 RID: 8996
	private float health;

	// Token: 0x04002325 RID: 8997
	private float pointAt;

	// Token: 0x04002326 RID: 8998
	private float velocity;

	// Token: 0x04002327 RID: 8999
	private float angle;

	// Token: 0x04002328 RID: 9000
	private DamageReceiver damageReceiver;

	// Token: 0x02000566 RID: 1382
	public enum State
	{
		// Token: 0x0400232A RID: 9002
		Spawned,
		// Token: 0x0400232B RID: 9003
		Unspawned
	}
}
