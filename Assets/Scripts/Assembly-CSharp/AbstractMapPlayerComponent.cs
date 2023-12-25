using System;
using UnityEngine;

// Token: 0x02000972 RID: 2418
public abstract class AbstractMapPlayerComponent : AbstractPausableComponent
{
	// Token: 0x1700048C RID: 1164
	// (get) Token: 0x0600384E RID: 14414 RVA: 0x002033E5 File Offset: 0x002017E5
	// (set) Token: 0x0600384F RID: 14415 RVA: 0x002033ED File Offset: 0x002017ED
	public MapPlayerController player { get; private set; }

	// Token: 0x1700048D RID: 1165
	// (get) Token: 0x06003850 RID: 14416 RVA: 0x002033F6 File Offset: 0x002017F6
	// (set) Token: 0x06003851 RID: 14417 RVA: 0x002033FE File Offset: 0x002017FE
	public PlayerInput input { get; private set; }

	// Token: 0x06003852 RID: 14418 RVA: 0x00203407 File Offset: 0x00201807
	protected override void Awake()
	{
		base.Awake();
		this.player = base.GetComponent<MapPlayerController>();
		this.input = base.GetComponent<PlayerInput>();
		this.RegisterEvents();
	}

	// Token: 0x06003853 RID: 14419 RVA: 0x0020342D File Offset: 0x0020182D
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.UnregisterEvents();
	}

	// Token: 0x06003854 RID: 14420 RVA: 0x0020343C File Offset: 0x0020183C
	private void RegisterEvents()
	{
		this.player.LadderEnterEvent += this.OnLadderEnter;
		this.player.LadderEnterCompleteEvent += this.OnLadderEnterComplete;
		this.player.LadderExitEvent += this.OnLadderExit;
		this.player.LadderExitCompleteEvent += this.OnLadderExitComplete;
	}

	// Token: 0x06003855 RID: 14421 RVA: 0x002034AC File Offset: 0x002018AC
	private void UnregisterEvents()
	{
		this.player.LadderEnterEvent -= this.OnLadderEnter;
		this.player.LadderEnterCompleteEvent -= this.OnLadderEnterComplete;
		this.player.LadderExitEvent -= this.OnLadderExit;
		this.player.LadderExitCompleteEvent -= this.OnLadderExitComplete;
	}

	// Token: 0x06003856 RID: 14422 RVA: 0x00203519 File Offset: 0x00201919
	protected virtual void OnLadderEnter(Vector2 point, MapPlayerLadderObject ladder, MapLadder.Location location)
	{
	}

	// Token: 0x06003857 RID: 14423 RVA: 0x0020351B File Offset: 0x0020191B
	protected virtual void OnLadderExit(Vector2 point, Vector2 exit, MapLadder.Location location)
	{
	}

	// Token: 0x06003858 RID: 14424 RVA: 0x0020351D File Offset: 0x0020191D
	protected virtual void OnLadderEnterComplete()
	{
	}

	// Token: 0x06003859 RID: 14425 RVA: 0x0020351F File Offset: 0x0020191F
	protected virtual void OnLadderExitComplete()
	{
	}
}
