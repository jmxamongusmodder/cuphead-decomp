using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A95 RID: 2709
public class PlanePlayerColliderManager : AbstractPlanePlayerComponent
{
	// Token: 0x170005A4 RID: 1444
	// (get) Token: 0x060040EC RID: 16620 RVA: 0x0023523C File Offset: 0x0023363C
	// (set) Token: 0x060040ED RID: 16621 RVA: 0x00235244 File Offset: 0x00233644
	public PlanePlayerColliderManager.State state
	{
		get
		{
			return this._state;
		}
		set
		{
			if (this._state != value)
			{
				this.pairs[value].SetCollider(this.boxCollider);
			}
			this._state = value;
		}
	}

	// Token: 0x170005A5 RID: 1445
	// (get) Token: 0x060040EE RID: 16622 RVA: 0x00235270 File Offset: 0x00233670
	public PlanePlayerColliderManager.ColliderProperties @default
	{
		get
		{
			return this.colliders.@default;
		}
	}

	// Token: 0x060040EF RID: 16623 RVA: 0x00235280 File Offset: 0x00233680
	protected override void OnAwake()
	{
		base.OnAwake();
		this.boxCollider = base.GetComponent<BoxCollider2D>();
		this.pairs = new Dictionary<PlanePlayerColliderManager.State, PlanePlayerColliderManager.ColliderProperties>();
		this.pairs[PlanePlayerColliderManager.State.Default] = this.colliders.@default;
		this.pairs[PlanePlayerColliderManager.State.Shrunk] = this.colliders.shrunk;
		this.state = PlanePlayerColliderManager.State.Default;
	}

	// Token: 0x060040F0 RID: 16624 RVA: 0x002352DF File Offset: 0x002336DF
	private void Update()
	{
		this.UpdateColliders();
	}

	// Token: 0x060040F1 RID: 16625 RVA: 0x002352E8 File Offset: 0x002336E8
	private void UpdateColliders()
	{
		this.boxCollider.enabled = base.player.CanTakeDamage;
		if (base.player.Shrunk)
		{
			if (this.state != PlanePlayerColliderManager.State.Shrunk)
			{
				this.state = PlanePlayerColliderManager.State.Shrunk;
			}
			return;
		}
		if (this.state != PlanePlayerColliderManager.State.Default)
		{
			this.state = PlanePlayerColliderManager.State.Default;
		}
	}

	// Token: 0x04004793 RID: 18323
	[SerializeField]
	private PlanePlayerColliderManager.ColliderPropertiesGroup colliders;

	// Token: 0x04004794 RID: 18324
	private Dictionary<PlanePlayerColliderManager.State, PlanePlayerColliderManager.ColliderProperties> pairs;

	// Token: 0x04004795 RID: 18325
	private BoxCollider2D boxCollider;

	// Token: 0x04004796 RID: 18326
	private PlanePlayerColliderManager.State _state;

	// Token: 0x02000A96 RID: 2710
	public enum State
	{
		// Token: 0x04004798 RID: 18328
		Default,
		// Token: 0x04004799 RID: 18329
		Shrunk
	}

	// Token: 0x02000A97 RID: 2711
	[Serializable]
	public class ColliderProperties
	{
		// Token: 0x060040F2 RID: 16626 RVA: 0x00235341 File Offset: 0x00233741
		public ColliderProperties(Vector2 center, Vector2 size)
		{
			this.center = center;
			this.size = size;
		}

		// Token: 0x060040F3 RID: 16627 RVA: 0x00235358 File Offset: 0x00233758
		public BoxCollider2D CreateCollider(GameObject gameObject)
		{
			BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
			boxCollider2D.offset = this.center;
			boxCollider2D.size = this.size;
			boxCollider2D.isTrigger = true;
			return boxCollider2D;
		}

		// Token: 0x060040F4 RID: 16628 RVA: 0x0023538C File Offset: 0x0023378C
		public void SetCollider(BoxCollider2D boxCollider)
		{
			boxCollider.offset = this.center;
			boxCollider.size = this.size;
			boxCollider.isTrigger = true;
		}

		// Token: 0x0400479A RID: 18330
		public Vector2 center;

		// Token: 0x0400479B RID: 18331
		public Vector2 size;
	}

	// Token: 0x02000A98 RID: 2712
	[Serializable]
	public class ColliderPropertiesGroup
	{
		// Token: 0x0400479C RID: 18332
		public PlanePlayerColliderManager.ColliderProperties @default = new PlanePlayerColliderManager.ColliderProperties(new Vector2(-10f, 20f), new Vector2(75f, 75f));

		// Token: 0x0400479D RID: 18333
		public PlanePlayerColliderManager.ColliderProperties shrunk = new PlanePlayerColliderManager.ColliderProperties(new Vector2(-10f, 20f), new Vector2(45f, 45f));
	}
}
