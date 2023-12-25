using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x02000B3F RID: 2879
public class OnScreenConsole : MonoBehaviour
{
	// Token: 0x060045B5 RID: 17845 RVA: 0x0024C452 File Offset: 0x0024A852
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.logger = new OnScreenConsole.OnScreenConsoleLogger();
	}

	// Token: 0x060045B6 RID: 17846 RVA: 0x0024C46C File Offset: 0x0024A86C
	private void OnGUI()
	{
		if (this.style == null)
		{
			this.style = new GUIStyle(GUI.skin.GetStyle("Box"));
			this.style.alignment = TextAnchor.LowerLeft;
			this.style.wordWrap = true;
		}
		foreach (string text in this.logger.logQueue)
		{
			string value = text;
			if (text.Length > OnScreenConsole.MaximumStringLength)
			{
				value = text.Substring(0, OnScreenConsole.MaximumStringLength);
			}
			this.builder.AppendLine(value);
		}
		if (this.builder.Length > 0)
		{
			this.builder.Length--;
		}
		int num = (int)(OnScreenConsole.Size.x * (float)Screen.width);
		int num2 = (int)(OnScreenConsole.Size.y * (float)Screen.height);
		GUI.Box(new Rect((float)(Screen.width - num), (float)(Screen.height - num2), (float)num, (float)num2), this.builder.ToString(), this.style);
		this.builder.Length = 0;
	}

	// Token: 0x04004BDC RID: 19420
	private static readonly Vector2 Size = new Vector2(0.5f, 0.4f);

	// Token: 0x04004BDD RID: 19421
	private static readonly int MaximumStringLength = 500;

	// Token: 0x04004BDE RID: 19422
	private OnScreenConsole.OnScreenConsoleLogger logger;

	// Token: 0x04004BDF RID: 19423
	private GUIStyle style;

	// Token: 0x04004BE0 RID: 19424
	private StringBuilder builder = new StringBuilder();

	// Token: 0x02000B40 RID: 2880
	private class OnScreenConsoleLogger : ILogHandler
	{
		// Token: 0x060045B8 RID: 17848 RVA: 0x0024C5E0 File Offset: 0x0024A9E0
		public OnScreenConsoleLogger()
		{
			UnityEngine.Debug.unityLogger.logHandler = this;
		}

		// Token: 0x060045B9 RID: 17849 RVA: 0x0024C613 File Offset: 0x0024AA13
		public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
		{
			this.addToQueue(string.Format("[{0}, {1}] {2}", logType, Time.frameCount, string.Format(format, args)));
			this.defaultLogHandler.LogFormat(logType, context, format, args);
		}

		// Token: 0x060045BA RID: 17850 RVA: 0x0024C64D File Offset: 0x0024AA4D
		public void LogException(Exception exception, UnityEngine.Object context)
		{
			this.defaultLogHandler.LogException(exception, context);
		}

		// Token: 0x060045BB RID: 17851 RVA: 0x0024C65C File Offset: 0x0024AA5C
		private void addToQueue(string value)
		{
			if (this.logQueue.Count == OnScreenConsole.OnScreenConsoleLogger.QueueSize)
			{
				this.logQueue.Dequeue();
			}
			this.logQueue.Enqueue(value);
		}

		// Token: 0x04004BE1 RID: 19425
		private static readonly int QueueSize = 15;

		// Token: 0x04004BE2 RID: 19426
		public Queue<string> logQueue = new Queue<string>(OnScreenConsole.OnScreenConsoleLogger.QueueSize);

		// Token: 0x04004BE3 RID: 19427
		private ILogHandler defaultLogHandler = UnityEngine.Debug.unityLogger.logHandler;
	}
}
