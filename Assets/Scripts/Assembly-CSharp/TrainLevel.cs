using UnityEngine;

public class TrainLevel : Level
{
	[SerializeField]
	private TrainLevelTrain train;
	[SerializeField]
	private TrainLevelPumpkin pumpkinPrefab;
	[SerializeField]
	private Transform leftValve;
	[SerializeField]
	private Transform rightValve;
	[SerializeField]
	private TrainLevelBlindSpecter blindSpecter;
	[SerializeField]
	private TrainLevelSkeleton skeleton;
	[SerializeField]
	private TrainLevelLollipopGhoulsManager ghouls;
	[SerializeField]
	private TrainLevelEngineBoss engine;
	public Collider2D handCarCollider;
	[SerializeField]
	private Sprite _bossPortraitSpecter;
	[SerializeField]
	private Sprite _bossPortraitSkeleton;
	[SerializeField]
	private Sprite _bossPortraitLollipop;
	[SerializeField]
	private Sprite _bossPortraitEngine;
	[SerializeField]
	private string _bossQuoteSpecter;
	[SerializeField]
	private string _bossQuoteSkeleton;
	[SerializeField]
	private string _bossQuoteLollipop;
	[SerializeField]
	private string _bossQuoteEngine;
}
