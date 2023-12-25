using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000930 RID: 2352
public class MapCastleZoneCollider : AbstractCollidableObject
{
	// Token: 0x14000067 RID: 103
	// (add) Token: 0x06003707 RID: 14087 RVA: 0x001FB674 File Offset: 0x001F9A74
	// (remove) Token: 0x06003708 RID: 14088 RVA: 0x001FB6AC File Offset: 0x001F9AAC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MapCastleZoneCollider.MapCastleZoneCollision OnMapCastleZoneCollision;

	// Token: 0x06003709 RID: 14089 RVA: 0x001FB6E4 File Offset: 0x001F9AE4
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Vector2 vector = this.interactionPoint.position;
		Gizmos.DrawWireSphere(vector, 0.2f);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(vector + this.returnPositions.singlePlayer, 0.2f);
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(vector + this.returnPositions.playerOne, Vector3.one * 0.2f);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(vector + this.returnPositions.playerTwo, Vector3.one * 0.2f);
	}

	// Token: 0x0600370A RID: 14090 RVA: 0x001FB7AC File Offset: 0x001F9BAC
	protected override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (!hit.CompareTag("Player_Map"))
		{
			return;
		}
		if ((phase == CollisionPhase.Enter || phase == CollisionPhase.Exit) && this.OnMapCastleZoneCollision != null)
		{
			this.OnMapCastleZoneCollision(this, hit, phase);
		}
	}

	// Token: 0x04003F3B RID: 16187
	[SerializeField]
	public MapCastleZones.Zone zone;

	// Token: 0x04003F3C RID: 16188
	[SerializeField]
	public Transform interactionPoint;

	// Token: 0x04003F3D RID: 16189
	[SerializeField]
	public bool enableLadderShadow = true;

	// Token: 0x04003F3E RID: 16190
	[SerializeField]
	public AbstractMapInteractiveEntity.PositionProperties returnPositions;

	// Token: 0x02000931 RID: 2353
	// (Invoke) Token: 0x0600370C RID: 14092
	public delegate void MapCastleZoneCollision(MapCastleZoneCollider collider, GameObject other, CollisionPhase phase);
}
