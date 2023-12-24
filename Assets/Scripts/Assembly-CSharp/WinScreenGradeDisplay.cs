using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class WinScreenGradeDisplay : AbstractMonoBehaviour
{
	[SerializeField]
	private Text text;
	[SerializeField]
	private TextMeshProUGUI topGradeLabel;
	[SerializeField]
	private Text topGradeValue;
	[SerializeField]
	private string[] grades;
	[SerializeField]
	private Animator circle;
	[SerializeField]
	private Animator recordBanner;
	[SerializeField]
	private GameObject[] recordEnglish;
	[SerializeField]
	private GameObject[] recordOther;
	[SerializeField]
	private Image recordBannerEnglish;
	[SerializeField]
	private Image recordBannerOther;
	[SerializeField]
	private Animator gollyBanner;
	[SerializeField]
	private GameObject[] gollyEnglish;
	[SerializeField]
	private GameObject[] gollyOther;
	[SerializeField]
	private Image gollyBannerEnglish;
	[SerializeField]
	private Image gollyBannerOther;
	[SerializeField]
	private SpriteRenderer tryRegular;
	[SerializeField]
	private SpriteRenderer tryExpert;
	[SerializeField]
	private GameObject[] normalBannerTexts;
	[SerializeField]
	private GameObject[] topScoreBannerTexts;
}
