using System;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;

// Token: 0x02000B3B RID: 2875
public class DEBUG_HeapPrinter : MonoBehaviour
{
	// Token: 0x060045A6 RID: 17830 RVA: 0x0024BE80 File Offset: 0x0024A280
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x060045A7 RID: 17831 RVA: 0x0024BE90 File Offset: 0x0024A290
	private void OnGUI()
	{
		if (!this.styleInitialized)
		{
			this.styleInitialized = true;
			this.style = new GUIStyle(GUI.skin.box);
			this.style.alignment = TextAnchor.MiddleRight;
			this.style.fontStyle = FontStyle.Bold;
			this.style.richText = true;
			this.style.fontSize = 24;
		}
		long totalMemory = GC.GetTotalMemory(false);
		long value = totalMemory - this.previousMemory;
		this.counter.Add(value);
		if (this.previousMemory > totalMemory)
		{
			this.highlightTimer = 0f;
		}
		float num = DEBUG_HeapPrinter.Size.x;
		float num2 = DEBUG_HeapPrinter.Size.y;
		string value2 = string.Empty;
		if (this.highlightTimer < DEBUG_HeapPrinter.HighlightTime)
		{
			this.highlightTimer += Time.unscaledDeltaTime;
			this.style.fontSize = DEBUG_HeapPrinter.LargeFontSize;
			this.builder.Append("<color=red>");
			value2 = "</color>";
			num *= 2f;
			num2 *= 2f;
		}
		else
		{
			this.style.fontSize = DEBUG_HeapPrinter.SmallFontSize;
		}
		long value3 = totalMemory / 1024L;
		long value4 = Profiler.GetMonoHeapSizeLong() / 1024L;
		this.builder.Append(value3);
		this.builder.Append(" / ");
		this.builder.Append(value4);
		this.builder.Append("\n");
		this.builder.Append((this.counter.Average() / 1024f).ToString("F2"));
		this.builder.Append("kb / frame");
		this.builder.Append(value2);
		GUI.Box(new Rect((float)Screen.width - num, (float)Screen.height - num2, num, num2), this.builder.ToString(), this.style);
		this.builder.Length = 0;
		this.previousMemory = totalMemory;
	}

	// Token: 0x04004BCD RID: 19405
	private static readonly Vector2 Size = new Vector2(250f, 70f);

	// Token: 0x04004BCE RID: 19406
	private static readonly float HighlightTime = 3f;

	// Token: 0x04004BCF RID: 19407
	private static readonly int SmallFontSize = 24;

	// Token: 0x04004BD0 RID: 19408
	private static readonly int LargeFontSize = 50;

	// Token: 0x04004BD1 RID: 19409
	private static readonly int CounterSize = 30;

	// Token: 0x04004BD2 RID: 19410
	private bool styleInitialized;

	// Token: 0x04004BD3 RID: 19411
	private GUIStyle style;

	// Token: 0x04004BD4 RID: 19412
	private long previousMemory = long.MaxValue;

	// Token: 0x04004BD5 RID: 19413
	private float highlightTimer = float.MaxValue;

	// Token: 0x04004BD6 RID: 19414
	private StringBuilder builder = new StringBuilder(100);

	// Token: 0x04004BD7 RID: 19415
	private DEBUG_HeapPrinter.CircularCounter counter = new DEBUG_HeapPrinter.CircularCounter(DEBUG_HeapPrinter.CounterSize);

	// Token: 0x02000B3C RID: 2876
	private class CircularCounter
	{
		// Token: 0x060045A9 RID: 17833 RVA: 0x0024C0D5 File Offset: 0x0024A4D5
		public CircularCounter(int size)
		{
			this.values = new long[size];
		}

		// Token: 0x060045AA RID: 17834 RVA: 0x0024C0E9 File Offset: 0x0024A4E9
		public void Add(long value)
		{
			this.values[this.currentIndex] = value;
			this.currentIndex++;
			if (this.currentIndex >= this.values.Length)
			{
				this.currentIndex = 0;
			}
		}

		// Token: 0x060045AB RID: 17835 RVA: 0x0024C124 File Offset: 0x0024A524
		public float Average()
		{
			long num = 0L;
			for (int i = 0; i < this.values.Length; i++)
			{
				num += this.values[i];
			}
			return (float)num / (float)this.values.Length;
		}

		// Token: 0x04004BD8 RID: 19416
		private long[] values;

		// Token: 0x04004BD9 RID: 19417
		private int currentIndex;
	}
}
