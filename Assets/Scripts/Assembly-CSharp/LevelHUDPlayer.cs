using UnityEngine;

public class LevelHUDPlayer : AbstractPausableComponent
{
	[SerializeField]
	private LevelHUDPlayerHealth health;
	[SerializeField]
	private LevelHUDPlayerSuper super;
	[SerializeField]
	private RectTransform weaponRoot;
	[SerializeField]
	private LevelHUDWeapon weaponIconPrefab;
	[SerializeField]
	private CanvasGroup weaponSwitchNotification;
	public float weaponSwitchWobbleSpeed;
	public float weaponSwitchWobbleScale;
}
