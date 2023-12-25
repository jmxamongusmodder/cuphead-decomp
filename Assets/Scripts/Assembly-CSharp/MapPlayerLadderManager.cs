using System;
using UnityEngine;

// Token: 0x0200097D RID: 2429
public class MapPlayerLadderManager : AbstractMapPlayerComponent
{
	// Token: 0x1700049A RID: 1178
	// (get) Token: 0x060038B1 RID: 14513 RVA: 0x00204960 File Offset: 0x00202D60
	// (set) Token: 0x060038B2 RID: 14514 RVA: 0x00204968 File Offset: 0x00202D68
	public MapPlayerLadderObject Current { get; private set; }

	// Token: 0x060038B3 RID: 14515 RVA: 0x00204971 File Offset: 0x00202D71
	protected override void Awake()
	{
		base.Awake();
	}

	// Token: 0x060038B4 RID: 14516 RVA: 0x00204979 File Offset: 0x00202D79
	protected override void OnLadderEnter(Vector2 point, MapPlayerLadderObject ladder, MapLadder.Location location)
	{
		base.OnLadderEnter(point, ladder, location);
		base.GetComponent<Collider2D>().enabled = false;
		this.Current = ladder;
	}

	// Token: 0x060038B5 RID: 14517 RVA: 0x00204997 File Offset: 0x00202D97
	protected override void OnLadderExitComplete()
	{
		base.OnLadderExitComplete();
		base.GetComponent<Collider2D>().enabled = true;
		this.Current = null;
	}
}
