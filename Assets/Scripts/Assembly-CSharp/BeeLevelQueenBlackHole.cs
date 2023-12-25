using System;
using UnityEngine;

// Token: 0x0200051C RID: 1308
public class BeeLevelQueenBlackHole : AbstractProjectile
{
	// Token: 0x06001762 RID: 5986 RVA: 0x000D2A94 File Offset: 0x000D0E94
	protected override void Start()
	{
		base.Start();
		this.direction = ((base.transform.position.x >= 0f) ? -1 : 1);
	}

	// Token: 0x06001763 RID: 5987 RVA: 0x000D2AD4 File Offset: 0x000D0ED4
	protected override void Update()
	{
		base.Update();
		base.transform.AddPosition(this.speed * (float)this.direction * CupheadTime.Delta, 0f, 0f);
		this.timer += CupheadTime.Delta;
		if (this.timer >= this.childDelay)
		{
			this.childPrefab.Create(base.transform.position, 90f, this.childSpeed);
			this.childPrefab.Create(base.transform.position, -90f, this.childSpeed).GetComponent<Animator>().Play("Reverse");
			this.timer = 0f;
		}
	}

	// Token: 0x06001764 RID: 5988 RVA: 0x000D2BA4 File Offset: 0x000D0FA4
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.childPrefab = null;
	}

	// Token: 0x04002099 RID: 8345
	[HideInInspector]
	public float health;

	// Token: 0x0400209A RID: 8346
	[HideInInspector]
	public float speed;

	// Token: 0x0400209B RID: 8347
	[HideInInspector]
	public float childDelay;

	// Token: 0x0400209C RID: 8348
	[HideInInspector]
	public float childSpeed;

	// Token: 0x0400209D RID: 8349
	[SerializeField]
	private BasicProjectile childPrefab;

	// Token: 0x0400209E RID: 8350
	private int direction;

	// Token: 0x0400209F RID: 8351
	private float timer;
}
