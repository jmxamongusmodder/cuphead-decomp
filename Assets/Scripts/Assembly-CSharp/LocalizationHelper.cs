using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LocalizationHelper : MonoBehaviour
{
	public bool existingKey;
	public int currentID;
	public Localization.Languages currentLanguage;
	public Localization.Categories currentCategory;
	public bool currentCustomFont;
	public Text textComponent;
	public Image imageComponent;
	public SpriteRenderer spriteRendererComponent;
	public TMP_Text textMeshProComponent;
}
