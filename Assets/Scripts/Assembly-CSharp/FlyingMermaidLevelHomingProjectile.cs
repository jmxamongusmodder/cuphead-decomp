using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200068B RID: 1675
public class FlyingMermaidLevelHomingProjectile : HomingProjectile
{
	// Token: 0x06002350 RID: 9040 RVA: 0x0014BB64 File Offset: 0x00149F64
	public FlyingMermaidLevelHomingProjectile Create(Vector3 pos, float rotation, AbstractPlayerController player, LevelProperties.FlyingMermaid.HomerFish properties)
	{
		FlyingMermaidLevelHomingProjectile flyingMermaidLevelHomingProjectile = base.Create(pos, rotation, properties.initSpeed, properties.bulletSpeed, properties.rotationSpeed, properties.timeBeforeDeath, properties.timeBeforeHoming, player) as FlyingMermaidLevelHomingProjectile;
		flyingMermaidLevelHomingProjectile.properties = properties;
		flyingMermaidLevelHomingProjectile.transform.position = pos;
		return flyingMermaidLevelHomingProjectile;
	}

	// Token: 0x06002351 RID: 9041 RVA: 0x0014BBBD File Offset: 0x00149FBD
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.timer_cr());
	}

	// Token: 0x06002352 RID: 9042 RVA: 0x0014BBD4 File Offset: 0x00149FD4
	private IEnumerator timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		this.mainSprite.sortingLayerName = "Foreground";
		this.mainSprite.sortingOrder = 30;
		yield return CupheadTime.WaitForSeconds(this, this.properties.timeBeforeDeath - 0.2f);
		base.HomingEnabled = false;
		base.animator.SetTrigger("StopTracking");
		for (;;)
		{
			base.transform.position += this.velocity.normalized * this.properties.bulletSpeed * 1.4f * CupheadTime.Delta;
			base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(MathUtils.DirectionToAngle(this.velocity) + 180f));
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002BF2 RID: 11250
	[SerializeField]
	private SpriteRenderer mainSprite;

	// Token: 0x04002BF3 RID: 11251
	private LevelProperties.FlyingMermaid.HomerFish properties;
}
