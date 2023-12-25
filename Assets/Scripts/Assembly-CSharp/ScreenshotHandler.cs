using System;
using UnityEngine;

// Token: 0x02000420 RID: 1056
public class ScreenshotHandler : MonoBehaviour
{
	// Token: 0x06000F52 RID: 3922 RVA: 0x0009725A File Offset: 0x0009565A
	private void Awake()
	{
		ScreenshotHandler.instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x06000F53 RID: 3923 RVA: 0x00097273 File Offset: 0x00095673
	public static void TakeScreenshot_Static(ScreenshotHandler.cameraType _camera, string _folderName, string _fileName)
	{
	}

	// Token: 0x04001855 RID: 6229
	private static ScreenshotHandler instance;

	// Token: 0x04001856 RID: 6230
	private bool takeScreenshotNextFrame;

	// Token: 0x04001857 RID: 6231
	private Camera myCamera;

	// Token: 0x04001858 RID: 6232
	private string fileName;

	// Token: 0x04001859 RID: 6233
	private string folderName;

	// Token: 0x0400185A RID: 6234
	private ScreenshotHandler.cameraType currentCameraType;

	// Token: 0x02000421 RID: 1057
	public enum cameraType
	{
		// Token: 0x0400185C RID: 6236
		Map,
		// Token: 0x0400185D RID: 6237
		UI,
		// Token: 0x0400185E RID: 6238
		Level
	}
}
