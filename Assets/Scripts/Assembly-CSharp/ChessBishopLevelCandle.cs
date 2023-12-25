using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000539 RID: 1337
public class ChessBishopLevelCandle : AbstractCollidableObject
{
	// Token: 0x17000332 RID: 818
	// (get) Token: 0x06001845 RID: 6213 RVA: 0x000DBD12 File Offset: 0x000DA112
	// (set) Token: 0x06001846 RID: 6214 RVA: 0x000DBD1A File Offset: 0x000DA11A
	public bool isLit { get; private set; }

	// Token: 0x06001847 RID: 6215 RVA: 0x000DBD24 File Offset: 0x000DA124
	public void Init(float distToBlowout)
	{
		this.glow.SetActive(false);
		this.distToBlowout = distToBlowout;
		this.basePos = base.transform.position;
		this.shadowPos = this.shadow.transform.position;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001848 RID: 6216 RVA: 0x000DBD78 File Offset: 0x000DA178
	private float EaseOvershoot(float start, float end, float value, float overshoot)
	{
		float num = Mathf.Lerp(start, end, value);
		return num + Mathf.Sin(value * 3.1415927f) * ((end - start) * overshoot);
	}

	// Token: 0x06001849 RID: 6217 RVA: 0x000DBDA8 File Offset: 0x000DA1A8
	private IEnumerator intro_cr()
	{
		this.introPos = this.introCandle.transform.position;
		yield return null;
		while (!this.introCandle.moving)
		{
			yield return null;
		}
		float t = 0f;
		while (t < 1f)
		{
			this.introCandle.transform.position = this.introPos + Vector3.up * 800f * EaseUtils.EaseOutSine(0f, 1f, Mathf.InverseLerp(0f, 1f, t));
			t += 0.041666668f;
			yield return CupheadTime.WaitForSeconds(this, 0.041666668f);
		}
		base.transform.position = this.basePos + Vector3.up * 800f;
		base.animator.Play("IntroToIdle");
		t = 0f;
		while (t < 1f)
		{
			base.transform.position = this.basePos + (Vector3.up * 800f * this.EaseOvershoot(1f, 0f, t, this.introOvershoot) + this.floatAmplitude * Vector3.up);
			t += 0.041666668f;
			yield return CupheadTime.WaitForSeconds(this, 0.041666668f);
		}
		this.introPlaying = false;
		yield break;
	}

	// Token: 0x0600184A RID: 6218 RVA: 0x000DBDC4 File Offset: 0x000DA1C4
	private bool PlayerInRange()
	{
		if (this.player1 && !this.player1.IsDead)
		{
			float num = Vector3.SqrMagnitude(this.blowoutRoot.position - this.player1.center);
			if (num < this.distToBlowout * this.distToBlowout && this.player1.center != this.lastPlayer1Position)
			{
				return true;
			}
		}
		if (this.player2 && !this.player2.IsDead)
		{
			float num2 = Vector3.SqrMagnitude(this.blowoutRoot.position - this.player2.center);
			if (num2 < this.distToBlowout * this.distToBlowout && this.player2.center != this.lastPlayer2Position)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600184B RID: 6219 RVA: 0x000DBEB4 File Offset: 0x000DA2B4
	private void Update()
	{
		this.player1 = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		this.player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (this.PlayerInRange())
		{
			if (this.isLit)
			{
				this.StopAllCoroutines();
				base.StartCoroutine(this.light_out_cr());
			}
			else if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
			{
				base.animator.Play("Walkby", 0, 0f);
			}
		}
		if (this.player1 && !this.player1.IsDead)
		{
			this.lastPlayer1Position = this.player1.center;
		}
		if (this.player2 && !this.player2.IsDead)
		{
			this.lastPlayer2Position = this.player2.center;
		}
		this.stepTimer += CupheadTime.Delta;
		while (this.stepTimer > 0.041666668f)
		{
			this.Step();
			this.stepTimer -= 0.041666668f;
		}
	}

	// Token: 0x0600184C RID: 6220 RVA: 0x000DBFE4 File Offset: 0x000DA3E4
	private void Step()
	{
		this.shadow.transform.position = this.shadowPos;
		if (this.introPlaying)
		{
			return;
		}
		base.transform.position = this.basePos;
		if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || base.animator.GetCurrentAnimatorStateInfo(0).IsName("Lit"))
		{
			this.easeToFloat = Mathf.Clamp(0f, 1f, this.easeToFloat + 0.041666668f);
			float num = (!this.offsetFloat) ? base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime : (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime + 0.25f);
			base.transform.position += Vector3.up * Mathf.Cos(num * 3.1415927f * 2f) * this.floatAmplitude * this.easeToFloat;
		}
		else
		{
			this.easeToFloat = 0f;
		}
	}

	// Token: 0x0600184D RID: 6221 RVA: 0x000DC119 File Offset: 0x000DA519
	public void LightUp()
	{
		this.isLit = true;
		this.StopAllCoroutines();
		base.StartCoroutine(this.light_up_cr());
	}

	// Token: 0x0600184E RID: 6222 RVA: 0x000DC138 File Offset: 0x000DA538
	private IEnumerator light_up_cr()
	{
		base.animator.Play((!Rand.Bool()) ? "ReigniteB" : "ReigniteA", 1);
		this.glow.SetActive(true);
		yield return base.animator.WaitForAnimationToStart(this, "None", 1, false);
		base.animator.Play("Lit", 0, base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
		this.SFX_KOG_Bishop_CandlesLightUp();
		yield break;
	}

	// Token: 0x0600184F RID: 6223 RVA: 0x000DC154 File Offset: 0x000DA554
	private IEnumerator light_out_cr()
	{
		this.isLit = false;
		this.glow.SetActive(false);
		base.animator.Play("Stagger");
		this.SFX_KOG_Bishop_CandleSnuff();
		this.smoke.transform.eulerAngles = new Vector3(0f, 0f, (float)UnityEngine.Random.Range(-5, 5));
		this.vanquishFX.transform.eulerAngles = new Vector3(0f, 0f, (float)UnityEngine.Random.Range(0, 360));
		this.vanquishSpark.transform.eulerAngles = new Vector3(0f, 0f, (float)UnityEngine.Random.Range(0, 360));
		base.animator.Play((!Rand.Bool()) ? "SmokeB" : "SmokeA", 2);
		yield return CupheadTime.WaitForSeconds(this, this.staggerLoopTime);
		base.animator.SetTrigger("EndStaggerLoop");
		yield break;
	}

	// Token: 0x06001850 RID: 6224 RVA: 0x000DC16F File Offset: 0x000DA56F
	private void SFX_KOG_Bishop_CandlesLightUp()
	{
		AudioManager.Play("sfx_dlc_kog_bishop_candleslightup");
		this.emitAudioFromObject.Add("sfx_dlc_kog_bishop_candleslightup");
	}

	// Token: 0x06001851 RID: 6225 RVA: 0x000DC18B File Offset: 0x000DA58B
	private void SFX_KOG_Bishop_CandleSnuff()
	{
		AudioManager.Play("sfx_dlc_kog_bishop_candlesnuff");
		this.emitAudioFromObject.Add("sfx_dlc_kog_bishop_candlesnuff");
	}

	// Token: 0x04002171 RID: 8561
	[SerializeField]
	private Transform blowoutRoot;

	// Token: 0x04002172 RID: 8562
	[SerializeField]
	private GameObject smoke;

	// Token: 0x04002173 RID: 8563
	[SerializeField]
	private GameObject vanquishFX;

	// Token: 0x04002174 RID: 8564
	[SerializeField]
	private GameObject vanquishSpark;

	// Token: 0x04002175 RID: 8565
	[SerializeField]
	private GameObject shadow;

	// Token: 0x04002176 RID: 8566
	[SerializeField]
	private float staggerLoopTime;

	// Token: 0x04002177 RID: 8567
	[SerializeField]
	private float floatAmplitude;

	// Token: 0x04002178 RID: 8568
	[SerializeField]
	private bool offsetFloat;

	// Token: 0x04002179 RID: 8569
	private float easeToFloat = 1f;

	// Token: 0x0400217A RID: 8570
	private Vector3 basePos;

	// Token: 0x0400217B RID: 8571
	private Vector3 shadowPos;

	// Token: 0x0400217C RID: 8572
	private Vector3 introPos;

	// Token: 0x0400217E RID: 8574
	private float distToBlowout;

	// Token: 0x0400217F RID: 8575
	private AbstractPlayerController player1;

	// Token: 0x04002180 RID: 8576
	private AbstractPlayerController player2;

	// Token: 0x04002181 RID: 8577
	private Vector3 lastPlayer1Position = Vector3.zero;

	// Token: 0x04002182 RID: 8578
	private Vector3 lastPlayer2Position = Vector3.zero;

	// Token: 0x04002183 RID: 8579
	private float stepTimer;

	// Token: 0x04002184 RID: 8580
	[SerializeField]
	private GameObject glow;

	// Token: 0x04002185 RID: 8581
	[SerializeField]
	private ChessBishopLevelIntroCandle introCandle;

	// Token: 0x04002186 RID: 8582
	private bool introPlaying = true;

	// Token: 0x04002187 RID: 8583
	[SerializeField]
	private bool isLastIntro;

	// Token: 0x04002188 RID: 8584
	[SerializeField]
	private float introOvershoot;
}
