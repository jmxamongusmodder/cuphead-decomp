using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000801 RID: 2049
public class SnowCultLevelYetiLegs : BasicDamageDealingObject
{
	// Token: 0x06002F5B RID: 12123 RVA: 0x001C1443 File Offset: 0x001BF843
	private void Start()
	{
		base.StartCoroutine(this.run_away_cr());
	}

	// Token: 0x06002F5C RID: 12124 RVA: 0x001C1454 File Offset: 0x001BF854
	private IEnumerator run_away_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.rend.sortingLayerName = "Player";
		this.rend.sortingOrder = -19;
		base.animator.Play("Run");
		this.SFX_SNOWCULT_YetiLegsWalkOff();
		for (int i = 0; i < 1000; i++)
		{
			base.transform.position += Vector3.left * Mathf.Sign(base.transform.localScale.x) * 1000f * CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06002F5D RID: 12125 RVA: 0x001C146F File Offset: 0x001BF86F
	private void SFX_SNOWCULT_YetiLegsWalkOff()
	{
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_death_stompoffscreen");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_death_stompoffscreen");
	}

	// Token: 0x0400382B RID: 14379
	private const float RUN_SPEED = 1000f;

	// Token: 0x0400382C RID: 14380
	private const float RUN_DELAY = 1f;

	// Token: 0x0400382D RID: 14381
	[SerializeField]
	private SpriteRenderer rend;
}
