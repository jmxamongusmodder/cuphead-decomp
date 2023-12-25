using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000B23 RID: 2851
public class CollisionChild : AbstractCollidableObject
{
	// Token: 0x140000C9 RID: 201
	// (add) Token: 0x060044FA RID: 17658 RVA: 0x0011DA2C File Offset: 0x0011BE2C
	// (remove) Token: 0x060044FB RID: 17659 RVA: 0x0011DA64 File Offset: 0x0011BE64
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event CollisionChild.OnCollisionHandler OnAnyCollision;

	// Token: 0x140000CA RID: 202
	// (add) Token: 0x060044FC RID: 17660 RVA: 0x0011DA9C File Offset: 0x0011BE9C
	// (remove) Token: 0x060044FD RID: 17661 RVA: 0x0011DAD4 File Offset: 0x0011BED4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event CollisionChild.OnCollisionHandler OnWallCollision;

	// Token: 0x140000CB RID: 203
	// (add) Token: 0x060044FE RID: 17662 RVA: 0x0011DB0C File Offset: 0x0011BF0C
	// (remove) Token: 0x060044FF RID: 17663 RVA: 0x0011DB44 File Offset: 0x0011BF44
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event CollisionChild.OnCollisionHandler OnGroundCollision;

	// Token: 0x140000CC RID: 204
	// (add) Token: 0x06004500 RID: 17664 RVA: 0x0011DB7C File Offset: 0x0011BF7C
	// (remove) Token: 0x06004501 RID: 17665 RVA: 0x0011DBB4 File Offset: 0x0011BFB4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event CollisionChild.OnCollisionHandler OnCeilingCollision;

	// Token: 0x140000CD RID: 205
	// (add) Token: 0x06004502 RID: 17666 RVA: 0x0011DBEC File Offset: 0x0011BFEC
	// (remove) Token: 0x06004503 RID: 17667 RVA: 0x0011DC24 File Offset: 0x0011C024
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event CollisionChild.OnCollisionHandler OnPlayerCollision;

	// Token: 0x140000CE RID: 206
	// (add) Token: 0x06004504 RID: 17668 RVA: 0x0011DC5C File Offset: 0x0011C05C
	// (remove) Token: 0x06004505 RID: 17669 RVA: 0x0011DC94 File Offset: 0x0011C094
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event CollisionChild.OnCollisionHandler OnPlayerProjectileCollision;

	// Token: 0x140000CF RID: 207
	// (add) Token: 0x06004506 RID: 17670 RVA: 0x0011DCCC File Offset: 0x0011C0CC
	// (remove) Token: 0x06004507 RID: 17671 RVA: 0x0011DD04 File Offset: 0x0011C104
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event CollisionChild.OnCollisionHandler OnEnemyCollision;

	// Token: 0x140000D0 RID: 208
	// (add) Token: 0x06004508 RID: 17672 RVA: 0x0011DD3C File Offset: 0x0011C13C
	// (remove) Token: 0x06004509 RID: 17673 RVA: 0x0011DD74 File Offset: 0x0011C174
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event CollisionChild.OnCollisionHandler OnEnemyProjectileCollision;

	// Token: 0x140000D1 RID: 209
	// (add) Token: 0x0600450A RID: 17674 RVA: 0x0011DDAC File Offset: 0x0011C1AC
	// (remove) Token: 0x0600450B RID: 17675 RVA: 0x0011DDE4 File Offset: 0x0011C1E4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event CollisionChild.OnCollisionHandler OnOtherCollision;

	// Token: 0x0600450C RID: 17676 RVA: 0x0011DE1A File Offset: 0x0011C21A
	public bool ForwardParry(out AbstractCollidableObject collisionParent)
	{
		collisionParent = this.collisionParent;
		return this.forwardParry;
	}

	// Token: 0x0600450D RID: 17677 RVA: 0x0011DE2A File Offset: 0x0011C22A
	private void Start()
	{
		if (this.collisionParent != null)
		{
			this.collisionParent.RegisterCollisionChild(this);
		}
	}

	// Token: 0x0600450E RID: 17678 RVA: 0x0011DE49 File Offset: 0x0011C249
	protected override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		if (this.OnAnyCollision != null)
		{
			this.OnAnyCollision(hit, phase);
		}
	}

	// Token: 0x0600450F RID: 17679 RVA: 0x0011DE63 File Offset: 0x0011C263
	protected override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		if (this.OnWallCollision != null)
		{
			this.OnWallCollision(hit, phase);
		}
	}

	// Token: 0x06004510 RID: 17680 RVA: 0x0011DE7D File Offset: 0x0011C27D
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		if (this.OnGroundCollision != null)
		{
			this.OnGroundCollision(hit, phase);
		}
	}

	// Token: 0x06004511 RID: 17681 RVA: 0x0011DE97 File Offset: 0x0011C297
	protected override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		if (this.OnCeilingCollision != null)
		{
			this.OnCeilingCollision(hit, phase);
		}
	}

	// Token: 0x06004512 RID: 17682 RVA: 0x0011DEB1 File Offset: 0x0011C2B1
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.OnPlayerCollision != null)
		{
			this.OnPlayerCollision(hit, phase);
		}
	}

	// Token: 0x06004513 RID: 17683 RVA: 0x0011DECB File Offset: 0x0011C2CB
	protected override void OnCollisionPlayerProjectile(GameObject hit, CollisionPhase phase)
	{
		if (this.OnPlayerProjectileCollision != null)
		{
			this.OnPlayerProjectileCollision(hit, phase);
		}
	}

	// Token: 0x06004514 RID: 17684 RVA: 0x0011DEE5 File Offset: 0x0011C2E5
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		if (this.OnEnemyCollision != null)
		{
			this.OnEnemyCollision(hit, phase);
		}
	}

	// Token: 0x06004515 RID: 17685 RVA: 0x0011DEFF File Offset: 0x0011C2FF
	protected override void OnCollisionEnemyProjectile(GameObject hit, CollisionPhase phase)
	{
		if (this.OnEnemyProjectileCollision != null)
		{
			this.OnEnemyProjectileCollision(hit, phase);
		}
	}

	// Token: 0x06004516 RID: 17686 RVA: 0x0011DF19 File Offset: 0x0011C319
	protected override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		if (this.OnOtherCollision != null)
		{
			this.OnOtherCollision(hit, phase);
		}
	}

	// Token: 0x04004ACF RID: 19151
	[SerializeField]
	[Tooltip("OPTIONAL: Drag collision parent to this slot to register all collision events to this child. If null, no collisions are registered.")]
	private AbstractCollidableObject collisionParent;

	// Token: 0x04004AD0 RID: 19152
	[SerializeField]
	private bool forwardParry;

	// Token: 0x02000B24 RID: 2852
	// (Invoke) Token: 0x06004518 RID: 17688
	public delegate void OnCollisionHandler(GameObject hit, CollisionPhase phase);
}
