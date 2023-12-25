using System;

// Token: 0x020005F8 RID: 1528
public class DragonLevelPeashot : BasicProjectile
{
	// Token: 0x17000376 RID: 886
	// (get) Token: 0x06001E6B RID: 7787 RVA: 0x00118AE8 File Offset: 0x00116EE8
	// (set) Token: 0x06001E6C RID: 7788 RVA: 0x00118AF0 File Offset: 0x00116EF0
	public int color
	{
		get
		{
			return this._color;
		}
		set
		{
			this._color = value;
			this.SetColor();
		}
	}

	// Token: 0x06001E6D RID: 7789 RVA: 0x00118AFF File Offset: 0x00116EFF
	private void SetColor()
	{
		base.animator.SetInteger("Color", this._color);
		if (this.color == 2)
		{
			this.SetParryable(true);
		}
	}

	// Token: 0x04002748 RID: 10056
	private int _color;
}
