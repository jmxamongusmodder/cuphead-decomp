using System;
using UnityEngine;

// Token: 0x02000B1F RID: 2847
public class SpriteErrorManager : AbstractMonoBehaviour
{
	// Token: 0x060044EE RID: 17646 RVA: 0x00247518 File Offset: 0x00245918
	protected override void Awake()
	{
		base.Awake();
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		SpriteErrorManager.Pair.InitializePairs(this.errors);
	}

	// Token: 0x060044EF RID: 17647 RVA: 0x00247538 File Offset: 0x00245938
	private void OnWillRenderObject()
	{
		foreach (SpriteErrorManager.Pair pair in this.errors)
		{
			string name = this.spriteRenderer.sprite.name;
			if (name == pair.name)
			{
				if (this.lastFrame == name || pair.chance > UnityEngine.Random.Range(1, 101))
				{
					this.spriteRenderer.sprite = pair.sprite;
					this.lastFrame = name;
				}
				return;
			}
			this.lastFrame = string.Empty;
		}
	}

	// Token: 0x04004AC3 RID: 19139
	public const string ERROR_STRING = "_error";

	// Token: 0x04004AC4 RID: 19140
	[SerializeField]
	private SpriteErrorManager.Pair[] errors;

	// Token: 0x04004AC5 RID: 19141
	private string lastFrame;

	// Token: 0x04004AC6 RID: 19142
	private SpriteRenderer spriteRenderer;

	// Token: 0x02000B20 RID: 2848
	[Serializable]
	public class Pair
	{
		// Token: 0x060044F1 RID: 17649 RVA: 0x002475E0 File Offset: 0x002459E0
		public static void InitializePairs(SpriteErrorManager.Pair[] p)
		{
			for (int i = 0; i < p.Length; i++)
			{
				p[i].name = p[i].sprite.name.Replace("_error", string.Empty);
			}
		}

		// Token: 0x04004AC7 RID: 19143
		public Sprite sprite;

		// Token: 0x04004AC8 RID: 19144
		[Range(1f, 100f)]
		public int chance = 10;

		// Token: 0x04004AC9 RID: 19145
		[HideInInspector]
		public string name;
	}
}
