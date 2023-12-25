using System;
using DialoguerCore;
using DialoguerEditor;
using UnityEngine;

// Token: 0x02000B6C RID: 2924
public class WaitPhaseComponent : MonoBehaviour
{
	// Token: 0x06004691 RID: 18065 RVA: 0x0024E5ED File Offset: 0x0024C9ED
	public void Init(WaitPhase phase, DialogueEditorWaitTypes type, float duration)
	{
		this.phase = phase;
		this.type = type;
		this.duration = duration;
		this.elapsed = 0f;
		this.go = true;
	}

	// Token: 0x06004692 RID: 18066 RVA: 0x0024E618 File Offset: 0x0024CA18
	private void Update()
	{
		if (!this.go)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		DialogueEditorWaitTypes dialogueEditorWaitTypes = this.type;
		if (dialogueEditorWaitTypes != DialogueEditorWaitTypes.Seconds)
		{
			if (dialogueEditorWaitTypes == DialogueEditorWaitTypes.Frames)
			{
				this.elapsed += 1f;
				if (this.elapsed >= this.duration)
				{
					this.waitComplete();
				}
			}
		}
		else
		{
			this.elapsed += deltaTime;
			if (this.elapsed >= this.duration)
			{
				this.waitComplete();
			}
		}
	}

	// Token: 0x06004693 RID: 18067 RVA: 0x0024E6A8 File Offset: 0x0024CAA8
	private void waitComplete()
	{
		this.go = false;
		this.phase.waitComplete();
		this.phase = null;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04004C6B RID: 19563
	public DialogueEditorWaitTypes type;

	// Token: 0x04004C6C RID: 19564
	public WaitPhase phase;

	// Token: 0x04004C6D RID: 19565
	public bool go;

	// Token: 0x04004C6E RID: 19566
	public float duration;

	// Token: 0x04004C6F RID: 19567
	public float elapsed;
}
