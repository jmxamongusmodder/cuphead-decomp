using System;
using TMPro;
using UnityEngine;

// Token: 0x02000999 RID: 2457
public class MapBasicStartUI : AbstractMapSceneStartUI
{
	// Token: 0x170004A9 RID: 1193
	// (get) Token: 0x06003974 RID: 14708 RVA: 0x00209EBB File Offset: 0x002082BB
	// (set) Token: 0x06003975 RID: 14709 RVA: 0x00209EC2 File Offset: 0x002082C2
	public static MapBasicStartUI Current { get; private set; }

	// Token: 0x06003976 RID: 14710 RVA: 0x00209ECA File Offset: 0x002082CA
	protected override void Awake()
	{
		base.Awake();
		MapBasicStartUI.Current = this;
	}

	// Token: 0x06003977 RID: 14711 RVA: 0x00209ED8 File Offset: 0x002082D8
	private void OnDestroy()
	{
		if (MapBasicStartUI.Current == this)
		{
			MapBasicStartUI.Current = null;
		}
	}

	// Token: 0x06003978 RID: 14712 RVA: 0x00209EF0 File Offset: 0x002082F0
	private void UpdateCursor()
	{
		this.cursor.transform.position = this.enter.transform.position;
		this.cursor.sizeDelta = new Vector2(this.enter.sizeDelta.x + 30f, this.enter.sizeDelta.y + 20f);
	}

	// Token: 0x06003979 RID: 14713 RVA: 0x00209F5F File Offset: 0x0020835F
	private void Update()
	{
		this.UpdateCursor();
		if (base.CurrentState == AbstractMapSceneStartUI.State.Active)
		{
			this.CheckInput();
		}
	}

	// Token: 0x0600397A RID: 14714 RVA: 0x00209F79 File Offset: 0x00208379
	private void CheckInput()
	{
		if (!base.Able)
		{
			return;
		}
		if (base.GetButtonDown(CupheadButton.Cancel))
		{
			base.Out();
		}
		if (base.GetButtonDown(CupheadButton.Accept))
		{
			base.LoadLevel();
		}
	}

	// Token: 0x0600397B RID: 14715 RVA: 0x00209FAD File Offset: 0x002083AD
	public new void In(MapPlayerController playerController)
	{
		base.In(playerController);
		if (this.Animator != null)
		{
			this.Animator.SetTrigger("ZoomIn");
			AudioManager.Play("world_map_level_menu_open");
		}
		this.InitUI(this.level);
	}

	// Token: 0x0600397C RID: 14716 RVA: 0x00209FF0 File Offset: 0x002083F0
	public void InitUI(string level)
	{
		TranslationElement translationElement = Localization.Find(level);
		if (translationElement != null)
		{
			this.Title.GetComponent<LocalizationHelper>().ApplyTranslation(translationElement, null);
			if (Localization.language == Localization.Languages.Japanese)
			{
				this.Title.lineSpacing = 0f;
			}
			else
			{
				this.Title.lineSpacing = 17.46f;
			}
		}
	}

	// Token: 0x0400411A RID: 16666
	public Animator Animator;

	// Token: 0x0400411B RID: 16667
	public TMP_Text Title;

	// Token: 0x0400411C RID: 16668
	[SerializeField]
	private RectTransform cursor;

	// Token: 0x0400411D RID: 16669
	[Header("Options")]
	[SerializeField]
	private RectTransform enter;
}
