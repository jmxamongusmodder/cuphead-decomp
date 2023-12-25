using System;
using UnityEngine;

// Token: 0x020008AF RID: 2223
public class StartHotdog : MonoBehaviour
{
	// Token: 0x060033D1 RID: 13265 RVA: 0x001E14A2 File Offset: 0x001DF8A2
	private void OnTriggerEnter2D(Collider2D c)
	{
		if (c.tag == "Player")
		{
			this.hotdog.ProjectilesCanHit = true;
			base.gameObject.SetActive(false);
		}
	}

	// Token: 0x04003C1E RID: 15390
	private const string PlayerTag = "Player";

	// Token: 0x04003C1F RID: 15391
	[SerializeField]
	private CircusPlatformingLevelHotdog hotdog;
}
