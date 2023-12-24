using UnityEngine;

public class DicePalacePachinkoLevel : AbstractDicePalaceLevel
{
	[SerializeField]
	private Transform[] starDiscs;
	[SerializeField]
	private DicePalacePachinkoLevelPipes pipes;
	[SerializeField]
	private DicePalacePachinkoLevelPachinko pachinko;
	[SerializeField]
	private Sprite _bossPortrait;
	[SerializeField]
	private string _bossQuote;
}
