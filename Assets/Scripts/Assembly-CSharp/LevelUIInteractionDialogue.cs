using System;
using UnityEngine;

// Token: 0x02000487 RID: 1159
public class LevelUIInteractionDialogue : AbstractUIInteractionDialogue
{
	// Token: 0x0600121D RID: 4637 RVA: 0x000A89C4 File Offset: 0x000A6DC4
	public static LevelUIInteractionDialogue Create(AbstractUIInteractionDialogue.Properties properties, PlayerInput player, Vector2 offset, float glyphOffsetAddition = 0f, LevelUIInteractionDialogue.TailPosition tailPosition = LevelUIInteractionDialogue.TailPosition.Bottom, bool playerTarget = true)
	{
		LevelUIInteractionDialogue levelUIInteractionDialogue = UnityEngine.Object.Instantiate<LevelUIInteractionDialogue>(Level.Current.LevelResources.levelUIInteractionDialogue);
		levelUIInteractionDialogue.glyphOffsetAddition = glyphOffsetAddition;
		levelUIInteractionDialogue.tailPosition = tailPosition;
		levelUIInteractionDialogue.Init(properties, player, offset);
		if (tailPosition == LevelUIInteractionDialogue.TailPosition.Right)
		{
			levelUIInteractionDialogue.dialogueOffset = new Vector2(offset.x - levelUIInteractionDialogue.back.sizeDelta.x * 0.5f - 14f, offset.y);
		}
		else if (tailPosition == LevelUIInteractionDialogue.TailPosition.Left)
		{
			levelUIInteractionDialogue.dialogueOffset = new Vector2(offset.x + levelUIInteractionDialogue.back.sizeDelta.x * 0.5f + 14f, offset.y);
		}
		if (!playerTarget && LevelUIInteractionDialogue.defaultTarget == null)
		{
			LevelUIInteractionDialogue.defaultTarget = GameObject.CreatePrimitive(PrimitiveType.Cube);
			LevelUIInteractionDialogue.defaultTarget.transform.position = Vector3.zero;
			LevelUIInteractionDialogue.defaultTarget.transform.localScale = Vector3.zero;
			levelUIInteractionDialogue.target = LevelUIInteractionDialogue.defaultTarget.transform;
		}
		else if (!playerTarget)
		{
			levelUIInteractionDialogue.target = LevelUIInteractionDialogue.defaultTarget.transform;
		}
		return levelUIInteractionDialogue;
	}

	// Token: 0x170002CD RID: 717
	// (get) Token: 0x0600121E RID: 4638 RVA: 0x000A8AFC File Offset: 0x000A6EFC
	protected override float PreferredWidth
	{
		get
		{
			if (this.tmpText.text.Length == 0)
			{
				return this.tmpText.preferredWidth + this.glyph.preferredWidth + 5.3f + this.glyphOffsetAddition;
			}
			return this.tmpText.preferredWidth + this.glyph.preferredWidth + 27f + this.glyphOffsetAddition;
		}
	}

	// Token: 0x0600121F RID: 4639 RVA: 0x000A8B67 File Offset: 0x000A6F67
	protected override void Awake()
	{
		base.Awake();
		base.transform.SetParent(LevelHUD.Current.Canvas.transform, false);
	}

	// Token: 0x06001220 RID: 4640 RVA: 0x000A8B8A File Offset: 0x000A6F8A
	protected override void Init(AbstractUIInteractionDialogue.Properties properties, PlayerInput player, Vector2 offset)
	{
		base.Init(properties, player, offset);
		this.UpdatePos();
	}

	// Token: 0x06001221 RID: 4641 RVA: 0x000A8B9B File Offset: 0x000A6F9B
	private void Update()
	{
		this.UpdatePos();
		this.UpdateTailPosition();
	}

	// Token: 0x06001222 RID: 4642 RVA: 0x000A8BA9 File Offset: 0x000A6FA9
	protected virtual void UpdatePos()
	{
		if (this.target != null)
		{
			base.transform.position = this.target.position + this.dialogueOffset;
		}
	}

	// Token: 0x06001223 RID: 4643 RVA: 0x000A8BE8 File Offset: 0x000A6FE8
	private void UpdateTailPosition()
	{
		LevelUIInteractionDialogue.TailPosition tailPosition = this.tailPosition;
		if (tailPosition != LevelUIInteractionDialogue.TailPosition.Bottom)
		{
			if (tailPosition != LevelUIInteractionDialogue.TailPosition.Right)
			{
				if (tailPosition == LevelUIInteractionDialogue.TailPosition.Left)
				{
					this.leftTail.SetActive(true);
				}
			}
			else
			{
				this.rightTail.SetActive(true);
			}
		}
		else
		{
			this.bottomTail.SetActive(true);
		}
	}

	// Token: 0x04001B91 RID: 7057
	private const float TAIL_WIDTH = 14f;

	// Token: 0x04001B92 RID: 7058
	private const float OFFSET_GLYPH = 27f;

	// Token: 0x04001B93 RID: 7059
	private const float OFFSET_GLYPH_ONLY = 5.3f;

	// Token: 0x04001B94 RID: 7060
	private float glyphOffsetAddition;

	// Token: 0x04001B95 RID: 7061
	private LevelUIInteractionDialogue.TailPosition tailPosition;

	// Token: 0x04001B96 RID: 7062
	[SerializeField]
	private GameObject bottomTail;

	// Token: 0x04001B97 RID: 7063
	[SerializeField]
	private GameObject leftTail;

	// Token: 0x04001B98 RID: 7064
	[SerializeField]
	private GameObject rightTail;

	// Token: 0x04001B99 RID: 7065
	private static GameObject defaultTarget;

	// Token: 0x02000488 RID: 1160
	public enum TailPosition
	{
		// Token: 0x04001B9B RID: 7067
		Right,
		// Token: 0x04001B9C RID: 7068
		Left,
		// Token: 0x04001B9D RID: 7069
		Bottom
	}
}
