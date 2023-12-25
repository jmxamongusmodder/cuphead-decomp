using System;
using UnityEngine;

// Token: 0x02000621 RID: 1569
public class FlyingBirdLevelLaser : AbstractMonoBehaviour
{
	// Token: 0x06001FEC RID: 8172 RVA: 0x00125514 File Offset: 0x00123914
	public FlyingBirdLevelLaser Create(Vector2 pos, float speed)
	{
		FlyingBirdLevelLaser flyingBirdLevelLaser = this.InstantiatePrefab<FlyingBirdLevelLaser>();
		flyingBirdLevelLaser.transform.position = pos + new Vector2(flyingBirdLevelLaser.size.x, 0f);
		flyingBirdLevelLaser.speed = speed;
		return flyingBirdLevelLaser;
	}

	// Token: 0x06001FED RID: 8173 RVA: 0x0012555C File Offset: 0x0012395C
	protected override void Awake()
	{
		base.Awake();
		SpriteRenderer component = base.transform.GetComponent<SpriteRenderer>();
		this.size = component.sprite.bounds.size;
	}

	// Token: 0x06001FEE RID: 8174 RVA: 0x0012559C File Offset: 0x0012399C
	private void Update()
	{
		base.transform.AddPosition(-this.speed * CupheadTime.Delta, 0f, 0f);
		if (base.transform.position.x < -740f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04002869 RID: 10345
	private Vector2 size;

	// Token: 0x0400286A RID: 10346
	private float speed;
}
