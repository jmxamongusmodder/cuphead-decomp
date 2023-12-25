using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009D9 RID: 2521
public class ArcadePlayerColliderManager : AbstractArcadePlayerComponent
{
	// Token: 0x170004EB RID: 1259
	// (get) Token: 0x06003B72 RID: 15218 RVA: 0x00215009 File Offset: 0x00213409
	public ArcadePlayerColliderManager.ColliderProperties @default
	{
		get
		{
			return this.colliders.@default;
		}
	}

	// Token: 0x170004EC RID: 1260
	// (get) Token: 0x06003B73 RID: 15219 RVA: 0x00215016 File Offset: 0x00213416
	public float DefaultWidth
	{
		get
		{
			return this.colliders.@default.size.x;
		}
	}

	// Token: 0x170004ED RID: 1261
	// (get) Token: 0x06003B74 RID: 15220 RVA: 0x0021502D File Offset: 0x0021342D
	public float DefaultHeight
	{
		get
		{
			return this.colliders.@default.size.y;
		}
	}

	// Token: 0x170004EE RID: 1262
	// (get) Token: 0x06003B75 RID: 15221 RVA: 0x00215044 File Offset: 0x00213444
	public float Width
	{
		get
		{
			return this.pairs[this._state].size.x;
		}
	}

	// Token: 0x170004EF RID: 1263
	// (get) Token: 0x06003B76 RID: 15222 RVA: 0x00215061 File Offset: 0x00213461
	public float Height
	{
		get
		{
			return this.pairs[this._state].size.y;
		}
	}

	// Token: 0x170004F0 RID: 1264
	// (get) Token: 0x06003B77 RID: 15223 RVA: 0x0021507E File Offset: 0x0021347E
	public Vector2 Center
	{
		get
		{
			return this.boxCollider.offset + base.transform.position;
		}
	}

	// Token: 0x170004F1 RID: 1265
	// (get) Token: 0x06003B78 RID: 15224 RVA: 0x002150A0 File Offset: 0x002134A0
	// (set) Token: 0x06003B79 RID: 15225 RVA: 0x002150A8 File Offset: 0x002134A8
	public ArcadePlayerColliderManager.State state
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

	// Token: 0x06003B7A RID: 15226 RVA: 0x002150D4 File Offset: 0x002134D4
	protected override void OnAwake()
	{
		base.OnAwake();
		this.boxCollider = base.GetComponent<BoxCollider2D>();
		this.pairs = new Dictionary<ArcadePlayerColliderManager.State, ArcadePlayerColliderManager.ColliderProperties>();
		this.pairs[ArcadePlayerColliderManager.State.Default] = this.colliders.@default;
		this.pairs[ArcadePlayerColliderManager.State.Air] = this.colliders.air;
		this.pairs[ArcadePlayerColliderManager.State.Dashing] = this.colliders.dashing;
		this.pairs[ArcadePlayerColliderManager.State.Rocket] = this.colliders.rocket;
		this.state = ArcadePlayerColliderManager.State.Default;
	}

	// Token: 0x06003B7B RID: 15227 RVA: 0x00215161 File Offset: 0x00213561
	private void Update()
	{
		this.UpdateColliders();
	}

	// Token: 0x06003B7C RID: 15228 RVA: 0x0021516C File Offset: 0x0021356C
	private void UpdateColliders()
	{
		this.boxCollider.enabled = base.player.CanTakeDamage;
		if (base.player.controlScheme == ArcadePlayerController.ControlScheme.Rocket)
		{
			if (this.state != ArcadePlayerColliderManager.State.Rocket)
			{
				this.state = ArcadePlayerColliderManager.State.Rocket;
			}
			return;
		}
		if (base.player.motor.Dashing)
		{
			if (this.state != ArcadePlayerColliderManager.State.Dashing)
			{
				this.state = ArcadePlayerColliderManager.State.Dashing;
			}
			return;
		}
		if (!base.player.motor.Grounded)
		{
			if (this.state != ArcadePlayerColliderManager.State.Air)
			{
				this.state = ArcadePlayerColliderManager.State.Air;
			}
			return;
		}
		if (this.state != ArcadePlayerColliderManager.State.Default)
		{
			this.state = ArcadePlayerColliderManager.State.Default;
		}
	}

	// Token: 0x04004302 RID: 17154
	[SerializeField]
	private ArcadePlayerColliderManager.ColliderPropertiesGroup colliders;

	// Token: 0x04004303 RID: 17155
	private Dictionary<ArcadePlayerColliderManager.State, ArcadePlayerColliderManager.ColliderProperties> pairs;

	// Token: 0x04004304 RID: 17156
	private BoxCollider2D boxCollider;

	// Token: 0x04004305 RID: 17157
	private ArcadePlayerColliderManager.State _state;

	// Token: 0x020009DA RID: 2522
	public enum State
	{
		// Token: 0x04004307 RID: 17159
		Default,
		// Token: 0x04004308 RID: 17160
		Air,
		// Token: 0x04004309 RID: 17161
		Dashing,
		// Token: 0x0400430A RID: 17162
		Rocket
	}

	// Token: 0x020009DB RID: 2523
	[Serializable]
	public class ColliderProperties
	{
		// Token: 0x06003B7D RID: 15229 RVA: 0x00215218 File Offset: 0x00213618
		public ColliderProperties(Vector2 center, Vector2 size)
		{
			this.center = center;
			this.size = size;
		}

		// Token: 0x06003B7E RID: 15230 RVA: 0x00215230 File Offset: 0x00213630
		public BoxCollider2D CreateCollider(GameObject gameObject)
		{
			BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
			boxCollider2D.offset = this.center;
			boxCollider2D.size = this.size;
			boxCollider2D.isTrigger = true;
			return boxCollider2D;
		}

		// Token: 0x06003B7F RID: 15231 RVA: 0x00215264 File Offset: 0x00213664
		public void SetCollider(BoxCollider2D boxCollider)
		{
			boxCollider.offset = this.center;
			boxCollider.size = this.size;
			boxCollider.isTrigger = true;
		}

		// Token: 0x0400430B RID: 17163
		public Vector2 center;

		// Token: 0x0400430C RID: 17164
		public Vector2 size;
	}

	// Token: 0x020009DC RID: 2524
	[Serializable]
	public class ColliderPropertiesGroup
	{
		// Token: 0x0400430D RID: 17165
		public ArcadePlayerColliderManager.ColliderProperties @default = new ArcadePlayerColliderManager.ColliderProperties(new Vector2(0f, 40f), new Vector2(33f, 70f));

		// Token: 0x0400430E RID: 17166
		public ArcadePlayerColliderManager.ColliderProperties air = new ArcadePlayerColliderManager.ColliderProperties(new Vector2(0f, 33f), new Vector2(33f, 33f));

		// Token: 0x0400430F RID: 17167
		public ArcadePlayerColliderManager.ColliderProperties dashing = new ArcadePlayerColliderManager.ColliderProperties(new Vector2(0f, 18f), new Vector2(33f, 23f));

		// Token: 0x04004310 RID: 17168
		public ArcadePlayerColliderManager.ColliderProperties rocket = new ArcadePlayerColliderManager.ColliderProperties(new Vector2(3.2f, 4f), new Vector2(4f, 66f));
	}
}
