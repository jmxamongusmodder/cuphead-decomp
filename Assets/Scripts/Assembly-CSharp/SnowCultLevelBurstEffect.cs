using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007E5 RID: 2021
public class SnowCultLevelBurstEffect : Effect
{
	// Token: 0x06002E42 RID: 11842 RVA: 0x001B46B4 File Offset: 0x001B2AB4
	public SnowCultLevelBurstEffect Create(Vector3 pos, float direction)
	{
		SnowCultLevelBurstEffect snowCultLevelBurstEffect = base.Create(pos) as SnowCultLevelBurstEffect;
		snowCultLevelBurstEffect.direction = direction;
		return snowCultLevelBurstEffect;
	}

	// Token: 0x06002E43 RID: 11843 RVA: 0x001B46D8 File Offset: 0x001B2AD8
	private void Start()
	{
		this.startPosY = base.transform.position.y;
		if (this.isSnowFall)
		{
			base.StartCoroutine(this.move_cr());
		}
	}

	// Token: 0x06002E44 RID: 11844 RVA: 0x001B4718 File Offset: 0x001B2B18
	private void SpawnEffect()
	{
		Vector3 pos = new Vector3(base.transform.position.x + 127f * this.direction, (!this.isSnowFall) ? 95f : this.startPosY);
		if (pos.x > -740f && pos.x < 740f)
		{
			if (this.isTypeA)
			{
				this.typeA.Create(pos, this.direction);
			}
			else
			{
				this.typeB.Create(pos, this.direction);
			}
		}
	}

	// Token: 0x06002E45 RID: 11845 RVA: 0x001B47C0 File Offset: 0x001B2BC0
	private IEnumerator move_cr()
	{
		while (base.transform.position.y > -360f)
		{
			base.transform.position += Vector3.down * 150f * CupheadTime.Delta;
			yield return null;
		}
		this.OnEffectComplete();
		yield return null;
		yield break;
	}

	// Token: 0x040036C5 RID: 14021
	private const float DIST_X_TO_MOVE = 127f;

	// Token: 0x040036C6 RID: 14022
	private const float Y_TO_SPAWN = 95f;

	// Token: 0x040036C7 RID: 14023
	private const float MOVE_SPEED = 150f;

	// Token: 0x040036C8 RID: 14024
	[SerializeField]
	private bool isSnowFall;

	// Token: 0x040036C9 RID: 14025
	[SerializeField]
	private bool isTypeA;

	// Token: 0x040036CA RID: 14026
	[SerializeField]
	private SnowCultLevelBurstEffect typeA;

	// Token: 0x040036CB RID: 14027
	[SerializeField]
	private SnowCultLevelBurstEffect typeB;

	// Token: 0x040036CC RID: 14028
	private float startPosY;

	// Token: 0x040036CD RID: 14029
	private float direction;
}
