using System;
using UnityEngine;

// Token: 0x02000767 RID: 1895
public class RetroArcadeUFOTurret : AbstractCollidableObject
{
	// Token: 0x06002946 RID: 10566 RVA: 0x00181134 File Offset: 0x0017F534
	public RetroArcadeUFOTurret Create(RetroArcadeUFO parent, LevelProperties.RetroArcade.UFO properties, float t)
	{
		RetroArcadeUFOTurret retroArcadeUFOTurret = this.InstantiatePrefab<RetroArcadeUFOTurret>();
		retroArcadeUFOTurret.properties = properties;
		retroArcadeUFOTurret.parent = parent;
		retroArcadeUFOTurret.t = t;
		retroArcadeUFOTurret.transform.parent = parent.transform;
		retroArcadeUFOTurret.transform.position = parent.transform.position;
		return retroArcadeUFOTurret;
	}

	// Token: 0x06002947 RID: 10567 RVA: 0x00181188 File Offset: 0x0017F588
	private void FixedUpdate()
	{
		this.t += CupheadTime.FixedDelta * (this.properties.projectileSpeed / 600f);
		float f = this.t % 1f * 3.1415927f;
		Vector2 vector = new Vector2(Mathf.Cos(f) * 600f / 2f, -Mathf.Sin(f) * 300f / 2f);
		base.transform.SetPosition(new float?(this.parent.transform.position.x + vector.x), new float?(this.parent.transform.position.y + vector.y), null);
		float value = MathUtils.DirectionToAngle(new Vector2(Mathf.Cos(f) * 300f / 2f, -Mathf.Sin(f) * 600f / 2f)) + -90f;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(value));
	}

	// Token: 0x06002948 RID: 10568 RVA: 0x001812B4 File Offset: 0x0017F6B4
	public void Shoot()
	{
		this.projectilePrefab.Create(this.projectileRoot.position, base.transform.eulerAngles.z - -90f, this.properties.projectileSpeed);
	}

	// Token: 0x04003240 RID: 12864
	private const float ANGLE_OFFSET = -90f;

	// Token: 0x04003241 RID: 12865
	[SerializeField]
	private BasicProjectile projectilePrefab;

	// Token: 0x04003242 RID: 12866
	[SerializeField]
	private Transform projectileRoot;

	// Token: 0x04003243 RID: 12867
	private LevelProperties.RetroArcade.UFO properties;

	// Token: 0x04003244 RID: 12868
	private RetroArcadeUFO parent;

	// Token: 0x04003245 RID: 12869
	private float t;
}
