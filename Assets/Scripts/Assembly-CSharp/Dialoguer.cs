using System;
using DialoguerCore;

// Token: 0x02000B57 RID: 2903
public class Dialoguer
{
	// Token: 0x1700063D RID: 1597
	// (get) Token: 0x060045F5 RID: 17909 RVA: 0x0024D933 File Offset: 0x0024BD33
	// (set) Token: 0x060045F6 RID: 17910 RVA: 0x0024D93A File Offset: 0x0024BD3A
	public static bool ready { get; private set; }

	// Token: 0x060045F7 RID: 17911 RVA: 0x0024D944 File Offset: 0x0024BD44
	public static void Initialize()
	{
		if (Dialoguer.ready)
		{
			return;
		}
		Dialoguer.events = new DialoguerEvents();
		DialoguerDataManager.Initialize();
		DialoguerEventManager.onStarted += Dialoguer.events.handler_onStarted;
		DialoguerEventManager.onEnded += Dialoguer.events.handler_onEnded;
		DialoguerEventManager.onSuddenlyEnded += Dialoguer.events.handler_SuddenlyEnded;
		DialoguerEventManager.onTextPhase += Dialoguer.events.handler_TextPhase;
		DialoguerEventManager.onWindowClose += Dialoguer.events.handler_WindowClose;
		DialoguerEventManager.onWaitStart += Dialoguer.events.handler_WaitStart;
		DialoguerEventManager.onWaitComplete += Dialoguer.events.handler_WaitComplete;
		DialoguerEventManager.onMessageEvent += Dialoguer.events.handler_MessageEvent;
		Dialoguer.ready = true;
	}

	// Token: 0x060045F8 RID: 17912 RVA: 0x0024DA19 File Offset: 0x0024BE19
	public static void StartDialogue(DialoguerDialogues dialogue)
	{
		DialoguerDialogueManager.startDialogue((int)dialogue);
	}

	// Token: 0x060045F9 RID: 17913 RVA: 0x0024DA21 File Offset: 0x0024BE21
	public static void StartDialogue(DialoguerDialogues dialogue, DialoguerCallback callback)
	{
		DialoguerDialogueManager.startDialogueWithCallback((int)dialogue, callback);
	}

	// Token: 0x060045FA RID: 17914 RVA: 0x0024DA2A File Offset: 0x0024BE2A
	public static void StartDialogue(int dialogueId)
	{
		DialoguerDialogueManager.startDialogue(dialogueId);
	}

	// Token: 0x060045FB RID: 17915 RVA: 0x0024DA32 File Offset: 0x0024BE32
	public static void StartDialogue(int dialogueId, DialoguerCallback callback)
	{
		DialoguerDialogueManager.startDialogueWithCallback(dialogueId, callback);
	}

	// Token: 0x060045FC RID: 17916 RVA: 0x0024DA3B File Offset: 0x0024BE3B
	public static void ContinueDialogue(int choice)
	{
		DialoguerDialogueManager.continueDialogue(choice);
	}

	// Token: 0x060045FD RID: 17917 RVA: 0x0024DA43 File Offset: 0x0024BE43
	public static void ContinueDialogue()
	{
		DialoguerDialogueManager.continueDialogue(0);
	}

	// Token: 0x060045FE RID: 17918 RVA: 0x0024DA4B File Offset: 0x0024BE4B
	public static void EndDialogue()
	{
		DialoguerDialogueManager.endDialogue();
	}

	// Token: 0x060045FF RID: 17919 RVA: 0x0024DA52 File Offset: 0x0024BE52
	public static void SetGlobalBoolean(int booleanId, bool booleanValue)
	{
		DialoguerDataManager.SetGlobalBoolean(booleanId, booleanValue);
	}

	// Token: 0x06004600 RID: 17920 RVA: 0x0024DA5B File Offset: 0x0024BE5B
	public static bool GetGlobalBoolean(int booleanId)
	{
		return DialoguerDataManager.GetGlobalBoolean(booleanId);
	}

	// Token: 0x06004601 RID: 17921 RVA: 0x0024DA63 File Offset: 0x0024BE63
	public static void SetGlobalFloat(int floatId, float floatValue)
	{
		DialoguerDataManager.SetGlobalFloat(floatId, floatValue);
	}

	// Token: 0x06004602 RID: 17922 RVA: 0x0024DA6C File Offset: 0x0024BE6C
	public static float GetGlobalFloat(int floatId)
	{
		return DialoguerDataManager.GetGlobalFloat(floatId);
	}

	// Token: 0x06004603 RID: 17923 RVA: 0x0024DA74 File Offset: 0x0024BE74
	public static void SetGlobalString(int stringId, string stringValue)
	{
		DialoguerDataManager.SetGlobalString(stringId, stringValue);
	}

	// Token: 0x06004604 RID: 17924 RVA: 0x0024DA7D File Offset: 0x0024BE7D
	public static string GetGlobalString(int stringId)
	{
		return DialoguerDataManager.GetGlobalString(stringId);
	}

	// Token: 0x06004605 RID: 17925 RVA: 0x0024DA85 File Offset: 0x0024BE85
	public static string GetGlobalVariablesState()
	{
		return DialoguerDataManager.GetGlobalVariablesState();
	}

	// Token: 0x06004606 RID: 17926 RVA: 0x0024DA8C File Offset: 0x0024BE8C
	public static void SetGlobalVariablesState(string globalVariablesXml)
	{
		DialoguerDataManager.LoadGlobalVariablesState(globalVariablesXml);
	}

	// Token: 0x1700063E RID: 1598
	// (get) Token: 0x06004607 RID: 17927 RVA: 0x0024DA94 File Offset: 0x0024BE94
	// (set) Token: 0x06004608 RID: 17928 RVA: 0x0024DA9B File Offset: 0x0024BE9B
	public static DialoguerEvents events { get; private set; }
}
