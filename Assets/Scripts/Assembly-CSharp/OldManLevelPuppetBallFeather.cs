using System;
using UnityEngine;

// Token: 0x0200070C RID: 1804
public class OldManLevelPuppetBallFeather : Effect
{
	// Token: 0x060026F4 RID: 9972 RVA: 0x0016D0B0 File Offset: 0x0016B4B0
	public override Effect Create(Vector3 position, Vector3 scale)
	{
		OldManLevelPuppetBallFeather oldManLevelPuppetBallFeather = base.Create(position, scale) as OldManLevelPuppetBallFeather;
		oldManLevelPuppetBallFeather.vel = MathUtils.AngleToDirection((float)UnityEngine.Random.Range(0, 360)) * (UnityEngine.Random.Range(this.startSpeedMin, this.startSpeedMax) + ((UnityEngine.Random.Range(0f, 6f) >= 1f) ? 0f : this.startSpeedMax));
		oldManLevelPuppetBallFeather.rotateDir = (float)MathUtils.PlusOrMinus();
		oldManLevelPuppetBallFeather.fallFactor = UnityEngine.Random.Range(oldManLevelPuppetBallFeather.fallFactorMin, oldManLevelPuppetBallFeather.fallFactorMax);
		oldManLevelPuppetBallFeather.fallVel.y = UnityEngine.Random.Range(0f, 0.5f);
		oldManLevelPuppetBallFeather.anim.speed = UnityEngine.Random.Range(0.5f, 0.75f);
		return oldManLevelPuppetBallFeather;
	}

	// Token: 0x060026F5 RID: 9973 RVA: 0x0016D180 File Offset: 0x0016B580
	private void PhysicsUpdate()
	{
		if (CupheadTime.FixedDelta == 0f)
		{
			return;
		}
		base.transform.position += this.vel + this.fallVel;
		this.vel *= this.slowFactor;
		float magnitude = this.vel.magnitude;
		base.transform.Rotate(new Vector3(0f, 0f, this.rotateDir * Mathf.InverseLerp(0f, this.startSpeedMax, magnitude)) * 100f);
		if (magnitude < 1f)
		{
			this.fallVel.y = this.fallVel.y - this.fallFactor;
			base.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(base.transform.eulerAngles.z, 0f, 0.5f));
		}
		if (base.transform.position.y < -560f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060026F6 RID: 9974 RVA: 0x0016D2AA File Offset: 0x0016B6AA
	private void FixedUpdate()
	{
		this.skipFrame = !this.skipFrame;
		if (this.skipFrame)
		{
			return;
		}
		this.PhysicsUpdate();
		this.PhysicsUpdate();
	}

	// Token: 0x04002FA0 RID: 12192
	[SerializeField]
	private Animator anim;

	// Token: 0x04002FA1 RID: 12193
	[SerializeField]
	private Vector3 vel;

	// Token: 0x04002FA2 RID: 12194
	private Vector3 fallVel;

	// Token: 0x04002FA3 RID: 12195
	[SerializeField]
	private float startSpeedMin = 10f;

	// Token: 0x04002FA4 RID: 12196
	[SerializeField]
	private float startSpeedMax = 20f;

	// Token: 0x04002FA5 RID: 12197
	[SerializeField]
	private float slowFactor = 0.95f;

	// Token: 0x04002FA6 RID: 12198
	[SerializeField]
	private float fallFactorMin = 0.1f;

	// Token: 0x04002FA7 RID: 12199
	[SerializeField]
	private float fallFactorMax = 0.2f;

	// Token: 0x04002FA8 RID: 12200
	private float fallFactor;

	// Token: 0x04002FA9 RID: 12201
	private float rotateDir;

	// Token: 0x04002FAA RID: 12202
	private bool skipFrame;
}
