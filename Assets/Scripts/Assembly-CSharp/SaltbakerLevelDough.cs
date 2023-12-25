using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007C6 RID: 1990
public class SaltbakerLevelDough : SaltbakerLevelPhaseOneProjectile
{
	// Token: 0x06002D09 RID: 11529 RVA: 0x001A8920 File Offset: 0x001A6D20
	public virtual SaltbakerLevelDough Init(Vector3 startPos, float speedX, float speedY, float gravity, float hp, int sortingOrder, int animalType)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = startPos;
		this.speedX = speedX;
		this.speedY = speedY;
		this.gravity = gravity;
		this.fromLeft = (speedX > 0f);
		this.hp = hp;
		this.Jump();
		base.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
		this.animalType = animalType;
		base.animator.Play(this.clipNames[animalType] + "Up");
		return this;
	}

	// Token: 0x06002D0A RID: 11530 RVA: 0x001A89AB File Offset: 0x001A6DAB
	protected override void Start()
	{
		base.Start();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06002D0B RID: 11531 RVA: 0x001A89D6 File Offset: 0x001A6DD6
	protected override bool SparksFollow()
	{
		return Rand.Bool();
	}

	// Token: 0x06002D0C RID: 11532 RVA: 0x001A89DD File Offset: 0x001A6DDD
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002D0D RID: 11533 RVA: 0x001A89FB File Offset: 0x001A6DFB
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06002D0E RID: 11534 RVA: 0x001A8A26 File Offset: 0x001A6E26
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002D0F RID: 11535 RVA: 0x001A8A44 File Offset: 0x001A6E44
	private void Jump()
	{
		base.StartCoroutine(this.jump_cr());
	}

	// Token: 0x06002D10 RID: 11536 RVA: 0x001A8A54 File Offset: 0x001A6E54
	private IEnumerator jump_cr()
	{
		base.animator.Play(UnityEngine.Random.Range(0, 4).ToString(), 1, 0f);
		base.transform.localScale = new Vector3(Mathf.Sign(this.speedX), 1f);
		float velocityX = this.speedX;
		float velocityY = this.speedY;
		float sizeX = base.GetComponent<Collider2D>().bounds.size.x;
		float sizeY = base.GetComponent<Collider2D>().bounds.size.y;
		float ground = (float)Level.Current.Ground + sizeY / 2f + 50f;
		bool jumping = false;
		bool goingUp = false;
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			velocityY = this.speedY;
			velocityX = this.speedX;
			jumping = true;
			goingUp = true;
			bool arcTriggered = false;
			base.transform.AddPosition(velocityX * CupheadTime.FixedDelta, velocityY * CupheadTime.FixedDelta, 0f);
			while (jumping)
			{
				velocityY -= this.gravity * CupheadTime.FixedDelta;
				base.transform.AddPosition(velocityX * CupheadTime.FixedDelta, velocityY * CupheadTime.FixedDelta, 0f);
				base.HandleShadow(0f, 0f);
				if (goingUp && !arcTriggered && velocityY <= this.gravity * 4f * CupheadTime.FixedDelta)
				{
					base.animator.SetTrigger("Arc");
					arcTriggered = true;
				}
				if (velocityY < 0f && goingUp)
				{
					goingUp = false;
					arcTriggered = false;
				}
				if (velocityY < 0f && jumping && base.transform.position.y - velocityY * CupheadTime.FixedDelta <= ground)
				{
					jumping = false;
					base.transform.position = new Vector3(base.transform.position.x, ground);
					base.HandleShadow(0f, 0f);
					base.animator.SetTrigger("Bounce");
				}
				if ((base.transform.position.x < (float)Level.Current.Left - sizeX && !this.fromLeft) || (base.transform.position.x > (float)Level.Current.Right + sizeX && this.fromLeft))
				{
					this.Die();
				}
				yield return wait;
			}
			yield return base.animator.WaitForAnimationToStart(this, this.clipNames[this.animalType] + "Bounce", false);
			while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.75f)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06002D11 RID: 11537 RVA: 0x001A8A70 File Offset: 0x001A6E70
	private void AniEvent_SpawnDustCloud()
	{
		this.dustEffect.Create(new Vector3(base.transform.position.x, (float)Level.Current.Ground));
	}

	// Token: 0x06002D12 RID: 11538 RVA: 0x001A8AAC File Offset: 0x001A6EAC
	protected override void Die()
	{
		this.StopAllCoroutines();
		this.coll.enabled = false;
		this.shadow.enabled = false;
		int num = UnityEngine.Random.Range(1, 4);
		for (int i = 0; i < num; i++)
		{
			this.debris.Create(base.transform.position + MathUtils.RandomPointInUnitCircle() * 20f);
		}
		int num2 = UnityEngine.Random.Range(1, 3);
		base.animator.Play("Death_" + num2);
		base.transform.localScale = new Vector3((float)MathUtils.PlusOrMinus(), (float)((num2 >= 2) ? 1 : MathUtils.PlusOrMinus()));
		if (num2 < 2)
		{
			base.transform.eulerAngles = new Vector3(0f, 0f, (float)UnityEngine.Random.Range(0, 360));
		}
		base.animator.Update(0f);
	}

	// Token: 0x06002D13 RID: 11539 RVA: 0x001A8BAA File Offset: 0x001A6FAA
	private void AnimationEvent_SFX_SALTBAKER_CookieBounce()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_cookiebounce");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p1_cookiebounce");
	}

	// Token: 0x06002D14 RID: 11540 RVA: 0x001A8BC6 File Offset: 0x001A6FC6
	private void AnimationEvent_SFX_SALTBAKER_Cookie_AnimalCamel()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_cookie_animalcamel");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p1_cookie_animalcamel");
	}

	// Token: 0x06002D15 RID: 11541 RVA: 0x001A8BE2 File Offset: 0x001A6FE2
	private void AnimationEvent_SFX_SALTBAKER_Cookie_AnimalLion()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_cookie_animalLion");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p1_cookie_animalLion");
	}

	// Token: 0x06002D16 RID: 11542 RVA: 0x001A8BFE File Offset: 0x001A6FFE
	private void AnimationEvent_SFX_SALTBAKER_Cookie_AnimalElephant()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p1_cookie_animalElephant");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p1_cookie_animalElephant");
	}

	// Token: 0x04003588 RID: 13704
	private const float GROUND_OFFSET = 50f;

	// Token: 0x04003589 RID: 13705
	private float speedX;

	// Token: 0x0400358A RID: 13706
	private float speedY;

	// Token: 0x0400358B RID: 13707
	private float gravity;

	// Token: 0x0400358C RID: 13708
	private float hp;

	// Token: 0x0400358D RID: 13709
	private bool fromLeft;

	// Token: 0x0400358E RID: 13710
	private DamageReceiver damageReceiver;

	// Token: 0x0400358F RID: 13711
	[SerializeField]
	private Collider2D coll;

	// Token: 0x04003590 RID: 13712
	[SerializeField]
	private Effect dustEffect;

	// Token: 0x04003591 RID: 13713
	[SerializeField]
	private Effect debris;

	// Token: 0x04003592 RID: 13714
	private string[] clipNames = new string[]
	{
		"Elephant",
		"Lion",
		"Camel"
	};

	// Token: 0x04003593 RID: 13715
	private int animalType;
}
