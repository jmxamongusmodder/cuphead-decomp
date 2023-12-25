using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004D4 RID: 1236
public class AirshipJellyLevelJelly : LevelProperties.AirshipJelly.Entity
{
	// Token: 0x06001511 RID: 5393 RVA: 0x000BD2DC File Offset: 0x000BB6DC
	protected override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06001512 RID: 5394 RVA: 0x000BD308 File Offset: 0x000BB708
	private void Start()
	{
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		this.startColor = base.GetComponent<SpriteRenderer>().color;
		Level.Current.OnLevelStartEvent += this.OnLevelStart;
		CupheadLevelCamera.Current.StartFloat(25f, 3f);
	}

	// Token: 0x06001513 RID: 5395 RVA: 0x000BD35C File Offset: 0x000BB75C
	public override void LevelInit(LevelProperties.AirshipJelly properties)
	{
		base.LevelInit(properties);
		this.knobSwitch = AirshipJellyLevelKnob.Create(this);
		this.knobSwitch.OnActivate += this.OnKnobParry;
		this.knobSwitch.OnPrePauseActivate += this.OnKnobPreParry;
		this.maxHealth = properties.CurrentHealth;
		this.defaultSpeed = properties.CurrentState.main.speed.min;
		this.speed = this.defaultSpeed;
		this.damageDealer = new DamageDealer(1f, 0.3f, DamageDealer.DamageSource.Enemy, true, false, false);
	}

	// Token: 0x06001514 RID: 5396 RVA: 0x000BD3F8 File Offset: 0x000BB7F8
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
		base.StartCoroutine(this.flash_cr());
		this.GetNewSpeed();
		if (base.properties.CurrentHealth <= 0f && this.state != AirshipJellyLevelJelly.State.Dead)
		{
			this.state = AirshipJellyLevelJelly.State.Dead;
			base.animator.Play("Death");
		}
	}

	// Token: 0x06001515 RID: 5397 RVA: 0x000BD461 File Offset: 0x000BB861
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001516 RID: 5398 RVA: 0x000BD479 File Offset: 0x000BB879
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001517 RID: 5399 RVA: 0x000BD49B File Offset: 0x000BB89B
	private void OnLevelStart()
	{
		this.state = AirshipJellyLevelJelly.State.Running;
		base.StartCoroutine(this.start_cr());
	}

	// Token: 0x06001518 RID: 5400 RVA: 0x000BD4B4 File Offset: 0x000BB8B4
	private void GetNewSpeed()
	{
		MinMax minMax = base.properties.CurrentState.main.speed;
		float num = base.properties.CurrentHealth / this.maxHealth;
		float num2 = 1f - num;
		this.speed = this.defaultSpeed + minMax.max * num2;
	}

	// Token: 0x06001519 RID: 5401 RVA: 0x000BD508 File Offset: 0x000BB908
	private void OnTurnComplete()
	{
		base.transform.SetScale(new float?(-base.transform.localScale.x), null, null);
	}

	// Token: 0x0600151A RID: 5402 RVA: 0x000BD54B File Offset: 0x000BB94B
	private void OnKnobParry()
	{
		base.StartCoroutine(this.hurt_cr());
	}

	// Token: 0x0600151B RID: 5403 RVA: 0x000BD55A File Offset: 0x000BB95A
	private void OnKnobPreParry()
	{
		AudioManager.Play("levels_airship_jelly_hit");
		this.smashEffect.Create(this.knobRoot.position);
		CupheadLevelCamera.Current.Shake(10f, 0.6f, false);
	}

	// Token: 0x0600151C RID: 5404 RVA: 0x000BD592 File Offset: 0x000BB992
	private void SfxWalk()
	{
		AudioManager.Play("levels_airship_jelly_walk");
	}

	// Token: 0x0600151D RID: 5405 RVA: 0x000BD59E File Offset: 0x000BB99E
	private void ResetMove()
	{
		if (this.moveCoroutine != null)
		{
			base.StopCoroutine(this.moveCoroutine);
			this.moveCoroutine = null;
		}
		this.moveCoroutine = base.StartCoroutine(this.jelly_cr());
	}

	// Token: 0x0600151E RID: 5406 RVA: 0x000BD5D0 File Offset: 0x000BB9D0
	private IEnumerator start_cr()
	{
		base.animator.SetTrigger("OnIntroComplete");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro_Transition", false, true);
		this.ResetMove();
		yield break;
	}

	// Token: 0x0600151F RID: 5407 RVA: 0x000BD5EC File Offset: 0x000BB9EC
	private IEnumerator jelly_cr()
	{
		while (base.properties == null)
		{
		}
		float offset = 100f;
		for (;;)
		{
			Vector3 pos = base.transform.position;
			if (this.direction == AirshipJellyLevelJelly.Direction.Left)
			{
				while (base.transform.position.x > -640f + offset)
				{
					if (!this.Moving)
					{
						yield return base.StartCoroutine(this.waitForMove_cr());
					}
					pos.x = Mathf.MoveTowards(base.transform.position.x, -640f + offset, this.speed * CupheadTime.Delta);
					base.transform.position = pos;
					yield return null;
				}
				base.animator.SetTrigger("OnTurn");
				this.direction = AirshipJellyLevelJelly.Direction.Right;
			}
			else if (this.direction == AirshipJellyLevelJelly.Direction.Right)
			{
				while (base.transform.position.x < 640f - offset)
				{
					if (!this.Moving)
					{
						yield return base.StartCoroutine(this.waitForMove_cr());
					}
					pos.x = Mathf.MoveTowards(base.transform.position.x, 640f - offset, this.speed * CupheadTime.Delta);
					base.transform.position = pos;
					yield return null;
				}
				base.animator.SetTrigger("OnTurn");
				this.direction = AirshipJellyLevelJelly.Direction.Left;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x17000317 RID: 791
	// (get) Token: 0x06001520 RID: 5408 RVA: 0x000BD607 File Offset: 0x000BBA07
	private bool Moving
	{
		get
		{
			return this.state == AirshipJellyLevelJelly.State.Running;
		}
	}

	// Token: 0x06001521 RID: 5409 RVA: 0x000BD614 File Offset: 0x000BBA14
	private IEnumerator waitForMove_cr()
	{
		while (!this.Moving)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001522 RID: 5410 RVA: 0x000BD630 File Offset: 0x000BBA30
	private IEnumerator hurt_cr()
	{
		base.properties.DealDamage(base.properties.CurrentState.main.parryDamage);
		this.GetNewSpeed();
		this.knobSprite.enabled = false;
		this.knobSwitch.enabled = false;
		AudioManager.Play("levels_airship_jelly_hurt");
		if (base.properties.CurrentHealth <= 0f)
		{
			this.state = AirshipJellyLevelJelly.State.Dead;
			base.animator.Play("Death");
		}
		else
		{
			base.animator.SetTrigger("OnHurt");
			AirshipJellyLevelJelly.State lastState = this.state;
			this.state = AirshipJellyLevelJelly.State.Hurt;
			this.ResetMove();
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.main.hurtDelay);
			this.state = lastState;
			base.animator.SetTrigger("OnHurtComplete");
			yield return base.animator.WaitForAnimationToEnd(this, "Hurt_Loop", false, true);
			this.state = AirshipJellyLevelJelly.State.Running;
			base.StartCoroutine(this.enableKnob_cr());
		}
		yield break;
	}

	// Token: 0x06001523 RID: 5411 RVA: 0x000BD64C File Offset: 0x000BBA4C
	private IEnumerator enableKnob_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.main.orbDelay);
		this.knobSprite.enabled = true;
		this.knobSwitch.enabled = true;
		yield break;
	}

	// Token: 0x06001524 RID: 5412 RVA: 0x000BD668 File Offset: 0x000BBA68
	private void SetColor(float t)
	{
		Color color = Color.Lerp(this.flashColor, Color.black, t);
		this.spriteRenderer.color = color;
	}

	// Token: 0x06001525 RID: 5413 RVA: 0x000BD694 File Offset: 0x000BBA94
	private IEnumerator flash_cr()
	{
		float t = 0f;
		float time = 0.15f;
		while (t < time)
		{
			float val = t / time;
			this.SetColor(val);
			t += Time.deltaTime;
			yield return null;
		}
		base.GetComponent<SpriteRenderer>().color = this.startColor;
		yield break;
	}

	// Token: 0x04001E73 RID: 7795
	private Color startColor;

	// Token: 0x04001E74 RID: 7796
	private Color flashColor = Color.red;

	// Token: 0x04001E75 RID: 7797
	public Transform knobRoot;

	// Token: 0x04001E76 RID: 7798
	[SerializeField]
	private SpriteRenderer knobSprite;

	// Token: 0x04001E77 RID: 7799
	[Space(10f)]
	[SerializeField]
	private Effect smashEffect;

	// Token: 0x04001E78 RID: 7800
	private float speed;

	// Token: 0x04001E79 RID: 7801
	private float defaultSpeed;

	// Token: 0x04001E7A RID: 7802
	private float maxHealth;

	// Token: 0x04001E7B RID: 7803
	private SpriteRenderer spriteRenderer;

	// Token: 0x04001E7C RID: 7804
	private DamageDealer damageDealer;

	// Token: 0x04001E7D RID: 7805
	private DamageReceiver damageReceiver;

	// Token: 0x04001E7E RID: 7806
	private AirshipJellyLevelKnob knobSwitch;

	// Token: 0x04001E7F RID: 7807
	private AirshipJellyLevelJelly.State state;

	// Token: 0x04001E80 RID: 7808
	private AirshipJellyLevelJelly.Direction direction = AirshipJellyLevelJelly.Direction.Left;

	// Token: 0x04001E81 RID: 7809
	private const float MIN_X = -550f;

	// Token: 0x04001E82 RID: 7810
	private const float MAX_X = 550f;

	// Token: 0x04001E83 RID: 7811
	private Coroutine moveCoroutine;

	// Token: 0x020004D5 RID: 1237
	public enum State
	{
		// Token: 0x04001E85 RID: 7813
		Init,
		// Token: 0x04001E86 RID: 7814
		Running,
		// Token: 0x04001E87 RID: 7815
		Hurt,
		// Token: 0x04001E88 RID: 7816
		Dead
	}

	// Token: 0x020004D6 RID: 1238
	public enum Direction
	{
		// Token: 0x04001E8A RID: 7818
		Right,
		// Token: 0x04001E8B RID: 7819
		Left
	}
}
