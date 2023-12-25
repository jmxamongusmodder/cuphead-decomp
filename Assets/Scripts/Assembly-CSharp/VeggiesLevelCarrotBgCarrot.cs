using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000847 RID: 2119
public class VeggiesLevelCarrotBgCarrot : AbstractMonoBehaviour
{
	// Token: 0x0600310A RID: 12554 RVA: 0x001CC940 File Offset: 0x001CAD40
	public VeggiesLevelCarrotBgCarrot Create(int side, float speed, VeggiesLevelCarrot parentCarrot)
	{
		VeggiesLevelCarrotBgCarrot veggiesLevelCarrotBgCarrot = this.InstantiatePrefab<VeggiesLevelCarrotBgCarrot>();
		veggiesLevelCarrotBgCarrot.Init(side, speed, parentCarrot);
		return veggiesLevelCarrotBgCarrot;
	}

	// Token: 0x0600310B RID: 12555 RVA: 0x001CC960 File Offset: 0x001CAD60
	private void Init(int side, float speed, VeggiesLevelCarrot parentCarrot)
	{
		base.transform.SetPosition(new float?(UnityEngine.Random.Range(150f, 600f) * (float)side), new float?(-360f), new float?(0f));
		this.parentCarrot = parentCarrot;
		base.StartCoroutine(this.float_cr(speed));
	}

	// Token: 0x0600310C RID: 12556 RVA: 0x001CC9B8 File Offset: 0x001CADB8
	private void End()
	{
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600310D RID: 12557 RVA: 0x001CC9CC File Offset: 0x001CADCC
	private IEnumerator float_cr(float speed)
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			if (base.transform.position.y > 720f)
			{
				this.parentCarrot.ShootHoming();
				this.End();
			}
			base.transform.AddPosition(0f, speed * CupheadTime.FixedDelta, 0f);
			yield return wait;
		}
		yield break;
	}

	// Token: 0x040039AF RID: 14767
	private const float RANGE_MIN = 150f;

	// Token: 0x040039B0 RID: 14768
	private const float RANGE_MAX = 600f;

	// Token: 0x040039B1 RID: 14769
	private VeggiesLevelCarrot parentCarrot;
}
