using System;
using UnityEngine;

// Token: 0x02000A5B RID: 2651
public class PlayerSuperGhostHeart : AbstractLevelEntity
{
	// Token: 0x06003F30 RID: 16176 RVA: 0x00229850 File Offset: 0x00227C50
	private void FixedUpdate()
	{
		base.transform.AddPosition(0f, WeaponProperties.LevelSuperGhost.heartSpeed * CupheadTime.FixedDelta * this.gravityMultiplier, 0f);
		if (!CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(50f, 50f)))
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06003F31 RID: 16177 RVA: 0x002298C0 File Offset: 0x00227CC0
	public PlayerSuperGhostHeart Create(Vector2 pos, float gravityMultiplier)
	{
		PlayerSuperGhostHeart playerSuperGhostHeart = this.InstantiatePrefab<PlayerSuperGhostHeart>();
		playerSuperGhostHeart.transform.position = pos;
		playerSuperGhostHeart.transform.localScale = new Vector3(playerSuperGhostHeart.transform.localScale.x, gravityMultiplier, playerSuperGhostHeart.transform.localScale.z);
		playerSuperGhostHeart.gravityMultiplier = gravityMultiplier;
		return playerSuperGhostHeart;
	}

	// Token: 0x06003F32 RID: 16178 RVA: 0x00229924 File Offset: 0x00227D24
	public override void OnParry(AbstractPlayerController player)
	{
		base.OnParry(player);
		player.stats.AddEx();
		this.spark.Create(base.transform.position);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04004644 RID: 17988
	private float gravityMultiplier;

	// Token: 0x04004645 RID: 17989
	[SerializeField]
	private Effect spark;
}
