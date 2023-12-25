using System;
using UnityEngine;

// Token: 0x02000AC3 RID: 2755
public class PlayerCameraController : AbstractPlayerComponent
{
	// Token: 0x06004228 RID: 16936 RVA: 0x0023BAC4 File Offset: 0x00239EC4
	public void LevelInit()
	{
		this.rect = default(Rect);
		this.rect.x = base.transform.position.x - 100f;
		this.rect.width = 200f;
		this.rect.y = base.transform.position.y - 150f;
		this.rect.height = 300f;
	}

	// Token: 0x06004229 RID: 16937 RVA: 0x0023BB48 File Offset: 0x00239F48
	private void Update()
	{
		if (base.basePlayer.right > this.rect.x + this.rect.width)
		{
			this.rect.x = base.basePlayer.right - 200f;
		}
		if (base.basePlayer.left < this.rect.xMin)
		{
			this.rect.x = base.basePlayer.left;
		}
		if (base.basePlayer.top > this.rect.y + 300f)
		{
			this.rect.y = base.basePlayer.top - 300f;
		}
		if (base.basePlayer.bottom < this.rect.y)
		{
			this.rect.y = base.basePlayer.bottom;
		}
	}

	// Token: 0x0600422A RID: 16938 RVA: 0x0023BC38 File Offset: 0x0023A038
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (!PlayerDebug.Enabled)
		{
			return;
		}
		Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
		Gizmos.DrawWireCube(this.rect.center, new Vector3(this.rect.width, this.rect.height, 1f));
		Gizmos.color = Color.white;
	}

	// Token: 0x170005CC RID: 1484
	// (get) Token: 0x0600422B RID: 16939 RVA: 0x0023BCB3 File Offset: 0x0023A0B3
	public Vector2 center
	{
		get
		{
			return this.rect.center;
		}
	}

	// Token: 0x040048A3 RID: 18595
	public const float WIDTH = 200f;

	// Token: 0x040048A4 RID: 18596
	public const float HEIGHT = 300f;

	// Token: 0x040048A5 RID: 18597
	private Rect rect = default(Rect);
}
