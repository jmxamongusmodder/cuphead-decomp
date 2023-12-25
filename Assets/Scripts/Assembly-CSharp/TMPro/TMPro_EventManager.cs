using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000C96 RID: 3222
	public static class TMPro_EventManager
	{
		// Token: 0x0600515F RID: 20831 RVA: 0x00298F9B File Offset: 0x0029739B
		public static void ON_PRE_RENDER_OBJECT_CHANGED()
		{
			TMPro_EventManager.OnPreRenderObject_Event.Call();
		}

		// Token: 0x06005160 RID: 20832 RVA: 0x00298FA7 File Offset: 0x002973A7
		public static void ON_MATERIAL_PROPERTY_CHANGED(bool isChanged, Material mat)
		{
			TMPro_EventManager.MATERIAL_PROPERTY_EVENT.Call(isChanged, mat);
		}

		// Token: 0x06005161 RID: 20833 RVA: 0x00298FB5 File Offset: 0x002973B5
		public static void ON_FONT_PROPERTY_CHANGED(bool isChanged, TMP_FontAsset font)
		{
			TMPro_EventManager.FONT_PROPERTY_EVENT.Call(isChanged, font);
		}

		// Token: 0x06005162 RID: 20834 RVA: 0x00298FC3 File Offset: 0x002973C3
		public static void ON_SPRITE_ASSET_PROPERTY_CHANGED(bool isChanged, UnityEngine.Object obj)
		{
			TMPro_EventManager.SPRITE_ASSET_PROPERTY_EVENT.Call(isChanged, obj);
		}

		// Token: 0x06005163 RID: 20835 RVA: 0x00298FD1 File Offset: 0x002973D1
		public static void ON_TEXTMESHPRO_PROPERTY_CHANGED(bool isChanged, TextMeshPro obj)
		{
			TMPro_EventManager.TEXTMESHPRO_PROPERTY_EVENT.Call(isChanged, obj);
		}

		// Token: 0x06005164 RID: 20836 RVA: 0x00298FDF File Offset: 0x002973DF
		public static void ON_DRAG_AND_DROP_MATERIAL_CHANGED(GameObject sender, Material currentMaterial, Material newMaterial)
		{
			TMPro_EventManager.DRAG_AND_DROP_MATERIAL_EVENT.Call(sender, currentMaterial, newMaterial);
		}

		// Token: 0x06005165 RID: 20837 RVA: 0x00298FEE File Offset: 0x002973EE
		public static void ON_TEXT_STYLE_PROPERTY_CHANGED(bool isChanged)
		{
			TMPro_EventManager.TEXT_STYLE_PROPERTY_EVENT.Call(isChanged);
		}

		// Token: 0x06005166 RID: 20838 RVA: 0x00298FFB File Offset: 0x002973FB
		public static void ON_TEXT_CHANGED(UnityEngine.Object obj)
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Call(obj);
		}

		// Token: 0x06005167 RID: 20839 RVA: 0x00299008 File Offset: 0x00297408
		public static void ON_TMP_SETTINGS_CHANGED()
		{
			TMPro_EventManager.TMP_SETTINGS_PROPERTY_EVENT.Call();
		}

		// Token: 0x06005168 RID: 20840 RVA: 0x00299014 File Offset: 0x00297414
		public static void ON_TEXTMESHPRO_UGUI_PROPERTY_CHANGED(bool isChanged, TextMeshProUGUI obj)
		{
			TMPro_EventManager.TEXTMESHPRO_UGUI_PROPERTY_EVENT.Call(isChanged, obj);
		}

		// Token: 0x06005169 RID: 20841 RVA: 0x00299022 File Offset: 0x00297422
		public static void ON_BASE_MATERIAL_CHANGED(Material mat)
		{
			TMPro_EventManager.BASE_MATERIAL_EVENT.Call(mat);
		}

		// Token: 0x0600516A RID: 20842 RVA: 0x0029902F File Offset: 0x0029742F
		public static void ON_COMPUTE_DT_EVENT(object Sender, Compute_DT_EventArgs e)
		{
			TMPro_EventManager.COMPUTE_DT_EVENT.Call(Sender, e);
		}

		// Token: 0x040053FD RID: 21501
		public static readonly FastAction<object, Compute_DT_EventArgs> COMPUTE_DT_EVENT = new FastAction<object, Compute_DT_EventArgs>();

		// Token: 0x040053FE RID: 21502
		public static readonly FastAction<bool, Material> MATERIAL_PROPERTY_EVENT = new FastAction<bool, Material>();

		// Token: 0x040053FF RID: 21503
		public static readonly FastAction<bool, TMP_FontAsset> FONT_PROPERTY_EVENT = new FastAction<bool, TMP_FontAsset>();

		// Token: 0x04005400 RID: 21504
		public static readonly FastAction<bool, UnityEngine.Object> SPRITE_ASSET_PROPERTY_EVENT = new FastAction<bool, UnityEngine.Object>();

		// Token: 0x04005401 RID: 21505
		public static readonly FastAction<bool, TextMeshPro> TEXTMESHPRO_PROPERTY_EVENT = new FastAction<bool, TextMeshPro>();

		// Token: 0x04005402 RID: 21506
		public static readonly FastAction<GameObject, Material, Material> DRAG_AND_DROP_MATERIAL_EVENT = new FastAction<GameObject, Material, Material>();

		// Token: 0x04005403 RID: 21507
		public static readonly FastAction<bool> TEXT_STYLE_PROPERTY_EVENT = new FastAction<bool>();

		// Token: 0x04005404 RID: 21508
		public static readonly FastAction TMP_SETTINGS_PROPERTY_EVENT = new FastAction();

		// Token: 0x04005405 RID: 21509
		public static readonly FastAction<bool, TextMeshProUGUI> TEXTMESHPRO_UGUI_PROPERTY_EVENT = new FastAction<bool, TextMeshProUGUI>();

		// Token: 0x04005406 RID: 21510
		public static readonly FastAction<Material> BASE_MATERIAL_EVENT = new FastAction<Material>();

		// Token: 0x04005407 RID: 21511
		public static readonly FastAction OnPreRenderObject_Event = new FastAction();

		// Token: 0x04005408 RID: 21512
		public static readonly FastAction<UnityEngine.Object> TEXT_CHANGED_EVENT = new FastAction<UnityEngine.Object>();

		// Token: 0x04005409 RID: 21513
		public static readonly FastAction WILL_RENDER_CANVASES = new FastAction();
	}
}
