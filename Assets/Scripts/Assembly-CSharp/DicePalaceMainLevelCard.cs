using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005D0 RID: 1488
public class DicePalaceMainLevelCard : AbstractProjectile
{
	// Token: 0x1700036A RID: 874
	// (get) Token: 0x06001D3B RID: 7483 RVA: 0x0010C55C File Offset: 0x0010A95C
	public override float ParryMeterMultiplier
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x06001D3C RID: 7484 RVA: 0x0010C564 File Offset: 0x0010A964
	public DicePalaceMainLevelCard Create(Vector3 pos, LevelProperties.DicePalaceMain.Cards properties, bool onLeft)
	{
		DicePalaceMainLevelCard dicePalaceMainLevelCard = base.Create() as DicePalaceMainLevelCard;
		dicePalaceMainLevelCard.properties = properties;
		dicePalaceMainLevelCard.transform.position = pos;
		dicePalaceMainLevelCard.onLeft = onLeft;
		return dicePalaceMainLevelCard;
	}

	// Token: 0x06001D3D RID: 7485 RVA: 0x0010C598 File Offset: 0x0010A998
	protected override void Start()
	{
		base.Start();
		this.direction = ((!this.onLeft) ? (-base.transform.right) : base.transform.right);
		if (base.CanParry && (PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.isChalice || (PlayerManager.Multiplayer && PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.isChalice)))
		{
			this.nextRisingHeart = UnityEngine.Random.Range(0, this.risingHeartAnimator.Length);
			this.chaliceParryableHearts.SetActive(true);
			base.StartCoroutine(this.rising_hearts_cr());
		}
	}

	// Token: 0x06001D3E RID: 7486 RVA: 0x0010C648 File Offset: 0x0010AA48
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001D3F RID: 7487 RVA: 0x0010C666 File Offset: 0x0010AA66
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001D40 RID: 7488 RVA: 0x0010C684 File Offset: 0x0010AA84
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		base.transform.position += this.direction * this.properties.cardSpeed * CupheadTime.FixedDelta;
		if (base.CanParry && this.risingHeartRenderer[0].sortingOrder == -1 && Mathf.Abs(base.transform.position.x) < 230f)
		{
			for (int i = 0; i < this.risingHeartRenderer.Length; i++)
			{
				this.risingHeartRenderer[i].sortingOrder = 2;
			}
		}
	}

	// Token: 0x06001D41 RID: 7489 RVA: 0x0010C734 File Offset: 0x0010AB34
	private IEnumerator rising_hearts_cr()
	{
		this.risingHeartAnimator[this.nextRisingHeart].Play(UnityEngine.Random.Range(0, 6).ToString(), 0, 0.25f);
		this.nextRisingHeart = (this.nextRisingHeart + 1) % this.risingHeartAnimator.Length;
		this.risingHeartAnimator[this.nextRisingHeart].Play(UnityEngine.Random.Range(0, 6).ToString(), 0, 0.5f);
		this.nextRisingHeart = (this.nextRisingHeart + 1) % this.risingHeartAnimator.Length;
		this.risingHeartAnimator[this.nextRisingHeart].Play(UnityEngine.Random.Range(0, 6).ToString(), 0, 0.75f);
		this.nextRisingHeart = (this.nextRisingHeart + 1) % this.risingHeartAnimator.Length;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.risingHeartSpawnTimeRange.RandomFloat());
			this.risingHeartAnimator[this.nextRisingHeart].Play(UnityEngine.Random.Range(0, 6).ToString());
			this.nextRisingHeart = (this.nextRisingHeart + 1) % this.risingHeartAnimator.Length;
		}
		yield break;
	}

	// Token: 0x06001D42 RID: 7490 RVA: 0x0010C74F File Offset: 0x0010AB4F
	public override void OnParry(AbstractPlayerController player)
	{
		this.SetParryable(false);
		base.StartCoroutine(this.parryCooldown_cr());
	}

	// Token: 0x06001D43 RID: 7491 RVA: 0x0010C768 File Offset: 0x0010AB68
	private IEnumerator parryCooldown_cr()
	{
		float t = 0f;
		while (t < this.coolDown)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		this.SetParryable(true);
		yield return null;
		yield break;
	}

	// Token: 0x04002624 RID: 9764
	private LevelProperties.DicePalaceMain.Cards properties;

	// Token: 0x04002625 RID: 9765
	private bool onLeft;

	// Token: 0x04002626 RID: 9766
	private Vector3 direction;

	// Token: 0x04002627 RID: 9767
	[SerializeField]
	private float coolDown = 0.4f;

	// Token: 0x04002628 RID: 9768
	[SerializeField]
	private GameObject chaliceParryableHearts;

	// Token: 0x04002629 RID: 9769
	[SerializeField]
	private Animator[] risingHeartAnimator;

	// Token: 0x0400262A RID: 9770
	private int nextRisingHeart;

	// Token: 0x0400262B RID: 9771
	[SerializeField]
	private SpriteRenderer[] risingHeartRenderer;

	// Token: 0x0400262C RID: 9772
	[SerializeField]
	private MinMax risingHeartSpawnTimeRange = new MinMax(0.1667f, 0.2333f);
}
