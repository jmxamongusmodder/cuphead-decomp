using UnityEngine.UI;
using UnityEngine;

public class UserProfileDisplay : AbstractMonoBehaviour
{
	[SerializeField]
	private Image gamerPic;
	[SerializeField]
	private Text gamerTag;
	[SerializeField]
	private PlayerId player;
	[SerializeField]
	private GameObject root;
	[SerializeField]
	private GameObject switchPromptRoot;
	[SerializeField]
	private bool showForMultipleUsersUnsupported;
	[SerializeField]
	private Sprite defaultAvatarCuphead;
	[SerializeField]
	private Sprite defaultAvatarMugman;
}
