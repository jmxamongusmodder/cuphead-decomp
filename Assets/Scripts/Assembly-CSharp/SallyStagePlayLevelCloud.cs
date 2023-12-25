using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007AB RID: 1963
public class SallyStagePlayLevelCloud : AbstractPausableComponent
{
	// Token: 0x06002C1F RID: 11295 RVA: 0x0019F015 File Offset: 0x0019D415
	private void Start()
	{
		base.FrameDelayedCallback(new Action(this.GetSprites), 1);
	}

	// Token: 0x06002C20 RID: 11296 RVA: 0x0019F02C File Offset: 0x0019D42C
	private void GetSprites()
	{
		this.normalClones = this.normalSprite.gameObject.transform.GetComponentsInChildren<SpriteRenderer>();
		this.shadowClones = this.shadowSprite.gameObject.transform.GetComponentsInChildren<SpriteRenderer>();
		base.StartCoroutine(this.move_all_cr());
		base.StartCoroutine(this.move_shadow_local_pos_cr());
	}

	// Token: 0x06002C21 RID: 11297 RVA: 0x0019F08C File Offset: 0x0019D48C
	private IEnumerator move_all_cr()
	{
		float size = 500f;
		float speed = 30f;
		for (;;)
		{
			for (int i = 0; i < this.normalClones.Length; i++)
			{
				if (this.normalClones[i].transform.position.x > -640f - size)
				{
					this.normalClones[i].transform.position += Vector3.left * speed * CupheadTime.Delta;
				}
				else
				{
					this.normalClones[i].transform.position = new Vector3(640f + size, this.normalClones[i].transform.position.y, 0f);
				}
			}
			for (int j = 0; j < this.shadowClones.Length; j++)
			{
				if (this.shadowClones[j].transform.position.x > -640f - size)
				{
					this.shadowClones[j].transform.position += Vector3.left * speed * CupheadTime.Delta;
				}
				else
				{
					Vector3 position = this.shadowClones[j].transform.position;
					position.x = this.normalClones[j].transform.position.x;
					this.shadowClones[j].transform.position = position;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002C22 RID: 11298 RVA: 0x0019F0A8 File Offset: 0x0019D4A8
	private IEnumerator move_shadow_local_pos_cr()
	{
		float speed = 1.3f;
		float shadowOffset = 5f;
		for (;;)
		{
			for (int i = 0; i < this.shadowClones.Length; i++)
			{
				if (this.normalClones[i].transform.position.x > 0f && this.normalClones[i].transform.position.x < 440f)
				{
					if (this.shadowClones[i].transform.position.x < this.normalClones[i].transform.position.x + shadowOffset)
					{
						this.shadowClones[i].transform.position += Vector3.right * speed * CupheadTime.Delta;
					}
				}
				else if (this.normalClones[i].transform.position.x < 0f && this.normalClones[i].transform.position.x > -640f && this.shadowClones[i].transform.position.x > this.normalClones[i].transform.position.x - shadowOffset)
				{
					this.shadowClones[i].transform.position -= Vector3.right * speed * CupheadTime.Delta;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x040034D2 RID: 13522
	[SerializeField]
	private SpriteRenderer shadowSprite;

	// Token: 0x040034D3 RID: 13523
	private SpriteRenderer[] shadowClones;

	// Token: 0x040034D4 RID: 13524
	[SerializeField]
	private SpriteRenderer normalSprite;

	// Token: 0x040034D5 RID: 13525
	private SpriteRenderer[] normalClones;
}
