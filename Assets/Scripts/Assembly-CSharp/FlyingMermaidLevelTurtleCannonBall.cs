using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006A2 RID: 1698
public class FlyingMermaidLevelTurtleCannonBall : AbstractProjectile
{
	// Token: 0x06002400 RID: 9216 RVA: 0x001522E0 File Offset: 0x001506E0
	public FlyingMermaidLevelTurtleCannonBall Create(Vector2 pos, string explodePattern, LevelProperties.FlyingMermaid.Turtle properties)
	{
		FlyingMermaidLevelTurtleCannonBall flyingMermaidLevelTurtleCannonBall = base.Create() as FlyingMermaidLevelTurtleCannonBall;
		flyingMermaidLevelTurtleCannonBall.properties = properties;
		flyingMermaidLevelTurtleCannonBall.explodePattern = explodePattern;
		flyingMermaidLevelTurtleCannonBall.transform.position = pos;
		return flyingMermaidLevelTurtleCannonBall;
	}

	// Token: 0x06002401 RID: 9217 RVA: 0x00152319 File Offset: 0x00150719
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x06002402 RID: 9218 RVA: 0x0015232E File Offset: 0x0015072E
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002403 RID: 9219 RVA: 0x0015234C File Offset: 0x0015074C
	private IEnumerator loop_cr()
	{
		AudioManager.Play("level_mermaid_turtle_cannon");
		float t = 0f;
		float bulletTime = this.properties.bulletTimeToExplode.RandomFloat();
		float targetDistance = this.properties.bulletSpeed * bulletTime;
		float apex = targetDistance + 50f;
		float launchSpeed = Mathf.Sqrt(4000f * apex);
		float timeToApex = launchSpeed / 2000f;
		float launchY = base.transform.position.y;
		while (t < timeToApex || base.transform.position.y > targetDistance + launchY)
		{
			t += CupheadTime.FixedDelta;
			float y = launchY + launchSpeed * t - 1000f * t * t;
			base.transform.SetPosition(null, new float?(y), null);
			yield return new WaitForFixedUpdate();
		}
		foreach (string s in this.explodePattern.Split(new char[]
		{
			'-'
		}))
		{
			float rotation = 0f;
			Parser.FloatTryParse(s, out rotation);
			this.spreadshotPrefab.Create(base.transform.position, rotation, this.properties.spreadshotBulletSpeed, this.properties.spiralRate);
		}
		this.explodeEffectPrefab.Create(base.transform.position);
		this.Die();
		yield break;
	}

	// Token: 0x06002404 RID: 9220 RVA: 0x00152367 File Offset: 0x00150767
	protected override void Die()
	{
		base.Die();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04002CD0 RID: 11472
	[SerializeField]
	private FlyingMermaidLevelTurtleSpiralProjectile spreadshotPrefab;

	// Token: 0x04002CD1 RID: 11473
	[SerializeField]
	private Effect explodeEffectPrefab;

	// Token: 0x04002CD2 RID: 11474
	private string explodePattern;

	// Token: 0x04002CD3 RID: 11475
	private LevelProperties.FlyingMermaid.Turtle properties;
}
