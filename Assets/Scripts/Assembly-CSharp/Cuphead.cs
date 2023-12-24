using UnityEngine;
using Rewired;
using Rewired.UI.ControlMapper;

public class Cuphead : AbstractMonoBehaviour
{
	[SerializeField]
	private AudioNoiseHandler noiseHandler;
	[SerializeField]
	private InputManager rewired;
	public ControlMapper controlMapper;
	[SerializeField]
	private CupheadEventSystem eventSystem;
	[SerializeField]
	private CupheadRenderer renderer;
	[SerializeField]
	private ScoringEditorData scoringProperties;
	[SerializeField]
	private AchievementToastManager achievementToastManagerPrefab;
}
