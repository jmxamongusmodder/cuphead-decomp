using UnityEngine;

public class TowerOfPowerLevelGameManager : LevelProperties.TowerOfPower.Entity
{
	[SerializeField]
	private float advanceDelay;
	public bool[] slotsAreSpinning;
	public bool[] slotsCanSpinAgain;
	public bool[] slotsConfirm;
	[SerializeField]
	private TowerOfPowerScorecard scorecard;
	public bool showingScorecard;
	[SerializeField]
	private bool debugForceSlotMachineEveryTurn;
	[SerializeField]
	private bool debugForceSlotMachineAfterOneFight;
	[SerializeField]
	private bool debugSkipToLastFight;
}
