using System;
using System.Diagnostics;

// Token: 0x02000ADC RID: 2780
public class DummyPlmInterface : PlmInterface
{
	// Token: 0x140000B4 RID: 180
	// (add) Token: 0x06004310 RID: 17168 RVA: 0x0023FB1C File Offset: 0x0023DF1C
	// (remove) Token: 0x06004311 RID: 17169 RVA: 0x0023FB54 File Offset: 0x0023DF54
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event OnSuspendHandler OnSuspend;

	// Token: 0x140000B5 RID: 181
	// (add) Token: 0x06004312 RID: 17170 RVA: 0x0023FB8C File Offset: 0x0023DF8C
	// (remove) Token: 0x06004313 RID: 17171 RVA: 0x0023FBC4 File Offset: 0x0023DFC4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event OnResumeHandler OnResume;

	// Token: 0x140000B6 RID: 182
	// (add) Token: 0x06004314 RID: 17172 RVA: 0x0023FBFC File Offset: 0x0023DFFC
	// (remove) Token: 0x06004315 RID: 17173 RVA: 0x0023FC34 File Offset: 0x0023E034
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event OnConstrainedHandler OnConstrained;

	// Token: 0x140000B7 RID: 183
	// (add) Token: 0x06004316 RID: 17174 RVA: 0x0023FC6C File Offset: 0x0023E06C
	// (remove) Token: 0x06004317 RID: 17175 RVA: 0x0023FCA4 File Offset: 0x0023E0A4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event OnUnconstrainedHandler OnUnconstrained;

	// Token: 0x06004318 RID: 17176 RVA: 0x0023FCDA File Offset: 0x0023E0DA
	public void Init()
	{
	}

	// Token: 0x06004319 RID: 17177 RVA: 0x0023FCDC File Offset: 0x0023E0DC
	public bool IsConstrained()
	{
		return false;
	}
}
