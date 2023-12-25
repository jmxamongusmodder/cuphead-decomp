using System;
using UnityEngine;

// Token: 0x0200096D RID: 2413
public class MapLadder : AbstractMonoBehaviour
{
	// Token: 0x0600383E RID: 14398 RVA: 0x00203078 File Offset: 0x00201478
	protected override void OnDrawGizmos()
	{
		float num = 0.1f;
		base.OnDrawGizmos();
		Vector3 position = base.baseTransform.position;
		Vector3 vector = position + new Vector3(0f, this.height, 0f);
		this.DrawPointGizmos(position, this.bottom);
		this.DrawPointGizmos(vector, this.top);
		Gizmos.color = Color.black;
		Gizmos.DrawLine(position, vector);
		Gizmos.DrawLine(new Vector2(position.x - num, position.y), new Vector2(position.x + num, position.y));
	}

	// Token: 0x0600383F RID: 14399 RVA: 0x00203128 File Offset: 0x00201528
	private void DrawPointGizmos(Vector2 point, MapLadder.PointProperties properties)
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(point + properties.dialogueOffset, 0.05f);
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(point + properties.dialogueOffset, 0.07f);
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(point + properties.interactionPoint, 0.05f);
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(point + properties.interactionPoint, 0.07f);
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(point + properties.interactionPoint, properties.interactionDistance);
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(point + properties.interactionPoint, properties.interactionDistance + 0.02f);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(point + properties.exit, Vector3.one * 0.05f);
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(point + properties.exit, Vector3.one * 0.07f);
	}

	// Token: 0x06003840 RID: 14400 RVA: 0x00203279 File Offset: 0x00201679
	private void SetLayer(SpriteRenderer renderer)
	{
		if (renderer == null)
		{
			return;
		}
		renderer.sortingLayerName = "Background";
		renderer.sortingOrder = 100;
	}

	// Token: 0x04004016 RID: 16406
	public static readonly Vector2 DIALOGUE_OFFSET = new Vector2(0f, 0.5f);

	// Token: 0x04004017 RID: 16407
	public static readonly Vector2 INTERACTION_POINT_TOP = new Vector2(0f, 0.1f);

	// Token: 0x04004018 RID: 16408
	public static readonly Vector2 INTERACTION_POINT_BOTTOM = new Vector2(0f, -0.1f);

	// Token: 0x04004019 RID: 16409
	public const float INTERACTION_DISTANCE = 0.2f;

	// Token: 0x0400401A RID: 16410
	public static readonly AbstractUIInteractionDialogue.Properties DIALOGUE_ENTER = new AbstractUIInteractionDialogue.Properties("CLIMB");

	// Token: 0x0400401B RID: 16411
	public static readonly AbstractUIInteractionDialogue.Properties DIALOGUE_EXIT = new AbstractUIInteractionDialogue.Properties("EXIT");

	// Token: 0x0400401C RID: 16412
	public static readonly Vector2 EXIT_TOP = new Vector2(0f, 0.2f);

	// Token: 0x0400401D RID: 16413
	public static readonly Vector2 EXIT_BOTTOM = new Vector2(0f, -0.2f);

	// Token: 0x0400401E RID: 16414
	public float height = 1f;

	// Token: 0x0400401F RID: 16415
	[SerializeField]
	private MapLadder.PointProperties top = MapLadder.PointProperties.TopDefault();

	// Token: 0x04004020 RID: 16416
	[SerializeField]
	private MapLadder.PointProperties bottom = MapLadder.PointProperties.BottomDefault();

	// Token: 0x0200096E RID: 2414
	public enum Location
	{
		// Token: 0x04004022 RID: 16418
		Top,
		// Token: 0x04004023 RID: 16419
		Bottom
	}

	// Token: 0x0200096F RID: 2415
	[Serializable]
	public class PointProperties
	{
		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06003843 RID: 14403 RVA: 0x0020335F File Offset: 0x0020175F
		// (set) Token: 0x06003844 RID: 14404 RVA: 0x00203367 File Offset: 0x00201767
		public MapLadder.Location location { get; private set; }

		// Token: 0x06003845 RID: 14405 RVA: 0x00203370 File Offset: 0x00201770
		public static MapLadder.PointProperties TopDefault()
		{
			return new MapLadder.PointProperties
			{
				interactionPoint = MapLadder.INTERACTION_POINT_TOP,
				exit = MapLadder.EXIT_TOP,
				location = MapLadder.Location.Top
			};
		}

		// Token: 0x06003846 RID: 14406 RVA: 0x002033A4 File Offset: 0x002017A4
		public static MapLadder.PointProperties BottomDefault()
		{
			return new MapLadder.PointProperties
			{
				interactionPoint = MapLadder.INTERACTION_POINT_BOTTOM,
				exit = MapLadder.EXIT_BOTTOM,
				location = MapLadder.Location.Bottom
			};
		}

		// Token: 0x04004024 RID: 16420
		public Vector2 interactionPoint = Vector2.zero;

		// Token: 0x04004025 RID: 16421
		public float interactionDistance = 0.2f;

		// Token: 0x04004026 RID: 16422
		public Vector2 dialogueOffset = MapLadder.DIALOGUE_OFFSET;

		// Token: 0x04004027 RID: 16423
		public Vector2 exit = Vector2.zero;
	}
}
