using System;
using UnityEngine;

// Token: 0x020006D5 RID: 1749
public class MausoleumLevelCircleGhost : MausoleumLevelGhostBase
{
	// Token: 0x170003BB RID: 955
	// (get) Token: 0x0600253C RID: 9532 RVA: 0x0015D309 File Offset: 0x0015B709
	protected override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600253D RID: 9533 RVA: 0x0015D30C File Offset: 0x0015B70C
	public virtual AbstractCollidableObject Create(Vector2 position, Vector2 urnPosition, float rotation, float speed, float rotationSpeed)
	{
		MausoleumLevelCircleGhost mausoleumLevelCircleGhost = base.Create(position, rotation, speed) as MausoleumLevelCircleGhost;
		mausoleumLevelCircleGhost.rotationSpeed = rotationSpeed;
		mausoleumLevelCircleGhost.rotationBase = new GameObject("CircleGhostBase").transform;
		mausoleumLevelCircleGhost.rotationBase.position = urnPosition;
		mausoleumLevelCircleGhost.rotation = rotation;
		mausoleumLevelCircleGhost.transform.parent = mausoleumLevelCircleGhost.rotationBase;
		return mausoleumLevelCircleGhost;
	}

	// Token: 0x0600253E RID: 9534 RVA: 0x0015D370 File Offset: 0x0015B770
	protected override void Start()
	{
		base.Start();
		bool flag = Rand.Bool();
		this.setDirection = (float)((!flag) ? -360 : 360);
		base.GetComponent<SpriteRenderer>().flipY = flag;
	}

	// Token: 0x0600253F RID: 9535 RVA: 0x0015D3B4 File Offset: 0x0015B7B4
	protected override void Move()
	{
		base.transform.localPosition += MathUtils.AngleToDirection(this.rotation) * this.Speed * CupheadTime.FixedDelta;
		this.rotationBase.AddEulerAngles(0f, 0f, this.rotationSpeed * this.setDirection * CupheadTime.FixedDelta);
	}

	// Token: 0x06002540 RID: 9536 RVA: 0x0015D424 File Offset: 0x0015B824
	public override void OnParry(AbstractPlayerController player)
	{
		base.OnParry(player);
		AudioManager.Play("mausoleum_circle_ghost_death");
		this.emitAudioFromObject.Add("mausoleum_circle_ghost_death");
		UnityEngine.Object.Destroy(this.rotationBase.gameObject);
	}

	// Token: 0x04002DDE RID: 11742
	private float rotationSpeed;

	// Token: 0x04002DDF RID: 11743
	private float rotation;

	// Token: 0x04002DE0 RID: 11744
	private float setDirection;

	// Token: 0x04002DE1 RID: 11745
	private Transform rotationBase;
}
