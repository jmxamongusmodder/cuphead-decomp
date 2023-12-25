using System;
using UnityEngine;

// Token: 0x0200088D RID: 2189
public class TreePlatformingLevelDragonflyShot : PlatformingLevelPathMovementEnemy
{
	// Token: 0x17000440 RID: 1088
	// (get) Token: 0x060032E6 RID: 13030 RVA: 0x001D97BB File Offset: 0x001D7BBB
	// (set) Token: 0x060032E7 RID: 13031 RVA: 0x001D97C3 File Offset: 0x001D7BC3
	public bool isActivated { get; private set; }

	// Token: 0x060032E8 RID: 13032 RVA: 0x001D97CC File Offset: 0x001D7BCC
	protected override void Awake()
	{
		base.Awake();
		this.isActivated = false;
	}

	// Token: 0x060032E9 RID: 13033 RVA: 0x001D97DB File Offset: 0x001D7BDB
	protected override void Die()
	{
		this.Deactivate();
	}

	// Token: 0x060032EA RID: 13034 RVA: 0x001D97E3 File Offset: 0x001D7BE3
	public void Activate()
	{
		base.GetComponent<Collider2D>().enabled = true;
		base.GetComponent<SpriteRenderer>().enabled = true;
		this.isActivated = true;
		this.PrepareShot();
	}

	// Token: 0x060032EB RID: 13035 RVA: 0x001D980A File Offset: 0x001D7C0A
	public void Deactivate()
	{
		base.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<SpriteRenderer>().enabled = false;
		this.isActivated = false;
		base.ResetStartingCondition();
	}

	// Token: 0x060032EC RID: 13036 RVA: 0x001D9831 File Offset: 0x001D7C31
	private void PrepareShot()
	{
		if (Rand.Bool())
		{
			this.startPosition = 0f;
			this._direction = PlatformingLevelPathMovementEnemy.Direction.Forward;
		}
		else
		{
			this.startPosition = 1f;
			this._direction = PlatformingLevelPathMovementEnemy.Direction.Back;
		}
		base.StartFromCustom();
	}
}
