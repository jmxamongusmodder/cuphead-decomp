using UnityEngine;

public class DragonLevel : Level
{
	[SerializeField]
	private DragonLevelBackgroundFlash[] lightningFlashes;
	[SerializeField]
	private DragonLevelCloudPlatform[] platforms;
	[SerializeField]
	private SpriteRenderer spire;
	[SerializeField]
	private SpriteRenderer darkSpire;
	[SerializeField]
	private DragonLevelDragon dragon;
	[SerializeField]
	private DragonLevelLeftSideDragon leftSideDragon;
	[SerializeField]
	private DragonLevelTail tail;
	[SerializeField]
	private DragonLevelPlatformManager manager;
	[SerializeField]
	private DragonLevelLightning lightningStrikes;
	[SerializeField]
	private Material dragonFlashMaterial;
	[SerializeField]
	private SpriteRenderer[] backgroundClouds;
	[SerializeField]
	private Sprite _bossPortraitMain;
	[SerializeField]
	private Sprite _bossPortraitFireMarchers;
	[SerializeField]
	private Sprite _bossPortraitThreeHeads;
	[SerializeField]
	private string _bossQuoteMain;
	[SerializeField]
	private string _bossQuoteFireMarchers;
	[SerializeField]
	private string _bossQuoteThreeHeads;
}
