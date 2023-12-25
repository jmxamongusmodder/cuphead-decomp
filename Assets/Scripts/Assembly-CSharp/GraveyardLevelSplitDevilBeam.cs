using System;
using UnityEngine;

// Token: 0x020006C8 RID: 1736
public class GraveyardLevelSplitDevilBeam : AbstractProjectile
{
	// Token: 0x170003B8 RID: 952
	// (get) Token: 0x060024F1 RID: 9457 RVA: 0x0015A7E6 File Offset: 0x00158BE6
	// (set) Token: 0x060024F2 RID: 9458 RVA: 0x0015A7EE File Offset: 0x00158BEE
	public GraveyardLevelSplitDevil devil { get; private set; }

	// Token: 0x060024F3 RID: 9459 RVA: 0x0015A7F7 File Offset: 0x00158BF7
	protected override void RandomizeVariant()
	{
	}

	// Token: 0x060024F4 RID: 9460 RVA: 0x0015A7FC File Offset: 0x00158BFC
	public GraveyardLevelSplitDevilBeam Create(Vector3 pos, float xVelocity, float warningTime, GraveyardLevelSplitDevil devil)
	{
		GraveyardLevelSplitDevilBeam graveyardLevelSplitDevilBeam = base.Create(pos) as GraveyardLevelSplitDevilBeam;
		graveyardLevelSplitDevilBeam.xVelocity = xVelocity;
		graveyardLevelSplitDevilBeam.DestroyDistance = (float)(Level.Current.Width + 200);
		graveyardLevelSplitDevilBeam.devil = devil;
		graveyardLevelSplitDevilBeam.warningTime = warningTime;
		graveyardLevelSplitDevilBeam.fireOn = !graveyardLevelSplitDevilBeam.devil.isAngel;
		graveyardLevelSplitDevilBeam.coll.enabled = !graveyardLevelSplitDevilBeam.devil.isAngel;
		graveyardLevelSplitDevilBeam.UpdateFade(1f);
		if (graveyardLevelSplitDevilBeam.fireOn)
		{
			graveyardLevelSplitDevilBeam.fireAnim.Play("Form", 1, 0f);
			graveyardLevelSplitDevilBeam.fireAnim.Update(0f);
			Effect effect = this.igniteFX.Create(graveyardLevelSplitDevilBeam.transform.position, this.fireAnim);
			effect.transform.parent = graveyardLevelSplitDevilBeam.transform;
			AudioManager.Play("sfx_dlc_graveyard_beamchange_fireon");
			graveyardLevelSplitDevilBeam.emitAudioFromObject.Add("sfx_dlc_graveyard_beamchange_fireon");
		}
		else
		{
			foreach (SpriteRenderer spriteRenderer in graveyardLevelSplitDevilBeam.lightRend)
			{
				spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
			}
		}
		CupheadLevelCamera.Current.StartShake(4f);
		return graveyardLevelSplitDevilBeam;
	}

	// Token: 0x060024F5 RID: 9461 RVA: 0x0015A954 File Offset: 0x00158D54
	protected override void Update()
	{
		base.Update();
		if (base.dead)
		{
			return;
		}
		if (this.devil.dead)
		{
			this.coll.enabled = false;
			this.forceFade = true;
		}
		if (this.warningTime <= 0f)
		{
			base.transform.AddPosition(this.xVelocity * CupheadTime.Delta, 0f, 0f);
			if (this.fireAnim.GetBool("Smoke"))
			{
				this.flameTrailDistanceTracker += Mathf.Abs(this.xVelocity) * CupheadTime.Delta;
			}
		}
		else
		{
			this.warningTime -= CupheadTime.Delta;
		}
		while (this.flameTrailDistanceTracker > this.flameTrailSpacing && !this.forceFade)
		{
			this.flameTrailDistanceTracker -= this.flameTrailSpacing;
			this.SpawnTrailFX();
		}
		if (Mathf.Abs(base.transform.position.x) < (float)((Mathf.Sign(base.transform.position.x) != Mathf.Sign(this.xVelocity)) ? 600 : 400) && !this.fireAnim.GetBool("Smoke"))
		{
			this.SpawnTrailFX();
		}
		if (Mathf.Abs(base.transform.position.x) < 550f && !this.onGround)
		{
			this.onGround = true;
			this.lightAnim.Play((!Rand.Bool()) ? "B" : "A", 1, 0f);
		}
		this.fireAnim.SetBool("Smoke", Mathf.Abs(base.transform.position.x) < (float)((Mathf.Sign(base.transform.position.x) != Mathf.Sign(this.xVelocity)) ? 600 : 400));
		this.coll.enabled = !this.devil.isAngel;
		if (this.fireOn != !this.devil.isAngel)
		{
			if (this.fireOn && !this.fireAnim.GetCurrentAnimatorStateInfo(1).IsName("Form") && this.fireAnim.GetBool("Smoke"))
			{
				Effect effect = this.bottomSmokeFX.Create(base.transform.position);
				if (this.bottomSmokeFXTypeA)
				{
					effect.Play();
				}
				this.bottomSmokeFXTypeA = !this.bottomSmokeFXTypeA;
				effect.transform.parent = base.transform;
				Effect effect2 = this.midSmokeFX.Create(this.midSmokePos.position);
				effect2.transform.localScale = this.midSmokePos.localScale;
				effect2.transform.parent = base.transform;
			}
			if (!this.fireOn)
			{
				AudioManager.Play("sfx_dlc_graveyard_beamchange_fireon");
				this.emitAudioFromObject.Add("sfx_dlc_graveyard_beamchange_fireon");
				Effect effect3 = this.igniteFX.Create(base.transform.position, this.fireAnim);
				effect3.transform.parent = base.transform;
			}
			this.fireAnim.Play((!this.fireOn) ? "Form" : "Dissipate", 1, 0f);
			this.fireAnim.Update(0f);
			this.fireOn = !this.devil.isAngel;
		}
		this.frameTimer += CupheadTime.Delta;
		while (this.frameTimer > 0.041666668f)
		{
			this.frameTimer -= 0.041666668f;
			this.UpdateFade(0.25f);
		}
	}

	// Token: 0x060024F6 RID: 9462 RVA: 0x0015AD78 File Offset: 0x00159178
	private void UpdateFade(float amount)
	{
		foreach (SpriteRenderer spriteRenderer in this.fireRend)
		{
			spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.Clamp(spriteRenderer.color.a + ((!(this.coll.enabled & !this.forceFade)) ? (-amount) : amount), 0f, 1f));
		}
		foreach (SpriteRenderer spriteRenderer2 in this.lightRend)
		{
			spriteRenderer2.color = new Color(spriteRenderer2.color.r, spriteRenderer2.color.g, spriteRenderer2.color.b, Mathf.Clamp(spriteRenderer2.color.a + ((!this.coll.enabled && !this.forceFade) ? amount : (-amount)), 0f, 1f));
		}
		this.groundSpotlight.color = new Color(1f, 1f, 1f, Mathf.Clamp(this.groundSpotlight.color.a + ((this.coll.enabled || !(this.onGround & !this.forceFade)) ? (-amount) : amount), 0f, 1f));
	}

	// Token: 0x060024F7 RID: 9463 RVA: 0x0015AF34 File Offset: 0x00159334
	private void SpawnTrailFX()
	{
		this.trailFX.Create(new Vector3(Mathf.Clamp(base.transform.position.x + this.flameTrailSpacing * Mathf.Sign(this.xVelocity), -550f, 550f), base.transform.position.y), new Vector3(-Mathf.Sign(this.xVelocity), 1f), this, this.flameTrailAnim);
		this.flameTrailAnim = (this.flameTrailAnim + 1) % 3;
	}

	// Token: 0x060024F8 RID: 9464 RVA: 0x0015AFC8 File Offset: 0x001593C8
	private void LateUpdate()
	{
		this.fireRend[0].enabled = (this.fireFormDissipate.sprite == null);
		this.sparkleBeam.transform.localPosition = new Vector3(0f, (float)(-1280 + (int)this.lightAnim.GetCurrentAnimatorStateInfo(0).normalizedTime % 8 * 280));
	}

	// Token: 0x060024F9 RID: 9465 RVA: 0x0015B031 File Offset: 0x00159431
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060024FA RID: 9466 RVA: 0x0015B04F File Offset: 0x0015944F
	protected override void Die()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04002D93 RID: 11667
	private float xVelocity;

	// Token: 0x04002D95 RID: 11669
	[SerializeField]
	private Animator fireAnim;

	// Token: 0x04002D96 RID: 11670
	[SerializeField]
	private Animator lightAnim;

	// Token: 0x04002D97 RID: 11671
	[SerializeField]
	private SpriteRenderer[] fireRend;

	// Token: 0x04002D98 RID: 11672
	[SerializeField]
	private SpriteRenderer[] lightRend;

	// Token: 0x04002D99 RID: 11673
	[SerializeField]
	private SpriteRenderer fireFormDissipate;

	// Token: 0x04002D9A RID: 11674
	[SerializeField]
	private Effect bottomSmokeFX;

	// Token: 0x04002D9B RID: 11675
	private bool bottomSmokeFXTypeA = true;

	// Token: 0x04002D9C RID: 11676
	[SerializeField]
	private Effect midSmokeFX;

	// Token: 0x04002D9D RID: 11677
	[SerializeField]
	private Transform midSmokePos;

	// Token: 0x04002D9E RID: 11678
	[SerializeField]
	private GraveyardLevelSplitDevilBeamIgniteFX igniteFX;

	// Token: 0x04002D9F RID: 11679
	[SerializeField]
	private GraveyardLevelSplitDevilBeamTrailFX trailFX;

	// Token: 0x04002DA0 RID: 11680
	[SerializeField]
	private float flameTrailSpacing = 128f;

	// Token: 0x04002DA1 RID: 11681
	[SerializeField]
	private GameObject sparkleBeam;

	// Token: 0x04002DA2 RID: 11682
	[SerializeField]
	private SpriteRenderer groundSpotlight;

	// Token: 0x04002DA3 RID: 11683
	private bool onGround;

	// Token: 0x04002DA4 RID: 11684
	private bool fireOn;

	// Token: 0x04002DA5 RID: 11685
	[SerializeField]
	private Collider2D coll;

	// Token: 0x04002DA6 RID: 11686
	private float warningTime;

	// Token: 0x04002DA7 RID: 11687
	private float frameTimer;

	// Token: 0x04002DA8 RID: 11688
	private float flameTrailDistanceTracker;

	// Token: 0x04002DA9 RID: 11689
	private int flameTrailAnim;

	// Token: 0x04002DAA RID: 11690
	private bool forceFade;
}
