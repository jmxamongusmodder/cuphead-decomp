using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200061E RID: 1566
public class FlyingBirdLevelGarbage : BasicProjectile
{
	// Token: 0x06001FDD RID: 8157 RVA: 0x00124710 File Offset: 0x00122B10
	protected override void Start()
	{
		base.Start();
		if (!this.isBoot)
		{
			base.StartCoroutine(this.not_boot_cr());
		}
		else
		{
			base.animator.SetBool("OnClockwise", Rand.Bool());
		}
		base.StartCoroutine(this.change_layer_cr());
	}

	// Token: 0x06001FDE RID: 8158 RVA: 0x00124764 File Offset: 0x00122B64
	private IEnumerator not_boot_cr()
	{
		float frameTime = 0f;
		this.bootSpeed = ((!Rand.Bool()) ? 600f : 300f);
		this.bootSpeed = ((!Rand.Bool()) ? this.bootSpeed : (-this.bootSpeed));
		for (;;)
		{
			frameTime += CupheadTime.Delta;
			if (frameTime > 0.041666668f)
			{
				frameTime -= 0.041666668f;
				base.transform.Rotate(0f, 0f, this.bootSpeed * CupheadTime.Delta);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001FDF RID: 8159 RVA: 0x00124780 File Offset: 0x00122B80
	private IEnumerator change_layer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		base.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Projectiles.ToString();
		yield break;
	}

	// Token: 0x06001FE0 RID: 8160 RVA: 0x0012479B File Offset: 0x00122B9B
	protected override void Die()
	{
		base.Die();
		base.GetComponent<SpriteRenderer>().enabled = false;
	}

	// Token: 0x04002857 RID: 10327
	private const float ROTATE_FRAME_TIME = 0.041666668f;

	// Token: 0x04002858 RID: 10328
	[SerializeField]
	private bool isBoot;

	// Token: 0x04002859 RID: 10329
	private float bootSpeed;
}
