using UnityEngine;

public class DicePalacePachinkoLevelPachinko : LevelProperties.DicePalacePachinko.Entity
{
	[SerializeField]
	private Animator fire;
	[SerializeField]
	private Transform[] lights;
	[SerializeField]
	private Sprite[] beamSprites;
	[SerializeField]
	private GameObject beam;
}
