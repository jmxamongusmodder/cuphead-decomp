using System;
using UnityEngine;

// Token: 0x0200035E RID: 862
public class FramerateCounter : MonoBehaviour
{
	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x06000990 RID: 2448 RVA: 0x0007BEA9 File Offset: 0x0007A2A9
	// (set) Token: 0x06000991 RID: 2449 RVA: 0x0007BEB0 File Offset: 0x0007A2B0
	public static FramerateCounter Current { get; private set; }

	// Token: 0x06000992 RID: 2450 RVA: 0x0007BEB8 File Offset: 0x0007A2B8
	public static void Init()
	{
		if (FramerateCounter.Current == null)
		{
			GameObject gameObject = new GameObject("Framerate Counter");
			FramerateCounter.Current = gameObject.AddComponent<FramerateCounter>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}
	}

	// Token: 0x06000993 RID: 2451 RVA: 0x0007BEF1 File Offset: 0x0007A2F1
	protected virtual void Start()
	{
		this.timeleft = this.updateInterval;
	}

	// Token: 0x06000994 RID: 2452 RVA: 0x0007BF00 File Offset: 0x0007A300
	private void Update()
	{
		this.timeleft -= Time.deltaTime;
		this.accum += Time.timeScale / Time.deltaTime;
		this.frames++;
		if ((double)this.timeleft <= 0.0)
		{
			float num = this.accum / (float)this.frames;
			string text = string.Format("{0:F2} FPS\n{1} HP", num, this.hpCounter);
			this.text = text;
			if (num < 10f)
			{
				this.color = "red";
			}
			else if (num < 30f)
			{
				this.color = "orange";
			}
			else
			{
				this.color = "lime";
			}
			this.timeleft = this.updateInterval;
			this.accum = 0f;
			this.frames = 0;
		}
	}

	// Token: 0x06000995 RID: 2453 RVA: 0x0007BFEC File Offset: 0x0007A3EC
	protected virtual void OnGUI()
	{
		if (!FramerateCounter.SHOW)
		{
			return;
		}
		if (this.style == null)
		{
			this.style = new GUIStyle(GUI.skin.label);
			this.style.alignment = TextAnchor.UpperRight;
			this.style.richText = true;
			this.style.padding = new RectOffset(20, 20, 20, 20);
		}
		GUI.Label(new Rect(0f, 0f, (float)(Screen.width + 1), (float)(Screen.height + 1)), "<color=black>" + this.text + "</color>", this.style);
		GUI.Label(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), string.Concat(new string[]
		{
			"<color=",
			this.color,
			">",
			this.text,
			"</color>"
		}), this.style);
	}

	// Token: 0x04001431 RID: 5169
	public static bool SHOW;

	// Token: 0x04001432 RID: 5170
	public float updateInterval = 0.25f;

	// Token: 0x04001433 RID: 5171
	public int hpCounter;

	// Token: 0x04001434 RID: 5172
	private float accum;

	// Token: 0x04001435 RID: 5173
	private int frames;

	// Token: 0x04001436 RID: 5174
	private float timeleft;

	// Token: 0x04001437 RID: 5175
	private GUIStyle style;

	// Token: 0x04001438 RID: 5176
	private string text;

	// Token: 0x04001439 RID: 5177
	private string color = "white";
}
