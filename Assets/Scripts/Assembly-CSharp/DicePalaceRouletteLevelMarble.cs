using System;
using UnityEngine;

// Token: 0x020005E2 RID: 1506
public class DicePalaceRouletteLevelMarble : BasicProjectile
{
	// Token: 0x06001DDA RID: 7642 RVA: 0x00112C8A File Offset: 0x0011108A
	protected override void Start()
	{
		base.Start();
		base.animator.Play("Fall", 0, UnityEngine.Random.value);
	}

	// Token: 0x06001DDB RID: 7643 RVA: 0x00112CA8 File Offset: 0x001110A8
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		if (phase == CollisionPhase.Enter)
		{
			base.animator.SetFloat("Variation", (float)UnityEngine.Random.Range(0, 3) / 2f);
			base.animator.SetTrigger("Ground");
			this.move = false;
			this.Speed = 0f;
		}
	}

	// Token: 0x06001DDC RID: 7644 RVA: 0x00112CFB File Offset: 0x001110FB
	public void OnAnimEnd()
	{
		AudioManager.Play("dice_palace_roulette_balls_splat");
		this.emitAudioFromObject.Add("dice_palace_roulette_balls_splat");
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x040026A9 RID: 9897
	private const string FallState = "Fall";

	// Token: 0x040026AA RID: 9898
	private const string GroundParameterName = "Ground";

	// Token: 0x040026AB RID: 9899
	private const string VariationParameterName = "Variation";

	// Token: 0x040026AC RID: 9900
	private const int VariationCount = 3;
}
