using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000373 RID: 883
public class GameObjectHelperGO : MonoBehaviour
{
	// Token: 0x14000007 RID: 7
	// (add) Token: 0x06000A2F RID: 2607 RVA: 0x0007E438 File Offset: 0x0007C838
	// (remove) Token: 0x06000A30 RID: 2608 RVA: 0x0007E470 File Offset: 0x0007C870
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event GameObjectHelperGO.OnUpdateHandler onUpdate;

	// Token: 0x06000A31 RID: 2609 RVA: 0x0007E4A6 File Offset: 0x0007C8A6
	protected void Update()
	{
		if (this.onUpdate != null)
		{
			this.onUpdate();
		}
	}

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x06000A32 RID: 2610 RVA: 0x0007E4C0 File Offset: 0x0007C8C0
	// (remove) Token: 0x06000A33 RID: 2611 RVA: 0x0007E4F8 File Offset: 0x0007C8F8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event GameObjectHelperGO.OnFixedUpdateHandler onFixedUpdate;

	// Token: 0x06000A34 RID: 2612 RVA: 0x0007E52E File Offset: 0x0007C92E
	protected void FixedUpdate()
	{
		if (this.onFixedUpdate != null)
		{
			this.onFixedUpdate();
		}
	}

	// Token: 0x14000009 RID: 9
	// (add) Token: 0x06000A35 RID: 2613 RVA: 0x0007E548 File Offset: 0x0007C948
	// (remove) Token: 0x06000A36 RID: 2614 RVA: 0x0007E580 File Offset: 0x0007C980
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event GameObjectHelperGO.OnLateUpdateHandler onLateUpdate;

	// Token: 0x06000A37 RID: 2615 RVA: 0x0007E5B6 File Offset: 0x0007C9B6
	protected void LateUpdate()
	{
		if (this.onLateUpdate != null)
		{
			this.onLateUpdate();
		}
	}

	// Token: 0x1400000A RID: 10
	// (add) Token: 0x06000A38 RID: 2616 RVA: 0x0007E5D0 File Offset: 0x0007C9D0
	// (remove) Token: 0x06000A39 RID: 2617 RVA: 0x0007E608 File Offset: 0x0007CA08
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event GameObjectHelperGO.OnDrawGizmosHandler onDrawGizmos;

	// Token: 0x06000A3A RID: 2618 RVA: 0x0007E63E File Offset: 0x0007CA3E
	protected void OnDrawGizmos()
	{
		if (this.onDrawGizmos != null)
		{
			this.onDrawGizmos();
		}
	}

	// Token: 0x1400000B RID: 11
	// (add) Token: 0x06000A3B RID: 2619 RVA: 0x0007E658 File Offset: 0x0007CA58
	// (remove) Token: 0x06000A3C RID: 2620 RVA: 0x0007E690 File Offset: 0x0007CA90
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event GameObjectHelperGO.OnDestroyHandler onDestroy;

	// Token: 0x06000A3D RID: 2621 RVA: 0x0007E6C6 File Offset: 0x0007CAC6
	protected void OnDestroy()
	{
		if (this.onDestroy != null)
		{
			this.onDestroy();
		}
		this.clear();
	}

	// Token: 0x06000A3E RID: 2622 RVA: 0x0007E6E4 File Offset: 0x0007CAE4
	private void clear()
	{
		this.onUpdate = null;
		this.onFixedUpdate = null;
		this.onLateUpdate = null;
		this.onDrawGizmos = null;
	}

	// Token: 0x02000374 RID: 884
	// (Invoke) Token: 0x06000A40 RID: 2624
	public delegate void OnUpdateHandler();

	// Token: 0x02000375 RID: 885
	// (Invoke) Token: 0x06000A44 RID: 2628
	public delegate void OnFixedUpdateHandler();

	// Token: 0x02000376 RID: 886
	// (Invoke) Token: 0x06000A48 RID: 2632
	public delegate void OnLateUpdateHandler();

	// Token: 0x02000377 RID: 887
	// (Invoke) Token: 0x06000A4C RID: 2636
	public delegate void OnDrawGizmosHandler();

	// Token: 0x02000378 RID: 888
	// (Invoke) Token: 0x06000A50 RID: 2640
	public delegate void OnDestroyHandler();
}
