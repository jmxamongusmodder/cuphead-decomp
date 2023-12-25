using System;

// Token: 0x020009D1 RID: 2513
public class AbstractPlayerComponent : AbstractCollidableObject
{
	// Token: 0x170004D4 RID: 1236
	// (get) Token: 0x06003B0A RID: 15114 RVA: 0x00213827 File Offset: 0x00211C27
	public AbstractPlayerController basePlayer
	{
		get
		{
			if (this._basePlayer == null)
			{
				this._basePlayer = base.GetComponent<AbstractPlayerController>();
			}
			return this._basePlayer;
		}
	}

	// Token: 0x06003B0B RID: 15115 RVA: 0x0021384C File Offset: 0x00211C4C
	protected sealed override void Awake()
	{
		base.Awake();
		this.OnAwake();
	}

	// Token: 0x06003B0C RID: 15116 RVA: 0x0021385A File Offset: 0x00211C5A
	protected virtual void OnAwake()
	{
	}

	// Token: 0x06003B0D RID: 15117 RVA: 0x0021385C File Offset: 0x00211C5C
	public virtual void OnLevelStart()
	{
	}

	// Token: 0x040042C6 RID: 17094
	private AbstractPlayerController _basePlayer;
}
