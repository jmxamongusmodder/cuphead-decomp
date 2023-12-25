using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200085C RID: 2140
public class PlatformingLevelBigEnemy : PlatformingLevelShootingEnemy
{
	// Token: 0x060031B6 RID: 12726 RVA: 0x001D0CE9 File Offset: 0x001CF0E9
	protected override void Start()
	{
		base.Start();
		base.FrameDelayedCallback(new Action(this.StartLockCheck), 1);
	}

	// Token: 0x060031B7 RID: 12727 RVA: 0x001D0D05 File Offset: 0x001CF105
	private void StartLockCheck()
	{
		base.StartCoroutine(this.camera_locking_cr());
	}

	// Token: 0x060031B8 RID: 12728 RVA: 0x001D0D14 File Offset: 0x001CF114
	private IEnumerator camera_locking_cr()
	{
		while (!this.isDead)
		{
			if (!this.bigEnemyCameraLock)
			{
				this.dist = PlayerManager.Center.x - base.transform.position.x;
				if (this.dist > -this.LockDistance)
				{
					this.bigEnemyCameraLock = true;
					CupheadLevelCamera.Current.LockCamera(true);
					this.OnLock();
				}
			}
			else if (this.bigEnemyCameraLock)
			{
				this.dist = PlayerManager.Center.x - this.passDistance.transform.position.x;
				if (this.dist > 0f)
				{
					this.bigEnemyCameraLock = false;
					CupheadLevelCamera.Current.LockCamera(false);
					this.OnPass();
					break;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060031B9 RID: 12729 RVA: 0x001D0D2F File Offset: 0x001CF12F
	protected virtual void OnPass()
	{
	}

	// Token: 0x060031BA RID: 12730 RVA: 0x001D0D31 File Offset: 0x001CF131
	protected virtual void OnLock()
	{
	}

	// Token: 0x060031BB RID: 12731 RVA: 0x001D0D33 File Offset: 0x001CF133
	protected override void Die()
	{
		this.bigEnemyCameraLock = false;
		CupheadLevelCamera.Current.LockCamera(false);
		base.Die();
	}

	// Token: 0x060031BC RID: 12732 RVA: 0x001D0D50 File Offset: 0x001CF150
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.cyan;
		Gizmos.DrawLine(this.passDistance.transform.position, new Vector3(this.passDistance.transform.position.x, this.passDistance.transform.position.y - 1000f));
	}

	// Token: 0x04003A19 RID: 14873
	[SerializeField]
	private Transform passDistance;

	// Token: 0x04003A1A RID: 14874
	protected float LockDistance = 500f;

	// Token: 0x04003A1B RID: 14875
	protected bool bigEnemyCameraLock;

	// Token: 0x04003A1C RID: 14876
	protected bool isDead;

	// Token: 0x04003A1D RID: 14877
	private float dist;
}
