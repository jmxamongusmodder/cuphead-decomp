using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A19 RID: 2585
public class LevelPlayerColliderManager : AbstractLevelPlayerComponent
{
	// Token: 0x17000529 RID: 1321
	// (get) Token: 0x06003D46 RID: 15686 RVA: 0x0021E4D1 File Offset: 0x0021C8D1
	public LevelPlayerColliderManager.ColliderProperties @default
	{
		get
		{
			return this.colliders.@default;
		}
	}

	// Token: 0x1700052A RID: 1322
	// (get) Token: 0x06003D47 RID: 15687 RVA: 0x0021E4DE File Offset: 0x0021C8DE
	public float DefaultWidth
	{
		get
		{
			return this.colliders.@default.size.x;
		}
	}

	// Token: 0x1700052B RID: 1323
	// (get) Token: 0x06003D48 RID: 15688 RVA: 0x0021E4F5 File Offset: 0x0021C8F5
	public float DefaultHeight
	{
		get
		{
			return this.colliders.@default.size.y;
		}
	}

	// Token: 0x1700052C RID: 1324
	// (get) Token: 0x06003D49 RID: 15689 RVA: 0x0021E50C File Offset: 0x0021C90C
	public float Width
	{
		get
		{
			return this.pairs[(int)this._state].size.x;
		}
	}

	// Token: 0x1700052D RID: 1325
	// (get) Token: 0x06003D4A RID: 15690 RVA: 0x0021E529 File Offset: 0x0021C929
	public float Height
	{
		get
		{
			return this.pairs[(int)this._state].size.y;
		}
	}

	// Token: 0x1700052E RID: 1326
	// (get) Token: 0x06003D4B RID: 15691 RVA: 0x0021E548 File Offset: 0x0021C948
	public Vector2 Center
	{
		get
		{
			return new Vector2(this.boxCollider.offset.x, this.boxCollider.offset.y * base.player.motor.GravityReversalMultiplier) + base.transform.position;
		}
	}

	// Token: 0x1700052F RID: 1327
	// (get) Token: 0x06003D4C RID: 15692 RVA: 0x0021E5A8 File Offset: 0x0021C9A8
	public Vector2 DefaultCenter
	{
		get
		{
			return new Vector2(this.colliders.@default.center.x, this.colliders.@default.center.y * base.player.motor.GravityReversalMultiplier) + base.transform.position;
		}
	}

	// Token: 0x17000530 RID: 1328
	// (get) Token: 0x06003D4D RID: 15693 RVA: 0x0021E60A File Offset: 0x0021CA0A
	// (set) Token: 0x06003D4E RID: 15694 RVA: 0x0021E612 File Offset: 0x0021CA12
	public LevelPlayerColliderManager.State state
	{
		get
		{
			return this._state;
		}
		set
		{
			if (this._state != value)
			{
				this.pairs[(int)value].SetCollider(this.boxCollider);
			}
			this._state = value;
		}
	}

	// Token: 0x06003D4F RID: 15695 RVA: 0x0021E640 File Offset: 0x0021CA40
	protected override void OnAwake()
	{
		base.OnAwake();
		this.boxCollider = base.GetComponent<BoxCollider2D>();
		this.pairs = new Dictionary<int, LevelPlayerColliderManager.ColliderProperties>();
		this.pairs[0] = this.colliders.@default;
		this.pairs[1] = this.colliders.air;
		this.pairs[2] = this.colliders.ducking;
		this.pairs[3] = this.colliders.ducking;
		this.pairs[4] = this.colliders.chaliceFirstJump;
		this.state = LevelPlayerColliderManager.State.Default;
	}

	// Token: 0x06003D50 RID: 15696 RVA: 0x0021E6E4 File Offset: 0x0021CAE4
	private void FixedUpdate()
	{
		this.UpdateColliders();
	}

	// Token: 0x06003D51 RID: 15697 RVA: 0x0021E6EC File Offset: 0x0021CAEC
	private void UpdateColliders()
	{
		base.gameObject.layer = ((!base.player.CanTakeDamage) ? 9 : 8);
		if (base.player.motor.Dashing)
		{
			if (this.state != LevelPlayerColliderManager.State.Dashing)
			{
				this.state = LevelPlayerColliderManager.State.Dashing;
			}
			return;
		}
		if (!base.player.motor.Grounded)
		{
			if (!base.player.stats.isChalice)
			{
				if (this.state != LevelPlayerColliderManager.State.Air)
				{
					this.state = LevelPlayerColliderManager.State.Air;
				}
				return;
			}
			if (!base.player.motor.ChaliceDoubleJumped)
			{
				if (this.state != LevelPlayerColliderManager.State.ChaliceFirstJump)
				{
					this.state = LevelPlayerColliderManager.State.ChaliceFirstJump;
				}
				return;
			}
			if (this.state != LevelPlayerColliderManager.State.Air)
			{
				this.state = LevelPlayerColliderManager.State.Air;
			}
			return;
		}
		else
		{
			if (base.player.motor.Ducking)
			{
				if (this.state != LevelPlayerColliderManager.State.Ducking)
				{
					this.state = LevelPlayerColliderManager.State.Ducking;
				}
				return;
			}
			if (this.state != LevelPlayerColliderManager.State.Default)
			{
				this.state = LevelPlayerColliderManager.State.Default;
			}
			return;
		}
	}

	// Token: 0x04004494 RID: 17556
	[SerializeField]
	private LevelPlayerColliderManager.ColliderPropertiesGroup colliders;

	// Token: 0x04004495 RID: 17557
	private Dictionary<int, LevelPlayerColliderManager.ColliderProperties> pairs;

	// Token: 0x04004496 RID: 17558
	private BoxCollider2D boxCollider;

	// Token: 0x04004497 RID: 17559
	private LevelPlayerColliderManager.State _state;

	// Token: 0x02000A1A RID: 2586
	public enum State
	{
		// Token: 0x04004499 RID: 17561
		Default,
		// Token: 0x0400449A RID: 17562
		Air,
		// Token: 0x0400449B RID: 17563
		Ducking,
		// Token: 0x0400449C RID: 17564
		Dashing,
		// Token: 0x0400449D RID: 17565
		ChaliceFirstJump
	}

	// Token: 0x02000A1B RID: 2587
	[Serializable]
	public class ColliderProperties
	{
		// Token: 0x06003D52 RID: 15698 RVA: 0x0021E7FB File Offset: 0x0021CBFB
		public ColliderProperties(Vector2 center, Vector2 size)
		{
			this.center = center;
			this.size = size;
		}

		// Token: 0x06003D53 RID: 15699 RVA: 0x0021E814 File Offset: 0x0021CC14
		public BoxCollider2D CreateCollider(GameObject gameObject)
		{
			BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
			boxCollider2D.offset = this.center;
			boxCollider2D.size = this.size;
			boxCollider2D.isTrigger = true;
			return boxCollider2D;
		}

		// Token: 0x06003D54 RID: 15700 RVA: 0x0021E848 File Offset: 0x0021CC48
		public void SetCollider(BoxCollider2D boxCollider)
		{
			boxCollider.offset = this.center;
			boxCollider.size = this.size;
			boxCollider.isTrigger = true;
		}

		// Token: 0x0400449E RID: 17566
		public Vector2 center;

		// Token: 0x0400449F RID: 17567
		public Vector2 size;
	}

	// Token: 0x02000A1C RID: 2588
	[Serializable]
	public class ColliderPropertiesGroup
	{
		// Token: 0x040044A0 RID: 17568
		public LevelPlayerColliderManager.ColliderProperties @default = new LevelPlayerColliderManager.ColliderProperties(new Vector2(0f, 62f), new Vector2(50f, 105f));

		// Token: 0x040044A1 RID: 17569
		public LevelPlayerColliderManager.ColliderProperties air = new LevelPlayerColliderManager.ColliderProperties(new Vector2(0f, 50f), new Vector2(50f, 50f));

		// Token: 0x040044A2 RID: 17570
		public LevelPlayerColliderManager.ColliderProperties ducking = new LevelPlayerColliderManager.ColliderProperties(new Vector2(0f, 27f), new Vector2(50f, 35f));

		// Token: 0x040044A3 RID: 17571
		public LevelPlayerColliderManager.ColliderProperties dashing;

		// Token: 0x040044A4 RID: 17572
		public LevelPlayerColliderManager.ColliderProperties chaliceFirstJump = new LevelPlayerColliderManager.ColliderProperties(new Vector2(0f, 78f), new Vector2(50f, 75f));
	}
}
