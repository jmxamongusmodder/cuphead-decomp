using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008FD RID: 2301
public class PlatformingLevelAutoscrollObject : AbstractCollidableObject
{
	// Token: 0x17000461 RID: 1121
	// (get) Token: 0x060035FA RID: 13818 RVA: 0x001EA4C1 File Offset: 0x001E88C1
	// (set) Token: 0x060035FB RID: 13819 RVA: 0x001EA4C9 File Offset: 0x001E88C9
	private protected bool isMoving { protected get; private set; }

	// Token: 0x060035FC RID: 13820 RVA: 0x001EA4D2 File Offset: 0x001E88D2
	protected override void Awake()
	{
		base.Awake();
		this.isMoving = false;
		this.isLocked = false;
	}

	// Token: 0x060035FD RID: 13821 RVA: 0x001EA4E8 File Offset: 0x001E88E8
	protected virtual void Start()
	{
		if (this.checkToLock)
		{
			base.StartCoroutine(this.check_to_lock_cr());
		}
	}

	// Token: 0x060035FE RID: 13822 RVA: 0x001EA504 File Offset: 0x001E8904
	protected virtual void Update()
	{
		if (this.isMoving && base.transform.position.x > this.endPosition.transform.position.x)
		{
			this.StartEndingAutoscroll();
			this.isMoving = false;
		}
	}

	// Token: 0x060035FF RID: 13823 RVA: 0x001EA55C File Offset: 0x001E895C
	protected virtual IEnumerator check_to_lock_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.1f);
		float dist = PlayerManager.Center.x - base.transform.position.x;
		while (dist < -this.lockDistance)
		{
			dist = PlayerManager.Center.x - base.transform.position.x;
			yield return null;
		}
		CupheadLevelCamera.Current.LockCamera(true);
		this.isLocked = true;
		yield return null;
		yield break;
	}

	// Token: 0x06003600 RID: 13824 RVA: 0x001EA577 File Offset: 0x001E8977
	protected virtual void StartAutoscroll()
	{
		this.isMoving = true;
		this.isLocked = false;
		CupheadLevelCamera.Current.LockCamera(false);
		CupheadLevelCamera.Current.SetAutoScroll(true);
	}

	// Token: 0x06003601 RID: 13825 RVA: 0x001EA59D File Offset: 0x001E899D
	protected virtual void StartEndingAutoscroll()
	{
		base.StartCoroutine(this.end_autoscroll());
	}

	// Token: 0x06003602 RID: 13826 RVA: 0x001EA5AC File Offset: 0x001E89AC
	protected virtual void EndAutoscroll()
	{
	}

	// Token: 0x06003603 RID: 13827 RVA: 0x001EA5B0 File Offset: 0x001E89B0
	private IEnumerator end_autoscroll()
	{
		CupheadLevelCamera.Current.SetAutoScroll(false);
		this.EndAutoscroll();
		yield return null;
		yield break;
	}

	// Token: 0x06003604 RID: 13828 RVA: 0x001EA5CC File Offset: 0x001E89CC
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (this.endPosition.transform != null)
		{
			Gizmos.DrawLine(new Vector3(this.endPosition.transform.position.x, this.endPosition.transform.position.y + 1500f), new Vector3(this.endPosition.transform.position.x, this.endPosition.transform.position.y - 1500f));
		}
	}

	// Token: 0x04003E08 RID: 15880
	[SerializeField]
	private Transform endPosition;

	// Token: 0x04003E09 RID: 15881
	protected float lockDistance = 600f;

	// Token: 0x04003E0A RID: 15882
	protected float endDelay = 1f;

	// Token: 0x04003E0C RID: 15884
	protected bool checkToLock;

	// Token: 0x04003E0D RID: 15885
	protected bool isLocked;
}
