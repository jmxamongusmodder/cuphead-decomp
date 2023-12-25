using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro
{
	// Token: 0x02000C92 RID: 3218
	public class TMP_UpdateManager
	{
		// Token: 0x06005144 RID: 20804 RVA: 0x00298A18 File Offset: 0x00296E18
		protected TMP_UpdateManager()
		{
			Camera.onPreRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPreRender, new Camera.CameraCallback(this.OnCameraPreRender));
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x06005145 RID: 20805 RVA: 0x00298A77 File Offset: 0x00296E77
		public static TMP_UpdateManager instance
		{
			get
			{
				if (TMP_UpdateManager.s_Instance == null)
				{
					TMP_UpdateManager.s_Instance = new TMP_UpdateManager();
				}
				return TMP_UpdateManager.s_Instance;
			}
		}

		// Token: 0x06005146 RID: 20806 RVA: 0x00298A92 File Offset: 0x00296E92
		public static void RegisterTextElementForLayoutRebuild(TMP_Text element)
		{
			TMP_UpdateManager.instance.InternalRegisterTextElementForLayoutRebuild(element);
		}

		// Token: 0x06005147 RID: 20807 RVA: 0x00298AA0 File Offset: 0x00296EA0
		private bool InternalRegisterTextElementForLayoutRebuild(TMP_Text element)
		{
			int instanceID = element.GetInstanceID();
			if (this.m_LayoutQueueLookup.ContainsKey(instanceID))
			{
				return false;
			}
			this.m_LayoutQueueLookup[instanceID] = instanceID;
			this.m_LayoutRebuildQueue.Add(element);
			return true;
		}

		// Token: 0x06005148 RID: 20808 RVA: 0x00298AE1 File Offset: 0x00296EE1
		public static void RegisterTextElementForGraphicRebuild(TMP_Text element)
		{
			TMP_UpdateManager.instance.InternalRegisterTextElementForGraphicRebuild(element);
		}

		// Token: 0x06005149 RID: 20809 RVA: 0x00298AF0 File Offset: 0x00296EF0
		private bool InternalRegisterTextElementForGraphicRebuild(TMP_Text element)
		{
			int instanceID = element.GetInstanceID();
			if (this.m_GraphicQueueLookup.ContainsKey(instanceID))
			{
				return false;
			}
			this.m_GraphicQueueLookup[instanceID] = instanceID;
			this.m_GraphicRebuildQueue.Add(element);
			return true;
		}

		// Token: 0x0600514A RID: 20810 RVA: 0x00298B34 File Offset: 0x00296F34
		private void OnCameraPreRender(Camera cam)
		{
			for (int i = 0; i < this.m_LayoutRebuildQueue.Count; i++)
			{
				this.m_LayoutRebuildQueue[i].Rebuild(CanvasUpdate.Prelayout);
			}
			if (this.m_LayoutRebuildQueue.Count > 0)
			{
				this.m_LayoutRebuildQueue.Clear();
				this.m_LayoutQueueLookup.Clear();
			}
			for (int j = 0; j < this.m_GraphicRebuildQueue.Count; j++)
			{
				this.m_GraphicRebuildQueue[j].Rebuild(CanvasUpdate.PreRender);
			}
			if (this.m_GraphicRebuildQueue.Count > 0)
			{
				this.m_GraphicRebuildQueue.Clear();
				this.m_GraphicQueueLookup.Clear();
			}
		}

		// Token: 0x0600514B RID: 20811 RVA: 0x00298BEB File Offset: 0x00296FEB
		public static void UnRegisterTextElementForRebuild(TMP_Text element)
		{
			TMP_UpdateManager.instance.InternalUnRegisterTextElementForGraphicRebuild(element);
			TMP_UpdateManager.instance.InternalUnRegisterTextElementForLayoutRebuild(element);
		}

		// Token: 0x0600514C RID: 20812 RVA: 0x00298C04 File Offset: 0x00297004
		private void InternalUnRegisterTextElementForGraphicRebuild(TMP_Text element)
		{
			int instanceID = element.GetInstanceID();
			TMP_UpdateManager.instance.m_GraphicRebuildQueue.Remove(element);
			this.m_GraphicQueueLookup.Remove(instanceID);
		}

		// Token: 0x0600514D RID: 20813 RVA: 0x00298C38 File Offset: 0x00297038
		private void InternalUnRegisterTextElementForLayoutRebuild(TMP_Text element)
		{
			int instanceID = element.GetInstanceID();
			TMP_UpdateManager.instance.m_LayoutRebuildQueue.Remove(element);
			this.m_LayoutQueueLookup.Remove(instanceID);
		}

		// Token: 0x040053EE RID: 21486
		private static TMP_UpdateManager s_Instance;

		// Token: 0x040053EF RID: 21487
		private readonly List<TMP_Text> m_LayoutRebuildQueue = new List<TMP_Text>();

		// Token: 0x040053F0 RID: 21488
		private Dictionary<int, int> m_LayoutQueueLookup = new Dictionary<int, int>();

		// Token: 0x040053F1 RID: 21489
		private readonly List<TMP_Text> m_GraphicRebuildQueue = new List<TMP_Text>();

		// Token: 0x040053F2 RID: 21490
		private Dictionary<int, int> m_GraphicQueueLookup = new Dictionary<int, int>();
	}
}
