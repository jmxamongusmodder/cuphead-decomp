using UnityEngine;

public class RetroArcadeLevel : Level
{
	[SerializeField]
	private Sprite _bossPortrait;
	[SerializeField]
	[MultilineAttribute]
	private string _bossQuote;
	[SerializeField]
	private RetroArcadeTrafficManager trafficManager;
	[SerializeField]
	private RetroArcadeTentacleManager tentacleManager;
	[SerializeField]
	private RetroArcadeSnakeManager snakeManager;
	[SerializeField]
	private RetroArcadeSheriffManager sheriffManager;
	[SerializeField]
	private RetroArcadeChaserManager chaserManager;
	[SerializeField]
	private RetroArcadeBouncyManager bouncyManager;
	[SerializeField]
	private RetroArcadeAlienManager alienManager;
	[SerializeField]
	private RetroArcadeCaterpillarManager caterpillarManager;
	[SerializeField]
	private RetroArcadeRobotManager robotManager;
	[SerializeField]
	private RetroArcadePaddleShip paddleShip;
	[SerializeField]
	private RetroArcadeQShip qShip;
	[SerializeField]
	private RetroArcadeUFO ufo;
	[SerializeField]
	private RetroArcadeToadManager toadManager;
	[SerializeField]
	private RetroArcadeMissileMan missileMan;
	[SerializeField]
	private RetroArcadeWorm worm;
	[SerializeField]
	private RetroArcadeBigPlayer bigCuphead;
	[SerializeField]
	private RetroArcadeBigPlayer bigMugman;
	public ArcadePlayerController playerPrefab;
}
