using System;
using UnityEngine;

// Token: 0x0200053A RID: 1338
public class ChessBishopLevelIntroCandle : MonoBehaviour
{
	// Token: 0x06001853 RID: 6227 RVA: 0x000DC720 File Offset: 0x000DAB20
	private void AniEvent_StartMove()
	{
		this.moving = true;
		this.glow.SetActive(false);
	}

	// Token: 0x06001854 RID: 6228 RVA: 0x000DC738 File Offset: 0x000DAB38
	private void Update()
	{
		this.shadow.transform.position = new Vector3(this.shadow.transform.position.x, -40f);
	}

	// Token: 0x04002189 RID: 8585
	public bool moving;

	// Token: 0x0400218A RID: 8586
	[SerializeField]
	private GameObject glow;

	// Token: 0x0400218B RID: 8587
	[SerializeField]
	private GameObject shadow;
}
