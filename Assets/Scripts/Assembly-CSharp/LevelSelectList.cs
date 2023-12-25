using System;
using System.Collections.Generic;
using RektTransform;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009A5 RID: 2469
public class LevelSelectList : AbstractMonoBehaviour
{
	// Token: 0x060039F2 RID: 14834 RVA: 0x0020F188 File Offset: 0x0020D588
	protected override void Awake()
	{
		base.Awake();
		this.SetupList();
	}

	// Token: 0x060039F3 RID: 14835 RVA: 0x0020F198 File Offset: 0x0020D598
	private void SetupList()
	{
		List<Scenes> list = new List<Scenes>();
		foreach (Scenes scenes in EnumUtils.GetValues<Scenes>())
		{
			if (this.GetSceneGroup(scenes).included)
			{
				list.Add(scenes);
			}
		}
		int num = 0;
		foreach (Scenes scenes2 in list)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.button.gameObject);
			Button b = gameObject.GetComponent<Button>();
			string text = scenes2.ToString().Replace("scene_", string.Empty).Replace("level_", string.Empty).Replace("dice_palace_", string.Empty).Replace("platforming_", string.Empty);
			b.name = scenes2.ToString();
			gameObject.GetComponentInChildren<Text>().text = text;
			b.onClick.AddListener(delegate()
			{
				SceneLoader.LoadScene(b.name, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
			});
			b.transform.SetParent(this.button.transform.parent);
			b.transform.ResetLocalTransforms();
			num++;
		}
		this.button.gameObject.SetActive(false);
		this.contentPanel.SetHeight(30f * (float)num);
	}

	// Token: 0x060039F4 RID: 14836 RVA: 0x0020F34C File Offset: 0x0020D74C
	public LevelSelectList.SceneGroup GetSceneGroup(Scenes s)
	{
		foreach (LevelSelectList.SceneGroup sceneGroup in this.scenes)
		{
			if (sceneGroup.scene == s)
			{
				return sceneGroup;
			}
		}
		return new LevelSelectList.SceneGroup();
	}

	// Token: 0x060039F5 RID: 14837 RVA: 0x0020F38C File Offset: 0x0020D78C
	public bool ContainsScene(Scenes s)
	{
		foreach (LevelSelectList.SceneGroup sceneGroup in this.scenes)
		{
			if (sceneGroup.scene == s)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x040041DA RID: 16858
	[HideInInspector]
	public LevelSelectList.SceneGroup[] scenes;

	// Token: 0x040041DB RID: 16859
	public Button button;

	// Token: 0x040041DC RID: 16860
	public RectTransform contentPanel;

	// Token: 0x020009A6 RID: 2470
	[Serializable]
	public class SceneGroup
	{
		// Token: 0x040041DD RID: 16861
		public bool included;

		// Token: 0x040041DE RID: 16862
		public Scenes scene;
	}
}
