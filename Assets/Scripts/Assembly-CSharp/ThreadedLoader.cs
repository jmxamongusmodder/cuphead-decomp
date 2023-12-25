using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

// Token: 0x020003B4 RID: 948
public class ThreadedLoader
{
	// Token: 0x06000BB8 RID: 3000 RVA: 0x00084945 File Offset: 0x00082D45
	public ThreadedLoader(MonoBehaviour coroutineParent)
	{
		this.coroutineParent = coroutineParent;
	}

	// Token: 0x06000BB9 RID: 3001 RVA: 0x00084960 File Offset: 0x00082D60
	public ThreadedLoader.LoadOperation LoadAssetBundle(string path)
	{
		ThreadedLoader.LoadOperation loadOperation = new ThreadedLoader.LoadOperation();
		loadOperation.path = path;
		if (this.busy)
		{
			this.operationQueue.Add(loadOperation);
			return loadOperation;
		}
		this.startLoad(loadOperation);
		return loadOperation;
	}

	// Token: 0x06000BBA RID: 3002 RVA: 0x0008499C File Offset: 0x00082D9C
	private void startLoad(ThreadedLoader.LoadOperation operation)
	{
		this.busy = true;
		this.threadBusy = true;
		this.coroutineParent.StartCoroutine(this.threadWait_cr(operation));
		Thread thread = new Thread(delegate()
		{
			this.loadData(operation);
		});
		thread.Start();
	}

	// Token: 0x06000BBB RID: 3003 RVA: 0x000849FC File Offset: 0x00082DFC
	private IEnumerator threadWait_cr(ThreadedLoader.LoadOperation operation)
	{
		while (this.threadBusy)
		{
			yield return null;
		}
		AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(operation.data);
		yield return request;
		operation.SetComplete(request.assetBundle);
		this.busy = false;
		if (this.operationQueue.Count > 0)
		{
			ThreadedLoader.LoadOperation operation2 = this.operationQueue[0];
			this.operationQueue.RemoveAt(0);
			this.startLoad(operation2);
		}
		yield break;
	}

	// Token: 0x06000BBC RID: 3004 RVA: 0x00084A1E File Offset: 0x00082E1E
	private void loadData(ThreadedLoader.LoadOperation operation)
	{
		operation.data = File.ReadAllBytes(operation.path);
		this.threadBusy = false;
	}

	// Token: 0x0400156B RID: 5483
	private MonoBehaviour coroutineParent;

	// Token: 0x0400156C RID: 5484
	private List<ThreadedLoader.LoadOperation> operationQueue = new List<ThreadedLoader.LoadOperation>();

	// Token: 0x0400156D RID: 5485
	private bool busy;

	// Token: 0x0400156E RID: 5486
	private bool threadBusy;

	// Token: 0x020003B5 RID: 949
	public class LoadOperation : DLCManager.AssetBundleLoadWaitInstruction
	{
		// Token: 0x06000BBE RID: 3006 RVA: 0x00084A69 File Offset: 0x00082E69
		public void SetComplete(AssetBundle bundle)
		{
			this.complete = true;
			base.assetBundle = bundle;
		}

		// Token: 0x0400156F RID: 5487
		public string path;

		// Token: 0x04001570 RID: 5488
		public byte[] data;
	}
}
