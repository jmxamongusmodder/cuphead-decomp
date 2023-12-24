using UnityEngine;
using UnityEngine.UI;

public class DLCGUI : AbstractMonoBehaviour
{
	[SerializeField]
	private GameObject notInstalled;
	[SerializeField]
	private GameObject installed;
	[SerializeField]
	private Transform notInstalledScaler;
	[SerializeField]
	private Transform installedScaler;
	[SerializeField]
	private Image fader;
	[SerializeField]
	private Text notInstalledText;
	[SerializeField]
	private Text installedText;
}
