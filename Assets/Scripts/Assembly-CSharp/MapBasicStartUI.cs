using UnityEngine;
using TMPro;

public class MapBasicStartUI : AbstractMapSceneStartUI
{
	public Animator Animator;
	public TMP_Text Title;
	[SerializeField]
	private RectTransform cursor;
	[SerializeField]
	private RectTransform enter;
}
