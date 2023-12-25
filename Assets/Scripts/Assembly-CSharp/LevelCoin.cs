using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020004A1 RID: 1185
[DefaultExecutionOrder(100)]
[RequireComponent(typeof(Animator))]
public class LevelCoin : AbstractCollidableObject
{
	// Token: 0x0600134B RID: 4939 RVA: 0x000AAA6B File Offset: 0x000A8E6B
	public static void OnLevelStart()
	{
		PlayerData.Data.ResetLevelCoinManager();
	}

	// Token: 0x0600134C RID: 4940 RVA: 0x000AAA77 File Offset: 0x000A8E77
	public static void OnLevelComplete()
	{
		PlayerData.Data.ApplyLevelCoins();
	}

	// Token: 0x1700030A RID: 778
	// (get) Token: 0x0600134D RID: 4941 RVA: 0x000AAA84 File Offset: 0x000A8E84
	public string GlobalID
	{
		get
		{
			return SceneManager.GetActiveScene().name + "::" + base.gameObject.name;
		}
	}

	// Token: 0x0600134E RID: 4942 RVA: 0x000AAAB4 File Offset: 0x000A8EB4
	protected override void Awake()
	{
		PlatformingLevel platformingLevel = Level.Current as PlatformingLevel;
		if (platformingLevel)
		{
			platformingLevel.LevelCoinsIDs.Add(new CoinPositionAndID(this.GlobalID, base.transform.position.x));
		}
		base.Awake();
		if (PlayerData.Data.GetCoinCollected(this))
		{
			this._collected = true;
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		this._spriteRenderer = base.GetComponent<SpriteRenderer>();
	}

	// Token: 0x0600134F RID: 4943 RVA: 0x000AAB38 File Offset: 0x000A8F38
	private void Update()
	{
		if (this._collected)
		{
			return;
		}
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player != null && Vector2.Distance(base.transform.position, player.center) < 100f)
		{
			this.Collect(player.id);
			return;
		}
		if (player2 != null && Vector2.Distance(base.transform.position, player2.center) < 100f)
		{
			this.Collect(player2.id);
			return;
		}
	}

	// Token: 0x06001350 RID: 4944 RVA: 0x000AABE8 File Offset: 0x000A8FE8
	private void Collect(PlayerId player)
	{
		if (this._collected)
		{
			return;
		}
		PlayerData.Data.SetLevelCoinCollected(this, true, player);
		this._collected = true;
		AudioManager.Play("level_coin_pickup");
		base.animator.SetTrigger("OnDeath");
		base.transform.localScale *= 1.2f;
		this._spriteRenderer.flipX = MathUtils.RandomBool();
		this._spriteRenderer.flipY = MathUtils.RandomBool();
	}

	// Token: 0x06001351 RID: 4945 RVA: 0x000AAC6A File Offset: 0x000A906A
	private void OnDeathAnimComplete()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04001C72 RID: 7282
	private const float COLLECT_RANGE = 100f;

	// Token: 0x04001C73 RID: 7283
	public const int NUM_COINS = 40;

	// Token: 0x04001C74 RID: 7284
	private SpriteRenderer _spriteRenderer;

	// Token: 0x04001C75 RID: 7285
	private bool _collected;
}
