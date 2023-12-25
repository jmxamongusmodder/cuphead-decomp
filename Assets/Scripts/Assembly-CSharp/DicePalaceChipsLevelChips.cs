using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005AB RID: 1451
public class DicePalaceChipsLevelChips : LevelProperties.DicePalaceChips.Entity
{
	// Token: 0x06001BF1 RID: 7153 RVA: 0x000FFEED File Offset: 0x000FE2ED
	protected override void Awake()
	{
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.Awake();
	}

	// Token: 0x06001BF2 RID: 7154 RVA: 0x000FFF18 File Offset: 0x000FE318
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001BF3 RID: 7155 RVA: 0x000FFF2B File Offset: 0x000FE32B
	protected virtual float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x06001BF4 RID: 7156 RVA: 0x000FFF4C File Offset: 0x000FE34C
	public override void LevelInit(LevelProperties.DicePalaceChips properties)
	{
		Level.Current.OnLevelStartEvent += this.StartAttacking;
		Level.Current.OnWinEvent += this.Death;
		this.leftScreenXPos = (float)Level.Current.Left + 100f;
		this.rightScreenXPos = (float)Level.Current.Right - 100f;
		this.rightScreenXPosStart = this.chips[0].chipTransform.position.x;
		for (int i = 0; i < this.chips.Length; i++)
		{
			this.chips[i].startPosition = this.chips[i].chipTransform.position;
		}
		this.currentAttackCount = 0;
		base.LevelInit(properties);
	}

	// Token: 0x06001BF5 RID: 7157 RVA: 0x00100019 File Offset: 0x000FE419
	private void StartAttacking()
	{
		base.StartCoroutine(this.chipAttack_cr());
	}

	// Token: 0x06001BF6 RID: 7158 RVA: 0x00100028 File Offset: 0x000FE428
	private IEnumerator chipAttack_cr()
	{
		LevelProperties.DicePalaceChips.Chips p = base.properties.CurrentState.chips;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.chips.initialAttackDelay);
		int mainStringIndex = UnityEngine.Random.Range(0, p.chipAttackString.Length);
		int dir = -1;
		int attackIndex = UnityEngine.Random.Range(0, this.maxAttacksPerCycle);
		for (;;)
		{
			string[] currentAttackChips = p.chipAttackString[mainStringIndex].Split(new char[]
			{
				','
			});
			this.maxAttacksPerCycle = currentAttackChips.Length;
			for (int j = 0; j < this.chips.Length; j++)
			{
				float rotationSpeed = (!Rand.Bool()) ? -5f : 5f;
				this.chips[j].rotationSpeed = rotationSpeed;
			}
			base.animator.SetBool("IsSpread", true);
			yield return base.animator.WaitForAnimationToStart(this, "Spread_Open", false);
			float startPos = this.chips[this.chips.Length - 1].chipTransform.position.y;
			float frameTime = 0f;
			float time = 1.5f;
			float t = 0f;
			int counter = 0;
			while (t < time)
			{
				float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
				frameTime += CupheadTime.Delta * base.animator.speed;
				t += CupheadTime.Delta * base.animator.speed;
				if (frameTime > 0.041666668f)
				{
					frameTime -= 0.041666668f;
					for (int k = this.chips.Length - 1; k >= 0; k--)
					{
						float num = (k != 0) ? (this.chips[k].chipTransform.GetComponent<Renderer>().bounds.size.y / 1.7f) : (this.chips[k].chipTransform.GetComponent<Renderer>().bounds.size.y / 5.5f);
						float b = startPos + (float)counter * num;
						Vector3 position = this.chips[k].chipTransform.position;
						position.y = Mathf.Lerp(position.y, b, val);
						this.chips[k].chipTransform.position = position;
						counter = (counter + 1) % this.chips.Length;
						float num2 = Mathf.Sin(t / 0.7f);
						this.chips[k].chipTransform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, num2 * this.chips[k].rotationSpeed));
					}
				}
				yield return null;
			}
			this.currentlyFloating = true;
			foreach (DicePalaceChipsLevelChips.ChipPieces chipPieces in this.chips)
			{
				base.StartCoroutine(this.rotate_chips_cr(chipPieces.chipTransform, chipPieces.rotationSpeed, 0.7f, t));
			}
			yield return null;
			for (int i = attackIndex; i < currentAttackChips.Length; i++)
			{
				string[] currentAttackChipsMultiple = currentAttackChips[i].Split(new char[]
				{
					'-'
				});
				this.SFX_DicePalaceChipsShoot();
				foreach (string chip in currentAttackChipsMultiple)
				{
					if (chip[0] == 'D')
					{
						yield return CupheadTime.WaitForSeconds(this, Parser.FloatParse(chip.Substring(1)));
					}
					else if (this.currentAttackCount < this.maxAttacksPerCycle - 1)
					{
						base.StartCoroutine(this.moveChip_cr(base.transform.GetChild(Parser.IntParse(chip) - 1).transform, dir, false));
					}
					else
					{
						base.StartCoroutine(this.moveChip_cr(base.transform.GetChild(Parser.IntParse(chip) - 1).transform, dir, true));
					}
				}
				this.currentAttackCount++;
				if (this.currentAttackCount >= this.maxAttacksPerCycle)
				{
					this.currentAttackCount = 0;
					attackIndex = UnityEngine.Random.Range(0, this.maxAttacksPerCycle);
				}
				else
				{
					yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.chips.chipAttackDelay);
				}
				attackIndex = 0;
			}
			while (this.chipInFlight)
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, 1f);
			time = 0.3f;
			t = 0f;
			counter = 0;
			base.animator.SetBool("IsSpread", false);
			yield return base.animator.WaitForAnimationToStart(this, "Spread_Close", false);
			yield return CupheadTime.WaitForSeconds(this, 0.4f);
			this.currentlyFloating = false;
			while (t < time)
			{
				float val2 = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
				for (int n = this.chips.Length - 1; n >= 0; n--)
				{
					float num3 = 0f;
					if (n != this.chips.Length - 1)
					{
						num3 = 10f;
					}
					float b2 = this.chips[n].startPosition.y - (float)counter * num3;
					Vector3 position2 = this.chips[n].chipTransform.position;
					position2.y = Mathf.Lerp(position2.y, b2, val2);
					this.chips[n].chipTransform.position = position2;
					counter = (counter + 1) % this.chips.Length;
				}
				t += CupheadTime.Delta;
				yield return null;
			}
			dir *= -1;
			yield return null;
			mainStringIndex = (mainStringIndex + 1) % p.chipAttackString.Length;
			float tt = 0f;
			while (tt < base.properties.CurrentState.chips.attackCycleDelay)
			{
				tt += CupheadTime.Delta * base.animator.speed;
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06001BF7 RID: 7159 RVA: 0x00100044 File Offset: 0x000FE444
	private void FlipSprite()
	{
		this.mainLayer.transform.SetScale(new float?(-this.mainLayer.transform.localScale.x), new float?(1f), new float?(1f));
		foreach (DicePalaceChipsLevelChips.ChipPieces chipPieces in this.chips)
		{
			Vector3 position = chipPieces.chipTransform.position;
			position.y = chipPieces.startPosition.y;
			chipPieces.chipTransform.position = position;
		}
	}

	// Token: 0x06001BF8 RID: 7160 RVA: 0x001000E0 File Offset: 0x000FE4E0
	private IEnumerator moveChip_cr(Transform chip, int dir, bool lastChipOfCycle)
	{
		this.chipInFlight = lastChipOfCycle;
		float start = (dir != 1) ? this.rightScreenXPos : this.leftScreenXPos;
		float end = (dir != 1) ? this.leftScreenXPos : this.rightScreenXPos;
		Vector3 pos = chip.position;
		if (this.firstTimeMoving)
		{
			start = this.rightScreenXPosStart;
		}
		float pct = 0f;
		while (pct < 1f)
		{
			pos.x = start + (end - start) * pct;
			chip.position = pos;
			pct += CupheadTime.Delta * base.properties.CurrentState.chips.chipSpeedMultiplier * this.hitPauseCoefficient() * base.animator.speed;
			yield return null;
		}
		pos.x = end;
		chip.position = pos;
		this.chipInFlight = false;
		if (lastChipOfCycle)
		{
			this.firstTimeMoving = false;
		}
		yield break;
	}

	// Token: 0x06001BF9 RID: 7161 RVA: 0x00100110 File Offset: 0x000FE510
	private IEnumerator rotate_chips_cr(Transform chip, float speed, float time, float t)
	{
		while (this.currentlyFloating)
		{
			t += CupheadTime.Delta;
			float phase = Mathf.Sin(t / time);
			chip.localRotation = Quaternion.Euler(new Vector3(0f, 0f, phase * speed));
			yield return null;
		}
		chip.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
		yield return null;
		yield break;
	}

	// Token: 0x06001BFA RID: 7162 RVA: 0x00100148 File Offset: 0x000FE548
	private void Death()
	{
		this.StopAllCoroutines();
		base.animator.SetBool("IsSpread", true);
		base.animator.SetTrigger("OnDeath");
		this.chips[0].chipTransform.SetScale(new float?(this.mainLayer.transform.localScale.x), new float?(1f), new float?(1f));
		base.StartCoroutine(this.head_fall_cr());
		for (int i = 1; i < this.chips.Length; i++)
		{
			base.StartCoroutine(this.chips_die(this.chips[i].chipTransform));
		}
	}

	// Token: 0x06001BFB RID: 7163 RVA: 0x00100200 File Offset: 0x000FE600
	private IEnumerator chips_die(Transform chip)
	{
		float speed = 2500f;
		float angle = (float)UnityEngine.Random.Range(0, 360);
		Vector3 dir = MathUtils.AngleToDirection(-angle);
		chip.GetComponent<Collider2D>().enabled = false;
		for (;;)
		{
			chip.position += dir * speed * CupheadTime.FixedDelta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001BFC RID: 7164 RVA: 0x0010021B File Offset: 0x000FE61B
	private void SpawnHat()
	{
		this.hat.SetActive(true);
		base.StartCoroutine(this.hat_fall_cr());
	}

	// Token: 0x06001BFD RID: 7165 RVA: 0x00100238 File Offset: 0x000FE638
	private IEnumerator head_fall_cr()
	{
		float velocity = 800f;
		float posY = (float)Level.Current.Ground + this.chips[0].chipTransform.GetComponent<Collider2D>().bounds.size.y / 1.2f;
		while (this.chips[0].chipTransform.position.y > posY)
		{
			this.chips[0].chipTransform.position += Vector3.down * velocity * CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetTrigger("Continue");
		yield return null;
		yield break;
	}

	// Token: 0x06001BFE RID: 7166 RVA: 0x00100254 File Offset: 0x000FE654
	private IEnumerator hat_fall_cr()
	{
		float velocity = 30f;
		while (this.hat.transform.position.y > -250f)
		{
			this.hat.transform.position += Vector3.down * velocity * CupheadTime.Delta;
			yield return null;
		}
		this.hat.GetComponent<Animator>().SetTrigger("Continue");
		yield return null;
		yield break;
	}

	// Token: 0x06001BFF RID: 7167 RVA: 0x0010026F File Offset: 0x000FE66F
	private void SFX_DicePalaceChipsIntro()
	{
		AudioManager.Play("chips_intro");
		this.emitAudioFromObject.Add("chips_intro");
		AudioManager.Play("vox_intro");
		this.emitAudioFromObject.Add("vox_intro");
	}

	// Token: 0x06001C00 RID: 7168 RVA: 0x001002A8 File Offset: 0x000FE6A8
	private void SFX_DicePalaceChipsDeath()
	{
		if (!this.DeathSoundPlaying)
		{
			AudioManager.PlayLoop("chips_death");
			this.emitAudioFromObject.Add("chips_death");
			AudioManager.Play("vox_die");
			this.emitAudioFromObject.Add("vox_die");
			this.DeathSoundPlaying = true;
		}
	}

	// Token: 0x06001C01 RID: 7169 RVA: 0x001002FC File Offset: 0x000FE6FC
	private void SFX_DicePalaceChipsExpand()
	{
		if (!this.ExpandSoundPlaying)
		{
			AudioManager.Play("chips_expand");
			this.emitAudioFromObject.Add("chips_expand");
			AudioManager.Play("vox_idle");
			this.emitAudioFromObject.Add("vox_idle");
			this.ExpandSoundPlaying = true;
		}
	}

	// Token: 0x06001C02 RID: 7170 RVA: 0x0010034F File Offset: 0x000FE74F
	private void SFX_DicePalaceChipsRetract()
	{
		AudioManager.Play("chips_retract");
		AudioManager.Play("vox_idle");
		this.ExpandSoundPlaying = false;
	}

	// Token: 0x06001C03 RID: 7171 RVA: 0x0010036C File Offset: 0x000FE76C
	private void SFX_DicePalaceChipsShoot()
	{
		AudioManager.Play("chips_shoot");
		this.emitAudioFromObject.Add("chips_shoot");
	}

	// Token: 0x06001C04 RID: 7172 RVA: 0x00100388 File Offset: 0x000FE788
	private void SFX_DicePalaceChipsSpinLoop()
	{
		if (!this.SpinSoundPlaying)
		{
			AudioManager.PlayLoop("chips_spin_loop");
			this.emitAudioFromObject.Add("chips_spin_loop");
			this.SpinSoundPlaying = true;
		}
	}

	// Token: 0x06001C05 RID: 7173 RVA: 0x001003B6 File Offset: 0x000FE7B6
	private void SFX_DicePalaceChipsSpinLoopStop()
	{
		AudioManager.Stop("chips_spin_loop");
		this.SpinSoundPlaying = false;
	}

	// Token: 0x06001C06 RID: 7174 RVA: 0x001003C9 File Offset: 0x000FE7C9
	private void SFX_DicePalaceChipsBounce()
	{
		AudioManager.Play("chips_bounce");
	}

	// Token: 0x04002505 RID: 9477
	private const float FRAME_TIME = 0.041666668f;

	// Token: 0x04002506 RID: 9478
	[SerializeField]
	private DicePalaceChipsLevelChips.ChipPieces[] chips;

	// Token: 0x04002507 RID: 9479
	[SerializeField]
	private Transform mainLayer;

	// Token: 0x04002508 RID: 9480
	[SerializeField]
	private GameObject hat;

	// Token: 0x04002509 RID: 9481
	private float leftScreenXPos;

	// Token: 0x0400250A RID: 9482
	private float rightScreenXPos;

	// Token: 0x0400250B RID: 9483
	private float rightScreenXPosStart;

	// Token: 0x0400250C RID: 9484
	private int currentAttackCount;

	// Token: 0x0400250D RID: 9485
	private int maxAttacksPerCycle;

	// Token: 0x0400250E RID: 9486
	private bool chipInFlight;

	// Token: 0x0400250F RID: 9487
	private bool currentlyFloating;

	// Token: 0x04002510 RID: 9488
	private bool firstTimeMoving = true;

	// Token: 0x04002511 RID: 9489
	private DamageReceiver damageReceiver;

	// Token: 0x04002512 RID: 9490
	private bool DeathSoundPlaying;

	// Token: 0x04002513 RID: 9491
	private bool SpinSoundPlaying;

	// Token: 0x04002514 RID: 9492
	private bool ExpandSoundPlaying;

	// Token: 0x020005AC RID: 1452
	[Serializable]
	public class ChipPieces
	{
		// Token: 0x04002515 RID: 9493
		public Transform chipTransform;

		// Token: 0x04002516 RID: 9494
		public Vector3 startPosition;

		// Token: 0x04002517 RID: 9495
		public float rotationSpeed;
	}
}
