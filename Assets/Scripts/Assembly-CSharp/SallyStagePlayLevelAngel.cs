using UnityEngine;

public class SallyStagePlayLevelAngel : LevelProperties.SallyStagePlay.Entity
{
	[SerializeField]
	private Material phase4Material;
	[SerializeField]
	private SallyStagePlayApplauseHandler applauseHandler;
	[SerializeField]
	private Animator sign;
	[SerializeField]
	private SallyStagePlayLevelWave wave;
	[SerializeField]
	private SallyStagePlayLevelMeteor meteorPrefab;
	[SerializeField]
	private SallyStagePlayLevelLightning lightningPrefab;
	[SerializeField]
	private SallyStagePlayLevelUmbrella umbrellaPrefab;
	[SerializeField]
	private GameObject birdsDeath;
	[SerializeField]
	private SallyStagePlayLevelFianceDeity husband;
	[SerializeField]
	private GameObject[] shadows;
	[SerializeField]
	private Transform phase4Root;
	[SerializeField]
	private Transform birdRoot;
	[SerializeField]
	private Transform phase3Root;
}
