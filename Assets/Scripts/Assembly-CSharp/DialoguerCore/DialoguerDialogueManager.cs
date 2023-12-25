using System;
using System.Runtime.CompilerServices;

namespace DialoguerCore
{
	// Token: 0x02000B61 RID: 2913
	public class DialoguerDialogueManager
	{
		// Token: 0x06004644 RID: 17988 RVA: 0x0024DF28 File Offset: 0x0024C328
		public static void startDialogueWithCallback(int dialogueId, DialoguerCallback callback)
		{
			DialoguerDialogueManager.onEndCallback = callback;
			DialoguerDialogueManager.startDialogue(dialogueId);
		}

		// Token: 0x06004645 RID: 17989 RVA: 0x0024DF36 File Offset: 0x0024C336
		public static void startDialogue(int dialogueId)
		{
			if (DialoguerDialogueManager.dialogue != null)
			{
				DialoguerEventManager.dispatchOnSuddenlyEnded();
			}
			DialoguerEventManager.dispatchOnStarted();
			DialoguerDialogueManager.dialogue = DialoguerDataManager.GetDialogueById(dialogueId);
			DialoguerDialogueManager.dialogue.Reset();
			DialoguerDialogueManager.setupPhase(DialoguerDialogueManager.dialogue.startPhaseId);
		}

		// Token: 0x06004646 RID: 17990 RVA: 0x0024DF70 File Offset: 0x0024C370
		public static void continueDialogue(int outId)
		{
			if (DialoguerDialogueManager.currentPhase != null)
			{
				DialoguerDialogueManager.currentPhase.Continue(outId);
			}
		}

		// Token: 0x06004647 RID: 17991 RVA: 0x0024DF87 File Offset: 0x0024C387
		public static void endDialogue()
		{
			if (DialoguerDialogueManager.dialogue == null)
			{
				return;
			}
			if (DialoguerDialogueManager.onEndCallback != null)
			{
				DialoguerDialogueManager.onEndCallback();
			}
			DialoguerEventManager.dispatchOnWindowClose();
			DialoguerEventManager.dispatchOnEnded();
			DialoguerDialogueManager.dialogue.Reset();
			DialoguerDialogueManager.reset();
		}

		// Token: 0x06004648 RID: 17992 RVA: 0x0024DFC4 File Offset: 0x0024C3C4
		private static void setupPhase(int nextPhaseId)
		{
			if (DialoguerDialogueManager.dialogue == null)
			{
				return;
			}
			AbstractDialoguePhase abstractDialoguePhase = DialoguerDialogueManager.dialogue.phases[nextPhaseId];
			if (abstractDialoguePhase is EndPhase)
			{
				DialoguerDialogueManager.endDialogue();
				return;
			}
			if (DialoguerDialogueManager.currentPhase != null)
			{
				DialoguerDialogueManager.currentPhase.resetEvents();
			}
			AbstractDialoguePhase abstractDialoguePhase2 = abstractDialoguePhase;
			if (DialoguerDialogueManager.<>f__mg$cache0 == null)
			{
				DialoguerDialogueManager.<>f__mg$cache0 = new AbstractDialoguePhase.PhaseCompleteHandler(DialoguerDialogueManager.phaseComplete);
			}
			abstractDialoguePhase2.onPhaseComplete += DialoguerDialogueManager.<>f__mg$cache0;
			if (abstractDialoguePhase is TextPhase || abstractDialoguePhase is BranchedTextPhase)
			{
				DialoguerEventManager.dispatchOnTextPhase((abstractDialoguePhase as TextPhase).data);
			}
			DialoguerDialogueManager.currentPhase = abstractDialoguePhase;
			abstractDialoguePhase.Start(DialoguerDialogueManager.dialogue.localVariables);
		}

		// Token: 0x06004649 RID: 17993 RVA: 0x0024E071 File Offset: 0x0024C471
		private static void phaseComplete(int nextPhaseId)
		{
			DialoguerDialogueManager.setupPhase(nextPhaseId);
		}

		// Token: 0x0600464A RID: 17994 RVA: 0x0024E079 File Offset: 0x0024C479
		private static bool isWindowed(AbstractDialoguePhase phase)
		{
			return phase is TextPhase || phase is BranchedTextPhase;
		}

		// Token: 0x0600464B RID: 17995 RVA: 0x0024E094 File Offset: 0x0024C494
		private static void reset()
		{
			DialoguerDialogueManager.currentPhase = null;
			DialoguerDialogueManager.dialogue = null;
			DialoguerDialogueManager.onEndCallback = null;
		}

		// Token: 0x04004C5E RID: 19550
		private static AbstractDialoguePhase currentPhase;

		// Token: 0x04004C5F RID: 19551
		private static DialoguerDialogue dialogue;

		// Token: 0x04004C60 RID: 19552
		private static DialoguerCallback onEndCallback;

		// Token: 0x04004C61 RID: 19553
		[CompilerGenerated]
		private static AbstractDialoguePhase.PhaseCompleteHandler <>f__mg$cache0;
	}
}
