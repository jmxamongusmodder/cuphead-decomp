using System;
using UnityEngine;

// Token: 0x020009A3 RID: 2467
public class MapUIInteractionDialogue : AbstractUIInteractionDialogue
{
	// Token: 0x060039E2 RID: 14818 RVA: 0x0020EEE0 File Offset: 0x0020D2E0
	public static MapUIInteractionDialogue Create(AbstractUIInteractionDialogue.Properties properties, PlayerInput player, Vector2 offset)
	{
		MapUIInteractionDialogue mapUIInteractionDialogue = UnityEngine.Object.Instantiate<MapUIInteractionDialogue>(Map.Current.MapResources.mapUIInteractionDialogue);
		properties.text = string.Empty;
		mapUIInteractionDialogue.Init(properties, player, offset);
		return mapUIInteractionDialogue;
	}

	// Token: 0x170004B3 RID: 1203
	// (get) Token: 0x060039E3 RID: 14819 RVA: 0x0020EF17 File Offset: 0x0020D317
	protected override float PreferredWidth
	{
		get
		{
			return this.tmpText.preferredWidth + this.glyph.preferredWidth + 10f;
		}
	}

	// Token: 0x060039E4 RID: 14820 RVA: 0x0020EF36 File Offset: 0x0020D336
	protected override void Awake()
	{
		base.Awake();
		base.transform.SetParent(MapUI.Current.sceneCanvas.transform);
		base.transform.ResetLocalTransforms();
	}

	// Token: 0x060039E5 RID: 14821 RVA: 0x0020EF63 File Offset: 0x0020D363
	private void Update()
	{
		this.UpdatePos();
	}

	// Token: 0x060039E6 RID: 14822 RVA: 0x0020EF6C File Offset: 0x0020D36C
	private void UpdatePos()
	{
		if (this.target == null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Vector2 v = this.target.position + this.dialogueOffset;
		base.transform.position = v;
	}

	// Token: 0x040041D6 RID: 16854
	private const float OFFSET_GLYPH = 10f;
}
