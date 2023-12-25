using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000904 RID: 2308
public class PlatformingLevelExit : AbstractCollidableObject
{
	// Token: 0x14000063 RID: 99
	// (add) Token: 0x0600361F RID: 13855 RVA: 0x001F7014 File Offset: 0x001F5414
	// (remove) Token: 0x06003620 RID: 13856 RVA: 0x001F7048 File Offset: 0x001F5448
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event Action OnWinStartEvent;

	// Token: 0x14000064 RID: 100
	// (add) Token: 0x06003621 RID: 13857 RVA: 0x001F707C File Offset: 0x001F547C
	// (remove) Token: 0x06003622 RID: 13858 RVA: 0x001F70B0 File Offset: 0x001F54B0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event Action OnWinCompleteEvent;

	// Token: 0x06003623 RID: 13859 RVA: 0x001F70E4 File Offset: 0x001F54E4
	private void FixedUpdate()
	{
		if (this._activated)
		{
			if (!this._exited)
			{
				for (int i = 0; i < 2; i++)
				{
					AbstractPlayerController player = PlayerManager.GetPlayer((i != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne);
					if (!(player == null))
					{
						if (player.center.x > base.transform.position.x + this._exitDistance)
						{
							this._exited = true;
							if (PlatformingLevelExit.OnWinCompleteEvent != null)
							{
								PlatformingLevelExit.OnWinCompleteEvent();
								PlatformingLevelExit.OnWinCompleteEvent = null;
							}
							break;
						}
					}
				}
			}
		}
		else
		{
			for (int j = 0; j < 2; j++)
			{
				AbstractPlayerController player2 = PlayerManager.GetPlayer((j != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne);
				if (!(player2 == null) && !player2.IsDead)
				{
					if (player2.center.x > base.transform.position.x)
					{
						this._activated = true;
						if (PlatformingLevelExit.OnWinStartEvent != null)
						{
							PlatformingLevelExit.OnWinStartEvent();
							PlatformingLevelExit.OnWinStartEvent = null;
						}
						PlatformingLevelEnd.Win();
						base.StartCoroutine(this.on_win_complete_cr());
						break;
					}
				}
			}
		}
	}

	// Token: 0x06003624 RID: 13860 RVA: 0x001F723F File Offset: 0x001F563F
	protected override void OnDestroy()
	{
		base.OnDestroy();
		PlatformingLevelExit.OnWinStartEvent = null;
		PlatformingLevelExit.OnWinCompleteEvent = null;
	}

	// Token: 0x06003625 RID: 13861 RVA: 0x001F7254 File Offset: 0x001F5654
	private IEnumerator on_win_complete_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.onCompleteWaitTime);
		if (PlatformingLevelExit.OnWinCompleteEvent != null)
		{
			PlatformingLevelExit.OnWinCompleteEvent();
			PlatformingLevelExit.OnWinCompleteEvent = null;
		}
		yield break;
	}

	// Token: 0x06003626 RID: 13862 RVA: 0x001F726F File Offset: 0x001F566F
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.5f);
	}

	// Token: 0x06003627 RID: 13863 RVA: 0x001F7282 File Offset: 0x001F5682
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x06003628 RID: 13864 RVA: 0x001F7298 File Offset: 0x001F5698
	private void DrawGizmos(float a)
	{
		Gizmos.color = new Color(0f, 1f, 0f, a);
		Vector3 vector = base.baseTransform.position + new Vector3(this._exitDistance, 0f, 0f);
		Gizmos.DrawLine(base.baseTransform.position, vector);
		Gizmos.DrawLine(vector + new Vector3(0f, -5000f, 0f), vector + new Vector3(0f, 5000f, 0f));
		Gizmos.color = Color.white;
	}

	// Token: 0x04003E27 RID: 15911
	[SerializeField]
	[Range(200f, 1500f)]
	private float _exitDistance = 500f;

	// Token: 0x04003E28 RID: 15912
	[SerializeField]
	private float onCompleteWaitTime = 2f;

	// Token: 0x04003E29 RID: 15913
	private bool _activated;

	// Token: 0x04003E2A RID: 15914
	private bool _exited;
}
