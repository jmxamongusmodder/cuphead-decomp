using UnityEngine;

public class SlimeLevelSlime : LevelProperties.Slime.Entity
{
	[SerializeField]
	private Animator[] questionMarks;
	[SerializeField]
	private SpriteRenderer shadow;
	[SerializeField]
	private float shadowMaxY;
	[SerializeField]
	private SlimeLevelSlime bigSlime;
	[SerializeField]
	private SlimeLevelTombstone tombStone;
	[SerializeField]
	private Effect explosionPrefab;
	[SerializeField]
	private Effect dustPrefab;
	[SerializeField]
	private bool isBig;
	[SerializeField]
	private float punchMaxX;
	[SerializeField]
	private float punchMinX;
	[SerializeField]
	private float maxX;
	[SerializeField]
	private Transform eyeMaxPosition;
}
