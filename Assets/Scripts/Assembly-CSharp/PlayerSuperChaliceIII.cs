using UnityEngine;

public class PlayerSuperChaliceIII : AbstractPlayerSuper
{
	[SerializeField]
	private BasicProjectile minionPrefab;
	[SerializeField]
	private int minionCount;
	[SerializeField]
	private bool wave;
	[SerializeField]
	private float aimSinkRate;
	[SerializeField]
	private SpriteRenderer chaliceSprite;
	[SerializeField]
	private SpriteRenderer fxRenderer;
	[SerializeField]
	private MinMax[] minionVerticalRange;
	[SerializeField]
	private MinMax[] minionScaleRange;
	[SerializeField]
	private float[] minionSpeed;
	[SerializeField]
	private float[] minionDamage;
	[SerializeField]
	private string minionSpawnString;
	[SerializeField]
	private string minionSpawnTimingString;
	[SerializeField]
	private int[] minionTypeCount;
	[SerializeField]
	private PlayerSuperChaliceIIISpear spear;
	[SerializeField]
	private Transform target;
	[SerializeField]
	private bool linkSpeedToZoom;
	[SerializeField]
	private bool linkScaleToZoom;
	[SerializeField]
	private bool linkRangeToZoom;
	[SerializeField]
	private bool linkDamageToZoom;
	[SerializeField]
	private bool linkSpawnRateToZoom;
	[SerializeField]
	private float spawnRateZoomModifier;
}
