using UnityEngine.UI;
using UnityEngine;

public class OldManLevel : Level
{
	[SerializeField]
	private Image fader;
	[SerializeField]
	private GameObject[] hairObjects;
	[SerializeField]
	private OldManLevelScubaGnome scubaGnomePrefab;
	[SerializeField]
	private GameObject mainPlatform;
	public OldManLevelPlatformManager platformManager;
	[SerializeField]
	private OldManLevelOldMan oldMan;
	[SerializeField]
	private OldManLevelSockPuppetHandler sockPuppet;
	[SerializeField]
	private OldManLevelGnomeLeader gnomeLeader;
	[SerializeField]
	private OldManLevelGnomeClimber gnomeClimberPrefab;
	[SerializeField]
	private OldManLevelSpikeFloor[] spikes;
	[SerializeField]
	private GameObject mountainBG;
	[SerializeField]
	private GameObject cloudLeft;
	[SerializeField]
	private GameObject cloudRight;
	[SerializeField]
	private GameObject stomachBG;
	[SerializeField]
	private Collider2D phaseTransitionTrigger;
	[SerializeField]
	private GameObject mainPit;
	[SerializeField]
	private GameObject bleachers;
	public bool playedFirstSpikeSound;
	[SerializeField]
	private Effect smokePrefab;
	[SerializeField]
	private Effect sparklePrefab;
	[SerializeField]
	private Sprite _bossPortraitMain;
	[SerializeField]
	private Sprite _bossPortraitPhaseTwo;
	[SerializeField]
	private Sprite _bossPortraitPhaseThree;
	[SerializeField]
	private string _bossQuoteMain;
	[SerializeField]
	private string _bossQuotePhaseTwo;
	[SerializeField]
	private string _bossQuotePhaseThree;
}
