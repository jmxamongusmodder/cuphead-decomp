using UnityEngine;

public class FlyingMermaidLevelSplashManager : AbstractPausableComponent
{
	public Transform spawnRootFront;
	public Transform spawnRootBack;
	[SerializeField]
	private Effect MegasplashLarge;
	[SerializeField]
	private Effect MegasplashMedium;
	[SerializeField]
	private Effect SplashMedium;
	[SerializeField]
	private Effect SplashSmall;
}
