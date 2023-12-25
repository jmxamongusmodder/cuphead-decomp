using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006CB RID: 1739
public class GraveyardLevelSplitDevilProjectile : BasicProjectileContinuesOnLevelEnd
{
	// Token: 0x06002504 RID: 9476 RVA: 0x0015B3D4 File Offset: 0x001597D4
	public GraveyardLevelSplitDevilProjectile Create(Vector2 position, float rotation, float speed, GraveyardLevelSplitDevil devil)
	{
		GraveyardLevelSplitDevilProjectile graveyardLevelSplitDevilProjectile = base.Create(position, rotation, speed) as GraveyardLevelSplitDevilProjectile;
		graveyardLevelSplitDevilProjectile.devil = devil;
		graveyardLevelSplitDevilProjectile.animator.SetInteger("FireVariant", UnityEngine.Random.Range(0, 3));
		graveyardLevelSplitDevilProjectile.animator.SetInteger("LightVariant", UnityEngine.Random.Range(0, 2));
		graveyardLevelSplitDevilProjectile.SetBool("IsFire", !devil.isAngel);
		graveyardLevelSplitDevilProjectile.coll.enabled = !devil.isAngel;
		graveyardLevelSplitDevilProjectile.UpdateFade(2f);
		graveyardLevelSplitDevilProjectile.StartCoroutine(graveyardLevelSplitDevilProjectile.spawn_fx_cr());
		return graveyardLevelSplitDevilProjectile;
	}

	// Token: 0x06002505 RID: 9477 RVA: 0x0015B46C File Offset: 0x0015986C
	private IEnumerator spawn_fx_cr()
	{
		this.fxAngle = (float)UnityEngine.Random.Range(0, 360);
		while (!this.dead && !this.impacted)
		{
			yield return CupheadTime.WaitForSeconds(this, this.fxSpawnDelay);
			int count = 1;
			if (this.fxSpawnDelay < CupheadTime.Delta)
			{
				count = (int)(CupheadTime.Delta / this.fxSpawnDelay);
			}
			for (int i = 0; i < count; i++)
			{
				Effect effect = (!this.devil.isAngel) ? this.fireFX.Create(base.transform.position + MathUtils.AngleToDirection(this.fxAngle) * this.fxDistanceRange.RandomFloat()) : this.lightFX.Create(base.transform.position + MathUtils.AngleToDirection(this.fxAngle) * this.fxDistanceRange.RandomFloat());
				if (!this.devil.isAngel)
				{
					effect.transform.eulerAngles = new Vector3(0f, 0f, base.transform.eulerAngles.z + 50f);
				}
				this.fxAngle = (this.fxAngle + this.fxAngleShiftRange.RandomFloat()) % 360f;
			}
		}
		yield break;
	}

	// Token: 0x170003B9 RID: 953
	// (get) Token: 0x06002506 RID: 9478 RVA: 0x0015B487 File Offset: 0x00159887
	protected override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06002507 RID: 9479 RVA: 0x0015B48C File Offset: 0x0015988C
	protected override void Update()
	{
		if (!this.impacted)
		{
			base.Update();
		}
		if (!this.dead)
		{
			this.coll.enabled = !this.devil.isAngel;
		}
		if (!this.impacted)
		{
			if (base.animator.GetBool("IsFire") == this.devil.isAngel)
			{
				base.animator.Play("LightTransition" + UnityEngine.Random.Range(0, 3).ToString(), 2, 0f);
			}
			base.animator.SetBool("IsFire", !this.devil.isAngel);
			this.frameTimer += CupheadTime.Delta;
			while (this.frameTimer > 0.041666668f)
			{
				this.frameTimer -= 0.041666668f;
				this.UpdateFade(0.25f);
			}
		}
		if (!this.impacted && Mathf.Abs(base.transform.position.x) < 550f && base.transform.position.y < -297f)
		{
			this.impacted = true;
			this.Speed = 0f;
			base.transform.eulerAngles = Vector3.zero;
			this.fireRend[0].transform.eulerAngles = Vector3.zero;
			this.lightRend[0].transform.eulerAngles = Vector3.zero;
			base.animator.Play((!Rand.Bool()) ? "ImpactB" : "ImpactA", this.devil.isAngel ? 1 : 0);
			this.UpdateFade(2f);
			this.Die();
		}
	}

	// Token: 0x06002508 RID: 9480 RVA: 0x0015B678 File Offset: 0x00159A78
	private void UpdateFade(float amount)
	{
		foreach (SpriteRenderer spriteRenderer in this.fireRend)
		{
			spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.Clamp(spriteRenderer.color.a + ((!this.coll.enabled) ? (-amount) : amount), 0f, 1f));
		}
		foreach (SpriteRenderer spriteRenderer2 in this.lightRend)
		{
			spriteRenderer2.color = new Color(spriteRenderer2.color.r, spriteRenderer2.color.g, spriteRenderer2.color.b, Mathf.Clamp(spriteRenderer2.color.a + ((!this.coll.enabled) ? amount : ((!(spriteRenderer2.gameObject.name == "Ring")) ? (-amount) : (-amount * 0.7f))), 0f, 1f));
		}
	}

	// Token: 0x06002509 RID: 9481 RVA: 0x0015B7D6 File Offset: 0x00159BD6
	protected override void Die()
	{
		this.dead = true;
		this.coll.enabled = false;
	}

	// Token: 0x04002DB2 RID: 11698
	private GraveyardLevelSplitDevil devil;

	// Token: 0x04002DB3 RID: 11699
	[SerializeField]
	private SpriteRenderer[] fireRend;

	// Token: 0x04002DB4 RID: 11700
	[SerializeField]
	private SpriteRenderer[] lightRend;

	// Token: 0x04002DB5 RID: 11701
	[SerializeField]
	private Effect fireFX;

	// Token: 0x04002DB6 RID: 11702
	[SerializeField]
	private Effect lightFX;

	// Token: 0x04002DB7 RID: 11703
	[SerializeField]
	private Collider2D coll;

	// Token: 0x04002DB8 RID: 11704
	private float frameTimer;

	// Token: 0x04002DB9 RID: 11705
	private new bool dead;

	// Token: 0x04002DBA RID: 11706
	private bool impacted;

	// Token: 0x04002DBB RID: 11707
	[SerializeField]
	private float fxSpawnDelay = 0.15f;

	// Token: 0x04002DBC RID: 11708
	[SerializeField]
	private MinMax fxAngleShiftRange = new MinMax(60f, 300f);

	// Token: 0x04002DBD RID: 11709
	[SerializeField]
	private MinMax fxDistanceRange = new MinMax(0f, 20f);

	// Token: 0x04002DBE RID: 11710
	private float fxAngle;
}
