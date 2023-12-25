using System;
using System.Diagnostics;

// Token: 0x02000B58 RID: 2904
public class DialoguerEvents
{
	// Token: 0x0600460A RID: 17930 RVA: 0x0024DAAB File Offset: 0x0024BEAB
	public void ClearAll()
	{
		this.onStarted = null;
		this.onEnded = null;
		this.onTextPhase = null;
		this.onWindowClose = null;
		this.onWaitStart = null;
		this.onWaitComplete = null;
		this.onMessageEvent = null;
	}

	// Token: 0x140000D3 RID: 211
	// (add) Token: 0x0600460B RID: 17931 RVA: 0x0024DAE0 File Offset: 0x0024BEE0
	// (remove) Token: 0x0600460C RID: 17932 RVA: 0x0024DB18 File Offset: 0x0024BF18
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event DialoguerEvents.StartedHandler onStarted;

	// Token: 0x0600460D RID: 17933 RVA: 0x0024DB4E File Offset: 0x0024BF4E
	public void handler_onStarted()
	{
		if (this.onStarted != null)
		{
			this.onStarted();
		}
	}

	// Token: 0x140000D4 RID: 212
	// (add) Token: 0x0600460E RID: 17934 RVA: 0x0024DB68 File Offset: 0x0024BF68
	// (remove) Token: 0x0600460F RID: 17935 RVA: 0x0024DBA0 File Offset: 0x0024BFA0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event DialoguerEvents.EndedHandler onEnded;

	// Token: 0x06004610 RID: 17936 RVA: 0x0024DBD6 File Offset: 0x0024BFD6
	public void handler_onEnded()
	{
		if (this.onEnded != null)
		{
			this.onEnded();
		}
	}

	// Token: 0x140000D5 RID: 213
	// (add) Token: 0x06004611 RID: 17937 RVA: 0x0024DBF0 File Offset: 0x0024BFF0
	// (remove) Token: 0x06004612 RID: 17938 RVA: 0x0024DC28 File Offset: 0x0024C028
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event DialoguerEvents.SuddenlyEndedHandler onInstantlyEnded;

	// Token: 0x06004613 RID: 17939 RVA: 0x0024DC5E File Offset: 0x0024C05E
	public void handler_SuddenlyEnded()
	{
		if (this.onInstantlyEnded != null)
		{
			this.onInstantlyEnded();
		}
	}

	// Token: 0x140000D6 RID: 214
	// (add) Token: 0x06004614 RID: 17940 RVA: 0x0024DC78 File Offset: 0x0024C078
	// (remove) Token: 0x06004615 RID: 17941 RVA: 0x0024DCB0 File Offset: 0x0024C0B0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event DialoguerEvents.TextPhaseHandler onTextPhase;

	// Token: 0x06004616 RID: 17942 RVA: 0x0024DCE6 File Offset: 0x0024C0E6
	public void handler_TextPhase(DialoguerTextData data)
	{
		if (this.onTextPhase != null)
		{
			this.onTextPhase(data);
		}
	}

	// Token: 0x140000D7 RID: 215
	// (add) Token: 0x06004617 RID: 17943 RVA: 0x0024DD00 File Offset: 0x0024C100
	// (remove) Token: 0x06004618 RID: 17944 RVA: 0x0024DD38 File Offset: 0x0024C138
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event DialoguerEvents.WindowCloseHandler onWindowClose;

	// Token: 0x06004619 RID: 17945 RVA: 0x0024DD6E File Offset: 0x0024C16E
	public void handler_WindowClose()
	{
		if (this.onWindowClose != null)
		{
			this.onWindowClose();
		}
	}

	// Token: 0x140000D8 RID: 216
	// (add) Token: 0x0600461A RID: 17946 RVA: 0x0024DD88 File Offset: 0x0024C188
	// (remove) Token: 0x0600461B RID: 17947 RVA: 0x0024DDC0 File Offset: 0x0024C1C0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event DialoguerEvents.WaitStartHandler onWaitStart;

	// Token: 0x0600461C RID: 17948 RVA: 0x0024DDF6 File Offset: 0x0024C1F6
	public void handler_WaitStart()
	{
		if (this.onWaitStart != null)
		{
			this.onWaitStart();
		}
	}

	// Token: 0x140000D9 RID: 217
	// (add) Token: 0x0600461D RID: 17949 RVA: 0x0024DE10 File Offset: 0x0024C210
	// (remove) Token: 0x0600461E RID: 17950 RVA: 0x0024DE48 File Offset: 0x0024C248
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event DialoguerEvents.WaitCompleteHandler onWaitComplete;

	// Token: 0x0600461F RID: 17951 RVA: 0x0024DE7E File Offset: 0x0024C27E
	public void handler_WaitComplete()
	{
		if (this.onWaitComplete != null)
		{
			this.onWaitComplete();
		}
	}

	// Token: 0x140000DA RID: 218
	// (add) Token: 0x06004620 RID: 17952 RVA: 0x0024DE98 File Offset: 0x0024C298
	// (remove) Token: 0x06004621 RID: 17953 RVA: 0x0024DED0 File Offset: 0x0024C2D0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event DialoguerEvents.MessageEventHandler onMessageEvent;

	// Token: 0x06004622 RID: 17954 RVA: 0x0024DF06 File Offset: 0x0024C306
	public void handler_MessageEvent(string message, string metadata)
	{
		if (this.onMessageEvent != null)
		{
			this.onMessageEvent(message, metadata);
		}
	}

	// Token: 0x02000B59 RID: 2905
	// (Invoke) Token: 0x06004624 RID: 17956
	public delegate void StartedHandler();

	// Token: 0x02000B5A RID: 2906
	// (Invoke) Token: 0x06004628 RID: 17960
	public delegate void EndedHandler();

	// Token: 0x02000B5B RID: 2907
	// (Invoke) Token: 0x0600462C RID: 17964
	public delegate void SuddenlyEndedHandler();

	// Token: 0x02000B5C RID: 2908
	// (Invoke) Token: 0x06004630 RID: 17968
	public delegate void TextPhaseHandler(DialoguerTextData data);

	// Token: 0x02000B5D RID: 2909
	// (Invoke) Token: 0x06004634 RID: 17972
	public delegate void WindowCloseHandler();

	// Token: 0x02000B5E RID: 2910
	// (Invoke) Token: 0x06004638 RID: 17976
	public delegate void WaitStartHandler();

	// Token: 0x02000B5F RID: 2911
	// (Invoke) Token: 0x0600463C RID: 17980
	public delegate void WaitCompleteHandler();

	// Token: 0x02000B60 RID: 2912
	// (Invoke) Token: 0x06004640 RID: 17984
	public delegate void MessageEventHandler(string message, string metadata);
}
