using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005D8 RID: 1496
public class DicePalacePachinkoLevelPachinko : LevelProperties.DicePalacePachinko.Entity
{
	// Token: 0x1700036E RID: 878
	// (get) Token: 0x06001D7E RID: 7550 RVA: 0x0010EC2D File Offset: 0x0010D02D
	// (set) Token: 0x06001D7F RID: 7551 RVA: 0x0010EC35 File Offset: 0x0010D035
	public bool attacking { get; private set; }

	// Token: 0x06001D80 RID: 7552 RVA: 0x0010EC40 File Offset: 0x0010D040
	protected override void Awake()
	{
		this.reversing = false;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		Level.Current.OnWinEvent += this.OnDeath;
		base.Awake();
	}

	// Token: 0x06001D81 RID: 7553 RVA: 0x0010EC9E File Offset: 0x0010D09E
	private void Update()
	{
		this.damageDealer.Update();
	}

	// Token: 0x06001D82 RID: 7554 RVA: 0x0010ECAB File Offset: 0x0010D0AB
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.pct = 1f - base.properties.CurrentHealth / this.initialHP;
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001D83 RID: 7555 RVA: 0x0010ECDC File Offset: 0x0010D0DC
	public override void LevelInit(LevelProperties.DicePalacePachinko properties)
	{
		Level.Current.OnIntroEvent += this.OnIntroEnd;
		base.LevelInit(properties);
		this.attacking = false;
		this.direction = 1;
		this.pct = 0f;
		this.initialHP = properties.CurrentHealth;
		this.baseSpeed = properties.CurrentState.boss.movementSpeed.min;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001D84 RID: 7556 RVA: 0x0010ED54 File Offset: 0x0010D154
	private IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1.2f);
		base.animator.SetTrigger("Continue");
		AudioManager.Play("dice_palace_pachinko_intro");
		this.emitAudioFromObject.Add("dice_palace_pachinko_intro");
		yield return null;
		yield break;
	}

	// Token: 0x06001D85 RID: 7557 RVA: 0x0010ED6F File Offset: 0x0010D16F
	private void OnIntroEnd()
	{
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.attack_cr());
		base.StartCoroutine(this.check_position_cr());
	}

	// Token: 0x06001D86 RID: 7558 RVA: 0x0010ED98 File Offset: 0x0010D198
	protected virtual float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x06001D87 RID: 7559 RVA: 0x0010EDBC File Offset: 0x0010D1BC
	private IEnumerator move_cr()
	{
		AudioManager.PlayLoop("dice_palace_pachinko_movement_loop");
		this.emitAudioFromObject.Add("dice_palace_pachinko_movement_loop");
		for (;;)
		{
			float speed = this.baseSpeed + (base.properties.CurrentState.boss.movementSpeed.max - base.properties.CurrentState.boss.movementSpeed.min) * this.pct;
			base.transform.position += Vector3.right * speed * (float)this.direction * CupheadTime.Delta * this.hitPauseCoefficient();
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001D88 RID: 7560 RVA: 0x0010EDD8 File Offset: 0x0010D1D8
	private IEnumerator check_position_cr()
	{
		for (;;)
		{
			if ((base.transform.position.x < -640f + base.properties.CurrentState.boss.leftBoundaryOffset && this.direction == -1) || (base.transform.position.x > 640f - base.properties.CurrentState.boss.rightBoundaryOffset && this.direction == 1))
			{
				base.StartCoroutine(this.reverse_cr());
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001D89 RID: 7561 RVA: 0x0010EDF4 File Offset: 0x0010D1F4
	private IEnumerator attack_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.boss.initialAttackDelay);
		for (;;)
		{
			base.StartCoroutine(this.lights_cr());
			base.animator.SetTrigger("OnAttack");
			yield return base.animator.WaitForAnimationToEnd(this, "Attack_Warning_Start", false, true);
			this.BeamWarning();
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.boss.warningDuration);
			base.animator.SetTrigger("Continue");
			AudioManager.Play("dice_palace_pachinko_warning_trans");
			this.emitAudioFromObject.Add("dice_palace_pachinko_warning_trans");
			this.attacking = true;
			this.BeamOn();
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.boss.beamDuration);
			this.BeamOff();
			this.attacking = false;
			base.animator.SetTrigger("OnEnd");
			AudioManager.Play("dice_palace_pachinko_trans_out");
			this.emitAudioFromObject.Add("dice_palace_pachinko_trans_out");
			yield return base.animator.WaitForAnimationToEnd(this, "Attack_Trans_Out", false, true);
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.boss.attackDelay.RandomFloat());
		}
		yield break;
	}

	// Token: 0x06001D8A RID: 7562 RVA: 0x0010EE0F File Offset: 0x0010D20F
	private void BeamWarning()
	{
		this.beam.SetActive(true);
		this.beam.GetComponent<Collider2D>().enabled = false;
		this.beam.GetComponent<SpriteRenderer>().sprite = this.beamSprites[0];
	}

	// Token: 0x06001D8B RID: 7563 RVA: 0x0010EE46 File Offset: 0x0010D246
	private void BeamOn()
	{
		this.beam.SetActive(true);
		this.beam.GetComponent<Collider2D>().enabled = true;
		this.beam.GetComponent<SpriteRenderer>().sprite = this.beamSprites[1];
	}

	// Token: 0x06001D8C RID: 7564 RVA: 0x0010EE7D File Offset: 0x0010D27D
	private void BeamOff()
	{
		this.beam.SetActive(false);
		this.beam.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x06001D8D RID: 7565 RVA: 0x0010EE9C File Offset: 0x0010D29C
	private IEnumerator lights_cr()
	{
		float fastSpeed = 6f;
		float fadeTime = 0.01f;
		foreach (Transform light in this.lights)
		{
			light.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.boss.warningDuration / (float)this.lights.Length);
		}
		bool fadingOut = false;
		while (!this.attacking)
		{
			yield return null;
		}
		this.fire.speed = fastSpeed;
		while (this.attacking)
		{
			foreach (Transform transform in this.lights)
			{
				if (fadingOut)
				{
					transform.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
				}
				else
				{
					transform.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
				}
			}
			fadingOut = !fadingOut;
			yield return CupheadTime.WaitForSeconds(this, fadeTime);
			yield return null;
		}
		base.StartCoroutine(this.fire_speed_cr(fastSpeed));
		foreach (Transform transform2 in this.lights)
		{
			transform2.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
		}
		foreach (Transform light2 in this.lights)
		{
			light2.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.boss.warningDuration / (float)this.lights.Length);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001D8E RID: 7566 RVA: 0x0010EEB8 File Offset: 0x0010D2B8
	private IEnumerator fire_speed_cr(float fastSpeed)
	{
		while (fastSpeed > 0f)
		{
			fastSpeed -= 0.1f;
			this.fire.speed = fastSpeed;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001D8F RID: 7567 RVA: 0x0010EEDC File Offset: 0x0010D2DC
	private IEnumerator reverse_cr()
	{
		if (!this.reversing)
		{
			this.reversing = true;
			this.direction *= -1;
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
			this.reversing = false;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001D90 RID: 7568 RVA: 0x0010EEF7 File Offset: 0x0010D2F7
	private void OnDeath()
	{
		AudioManager.Stop("dice_palace_pachinko_movement_loop");
		AudioManager.Play("dice_palace_pachinko_death");
		this.StopAllCoroutines();
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetTrigger("OnDeath");
	}

	// Token: 0x06001D91 RID: 7569 RVA: 0x0010EF2F File Offset: 0x0010D32F
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		this.damageDealer.DealDamage(hit);
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001D92 RID: 7570 RVA: 0x0010EF46 File Offset: 0x0010D346
	protected override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
	}

	// Token: 0x04002669 RID: 9833
	[SerializeField]
	private Animator fire;

	// Token: 0x0400266A RID: 9834
	[SerializeField]
	private Transform[] lights;

	// Token: 0x0400266B RID: 9835
	[SerializeField]
	private Sprite[] beamSprites;

	// Token: 0x0400266C RID: 9836
	[SerializeField]
	private GameObject beam;

	// Token: 0x0400266D RID: 9837
	private bool reversing;

	// Token: 0x0400266E RID: 9838
	private int direction;

	// Token: 0x0400266F RID: 9839
	private float baseSpeed;

	// Token: 0x04002670 RID: 9840
	private float pct;

	// Token: 0x04002671 RID: 9841
	private float initialHP;

	// Token: 0x04002672 RID: 9842
	private DamageDealer damageDealer;

	// Token: 0x04002673 RID: 9843
	private DamageReceiver damageReceiver;
}
