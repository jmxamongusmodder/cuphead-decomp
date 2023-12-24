using UnityEngine;
using UnityEngine.UI;

public class CupheadGlyph : MonoBehaviour
{
	public enum LetterOffset
	{
		Normal = 0,
		Small = 1,
	}

	public enum PlatformGlyphType
	{
		Normal = 0,
		TutorialInstruction = 1,
		TutorialInstructionDescend = 2,
		LevelUIInteractionDialogue = 3,
		Shop = 4,
		SwitchWeapon = 5,
		ShmupTutorial = 6,
		Equip = 7,
		OffsetPrompt = 8,
	}

	public int rewiredPlayerId;
	public CupheadButton button;
	[SerializeField]
	private Image glyphSymbolText;
	[SerializeField]
	private Text glyphText;
	[SerializeField]
	private Image glyphSymbolChar;
	[SerializeField]
	private Text glyphChar;
	[SerializeField]
	private RectTransform[] rectTransformTexts;
	[SerializeField]
	protected Vector2 startSize;
	[SerializeField]
	protected float paddingText;
	[SerializeField]
	private float maxSize;
	[SerializeField]
	private LetterOffset letterOffset;
	[SerializeField]
	private PlatformGlyphType platformGlyphType;
	[SerializeField]
	private CustomLanguageLayout[] glyphLayouts;
}
