using System;
using UnityEngine;

// Token: 0x0200053B RID: 1339
public class ChessKingLevelGroundTrigger : AbstractCollidableObject
{
	// Token: 0x17000333 RID: 819
	// (get) Token: 0x06001856 RID: 6230 RVA: 0x000DC77F File Offset: 0x000DAB7F
	// (set) Token: 0x06001857 RID: 6231 RVA: 0x000DC787 File Offset: 0x000DAB87
	public bool PLAYER_FALLEN { get; private set; }

	// Token: 0x06001858 RID: 6232 RVA: 0x000DC790 File Offset: 0x000DAB90
	public void CheckPlayer(bool checkPlayer)
	{
		this.checkingPlayer = checkPlayer;
		this.PLAYER_FALLEN = false;
	}

	// Token: 0x06001859 RID: 6233 RVA: 0x000DC7A0 File Offset: 0x000DABA0
	private void Update()
	{
		if (this.checkingPlayer)
		{
			if (PlayerManager.GetPlayer(PlayerId.PlayerOne).transform.position.y < base.transform.position.y)
			{
				this.PLAYER_FALLEN = true;
			}
			else
			{
				this.PLAYER_FALLEN = false;
			}
		}
	}

	// Token: 0x0600185A RID: 6234 RVA: 0x000DC7FC File Offset: 0x000DABFC
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawLine(new Vector3(-800f, base.transform.position.y), new Vector3(800f, base.transform.position.y));
	}

	// Token: 0x0400218D RID: 8589
	private bool checkingPlayer;
}
