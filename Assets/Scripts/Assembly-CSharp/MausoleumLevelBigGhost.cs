using System;
using UnityEngine;

// Token: 0x020006D4 RID: 1748
public class MausoleumLevelBigGhost : MausoleumLevelGhostBase
{
	// Token: 0x06002539 RID: 9529 RVA: 0x0015D0CC File Offset: 0x0015B4CC
	public MausoleumLevelBigGhost Create(Vector2 position, float rotation, float speed, LevelProperties.Mausoleum.BigGhost properties, GameObject urn)
	{
		MausoleumLevelBigGhost mausoleumLevelBigGhost = base.Create(position, rotation, speed) as MausoleumLevelBigGhost;
		mausoleumLevelBigGhost.properties = properties;
		mausoleumLevelBigGhost.urn = urn;
		return mausoleumLevelBigGhost;
	}

	// Token: 0x0600253A RID: 9530 RVA: 0x0015D0FC File Offset: 0x0015B4FC
	public override void OnParry(AbstractPlayerController player)
	{
		Vector2 vector = this.smallRoot1.transform.position;
		Vector2 a = vector;
		Vector2 vector2 = new Vector2(UnityEngine.Random.value * (float)((!Rand.Bool()) ? -1 : 1), UnityEngine.Random.value * (float)((!Rand.Bool()) ? -1 : 1));
		vector = a + vector2.normalized * this.smallRoot1.radius * UnityEngine.Random.value;
		Vector2 vector3 = this.smallRoot2.transform.position;
		Vector2 a2 = vector3;
		Vector2 vector4 = new Vector2(UnityEngine.Random.value * (float)((!Rand.Bool()) ? -1 : 1), UnityEngine.Random.value * (float)((!Rand.Bool()) ? -1 : 1));
		vector3 = a2 + vector4.normalized * this.smallRoot2.radius * UnityEngine.Random.value;
		Vector3 v = this.urn.transform.position - vector;
		Vector3 v2 = this.urn.transform.position - vector3;
		MausoleumLevelRegularGhost mausoleumLevelRegularGhost = this.regGhost.Create(vector, MathUtils.DirectionToAngle(v), this.properties.littleGhostSpeed) as MausoleumLevelRegularGhost;
		mausoleumLevelRegularGhost.GetParent(this.parent);
		MausoleumLevelRegularGhost mausoleumLevelRegularGhost2 = this.regGhost.Create(vector3, MathUtils.DirectionToAngle(v2), this.properties.littleGhostSpeed) as MausoleumLevelRegularGhost;
		mausoleumLevelRegularGhost2.GetParent(this.parent);
		mausoleumLevelRegularGhost.transform.SetScale(new float?(0.7f), new float?(0.7f), new float?(0.7f));
		mausoleumLevelRegularGhost2.transform.SetScale(new float?(0.7f), new float?(0.7f), new float?(0.7f));
		base.OnParry(player);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04002DD9 RID: 11737
	[SerializeField]
	private MausoleumLevelRegularGhost regGhost;

	// Token: 0x04002DDA RID: 11738
	[SerializeField]
	private FlyingBlimpLevelSpawnRadius smallRoot1;

	// Token: 0x04002DDB RID: 11739
	[SerializeField]
	private FlyingBlimpLevelSpawnRadius smallRoot2;

	// Token: 0x04002DDC RID: 11740
	private LevelProperties.Mausoleum.BigGhost properties;

	// Token: 0x04002DDD RID: 11741
	private GameObject urn;
}
