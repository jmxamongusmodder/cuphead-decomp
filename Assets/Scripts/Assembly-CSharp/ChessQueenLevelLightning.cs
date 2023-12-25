using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200054B RID: 1355
public class ChessQueenLevelLightning : AbstractProjectile
{
	// Token: 0x1700033E RID: 830
	// (get) Token: 0x06001904 RID: 6404 RVA: 0x000E2C38 File Offset: 0x000E1038
	public override float ParryMeterMultiplier
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x1700033F RID: 831
	// (get) Token: 0x06001905 RID: 6405 RVA: 0x000E2C3F File Offset: 0x000E103F
	// (set) Token: 0x06001906 RID: 6406 RVA: 0x000E2C47 File Offset: 0x000E1047
	public bool isGone { get; private set; }

	// Token: 0x06001907 RID: 6407 RVA: 0x000E2C50 File Offset: 0x000E1050
	public ChessQueenLevelLightning Create(float posX, LevelProperties.ChessQueen.Lightning properties)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = new Vector3(posX, -385f);
		this.properties = properties;
		this.lionsLandDustFX.Create(this.dropDustPos.transform.position);
		base.StartCoroutine(this.move_cr());
		return this;
	}

	// Token: 0x06001908 RID: 6408 RVA: 0x000E2CB0 File Offset: 0x000E10B0
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001909 RID: 6409 RVA: 0x000E2CCE File Offset: 0x000E10CE
	public override void OnParry(AbstractPlayerController player)
	{
		this.StopAllCoroutines();
		this.Die();
		this.isGone = true;
		base.StartCoroutine(this.death_cr());
	}

	// Token: 0x0600190A RID: 6410 RVA: 0x000E2CF0 File Offset: 0x000E10F0
	private void LateUpdate()
	{
		int num = Mathf.Clamp((int)(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime * (float)this.deathSparkSprites.Length), 0, this.deathSparkSprites.Length - 1);
		if (num < 0)
		{
			return;
		}
		this.bottomRenderer.sortingOrder = ((!ChessQueenLevelLightning.BottomInFront[num]) ? -1 : 1);
		this.topRenderer.sortingOrder = ((!ChessQueenLevelLightning.MiddleInFront[num]) ? 1 : -1);
	}

	// Token: 0x0600190B RID: 6411 RVA: 0x000E2D74 File Offset: 0x000E1174
	private IEnumerator move_cr()
	{
		this.SFX_KOG_QUEEN_ChessPiecesFall();
		base.transform.localScale = new Vector3(Mathf.Sign(base.transform.position.x), 1f);
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", false, true);
		float delayTime = this.properties.lightningDelayTime - 0.5416667f;
		yield return CupheadTime.WaitForSeconds(this, delayTime);
		this.SFX_KOG_QUEEN_ChessPieceRoar();
		this.speed = Mathf.Sign(base.transform.position.x) * -this.properties.lightningSweepSpeed;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (base.transform.position.x > (float)Level.Current.Left - 400f && base.transform.position.x < (float)Level.Current.Right + 400f)
		{
			base.transform.AddPosition(this.speed * CupheadTime.FixedDelta, 0f, 0f);
			yield return wait;
		}
		this.isGone = true;
		this.Recycle<ChessQueenLevelLightning>();
		yield break;
	}

	// Token: 0x0600190C RID: 6412 RVA: 0x000E2D90 File Offset: 0x000E1190
	private IEnumerator death_cr()
	{
		AnimationHelper animationHelper = base.GetComponent<AnimationHelper>();
		animationHelper.Speed = 0f;
		int index = Mathf.Clamp((int)(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime * (float)this.deathSparkSprites.Length), 0, this.deathSparkSprites.Length - 1);
		if (index < 0)
		{
			index = 0;
		}
		this.bottomRenderer.enabled = false;
		this.deathSparkRenderer.sprite = this.deathSparkSprites[index];
		yield return CupheadTime.WaitForSeconds(this, 0.041666668f);
		this.deathSparkRenderer.sprite = null;
		this.bottomRenderer.enabled = true;
		animationHelper.Speed = 1f;
		base.animator.Play("Death");
		this.SFX_KOG_QUEEN_ChessPiecesParried();
		base.StartCoroutine(this.SFX_KOG_QUEEN_ChessPieceMeow_cr());
		base.animator.SetTrigger("DustEnd");
		float minSpeed = this.speed * 0.2f;
		float maxSpeed = this.speed * 0.8f;
		SpriteDeathParts part = this.deathParts.CreatePart(this.bottomRenderer.transform.position);
		part.SetVelocityX(minSpeed, maxSpeed);
		part.GetComponent<SpriteRenderer>().sortingOrder = 13;
		part.transform.localScale = base.transform.localScale;
		part = this.deathParts.CreatePart(this.middleRenderer.transform.position);
		part.SetVelocityX(minSpeed, maxSpeed);
		part.GetComponent<SpriteRenderer>().sortingOrder = 14;
		part.transform.localScale = new Vector3(-base.transform.localScale.x, 1f);
		part = this.deathParts.CreatePart(this.topRenderer.transform.position);
		part.SetVelocityX(minSpeed, maxSpeed);
		part.transform.localScale = base.transform.localScale;
		part = this.deathDust.CreatePart(this.dustRenderer.transform.position);
		part.SetVelocityX(this.speed * 0.5f, this.speed * 0.5f);
		part.transform.localScale = base.transform.localScale;
		yield break;
	}

	// Token: 0x0600190D RID: 6413 RVA: 0x000E2DAB File Offset: 0x000E11AB
	private void SFX_KOG_QUEEN_ChessPieceRoar()
	{
		AudioManager.Play("sfx_dlc_kog_queen_chesspieceroar");
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_chesspieceroar");
	}

	// Token: 0x0600190E RID: 6414 RVA: 0x000E2DC7 File Offset: 0x000E11C7
	private void SFX_KOG_QUEEN_ChessPiecesFall()
	{
		AudioManager.Play("sfx_dlc_kog_queen_chesspiecesfall");
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_chesspiecesfall");
	}

	// Token: 0x0600190F RID: 6415 RVA: 0x000E2DE3 File Offset: 0x000E11E3
	private void SFX_KOG_QUEEN_ChessPiecesParried()
	{
		AudioManager.Play("sfx_dlc_kog_queen_chesspiecesparried");
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_chesspiecesparried");
	}

	// Token: 0x06001910 RID: 6416 RVA: 0x000E2E00 File Offset: 0x000E1200
	private IEnumerator SFX_KOG_QUEEN_ChessPieceMeow_cr()
	{
		AudioManager.Stop("sfx_dlc_kog_queen_chesspieceroar");
		yield return CupheadTime.WaitForSeconds(this, 0.17f);
		AudioManager.Play("sfx_dlc_kog_queen_chesspiecemeow");
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_chesspiecemeow");
		yield break;
	}

	// Token: 0x0400221E RID: 8734
	private const float YPosition = -385f;

	// Token: 0x0400221F RID: 8735
	private static readonly bool[] BottomInFront = new bool[]
	{
		true,
		true,
		true,
		false,
		false,
		false,
		false,
		false,
		true,
		true,
		true,
		false,
		false,
		false,
		false,
		false
	};

	// Token: 0x04002220 RID: 8736
	private static readonly bool[] MiddleInFront = new bool[]
	{
		false,
		false,
		false,
		false,
		true,
		true,
		true,
		false,
		false,
		false,
		false,
		false,
		true,
		true,
		true,
		false
	};

	// Token: 0x04002221 RID: 8737
	[SerializeField]
	private SpriteRenderer bottomRenderer;

	// Token: 0x04002222 RID: 8738
	[SerializeField]
	private SpriteRenderer middleRenderer;

	// Token: 0x04002223 RID: 8739
	[SerializeField]
	private SpriteRenderer topRenderer;

	// Token: 0x04002224 RID: 8740
	[SerializeField]
	private SpriteRenderer dustRenderer;

	// Token: 0x04002225 RID: 8741
	[SerializeField]
	private SpriteRenderer deathSparkRenderer;

	// Token: 0x04002226 RID: 8742
	[SerializeField]
	private Sprite[] rotatingSprites;

	// Token: 0x04002227 RID: 8743
	[SerializeField]
	private Sprite[] deathSparkSprites;

	// Token: 0x04002228 RID: 8744
	[SerializeField]
	private Effect lionsLandDustFX;

	// Token: 0x04002229 RID: 8745
	[SerializeField]
	private Transform dropDustPos;

	// Token: 0x0400222A RID: 8746
	[SerializeField]
	private SpriteDeathParts deathParts;

	// Token: 0x0400222B RID: 8747
	[SerializeField]
	private SpriteDeathPartsDLC deathDust;

	// Token: 0x0400222D RID: 8749
	private LevelProperties.ChessQueen.Lightning properties;

	// Token: 0x0400222E RID: 8750
	private float speed;
}
