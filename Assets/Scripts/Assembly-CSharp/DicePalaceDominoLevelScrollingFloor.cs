using System;
using UnityEngine;

// Token: 0x020005BE RID: 1470
public class DicePalaceDominoLevelScrollingFloor : MonoBehaviour
{
	// Token: 0x06001C92 RID: 7314 RVA: 0x001050F7 File Offset: 0x001034F7
	private void Start()
	{
		this.RefreshTilesAndSpikes();
	}

	// Token: 0x06001C93 RID: 7315 RVA: 0x00105100 File Offset: 0x00103500
	private void Update()
	{
		Vector3 position = base.transform.position;
		position.x -= this.speed * CupheadTime.Delta;
		base.transform.position = position;
		if (base.transform.position.x <= 0f)
		{
			position.x += this.resetPositionX;
			base.transform.position = position;
			this.RefreshTilesAndSpikes();
		}
	}

	// Token: 0x06001C94 RID: 7316 RVA: 0x00105188 File Offset: 0x00103588
	private void RefreshTilesAndSpikes()
	{
		for (int i = 0; i < this.dominoLevelRandomTiles.Length; i++)
		{
			this.dominoLevelRandomTiles[i].ChangeTile();
		}
		for (int j = 0; j < this.dominoLevelRandomSpikes.Length; j++)
		{
			this.dominoLevelRandomSpikes[j].ChangeSpikes();
		}
	}

	// Token: 0x06001C95 RID: 7317 RVA: 0x001051E1 File Offset: 0x001035E1
	private void OnDestroy()
	{
		this.dominoLevelRandomTiles = null;
		this.dominoLevelRandomSpikes = null;
	}

	// Token: 0x0400257E RID: 9598
	public float speed;

	// Token: 0x0400257F RID: 9599
	public float resetPositionX = 2808f;

	// Token: 0x04002580 RID: 9600
	[SerializeField]
	private DicePalaceDominoLevelRandomTile[] dominoLevelRandomTiles;

	// Token: 0x04002581 RID: 9601
	[SerializeField]
	private DicePalaceDominoLevelRandomSpike[] dominoLevelRandomSpikes;
}
