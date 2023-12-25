using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000667 RID: 1639
public class FlyingGenieLevelGem : AbstractProjectile
{
	// Token: 0x17000392 RID: 914
	// (get) Token: 0x06002224 RID: 8740 RVA: 0x0013DF3B File Offset: 0x0013C33B
	protected override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06002225 RID: 8741 RVA: 0x0013DF40 File Offset: 0x0013C340
	public FlyingGenieLevelGem Create(Vector2 pos, AbstractPlayerController player, float offsetY, float speed, bool parryable, bool isBig)
	{
		FlyingGenieLevelGem flyingGenieLevelGem = base.Create() as FlyingGenieLevelGem;
		flyingGenieLevelGem.transform.position = pos;
		flyingGenieLevelGem.player = player;
		flyingGenieLevelGem.offsetY = offsetY;
		flyingGenieLevelGem.speed = speed;
		flyingGenieLevelGem.SetParryable(parryable);
		flyingGenieLevelGem.isBig = isBig;
		return flyingGenieLevelGem;
	}

	// Token: 0x06002226 RID: 8742 RVA: 0x0013DF94 File Offset: 0x0013C394
	protected override void Start()
	{
		base.Start();
		int num = (!this.isBig) ? UnityEngine.Random.Range(2, 9) : UnityEngine.Random.Range(0, 3);
		if (base.CanParry)
		{
			num = 9;
		}
		base.animator.SetFloat("Variation", (float)num / 9f);
		Vector3 b = new Vector3(0f, this.offsetY, 0f);
		float value = MathUtils.DirectionToAngle(this.player.transform.position - (base.transform.position + b));
		base.transform.SetEulerAngles(null, null, new float?(value));
		base.StartCoroutine(this.check_gem_cr());
	}

	// Token: 0x06002227 RID: 8743 RVA: 0x0013E065 File Offset: 0x0013C465
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002228 RID: 8744 RVA: 0x0013E084 File Offset: 0x0013C484
	private IEnumerator check_gem_cr()
	{
		float startPos = (base.transform.position + Vector3.up * this.outOfChestY).y;
		while (base.transform.position.y < startPos)
		{
			base.transform.AddPosition(0f, this.outOfChestY * this.outOfChestSpeed * CupheadTime.Delta, 0f);
			yield return null;
		}
		this.gemRenderer.sortingLayerName = "Projectiles";
		this.gemRenderer.sortingOrder = 2;
		while (base.transform.position.x > -640f && base.transform.position.y < 360f && base.transform.position.y > -360f)
		{
			base.transform.position += base.transform.right * this.speed * CupheadTime.Delta;
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x06002229 RID: 8745 RVA: 0x0013E09F File Offset: 0x0013C49F
	protected override void Die()
	{
		base.GetComponent<SpriteRenderer>().enabled = false;
		base.Die();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04002ACA RID: 10954
	private const int BigVariations = 3;

	// Token: 0x04002ACB RID: 10955
	private const int VariationsTotal = 10;

	// Token: 0x04002ACC RID: 10956
	private const string VariationParameterName = "Variation";

	// Token: 0x04002ACD RID: 10957
	private const string ProjectilesLayer = "Projectiles";

	// Token: 0x04002ACE RID: 10958
	[SerializeField]
	private SpriteRenderer gemRenderer;

	// Token: 0x04002ACF RID: 10959
	[SerializeField]
	private float outOfChestY;

	// Token: 0x04002AD0 RID: 10960
	[SerializeField]
	private float outOfChestSpeed;

	// Token: 0x04002AD1 RID: 10961
	private AbstractPlayerController player;

	// Token: 0x04002AD2 RID: 10962
	private float offsetY;

	// Token: 0x04002AD3 RID: 10963
	private bool isBig;

	// Token: 0x04002AD4 RID: 10964
	private float velocityX;

	// Token: 0x04002AD5 RID: 10965
	private float speed;
}
