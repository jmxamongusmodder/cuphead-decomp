using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro
{
	// Token: 0x02000C93 RID: 3219
	public class TMP_UpdateRegistry
	{
		// Token: 0x0600514E RID: 20814 RVA: 0x00298C6C File Offset: 0x0029706C
		protected TMP_UpdateRegistry()
		{
			Canvas.willRenderCanvases += this.PerformUpdateForCanvasRendererObjects;
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x0600514F RID: 20815 RVA: 0x00298CBC File Offset: 0x002970BC
		public static TMP_UpdateRegistry instance
		{
			get
			{
				if (TMP_UpdateRegistry.s_Instance == null)
				{
					TMP_UpdateRegistry.s_Instance = new TMP_UpdateRegistry();
				}
				return TMP_UpdateRegistry.s_Instance;
			}
		}

		// Token: 0x06005150 RID: 20816 RVA: 0x00298CD7 File Offset: 0x002970D7
		public static void RegisterCanvasElementForLayoutRebuild(ICanvasElement element)
		{
			TMP_UpdateRegistry.instance.InternalRegisterCanvasElementForLayoutRebuild(element);
		}

		// Token: 0x06005151 RID: 20817 RVA: 0x00298CE8 File Offset: 0x002970E8
		private bool InternalRegisterCanvasElementForLayoutRebuild(ICanvasElement element)
		{
			int instanceID = (element as UnityEngine.Object).GetInstanceID();
			if (this.m_LayoutQueueLookup.ContainsKey(instanceID))
			{
				return false;
			}
			this.m_LayoutQueueLookup[instanceID] = instanceID;
			this.m_LayoutRebuildQueue.Add(element);
			return true;
		}

		// Token: 0x06005152 RID: 20818 RVA: 0x00298D2E File Offset: 0x0029712E
		public static void RegisterCanvasElementForGraphicRebuild(ICanvasElement element)
		{
			TMP_UpdateRegistry.instance.InternalRegisterCanvasElementForGraphicRebuild(element);
		}

		// Token: 0x06005153 RID: 20819 RVA: 0x00298D3C File Offset: 0x0029713C
		private bool InternalRegisterCanvasElementForGraphicRebuild(ICanvasElement element)
		{
			int instanceID = (element as UnityEngine.Object).GetInstanceID();
			if (this.m_GraphicQueueLookup.ContainsKey(instanceID))
			{
				return false;
			}
			this.m_GraphicQueueLookup[instanceID] = instanceID;
			this.m_GraphicRebuildQueue.Add(element);
			return true;
		}

		// Token: 0x06005154 RID: 20820 RVA: 0x00298D84 File Offset: 0x00297184
		private void PerformUpdateForCanvasRendererObjects()
		{
			for (int i = 0; i < this.m_LayoutRebuildQueue.Count; i++)
			{
				ICanvasElement canvasElement = TMP_UpdateRegistry.instance.m_LayoutRebuildQueue[i];
				canvasElement.Rebuild(CanvasUpdate.Prelayout);
			}
			if (this.m_LayoutRebuildQueue.Count > 0)
			{
				this.m_LayoutRebuildQueue.Clear();
				this.m_LayoutQueueLookup.Clear();
			}
			for (int j = 0; j < this.m_GraphicRebuildQueue.Count; j++)
			{
				ICanvasElement canvasElement2 = TMP_UpdateRegistry.instance.m_GraphicRebuildQueue[j];
				canvasElement2.Rebuild(CanvasUpdate.PreRender);
			}
			if (this.m_GraphicRebuildQueue.Count > 0)
			{
				this.m_GraphicRebuildQueue.Clear();
				this.m_GraphicQueueLookup.Clear();
			}
		}

		// Token: 0x06005155 RID: 20821 RVA: 0x00298E47 File Offset: 0x00297247
		private void PerformUpdateForMeshRendererObjects()
		{
		}

		// Token: 0x06005156 RID: 20822 RVA: 0x00298E49 File Offset: 0x00297249
		public static void UnRegisterCanvasElementForRebuild(ICanvasElement element)
		{
			TMP_UpdateRegistry.instance.InternalUnRegisterCanvasElementForLayoutRebuild(element);
			TMP_UpdateRegistry.instance.InternalUnRegisterCanvasElementForGraphicRebuild(element);
		}

		// Token: 0x06005157 RID: 20823 RVA: 0x00298E64 File Offset: 0x00297264
		private void InternalUnRegisterCanvasElementForLayoutRebuild(ICanvasElement element)
		{
			int instanceID = (element as UnityEngine.Object).GetInstanceID();
			TMP_UpdateRegistry.instance.m_LayoutRebuildQueue.Remove(element);
			this.m_GraphicQueueLookup.Remove(instanceID);
		}

		// Token: 0x06005158 RID: 20824 RVA: 0x00298E9C File Offset: 0x0029729C
		private void InternalUnRegisterCanvasElementForGraphicRebuild(ICanvasElement element)
		{
			int instanceID = (element as UnityEngine.Object).GetInstanceID();
			TMP_UpdateRegistry.instance.m_GraphicRebuildQueue.Remove(element);
			this.m_LayoutQueueLookup.Remove(instanceID);
		}

		// Token: 0x040053F3 RID: 21491
		private static TMP_UpdateRegistry s_Instance;

		// Token: 0x040053F4 RID: 21492
		private readonly List<ICanvasElement> m_LayoutRebuildQueue = new List<ICanvasElement>();

		// Token: 0x040053F5 RID: 21493
		private Dictionary<int, int> m_LayoutQueueLookup = new Dictionary<int, int>();

		// Token: 0x040053F6 RID: 21494
		private readonly List<ICanvasElement> m_GraphicRebuildQueue = new List<ICanvasElement>();

		// Token: 0x040053F7 RID: 21495
		private Dictionary<int, int> m_GraphicQueueLookup = new Dictionary<int, int>();
	}
}
