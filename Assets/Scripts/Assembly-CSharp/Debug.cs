using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Internal;

// Token: 0x0200041F RID: 1055
public static class Debug
{
	// Token: 0x17000266 RID: 614
	// (get) Token: 0x06000F30 RID: 3888 RVA: 0x00097129 File Offset: 0x00095529
	// (set) Token: 0x06000F31 RID: 3889 RVA: 0x00097130 File Offset: 0x00095530
	public static bool developerConsoleVisible
	{
		get
		{
			return UnityEngine.Debug.developerConsoleVisible;
		}
		set
		{
			UnityEngine.Debug.developerConsoleVisible = value;
		}
	}

	// Token: 0x17000267 RID: 615
	// (get) Token: 0x06000F32 RID: 3890 RVA: 0x00097138 File Offset: 0x00095538
	public static bool isDebugBuild
	{
		get
		{
			return UnityEngine.Debug.isDebugBuild;
		}
	}

	// Token: 0x06000F33 RID: 3891 RVA: 0x0009713F File Offset: 0x0009553F
	[Conditional("UNITY_ASSERTIONS")]
	public static void Assert(bool condition)
	{
	}

	// Token: 0x06000F34 RID: 3892 RVA: 0x00097141 File Offset: 0x00095541
	[Conditional("UNITY_ASSERTIONS")]
	public static void Assert(bool condition, string message)
	{
	}

	// Token: 0x06000F35 RID: 3893 RVA: 0x00097143 File Offset: 0x00095543
	[Conditional("UNITY_ASSERTIONS")]
	public static void AssertFormat(bool condition, string format, params object[] args)
	{
	}

	// Token: 0x06000F36 RID: 3894 RVA: 0x00097145 File Offset: 0x00095545
	public static void Break()
	{
		UnityEngine.Debug.Break();
	}

	// Token: 0x06000F37 RID: 3895 RVA: 0x0009714C File Offset: 0x0009554C
	public static void ClearDeveloperConsole()
	{
		UnityEngine.Debug.ClearDeveloperConsole();
	}

	// Token: 0x06000F38 RID: 3896 RVA: 0x00097153 File Offset: 0x00095553
	public static void DebugBreak()
	{
		UnityEngine.Debug.DebugBreak();
	}

	// Token: 0x06000F39 RID: 3897 RVA: 0x0009715A File Offset: 0x0009555A
	public static void DrawLine(Vector3 start, Vector3 end)
	{
		UnityEngine.Debug.DrawLine(start, end);
	}

	// Token: 0x06000F3A RID: 3898 RVA: 0x00097163 File Offset: 0x00095563
	public static void DrawLine(Vector3 start, Vector3 end, Color color)
	{
		UnityEngine.Debug.DrawLine(start, end, color);
	}

	// Token: 0x06000F3B RID: 3899 RVA: 0x0009716D File Offset: 0x0009556D
	public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
	{
		UnityEngine.Debug.DrawLine(start, end, color, duration);
	}

	// Token: 0x06000F3C RID: 3900 RVA: 0x00097178 File Offset: 0x00095578
	public static void DrawLine(Vector3 start, Vector3 end, [DefaultValue("Color.white")] Color color, [DefaultValue("0.0f")] float duration, [DefaultValue("true")] bool depthTest)
	{
		UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
	}

	// Token: 0x06000F3D RID: 3901 RVA: 0x00097185 File Offset: 0x00095585
	public static void DrawRay(Vector3 start, Vector3 dir)
	{
		UnityEngine.Debug.DrawRay(start, dir);
	}

	// Token: 0x06000F3E RID: 3902 RVA: 0x0009718E File Offset: 0x0009558E
	public static void DrawRay(Vector3 start, Vector3 dir, Color color)
	{
		UnityEngine.Debug.DrawRay(start, dir, color);
	}

	// Token: 0x06000F3F RID: 3903 RVA: 0x00097198 File Offset: 0x00095598
	public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration)
	{
		UnityEngine.Debug.DrawRay(start, dir, color, duration);
	}

	// Token: 0x06000F40 RID: 3904 RVA: 0x000971A3 File Offset: 0x000955A3
	public static void DrawRay(Vector3 start, Vector3 dir, [DefaultValue("Color.white")] Color color, [DefaultValue("0.0f")] float duration, [DefaultValue("true")] bool depthTest)
	{
		UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest);
	}

	// Token: 0x06000F41 RID: 3905 RVA: 0x000971B0 File Offset: 0x000955B0
	public static void LogInfo(object message, UnityEngine.Object context = null)
	{
		UnityEngine.Debug.Log(message, context);
	}

	// Token: 0x06000F42 RID: 3906 RVA: 0x000971B9 File Offset: 0x000955B9
	public static void LogInfoCat(params object[] args)
	{
		UnityEngine.Debug.Log(string.Concat(args));
	}

	// Token: 0x06000F43 RID: 3907 RVA: 0x000971C6 File Offset: 0x000955C6
	[Conditional("VERBOSE")]
	public static void Log(object message, UnityEngine.Object context = null)
	{
		UnityEngine.Debug.Log(message, context);
	}

	// Token: 0x06000F44 RID: 3908 RVA: 0x000971CF File Offset: 0x000955CF
	[Conditional("VERBOSE")]
	public static void LogCat(params object[] args)
	{
		UnityEngine.Debug.Log(string.Concat(args));
	}

	// Token: 0x06000F45 RID: 3909 RVA: 0x000971DC File Offset: 0x000955DC
	public static void LogError(object message, UnityEngine.Object context = null)
	{
		UnityEngine.Debug.LogError(message, context);
	}

	// Token: 0x06000F46 RID: 3910 RVA: 0x000971E5 File Offset: 0x000955E5
	public static void LogErrorCat(params object[] args)
	{
		UnityEngine.Debug.LogError(string.Concat(args));
	}

	// Token: 0x06000F47 RID: 3911 RVA: 0x000971F2 File Offset: 0x000955F2
	public static void LogErrorFormat(string format, params object[] args)
	{
		UnityEngine.Debug.LogErrorFormat(format, args);
	}

	// Token: 0x06000F48 RID: 3912 RVA: 0x000971FB File Offset: 0x000955FB
	public static void LogErrorFormat(UnityEngine.Object context, string format, params object[] args)
	{
		UnityEngine.Debug.LogErrorFormat(context, format, args);
	}

	// Token: 0x06000F49 RID: 3913 RVA: 0x00097205 File Offset: 0x00095605
	public static void LogException(Exception exception)
	{
		UnityEngine.Debug.LogException(exception);
	}

	// Token: 0x06000F4A RID: 3914 RVA: 0x0009720D File Offset: 0x0009560D
	public static void LogException(Exception exception, UnityEngine.Object context)
	{
		UnityEngine.Debug.LogException(exception, context);
	}

	// Token: 0x06000F4B RID: 3915 RVA: 0x00097216 File Offset: 0x00095616
	[Conditional("VERBOSE")]
	public static void LogFormat(string format, params object[] args)
	{
		UnityEngine.Debug.LogFormat(format, args);
	}

	// Token: 0x06000F4C RID: 3916 RVA: 0x0009721F File Offset: 0x0009561F
	[Conditional("VERBOSE")]
	public static void LogFormat(UnityEngine.Object context, string format, params object[] args)
	{
		UnityEngine.Debug.LogFormat(context, format, args);
	}

	// Token: 0x06000F4D RID: 3917 RVA: 0x00097229 File Offset: 0x00095629
	[Conditional("VERBOSE")]
	public static void LogWarning(object message, UnityEngine.Object context = null)
	{
		UnityEngine.Debug.LogWarning(message, context);
	}

	// Token: 0x06000F4E RID: 3918 RVA: 0x00097232 File Offset: 0x00095632
	[Conditional("VERBOSE")]
	public static void LogWarningCat(params object[] args)
	{
		UnityEngine.Debug.LogWarning(string.Concat(args));
	}

	// Token: 0x06000F4F RID: 3919 RVA: 0x0009723F File Offset: 0x0009563F
	[Conditional("VERBOSE")]
	public static void LogWarningFormat(string format, params object[] args)
	{
		UnityEngine.Debug.LogWarningFormat(format, args);
	}

	// Token: 0x06000F50 RID: 3920 RVA: 0x00097248 File Offset: 0x00095648
	[Conditional("VERBOSE")]
	public static void LogWarningFormat(UnityEngine.Object context, string format, params object[] args)
	{
		UnityEngine.Debug.LogWarningFormat(context, format, args);
	}
}
